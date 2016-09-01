using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Proverb.Data.CommandQuery.Interfaces;
using Proverb.Data.EntityFramework;
using Proverb.Data.Models;

namespace Proverb.Data.CommandQuery
{
   public class UserQuery : IUserQuery
   {
      public async Task<ICollection<User>> GetAllAsync()
      {
         using (var context = new ProverbContext())
         {
            var users = await context.Users.ToListAsync();

            return users;
         }
      }

      public async Task<User> GetByIdAsync(int id)
      {
         using (var context = new ProverbContext())
         {
            var user = await context.Users.SingleAsync(x => x.Id == id);

            return user;
         }
      }

      public async Task<User> GetByUserNameAsync(string userName)
      {
         using (var context = new ProverbContext())
         {
            var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == userName);

            return user;
         }
      }
   }
}
