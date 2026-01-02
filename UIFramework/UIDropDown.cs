using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework
{
    public class UIDropDown : UIElement
    {
        public UIDropDown(IEnumerable<DropDownOption> options, string selected = null)
        {
            Props["options"] = options.ToList();
            State["selected"] = selected;
            State["enabled"] = true;
        }

        public string Selected
        {
            get => State.TryGetValue("selected", out var v) ? v?.ToString() : null;
            set => State["selected"] = value;
        }

        public bool Enabled
        {
            get => State.TryGetValue("enabled", out var v) && (bool)v;
            set => State["enabled"] = value;
        }
    }

    public class DropDownOption
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public DropDownOption(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }

    public class DropDownChangedCommand : ICommand
    {
        private readonly UIDropDown _dropDown;
        private readonly string _selected;

        public DropDownChangedCommand(UIDropDown dropDown, string selected)
        {
            _dropDown = dropDown;
            _selected = selected;
        }

        public void Execute()
        {
            _dropDown.Selected = _selected;
        }
    }

}
