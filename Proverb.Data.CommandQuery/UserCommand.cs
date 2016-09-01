using System.Data.Entity;
using System.Threading.Tasks;
using Proverb.Data.CommandQuery.Interfaces;
using Proverb.Data.EntityFramework;
using Proverb.Data.Models;

namespace Proverb.Data.CommandQuery
{
   public class UserCommand : IUserCommand
   {
      public async Task<int> CreateAsync(User user)
      {
         using (var context = new ProverbContext())
         {
            context.Users.Add(user);

            await context.SaveChangesAsync();

            return user.Id;
         }
      }

      public async Task DeleteAsync(int id)
      {
         using (var context = new ProverbContext())
         {
            var userToDelete = await context.Users.FindAsync(id);

            context.Users.Remove(userToDelete);

            await context.SaveChangesAsync();
         }
      }

      public async Task UpdateAsync(User user)
      {
         using (var context = new ProverbContext())
         {
            context.Entry(user).State = EntityState.Modified;

            await context.SaveChangesAsync();
         }
      }
   }
}
