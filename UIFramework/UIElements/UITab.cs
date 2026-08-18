using Newtonsoft.Json;
using System;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UITab : GridContainerElement, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();

        public UITab(IUIContext context) : base(context) { }

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

        public UITab(int rows, int columns, IUIContext context) : base(context)
        {
            Grid = new Grid(rows, columns);
        }

        public UITab(string tag, int rows, int columns, IUIContext context) : this(rows, columns, context)
        {
            Tag = tag;
            Title = tag;
        }
        

        public bool Add(UIElement element, int row, int column)
        {
            if (element is UISection section)
            {
                section.GridPosition.RowIndex = row;
                section.GridPosition.ColumnIndex = column;
                Add(section);
            }
            else
            {
                throw new InvalidOperationException("Only UISection elements can be added to UITab.");
            }
            return true;
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
    }
}
