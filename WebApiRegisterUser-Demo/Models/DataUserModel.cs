namespace WebApiRegisterUser_Demo.Models
{
    public class DataUserModel
    {
        public UserModel[] users { get; set; }
    }

    public class UserModel
    {
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public int age { get; set; } = 0;
        public string gender { get; set; } = null!;
        public string email { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string username { get; set; }
        public string password { get; set; }
        public string birthDate { get; set; } = null!;
    }
}