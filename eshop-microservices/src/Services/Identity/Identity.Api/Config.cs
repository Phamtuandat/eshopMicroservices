using Duende.IdentityServer.Models;

namespace Identity.Api
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("read"),
                new ApiScope("write"),
                new ApiScope("offline_access"),

            };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("catalog.api"),
            new ApiResource("basket.api"),
            new ApiResource("order.api"),

        };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
          
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "read", "write", "offline_access" }
                },
            };
    }
}
