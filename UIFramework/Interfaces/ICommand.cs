using System.Collections.Generic;
using UIFramework.UIElements.Base;

namespace UIFramework.Interfaces
{
    public interface ICommand
    {
        void Execute(Dictionary<string, object> newStates);
    }

    public class UpdateValueCommand<T> : ICommand where T : UIElement
    {
        public T Value { get; set; }

        public UpdateValueCommand(T value)
        {
            Value = value;
        }

        public virtual void Execute(Dictionary<string, object> newStates)
        {
            Value.UpdateStates(newStates);
        }
    }
}