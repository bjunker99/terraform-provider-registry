namespace TerraformProviderRegistry.Model.Response
{
    public class TerraformAvailableProvider
    {
        public List<TerraformAvailableVersion> versions { get; set; } = new List<TerraformAvailableVersion>();
    }
}
