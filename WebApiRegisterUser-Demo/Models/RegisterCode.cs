using MongoDB.Bson.Serialization.Attributes;

namespace WebApiRegisterUser_Demo.Models
{
    public class RegisterCode
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }
        public string email { get; set; }
        public string code { get; set; }
    }
}
