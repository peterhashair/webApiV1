using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace webApiV1.Models
{
    public class User
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Email")]
        [Required]
        public string Email { get; set; }

        [BsonElement("FirstName")]
        [Required]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        [Required]
        public string LastName { get; set; }

        [BsonElement("Password")]
        [Required]
        public string Password { get; set; }

        [BsonElement("Phone")]
        [Required]
        public string Phone { get; set; }

        [BsonElement("IsAcitve")]
        [BsonDefaultValue(false)]
        public bool IsAcitve { get; set; }
    }
}
