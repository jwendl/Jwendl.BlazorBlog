using FluentValidation;
using Jwendl.BlazorBlog.Models;

namespace Jwendl.BlazorBlog.Validators.Login;

public class FormPostRedirectionValidator
	: BaseValidator<FormPostRedirectionUrl>
{
	public FormPostRedirectionValidator()
	{
		RuleFor(f => f.ReturnUrl).NotEmpty();
	}
}
