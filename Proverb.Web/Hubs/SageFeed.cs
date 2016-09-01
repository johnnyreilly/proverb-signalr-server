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
   public class SageFeed : ISageFeed
   {
      private readonly ISageService _sageService;

      private readonly ConcurrentDictionary<int, Sage> _sages = new ConcurrentDictionary<int, Sage>();
      private volatile bool _sagesInitialised = false;

      public SageFeed(IConnectionManager connectionManager, ISageService sageService)
      {
         var context = connectionManager.GetHubContext<SageHub>();
         Clients = context.Clients;
         _sageService = sageService;
      }

      private IHubConnectionContext<dynamic> Clients { get; }

      public async Task<IEnumerable<Sage>> GetAll()
      {
         await EnsureFeedIsPrimed();

         Clients.All.getAllCalled("By you", System.DateTime.Now);

         return _sages.Values;
      }

      public async Task<Sage> Get(int id)
      {
         await EnsureFeedIsPrimed();
         Sage sage;
         return _sages.TryGetValue(id, out sage) ? sage : null;
      }

      public async Task<SaveResult> Save(Sage sage)
      {
         // Perform service validations
         var serviceValidations = _sageService.Validate(sage);
         if (serviceValidations.HasErrors)
            return SaveResult.Fail(serviceValidations);

         if (sage.Id > 0)
         {
            var updateResult = await _sageService.UpdateAsync(sage);
            return await HandleSaveResult(sage.Id, updateResult);
         }

         var createResult = await _sageService.CreateAsync(sage);
         return await HandleSaveResult(createResult.Value, createResult);
      }

      private async Task<SaveResult> HandleSaveResult(int sageId, Result result)
      {
         if (result.IsSuccess)
         {
            var newSaying = await _sageService.GetByIdAsync(sageId);
            _sages.AddOrUpdate(sageId, newSaying, (key, oldSaying) => newSaying);

            return SaveResult.Success(sageId);
         }

         return SaveResult.Fail(result.Error);
      }


      public async Task Remove(int id)
      {
         await _sageService.DeleteAsync(id); // todo: fix cache invalidation
      }

      private async Task EnsureFeedIsPrimed()
      {
         if (_sagesInitialised)
            return;

         var sages = await _sageService.GetAllAsync();
         foreach (var sage in sages)
         {
            _sages.AddOrUpdate(sage.Id, sage, (id, oldSage) => sage);
         }

         _sagesInitialised = sages.Any();
      }

      private void BroadcastStockPrice(Sage sage)
      {
         Clients.All.updateStockPrice(sage);
      }

   }
}