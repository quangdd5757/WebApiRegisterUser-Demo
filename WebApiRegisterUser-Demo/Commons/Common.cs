namespace WebApiRegisterUser_Demo.Commons
{
    public class Common
    {
        /// <summary>
        /// random string code with length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomCode(int length)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var rand = new Random();
            var code = "";
            for (int i = 0; i < length; i++)
            {
                int index = rand.Next(alphabet.Length);
                code = code + alphabet[index];
            }
            return code;
        }

        public static bool SendMailRegisterCode(string code)
        {
            return true;
        }
    }
}
