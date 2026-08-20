using CustomerService.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Infrastructure
{
    public sealed class ApiUrlProvider : IApiUrlProvider
    {
        public string BaseUrl { get; }

        public ApiUrlProvider(IConfiguration configuration)
        {
            BaseUrl = configuration["ApiBaseUrl"]!;
        }
    }
}
