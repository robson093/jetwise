using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jetwise.Services.Booking.Domain
{
    public class SampleDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = default!;

        [BsonElement("name")]
        public string Name { get; set; } = default!;
    }
}
