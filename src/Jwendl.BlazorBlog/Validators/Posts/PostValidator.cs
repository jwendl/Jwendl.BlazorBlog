using FluentValidation;
using Jwendl.BlazorBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwendl.BlazorBlog.Validators.Posts;

public class PostValidator
	: AbstractValidator<Post>
{
	public PostValidator()
	{
		RuleFor(x => x.Title).NotEmpty();
		RuleFor(x => x.Content).NotEmpty();
		RuleFor(x => x.CategoryId).NotEmpty();
	}

	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var validationContext = ValidationContext<Post>.CreateWithOptions((Post)model, x => x.IncludeProperties(propertyName));
		var validationResult = await ValidateAsync(validationContext);
		if (validationResult.IsValid)
		{
			return Array.Empty<string>();
		}

		return validationResult.Errors.Select(e => e.ErrorMessage);
	};
}
