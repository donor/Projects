using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Security.Policy;

namespace Inbid.Helpers
{
    public class MailHelper
    {
        public static void SendRegistrationAdminMail(string to, string userName, string password, Guid userId, string companyName, string absolutePath)
        {
            StringBuilder emailMessage = new StringBuilder();
            emailMessage.Append("Firma "+companyName+" zostala zarejestrowana w Inbid");
            emailMessage.Append("<br />");
            emailMessage.Append("Login i hasło administratora Firmy:  <br />");
            emailMessage.Append("Login: " + userName + " <br />");
            emailMessage.Append("Hasło: " + password + " <br />");
            emailMessage.Append("Nacisnij ponizszy link w celu aktywacji konta Administratora  <br />");
            
            emailMessage.Append(string.Format("<a href='"+absolutePath+"'> Aktywuj konto Administratora</a>"));
            MailMessage email = new MailMessage();
//            email.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Sender"]);
            email.From = new MailAddress("inbidmailer@gmail.com");

            email.To.Add(new MailAddress(to));
            email.Subject = "Aktywuj konto Administratora Firmy "+companyName+" w Inbid";
            email.Body = emailMessage.ToString();
            email.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
                client.Send(email);
        }

        public static void SendRegistrationBidderOrBidderViewMail(bool bidder,string to, string userName, string password, Guid userId, string companyName, string absolutePath)
        {
            StringBuilder emailMessage = new StringBuilder();
            emailMessage.Append("Administrator firmy " + companyName + " zarejestrował Ciebie w Inbid");
            emailMessage.Append("<br />");
                        
            if (bidder)
                emailMessage.Append("Możesz uczestniczyć w aukcjach</br>");
            else
                emailMessage.Append("Możesz obserwować przebjeg aukcji. </br>");

            emailMessage.Append("Login i hasło administratora Firmy:  <br />");
            emailMessage.Append("Login: " + userName + " <br />");
            emailMessage.Append("Hasło: " + password + " <br />");
            emailMessage.Append("Nacisnij ponizszy link w celu aktywacji swojego konta <br />");

            emailMessage.Append(string.Format("<a href='" + absolutePath + "'> Aktywuj konto </a>"));
            MailMessage email = new MailMessage();
            //            email.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Sender"]);
            email.From = new MailAddress("inbidmailer@gmail.com");

            email.To.Add(new MailAddress(to));
            email.Subject = "Aktywuj konto w Inbid";
            email.Body = emailMessage.ToString();
            email.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Send(email);
        }
    }
}