using Kentimce.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Kentimce.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId != null ? Guid.Parse(userId) : null;
        }

        public string GetUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public string GetUserIp()
        {
            try
            {
                var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                // Eğer IP null veya boş ise varsayılan bir değer dön
                if (string.IsNullOrEmpty(ip))
                {
                    return "0.0.0.0";
                }

                // IPv6 localhost (::1) gelirse okunabilir yap
                if (ip == "::1")
                {
                    return "127.0.0.1";
                }

                return ip;
            }
            catch
            {
                // Olası bir hata durumunda yine varsayılan dön
                return "0.0.0.0";
            }
        }
    }
}
