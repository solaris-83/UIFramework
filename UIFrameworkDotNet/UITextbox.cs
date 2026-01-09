using Newtonsoft.Json;
using System.Collections.Generic;

namespace UIFrameworkDotNet
{
    public class UITextbox : UIElement
    {
        public UITextbox(string placeholder, string initialValue)
        {
            Placeholder = placeholder?? "";
            Text = initialValue?? "";
            Enabled = true;
        }

        public UITextbox() : this("", "")
        {
            
        }

        private string _text;
        [JsonIgnore]
        public string Text
        {
            get => _text;
            set 
            {
                if (_text == value) return;
                _text = value;
                States["text"] = value; 
                OnPropertyChanged(nameof(Text)); 
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

    public class TextboxTextChangedCommand : ICommand
    {
        private readonly UITextbox _textBox;

        public TextboxTextChangedCommand(UITextbox textBox)
        {
            _textBox = textBox;
        }

        public void Execute(object newValue)
        {
            _textBox.Text = newValue.ToString();
        }
    }
}
