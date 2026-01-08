
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;

namespace UIFramework
{
    public class UIElement : INotifyPropertyChanged
    {
        public UIElement()
        {
            Tag = null;
            Type = GetType().Name;
            Style = new Style();
            Visible = true;
            Enabled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        public string ParentId { get; internal set; }

        private object _tag;
        [JsonIgnore]
        public object Tag
        {
            get => _tag;
            set  
            { 
                if (_tag == value) return;
                _tag = value;
                Props["tag"] = value; 
                OnPropertyChanged(nameof(Tag)); 
            }
        }

        private string _type;
        [JsonIgnore]
        public string Type
        {
            get => _type;
            set
            {
                if (_type == value) return;
                _type = value;
                Props["type"] = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private bool _enabled;
        [JsonIgnore]
        public bool Enabled
        {
            get => _enabled; 
            set
            {
                if (_enabled == value) return;
                _enabled = value;
                States["enabled"] = value; 
                OnPropertyChanged(nameof(Enabled)); 
            }
        }

        private bool _visible;
        [JsonIgnore]
        public bool Visible
        {
            get => _visible;
            set 
            { 
                if (_visible == value) return;
                _visible = value;
                States["visible"] = value; 
                OnPropertyChanged(nameof(Visible)); 
            }
        }

        private Style _style;
        [JsonIgnore]
        public Style Style
        {
            get => _style;
            set 
            { 
                if (_style!= null && _style.Equals(value)) return;
                _style = value;
                Props["style"] = value; 
                OnPropertyChanged(nameof(Style)); 
            }
        }

        public string Id { get; init; } = Guid.NewGuid().ToString();

        public Dictionary<string, object> Props { get; } = new();
        public Dictionary<string, object> States { get; } = new();

        public virtual void UpdateStates(Dictionary<string, object> newStates)
        {
            if (newStates == null)
                return;

            UpdateStates(states =>
            {
                foreach (var kv in newStates)
                    States[kv.Key] = kv.Value;
            });
        }

        public virtual void UpdateStates(Action<Dictionary<string, object>> updater)
        {
            updater(States);
        }

        public virtual void UpdateProps(Dictionary<string, object> newProps)
        {
            if (newProps == null)
                return;

            UpdateProps(props =>
            {
                foreach (var kv in newProps)
                    props[kv.Key] = kv.Value;
            });
        }

        public virtual void UpdateProps(Action<Dictionary<string, object>> updater)
        {
            updater(Props);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class UIElementEnabledChangedCommand : ICommand
        {
            private readonly UIElement _element;

            public UIElementEnabledChangedCommand(UIElement element)
            {
                _element = element;
            }

            public void Execute(object newValue)
            {
                _element.Enabled = Convert.ToBoolean(newValue);
            }
        }

        public class UIElementVisibleChangedCommand : ICommand
        {
            private readonly UIElement _element;

            public UIElementVisibleChangedCommand(UIElement element)
            {
                _element = element;
            }

            public void Execute(object newValue)
            {
                _element.Visible = Convert.ToBoolean(newValue);
            }
        }
    }

    public class Style
    {
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public string FontFamily { get; set; }
        public int FontSize { get; set; }
        public string Layout { get; set; }

        public Style()
        {
            Layout = "";
            BackgroundColor = ""; // "#FFFFFF";
            ForegroundColor = ""; //"#000000";
            FontFamily = ""; //"Arial";
            FontSize = 12;
        }

        public override bool Equals(object? obj)
        {
            return obj is Style style &&
                   BackgroundColor == style.BackgroundColor &&
                   ForegroundColor == style.ForegroundColor &&
                   FontFamily == style.FontFamily &&
                   FontSize == style.FontSize &&
                   Layout == style.Layout;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BackgroundColor, ForegroundColor, FontFamily, FontSize, Layout);
        }
    }
}
