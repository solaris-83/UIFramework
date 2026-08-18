using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using UIFramework.Interfaces;

namespace UIFramework.UIElements
{
    public class UIStatus : UIHeadingElement, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();
        public UIStatus()
        {
            LookAndFeel = ElementStatusEnum.Normal.GetDescription();
            Title = "";
            SubTitle = "";
            SetAppearance(ElementStatusEnum.Led.GetDescription());
        }

        // Valori utilizzabili "success", "error", "warning"
        private string _lookAndFeel;
        [JsonIgnore]
        public string LookAndFeel
        {
            get => _lookAndFeel;
            set => SetStatesProperty(ref _lookAndFeel, value, nameof(LookAndFeel));
        }

        private string _text;
        [JsonIgnore]
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    var translatedText = _translationBinding.SetKeysAndTryResolveTranslations(nameof(Text), _text);
                    States["text"] = translatedText;
                    OnPropertyChanged(Id, nameof(Text), translatedText);
                }
            }
        }

        /// <summary>
        /// Execute base AttachContext and
        /// </summary>
        /// <param name="uIContext"></param>
        public new void AttachContext(IUIContext uIContext)
        {
            base.AttachContext(uIContext);
            _translationBinding.AttachTranslator(Context.Translator);
            ResolveTranslationBindings();
        }

        public new void ResolveTranslationBindings()
        {
            base.ResolveTranslationBindings(); // Resolve Title and SubTitle translation as being in base class
            ResolveTextProperty(); // Then resolve Text property of UIStatus class
        }

        private void ResolveTextProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations(nameof(Text));
            States["text"] = translatedText;
            OnPropertyChanged(Id, nameof(Text), translatedText);
        }

        public bool Update(string msg, string lookAndFeel)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                Text = msg;
            }
            LookAndFeel = lookAndFeel;
            return true;
        }

        public bool Update(string lookAndFeel)
        {
            return Update("", lookAndFeel);
        }
    }
}
