namespace Jwendl.BlazorBlog.Data.Blog.Models;

public class Category
{
	public int Id { get; set; }

	public Guid StaticId { get; set; }

	public string Name { get; set; } = default!;

	public string Description { get; set; } = default!;

	public List<Post> Posts { get; } = [];
}
