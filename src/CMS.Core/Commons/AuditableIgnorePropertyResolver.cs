using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CMS.Core.Commons;

public class AuditableIgnorePropertyResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var p = base.CreateProperty(member, memberSerialization);
        p.Ignored = !p.PropertyType.IsPrimitive
            && p.PropertyType != typeof(Guid)
            && p.PropertyType != typeof(Enum)
            && p.PropertyType != typeof(string)
            && p.PropertyType != typeof(DateTime);

        return p;
    }
}
