namespace TerraformProviderRegistry.Model.Response
{
    public class TerraformAvailableVersion
    {

        public string? version { get; set; }
        public List<string>? protocols { get; set; }
        public List<TerraformAvailablePlatform>? platforms { get; set; }

    }
}
