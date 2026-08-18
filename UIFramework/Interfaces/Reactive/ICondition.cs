using UIFramework.UIElements.Base;

namespace UIFramework.Interfaces.Reactive
{
    public interface ICondition
    {
        bool Evaluate();
        UIElement GetTargetElement();
    }
}
