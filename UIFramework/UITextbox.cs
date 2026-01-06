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
            get => States.ContainsKey("value")? States["value"].ToString() : "";
            set => States["value"] = value;
        }

        [JsonIgnore]
        public bool Enabled
        {
            get => States.TryGetValue("enabled", out var v) && (bool)v;
            set => States["enabled"] = value;
        }
    }
    public class TextPropertyChangedCommand : ICommand
    {
        private readonly UITextbox _textBox;
        private readonly Dictionary<string, object> _states;

        public TextPropertyChangedCommand(UITextbox textBox, Dictionary<string, object> states)
        {
            _textBox = textBox;
            _states = states;
        }

        public void Execute()
        {
            _textBox.UpdateStates(_states);
        }
    }
}
