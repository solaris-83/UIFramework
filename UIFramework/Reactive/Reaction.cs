using log4net;
using System;
using UIFramework.Interfaces.Reactive;

namespace UIFramework.Reactive
{
    public sealed class CompositeReaction : IReaction
    {
        private readonly IReaction _onTrue;
        private readonly IReaction _onFalse;

        public CompositeReaction(
            IReaction onTrue,
            IReaction onFalse)
        {
            _onTrue = onTrue;
            _onFalse = onFalse;
        }

        public void Apply(bool conditionResult)
        {
            var reactions = conditionResult ? _onTrue : _onFalse;
            reactions.Apply(conditionResult: true);
        }
    }

    public sealed class Reaction : IReaction
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Reaction));
        private readonly object _target;
        private readonly string _property;
        private readonly object _value;
        public Reaction(object target, string property, object value)
        {
            _target = target;
            _property = property;
            _value = value;
        }

        public void Apply(bool conditionResult)
        {
            if (!conditionResult)
            {
                _logger.Info($"Condition is false, skipping reaction for property {_property} of targetElement {_target.ToString()}");
                return;
            }

            var prop = _target.GetType().GetProperty(_property) ?? throw new InvalidOperationException($"Property '{_property}' not found");
            if (prop != null && prop.CanWrite)
            {
                try
                {
                    var convertedValue = Convert.ChangeType(_value, prop.PropertyType);
                    prop.SetValue(_target, convertedValue);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Property {_property} could not be updated with value {_value}", ex);
                }
            }
            else
            {
                _logger.Error($"Property {_property} is either NULL or cannot be written");
            }
        }
    }
}
