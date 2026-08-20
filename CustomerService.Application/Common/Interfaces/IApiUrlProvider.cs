using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Common.Interfaces
{
    public interface IApiUrlProvider
    {
        string BaseUrl { get; }
    }
}
