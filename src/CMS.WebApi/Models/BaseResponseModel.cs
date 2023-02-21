using System.Diagnostics;
using CMS.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CMS.WebApi.Models;

[DebuggerStepThrough]
public class BaseResponseModel
{
    public ErrorCodes Code { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }
}

public class BaseResponseModel<T> : BaseResponseModel// where T : class
{
    public T Data { get; set; }
    public int Total { get; set; }
}
