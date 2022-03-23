using TerraformProviderRegistry.Model;

namespace TerraformProviderRegistry
{
    internal class ServiceConfiguration : IServiceConfiguration
    {
        public ServiceConfiguration()
        {
            ReadConfiguration();
        }

        public void ReadConfiguration()
        {
            TERRAFORM_PROVIDER_BUCKET = Environment.GetEnvironmentVariable("TERRAFORM_PROVIDER_BUCKET");
            TERRAFORM_PROVIDER_BUCKET_REGION = Environment.GetEnvironmentVariable("TERRAFORM_PROVIDER_BUCKET_REGION");
            GITHUB_OAUTH_CLIENT_ID = Environment.GetEnvironmentVariable("OAUTH_CLIENT_ID");
            GITHUB_OAUTH_CLIENT_SECRET = Environment.GetEnvironmentVariable("OAUTH_CLIENT_SECRET");
            GITHUB_BASE_URL = Environment.GetEnvironmentVariable("GITHUB_BASE_URL");
            TOKEN_SECRET_KEY = Environment.GetEnvironmentVariable("TOKEN_SECRET_KEY");

            if (bool.TryParse(Environment.GetEnvironmentVariable("TOKEN_SECRET_KEY"), out bool auth))
            {
                AUTHENTICATION_ENABLED = auth;
            }
        }

        public string? TERRAFORM_PROVIDER_BUCKET { get; set; } = string.Empty;
        public string? TERRAFORM_PROVIDER_BUCKET_REGION { get; set; } = string.Empty;
        public string? GITHUB_OAUTH_CLIENT_ID { get; set; } = string.Empty;
        public string? GITHUB_OAUTH_CLIENT_SECRET { get; set; } = string.Empty;
        public string? GITHUB_BASE_URL { get; set; } = string.Empty;
        public string? TOKEN_SECRET_KEY { get; set; } = string.Empty;
        public bool AUTHENTICATION_ENABLED { get; set; }
    }
}
