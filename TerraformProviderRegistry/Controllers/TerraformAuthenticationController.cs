using Microsoft.AspNetCore.Mvc;
using TerraformProviderRegistry.Model;
using TerraformProviderRegistry.Model.Request;

namespace TerraformProviderRegistry.Controllers
{

    [ApiController]
    [Route("/oauth")]
    public class TerraformAuthenticationController : ControllerBase
    {

        private readonly ILogger<TerraformProviderController> _logger;
        private readonly IServiceConfiguration _config;

        public TerraformAuthenticationController(ILogger<TerraformProviderController> logger, IServiceConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet("authorization")]
        public IActionResult Authorization([FromQuery] AuthorizationQueryObject request)
        {
            var tas = new TerraformAuthenticationService(_config.GITHUB_OAUTH_CLIENT_ID);
            Uri uri = tas.Authorize(request.state);

            return Redirect(uri.ToString());
        }

        [HttpGet("callback")]
        public IActionResult Callback([FromQuery] CallbackQueryObject request)
        {
            var tas = new TerraformAuthenticationService(_config.GITHUB_OAUTH_CLIENT_ID);
            Uri uri = tas.GenerateTokenRedirect(request.state, request.code);

            return Redirect(uri.ToString());
        }

        [HttpPost("access_token")]
        public async Task<IActionResult> AccessToken([FromForm] AccessTokenFormObject request)
        {
            var tas = new TerraformAuthenticationService(_config.GITHUB_OAUTH_CLIENT_ID, _config.GITHUB_OAUTH_CLIENT_SECRET);
            
            var oauthToken = await tas.GenerateOauthToken(request.code);
            var response = await TerraformAuthenticationService.GenerateJWTToken(oauthToken, _config.TOKEN_SECRET_KEY);

            return Ok(response);
        }

    }
}
