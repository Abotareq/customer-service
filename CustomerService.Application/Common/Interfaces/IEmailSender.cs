using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Common.Interfaces
{

    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default);
    }
}
