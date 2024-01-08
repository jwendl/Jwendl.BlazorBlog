using Jwendl.BlazorBlog.Data.Blog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace Jwendl.BlazorBlog.Data.Blog;

public class BlogContext(DbContextOptions<BlogContext> options)
	: DbContext(options)
{
	public DbSet<Category> Categories { get; set; }

	public DbSet<Comment> Comments { get; set; }

	public DbSet<Page> Pages { get; set; }

	public DbSet<Post> Posts { get; set; }

	public DbSet<PostTag> PostTags { get; set; }

	public DbSet<Tag> Tags { get; set; }

	public DbSet<User> Users { get; set; }

	private static string PropertyName<T>(Expression<Func<T, object>> property)
		where T : class
	{
		if (property.Body is MemberExpression memberExpressionBody)
		{
			return memberExpressionBody.Member.Name;
		}
		else if (property.Body is UnaryExpression unaryExpressionBody)
		{
			var memberExpression = (MemberExpression)unaryExpressionBody.Operand;
			return memberExpression.Member.Name;
		}

		return string.Empty;
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Category>()
			.HasKey(PropertyName<Category>(c => c.Id));

		modelBuilder.Entity<Category>()
			.Property(c => c.Id)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<Category>()
			.HasMany(c => c.Posts)
			.WithOne(p => p.Category);

		modelBuilder.Entity<Comment>()
			.HasKey(PropertyName<Comment>(c => c.Id));

		modelBuilder.Entity<Comment>()
			.Property(c => c.Id)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<Page>()
			.HasKey(PropertyName<Page>(p => p.Id));

		modelBuilder.Entity<Page>()
			.Property(p => p.Id)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<Post>()
			.HasKey(PropertyName<Post>(p => p.Id));

		modelBuilder.Entity<Post>()
			.Property(p => p.Id)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<Post>()
			.HasOne(p => p.Category)
			.WithMany(c => c.Posts)
			.HasForeignKey(p => p.CategoryId);

		modelBuilder.Entity<Post>()
			.HasMany(p => p.Tags)
			.WithMany(t => t.Posts)
			.UsingEntity<PostTag>(
				left => left.HasOne<Tag>().WithMany().HasForeignKey(pt => pt.TagId),
				right => right.HasOne<Post>().WithMany().HasForeignKey(pt => pt.PostId)
			);

		modelBuilder.Entity<PostTag>()
			.HasNoKey();

		modelBuilder.Entity<Tag>()
			.HasKey(PropertyName<Tag>(t => t.Id));

		modelBuilder.Entity<Tag>()
			.HasMany(t => t.Posts)
			.WithMany(p => p.Tags)
			.UsingEntity<PostTag>(
				left => left.HasOne<Post>().WithMany().HasForeignKey(pt => pt.PostId),
				right => right.HasOne<Tag>().WithMany().HasForeignKey(pt => pt.TagId)
			);

		modelBuilder.Entity<Tag>()
			.Property(t => t.Id)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<User>()
			.HasKey(PropertyName<User>(u => u.ObjectId));

		base.OnModelCreating(modelBuilder);
	}
}
