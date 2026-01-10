

using System.Security.Cryptography;

namespace UIFrameworkDotNet.PredefinedPages
{
    // Le PredefinedPage sono pagine un po' più avanzate della Page standard: viene automaticamente creato uno UITab e una unica sezione all'interno del CurrentTab.
    // Vengono utilizzate dalle Pagine custom es disclaimer, menu, result, ecc...
    public class PredefinedPage : Page
    {
        public PredefinedPage(string identifier) : base()
        {
            var tab = new UITab(identifier, 1, 1);
            base.TabControl.Add(tab);
            base.TabControl.CurrentTab = tab;
            UISection section = new UISection(1, 1);
            base.TabControl.CurrentTab.Add(section);
            base.TabControl.CurrentTab.CurrentSection = section;
        }

        public UIImage AddImage(string imageName)
        {
            return base.TabControl.CurrentTab.CurrentSection.AddImage(imageName);
        }

        public bool UpdateImage(string imageId, string newImageName)
        {
            return base.TabControl.CurrentTab.CurrentSection.UpdateImage(imageId, newImageName);
        }

        public UILabel AddBulletedItem(string idStr)
        {
            return base.TabControl.CurrentTab.CurrentSection.AddBulletedItem(idStr);
        }

        public bool UpdateBulletedItem(string itemId, string newIdStr)
        {
            return base.TabControl.CurrentTab.CurrentSection.UpdateBulletedItem(itemId, newIdStr);
        }

        // "list-item-ordered" è in Style.Appearance
        // index è in Tag
        public UILabel AddOrderedItem(string idStr, int index)
        {
            return base.TabControl.CurrentTab.CurrentSection.AddOrderedItem(idStr, index);
        }

        public UILabel AddOrderedItem(string idStr, string style, int index)
        {
            return base.TabControl.CurrentTab.CurrentSection.AddOrderedItem(idStr, style, index);
        }

        public UILabel AddOrderedItem(string idStr, string style)
        {
            return base.TabControl.CurrentTab.CurrentSection.AddOrderedItem(idStr, style);
        }

        public UILabel AddOrderedItem(string idStr)
        {
            return AddOrderedItem(idStr, "");
        }

        public UILabel AddParagraph(string idStr)
        {
            return AddParagraph(idStr, "paragraph", "");
        }

        public UILabel AddParagraph(string idStr, string style, string color) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        {
            return base.TabControl.CurrentTab.CurrentSection.AddParagraph(idStr, style, color);
        }

        public bool UpdateParagraph(string paragraphId, string newIdStr) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        {
            return base.TabControl.CurrentTab.CurrentSection.UpdateParagraph(paragraphId, newIdStr);
        }

        public bool UpdateParagraph(string paragraphId, string newIdStr, string style, string color) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        {
            return base.TabControl.CurrentTab.CurrentSection.UpdateParagraph(paragraphId, newIdStr, style, color);
        }
    }
}
