using System;
using System.ComponentModel.DataAnnotations;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace webApiV1.Models.Identity
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
	{


        [BsonElement("FirstName")]
        [Required]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        [Required]
        public string LastName { get; set; }


        [BsonElement("Phone")]
        [Required]
        public string Phone { get; set; }

        [BsonElement("IsAcitve")]
        [BsonDefaultValue(false)]
        public bool IsAcitve { get; set; }
        public ApplicationUser() : base()
		{
		}

		public ApplicationUser(string userName, string email) : base(userName, email)
		{
		}
	}
}
