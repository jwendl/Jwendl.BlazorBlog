namespace Jwendl.BlazorBlog.Data.Blog.Models;

public class User
{
	public Guid ObjectId { get; set; }

	public string UserPrincipalName { get; set; } = default!;

	public string IdentityProvider { get; set; } = default!;
}
