using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework.PredefinedPages
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
