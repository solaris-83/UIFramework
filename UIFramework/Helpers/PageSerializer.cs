using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UIFramework.Helpers
{

    public static class PageSerializer
    {

        public static string Serialize(object obj)
            => JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented, // Pretty print
                DateFormatString = "yyyy-MM-dd", // Custom date format
                ContractResolver = new CamelCasePropertyNamesContractResolver(), // camelCase
            });
    }
}
