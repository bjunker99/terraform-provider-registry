namespace TerraformProviderRegistry.Model
{
    public class TerraformProviderPlatform
    {
        public string OS { get; set; } = "";
        public string Arch { get; set; } = "";
        public string Filename { get; set; } = "";
        public string DownloadUrl { get; set; } = "";
        public string ShasumsUrl { get; set; } = "";
        public string ShasumsSignatureUrl { get; set; } = "";
        public string Shasum { get; set; } = "";
        public SigningKeys SigningKeys { get; set; } = new SigningKeys();
    }
}
