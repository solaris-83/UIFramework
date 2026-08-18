
using Newtonsoft.Json;
using System;

namespace UIFramework.UIElements
{
    public class UIChoice : UITextElement
    {
        public UIChoice(string text, bool isChecked, bool isEnabled) : base(text, readOnly: true)
        {
            Tag = default;
            Checked = isChecked;
            Enabled = isEnabled;
        }

        public UIChoice(string text, bool isChecked, bool isEnabled, string tag) : base(text, readOnly: true)
        {
            Tag = tag;
            Checked = isChecked;
            Enabled = isEnabled;
        }

        public UIChoice(string text, bool isEnabled, string tag) : base(text, readOnly: true)
        {
            Tag = tag;
            Checked = false;
            Enabled = isEnabled;
        }
        
        #region Props
       

        #endregion

        #region States

        private bool _checked;
        [JsonIgnore]
        public bool Checked
        {
            get => _checked;
            set 
            { 
                SetProperty(ref _checked, value, () =>
                {
                    States["checked"] = value;
                    OnSelectionChanged();
                }, nameof(Checked));
            }
        }

        #endregion

        #region Events

        private EventHandler _selectionChanged;
        public event EventHandler SelectionChanged
        {
            add
            {
                if (_selectionChanged != null)
                {
                    throw new InvalidOperationException("SelectionChanged event already has a subscriber.");
                }

                _selectionChanged += value;
            }
            remove
            {
                _selectionChanged -= value;
            }
        }

        public void OnSelectionChanged()
        {
            _selectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public override void Dispose()
        {
            base.Dispose();
            _selectionChanged = null; // Unsubscribe all handlers
        }

        #endregion
    }
}
