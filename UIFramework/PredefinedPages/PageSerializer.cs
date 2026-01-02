using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework.PredefinedPages
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public static class PageSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true, 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static string Serialize(Page page)
            => JsonSerializer.Serialize(page, Options);
    }
}
