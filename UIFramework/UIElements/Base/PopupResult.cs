using System.Collections.Generic;

namespace UIFramework.UIElements.Base
{
    public class PopupResult
    {
        public string CommandName { get; set; }
        public Dictionary<string, object> CommandArgs { get; set; }
        public object CommandArgByKey(string key)
        {
            object arg = default;
            CommandArgs?.TryGetValue(key, out arg);
            return arg;
        }
    }
}
