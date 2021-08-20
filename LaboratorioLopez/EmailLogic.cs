using LaboratorioLopez.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioLopez
{
    public class EmailLogic
    {
        //public static void SendEmail(string from, List<string> to, string subject, string body) { }
        private string _host;
        private string _from;
        private string _alias;
        public EmailLogic(IConfiguration iConfiguration)
        {
            var smtpSection = iConfiguration.GetSection("SMTP");
            if (smtpSection != null)
            {
                _host = smtpSection.GetSection("Host").Value;
                _from = smtpSection.GetSection("From").Value;
                _alias = smtpSection.GetSection("Alias").Value;
            }
        }

        public void EnviarEmail(EmailModel emailModel)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(_host, 587))
                {
                    
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(_from, _alias);
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.To.Add(emailModel.To);
                    mailMessage.Body = emailModel.Message;
                    mailMessage.Subject = emailModel.Subject;
                    mailMessage.IsBodyHtml = emailModel.IsBodyHtml;

                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("ntroncoso2010@hotmail.com", "?");
                    client.Send(mailMessage);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
