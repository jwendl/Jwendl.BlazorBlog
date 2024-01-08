using FluentValidation;
using Jwendl.BlazorBlog.Data.Blog.Models;

namespace Jwendl.BlazorBlog.Validators.Posts;

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
