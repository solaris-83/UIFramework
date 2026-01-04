

using Newtonsoft.Json;

namespace UIFramework
{
    public class UICheckbox : UIElement
    {
        public UICheckbox(string text, bool isChecked = false)
        {
            Props["text"] = text;
            States["checked"] = isChecked;
        }

        [JsonIgnore]
        public bool Checked
        {
            get => States.TryGetValue("checked", out var v) && (bool)v;
            set => States["checked"] = value;
        }
    }

    public class CheckboxChangedCommand : ICommand
    {
        private readonly UICheckbox _checkbox;
        private readonly Dictionary<string, object> _states;

        public CheckboxChangedCommand(UICheckbox checkbox, Dictionary<string, object> states)
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
