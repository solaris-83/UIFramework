using BaseCustomApp.Helpers;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using UIFramework.Interfaces;
using UIFramework.Reactive;

namespace UIFramework.UIElements.Base
{
    public class UIElement : INotifyPropertyChanged, IDisposable
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UIElement));
        private List<Binding> _bindings = new List<Binding>();

        public UIElement()
        {
            Tag = null;
            Type = GetType().Name;
            Style = new Style();
            Visible = true;
            Enabled = true;
            GridPosition = new GridPosition(0, 0);
        }

        // Useful method to set apperance in Style property. It can be overriden
        public virtual bool SetAppearance(string appearance)
        {
            var ok = EnumExtensions.GetEnumValueFromDescription(appearance, out ElementStatusEnum _);
            if (ok)
            {
                Style.Appearance = appearance;
                return true;
            }
            return false;
        }

        protected bool SetStatesProperty<T>(ref T field, T value, string propertyName)
        {
            var propertyNameToCamelCase = Char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
            return SetProperty<T>(ref field, value, () => States[propertyNameToCamelCase] = value, propertyName);
        }

        protected bool SetProperty<T>(ref T field, T value, Action actionBeforeNotify, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value) && States.ContainsKey(propertyName.ToCamelCase()))
                return false;

            field = value;
            actionBeforeNotify?.Invoke();
            OnPropertyChanged(Id, propertyName, value);
            return true;
        }

        protected bool SetPropsProperty<T>(ref T field, T value, string propertyName)
        {
            return SetProperty<T>(ref field, value, () => Props[propertyName.ToCamelCase()] = value, propertyName);
        }

        // Backing field for the event delegate
        private Action<object, UIPropertyChange> _propertyChanged;
        // Custom event with controlled add/remove
        public event Action<object, UIPropertyChange> PropertyChanged
        {
            add
            {
                if (_propertyChanged != null)
                {
                    throw new InvalidOperationException($"PropertyChanged event already has a subscriber for {Type} and {Id}.");
                }
                _propertyChanged += value;
            }
            remove
            {
                // Properly unsubscribe the delegate instead of overwriting it
                _propertyChanged -= value;
            }
        }

        [JsonIgnore]
        public string ParentId { get; internal set; }

        #region Props
        private object _tag;
        [JsonIgnore]
        public object Tag
        {
            get => _tag;
            set  
            {
                SetPropsProperty(ref _tag, value, nameof(Tag));
            }
        }

        private string _type;
        [JsonIgnore]
        public string Type
        {
            get => _type;
            set
            {
                SetPropsProperty(ref _type, value, nameof(Type));
            }
        }

        private Style _style;
        [JsonIgnore]
        public Style Style
        {
            get => _style;
            set
            {
                SetPropsProperty(ref _style, value, nameof(Style));
            }
        }

        private GridPosition _gridPosition;

        [JsonIgnore]
        public GridPosition GridPosition
        {
            get => _gridPosition;
            set => SetPropsProperty(ref _gridPosition, value, nameof(GridPosition));
        }

        #endregion

        #region States

        private bool _enabled;
        [JsonIgnore]
        public bool Enabled
        {
            get => _enabled; 
            set
            {
                SetStatesProperty(ref _enabled, value, nameof(Enabled));
            }
        }

        private bool _visible;
        [JsonIgnore]
        public bool Visible
        {
            get => _visible;
            set 
            {
                SetStatesProperty(ref _visible, value, nameof(Visible));
            }
        }

        #endregion
        
        public string Id { get; } = Guid.NewGuid().ToString();
        public Dictionary<string, object> Props { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> States { get; } = new Dictionary<string, object>();

        #region BINDING REACTIONS
        public void AddBinding(Binding binding)
        {
            if (_bindings == null)
                _bindings = new List<Binding>();
            _bindings.Add(binding);
        }

        #endregion

        public virtual void UpdateStates(Dictionary<string, object> newStates)
        {
            if (newStates == null)
                return;

            UpdateStates(states =>
            {
                var elementType = this.GetType();
                foreach (var kvp in newStates)
                {
                    //PropertyHelper.TrySetPropertyByNameRecursive(this, kvp.Key.ToPascalCase(), kvp.Value);
                    //var property =  PropertyHelper.FindPropertyRecursive(elementType, kvp.Key.ToPascalCase());
                    var property = elementType.GetProperty(kvp.Key.ToPascalCase(), BindingFlags.Public | BindingFlags.Instance);
                    if (property != null && property.CanWrite)
                    {
                        try
                        {
                            var convertedValue = Convert.ChangeType(kvp.Value, property.PropertyType);
                            property.SetValue(this, convertedValue);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Property {kvp.Key} could not be updated with value {kvp.Value}", ex);
                        }
                    }
                    else
                    {
                        _logger.Error($"Property {kvp.Key} is either NULL or cannot be written");
                    }
                }
            });
        }

        public virtual void UpdateStates(Action<Dictionary<string, object>> updater)
        {
            updater(States);
        }

        protected virtual void OnPropertyChanged(string id, string propertyName, object propertyValue)
        {
            _propertyChanged?.Invoke(this, new UIPropertyChange(id, propertyName, propertyValue));
            _bindings?.ForEach(b => b.Evaluate());
        }

        public virtual void Dispose()
        {
            
        }

        override public string ToString()
        {
            return $"{Type} (Id: {Id})";
        }
    }


    public class Style
    {
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public string Appearance { get; set; }
        public string CssClassName { get; set; } // L'idea è che CssClassName possa fare override di Appearance e ForegroundColor. Da decidere se usare una stringa che permetta di concatenare tramite spazio vuoto le varie classi css.

        public Style()
        {
            Appearance = "";
            BackgroundColor = ""; // "#FFFFFF";
            ForegroundColor = ""; //"#000000";
            CssClassName = "";
        }

        public override bool Equals(object obj)
        {
            return obj is Style style &&
                   BackgroundColor == style.BackgroundColor &&
                   ForegroundColor == style.ForegroundColor &&
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
                hash = hash * 23 + (Appearance != null ? Appearance.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
