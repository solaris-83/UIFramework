using System.Collections.Generic;

namespace UIFramework.UIElements.Base
{
    public sealed class ProgressValue
    {
        public ProgressValue(double percentage, string text)
        {
            Percentage = percentage;
            Text = text;
        }

        public double Percentage { get; set; }
        public string Text { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ProgressValue value &&
                   Percentage == value.Percentage &&
                   Text == value.Text;
        }

        public override int GetHashCode()
        {
            int hashCode = 1396827515;
            hashCode = hashCode * -1521134295 + Percentage.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            return hashCode;
        }
    }
}
