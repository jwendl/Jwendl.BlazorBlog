using System;

namespace Jwendl.BlazorBlog.Data.Blog.Models;

public class User
{
	public Guid ObjectId { get; set; }

	public string UserPrincipalName { get; set; }

	public string IdentityProvider { get; set; }
}
