using System.Net;
using System.Net.Mail;

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

        public static bool SendMailRegisterCode(string code, string mailFrom, string mailFromPass, string mailTo)
        {
            MailAddress from = new MailAddress(mailFrom);
            MailAddress to = new MailAddress(mailTo);
            MailMessage message = new MailMessage(from, to);
            string htmlString = @"<html>
                      <body>
                      <p>Dear friend,</p>
                      <p>We received a new registration request. Please use this registration code to complete the authentication: <strong>{0}</strong></p>
                      <p>Thank you,</br></p>
                      </body>
                      </html>
                     ";
            message.IsBodyHtml = true;
            message.Body = string.Format(htmlString, code);

            SmtpClient client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential(mailFrom, mailFromPass),
                EnableSsl = true
            };
            try
            {
                client.Send(message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
    }
}
