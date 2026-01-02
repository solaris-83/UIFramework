using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UIFramework
{
    public class UIButton : UIElement
    {
        public UIButton(string id, bool enabled) : base(id)
        {
            Props["text"] = id;
            State["enabled"] = enabled;
        }

        public UIButton(string id,  bool enabled, string style) : base(id)
        {
            Props["text"] = id;
            State["enabled"] = enabled;
            Style = new Style
            {
                Layout = style
            };
        }

        public UIButton(string id, bool enabled, string style, string text) : base(id)
        {
            Props["text"] = text;
            State["enabled"] = enabled;
            Style = new Style
            {
                Layout = style
            };
        }

        [JsonIgnore]
        public bool Enabled
        {
            get => State.TryGetValue("enabled", out var v) && (bool)v;
            set => State["enabled"] = value;
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
