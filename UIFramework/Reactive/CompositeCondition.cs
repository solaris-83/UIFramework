using System;
using System.Collections.Generic;
using System.Linq;
using UIFramework.Interfaces.Reactive;
using UIFramework.UIElements.Base;

namespace UIFramework.Reactive
{
    public abstract class CompositeCondition : ICondition
    {
        public List<ICondition> Conditions { get; set; }

        public bool AddCondition(ICondition condition)
        {
            Conditions.Add(condition);
            return true;
        }

        protected CompositeCondition(IEnumerable<ICondition> conditions)
        {
            Conditions = conditions.ToList();
        }

        protected CompositeCondition()
        {
            Conditions = new List<ICondition>();
        }

        public abstract bool Evaluate();

        public abstract UIElement GetTargetElement();    
    }

    public class AndCondition : CompositeCondition
    {
        public AndCondition() : base() { }
        public AndCondition(IEnumerable<ICondition> conditions) : base(conditions) { }

        public override bool Evaluate()
        {
            return Conditions.All(c => c.Evaluate());
        }

        public override UIElement GetTargetElement()
        {
            throw new NotImplementedException();
        }
    }

    public class OrCondition : CompositeCondition
    {
        public OrCondition() : base() { }
        public OrCondition(IEnumerable<ICondition> conditions)
            : base(conditions) { }

        public override bool Evaluate()
        {
            return Conditions.Any(c => c.Evaluate());
        }

        public override UIElement GetTargetElement()
        {
            throw new NotImplementedException();
        }
    }
}
