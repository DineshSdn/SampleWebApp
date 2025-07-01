using System;
using App.Common.Abstractions.Utilities;
using App.Common.Attributes;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Implementations.Utilities
{
    [TransientService]
    public class LoggedinUserService : ILoggedinUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggedinUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long UserId => Convert.ToInt64(_httpContextAccessor.HttpContext?.User?.FindFirst("user_id")?.Value ?? "0");

        public string IpAddress
        {
            get
            {
                var headerIp = _httpContextAccessor?.HttpContext?.Request?.Headers?["X-Ip-Address"];
                var ipAddress = headerIp.HasValue ? headerIp.Value.ToString() : null;

                if (!string.IsNullOrWhiteSpace(ipAddress))
                    return ipAddress;

                ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                if (!string.IsNullOrWhiteSpace(ipAddress))
                    return ipAddress;

                return "172.0.0.1";
            }
        }
    }
}
