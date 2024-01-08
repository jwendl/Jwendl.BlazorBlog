using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Jwendl.BlazorBlog.Components.Account;

internal sealed class IdentityRedirectManager(NavigationManager _navigationManager)
{
	public const string _statusCookieName = "Identity.StatusMessage";

	private static readonly CookieBuilder _statusCookieBuilder = new()
	{
		SameSite = SameSiteMode.Strict,
		HttpOnly = true,
		IsEssential = true,
		MaxAge = TimeSpan.FromSeconds(5),
	};

	[DoesNotReturn]
	public void RedirectTo(string uri)
	{
		uri ??= "";

		if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
		{
			uri = _navigationManager.ToBaseRelativePath(uri);
		}

		_navigationManager.NavigateTo(uri);
		throw new InvalidOperationException($"{nameof(IdentityRedirectManager)} can only be used during static rendering.");
	}

	[DoesNotReturn]
	public void RedirectTo(string uri, Dictionary<string, object?> queryParameters)
	{
		var uriWithoutQuery = _navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
		var newUri = _navigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);
		RedirectTo(newUri);
	}

	[DoesNotReturn]
	public void RedirectToWithStatus(string uri, string message, HttpContext context)
	{
		context.Response.Cookies.Append(_statusCookieName, message, _statusCookieBuilder.Build(context));
		RedirectTo(uri);
	}
	private string CurrentPath => _navigationManager.ToAbsoluteUri(_navigationManager.Uri).GetLeftPart(UriPartial.Path);

	[DoesNotReturn]
	public void RedirectToCurrentPage() => RedirectTo(CurrentPath);

	[DoesNotReturn]
	public void RedirectToCurrentPageWithStatus(string message, HttpContext context)
		=> RedirectToWithStatus(CurrentPath, message, context);
}
