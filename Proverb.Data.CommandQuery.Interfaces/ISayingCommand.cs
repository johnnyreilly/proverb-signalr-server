using System.Threading.Tasks;
using Proverb.Data.Common;
using Proverb.Data.Models;

namespace Proverb.Data.CommandQuery.Interfaces
{
    public interface ISayingCommand
    {
        Task<Result<int>> CreateAsync(Saying saying);
        Task<Result> DeleteAsync(int id);
        Task<Result> UpdateAsync(Saying saying);
    }
}
