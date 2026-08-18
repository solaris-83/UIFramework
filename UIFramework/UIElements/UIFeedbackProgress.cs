using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIFeedbackProgress : UIElement, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();

        [JsonIgnore]
        protected IUIContext Context { get; private set; }

        public UIFeedbackProgress()
        {
            LookAndFeel = ElementStatusEnum.Success.GetDescription();
        }

        public UIFeedbackProgress(int percentage) : this()
        {
            ProgressValue = new ProgressValue(percentage, "");
        }

        public UIFeedbackProgress(double percentage) : this()
        {
            ProgressValue = new ProgressValue(percentage, "");
        }

        public UIFeedbackProgress(int percentage, string idStr) : this()
        {
            ProgressValue = new ProgressValue(percentage, idStr);
        }

        public UIFeedbackProgress(double percentage, string idStr) : this()
        {
            ProgressValue = new ProgressValue(percentage, idStr);
        }

        public void AttachContext(IUIContext uIContext)
        {
            Context = uIContext;
            _translationBinding.AttachTranslator(uIContext.Translator);
            ResolveTranslationBindings();
        }

        public void ResolveTranslationBindings()
        {
            ResolveTextProperty();
        }

        private void ResolveTextProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations("ProgressValue.Text");
            if (States["progressValue"] is ProgressValue prgVal)
            {
                prgVal.Text = translatedText;
            }
        }

        public bool SendUpdate(double perc, string newIdStr)
        {
            ProgressValue = new ProgressValue(perc, newIdStr);
            return true;
        }

        public bool SendUpdate(string newIdStr)
        {
            ProgressValue = new ProgressValue(ProgressValue.Percentage, newIdStr);
            return true;
        }

        public bool SendUpdate(double perc)
        {
            ProgressValue = new ProgressValue(perc, ProgressValue.Text);
            return true;
        }

        #region States

        private ProgressValue _progressValue;
        [JsonIgnore]
        public ProgressValue ProgressValue
        {
            get => _progressValue;
            set
            {
                if (_progressValue == null || !_progressValue.Equals(value))
                {
                    _progressValue = new ProgressValue(value.Percentage, _translationBinding.SetKeysAndTryResolveTranslations("ProgressValue.Text", value.Text));
                    States["progressValue"] = _progressValue;
                    OnPropertyChanged(Id, nameof(ProgressValue), _progressValue);
                }
            }
        }

        // Valori utilizzabili "success", "error", "warning"
        private string _lookAndFeel;
        [JsonIgnore]
        public string LookAndFeel
        {
            get => _lookAndFeel;
            set => SetStatesProperty(ref _lookAndFeel, value, nameof(LookAndFeel));
        }

        #endregion
    }
}
