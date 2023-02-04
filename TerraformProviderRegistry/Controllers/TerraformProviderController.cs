using Microsoft.AspNetCore.Mvc;
using TerraformProviderRegistry.Model;

namespace TerraformProviderRegistry.Controllers
{

    [ApiController]
    [Route("/terraform/providers/v1/")]
    public class TerraformProviderController : ControllerBase
    {

        private readonly IServiceConfiguration _config;
        private readonly ILogger<TerraformProviderController> _logger;

        public TerraformProviderController(ILogger<TerraformProviderController> logger, IServiceConfiguration config)
        {
            _config = config;
            _logger = logger;
        }

        [HttpGet("{ns}/{name}/{version}/download/{os}/{arch}")]
        public async Task<IActionResult> ProviderPackage(string ns, string name, string version, string os, string arch)
        {
            if (_config.AUTHENTICATION_ENABLED)
            {
                try
                {
                    var token = TerraformAuthenticationService.ValidateJWTToken(ControllerContext.HttpContext.Request.Headers.Authorization, _config.TOKEN_SECRET_KEY);

                    if (token == null)
                        return Unauthorized();
                }
                catch (Exception)
                {
                    return Unauthorized();
                }
            }

            var tps = new TerraformProviderService(_config.TERRAFORM_PROVIDER_BUCKET, _config.TERRAFORM_PROVIDER_BUCKET_REGION);
            string response = await tps.ProviderPackage(ns, name, version, os, arch);

            if (string.IsNullOrEmpty(response))
                return NotFound();

            return Ok(response);
        }

        [HttpGet("{ns}/{name}/versions")]
        public async Task<IActionResult> Versions(string ns, string name)
        {
            if (_config.AUTHENTICATION_ENABLED)
            {
                try
                {
                    var token = TerraformAuthenticationService.ValidateJWTToken(ControllerContext.HttpContext.Request.Headers.Authorization, _config.TOKEN_SECRET_KEY);

                    if (token == null)
                        return Unauthorized();
                }
                catch (Exception)
                {
                    return Unauthorized();
                }
            }

            string response = string.Empty;

            try
            {
                var tps = new TerraformProviderService(_config.TERRAFORM_PROVIDER_BUCKET, _config.TERRAFORM_PROVIDER_BUCKET_REGION);
                response = await tps.Versions(ns, name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            if (string.IsNullOrEmpty(response))
                return NotFound();

            return Ok(response);
        }

    }
}
