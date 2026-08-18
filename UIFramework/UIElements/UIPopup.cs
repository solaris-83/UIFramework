using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using System;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIPopup : ContainerElement, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();

        [JsonIgnore]
        protected IUIContext Context { get; private set; }
        public UIPopup(IUIContext context) : base(context)
        {
            Context = context;
        }
        public UIPopup(bool isModal, IUIContext context) : this(context)
        {
            IsModal = isModal;
        }

        [JsonIgnore]
        public PopupResult Result { get; private set; }

        private bool _isModal;
        [JsonIgnore]
        public bool IsModal
        {
            get => _isModal;
            set => SetPropsProperty(ref _isModal, value, nameof(IsModal));
        }

        #region Translation properties
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

        #endregion

        public UIButton AddButton(string tag, string idStr, bool isEnabled)
        {
            return AddButton(tag, idStr, ElementStatusEnum.Primary.GetDescription(), isEnabled);
        }

        public UIButton AddButton(string tag, string idStr, string style, bool isEnabled)
        {
            var button = new UIButton(tag, isEnabled, style, idStr);
            button.Clicked += OnButtonClicked;
            Add(button);
            return button;
        }

        internal virtual void OnButtonClicked(object sender, EventArgs e)
        {
            if (sender is UIButton button)
            {
                Result = new PopupResult()
                {
                    CommandName = button.Tag.ToString()
                };
            }
        }
    }
}
