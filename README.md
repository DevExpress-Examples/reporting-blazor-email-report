<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1232536)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# Reporting for Blazor - How to Email a Report from the Native Blazor Report Viewer

This example uses the [Mailkit](https://mimekit.net/docs/html/Introduction.htm) library to send an email from our Native Blazor Report Viewer.

The **Send Email** button in the Viewer’s toolbar opens a [DxPopup](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxPopup). In the **Send Email** window, you can specify the list of recipients, subject, attachment, and body. Click the Send button to send the report with the specified settings:

![Report Viewer - Send Email Window](images/send-email-window.png)

The sent email may look as follows:

![Report Viewer - Sent Email Example](images/sent-email-example.png)

## Implementation Details

### UI Elements

The examples handles the [`CustomizeToolbar`](https://docs.devexpress.com/XtraReports/DevExpress.Blazor.Reporting.DxReportViewer.OnCustomizeToolbar) event to add the **Send Email** button to the Viewer's Toolbar. The code snippet below adds the button before the **Export To** button:

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

Click on the **Send Email** button to open the **Send Email** window that is a [DxPopup](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxPopup). In this dialog, you can specify the mail option: recipients, subject, body. The following components are used for email fields:
- [DxTagBox](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxTagBox-2) -  for the list of the recipients
- [DxTextBox](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxTextBox) - to specify the email subject and attachment file name
- [DxComboBox](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxComboBox-2) - to specify attachment format
- [DxHtmlEditor](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxHtmlEditor?v=24.1) - to specify the mail body

For the `DxPopup` configuration, refer to the following file: [ReportViewer.razor](BlazorReportViewer/Pages/ReportViewer.razor#L28).

The [DxToastProvider](https://docs.devexpress.devx/Blazor/DevExpress.Blazor.DxToastProvider?v=24.1) component displays toast notifications to users. 
For the `DxToastProvider` configuration, refer to the following file: [ReportViewer.razor](BlazorReportViewer/Pages/ReportViewer.razor#L73).

### Email Service 

> [!WARNING]  
> In this example, we specify credentials for the authenticate to a SMTP server. In production, we recommend you use [secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows) to store sensitive information.

A click on the **Send** button in the **Send Email** window triggers the server-side [`EmailService.SendEmailAsync`](BlazorReportViewer/Services/EmailService.cs) method. This method exports a report to the specified format, and emails the resulting report according to the specified email sending settings.


The example contains two services to send emails:

- [`DefaultEmailService`](BlazorReportViewer/Services/EmailService.cs) uses the .NET [`SmtpClient`](https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient?view=net-8.0) class. The services is set to work locally without a SMTP server. You can find the sent email in the `bin/Debug/net8.0` folder. 
- [`MailKitEmailService`](BlazorReportViewer/Services/EmailService.cs) uses the [MailKit ](https://mimekit.net/docs/html/Introduction.htm) library. You can configure the `SendEmailAsync` method to connect to your SMTP server.
 
Register the required service in the *Program.cs* file:

```cs
builder.Services.AddScoped<IEmailService, MailKitEmailService>();
// builder.Services.AddScoped<IEmailService, DefaultEmailService>();
```

Refer to the [EmailService.cs](BlazorReportViewer/Services/EmailService.cs) file to see the Email Sending Logic.
Refer to the [ReportViewer.razor](BlazorReportViewer/Pages/ReportViewer.razor) file to see how handle user inputs and call `EmailService`.

### Validation 

The example implements two levels of validation:
- When a user clicks the **Send Email** button in the Report Viewer's Toolbar, the **Send Email** dialog opens only if all editing fields are filled.
- When a user clicks the **Send** button in the **Send Email** dialog, the email is sent only when all the required fields are filled.

The [DxToastProvider](https://docs.devexpress.devx/Blazor/DevExpress.Blazor.DxToastProvider?v=24.1) component displays toast notifications to users.

Refer to the files below for details on validation implementation: 
- [ValidationErrorToast.razor](BlazorReportViewer/Pages/ValidationErrorToast.razor)
- [ReportViewer.razor](BlazorReportViewer/Pages/ReportViewer.razor#L73).

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
