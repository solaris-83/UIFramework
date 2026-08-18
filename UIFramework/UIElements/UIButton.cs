using System;
using System.Collections.Generic;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIButton : UITextElement
    {
        public UIButton(string tag, bool enabled, string style, string translation) : base(translation, readOnly: true)
        {
            Tag = tag; // IDstr originale, dentro Text ci sarà la traduzione
            Enabled = enabled;
            Style = new Style
            {
                Appearance = style
            };
        }

        private EventHandler _clicked;
        public event EventHandler Clicked
        {
            add 
            { 
                if (_clicked != null)
                {
                    throw new InvalidOperationException("Clicked event already has a subscriber.");
                }
            
                _clicked += value; 
            }
            remove 
            { 
                _clicked -= value; 
            }
        }

        public void OnClicked()
        {
            _clicked?.Invoke(this, EventArgs.Empty);
        }

        public override void Dispose()
        {
            base.Dispose();
            _clicked = null; // Unsubscribe all handlers
        }
    }

    public class ButtonCommand : ICommand
    {
        public UIButton Value { get; set; }

        public ButtonCommand(UIButton value)
        {
            Value = value;
        }

        public virtual void Execute(Dictionary<string, object> newStates)
        {
            Value.OnClicked();
        }
    }
}
