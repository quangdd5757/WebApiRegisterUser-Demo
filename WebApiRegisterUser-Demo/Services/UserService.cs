using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;
using WebApiRegisterUser_Demo.Commons;
using WebApiRegisterUser_Demo.Models;

namespace WebApiRegisterUser_Demo.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<User>(mongoDBSettings.Value.CollectionUsers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq("email", email);
            return _usersCollection.Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public bool CreateUsers(List<User> users)
        {
            _usersCollection.InsertMany(users);
            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public bool CreateUser(User user)
        {
            _usersCollection.InsertOne(user);
            return true;
        }

        public string? RegisterUser(User user)
        {
            // generate code
            string code = Common.RandomCode(10);

            return null;
        }

        public async Task ValidateRegisterUser(string email)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq("email", email);
            UpdateDefinition<User> update = Builders<User>.Update.Set("active", 1);
            await _usersCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task DeleteAsync(string id)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq("id", id);
            await _usersCollection.DeleteOneAsync(filter);
            return;
        }
    }
}
