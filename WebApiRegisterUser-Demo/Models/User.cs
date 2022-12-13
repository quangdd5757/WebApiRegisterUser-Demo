using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebApiRegisterUser_Demo.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement]
        [BsonIgnoreIfNull]
        public string firstName { get; set; } = null!;

        [BsonElement]
        [BsonIgnoreIfNull]
        public string lastName { get; set; } = null!;

        [BsonElement]
        [BsonIgnoreIfNull]
        public int age { get; set; } = 0;

        [BsonElement]
        [BsonIgnoreIfNull]
        public string gender { get; set; } = null!;

        [BsonElement]
        [BsonIgnoreIfNull]
        public string email { get; set; }

        [BsonElement]
        [BsonIgnoreIfNull]
        public string phone { get; set; } = null!;

        [BsonElement]
        [BsonIgnoreIfNull]
        public string username { get; set; } = null!;

        [BsonElement]
        [BsonIgnoreIfNull]
        public string password { get; set; }

        [BsonElement]
        [BsonIgnoreIfNull]
        public string birthDate { get; set; } = null!;

        [BsonElement]
        [BsonIgnoreIfNull]
        public int active { get; set; } = 0;

        public User(UserModel user)
        {
            firstName = user.firstName;
            lastName = user.lastName;
            age = user.age;
            gender = user.gender;
            email = user.email;
            phone = user.phone;
            username = user.username;
            password = user.password;
            birthDate = user.birthDate;
        }
    }
}
