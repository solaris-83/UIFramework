

using Newtonsoft.Json;

namespace UIFramework
{
    public class UILabel : UIElement
    {
        public UILabel(string text)
        {
            States["text"] = text;
        }

        [JsonIgnore]
        public string Text
        {
            get => States.ContainsKey("text") ? States["text"].ToString() : "";
            set => States["text"] = value;
        }
    }
}
