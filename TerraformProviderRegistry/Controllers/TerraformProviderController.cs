using Microsoft.AspNetCore.Mvc;
using TerraformProviderRegistry.Model;

namespace TerraformProviderRegistry.Controllers
{

    [ApiController]
    [Route("/terraform/providers/v1/")]
    public class TerraformProviderController : ControllerBase
    {

        private readonly ILogger<TerraformProviderController> _logger;
        private readonly IServiceConfiguration _config;

        public TerraformProviderController(ILogger<TerraformProviderController> logger, IServiceConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet("{ns}/{name}/{version}/download/{os}/{arch}")]
        public async Task<IActionResult> ProviderPackage(string ns, string name, string version, string os, string arch)
        {
            var token = TerraformAuthenticationService.ValidateJWTToken(ControllerContext.HttpContext.Request.Headers.Authorization, _config.TOKEN_SECRET_KEY);

            if (_config.AUTHENTICATION_ENABLED && token == null)
                return Unauthorized();

            var tps = new TerraformProviderService(_config.TERRAFORM_PROVIDER_BUCKET);
            string? response = await tps.ProviderPackage(ns, name, version, os, arch);

            if (string.IsNullOrEmpty(response))
                return NotFound();

            return Ok(response);
        }

        [HttpGet("{ns}/{name}/versions")]
        public async Task<IActionResult> Versions(string ns, string name)
        {
            var token = TerraformAuthenticationService.ValidateJWTToken(ControllerContext.HttpContext.Request.Headers.Authorization, _config.TOKEN_SECRET_KEY);

            if (_config.AUTHENTICATION_ENABLED && token == null)
                return Unauthorized();

            var tps = new TerraformProviderService(_config.TERRAFORM_PROVIDER_BUCKET);
            string? response = await tps.Versions(ns, name);

            if (string.IsNullOrEmpty(response))
                return NotFound();

            return Ok(response);
        }

    }
}
