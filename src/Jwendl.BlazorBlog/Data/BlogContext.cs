using Jwendl.BlazorBlog.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Jwendl.BlazorBlog.Data;

public class BlogContext(DbContextOptions<BlogContext> options)
	: DbContext(options)
{
	public DbSet<Post> Posts { get; set; }

	public DbSet<Category> Categories { get; set; }
}
