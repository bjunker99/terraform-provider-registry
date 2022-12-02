namespace TerraformProviderRegistry.Model.Request
{
    public class AuthorizationQueryObject
    {
        public string ClientId { get; set; } = "";
        public string CodeChallenge { get; set; } = "";
        public string CodeChallengeMethod { get; set; } = "";
        public string RedirectUri { get; set; } = "";
        public string ResponseType { get; set; } = "";
        public string State { get; set; } = "";

    }
}
