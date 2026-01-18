using Newtonsoft.Json;

namespace UIFramework
{
    public class UIImage : UIElement
    {
        public UIImage(string src)
        {
            Src = $"/img/{src}";
        }

        private string _src;
        [JsonIgnore]
        public string Src
        {
            get => _src;
            set 
            {
                if (_src == value) return;
                _src = value;
                States["src"] = value; 
                OnPropertyChanged(nameof(Src)); 
            }
        }
    }
}
