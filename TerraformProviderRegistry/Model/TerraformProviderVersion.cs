namespace TerraformProviderRegistry.Model
{
    public class TerraformProviderVersion
    {

        public string? version { get; set; }
        public List<string>? protocols { get; set; }
        public List<TerraformProviderPlatform>? platforms { get; set; }


    }
}
