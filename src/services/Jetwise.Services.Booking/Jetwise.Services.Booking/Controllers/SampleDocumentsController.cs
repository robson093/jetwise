using Jetwise.Services.Booking.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jetwise.Services.Booking.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleDocumentsController : ControllerBase
    {
        private readonly ISampleDocumentsService documentsService;

        public SampleDocumentsController(ISampleDocumentsService documentsService)
        {
            this.documentsService = documentsService;
        }

        [HttpGet(Name = "GetSampleDocuments")]
        public async Task<IActionResult> Get()
        {
            return Ok(await documentsService.GetAllAsync());
        }

        [HttpPost(Name = "CreateSampleDocument")]
        public async Task<IActionResult> Create()
        {
            return Ok(await documentsService.CreateDocumentAsync(new Domain.SampleDocument
            {
                Id = Guid.NewGuid(),
                Name = "DocumentName"
            }));
        }

    } 
}
