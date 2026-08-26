using ScriptLibraries.Data.Interfaces;
using UIFramework.Interfaces;
using UIFramework.UIElements;

namespace UIFramework.SpecializedPages
{
    // Le PredefinedPage sono pagine un po' più avanzate della Page standard: viene automaticamente creato uno UITab e una unica sezione all'interno del CurrentTab.
    // Vengono utilizzate dalle Pagine custom es disclaimer, menu, result, ecc...
    public abstract class SpecializedPage : Page
    {
        protected UISection SingleSection;

        public SpecializedPage(string identifier, IUIContext uiContext) : base(uiContext)
        {
            Tag = identifier;
            var tab = new UITab(identifier, 1, 1, Context);
            base.TabControl.Add(tab);
            base.TabControl.CurrentTab = tab;
            SingleSection = new UISection(1, 1, uiContext);
            base.TabControl.CurrentTab.Add(SingleSection);
        }

        public UIImage AddImage(string imageName)
        {
            return SingleSection.AddImage(imageName);
        }

        public bool UpdateImage(string imageId, string newImageName)
        {
            return SingleSection.UpdateImage(imageId, newImageName);
        }

        public UILabel AddBulletedItem(string idStr)
        {
            return SingleSection.AddBulletedItem(idStr);
        }

        public bool UpdateBulletedItem(string itemId, string newIdStr)
        {
            return SingleSection.UpdateBulletedItem(itemId, newIdStr);
        }

        public bool UpdateOrderedItem(string itemId, string newIdStr)
        {
            return SingleSection.UpdateOrderedItem(itemId, newIdStr);
        }

        // "list-item-ordered" è in Style.Appearance
        // index è in Tag
        public UILabel AddOrderedItem(string idStr, int index)
        {
            return SingleSection.AddOrderedItem(idStr, index);
        }

        public UILabel AddOrderedItem(string idStr, string style, int index)
        {
            return SingleSection.AddOrderedItem(idStr, style, index);
        }

        public UILabel AddOrderedItem(string idStr, string letter)
        {
            return SingleSection.AddOrderedItem(idStr, letter);
        }

        public UILabel AddOrderedItem(string idStr)
        {
            return SingleSection.AddOrderedItem(idStr);
        }

        public UILabel AddParagraph(string idStr)
        {
            return AddParagraph(idStr, "paragraph", "");
        }

        public UILabel AddParagraph(string idStr, string css, string color)
        {
            return SingleSection.AddParagraph(idStr, css, color);
        }

        public bool UpdateParagraph(string paragraphId, string newIdStr)
        {
            return SingleSection.UpdateParagraph(paragraphId, newIdStr);
        }

        // Questo non serve, Style è immodificabile (è una Props al momento)
        //public bool UpdateParagraph(string paragraphId, string newIdStr, string style, string color)
        //{
        //    return _singleSection.UpdateParagraph(paragraphId, newIdStr, style, color);
        //}
    }
}
