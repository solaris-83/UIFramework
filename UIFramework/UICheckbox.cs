

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
        private readonly bool _newValue;

        public CheckboxChangedCommand(UICheckbox checkbox, bool newValue)
        {
            _checkbox = checkbox;
            _newValue = newValue;
        }

        public void Execute()
        {
            _checkbox.Checked = _newValue;
        }
    }
}
