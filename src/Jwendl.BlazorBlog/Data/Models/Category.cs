﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jwendl.BlazorBlog.Data.Models;

public class Category
{
	public int Id { get; set; }

	public Guid StaticId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public List<Post> Posts { get; } = [];
}
