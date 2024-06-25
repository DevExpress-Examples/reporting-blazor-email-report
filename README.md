<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/798741636/24.1.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1232536)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# Reporting for Blazor - Email a Report from the Native Blazor Report Viewer

This example leverages the [Mailkit](https://mimekit.net/docs/html/Introduction.htm) library to send an email using our native Blazor Report Viewer.

The **Send Email** button in the Viewer’s toolbar opens the **Send Email** dialog ([DxPopup](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxPopup)).You can specify the recipient list, subject, attachment, and body within the dialog. Click the **Send** button to send the report with the specified settings:

![Report Viewer - Send Email Window](images/send-email-window.png)

Example of email output: 

![Report Viewer - Sent Email Example](images/sent-email-example.png)

## Implementation Details

### UI Elements

The example handles the [`CustomizeToolbar`](https://docs.devexpress.com/XtraReports/DevExpress.Blazor.Reporting.DxReportViewer.OnCustomizeToolbar) event to add a **Send Email** button to the Blazor Report Viewer's Toolbar. The code snippet below locates the **Export To** command and adds the new button next to it:

```cs
void OnCustomizeToolbar(ToolbarModel toolbarModel) {
    toolbarModel.AllItems.Insert(toolbarModel.AllItems.FindIndex(i => i.Id == ToolbarItemId.ExportTo), new ToolbarItem() {
        IconCssClass = "mail-icon",
        Text = "Send Email",
        AdaptiveText = "Send Email",
        AdaptivePriority = 1,
        Click = (args) => {
            IsPopupVisible = ValidateEditingFields();
            return Task.CompletedTask;
        }
    });
}
```

Clicking the newly added button opens a [DxPopup](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxPopup) used to specify email options: recipients, subject, and body. The pop-up form uses the following components: 

- [DxTagBox](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxTagBox-2) allows users to select individual recipients and build a list.
- [DxTextBox](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxTextBox) allows users to specify email subject and attachment file name. 
- [DxComboBox](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxComboBox-2) allows users to select attachment format.
- [DxHtmlEditor](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxHtmlEditor?v=24.1) allows users to specify the mail body. 

For `DxPopup` configuration, refer to the following file: [ReportViewer.razor](BlazorReportViewer/Pages/ReportViewer.razor#L28).

The [DxToastProvider](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxToastProvider?v=24.1) component displays data validation notifications to users. 
For `DxToastProvider` configuration, refer to the following file: [ReportViewer.razor](BlazorReportViewer/Pages/ReportViewer.razor#L73).

### Email Service 

> [!IMPORTANT]  
> This example specifies credentials for SMTP server authentication. In production projects, you should always use [secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows) to store sensitive information. Remember, always follow security best-practices to protect the integrity of your app.

Clicking the **Send** button in the **Send Email** window triggers the server-side [`EmailService.SendEmailAsync`](BlazorReportViewer/Services/EmailService.cs#L64) method. This method exports a report (to the specified format) and emails the generated report based upon specified settings. 

[`MailKitEmailService`](BlazorReportViewer/Services/EmailService.cs) uses the [MailKit ](https://mimekit.net/docs/html/Introduction.htm) library. You can configure the `SendEmailAsync` method to connect to your SMTP server of choice.

Register the service in the [*Program.cs*](BlazorReportViewer/Program.cs) file:

```cs
builder.Services.AddScoped<IEmailService, MailKitEmailService>();
```

You can use [MailDev](https://maildev.github.io/maildev/) to test your application during development.

Refer to the files below for more information:
- [EmailService.cs](BlazorReportViewer/Services/EmailService.cs) implements email send related logic. 
- [ReportViewer.razor](BlazorReportViewer/Pages/ReportViewer.razor) contains code that handles user inputs and uses the `EmailService`.

### Validation 

The example implements two levels of validation:

- When a user clicks the **Send Email** button in the Blazor Report Viewer's Toolbar, the **Send Email** dialog opens only if all editing fields are filled.
- When a user clicks the **Send** button in the **Send Email** dialog, the email is sent only when all the required fields are populated.

The [`DxToastProvider`](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxToastProvider?v=24.1) component displays validation notifications to users.

Refer to the files below to learn more about validation logic used in this example:
- [ValidationErrorToast.razor](BlazorReportViewer/Pages/ValidationErrorToast.razor)
- [ReportViewer.razor](BlazorReportViewer/Pages/ReportViewer.razor#L73)

## Files to Review

- [EmailService.cs](BlazorReportViewer/Services/EmailService.cs)
- [appsettings.json](BlazorReportViewer/appsettings.json)
- [ReportViewer.razor](BlazorReportViewer/Pages/ReportViewer.razor)
- [Program.cs](BlazorReportViewer/Program.cs)

## Documentation  

- [Native Report Viewer for Blazor](https://docs.devexpress.com/XtraReports/403594/web-reporting/blazor-reporting/server/blazor-report-viewer-native)
- [Email Reports](https://docs.devexpress.com/XtraReports/17634/detailed-guide-to-devexpress-reporting/store-and-distribute-reports/export-reports/email-reports)

## More Examples

- [Reporting for ASP.NET MVC - How to Email a Report from the Document Viewer](https://github.com/DevExpress-Examples/reporting-web-mvc-email-report)
- [Reporting for WinForms - How to Use MailKit to Email a Report](https://github.com/DevExpress-Examples/reporting-winforms-mailkit-email-report-pdf)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=reporting-blazor-email-report&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=reporting-blazor-email-report&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
