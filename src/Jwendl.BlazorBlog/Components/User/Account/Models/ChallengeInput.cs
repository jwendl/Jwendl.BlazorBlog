namespace Jwendl.BlazorBlog.Components.User.Account.Models;

public class ChallengeInput
{
	public string RedirectUrl { get; set; } = default!;

	public string ExternalProviderName { get; set; } = default!;
}
