using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotesAPI.Controllers
{
    [Route("/")]
    [ApiController]
    [AllowAnonymous]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public OkObjectResult Get() {
            return Ok("Hello");
        }

    }
}
