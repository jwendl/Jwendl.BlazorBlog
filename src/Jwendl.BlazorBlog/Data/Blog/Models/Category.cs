using System;
using System.Collections.Generic;

namespace Jwendl.BlazorBlog.Data.Blog.Models;

public class Category
{
	public int Id { get; set; }

	public Guid StaticId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public List<Post> Posts { get; } = [];
}
