

using Newtonsoft.Json;

namespace UIFrameworkDotNet
{
    public class UILabel : UIElement
    {
        public UILabel(string text)
        {
            Text = text;
        }

        private string _text;
        [JsonIgnore]
        public string Text
        {
            get => _text;
            set 
            { 
                if (_text == value) return;
                _text = value;
                States["text"] = value; 
                OnPropertyChanged(nameof(Text)); 
            }
        }
    }
}
