using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BusinessLogic
{
    public static class JsonSerializerSettingsExtensions
    {
        public static JsonSerializerSettings Configure(this JsonSerializerSettings settings)
        {
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            settings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";
            settings.Converters.Insert(0, new DefaultValueTypeFallbackConverter());
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            
            return settings;
        }
    }
}