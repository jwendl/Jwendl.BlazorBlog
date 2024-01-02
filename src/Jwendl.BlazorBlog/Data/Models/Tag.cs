using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jwendl.BlazorBlog.Data.Models;

public class Tag
{
	public int Id { get; set; }

	public List<Post> Posts { get; } = [];
}
