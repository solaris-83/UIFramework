using Newtonsoft.Json;

namespace UIFrameworkDotNet
{
    public class UIButton : UITextElement
    {
        public UIButton(string tag, bool enabled) : base(tag)
        {
            Tag = tag;
            Enabled = enabled;
        }

        public UIButton(string tag,  bool enabled, string style) : base(tag)
        {
            Tag = tag;
            Enabled = enabled;
            Style = new Style
            {
                Appearance = style
            };
        }

        public UIButton(string tag, bool enabled, string style, string translation) : base(translation)
        {
            Tag = tag;
            Enabled = enabled;
            Style = new Style
            {
                Appearance = style
            };
        }

        #region Props
        

        #endregion
    }
}
