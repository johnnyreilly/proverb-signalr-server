using Autofac;
using Microsoft.AspNet.SignalR;
using Proverb.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proverb.Data.Common;

namespace Proverb.Web.Hubs
{
   public class SageHub : Hub
   {
      //public string Send(string name, string message)
      //{
      //    // Call the broadcastMessage method to update clients.
      //    Clients.All.broadcastMessage(name, message);

      //    return name;
      //}
      private readonly ILifetimeScope _hubLifetimeScope;
      private readonly ISageFeed _sageFeed;

      public SageHub(ILifetimeScope lifetimeScope)
      {
         // Create a lifetime scope for the hub.
         _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

         // Resolve dependencies from the hub lifetime scope.
         _sageFeed = _hubLifetimeScope.Resolve<ISageFeed>();
      }

      protected override void Dispose(bool disposing)
      {
         // Dipose the hub lifetime scope when the hub is disposed.
         if (disposing && _hubLifetimeScope != null)
         {
            _hubLifetimeScope.Dispose();
         }

         base.Dispose(disposing);
      }

      public Task<IEnumerable<Sage>> GetAll()
      {
         return _sageFeed.GetAll();
      }

      public class GetQuery { public int Id { get; set; } }
      public Task<Sage> Get(GetQuery query)
      {
         return _sageFeed.Get(query.Id);
      }

      public class SaveCommand { public Sage Sage { get; set; } }
      public async Task<SaveResult> Save(SaveCommand command)
      {
         return await _sageFeed.Save(command.Sage);
      }

      public class RemoveCommand { public int Id { get; set; } }
      public Task Remove(RemoveCommand command)
      {
         return _sageFeed.Remove(command.Id);
      }
   }
}