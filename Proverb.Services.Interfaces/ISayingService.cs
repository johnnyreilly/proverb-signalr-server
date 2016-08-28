using System.Collections.Generic;
using System.Threading.Tasks;
using Proverb.Data.Common;
using Proverb.Data.Models;

namespace Proverb.Services.Interfaces
{
    public interface ISayingService
    {
        Task<Result<int>> CreateAsync(Saying saying);
        Task<Result> DeleteAsync(int id);
        Task<ICollection<Saying>> GetAllAsync();
        Task<Saying> GetByIdAsync(int id);
        Task<Result> UpdateAsync(Saying saying);
        ValidationMessages Validate(Saying saying);
    }
}
