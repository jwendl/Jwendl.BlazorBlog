using FluentValidation;
using Jwendl.BlazorBlog.Components.Shared.Validators;
using Jwendl.BlazorBlog.Data.Blog.Models;

namespace Jwendl.BlazorBlog.Components.Admin.Posts.Validators;

public class PostValidator
	: BaseValidator<Post>
{
	public PostValidator()
	{
		RuleFor(x => x.Title).NotEmpty();
		RuleFor(x => x.Content).NotEmpty();
		RuleFor(x => x.CategoryId).NotEmpty();
	}
}
