using System.Text.Json.Serialization;

namespace TerraformProviderRegistry.Model
{
    public class SigningKeys
    {
        [JsonPropertyName("gpg_public_keys")]
        public List<GPGPublicKeys> GPGPublicKeys { get; set; } = new List<GPGPublicKeys>();
    }
}
