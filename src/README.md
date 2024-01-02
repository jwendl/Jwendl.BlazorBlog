# Source Code

## Setting up secrets

Use the Microsoft for the following:
- [Google Login](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0)
- [Facebook Login](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins?view=aspnetcore-8.0)
- [Microsoft (hotmail / live) Logins](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-8.0)
- [Twitter](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/twitter-logins?view=aspnetcore-8.0)
- [Microsoft Entra](https://learn.microsoft.com/en-us/entra/identity-platform/tutorial-blazor-server)

## An example

```powershell
dotnet user-secrets init
dotnet user-secrets set "Authentication:AzureAd:Domain" ""
dotnet user-secrets set "Authentication:AzureAd:TenantId" ""
dotnet user-secrets set "Authentication:AzureAd:ClientId" ""
dotnet user-secrets set "Authentication:AzureAd:ClientSecret" ""
dotnet user-secrets set "Authentication:Google:ClientId" ""
dotnet user-secrets set "Authentication:Google:ClientSecret" ""
dotnet user-secrets set "Authentication:Facebook:AppId" ""
dotnet user-secrets set "Authentication:Facebook:AppSecret" ""
dotnet user-secrets set "Authentication:MicrosoftAccount:ClientId" ""
dotnet user-secrets set "Authentication:MicrosoftAccount:ClientSecret" ""
dotnet user-secrets set "Authentication:Twitter:ClientId" ""
dotnet user-secrets set "Authentication:Twitter:ClientSecret" ""
```
