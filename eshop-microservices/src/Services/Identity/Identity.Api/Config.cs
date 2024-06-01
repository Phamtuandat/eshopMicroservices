using Duende.IdentityServer.Models;
using IdentityModel;

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
                new ApiScope("catalog.api"),
                new ApiScope("basket.api"),
                new ApiScope("ordering.api"),
            };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
          {
            new ApiResource("catalog.api", "Catalog API")
            {
                Scopes = { "catalog.api", "read", "write" },
                ApiSecrets = { new Secret("secret".Sha256()) },
                 UserClaims = { JwtClaimTypes.Role }
            },
            new ApiResource("basket.api", "Basket API")
            {
                Scopes = { "basket.api", "read", "write" },
                ApiSecrets = { new Secret("secret".Sha256()) },
                 UserClaims = { JwtClaimTypes.Role }
            },
            new ApiResource("ordering.api", "Ordering API")
            {
                Scopes = { "ordering.api", "read", "write" },
                ApiSecrets = { new Secret("secret".Sha256()) },
                 UserClaims = { JwtClaimTypes.Role }
            },
          };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
          
                // interactive client using code flow + pkce
              new Client
                {
                    ClientId = "angular_spa",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "basket.api",
                        "catalog.api",
                        "ordering.api",
                        "read",
                        "write"
                    },
                    RedirectUris = { "http://localhost:4200/callback" },
                    PostLogoutRedirectUris = { "http://localhost:4200" },
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600, // 1 hour
                }
            };
    }
}
