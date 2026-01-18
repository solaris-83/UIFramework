
using ScriptLibraries.Data.Interfaces;
using System.Collections.Generic;

namespace UIFrameworkDotNet
{
    public interface ICommand
    {
        void Execute(object newValue);
    }

    public interface ICommand2
    {
        void Execute(Dictionary<string, object> newStates);
    }

    public class UpdateValueCommand<T> : ICommand2 where T : UIElement
    {
        public T Value { get; set; }

        public UpdateValueCommand(T value)
        {
            Value = value;
        }

        public void Execute(Dictionary<string, object> newStates)
        {
            Value.UpdateStates(newStates);
        }
    }
}
