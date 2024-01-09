namespace Jwendl.BlazorBlog.Data.Blog.Models;

public class Page
{
	public int Id { get; set; }

	public Guid StaticId { get; set; }

	public string Title { get; set; } = default!;

	public int UserId { get; set; }

	public User User { get; set; } = default!;

	public string Content { get; set; } = default!;
}
