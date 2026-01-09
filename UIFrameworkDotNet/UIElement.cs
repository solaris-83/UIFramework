
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UIFrameworkDotNet
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
                if (_style != null && _style.Equals(value)) return;
                _style = value;
                Props["style"] = value;
                OnPropertyChanged(nameof(Style));
            }
        }

        public string Id { get; } = Guid.NewGuid().ToString();

        public Dictionary<string, object> Props { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> States { get; } = new Dictionary<string, object>();

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
    public class Style
    {
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
      //  public string FontFamily { get; set; }
      //  public int FontSize { get; set; }
        public string Appearance { get; set; }
        public string CssClassName { get; set; } // L'idea è che CssClassName possa fare override di Appearance e ForegroundColor. Da decidere se usare una stringa che permetta di concatenare tramite spazio vuoto le varie classi css.

        public Style()
        {
            Appearance = "";
            BackgroundColor = ""; // "#FFFFFF";
            ForegroundColor = ""; //"#000000";
            CssClassName = "";
           // FontFamily = ""; //"Arial";
           // FontSize = 12;
        }

        public override bool Equals(object obj)
        {
            return obj is Style style &&
                   BackgroundColor == style.BackgroundColor &&
                   ForegroundColor == style.ForegroundColor &&
                   // FontFamily == style.FontFamily &&
                   // FontSize == style.FontSize &&
                   Appearance == style.Appearance &&
                   CssClassName == style.CssClassName;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (BackgroundColor != null ? BackgroundColor.GetHashCode() : 0);
                hash = hash * 23 + (ForegroundColor != null ? ForegroundColor.GetHashCode() : 0);
                hash = hash * 23 + (CssClassName != null ? CssClassName.GetHashCode() : 0);
                //   hash = hash * 23 + (FontFamily != null ? FontFamily.GetHashCode() : 0);
                //  hash = hash * 23 + FontSize.GetHashCode();
                hash = hash * 23 + (Appearance != null ? Appearance.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
