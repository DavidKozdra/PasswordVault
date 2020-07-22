using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PasswordValut
{
    public class EmailSystem
    {

        public string Message;
        public static User user;
        public string subject = "Code for";
        public EmailSystem(User U, string M)
        {
            Message = M;
            user = U;
        }


        public void Email()
        {

            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            if (user.Email != string.Empty) {
                message.From = new MailAddress("davidkozdra@gmail.com");
                message.To.Add(new MailAddress(user.Email));
                message.Subject = subject + user.Name;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = Message;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("magentaautumn@gmail.com", "142862Dk!!");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
        }
        }
    }

