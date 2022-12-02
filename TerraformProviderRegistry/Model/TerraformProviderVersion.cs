namespace TerraformProviderRegistry.Model
{
    public class TerraformProviderVersion
    {
        public string Version { get; set; } = "";
        public List<string> Protocols { get; set; } = new List<string>();
        public List<TerraformProviderPlatform> Platforms { get; set; } = new List<TerraformProviderPlatform>();
    }
}
