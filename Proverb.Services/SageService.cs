using System.Collections.Generic;
using System.Threading.Tasks;
using Proverb.Data.CommandQuery.Interfaces;
using Proverb.Data.Common;
using Proverb.Data.Models;
using Proverb.Services.Interfaces;

namespace Proverb.Services
{
   public class SageService : ISageService
   {
      public SageService(ISageCommand sageCommand, ISageQuery sageQuery)
      {
         _sageCommand = sageCommand;
         _sageQuery = sageQuery;
      }

      private readonly ISageCommand _sageCommand;
      private readonly ISageQuery _sageQuery;

      public async Task<Result<int>> CreateAsync(Sage sage)
      {
         return await _sageCommand.CreateAsync(sage);
      }

      public async Task DeleteAsync(int id)
      {
         await _sageCommand.DeleteAsync(id);
      }

      public async Task<ICollection<Sage>> GetAllAsync()
      {
         return await _sageQuery.GetAllAsync();
      }

      public async Task<Sage> GetByIdAsync(int id)
      {
         return await _sageQuery.GetByIdAsync(id);
      }

      public async Task<Result> UpdateAsync(Sage sage)
      {
         return await _sageCommand.UpdateAsync(sage);
      }

      public ValidationMessages Validate(Sage saying)
      {
         var validations = new Dictionary<string, IEnumerable<string>>(ValidationHelpers.GetFieldValidations(saying).Errors);

         return new ValidationMessages(validations);
      }

   }
}
