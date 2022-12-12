using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebApiRegisterUser_Demo.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public int age { get; set; } = 0;
        public string gender { get; set; } = null!;
        public string email { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string username { get; set; }
        public string password { get; set; }
        public string birthDate { get; set; } = null!;
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
