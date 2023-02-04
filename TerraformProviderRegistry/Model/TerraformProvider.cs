using System.Text.Json.Serialization;

namespace TerraformProviderRegistry.Model
{
    public class TerraformProvider
    {
        [JsonPropertyName("versions")]
        public List<TerraformProviderVersion> Versions { get; set; } = new List<TerraformProviderVersion>();
    }
}
