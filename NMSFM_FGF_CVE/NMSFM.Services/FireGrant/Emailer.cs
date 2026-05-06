using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace NMSFM.Services.FireGrant
{
    public class Emailer
    {
        /// <summary>
        ///     ''' Sends an mail message
        ///     ''' </summary>
        ///     ''' <param name="from">Sender address</param>
        ///     ''' <param name="recepient">Recepient address</param>
        ///     ''' <param name="bcc">Bcc recepient</param>
        ///     ''' <param name="cc">Cc recepient</param>
        ///     ''' <param name="subject">Subject of mail message</param>
        ///     ''' <param name="body">Body of mail message</param>
        ///     ''' <param name="Att">Is there an attachment</param>
        public void SendMailMessage(string from, string recepient, string bcc, string cc, string subject, string body, string Att = "")
        {
            // Instantiate a new instance of MailMessage
            MailMessage mMailMessage = new MailMessage();

            // Set the sender address of the mail message
            mMailMessage.From = new MailAddress(from);
            // Set the recepient address of the mail message
            //mMailMessage.To.Add(new MailAddress(recepient));
            if (recepient.Contains(";"))
            {
                char separator = ';';
                string[] recipientlist = recepient.Split(separator);
                for (int i = 0; i < recipientlist.Length; i++)
                {
                    MailAddress mmaddress = new MailAddress(recipientlist[i].ToString());
                    mMailMessage.To.Add(mmaddress);
                }
            }
            else
            {
                mMailMessage.To.Add(new MailAddress(recepient));
            }

            if (Att != "")
                // mMailMessage.Attachments.Add(New Attachment("C:\Inetpub\vhosts\hart-clayton.com\httpdocs\App_Data\Image.jpg"))
                mMailMessage.Attachments.Add(new Attachment(Att));

            // Check if the bcc value is nothing or an empty string
            if (bcc != null & bcc != string.Empty)
                // Set the Bcc address of the mail message
                mMailMessage.Bcc.Add(new MailAddress(bcc));

            // Check if the cc value is nothing or an empty value
            if (cc != null & cc != string.Empty)
                // Set the CC address of the mail message
                mMailMessage.CC.Add(new MailAddress(cc));

            // Set the subject of the mail message
            mMailMessage.Subject = subject;
            // Set the body of the mail message
            mMailMessage.Body = body;

            // Set the format of the mail message body as HTML
            mMailMessage.IsBodyHtml = true;
            // Set the priority of the mail message to normal
            mMailMessage.Priority = MailPriority.Normal;

            // Instantiate a new instance of SmtpClient
            SmtpClient mSmtpClient = new SmtpClient();

            //mSmtpClient.Host = "smtp.office365.com";
            //mSmtpClient.Port = 587;
            //mSmtpClient.EnableSsl = true;
            //mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //mSmtpClient.UseDefaultCredentials = false;
            //mSmtpClient.Credentials = new System.Net.NetworkCredential("", "");

            // Send the mail message
            bool emailsent = false;
            string exception = "";
            for (int i = 1; i < 6; i++)
            {
                try
                {
                    mSmtpClient.Send(mMailMessage);
                    emailsent = true;
                    break;
                }
                catch (Exception ex)
            {
                _ = ex;
                    emailsent = false;
                    exception = ex.Message;
                }
            }
            if (emailsent == false)
            {
                throw new Exception("The system was unable to send the email. If this problem persists please contact the system administrator. Error: " + exception);
            }
        }

        public bool EmailIsValid(string email)
        {
            string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (Regex.IsMatch(email, expression))
            {
                if (Regex.Replace(email, expression, string.Empty).Length == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

