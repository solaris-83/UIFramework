

using System;
using System.Collections.Generic;
using System.Linq;

namespace UIFrameworkDotNet
{
    public sealed class CommandRegistry
    {
        private readonly List<Func<UIElement, UIEventType, ICommand2>> _rules = new List<Func<UIElement, UIEventType, ICommand2>>();

        public void Register<TTarget>(
            string property,
            Func<TTarget, ICommand2> factory)
            where TTarget : UIElement
        {
            _rules.Add((element, prop) =>
            {
                if (element is TTarget typed && prop.ToString().ToLowerInvariant().Equals(property, StringComparison.InvariantCultureIgnoreCase))
                    return factory(typed);

                return null;
            });
        }

        public ICommand2 Resolve(
            UIElement element,
            UIEventType property)
        {
            return _rules
                .Select(rule => rule(element, property))
                .FirstOrDefault(cmd => cmd != null);
        }
    }
}
