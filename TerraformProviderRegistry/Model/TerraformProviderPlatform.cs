namespace TerraformProviderRegistry.Model
{
    public class TerraformProviderPlatform
    {

        public string? os { get; set; }
        public string? arch { get; set; }
        public string? filename { get; set; }
        public string? download_url { get; set; }
        public string? shasums_url { get; set; }
        public string? shasums_signature_url { get; set; }
        public string? shasum { get; set; }
        public SigningKeys? signing_keys { get; set; }
    }
}
