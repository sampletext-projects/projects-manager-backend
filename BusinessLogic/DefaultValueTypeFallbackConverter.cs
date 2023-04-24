using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BusinessLogic;

public class DefaultValueTypeFallbackConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsValueType || Nullable.GetUnderlyingType(objectType) is not null;
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null && Nullable.GetUnderlyingType(objectType) is not null)
            return null;
        
        var token = JToken.Load(reader);
        try
        {
            return token.ToObject(objectType);
        }
        catch (Exception)
        {
            if (Nullable.GetUnderlyingType(objectType) is not null)
            {
                return null;
            }
            return GetDefaultValue(objectType);
        }
    }

    public override bool CanWrite { get { return false; } }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
    
    private static object? GetDefaultValue(Type t)
    {
        if (t.IsValueType)
            return Activator.CreateInstance(t);

        return null;
    }
}