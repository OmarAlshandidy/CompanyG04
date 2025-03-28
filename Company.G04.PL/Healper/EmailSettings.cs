using System.Net.Mail;
using System.Net;

namespace Company.G04.PL.Healper
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("Omaralshandidy@gmail.com", "bkmydvkupbulwbjt");
            client.Send("Omaralshandidy@gmail.com", email.To, email.Subject, email.Body);

        }
    }
}
