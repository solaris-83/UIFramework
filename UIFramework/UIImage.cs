using Newtonsoft.Json;

namespace UIFramework
{
    public class UIImage : UIElement
    {
        public UIImage(string src)
        {
            Props["src"] = $"/img/{src}";
        }

        [JsonIgnore]
        public string Src
        {
            get => Props["src"]?.ToString();
            set => Props["src"] = value;
        }
    }
}
