using BaseCustomApp.Helpers;
using Newtonsoft.Json;

namespace UIFramework.UIElements
{
    public class UISequenceStep : UITextElement
    {
        public UISequenceStep(string text) : base(text, readOnly: true)
        {
            Status = ElementStatusEnum.Normal.GetDescription();
        }

        #region Props

        #endregion

        #region States
        private string _status;
        [JsonIgnore]
        public string Status
        {
            get => _status;
            set
            {
                SetStatesProperty(ref _status, value, nameof(Status));
            }
        }

        #endregion
    }
}
