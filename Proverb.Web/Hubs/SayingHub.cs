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

      public class GetQuery { public int Id { get; set; } }
      public Task<Saying> Get(GetQuery query)
      {
         return _sayingFeed.Get(query.Id);
      }

      public class SaveCommand { public Saying Saying { get; set; } }
      public async Task<SaveResult> Save(SaveCommand command)
      {
         return await _sayingFeed.Save(command.Saying);
      }

      public class RemoveCommand { public int Id { get; set; } }
      public Task Remove(RemoveCommand command)
      {
         return _sayingFeed.Remove(command.Id);
      }
   }
}