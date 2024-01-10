using Jwendl.BlazorBlog.Components.User.Account.Models;
using Jwendl.BlazorBlog.Components.User.Account.Pages;
using Jwendl.BlazorBlog.Data.Identity.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Json;

namespace Jwendl.BlazorBlog.Components.User.Identity;

internal static class IdentityComponentsEndpointRouteBuilderExtensions
{
	public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
	{
		ArgumentNullException.ThrowIfNull(endpoints);

		var accountGroup = endpoints.MapGroup("/account");

		accountGroup.MapPost("/external/challenge", (
			HttpContext context,
			[FromServices] SignInManager<ApplicationUser> signInManager,
			[FromBody] ChallengeInput challengeInput) =>
		{
			IEnumerable<KeyValuePair<string, StringValues>> query = [
				new("ReturnUrl", challengeInput.RedirectUrl),
				new("Action", "LoginCallback")];

			var redirectUrl = UriHelper.BuildRelative(context.Request.PathBase, "/user/account/external/login", QueryString.Create(query));

			var properties = signInManager.ConfigureExternalAuthenticationProperties(challengeInput.ExternalProviderName, redirectUrl);

			return TypedResults.Challenge(properties, [challengeInput.ExternalProviderName]);
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
