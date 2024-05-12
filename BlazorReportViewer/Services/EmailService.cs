using System.Collections.ObjectModel;
using System.Net.Mail;
using DevExpress.XtraPrinting;
using BlazorReportViewer.Data;
using BlazorReportViewer.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace BlazorReportViewer.Services {
    public interface IEmailService {
        Task SendEmailAsync(PrintingSystemBase printingSystem, EmailModel emailModel);
        Task<IEnumerable<string>> GetRecipientsAsync();
    }

    public class EmailServiceOptions {
        public string Host { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public abstract class EmailService(IOptions<EmailServiceOptions> options) : IEmailService {
        protected EmailServiceOptions Options { get; private set; } = options.Value;
        protected ReadOnlyDictionary<EmailExportFormat, string> FormatMimeTypes { get; } = new ReadOnlyDictionary<EmailExportFormat, string>(new Dictionary<EmailExportFormat, string>()
        {
            { EmailExportFormat.CSV, "text/csv" },
            { EmailExportFormat.XLS, "application/vnd.ms-excel" },
            { EmailExportFormat.PDF, "application/pdf" }
        });

        public async Task<IEnumerable<string>> GetRecipientsAsync() {
            await Task.Delay(TimeSpan.FromSeconds(3));
            return EmailsDataSource.Emails;
        }
        public abstract Task SendEmailAsync(PrintingSystemBase printingSystem, EmailModel emailModel);
        protected MailMessage GetMailMessage(PrintingSystemBase printingSystem, EmailModel emailModel) {
            MailMessage message = new();
            message.From = new MailAddress(Options.From);
            foreach(var mailAddress in emailModel.To)
                message.To.Add(new MailAddress(mailAddress));
            message.Subject = emailModel.Subject;
            message.IsBodyHtml = true;
            message.Body = emailModel.Body;
            FormatMimeTypes.TryGetValue(emailModel.Format, out string mimeType);
            var format = emailModel.Format.ToString().ToLower();
            message.Attachments.Add(new Attachment(GetAttachmentStream(printingSystem, emailModel.Format), $"{emailModel.Attachment}.{format}", mimeType));
            return message;
        }
        private MemoryStream GetAttachmentStream(PrintingSystemBase printingSystem, EmailExportFormat format) {
            var stream = new MemoryStream();
            switch(format) {
                case EmailExportFormat.PDF:
                    printingSystem.ExportToPdf(stream);
                    break;
                case EmailExportFormat.CSV:
                    printingSystem.ExportToCsv(stream);
                    break;
                case EmailExportFormat.XLS:
                    printingSystem.ExportToXls(stream);
                    break;
            }
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }

    public class FakeEmailService(IOptions<EmailServiceOptions> options) : EmailService(options) {
        public override async Task SendEmailAsync(PrintingSystemBase printingSystem, EmailModel emailModel) {
            using MailMessage message = GetMailMessage(printingSystem, emailModel);
            using var client = new SmtpClient(Options.Host, Options.Port);
            client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            client.PickupDirectoryLocation = AppDomain.CurrentDomain.BaseDirectory;
            await Task.Delay(TimeSpan.FromSeconds(5));
            await client.SendMailAsync(message);
        }
    }

    public class MailKitEmailService(IOptions<EmailServiceOptions> options) : EmailService(options) {
        public override async Task SendEmailAsync(PrintingSystemBase printingSystem, EmailModel emailModel) {
            using MailMessage mMessage = GetMailMessage(printingSystem, emailModel);
            using var message = (MimeKit.MimeMessage)mMessage;
            using var client = new MailKit.Net.Smtp.SmtpClient();
            client.Authenticate(Options.Username, Options.Password);
            try {
                client.Connect(Options.Host, Options.Port, SecureSocketOptions.Auto);
                await client.SendAsync(message);
            } finally {
                client.Disconnect(true);
            }
        }
    }
}