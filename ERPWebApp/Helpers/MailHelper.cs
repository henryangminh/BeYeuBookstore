using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using BeYeuBookstore.Utilities.Constants;

namespace Helper
{
	public class MailHelper
	{
		public void SendMail(string toEmailAddress, string subject, string content)
		{
			var fromEmailAddress = Const_HostEmail.Email;
            var fromEmailDisplayName = Const_HostEmail.EmailDisplayName;
			var fromEmailPassword = Const_HostEmail.Pass;
            var smtpHost = Const_HostEmail.Host;
            var smtpPort = Const_HostEmail.Port;

            bool enabledSsl = true;


			string body = content;
			MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
			message.Subject = subject;
			message.IsBodyHtml = true;
			message.Body = body;

			var client = new SmtpClient();
			client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
			client.Host = smtpHost;
			client.EnableSsl = enabledSsl;
			client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
			client.Send(message);

		}
	}
}
