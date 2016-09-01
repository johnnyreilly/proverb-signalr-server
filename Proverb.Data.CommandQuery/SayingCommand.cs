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
   public class SayingCommand : ISayingCommand
   {
      public async Task<Result<int>> CreateAsync(Saying saying)
      {
         try
         {
            using (var context = new ProverbContext())
            {
               context.Sayings.Add(saying);

               var recordsSaved = await context.SaveChangesAsync();

               return Result.Ok(saying.Id);
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
               var entity = await context.Sayings.FindAsync(id);

               context.Sayings.Remove(entity);

               var recordsSaved = await context.SaveChangesAsync();

               return Result.Ok();
            }
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
            using (var context = new ProverbContext())
            {
               var dbSaying = await context.Sayings.FindAsync(saying.Id);
               dbSaying.UpdateWith(saying);

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
