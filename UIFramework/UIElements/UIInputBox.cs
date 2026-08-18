using Newtonsoft.Json;
using UIFramework.Interfaces;

namespace UIFramework.UIElements
{
    public class UIInputBox : UIInputBoxBase, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();

        public UIInputBox(bool isReadOnly) : base()
        {
            IsReadOnly = isReadOnly;
            Placeholder = "";
            Description = "";
            Left = "";
            Right = "";
        }

        public void AttachContext(IUIContext uIContext)
        {
            base.AttachContext(uIContext);
            _translationBinding.AttachTranslator(uIContext.Translator);
            ResolveTranslationBindings();
        }

        public void ResolveTranslationBindings()
        {
            ResolvePlaceholderProperty();
            ResolveLeftProperty();
            ResolveRightProperty();
        }

        private void ResolvePlaceholderProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations(nameof(Placeholder));
            Props["placeholder"] = translatedText;
        }

        private void ResolveLeftProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations(nameof(Left));
            Props["left"] = translatedText;
        }

        private void ResolveRightProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations(nameof(Right));
            Props["right"] = translatedText;
        }

        #region States

        private bool _isReadOnly;
        [JsonIgnore]
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set => SetStatesProperty(ref _isReadOnly, value, nameof(IsReadOnly));
        }

        #endregion

        #region Props

        private string _placeholder;
        [JsonIgnore]
        public string Placeholder
        {
            get => _placeholder;
            set
            {
                if (_placeholder != value)
                {
                    _placeholder = value;
                    var translatedText = _translationBinding.SetKeysAndTryResolveTranslations(nameof(Placeholder), _placeholder);
                    Props["placeholder"] = translatedText;
                }
            }
        }

        private string _left;
        [JsonIgnore]
        public string Left
        {
            get => _left;
            set
            {
                if (_left != value)
                {
                    _left = value;
                    var translatedText = _translationBinding.SetKeysAndTryResolveTranslations(nameof(Left), _left);
                    Props["left"] = translatedText;
                }
            }
        }

        private string _right;
        [JsonIgnore]
        public string Right
        {
            get => _right;
            set
            {
                if (_right != value)
                {
                    _right = value;
                    var translatedText = _translationBinding.SetKeysAndTryResolveTranslations(nameof(Right), _right);
                    Props["right"] = translatedText;
                }
            }
        }

        #endregion
    }

   
}
