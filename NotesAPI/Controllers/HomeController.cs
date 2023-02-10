using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;
using System.Security.Claims;

namespace NotesAPI.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        

        [HttpGet]
        public OkObjectResult Get() {
            return Ok("Hello");
        }

        [HttpGet("token")]
        [Authorize]
        
        public IActionResult GetwithToken()
        {
            return Ok("Hello with Token: "+HttpContext.Request.Headers.Authorization.ToString() ?? string.Empty);
        }

    }
}
