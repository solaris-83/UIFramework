
using Newtonsoft.Json;
using System;
namespace UIFrameworkDotNet
{
    public class UICheckbox : UIElement
    {
        public UICheckbox(string text, bool isChecked, bool isEnabled)
        {
            Tag = default;
            Text = text;
            Checked = isChecked;
            Enabled = isEnabled;
        }

        public UICheckbox(string text, bool isChecked, bool isEnabled, string tag)
        {
            Tag = tag;
            Text = text;
            Checked = isChecked;
            Enabled = isEnabled;
        }

        public UICheckbox(string text, bool isEnabled, string tag)
        {
            Tag = tag;
            Text = text;
            Checked = false;
            Enabled = isEnabled;
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
                Props["text"] = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        private bool _checked;
        [JsonIgnore]
        public bool Checked
        {
            get => _checked;
            set 
            { 
                if (_checked == value) return;
                _checked = value;
                States["checked"] = value; 
                OnPropertyChanged(nameof(Checked)); 
            }
        }
    }

    public class CheckboxCheckedChangedCommand : ICommand
    {
        private readonly UICheckbox _checkbox;

        public CheckboxCheckedChangedCommand(UICheckbox checkbox)
        {
            _checkbox = checkbox;
        }

        public void Execute(object newValue)
        {
            _checkbox.Checked = Convert.ToBoolean(newValue);
        }
    }
}
