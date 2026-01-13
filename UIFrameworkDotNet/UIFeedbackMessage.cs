
namespace UIFrameworkDotNet
{
    public class UIFeedbackMessage : UIFeedback
    {
        public UIFeedbackMessage(string text) : base(text)
        {
            // Queste vanno messe nella UIFeedback base
            Props["style"] = new Style
            {
                Appearance = $"feedback-message"
            };
            Tag = $"feedback-message";
        }
    }
}
