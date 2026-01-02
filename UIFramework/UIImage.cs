using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework
{
    public class UIImage : UIElement
    {
        public UIImage(string src)
        {
            Props["src"] = src;
        }

        public string Src
        {
            get => Props["src"]?.ToString();
            set => Props["src"] = value;
        }
    }
}
