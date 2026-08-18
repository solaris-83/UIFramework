using log4net;
using System;
using UIFramework.Helpers;
using UIFramework.Interfaces.Reactive;
using UIFramework.UIElements.Base;

namespace UIFramework.Reactive
{
    public sealed class EqualsCondition : ICondition
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(EqualsCondition));

        private readonly UIElement _target;
        private readonly string _propertyName;
        private readonly object _expectedValue;

        public EqualsCondition(UIElement target, string property, object expected)
        {
            _target = target;
            _propertyName = property;
            _expectedValue = expected;
        }

        public bool Evaluate()
        {
            _logger.Debug($"Evaluating EqualsCondition for property '{_propertyName}' with expected value '{_expectedValue}' on target '{_target.ToString()}'.");
            var currentValue = PropertyPathResolver.GetPropertyValue(_target, _propertyName) ?? throw new InvalidOperationException($"Property '{_propertyName}' not found");
            var convertedValue = Convert.ChangeType(currentValue, _expectedValue.GetType());
            var result = Equals(convertedValue, _expectedValue);
            _logger.Debug($"EqualsCondition result for property '{_propertyName}' on target '{_target.ToString()}': {result}");
            return result;
        }

        public UIElement GetTargetElement()
        {
            return _target;
        }
    }
}
