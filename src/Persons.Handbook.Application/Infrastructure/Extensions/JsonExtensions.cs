using Newtonsoft.Json;

namespace Persons.Handbook.Application.Infrastructure.Extensions;

public static class JsonExtensions
{
    public static string? ToJson(this object? obj)
    {
        return obj == default ? default : JsonConvert.SerializeObject(obj);
    }

    public static T? FromJsonTo<T>(this string? str)
    {
        return string.IsNullOrEmpty(str)
            ? default
            : JsonConvert.DeserializeObject<T>(str);
    }
}