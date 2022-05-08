using MailKit.Net.Smtp;
using MimeKit;
using Project2.Models;

namespace Project2
{
    public class Email
    {

        public static string host = "smtp.seznam.cz";
        public static int smtpport = 465;
        public static string username = "motorbike.servis@email.cz";
        public static string password = "abc1234";

        public static async Task RegisterEmail(RegisterForm user)
        {
            using MimeMessage msg = new MimeMessage();

            msg.From.Add(new MailboxAddress("no-reply", "motorbike.servis@email.cz"));

            msg.To.Add(new MailboxAddress($"{user.FirstName} {user.LastName}", user.Email));

            msg.Subject = "Informace o Registraci";

            BodyBuilder builder = new BodyBuilder();

            builder.HtmlBody = File.ReadAllText("Assets/register_email_template.html");

            builder.HtmlBody = builder.HtmlBody.Replace("__NAME__", $"{user.FirstName} {user.LastName}");

            msg.Body = builder.ToMessageBody();

            using SmtpClient client = new SmtpClient();

            await client.ConnectAsync(host, smtpport, true);
            await client.AuthenticateAsync(username, password);

            await client.SendAsync(msg);

            await client.DisconnectAsync(true);
        }
    }
}
