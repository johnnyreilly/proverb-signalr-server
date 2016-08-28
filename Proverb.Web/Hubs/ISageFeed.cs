using System.Collections.Generic;
using System.Threading.Tasks;
using Proverb.Data.Common;
using Proverb.Data.Models;

namespace Proverb.Web.Hubs
{
    public interface ISageFeed
    {
        Task<IEnumerable<Sage>> GetAll();
        Task<Sage> Get(int id);
        Task<SaveResult> Save(Sage saying);
        Task Remove(int id);
    }
}