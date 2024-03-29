﻿using System;

namespace Jwendl.BlazorBlog.Data.Models;

public class Post
{
	public int Id { get; set; }

	public Guid StaticId { get; set; }

	public string Title { get; set; }

	public string Content { get; set; }

	public int CategoryId { get; set; }

	public Category Category { get; set; }
}
