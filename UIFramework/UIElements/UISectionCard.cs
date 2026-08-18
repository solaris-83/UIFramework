using Newtonsoft.Json;
using ScriptLibraries.Data.Interfaces;
using UIFramework.Interfaces;

namespace UIFramework.UIElements
{
    public sealed class UISectionCard : UISection, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();
        public UISectionCard(IUIContext context) : this(1, 1, context)
        {
            
        }

        public UISectionCard(int rows, int columns, IUIContext context) : base(rows, columns, context)
        {

        }

        public void AttachContext(IUIContext context)
        {
            _translationBinding.AttachTranslator(context.Translator);
            ResolveTranslationBindings();
        }

        public void ResolveTranslationBindings()
        {
            ResolveTitleProperty();
        }

        private void ResolveTitleProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations(nameof(Title));
            Props["title"] = translatedText;
        }

        #region Props

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
        #endregion
    }
}
