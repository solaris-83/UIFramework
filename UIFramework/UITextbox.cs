using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework
{
    public class UITextbox : UIElement
    {
        public UITextbox(string placeholder = "", string initialValue = "")
        {
            Props["placeholder"] = placeholder;
            State["value"] = initialValue;
            State["enabled"] = true;
        }

        public string Value
        {
            get => State.TryGetValue("value", out var v) ? v?.ToString() : "";
            set => State["value"] = value;
        }

        public bool Enabled
        {
            get => State.TryGetValue("enabled", out var v) && (bool)v;
            set => State["enabled"] = value;
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
