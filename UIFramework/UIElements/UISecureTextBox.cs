using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using UIFramework.Interfaces;

namespace UIFramework.UIElements
{
    public class UISecureTextBox : UIInputBox
    {
        private byte[] _internalValue = new byte[0];
        public UISecureTextBox(bool isReadOnly) : base(isReadOnly)
        {
        }

        public UISecureTextBox() : this(isReadOnly: false)
        {
        }

        [JsonIgnore]
        public EncryptedData Value => new EncryptedData(_internalValue);

        [JsonIgnore]
        public int Length => _internalValue.Length;

        public void SetValue(byte[] value)
        {
            _internalValue = value;
            ApplyValidationRules(_internalValue == null ? "" : Encoding.UTF8.GetString(_internalValue));
        }
    }

    public class SecureTextBoxCommand : ICommand
    {
        public UISecureTextBox Value { get; set; }

        public SecureTextBoxCommand(UISecureTextBox value)
        {
            Value = value;
        }

        public virtual void Execute(Dictionary<string, object> newStates)
        {
            newStates.TryGetValue("contents", out var contents);
            Value.SetValue(Encoding.UTF8.GetBytes(contents.ToString()));
        }
    }
}
