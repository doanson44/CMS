using System.Diagnostics;
using CMS.Core.Enums;

namespace CMS.Core;

[DebuggerStepThrough]
public class ResponseContext<T>
{
    public ErrorCodes StatusCode { get; set; } = ErrorCodes.Success;
    public T Data { get; set; }
    public string Message { get; set; }

    public bool IsSuccess => StatusCode == ErrorCodes.Success;

    public ResponseContext(ErrorCodes statusCode)
    {
        StatusCode = statusCode;
    }

    public ResponseContext(T data)
    {
        Data = data;
    }

    public ResponseContext(ErrorCodes statusCode, T data)
    {
        StatusCode = statusCode;
        Data = data;
    }
}
