using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApiRegisterUser_Demo.Models;

namespace WebApiRegisterUser_Demo.Services
{
    public class RegisterCodeService
    {
        private readonly IMongoCollection<RegisterCode> _registerCodeCollection;

        public RegisterCodeService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _registerCodeCollection = database.GetCollection<RegisterCode>(mongoDBSettings.Value.CollectionCodes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public RegisterCode GetByEmailCode(string email, string code)
        {
            var filterEmail = Builders<RegisterCode>.Filter.Eq("email", email);
            var filterCode = Builders<RegisterCode>.Filter.Eq("code", code);
            return _registerCodeCollection.Find(filterEmail & filterCode).FirstOrDefault();
        }

        /// <summary>
        /// active record
        /// </summary>
        /// <param name="id"></param>
        public void UpdateActive(string id)
        {
            FilterDefinition<RegisterCode> filter = Builders<RegisterCode>.Filter.Eq("id", id);
            UpdateDefinition<RegisterCode> update = Builders<RegisterCode>.Update.Set("active", 1);
            _registerCodeCollection.UpdateOne(filter, update);
        }

        public async Task CreateAsync(string email, string code)
        {
            // deactive 
            FilterDefinition<RegisterCode> filter = Builders<RegisterCode>.Filter.Eq("email", email);
            await _registerCodeCollection.DeleteOneAsync(filter);
            // create new record
            var temp = new RegisterCode()
            {
                email = email,
                code = code
            };
            await _registerCodeCollection.InsertOneAsync(temp);
            return;
        }
    }
}
