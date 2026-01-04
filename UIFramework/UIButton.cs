

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
        public bool Enabled
        {
            get => States.TryGetValue("enabled", out var v) && (bool)v;
            set => States["enabled"] = value;
        }

        [JsonIgnore]
        public Style Style
        {
            get
            {
                Props.TryGetValue("style", out var style);
                if (style is Style s)
                    return s;
                return null;
            }
            set => Props["style"] = value;
        }

        [JsonIgnore]
        public string Text
        {
            get => Props["text"]?.ToString();
            set => Props["text"] = value;
        }

        public class ButtonChangedCommand : ICommand
        {
            private readonly UIButton _button;
            private readonly bool _newValue;

            public ButtonChangedCommand(UIButton button, bool newValue)
            {
                _button = button;
                _newValue = newValue;
            }

            public void Execute()
            {
                _button.Enabled = _newValue;
            }
        }
    }
}
