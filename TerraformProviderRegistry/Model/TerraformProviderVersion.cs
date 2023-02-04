using System.Text.Json.Serialization;

namespace TerraformProviderRegistry.Model
{
    public class TerraformProviderVersion
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = "";
        [JsonPropertyName("protocols")]
        public List<string> Protocols { get; set; } = new List<string>();
        [JsonPropertyName("platforms")]
        public List<TerraformProviderPlatform> Platforms { get; set; } = new List<TerraformProviderPlatform>();
    }
}
