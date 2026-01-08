

using System;
using System.Collections.Generic;
using System.Linq;

namespace UIFrameworkDotNet
{
    public sealed class CommandRegistry
    {
        private readonly List<Func<UIElement, string, ICommand>> _rules = new List<Func<UIElement, string, ICommand>>();

        public void Register<T>(
            string property,
            Func<T, ICommand> factory)
            where T : UIElement
        {
            _rules.Add((element, prop) =>
            {
                if (element is T typed && prop.Equals(property, StringComparison.InvariantCultureIgnoreCase))
                    return factory(typed);

                return null;
            });
        }

        public ICommand Resolve(
            UIElement element,
            string property)
        {
            return _rules
                .Select(rule => rule(element, property))
                .FirstOrDefault(cmd => cmd != null);
        }
    }
}
