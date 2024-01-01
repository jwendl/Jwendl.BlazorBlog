using Jwendl.BlazorBlog;
using Jwendl.BlazorBlog.Data;
using Jwendl.BlazorBlog.Options;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
var configurationManager = builder.Configuration;
var downstreamApiScopes = configurationManager.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');
var connectionString = configurationManager.GetValue<string>("Database:ConnectionString");

builder.Services.Configure<BlogOptions>(configurationManager.GetSection(""));

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApp(configurationManager.GetSection("AzureAd"))
		.EnableTokenAcquisitionToCallDownstreamApi(downstreamApiScopes)
			.AddMicrosoftGraph(configurationManager.GetSection("DownstreamApi"))
			.AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews()
	.AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Blog Administrator", policy =>
	{
		policy.RequireClaim("groups", "fbdec060-6cec-4b99-bc5f-1178d9b44c35");
	});
	options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddDbContextFactory<BlogContext>(options =>
{
	if (string.IsNullOrWhiteSpace(connectionString))
	{
		options.UseInMemoryDatabase("Blog");
	}
	else
	{
		options.UseSqlServer(connectionString);
	}
});

builder.Services.AddMudServices();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
	.AddMicrosoftIdentityConsentHandler();

var app = builder.Build();

if (string.IsNullOrEmpty(connectionString))
{
	await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
	var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<BlogContext>>();
	await DatabaseUtility.EnsureDbCreatedAndSeedWithDataAsync(options);
}

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
