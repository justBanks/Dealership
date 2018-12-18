using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthorizationServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "vehicles_api",
                    DisplayName = "Vehicles API",
                    ApiSecrets = {new Secret("apisecret".Sha256())},
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "vehicles_api.read",
                            DisplayName = "Vehicles API read access",
                            Description = "Read access"
                        },
                        new Scope
                        {
                            Name = "vehicles_api.admin",
                            DisplayName = "Vehicles API admin access",
                            Description = "Read and write privileges"
                        }
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityScopes()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "simple_client",
                    ClientName = "Vehicles Client (auth code)",
                    AllowedScopes = {"vehicles_api.read", "vehicles_api.admin"},
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"http://localhost:50375/callback"},
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "implicit_client",
                    ClientName = "Implicit OAuth Client",
                    AllowedScopes = {"vehicles_api.read", "vehicles_api.admin"},
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = {"http://localhost:50375/callback.html"},
                    AllowedCorsOrigins = {"http://localhost:50375"},
                    AllowAccessTokensViaBrowser = true
                },
                new Client
                {
                    ClientId = "clientcreds_client",
                    ClientName = "Client Credentials OAuth Client",
                    AllowedScopes = {"vehicles_api.read", "vehicles_api.admin"},
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("secret".Sha256())}
                },
                new Client
                {
                    ClientId = "native_client",
                    ClientName = "Windows Native Client",
                    AllowedScopes = { "vehicles_api.read", "vehicles_api.admin" },
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"com.windows:/callback"},
                    RequireClientSecret = false,
                    RequirePkce = true
                },
                new Client
                {
                    ClientId = "oidc_client",
                    ClientName = "OpenID Connect Client",
                    AllowedScopes = {"openid", "profile","email", "vehicles_api.read", "vehicles_api.admin"},
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = {"http://localhost:5005/signin-oidc"},
                    ClientSecrets = {new Secret("secret".Sha256())}
                }
            };
        }
    }
}