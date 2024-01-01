using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System;
using System.Collections.Generic;

namespace Jwendl.BlazorBlog.Data.Models;

public class Category
{
	public int Id { get; set; }

	public Guid StaticId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public ICollection<Post> Posts { get; set; } = new List<Post>();
}
