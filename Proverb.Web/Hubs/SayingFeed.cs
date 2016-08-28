using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Proverb.Data.Models;
using Proverb.Services.Interfaces;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Infrastructure;
using Proverb.Data.Common;

namespace Proverb.Web.Hubs
{
   public class SayingFeed : ISayingFeed
   {
      private readonly ISayingService _sayingService;

      private readonly ConcurrentDictionary<int, Saying> _sayings = new ConcurrentDictionary<int, Saying>();
      private volatile bool _sayingsInitialised = false;

      public SayingFeed(IConnectionManager connectionManager, ISayingService sayingService)
      {
         var context = connectionManager.GetHubContext<SayingHub>();
         Clients = context.Clients;
         _sayingService = sayingService;
      }

      private IHubConnectionContext<dynamic> Clients { get; }

      public async Task<IEnumerable<Saying>> GetAll()
      {
         await EnsureFeedIsPrimed();

         Clients.All.getAllCalled("By you", System.DateTime.Now);

         return _sayings.Values;
      }

      public async Task<Saying> Get(int id)
      {
         await EnsureFeedIsPrimed();
         Saying saying;
         return _sayings.TryGetValue(id, out saying) ? saying : null;
      }

      public async Task<SaveResult> Save(Saying saying)
      {
         // Perform service validations
         var serviceValidations = _sayingService.Validate(saying);
         if (serviceValidations.HasErrors)
             return SaveResult.Fail(serviceValidations);

         if (saying.Id > 0)
         {
            var updateResult = await _sayingService.UpdateAsync(saying);
            return await HandleSaveResult(saying.Id, updateResult);
         }

         var createResult = await _sayingService.CreateAsync(saying);
         return await HandleSaveResult(createResult.Value, createResult);
      }

      private async Task<SaveResult> HandleSaveResult(int sayingId, Result result)
      {
         if (result.IsSuccess)
         {
            var newSaying = await _sayingService.GetByIdAsync(sayingId);
            _sayings.AddOrUpdate(sayingId, newSaying, (key, oldSaying) => newSaying);

            return SaveResult.Success(sayingId);
         }

         return SaveResult.Fail(result.Error);
      }


      public async Task Remove(int id)
      {
         await _sayingService.DeleteAsync(id); // todo: fix cache invalidation
      }

      private async Task EnsureFeedIsPrimed()
      {
         if (_sayingsInitialised)
            return;

         var sayings = await _sayingService.GetAllAsync();
         foreach (var saying in sayings)
         {
            _sayings.AddOrUpdate(saying.Id, saying, (id, oldSaying) => saying);
         }

         _sayingsInitialised = sayings.Any();
      }

      private void BroadcastStockPrice(Saying saying)
      {
         Clients.All.updateStockPrice(saying);
      }

   }
}