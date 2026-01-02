using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework
{
    public class UILabel : UIElement
    {
        public UILabel(string text)
        {
            Props["text"] = text;
        }
    }
}
