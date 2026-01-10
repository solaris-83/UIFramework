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

        #region States
        private string _text;
        [JsonIgnore]
        public string Text
        {
            get => _text;
            set 
            {
                SetStatesProperty(ref _text, value, nameof(Text));
            }
        }

        #endregion

        #region

        private string _placeholder;
        [JsonIgnore]
        public string Placeholder
        {
            get => _placeholder;
            set
            {
                SetPropsProperty(ref _placeholder, value, nameof(Placeholder));
            }
        }

        #endregion
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
