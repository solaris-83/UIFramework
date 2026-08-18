using BaseCustomApp.Helpers;
using UIFramework.Interfaces;
using UIFramework.Interfaces.Adapters;

namespace UIFramework.SpecializedPages
{
    public sealed class PageResult : SpecializedPage, IPageAdapter
    {
        public PageResult(IUIContext uicontext) : base("disclaimer", uicontext)
        {
            SetTitle("title", "BCA_RESULT", ElementStatusEnum.Result.GetDescription());
            AddButton("EXIT", true, ElementStatusEnum.Danger.GetDescription());
        }
    }
}
