using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Proverb.Data.CommandQuery.Interfaces;
using Proverb.Data.EntityFramework;
using Proverb.Data.Models;

namespace Proverb.Data.CommandQuery
{
    public class SayingQuery : ISayingQuery
    {
        public async Task<ICollection<Saying>> GetAllAsync()
        {
           using (var context = new ProverbContext())
           {
              var sayings = await context.Sayings.ToListAsync();

              return sayings;
           }
        }

        public async Task<Saying> GetByIdAsync(int id)
        {
           using (var context = new ProverbContext())
           {
              var saying = await context.Sayings.SingleAsync(x => x.Id == id);

              return saying;
           }
        }

        public async Task<ICollection<Saying>> GetBySageIdAsync(int sageId)
        {
           using (var context = new ProverbContext())
           {
              var sayings = await context.Sayings.Where(x => x.SageId == sageId).ToListAsync();

              return sayings;
           }
        }
    }
}
