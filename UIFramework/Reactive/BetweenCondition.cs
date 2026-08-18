using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Helpers;
using UIFramework.Interfaces.Reactive;
using UIFramework.SpecializedPages;
using UIFramework.UIElements.Base;

namespace UIFramework.Reactive
{
    public class BetweenInclusiveCondition : ICondition
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BetweenInclusiveCondition));

        private UIElement _targetElement;
        private readonly string _property;
        private readonly IComparable _lowerThreshold;
        private readonly IComparable _upperThreshold;

        public BetweenInclusiveCondition(UIElement targetElement, string property, IComparable lowerThreshold, IComparable upperThreshold)
        {
            _targetElement = targetElement;
            _property = property;
            _lowerThreshold = lowerThreshold;
            _upperThreshold = upperThreshold;
        }

        public bool Evaluate()
        {
            var value = PropertyPathResolver.GetPropertyValue(_targetElement, _property) ?? throw new InvalidOperationException($"Property '{_property}' not found");
            _logger.Debug($"Evaluating BetweenInclusiveCondition for property '{_property}', value '{value}' with thresholds [{_lowerThreshold}, {_upperThreshold}] on element '{_targetElement.ToString()}'");
            var result = ComparableHelper.SafeCompareTo(value, _lowerThreshold) >= 0 && ComparableHelper.SafeCompareTo(value, _upperThreshold) <= 0;
            _logger.Debug($"BetweenInclusiveCondition result for property '{_property}' on element '{_targetElement.ToString()}': {result}");
            return result;
            

            throw new InvalidOperationException(
                    $"Property '{_property}' does not implement IComparable");
        }

        public UIElement GetTargetElement()
        {
            return _targetElement;
        }
    }

    public class BetweenExclusiveCondition : ICondition
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BetweenExclusiveCondition));

        private UIElement _targetElement;
        private readonly string _property;
        private readonly IComparable _lowerThreshold;
        private readonly IComparable _upperThreshold;

        public BetweenExclusiveCondition(UIElement targetElement, string property, IComparable lowerThreshold, IComparable upperThreshold)
        {
            _targetElement = targetElement;
            _property = property;
            _lowerThreshold = lowerThreshold;
            _upperThreshold = upperThreshold;
        }

        public bool Evaluate()
        {
            var value = PropertyPathResolver.GetPropertyValue(_targetElement, _property) ?? throw new InvalidOperationException($"Property '{_property}' not found");
            _logger.Debug($"Evaluating BetweenExclusiveCondition for property '{_property}', value '{value}' with thresholds [{_lowerThreshold}, {_upperThreshold}] on element '{_targetElement.ToString()}'");
            var result = ComparableHelper.SafeCompareTo(value, _lowerThreshold) > 0 && ComparableHelper.SafeCompareTo(value, _upperThreshold) < 0;
            _logger.Debug($"BetweenExclusiveCondition result for property '{_property}' on element '{_targetElement.ToString()}': {result}");
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
