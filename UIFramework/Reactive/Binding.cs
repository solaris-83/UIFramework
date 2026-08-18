using UIFramework.Interfaces.Reactive;

namespace UIFramework.Reactive
{
    public sealed class Binding
    {
        private readonly ICondition _condition;
        private readonly IReaction _reaction;

        private bool? _lastValue;
        private bool _isEvaluating;

        public Binding(ICondition condition, IReaction reaction)
        {
            _condition = condition;
            _reaction = reaction;
        }

        public void Evaluate()
        {
            if (_isEvaluating)
                return;

            try
            {
                _isEvaluating = true;

                var current = _condition.Evaluate();

                if (_lastValue == current)
                    return;

                _reaction.Apply(current);
                _lastValue = current;
            }
            finally
            {
                _isEvaluating = false;
            }
        }
    }
}
