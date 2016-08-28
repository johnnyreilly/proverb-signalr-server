using System;
using System.ComponentModel.DataAnnotations;

namespace Proverb.Data.Models
{
   public class User
   {
      public int Id { get; set; }

      [Required]
      public string Name { get; set; }

      [Required]
      [StringLength(30, MinimumLength = 3)]
      public string UserName { get; set; }

      [EmailAddress]
      public string Email { get; set; }

      public DateTime? DateOfBirth { get; set; }

      public void UpdateWith(User updateWith)
      {
         Name = updateWith.Name;
         UserName = updateWith.UserName;
         Email = updateWith.Email;
         DateOfBirth = updateWith.DateOfBirth;
      }
   }
}
