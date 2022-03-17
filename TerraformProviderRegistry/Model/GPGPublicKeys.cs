namespace TerraformProviderRegistry.Model
{
    public class GPGPublicKeys
    {

        public string? key_id { get; set; }
        public string? ascii_armor { get; set; }
        public string? trust_signature { get; set; }
        public string? source { get; set; }
        public string? source_url { get; set; }
    }
}
