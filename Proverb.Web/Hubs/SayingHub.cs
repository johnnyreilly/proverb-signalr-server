using Autofac;
using Microsoft.AspNet.SignalR;
using Proverb.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proverb.Data.Common;

namespace Proverb.Web.Hubs
{
   public class SayingHub : Hub
   {
      //public string Send(string name, string message)
      //{
      //    // Call the broadcastMessage method to update clients.
      //    Clients.All.broadcastMessage(name, message);

      //    return name;
      //}
      private readonly ILifetimeScope _hubLifetimeScope;
      private readonly ISayingFeed _sayingFeed;

      public SayingHub(ILifetimeScope lifetimeScope)
      {
         // Create a lifetime scope for the hub.
         _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

         // Resolve dependencies from the hub lifetime scope.
         _sayingFeed = _hubLifetimeScope.Resolve<ISayingFeed>();
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

      public Task<IEnumerable<Saying>> GetAll()
      {
         return _sayingFeed.GetAll();
      }

      public Task<Saying> Get(int id)
      {
         return _sayingFeed.Get(id);
      }

      public async Task<SaveResult> Save(Saying saying)
      {
         return await _sayingFeed.Save(saying);
      }

      public Task Remove(int id)
      {
         return _sayingFeed.Remove(id);
      }
   }
}