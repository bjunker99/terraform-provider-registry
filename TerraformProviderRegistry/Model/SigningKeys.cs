namespace TerraformProviderRegistry.Model
{
    public class SigningKeys
    {
        public List<GPGPublicKeys> GPGPublicKeys { get; set; } = new List<GPGPublicKeys>();
    }
}
