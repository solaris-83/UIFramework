using Newtonsoft.Json;

namespace UIFramework
{
    public class UITextbox : UIElement
    {
        public UITextbox(string placeholder = "", string initialValue = "")
        {
            Props["placeholder"] = placeholder;
            States["value"] = initialValue;
            States["enabled"] = true;
        }

        [JsonIgnore]
        public string Value
        {
            get => States.TryGetValue("value", out var v) ? v?.ToString() : "";
            set => States["value"] = value;
        }

        [JsonIgnore]
        public bool Enabled
        {
            get => States.TryGetValue("enabled", out var v) && (bool)v;
            set => States["enabled"] = value;
        }
    }
    public class TextChangedCommand : ICommand
    {
        private readonly UITextbox _textBox;
        private readonly string _value;

        public TextChangedCommand(UITextbox textBox, string value)
        {
            _textBox = textBox;
            _value = value;
        }

        public void Execute()
        {
            _textBox.Value = _value;
        }
    }


}
