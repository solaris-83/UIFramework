

using Newtonsoft.Json;

namespace UIFramework
{
    public class UICheckbox : UIElement
    {
        public UICheckbox(string text, bool isChecked, bool isEnabled)
        {
            Props["tag"] = null;
            Props["text"] = text;
            States["checked"] = isChecked;
            States["enabled"] = isEnabled;
        }

        public UICheckbox(string text, bool isChecked, bool isEnabled, string tag)
        {
            Props["tag"] = tag;
            Props["text"] = text;
            States["checked"] = isChecked;
            States["enabled"] = isEnabled;
        }

        public UICheckbox(string text, bool isEnabled, string tag)
        {
            Props["tag"] = tag;
            Props["text"] = text;
            States["checked"] = false;
            States["enabled"] = isEnabled;
        }

        [JsonIgnore]
        public bool Checked
        {
            get => States.TryGetValue("checked", out var v) && (bool)v;
            set => States["checked"] = value;
        }

        [JsonIgnore]
        public bool Enabled
        {
            get => States.TryGetValue("enabled", out var v) && (bool)v;
            set => States["enabled"] = value;
        }
    }

    public class CheckboxPropertyChangedCommand : ICommand
    {
        private readonly UICheckbox _checkbox;
        private readonly Dictionary<string, object> _states;

        public CheckboxPropertyChangedCommand(UICheckbox checkbox, Dictionary<string, object> states)
        {
            _checkbox = checkbox;
            _states = states;
        }

        public void Execute()
        {
           _checkbox.UpdateStates(_states);
        }
    }
}
