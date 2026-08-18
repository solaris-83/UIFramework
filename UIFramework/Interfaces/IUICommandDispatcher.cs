
using ScriptLibraries.Data.Interfaces;
using System.Collections.Generic;
using UIFramework.UIElements.Base;

namespace UIFramework.Interfaces
{
    public interface IUICommandDispatcher
    {
        void HandleUIEvents(UIEventList incomingEvent);
        //void RegisterReaction(IReaction reaction);
        Dictionary<string, UIElement> LastSnapshot {  get; }
        void AddToSnapshot(UIElement element);
        void DequeueParameters(IObserverUpdateCollection data);
        void RemoveFromSnapshot(string elementId);
        void ConvertToUIEventAndResolve(IObservableUIUpdate data);
    }
}
