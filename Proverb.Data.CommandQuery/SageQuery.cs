using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Proverb.Data.CommandQuery.Interfaces;
using Proverb.Data.EntityFramework;
using Proverb.Data.Models;

namespace Proverb.Data.CommandQuery
{
   public class SageQuery : ISageQuery
   {
      public async Task<ICollection<Sage>> GetAllAsync()
      {
         using (var context = new ProverbContext())
         {
            var sages = await context.Sages.ToListAsync();

            return sages;
         }
      }

      public async Task<ICollection<Sage>> GetAllWithSayingsAsync()
      {
         using (var context = new ProverbContext())
         {
            var sagesWithSayings = await context.Sages.Include(x => x.Sayings).ToListAsync();

            return sagesWithSayings;
         }
      }

      public async Task<Sage> GetByIdAsync(int id)
      {
         using (var context = new ProverbContext())
         {
            var sage = await context.Sages.SingleAsync(x => x.Id == id);

            return sage;
         }
      }

      public async Task<Sage> GetByIdWithSayingsAsync(int id)
      {
         using (var context = new ProverbContext())
         {
            var sageWithSayings = await context.Sages
               .Include(x => x.Sayings)
               .SingleOrDefaultAsync(x => x.Id == id);

            return sageWithSayings;
         }
      }
   }
}
