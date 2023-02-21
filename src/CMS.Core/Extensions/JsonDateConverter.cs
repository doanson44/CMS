using Newtonsoft.Json.Converters;

namespace CMS.Core.Extensions;

public class JsonDateConverter : IsoDateTimeConverter
{
    public JsonDateConverter()
    {
        DateTimeFormat = "dd/MM/yyyy";
    }
}
