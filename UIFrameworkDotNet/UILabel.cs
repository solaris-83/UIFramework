

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

    public class LabelTextChangedCommand : ICommand
    {
        private readonly UILabel _label;

        public LabelTextChangedCommand(UILabel label)
        {
            _label = label;
        }

        public void Execute(object newValue)
        {
            _label.Text = newValue.ToString();
        }
    }
}
