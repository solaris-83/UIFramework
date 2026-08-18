using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIInputBoxBase : UIElement, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();
        private List<IValidationRule> _validationRules = new List<IValidationRule>();

        public UIInputBoxBase()
        {
            Name = "";
            Description = "";
            DataErrorInfo = new DataErrorInfo();
            SetAppearance(ElementStatusEnum.Normal.GetDescription());
            IsMandatory = false;
        }

        public void AttachContext(IUIContext context)
        {
            _translationBinding.AttachTranslator(context.Translator);
            ResolveTranslationBindings();
        }

        public void ResolveTranslationBindings()
        {
            ResolveNameProperty();
            ResolveDescriptionProperty();
        }

        private void ResolveNameProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations(nameof(Name));
            Props["name"] = translatedText;
        }

        private void ResolveDescriptionProperty()
        {
            var translatedText = _translationBinding.ResolveTranslations(nameof(Description));
            Props["description"] = translatedText;
        }

        public bool AddValidationRule(IValidationRule validationRule)
        {
            _validationRules.Add(validationRule);
            return true;
        }

        public IValidationRule CheckIfValidationRuleExists(string type)
        {
            return _validationRules.FirstOrDefault(validationRule => validationRule.Type == type);
        }

        protected void ApplyValidationRules(object valueToCheck)
        {
            var errorInfoList = new List<string>();
            foreach (var validationRule in _validationRules)
            {
                if (!validationRule.Validate(valueToCheck))
                {
                    errorInfoList.Add(validationRule.ErrorInfo);
                }
            }
            if (errorInfoList.Any())
            {
                DataErrorInfo = new DataErrorInfo(false, errorInfoList);
            }
            else
            {
                DataErrorInfo = new DataErrorInfo();
            }
        }

        public bool RemoveValidationRule(IValidationRule validationRule)
        {
            return _validationRules.Remove(validationRule);
        }


        #region States

        private DataErrorInfo _dataErrorInfo;
        [JsonIgnore]
        public DataErrorInfo DataErrorInfo
        {
            get => _dataErrorInfo;
            set => SetStatesProperty(ref _dataErrorInfo, value, nameof(DataErrorInfo));
        }

        #endregion

        #region Props 

        private bool _isMandatory;
        [JsonIgnore]
        public bool IsMandatory
        {
            get => _isMandatory;
            set => SetProperty(ref _isMandatory, value, () =>
            {
                if (_isMandatory)
                    SetAppearance(ElementStatusEnum.Mandatory.GetDescription());
                else
                    SetAppearance(ElementStatusEnum.Normal.GetDescription());
                Props["isMandatory"] = _isMandatory;
            }, nameof(IsMandatory));
        }

        private string _name;
        [JsonIgnore]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    var translatedText = _translationBinding.SetKeysAndTryResolveTranslations(nameof(Name), _name);
                    Props["name"] = translatedText;
                }
            }
        }

        private string _description;
        [JsonIgnore]
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    var translatedText = _translationBinding.SetKeysAndTryResolveTranslations(nameof(Description), _description);
                    Props["description"] = translatedText;
                }
            }
        }

        #endregion

    }


    public sealed class DataErrorInfo
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }

        public DataErrorInfo()
        {
            Errors = null;
            IsValid = true;
        }

        public DataErrorInfo(bool isValid, List<string> errors)
        {
            Errors = errors;
            IsValid = isValid;
        }

        public override bool Equals(object obj)
        {
            return obj is DataErrorInfo info &&
                   EqualityComparer<List<string>>.Default.Equals(Errors, info.Errors) &&
                   IsValid == info.IsValid;
        }

        public override int GetHashCode()
        {
            int hashCode = 653859863;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(Errors);
            hashCode = hashCode * -1521134295 + IsValid.GetHashCode();
            return hashCode;
        }
    }
}