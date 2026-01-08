

namespace UIFrameworkDotNet.PredefinedPages
{
    public sealed class PageResult : Page
    {
        public PageResult() : base()
        {
            SetTitle("Result", "result");
            AddTab("disclaimer", 1, 1);
            AddButton("EXIT", true, "danger");
        }
    }
}
