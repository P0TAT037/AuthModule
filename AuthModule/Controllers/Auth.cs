using Microsoft.AspNetCore.Mvc;

namespace AuthModule.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class Auth : ControllerBase
    {
        private readonly ILogger<Auth> _logger;

        public Auth(ILogger<Auth> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task Get()
        {


        }
        
    }
}
