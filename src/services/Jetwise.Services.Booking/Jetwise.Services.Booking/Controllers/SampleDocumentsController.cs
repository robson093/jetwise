using Jetwise.Services.Booking.Domain;
using Jetwise.Services.Booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jetwise.Services.Booking.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SampleDocumentsController : ControllerBase
    {
        private readonly ISampleDocumentsService documentsService;

        public SampleDocumentsController(ISampleDocumentsService documentsService)
        {
            this.documentsService = documentsService;
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetSampleDocuments")] 
        public async Task<IActionResult> Get()
        {
            return Ok((await documentsService.GetAllAsync()).Select(x => new SampleDocument()
            {
                Id = x.Id,
                Name = x.Name + "_v2"
            }));
        }

        [AllowAnonymous]
        [HttpPost(Name = "CreateSampleDocument")]
        public async Task<IActionResult> Create()
        {
            return Ok(await documentsService.CreateDocumentAsync(new Domain.SampleDocument
            {
                Id = Guid.NewGuid(),
                Name = "DocumentName"
            }));  
        }
         
        [HttpPost(Name = "GetAuthorizedUser")]
        [Route("user")]
        public async Task<IActionResult> GetUser()
        {
            Console.WriteLine( Request.Headers.Authorization.First().Split(' ').Last());
            Console.WriteLine(User.FindFirst("sub")?.Value);
            Console.WriteLine(User.FindFirst("sub")?.Value);
            Console.WriteLine(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            return Ok(Request.HttpContext.User.Claims.Select(x=>$"{x.Subject} {x.Value}"));
        }
    } 
}
