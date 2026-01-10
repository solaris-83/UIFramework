

using Newtonsoft.Json;
using System.Collections.Generic;

namespace UIFrameworkDotNet
{
    public class UIButton : UIElement
    {
        public UIButton(string tag, bool enabled)
        {
            Tag = tag;
            Text = tag;
            Enabled = enabled;
        }

        public UIButton(string tag,  bool enabled, string style)
        {
            Tag = tag;
            Text = tag;
            Enabled = enabled;
            Style = new Style
            {
                Appearance = style
            };
        }

        public UIButton(string tag, bool enabled, string style, string translation)
        {
            Tag = tag;
            Text = translation;
            Enabled = enabled;
            Style = new Style
            {
                Appearance = style
            };
        }

        #region Props
        private string _text;
        [JsonIgnore]
        public string Text
        {
            get => _text;
            set 
            {
                SetPropsProperty(ref _text, value, nameof(Text));
            }
        }

        #endregion
    }
}
