using System.Text.Json.Serialization;

namespace TerraformProviderRegistry.Model
{
    public class GPGPublicKeys
    {
        [JsonPropertyName("key_id")]
        public string KeyId { get; set; } = "";
        [JsonPropertyName("ascii_armor")]
        public string AsciiArmor { get; set; } = "";
        [JsonPropertyName("trust_signature")]
        public string TrustSignature { get; set; } = "";
        [JsonPropertyName("source")]
        public string Source { get; set; } = "";
        [JsonPropertyName("source_url")]
        public string SourceUrl { get; set; } = "";
    }
}
