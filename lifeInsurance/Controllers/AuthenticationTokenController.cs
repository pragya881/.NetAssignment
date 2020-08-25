using lifeInsurance.MiddilewareSettings;
using lifeInsurance.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace lifeInsurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationTokenController : ControllerBase
    {
        private IConfiguration _config;

        public AuthenticationTokenController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public string GetRandomToken()
        {
            var jwt = new JwtAuthenticationServices(_config);
            var token = jwt.GenerateSecurityToken("fake@email.com");
            return token;
        }
    }
}
