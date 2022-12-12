namespace WebApiRegisterUser_Demo.Models
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionUsers { get; set; } = null!;
        public string CollectionCodes { get; set; } = null!;
    }
}
