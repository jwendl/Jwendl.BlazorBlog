using Bogus;
using Jwendl.BlazorBlog.Data.Blog;
using Jwendl.BlazorBlog.Data.Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jwendl.BlazorBlog.Data;

public static class DatabaseUtility
{
	public static async Task EnsureDbCreatedAndSeedWithDataAsync(DbContextOptions<BlogContext> options)
	{
		var loggerFactory = new LoggerFactory();
		var dbContextBuilder = new DbContextOptionsBuilder<BlogContext>(options)
			.UseLoggerFactory(loggerFactory);

		using var blogContext = new BlogContext(dbContextBuilder.Options);
		if (await blogContext.Database.EnsureCreatedAsync())
		{
			var categoryIds = 0;
			var categoryGenerator = new Faker<Category>()
				.RuleFor(c => c.Id, categoryIds++)
				.RuleFor(c => c.StaticId, f => Guid.NewGuid())
				.RuleFor(c => c.Name, f => f.Commerce.Categories(1).First());

			var categories = categoryGenerator
				.RuleFor(c => c.Description, f => f.Lorem.Paragraph())
				.Generate(10);

			await blogContext.Categories.AddRangeAsync(categories);

			var postIds = 0;
			var postGenerator = new Faker<Post>()
				.RuleFor(p => p.Id, postIds++)
				.RuleFor(p => p.StaticId, f => Guid.NewGuid())
				.RuleFor(p => p.Title, f => f.Commerce.ProductName())
				.RuleFor(p => p.Category, f => f.PickRandom(categories))
				.RuleFor(p => p.Content, f => f.Lorem.Paragraphs(2));

			var posts = postGenerator
				.Generate(50);

			await blogContext.Posts.AddRangeAsync(posts);

			await blogContext.SaveChangesAsync();
		}
	}
}
