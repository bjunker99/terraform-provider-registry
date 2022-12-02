namespace TerraformProviderRegistry.Model.Request
{
    public class CallbackQueryObject
    {
        public string Code { get; set; } = ""; 
        public string? state { get; set; } = "";
    }
}
