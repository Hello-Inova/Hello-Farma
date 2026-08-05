using HelloFarma.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HelloFarma.Infrastructure.Identity;

public class RequestContextAccessor(IHttpContextAccessor httpContextAccessor) : IRequestContext
{
    public string? IpAddress => httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
}
