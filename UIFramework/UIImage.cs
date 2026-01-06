using Newtonsoft.Json;

namespace UIFramework
{
    public class UIImage : UIElement
    {
        public UIImage(string src)
        {
            States["src"] = $"/img/{src}";
        }

        [JsonIgnore]
        public string Src
        {
            get => States.ContainsKey("src")? States["src"].ToString() : "";
            set => States["src"] = value;
        }
    }
}
