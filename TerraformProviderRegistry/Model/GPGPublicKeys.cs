namespace TerraformProviderRegistry.Model
{
    public class GPGPublicKeys
    {
        public string KeyId { get; set; } = "";
        public string AsciiArmor { get; set; } = "";
        public string TrustSignature { get; set; } = "";
        public string Source { get; set; } = "";
        public string SourceUrl { get; set; } = "";
    }
}
