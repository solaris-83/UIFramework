using Newtonsoft.Json;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UITextElement : UIElement, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();

        [JsonIgnore]
        protected IUIContext Context { get; private set; }

        public UITextElement(string text, bool readOnly)
        {
            IsReadOnly = readOnly;
            Text = text;
        }

        #region Managed Translation for Text
        public void AttachContext(IUIContext uIContext)
        {
            Context = uIContext;
            _translationBinding.AttachTranslator(uIContext.Translator);
            ResolveTranslationBindings();
        }

        /// <summary>
        /// Elenco dei Resolve da fare quando viene attachato il contesto. Un resolve per ogni property che supporta la traduzione
        /// </summary>
        public void ResolveTranslationBindings()
        {
            ResolveTextProperty();
        }

        private void ResolveTextProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations(nameof(Text));
            if (IsReadOnly)
                Props["text"] = translatedText;
            else
            {
                States["text"] = translatedText;
                OnPropertyChanged(Id, nameof(Text), translatedText);
            }
        }

        private bool _isReadOnly;
        [JsonIgnore]
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set => SetStatesProperty(ref _isReadOnly, value, nameof(IsReadOnly));
        }

        private string _text;
        [JsonIgnore]
        public virtual string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    var translatedText = _translationBinding.SetKeysAndTryResolveTranslations(nameof(Text), _text);
                    if (IsReadOnly)
                        Props["text"] = translatedText;
                    else
                    {
                        States["text"] = translatedText;
                        OnPropertyChanged(Id, nameof(Text), translatedText);
                    }
                }
            }
        }

        public void UpdateText(string newText)
        {
            Text = newText;
        }

        #endregion
    }
}
