using System;
using System.Collections.Generic;
using System.Linq;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.Commands
{
    public sealed class CommandRegistry
    {
        private readonly List<Func<UIElement, UIEventType, ICommand>> _rules = new List<Func<UIElement, UIEventType, ICommand>>();

        public void Register<TTarget>(UIEventType eventType, Func<TTarget, ICommand> factory) where TTarget : UIElement
        {
            _rules.Add((element, ev) =>
            {
                if (element is TTarget typed && ev == eventType)
                    return factory(typed);

                return null;
            });
        }

        public ICommand Resolve(
            UIElement element,
            UIEventType property)
        {
            return _rules
                .Select(rule => rule(element, property))
                .FirstOrDefault(cmd => cmd != null);
        }
    }
}
