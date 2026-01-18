using Newtonsoft.Json;
using ScriptLibraries.Data.Interfaces;
using System;

namespace UIFrameworkDotNet
{
    public class UITextElement : UIElement
    {
        public UITextElement(string text)
        {
            SetTextKey(text);
            //Text = text;
        }

        internal IUIContext Context { get; private set; }

        private string _textKey;
        protected void SetTextKey(string key)
        {
            _textKey = key;

            // se il context esiste già → traduci subito
            if (Context != null)
                TranslateText();
        }

        private string _text;
        [JsonIgnore]
        public string Text
        {
            get => _text;
            private set => SetStatesProperty(ref _text, value, nameof(Text));
        }

        public void UpdateText(string newText)
        {
            SetTextKey(newText);
            // L'assegnamento a Text viene fatto dalla SetTextKey stessa se il Context è già stato inizializzato altrimenti viene fatta posticipata alla ShowAndWait/ShowAndContinue
            //Text = newText;
        }

        internal void AttachContext(IUIContext context)
        {
            // Se il Context è già stato caricato allora va bene quella istanza
            if (Context != null)
                return;

            Context = context;

            // hook fondamentale
            TranslateText();
        }

        private void TranslateText()
        {
            if (_textKey == null || Context == null)
                return;

            Text = Context.Translator.CurrentTranslations.GetLocalOrDefault(_textKey);
        }
    }

    //public class UITextElementTextChangedCommand : ICommand
    //{
    //    private readonly UITextElement _textElement;

    //    public UITextElementTextChangedCommand(UITextElement textElement)
    //    {
    //        _textElement = textElement;
    //    }

    //    public void Execute(object newValue)
    //    {
    //        //_textElement.UpdateText(_translationService.CurrentTranslations.GetLocalOrDefault(newValue.ToString()));
    //        _textElement.UpdateText(newValue.ToString());
    //    }
    //}
}
