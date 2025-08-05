using Jetwise.Services.Booking.Controllers;
using Jetwise.Services.Booking.Domain;
using Jetwise.Services.Booking.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Jetwise.Services.Booking.Services
{
    public interface ISampleDocumentsService
    {
        Task<SampleDocument> CreateDocumentAsync(SampleDocument document);
        Task<List<SampleDocument>> GetAllAsync();
    }
    public class SampleDocumentsService : ISampleDocumentsService
    {
        private readonly IMongoCollection<SampleDocument> _sampleDocuments;
        public SampleDocumentsService(IOptions<MongoDbSettings> settings, IMongoClient client)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _sampleDocuments = database.GetCollection<SampleDocument>(settings.Value.CollectionName);
        }

        public async Task<List<SampleDocument>> GetAllAsync() =>
            await _sampleDocuments.Find(_ => true).ToListAsync();

        public async Task<SampleDocument> CreateDocumentAsync(SampleDocument document)
        {
            await _sampleDocuments.InsertOneAsync(document);
            return await (_sampleDocuments.Find(x => x.Id == document.Id)).FirstOrDefaultAsync();
        }
    } 
}
