using System.Collections.Generic;
using System.Threading.Tasks;
using Proverb.Data.Common;
using Proverb.Data.Models;

namespace Proverb.Web.Hubs
{
    public interface ISayingFeed
    {
        Task<IEnumerable<Saying>> GetAll();
        Task<Saying> Get(int id);
        Task<SaveResult> Save(Saying saying);
        Task Remove(int id);
    }
}