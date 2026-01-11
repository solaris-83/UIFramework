using Newtonsoft.Json;
using ScriptLibraries.Data.Interfaces;

namespace UIFrameworkDotNet
{
    public class UITextbox : UITextElement
    {
        public UITextbox(string placeholder, string initialValue) : base(initialValue)
        {
            Placeholder = placeholder?? "";
            Enabled = true;
        }

        public UITextbox() : this("", "")
        {
            
        }

        #region States

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
}
