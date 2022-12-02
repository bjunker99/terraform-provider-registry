namespace TerraformProviderRegistry.Model.Response
{
    public class TerraformAvailableVersion
    {
        public string Version { get; set; } = "";
        public List<string> Protocols { get; set; } = new List<string>();
        public List<TerraformAvailablePlatform> Platforms { get; set; } = new List<TerraformAvailablePlatform>();
    }
}
