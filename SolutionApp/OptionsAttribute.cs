using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SolutionApp
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class OptionsAttribute : ValidationAttribute
	{
		private readonly object[] _options;
		private readonly Type _type;
		private readonly bool _allowNull;
		public OptionsAttribute(Type type, [NotNull] params object[] options)
		{
			_options = options;
			_type = type;
			_allowNull = false;
		}

		public OptionsAttribute(Type type, bool allowNull, [NotNull] params object[] options)
		{
			_options = options;
			_type = type;
			_allowNull = allowNull;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			switch (value)
			{
				case null when _allowNull:
					return ValidationResult.Success;
				case null:
					return new ValidationResult($"'{validationContext.MemberName}' cannot be null.");
				default:
					{
						var option = TypeDescriptor.GetConverter(_type).ConvertFrom(value);
						return _options.Contains(option)
							? ValidationResult.Success
							: new ValidationResult($"'{value}' is not a valid {validationContext.MemberName}.");
					}
			}
		}

	}
}
