using System.Threading.Tasks;
using Proverb.Data.CommandQuery.Interfaces;
using Proverb.Data.Common;
using Proverb.Data.EntityFramework;
using Proverb.Data.Models;

namespace Proverb.Data.CommandQuery
{
   public class SageCommand : ISageCommand
   {
      public async Task<Result<int>> CreateAsync(Sage sage)
      {
         try
         {
            using (var context = new ProverbContext())
            {
               context.Sages.Add(sage);

               var recordsSaved = await context.SaveChangesAsync();

               return Result.Ok(sage.Id);
            }
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
            using (var context = new ProverbContext())
            {
               var entity = await context.Sages.FindAsync(id);

               context.Sages.Remove(entity);

               var recordsSaved = await context.SaveChangesAsync();

               return Result.Ok();
            }
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
            using (var context = new ProverbContext())
            {
               var dbSaying = await context.Sages.FindAsync(sage.Id);
               dbSaying.UpdateWith(sage);

               var recordsSaved = await context.SaveChangesAsync();

               return Result.Ok();
            }
         }
         catch (System.Exception exc)
         {
            return Result.Fail(exc.Message);
         }
      }
   }
}
