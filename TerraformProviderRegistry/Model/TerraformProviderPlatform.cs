using System.Text.Json.Serialization;

namespace TerraformProviderRegistry.Model
{
    public class TerraformProviderPlatform
    {
        [JsonPropertyName("os")]
        public string OS { get; set; } = "";
        [JsonPropertyName("arch")]
        public string Arch { get; set; } = "";
        [JsonPropertyName("filename")]
        public string Filename { get; set; } = "";
        [JsonPropertyName("download_url")]
        public string DownloadUrl { get; set; } = "";
        [JsonPropertyName("shasums_url")]
        public string ShasumsUrl { get; set; } = "";
        [JsonPropertyName("shasums_signature_url")]
        public string ShasumsSignatureUrl { get; set; } = "";
        [JsonPropertyName("shasum")]
        public string Shasum { get; set; } = "";

        [JsonPropertyName("signing_keys")]
        public SigningKeys SigningKeys { get; set; } = new SigningKeys();
    }
}
