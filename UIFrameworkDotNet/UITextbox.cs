using Newtonsoft.Json;
using System.Collections.Generic;

namespace UIFrameworkDotNet
{
    public class UITextbox : UIElement
    {
        public UITextbox(string placeholder, string initialValue)
        {
            Placeholder = placeholder?? "";
            Value = initialValue?? "";
            Enabled = true;
        }

        public UITextbox() : this("", "")
        {
            
        }

        private string _value;
        [JsonIgnore]
        public string Value
        {
            get => _value;
            set 
            {
                if (_value == value) return;
                _value = value;
                States["value"] = value; 
                OnPropertyChanged(nameof(Value)); 
            }
        }

        private string _placeholder;
        [JsonIgnore]
        public string Placeholder
        {
            get => _placeholder;
            set
            {
                if (_placeholder == value) return;
                _placeholder = value;
                Props["placeholder"] = value;
                OnPropertyChanged(nameof(Placeholder));
            }
        }

    }
    public class TextPropertyChangedCommand : ICommandOld
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
