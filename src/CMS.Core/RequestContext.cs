using System;
using System.Diagnostics;
using System.Threading;

namespace CMS.Core;

public class RequestContext<T>
{
    public T Payload { get; set; }
    public Guid CurrentUserId { get; set; }
    public CancellationToken Token { get; set; } = default;

    public RequestContext(T payload)
    {
        Payload = payload;
    }

    [DebuggerStepThrough]
    public RequestContext(T payload, Guid currentUserId, CancellationToken token)
        : this(payload)
    {
        CurrentUserId = currentUserId;
        Token = token;
    }

    public RequestContext()
    {
    }

    public (T payload, Guid currentUser, CancellationToken cancellationToken) Params
        => (Payload, CurrentUserId, Token);

    [DebuggerStepThrough]
    public RequestContext<TCopy> CopyContext<TCopy>(TCopy payload)
    {
        return new RequestContext<TCopy>(payload, CurrentUserId, Token);
    }
}
