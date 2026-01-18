using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace UIFrameworkDotNet
{
    internal class UIFeedbackCountdown : UIFeedback
    {
        public UIFeedbackCountdown(int ms, bool isManual) : base("")
        {
            // Da mettere nella classe base
            /*
            Props["style"] = new Style
            {
                Appearance = style
            };
            Tag = $"feedback-{style}";
            Mode = feedbackMode.ToString();
            */
            Totalseconds = ms / 1000;
            IsManual = isManual;
            //  Text = feedbackMode == FeedbackMode.Countdown ? (int)Math.Round(ms / 1000d) + " seconds remaining" : text;
            Remaining = ms / 1000;
            Percentage = 0;
        }
    }
}
