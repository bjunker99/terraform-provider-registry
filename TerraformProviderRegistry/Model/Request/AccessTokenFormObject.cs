namespace TerraformProviderRegistry.Model.Request
{
    public class AccessTokenFormObject
    {

        public string? client_id { get; set; }
        public string? code { get; set; }
        public string? code_verifier { get; set; }
        public string? grant_type { get; set; }
        public string? redirect_uri { get; set; }

    }
}
