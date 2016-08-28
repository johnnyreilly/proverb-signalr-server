using System.Data.Entity;
using System.Threading.Tasks;
using Proverb.Data.CommandQuery.Interfaces;
using Proverb.Data.Common;
using Proverb.Data.EntityFramework;
using Proverb.Data.Models;

namespace Proverb.Data.CommandQuery
{
   public class SageCommand : BaseCommandQuery, ISageCommand
   {
      public SageCommand(ProverbContext dbContext) : base(dbContext) { }

      public async Task<Result<int>> CreateAsync(Sage sage)
      {
         try
         {
            DbContext.Sages.Add(sage);

            var recordsSaved = await DbContext.SaveChangesAsync();

            return Result.Ok(sage.Id);
         }
         catch (System.Exception exc)
         {
            return Result.Fail<int>(exc.Message);
         }
      }

      public async Task<Result> DeleteAsync(int id)
      {
         try
         {
            var entity = await DbContext.Sages.FindAsync(id);

            DbContext.Sages.Remove(entity);

            var recordsSaved = await DbContext.SaveChangesAsync();

            return Result.Ok();
         }
         catch (System.Exception exc)
         {
            return Result.Fail(exc.Message);
         }
      }

      public async Task<Result> UpdateAsync(Sage sage)
      {
         try
         {
            var dbSaying = await DbContext.Sages.FindAsync(sage.Id);
            dbSaying.UpdateWith(sage);

            var recordsSaved = await DbContext.SaveChangesAsync();

            return Result.Ok();
         }
         catch (System.Exception exc)
         {
            return Result.Fail(exc.Message);
         }
      }
   }
}
