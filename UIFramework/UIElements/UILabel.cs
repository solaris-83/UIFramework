

namespace UIFramework.UIElements
{
    public class UILabel : UITextElement
    {
        public UILabel(string text) : base(text, readOnly: false)
        {
        }
        public UILabel(string text, bool readOnly) : base(text, readOnly: readOnly)
        {
        }
    }
}
