using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Proverb.Data.Common
{
    public static class ValidationHelpers
    {
        public static string GetFieldName<T>(T entity, Expression<Func<T, object>> expression, bool camelCaseKeyName = true) where T : class
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                var ubody = (UnaryExpression)expression.Body;
                body = ubody.Operand as MemberExpression;
            }

            if (body == null)
                throw new ArgumentException("Invalid property expression");

            var entityName = typeof(T).Name; // eg "Saying"
            var property = body.Member.Name; // eg "SageId"
            var combined = entityName + "." + property;

            return combined;
        }

      /// <summary>
      /// Take a model and revalidate it to extract FieldValidations
      /// </summary>
      /// <typeparam name="TModelToValidate"></typeparam>
      /// <param name="modelToValidate"></param>
      /// <param name="fieldNameMaker">a Func that generates a the error name (used so you can control this depending on your GUI)</param>
      /// <param name="extraData"></param>
      /// <returns></returns>
      public static ValidationMessages GetFieldValidations<TModelToValidate>(TModelToValidate modelToValidate)
      {
         if (modelToValidate == null) throw new ArgumentNullException(nameof(modelToValidate));

         var context = new ValidationContext(modelToValidate, serviceProvider: null, items: null);
         var results = new List<ValidationResult>();

         var isValid = Validator.TryValidateObject(modelToValidate, context, results, true);

         if (isValid)
            return ValidationMessages.None;

         return new ValidationMessages(
              (from validationResult in results
               from errorProperty in validationResult.MemberNames
               let fieldName = errorProperty
               select new
               {
                  fieldName = fieldName,
                  message = validationResult.ErrorMessage
               })
               .GroupBy(x => x.fieldName)
               .ToDictionary(x => x.Key, x => x.Select(y => y.message))
         );
      }
   }
}
