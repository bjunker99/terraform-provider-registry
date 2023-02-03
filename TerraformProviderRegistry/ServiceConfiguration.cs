using System.Collections;
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
            foreach (DictionaryEntry envVar in Environment.GetEnvironmentVariables())
            {
                string? key = envVar.Key as string;

                if (envVar.Value is not string value)
                    continue;

                string envVal = value;

                switch (key)
                {
                    case "TERRAFORM_PROVIDER_BUCKET":
                        TERRAFORM_PROVIDER_BUCKET = envVal;
                        break;
                    case "TERRAFORM_PROVIDER_BUCKET_REGION":
                        TERRAFORM_PROVIDER_BUCKET_REGION = envVal;
                        break;
                    case "OAUTH_CLIENT_ID":
                        GITHUB_OAUTH_CLIENT_ID = envVal;
                        break;
                    case "OAUTH_CLIENT_SECRET":
                        GITHUB_OAUTH_CLIENT_SECRET = envVal;
                        break;
                    case "GITHUB_BASE_URL":
                        GITHUB_BASE_URL = envVal;
                        break;
                    case "TOKEN_SECRET_KEY":
                        TOKEN_SECRET_KEY = envVal;
                        break;
                    default:
                        break;
                }
            }

            if (bool.TryParse(Environment.GetEnvironmentVariable("AUTHENTICATION_ENABLED"), out bool auth))
            {
                AUTHENTICATION_ENABLED = auth;
            }
        }

        public string TERRAFORM_PROVIDER_BUCKET { get; set; } = string.Empty;
        public string TERRAFORM_PROVIDER_BUCKET_REGION { get; set; } = string.Empty;
        public string GITHUB_OAUTH_CLIENT_ID { get; set; } = string.Empty;
        public string GITHUB_OAUTH_CLIENT_SECRET { get; set; } = string.Empty;
        public string GITHUB_BASE_URL { get; set; } = string.Empty;
        public string TOKEN_SECRET_KEY { get; set; } = string.Empty;
        public bool AUTHENTICATION_ENABLED { get; set; }
    }
}
