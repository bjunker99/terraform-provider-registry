using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Octokit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TerraformProviderRegistry.Model;

namespace TerraformProviderRegistry
{
    public class TerraformAuthenticationService
    {

        private readonly string _clientId = string.Empty;
        private readonly string _clientSecret = string.Empty;

        public TerraformAuthenticationService(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public TerraformAuthenticationService(string clientId)
        {
            _clientId = clientId;
        }

        public Uri Authorize(string state, Uri? baseAddress = null)
        {
            GitHubClient client;

            if (baseAddress == null)
            {
                client = new GitHubClient(new ProductHeaderValue("terraform-provider-registry"));
            }
            else
            {
                client = new GitHubClient(new ProductHeaderValue("terraform-provider-registry"), baseAddress);
            }

            var request = new OauthLoginRequest(_clientId)
            {
                State = state,
            };

            return client.Oauth.GetGitHubLoginUrl(request);
        }

        public Uri GenerateTokenRedirect(string state, string code)
        {
            Uri uri = new($"http://localhost:18523/login?code={code}&state={state}");

            return uri;
        }

        public async Task<OauthToken?> GenerateOauthToken(string code, Uri? baseAddress = null)
        {
            OauthToken? token = null;

            try
            {
                GitHubClient client;

                if (baseAddress == null)
                {
                    client = new GitHubClient(new ProductHeaderValue("terraform-provider-registry"));
                }
                else
                {
                    client = new GitHubClient(new ProductHeaderValue("terraform-provider-registry"), baseAddress);
                }

                var request = new OauthTokenRequest(_clientId, _clientSecret, code);
                token = await client.Oauth.CreateAccessToken(request);
            }
            catch (Exception)
            {

            }

            return token;
        }

        public static async Task<ReturnToken?> GenerateJWTToken(OauthToken? token, string key)
        {
            ReturnToken? returnToken = null;

            var client = new GitHubClient(new ProductHeaderValue("terraform-provider-registry"))
            {
                Credentials = new Credentials(token?.AccessToken, AuthenticationType.Bearer)
            };

            var user = await client.User.Current();

            if (!string.IsNullOrEmpty(key))
            {
                byte[] key_data = System.Text.Encoding.UTF8.GetBytes(key);

                SymmetricSecurityKey ssk = new(key_data);
                var handler = new JwtSecurityTokenHandler();

                var now = DateTime.UtcNow;

                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, "terraform-cli"),
                    new Claim("login", user.Login)
                };

                var jwtToken = new JwtSecurityToken
                (
                    "terraform-cli",
                    "https://github.com/login/oauth/authorize",
                    claims,
                    now.AddMilliseconds(-30),
                    now.AddDays(30),
                    new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256Signature)
                );

                returnToken = new ReturnToken
                {
                    AccessToken = handler.WriteToken(jwtToken)
                };
            }

            return returnToken;
        }

        public static JwtSecurityToken? ValidateJWTToken(StringValues authHeader, string key)
        {
            if (authHeader.Count != 1)
                throw new Exception("No authorization header.");

            string[] parts = authHeader[0].Split(' ');

            if (parts.Length != 2)
                throw new Exception("Invalid authorization format.");

            JwtSecurityToken? jwtToken = null;
            var handler = new JwtSecurityTokenHandler();

            if (!string.IsNullOrEmpty(key))
            {
                byte[] key_data = System.Text.Encoding.UTF8.GetBytes(key);

                SymmetricSecurityKey ssk = new(key_data);

                try
                {
                    handler.ValidateToken(parts[1], new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = ssk,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    jwtToken = (JwtSecurityToken)validatedToken;
                }
                catch (Exception)
                { }
            }

            return jwtToken;
        }
    }
}
