using Jwendl.BlazorBlog.Components.User.Account.Pages;
using Jwendl.BlazorBlog.Data.Identity.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace Jwendl.BlazorBlog.Components.User.Identity;

internal static class IdentityComponentsEndpointRouteBuilderExtensions
{
	public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
	{
		ArgumentNullException.ThrowIfNull(endpoints);

		var accountGroup = endpoints.MapGroup("/account");

		accountGroup.MapGet("/account/external/challenge", (
			HttpContext context,
			[FromServices] SignInManager<ApplicationUser> signInManager,
			[FromQuery] string provider,
			[FromQuery] string returnUrl) =>
		{
			IEnumerable<KeyValuePair<string, StringValues>> query = [
				new("ReturnUrl", returnUrl),
				new("Action", ExternalLogin.LoginCallbackAction)];

			var redirectUrl = UriHelper.BuildRelative(
				context.Request.PathBase,
				"/user/account/external/login",
				QueryString.Create(query));

			var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			return TypedResults.Challenge(properties, [provider]);
		});

		accountGroup.MapPost("/Logout", async (
			ClaimsPrincipal user,
			SignInManager<ApplicationUser> signInManager,
			[FromForm] string returnUrl) =>
		{
			await signInManager.SignOutAsync();
			return TypedResults.LocalRedirect($"~/{returnUrl}");
		});

		return accountGroup;
	}
}
