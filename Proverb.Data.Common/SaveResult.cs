using System.Collections.Generic;

namespace Proverb.Data.Common
{
   public class SaveResult
   {
      public static SaveResult Success(int savedId)
      {
         return new SaveResult(isSaved: true, savedId: savedId, validations: ValidationMessages.None);
      }

      public static SaveResult Fail(ValidationMessages validationMessages)
      {
         return new SaveResult(isSaved: false, savedId: null, validations: validationMessages);
      }

      public static SaveResult Fail(string failureReason)
      {
         return new SaveResult(isSaved: false, savedId: null, validations: new ValidationMessages(new Dictionary<string, IEnumerable<string>>
         {
            { "", new[] {failureReason} }
         }));
      }

      private SaveResult(bool isSaved, int? savedId, ValidationMessages validations)
      {
         IsSaved = isSaved;
         SavedId = savedId;
         Validations = validations;
      }

      public bool IsSaved { get; }
      public int? SavedId { get; }
      public ValidationMessages Validations { get; }
   }
}