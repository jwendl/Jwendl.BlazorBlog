using Jwendl.BlazorBlog.Components;
using Jwendl.BlazorBlog.Components.Account;
using Jwendl.BlazorBlog.Data;
using Jwendl.BlazorBlog.Data.Blog;
using Jwendl.BlazorBlog.Data.Identity;
using Jwendl.BlazorBlog.Data.Identity.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
var configurationManager = builder.Configuration ?? throw new InvalidOperationException("builder.Configuration is null.");
var blogConnectionString = configurationManager.GetValue<string>("Database:BlogConnectionString") ?? throw new InvalidOperationException("Connection string 'Database:BlogConnectionString' not found.");
var userConnectionString = configurationManager.GetValue<string>("Database:UserConnectionString") ?? throw new InvalidOperationException("Connection string 'Database:UserConnectionString' not found.");

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddControllersWithViews()
	.AddMicrosoftIdentityUI();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = configurationManager.GetValue<string>("Authentication:MicrosoftAccount:ClientId") ?? string.Empty;
        microsoftOptions.ClientSecret = configurationManager.GetValue<string>("Authentication:MicrosoftAccount:ClientSecret") ?? string.Empty;
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = configurationManager.GetValue<string>("Authentication:Facebook:AppId") ?? string.Empty;
        facebookOptions.AppSecret = configurationManager.GetValue<string>("Authentication:Facebook:AppSecret") ?? string.Empty;
    })
    .AddTwitter(twitterOptions =>
    {
        twitterOptions.ConsumerKey = configurationManager.GetValue<string>("Authentication:Twitter:ClientId") ?? string.Empty;
        twitterOptions.ConsumerSecret = configurationManager.GetValue<string>("Authentication:Twitter:ClientSecret") ?? string.Empty;
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = configurationManager.GetValue<string>("Authentication:Google:ClientId") ?? string.Empty;
        googleOptions.ClientSecret = configurationManager.GetValue<string>("Authentication:Google:ClientSecret") ?? string.Empty;
    })
    .AddMicrosoftIdentityWebApp(azureAdOptions =>
    {
        azureAdOptions.Instance = configurationManager.GetValue<string>("Authentication:AzureAd:Instance") ?? string.Empty;
        azureAdOptions.CallbackPath = configurationManager.GetValue<string>("Authentication:AzureAd:CallbackPath");
        azureAdOptions.TenantId = configurationManager.GetValue<string>("Authentication:AzureAd:TenantId");
        azureAdOptions.Domain = configurationManager.GetValue<string>("Authentication:AzureAd:Domain");
        azureAdOptions.ClientId = configurationManager.GetValue<string>("Authentication:AzureAd:ClientId");
        azureAdOptions.ClientSecret = configurationManager.GetValue<string>("Authentication:AzureAd:ClientSecret");
    });

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
    if (string.IsNullOrWhiteSpace(blogConnectionString))
    {
        options.UseInMemoryDatabase("Blog");
    }
    else
    {
        options.UseSqlServer(blogConnectionString);
    }
});

builder.Services.AddDbContextFactory<IdentityContext>(options =>
{
    if (string.IsNullOrWhiteSpace(userConnectionString))
    {
        options.UseInMemoryDatabase("User");
    }
    else
    {
        options.UseSqlServer(userConnectionString);
    }
});

builder.Services.AddMudServices();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

var app = builder.Build();

if (string.IsNullOrEmpty(blogConnectionString))
{
	await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
	var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<BlogContext>>();
	await DatabaseUtility.EnsureDbCreatedAndSeedWithDataAsync(options);
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();
