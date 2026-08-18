using Newtonsoft.Json;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIHeadingElement : UIElement, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();

        [JsonIgnore]
        protected IUIContext Context { get; private set; }

        private string _title;
        [JsonIgnore]
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    var translatedText = _translationBinding.SetKeysAndTryResolveTranslations(nameof(Title), _title);
                    Props["title"] = translatedText;
                }
            }
        }

        private string _subTitle;
        [JsonIgnore]
        public string SubTitle
        {
            get => _subTitle;
            set
            {
                if (_subTitle != value)
                {
                    _subTitle = value;
                    var translatedText = _translationBinding.SetKeysAndTryResolveTranslations(nameof(SubTitle), _subTitle);
                    Props["subTitle"] = translatedText;
                }
            }
        }

        public void AttachContext(IUIContext context)
        {
            Context = context;
            _translationBinding.AttachTranslator(Context.Translator);
            ResolveTranslationBindings();
        }

        public void ResolveTranslationBindings()
        {
            ResolveTitleProperty();
            ResolveSubTitleProperty();
        }

        private void ResolveTitleProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations(nameof(Title));
            Props["title"] = translatedText;
        }

        private void ResolveSubTitleProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations(nameof(SubTitle));
            Props["subTitle"] = translatedText;
        }
    }
}
