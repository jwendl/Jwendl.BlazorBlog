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
var connectionString = configurationManager.GetValue<string>("Database:ConnectionString");

builder.Services.Configure<BlogOptions>(configurationManager.GetSection("Authentication:Google:ClientId"));

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
	.AddMicrosoftAccount(microsoftOptions =>
	{
		microsoftOptions.ClientId = configurationManager.GetValue<string>("Authentication:MicrosoftAccount:ClientId");
		microsoftOptions.ClientSecret = configurationManager.GetValue<string>("Authentication:MicrosoftAccount:ClientSecret");
	})
	.AddFacebook(facebookOptions =>
	{
		facebookOptions.AppId = configurationManager.GetValue<string>("Authentication:Facebook:AppId");
		facebookOptions.AppSecret = configurationManager.GetValue<string>("Authentication:Facebook:AppSecret");
	})
	.AddTwitter(twitterOptions =>
	{
		twitterOptions.ConsumerKey = configurationManager.GetValue<string>("Authentication:Twitter:ClientId");
		twitterOptions.ConsumerSecret = configurationManager.GetValue<string>("Authentication:Twitter:ClientSecret");
	})
	.AddGoogle(googleOptions =>
	{
		googleOptions.ClientId = configurationManager.GetValue<string>("Authentication:Google:ClientId");
		googleOptions.ClientSecret = configurationManager.GetValue<string>("Authentication:Google:ClientSecret");
	})
	.AddMicrosoftIdentityWebApp(azureAdOptions =>
	{
		azureAdOptions.Instance = configurationManager.GetValue<string>("Authentication:AzureAd:Instance");
		azureAdOptions.CallbackPath = configurationManager.GetValue<string>("Authentication:AzureAd:CallbackPath");
		azureAdOptions.TenantId = configurationManager.GetValue<string>("Authentication:AzureAd:TenantId");
		azureAdOptions.Domain = configurationManager.GetValue<string>("Authentication:AzureAd:Domain");
		azureAdOptions.ClientId = configurationManager.GetValue<string>("Authentication:AzureAd:ClientId");
		azureAdOptions.ClientSecret = configurationManager.GetValue<string>("Authentication:AzureAd:ClientSecret");
	});

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
