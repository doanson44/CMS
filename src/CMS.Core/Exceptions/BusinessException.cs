using CMS.Core.Enums;
using CMS.Core.Extensions;
using System;

namespace CMS.Core.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        public ErrorCodes StatusCode { get; set; }

        public BusinessException()
        {
        }

        public BusinessException(ErrorCodes statusCode) : base(string.Format(statusCode.GetDescription()))
        {
            StatusCode = statusCode;
        }

        public BusinessException(ErrorCodes statusCode, params object[] args) : base(string.Format(statusCode.GetDescription(), args))
        {
            StatusCode = statusCode;
        }

        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, Exception inner) : base(message, inner) { }

        protected BusinessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
