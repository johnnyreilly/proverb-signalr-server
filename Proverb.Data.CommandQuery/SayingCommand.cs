using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using Proverb.Data.CommandQuery.Interfaces;
using Proverb.Data.EntityFramework;
using Proverb.Data.Models;
using Proverb.Data.CommandQuery;
using Proverb.Data.Common;

namespace Proverb.Data.CommandQuery
{
   public class SayingCommand : BaseCommandQuery, ISayingCommand
   {
      public SayingCommand(ProverbContext dbContext) : base(dbContext) { }

      public async Task<Result<int>> CreateAsync(Saying saying)
      {
         try
         {
            DbContext.Sayings.Add(saying);

            var recordsSaved = await DbContext.SaveChangesAsync();

            return Result.Ok(saying.Id);
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
            var entity = await DbContext.Sayings.FindAsync(id);

            DbContext.Sayings.Remove(entity);

            var recordsSaved = await DbContext.SaveChangesAsync();

            return Result.Ok();
         }
         catch (System.Exception exc)
         {
            return Result.Fail(exc.Message);
         }
      }

      public async Task<Result> UpdateAsync(Saying saying)
      {
         try
         {
            var dbSaying = await DbContext.Sayings.FindAsync(saying.Id);
            dbSaying.UpdateWith(saying);

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
