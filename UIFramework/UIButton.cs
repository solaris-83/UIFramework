

using Newtonsoft.Json;

namespace UIFramework
{
    public class UIButton : UIElement
    {
        public UIButton(string tag, bool enabled)
        {
            Tag = tag;
            Text = tag;
            Enabled = enabled;
        }

        public UIButton(string tag,  bool enabled, string style)
        {
            Tag = tag;
            Text = tag;
            Enabled = enabled;
            Style = new Style
            {
                Layout = style
            };
        }

        public UIButton(string tag, bool enabled, string style, string translation)
        {
            Tag = tag;
            Text = translation;
            Enabled = enabled;
            Style = new Style
            {
                Layout = style
            };
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
                Props["text"] = value; 
                OnPropertyChanged(nameof(Text)); 
            }
        }

        public class ButtonPropertyChangedCommand : ICommand
        {
            private readonly UIButton _button;
            private readonly Dictionary<string, object> _states;

            public ButtonPropertyChangedCommand(UIButton button, Dictionary<string, object> states)
            {
                _button = button;
                _states = states;
            }

            public void Execute()
            {
                _button.UpdateStates(_states);
            }
        }
    }
}
