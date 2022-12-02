namespace TerraformProviderRegistry.Model
{
    public class TerraformProvider
    {
        public List<TerraformProviderVersion> Versions { get; set; } = new List<TerraformProviderVersion>();
    }
}
