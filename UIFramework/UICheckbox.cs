using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework
{
    public class UICheckbox : UIElement
    {
        public UICheckbox(string text, bool isChecked = false)
        {
            Props["text"] = text;
            State["checked"] = isChecked;
        }

        public bool Checked
        {
            get => State.TryGetValue("checked", out var v) && (bool)v;
            set => State["checked"] = value;
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
