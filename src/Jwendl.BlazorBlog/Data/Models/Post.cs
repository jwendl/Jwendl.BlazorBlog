using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jwendl.BlazorBlog.Data.Models;

public class Post
{
	public int Id { get; set; }

	public Guid StaticId { get; set; }

	public string Title { get; set; }

	public string Content { get; set; }

	public int CategoryId { get; set; }

	public Category Category { get; set; }

	public int UserId { get; set; }

	public User User { get; set; }

	public List<Tag> Tags { get; } = [];
}
