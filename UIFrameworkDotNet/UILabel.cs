

using Newtonsoft.Json;

namespace UIFrameworkDotNet
{
    public class UILabel : UITextElement
    {
        public UILabel(string text) : base(text)
        {
        }
    }

    //public class LabelTextChangedCommand : ICommand
    //{
    //    private readonly UILabel _label;

    //    public LabelTextChangedCommand(UILabel label)
    //    {
    //        _label = label;
    //    }

    //    public void Execute(object newValue)
    //    {
    //        _label.Text = newValue.ToString();
    //    }
    //}
}
