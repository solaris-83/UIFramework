using BaseCustomApp.Helpers;
using ScriptLibraries.Data.Interfaces;
using UIFramework.Interfaces;
using UIFramework.UIElements;

namespace UIFramework.SpecializedPopups
{
    public class UIWaitPopup : UIPopup
    {
        private UILabel _paragraph;
        public UIWaitPopup(IUIContext context) : this("TRANSL_MODAL_WAIT", context)
        {
        }

        public UIWaitPopup(string idStr, IUIContext context) : base(isModal: false, context)
        {
            var section = new UISection(2, 1, Context);
            var loader = new UILoader("spinner");
            section.Add(loader, 0, 0);
            _paragraph = section.CreateParagraph(idStr, css: ElementStatusEnum.Valid.GetDescription(), "");
            section.Add(_paragraph, 1, 0);
            Add(section);
        }

        public bool UpdateText(string newIdStr)
        {
            if (_paragraph != null)
            {
                _paragraph.UpdateText(newIdStr);
                return true;
            }
            return false;
        }
    }
}
