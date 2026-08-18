using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIDevice : UIHeadingElement, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();
        private List<string> _predefinedStatuses = new List<string>();
        public UIDevice(string type)
        {
            Title = "";
            SubTitle = "";
            CurrentStatus = new CurrentStatus("", ""); 
            SetAppearance(type);
        }

        #region States
        private CurrentStatus _currentStatus;
        [JsonIgnore]
        public CurrentStatus CurrentStatus
        {
            get => _currentStatus;
            set
            {
                if (_currentStatus == null || !_currentStatus.Equals(value))
                {
                    _currentStatus = new CurrentStatus(value.Tag, _translationBinding.SetKeysAndTryResolveTranslations("CurrentStatus.Text", value.Text));
                    States["currentStatus"] = _currentStatus;
                    OnPropertyChanged(Id, nameof(CurrentStatus), _currentStatus);
                }
            }
        }
        #endregion

        /// <summary>
        /// Execute base AttachContext and
        /// </summary>
        /// <param name="uIContext"></param>
        public new void AttachContext(IUIContext uIContext)
        {
            base.AttachContext(uIContext);
            _translationBinding.AttachTranslator(Context.Translator);
            ResolveTranslationBindings();
        }
        public new void ResolveTranslationBindings()
        {
            base.ResolveTranslationBindings(); // Resolve Title and SubTitle translation as being in base class
            ResolveTextProperty(); // Then resolve Text property
        }

        private void ResolveTextProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations("CurrentStatus.Text");
            if (States["currentStatus"] is CurrentStatus currentStatus)
            {
                currentStatus.Text = translatedText;
            }
        }

        public override bool SetAppearance(string appearance)
        {
            try
            {
                var ok = EnumExtensions.GetEnumValueFromDescription(appearance, out SwitchEnum switchEnum);
                Style.Appearance = appearance;
                switch (switchEnum)
                {
                    case SwitchEnum.Injector:
                        _predefinedStatuses.Add("active");
                        _predefinedStatuses.Add("inactive");
                        break;

                    case SwitchEnum.Battery:
                        _predefinedStatuses.Add("recharge");
                        _predefinedStatuses.Add("not_set");
                        _predefinedStatuses.Add("replace");
                        _predefinedStatuses.Add("ok");
                        break;
                }
                return true;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"UIElement {nameof(UIDevice)} does not support {appearance} appearance");
            }
        }

        public bool SetStatus(string status, string message)
        {
            if (_predefinedStatuses.Contains(status))
            {
                CurrentStatus = new CurrentStatus(status, message);
                return true;
            }
            return false;
        }
    }

    public enum SwitchEnum
    {
        [Description("injector")]
        Injector = 0,
        [Description("battery")]
        Battery = 1,
    }

    public class CurrentStatus
    {
        public CurrentStatus(string tag, string text)
        {
            Tag = tag;
            Text = text;
        }

        public string Tag { get; set; }
        public string Text { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CurrentStatus status &&
                   Tag == status.Tag &&
                   Text == status.Text;
        }

        public override int GetHashCode()
        {
            int hashCode = -576525115;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Tag);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            return hashCode;
        }
    }
}
