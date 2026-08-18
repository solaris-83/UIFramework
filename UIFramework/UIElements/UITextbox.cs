using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using UIFramework.Interfaces;

namespace UIFramework.UIElements
{
    public class UITextBox : UIInputBox, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();

        public UITextBox(string text) : base(isReadOnly: false)
        {
            Text = text;
        }

        public UITextBox() : this("")
        {
            
        }

        [JsonIgnore]
        public int Length => Text.Length;

        public new void AttachContext(IUIContext uIContext)
        {
            base.AttachContext(uIContext);
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

        #region States

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
                    if (IsReadOnly)
                        Props["text"] = translatedText;
                    else
                    {
                        States["text"] = translatedText;
                        OnPropertyChanged(Id, nameof(Text), translatedText);
                    }
                    ApplyValidationRules(value);
                }
            }
        }

        #endregion

       public string ToHex() // TODO MARCO validarli con alberto
       {
           return Conversions.ToHexString(Text);
       }

       public byte[] ToBytes() // TODO MARCO validarli con alberto
       {
           return Conversions.FromHexToBytes(ToHex());
       }
    }
}
