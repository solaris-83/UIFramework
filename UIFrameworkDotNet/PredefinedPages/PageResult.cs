

namespace UIFrameworkDotNet.PredefinedPages
{
    public sealed class PageResult : PredefinedPage
    {
        public PageResult(IUIContext uicontext) : base("disclaimer", uicontext)
        {
            SetTitle("title", "Result", "result");
           // AddTab("disclaimer", 1, 1);
            AddButton("EXIT", true, "danger");
        }
    }
}
