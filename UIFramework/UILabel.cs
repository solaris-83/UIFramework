

using Newtonsoft.Json;

namespace UIFramework
{
    public class UILabel : UIElement
    {
        public UILabel(string text)
        {
            Props["text"] = text;
        }

        [JsonIgnore]
        public string Text
        {
            get => Props.ContainsKey("text") ? (string)Props["text"] : "";
            set => Props["text"] = value;
        }
    }
}
