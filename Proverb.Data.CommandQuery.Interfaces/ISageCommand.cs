using System.Threading.Tasks;
using Proverb.Data.Common;
using Proverb.Data.Models;

namespace Proverb.Data.CommandQuery.Interfaces
{
    public interface ISageCommand
    {
        Task<Result<int>> CreateAsync(Sage sage);
        Task<Result> DeleteAsync(int id);
        Task<Result> UpdateAsync(Sage sage);
    }
}
