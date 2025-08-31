using Jetwise.Clients.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(options =>
{builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.Authority = "https://dev-onuukffx52o7o3w3.us.auth0.com/";
    options.ProviderOptions.ClientId = "EKuqSZ5jWrBTNRySkpSYsoQfs3IdEaeX";
    options.ProviderOptions.ResponseType = "code"; // PKCE
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
    options.ProviderOptions.DefaultScopes.Add("offline_access");
    //options.ProviderOptions.RedirectUri = "https://127.0.0.1:7289/authentication/login-callback";
    options.ProviderOptions.RedirectUri = "https://blazor.local/authentication/login-callback"; 
    // ⬇ to jest kluczowe dla dostępu do Twojego API (Audience)
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", "https://jetwise-gateway/"); 
});

// MessageHandler, który będzie automatycznie dołączał Bearer token do wywołań gateway
builder.Services.AddScoped<AuthorizationMessageHandler>(sp =>
{
    var handler = new AuthorizationMessageHandler(
        sp.GetRequiredService<IAccessTokenProvider>(),
        sp.GetRequiredService<NavigationManager>()
    );
    handler.ConfigureHandler(
        authorizedUrls: new[] { "https://localhost:5184" }//adresy do któych będą dokładane tokeny autoryzacji
        //,scopes: new[] { "bookings:read", "bookings:write" } // czego potrzebujesz
    );
    return handler;
});

builder.Services.AddHttpClient("GatewayAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:5184");
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

 
await builder.Build().RunAsync();
