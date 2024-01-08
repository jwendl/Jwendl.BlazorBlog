using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwendl.BlazorBlog.Validators;

public class BaseValidator<T>
	: AbstractValidator<T>
{
	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var validationContext = ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName));
		var validationResult = await ValidateAsync(validationContext);
		if (validationResult.IsValid)
		{
			return Array.Empty<string>();
		}

		return validationResult.Errors.Select(e => e.ErrorMessage);
	};
}
