using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace BlazorReportViewer.Models {
    public enum EmailExportFormat {
        PDF,
        CSV,
        XLS
    }
    public class EmailModel {
        [Required(ErrorMessage = "Please specify the email subject.")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Please specify at least one recipient.")]
        [EmailAddresses(ErrorMessage = "Invalid email address.")]
        public IEnumerable<string> To { get; set; }
        public string Body { get; set; } = "";
        [Required]
        [DisplayName("Attachment Format")]
        public EmailExportFormat Format { get; set; }
        [Required]
        [DisplayName("Attachment Name")]
        public string Attachment { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class EmailAddressesAttribute : ValidationAttribute {
        public override bool IsValid(object value) {
            IEnumerable<string> data = value as IEnumerable<string>;
            return data != null && data.Any() && data.All(x => {
                try {
                    var mailAddress = new MailAddress(x);
                    return true;
                } catch(FormatException) {
                    return false;
                }
            });
        }
    }
}
