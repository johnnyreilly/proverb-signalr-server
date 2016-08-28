using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Proverb.Data.Common
{
   public class ValidationMessages
   {
      public static ValidationMessages None { get; } = new ValidationMessages();
      private ValidationMessages()
      {
         Errors = new ReadOnlyDictionary<string, IEnumerable<string>>(new Dictionary<string, IEnumerable<string>>());
      }

      public ValidationMessages(Dictionary<string, IEnumerable<string>> errors)
      {
         Errors = new ReadOnlyDictionary<string, IEnumerable<string>>(errors);
      }

      public ReadOnlyDictionary<string, IEnumerable<string>> Errors { get; }

      public bool HasErrors => Errors.Keys.Any();
   }
}
