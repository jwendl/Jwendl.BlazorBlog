using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jwendl.BlazorBlog.Data.Models;

public class Page
{
	public int Id { get; set; }

	public Guid StaticId { get; set; }

	public string Title { get; set; }

	public int UserId { get; set; }

	public User User { get; set; }

	public string Content { get; set; }
}
