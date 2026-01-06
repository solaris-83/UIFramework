

using Newtonsoft.Json;

namespace UIFramework
{
    public class UIButton : UIElement
    {
        public UIButton(string tag, bool enabled)
        {
            Props["tag"] = tag;
            Props["text"] = tag;
            States["enabled"] = enabled;
        }

        public UIButton(string tag,  bool enabled, string style)
        {
            Props["tag"] = tag;
            Props["text"] = tag;
            States["enabled"] = enabled;
            Props["style"] = new Style
            {
                Layout = style
            };
        }

        public UIButton(string tag, bool enabled, string style, string translation)
        {
            Props["tag"] = tag;
            Props["text"] = translation;
            States["enabled"] = enabled;
            Props["style"] = new Style
            {
                Layout = style
            };
        }

        [JsonIgnore]
        public string Text
        {
            get => Props.ContainsKey("text")?  Props["text"].ToString() : "";
            set { Props["text"] = value; OnPropertyChanged(nameof(Text)); }
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
