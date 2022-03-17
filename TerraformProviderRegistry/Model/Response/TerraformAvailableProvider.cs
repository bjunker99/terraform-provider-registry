namespace TerraformProviderRegistry.Model.Response
{
    public class TerraformAvailableProvider
    {

        public TerraformAvailableProvider()
        {
            versions = new List<TerraformAvailableVersion>();
        }

        public List<TerraformAvailableVersion>? versions { get; set; }

    }
}
