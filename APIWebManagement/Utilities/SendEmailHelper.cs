using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace APIWebManagement.Utilities
{
    public static class SendEmailHelper
    {
        static bool MailServerEnable = true;
        static string MailServerHost = "webmail.lacviet.com.vn";
        static int MailServerPort = 587;
        static bool MailServerSsl = true;
        static string MailAccount = "noreply@lacviet.com.vn";
        static string MailAccountPass = "1s@1212";
        static string MailAccountAddess = "noreply@lacviet.com.vn";

        public static void SendEmail(this HttpContext httpContext, string EmailTo, string SubjectContent = null, string BodyContent = null, string ProductName = "")
        {
            if (!MailServerEnable)
                return;

            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("", MailAccountAddess);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(EmailTo);
                message.To.Add(to);

                message.Subject = SubjectContent;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = BodyContent;
                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                client.Connect(
                    MailServerHost,
                    MailServerPort,
                    MailServerSsl
                );
                client.Authenticate(MailAccount, MailAccountPass);

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi" + ex.Message, ex);
            }
        }

        public static void email_sender(Object state_object_mail)
        {
            var json = JsonConvert.SerializeObject(state_object_mail);
            var data = JsonConvert.DeserializeObject<MailExt>(json);

            data.mailInfo.httpContext.SendEmail(
               data.mailInfo.EmailTo,
               data.mailInfo.SubjectContent,
               data.mailInfo.BodyContent
               );

            Thread.Sleep(1000);
        }

        public class MailInfo
        {
            public HttpContext httpContext { get; set; }
            public string ProductName { get; set; }
            public List<string> EmailTos { get; set; }
            public string EmailTo { get; set; }
            public string SubjectContent { get; set; }
            public string BodyContent { get; set; }
        }

        public class MailExt
        {
            public MailInfo mailInfo { get; set; }
            public string webBase { get; set; }
        }
    }
}
