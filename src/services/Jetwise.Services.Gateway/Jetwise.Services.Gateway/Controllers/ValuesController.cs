using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jetwise.Services.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("testy")]
        public IActionResult Get()
        {
            return Ok("works!");
        }

        [Authorize]
        [HttpGet("testyauth")]
        public IActionResult Get2()
        { 
            return Ok("works auth!");
        }

    }
}
