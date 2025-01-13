using System.Collections.Generic;

namespace DND.Middleware.Dtos.Notification.Mail;

public class MailDto
{
    public List<string> ToEmailAddressList { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public bool IsBodyPlainText { get; set; }
}