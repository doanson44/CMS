using System;

namespace CMS.WebApi.AuthEndpoints;

public class AuthenticateResponse
{
    public AuthenticateResponse()
    {
    }

    protected Guid _correlationId = Guid.NewGuid();
    public Guid CorrelationId() => _correlationId;
    public bool Result { get; set; } = false;
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsLockedOut { get; set; } = false;
    public bool IsNotAllowed { get; set; } = false;
    public bool RequiresTwoFactor { get; set; } = false;
}
