using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    // Usato per gestire i differenti overlay supportati dalla UI (es. modali, popup, ecc.)
    public class UIOverlay : ContainerElement
    {
        public UIOverlay(IUIContext context) : base(context)
        {
        }
    }
}
