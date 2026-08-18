using ScriptLibraries.Data.Interfaces;
using UIFramework.Interfaces;
using UIFramework.UIElements;

namespace UIFramework.SpecializedPages
{
    public class PageCountdown : SpecializedPage
    {
        public PageCountdown(string idStr, int waitTime, IUIContext uicontext) : base("countdown", uicontext)
        {
            AddButtonStop();
            var loader = new UILoader("spinner-big");
            SingleSection.Add(loader);
            if (!string.IsNullOrEmpty(idStr))
            {
                SingleSection.AddParagraph(idStr);
            }
            AddFeedbackCountdown(waitTime, isManual: false);
        }
    }
}