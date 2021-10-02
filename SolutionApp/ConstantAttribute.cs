using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SolutionApp
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ConstantAttribute : ValidationAttribute
    {
        private readonly Type _type;

        public ConstantAttribute(Type type)
        {
            _type = type;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult($"'{validationContext.MemberName}' cannot be null.");
            var options = GetConstantValues<string>(_type);
            return options.Contains((string)value)
                ? ValidationResult.Success
                : new ValidationResult($"'{value}' is not a valid {validationContext.MemberName}.");
        }

        /// <summary>
        /// Gets all the constant value in class of the specified <paramref name="classType"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classType"></param>
        /// <returns></returns>
        private static List<T> GetConstantValues<T>(Type classType) =>
            classType.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(f => (T)f.GetRawConstantValue()).ToList();


    }
}
