
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

        #region Props
        private string _text;
        [JsonIgnore]
        public string Text
        {
            get => _text;
            set
            {
                SetPropsProperty(ref _text, value, nameof(Text));
            }
        }

        #endregion

        #region States

        private bool _checked;
        [JsonIgnore]
        public bool Checked
        {
            get => _checked;
            set 
            { 
                SetStatesProperty(ref _checked, value,  nameof(Checked));
            }
        }

        #endregion
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
