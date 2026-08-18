using log4net;
using System;
using UIFramework.Helpers;
using UIFramework.Interfaces.Reactive;
using UIFramework.UIElements.Base;

namespace UIFramework.Reactive
{
    public class GreaterThanOrEqualsCondition : ICondition
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(GreaterThanOrEqualsCondition));
        private UIElement _targetElement;
        private readonly string _property;
        private readonly IComparable _threshold;

        public GreaterThanOrEqualsCondition(UIElement target, string property, IComparable threshold) // funziona con int, double, DateTime, decimal, ecc.
        {
            _targetElement = target;
            _property = property;
            _threshold = threshold;
        }

        public bool Evaluate()
        {
            var value = PropertyPathResolver.GetPropertyValue(_targetElement, _property) ?? throw new InvalidOperationException($"Property '{_property}' not found");
            _logger.Debug($"Evaluating GreaterThanOrEqualsCondition for property '{_property}', value '{value}' with threshold '{_threshold}' on element '{_targetElement.ToString()}'");
            var result = ComparableHelper.SafeCompareTo(value, _threshold) >= 0;
            _logger.Debug($"GreaterThanOrEqualsCondition result for property '{_property}' on element '{_targetElement.ToString()}': {result}");
            return result;
            
            throw new InvalidOperationException(
                    $"Property '{_property}' does not implement IComparable");
        }

        public UIElement GetTargetElement()
        {
            return _targetElement;
        }
    }
}
