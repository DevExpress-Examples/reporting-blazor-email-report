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
        [Required]
        public string Subject { get; set; }
        [Required]
        [EmailAdresses(ErrorMessage = "Invalid email")]
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
    public class EmailAdressesAttribute : ValidationAttribute {
        public override bool IsValid(object value) {
            IEnumerable<string> data = value as IEnumerable<string>;
            return data != null && data.Any() && data.All(x => {
                try {
                    var mailAdress = new MailAddress(x);
                    return true;
                } catch(FormatException) {
                    return false;
                }
            });
        }
    }
}
