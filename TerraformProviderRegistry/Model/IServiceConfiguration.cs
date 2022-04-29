namespace TerraformProviderRegistry.Model
{
    public interface IServiceConfiguration
    {

        string? TERRAFORM_PROVIDER_BUCKET { get; set; }
        string? TERRAFORM_PROVIDER_BUCKET_REGION { get; set; }
        string? GITHUB_OAUTH_CLIENT_ID { get; set; }
        string? GITHUB_OAUTH_CLIENT_SECRET { get; set; }
        string? GITHUB_BASE_URL { get; set; }
        string? TOKEN_SECRET_KEY { get; set; }
        bool AUTHENTICATION_ENABLED { get; set; }

    }
}
