using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TerraformProviderRegistry.Controllers
{

    [ApiController]
    [Route("/")]
    public class TerraformServiceController : ControllerBase
    {
        public TerraformServiceController()
        {
        }

        [HttpGet(".well-known/terraform.json")]
        public IActionResult ServiceDiscovery()
        {
            string discovery = "{\"providers.v1\": \"/terraform/providers/v1/\"}";

            var assembly = typeof(TerraformServiceController).Assembly;
            Stream? resource = assembly?.GetManifestResourceStream("TerraformProviderRegistry.serviceDiscovery.json");

            if (resource != null)
            {
                using StreamReader reader = new(resource);
                discovery = reader.ReadToEnd();
            }

            JsonDocument doc = JsonDocument.Parse(discovery);

            return Ok(doc);
        }

    }
}
