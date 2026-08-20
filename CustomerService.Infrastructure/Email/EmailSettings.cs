using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Infrastructure.Email
{
    public sealed class EmailSettings
    {
        public const string SectionName = "EmailSettings";

        public string SmtpHost { get; init; } = null!;
        public int SmtpPort { get; init; }
        public string SenderEmail { get; init; } = null!;
        public string SenderName { get; init; } = null!;
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}

