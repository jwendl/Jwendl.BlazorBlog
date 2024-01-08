using Microsoft.EntityFrameworkCore;

namespace Jwendl.BlazorBlog.Data.Identity;

public class IdentityContext(DbContextOptions<IdentityContext> options)
	: DbContext(options)
{

}
