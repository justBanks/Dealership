using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthorizationServer
{
    public class TestUsers
    {
        public static readonly List<TestUser> Users = new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "A0E6dj8439md43k892d48s37d9js21",
                Username = "test-user",
                Password = "test",
                Claims =
                {
                    new Claim("given_name", "Jane"),
                    new Claim("family_name", "Jones"),
                }
            },
            new TestUser
            {
                SubjectId = "774a0068e9c04e97ba6a96f85f61c05c",
                Username = "scott",
                Password = "scott",
                Claims =
                {
                    new Claim("given_name", "Scott"),
                    new Claim("family_name", "Brady"),
                    new Claim("email", "scott@scottbrady91.com"),
                    new Claim("email_verified", "true"),
                    new Claim("website", "https://www.scottbrady91.com")
                }
            }
        };
    }
}