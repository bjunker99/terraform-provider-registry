using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TerraformProviderRegistry.Controllers
{

    [ApiController]
    [Route("/")]
    public class TerraformServiceController : ControllerBase
    {

        private readonly ILogger<TerraformServiceController> _logger;

        public TerraformServiceController(ILogger<TerraformServiceController> logger)
        {
            _logger = logger;
        }

        [HttpGet(".well-known/terraform.json")]
        public IActionResult ServiceDiscovery()
        {
            string discovery = "{\"providers.v1\": \"/terraform/providers/v1/\"}";

            _logger.LogInformation(".well-known/terraform.json");

            var assembly = typeof(TerraformServiceController).Assembly;
            Stream? resource = assembly?.GetManifestResourceStream("TerraformProviderRegistry.serviceDiscovery.json");

            if (resource != null)
            {
                using StreamReader reader = new(resource);
                discovery = reader.ReadToEnd();
            }
            else
            {
                return NoContent();
            }

            JsonDocument doc = JsonDocument.Parse(discovery);

            return Ok(doc);
        }

    }
}
