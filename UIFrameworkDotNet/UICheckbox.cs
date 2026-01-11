
using Newtonsoft.Json;
using System;
namespace UIFrameworkDotNet
{
    public class UICheckbox : UITextElement
    {
        public UICheckbox(string text, bool isChecked, bool isEnabled) : base(text)
        {
            Tag = default;
            Checked = isChecked;
            Enabled = isEnabled;
        }

        public UICheckbox(string text, bool isChecked, bool isEnabled, string tag) : base(text)
        {
            Tag = tag;
            Checked = isChecked;
            Enabled = isEnabled;
        }

        public UICheckbox(string text, bool isEnabled, string tag) : base(text)
        {
            Tag = tag;
            Checked = false;
            Enabled = isEnabled;
        }

        #region Props
       

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
