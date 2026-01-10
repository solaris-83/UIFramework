

namespace UIFrameworkDotNet.PredefinedPages
{
    public sealed class PageResult : PredefinedPage
    {
        public PageResult() : base("disclaimer")
        {
            SetTitle("title", "Result", "result");
           // AddTab("disclaimer", 1, 1);
            AddButton("EXIT", true, "danger");
        }
    }
}
