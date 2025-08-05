using Microsoft.AspNetCore.Mvc;

namespace Jetwise.Services.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {  
        public HelloController()
        {}

        [HttpGet]
        public string Get()
        {
            return "This is my gateway!";
        }
    }
}
