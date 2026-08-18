using System.Collections.Generic;
using System.ComponentModel;
using UIFramework.SpecializedPages;
using UIFramework.UIElements.Base;

namespace UIFramework
{
    public enum UIEventType
    {
        [Description("onScrollToEnd")]
        OnScrollToEnd = 0,
        [Description("onPropertyChanged")]
        OnPropertyChanged = 1,
        [Description("onBackwardCompatibility")]
        OnBackwardCompatibility = 2,
        [Description("onButtonClicked")]
        OnButtonClicked = 3,
        [Description("onFileSelected")]
        OnFileSelected = 4,
        [Description("onInputChanged")]
        OnInputChanged = 5
        //[Description("onNoChecked")]
        //OnNoChecked = 4,
        //[Description("onAtLeastOneChecked")]
        //OnAtLeastOneChecked = 5,
        //[Description("onSelectedChanged")]
        //OnSelectedChanged = 6,
        //[Description("onRequestedPropertyChanged")]
        //OnRequestedPropertyChanged = 7
    }

    public class UICustomEvent
    {
        public UICustomEvent(string sourceId, string targetId, UIEventType eventType, bool notifyBack, Dictionary<string, object> newStates)
        {
            SourceId = sourceId;
            TargetId = targetId;
            EventType = eventType;
            NotifyBack = notifyBack;
            NewStates = newStates;
        }

        public string SourceId { get; set; }  // elemento su cui è configurato l'evento
        public string TargetId { get; set; } // elemento su cui va agire l'evento
        public UIEventType EventType { get; set; }  // tipo di evento
        public bool NotifyBack { get; set; } = true;
        public Dictionary<string, object> NewStates { get; set; }
    }

    public sealed class UIEventList : List<UIEvent>
    {
        
    }

    public sealed class UIEvent
    {
        public UIEvent(string sourceId, string targetId, UIEventType eventType, Dictionary<string, object> newStates, bool notifyBack)
        {
            SourceId = sourceId;
            TargetId = targetId;
            EventType = eventType;
            NewStates = newStates;
            NotifyBack = notifyBack;
        }

        public string SourceId { get; set; }
        public string TargetId { get; set; } // elemento su cui va agire l'evento
        public UIEventType EventType { get; set; }  // tipo di evento
        public Dictionary<string, object> NewStates { get; set; }
        public bool NotifyBack { get; set; }
    }

    public sealed class DiffOperation
    {
        public DiffOperationType OperationType { get; }
        public string ElementId { get; }
        public object Payload { get; }

        public DiffOperation(
            DiffOperationType operationType,
            string elementId,
            object payload)
        {
            OperationType = operationType;
            ElementId = elementId;
            Payload = payload;
        }
    }


    public enum DiffOperationType
    {
        Add = 0,
        Remove = 1,
        UpdateState = 2,
        UpdateProp = 3,
        Move = 4
    }

    public static class SnapshotBuilder
    {
        public static Dictionary<string, UIElement> Init(Page page)
        {
            Dictionary<string, UIElement> dict = new Dictionary<string, UIElement>();
            ToFlatten(page, dict);
            return dict;
        }

        /// <summary>
        /// Add a new flattened element to the current snapshot
        /// </summary>
        /// <param name="newElement">New element to flatten</param>
        /// <param name="currentSnapshot">The current page presented as snapshot</param>
        public static void FlattenAndUpdate(UIElement newElement, Dictionary<string, UIElement> currentSnapshot)
        {
            var stack = new Stack<UIElement>(32);
            stack.Push(newElement);
            Dictionary<string, UIElement> newDict = new Dictionary<string, UIElement>();
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                newDict[current.Id] = current;

                if (current is ContainerElement container)
                {
                    var children = container.Children;
                    for (int i = 0; i < children.Count; i++)
                        stack.Push(children[i]);
                }
            }
            foreach (var item in newDict)
            {
                currentSnapshot.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Remove an element and its children from the current snapshot
        /// </summary>
        /// <param name="newElement">New element to flatten</param>
        /// <param name="currentSnapshot">The current page presented as snapshot</param>
        public static void FlattenAndRemove(UIElement newElement, Dictionary<string, UIElement> currentSnapshot)
        {
            var stack = new Stack<UIElement>(32);
            stack.Push(newElement);
            Dictionary<string, UIElement> newDict = new Dictionary<string, UIElement>();
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                newDict[current.Id] = current;

                if (current is ContainerElement container)
                {
                    var children = container.Children;
                    for (int i = 0; i < children.Count; i++)
                        stack.Push(children[i]);
                }
            }
            foreach (var item in newDict)
            {
                currentSnapshot.Remove(item.Key);
            }
        }

        private static void ToFlatten(UIElement root, Dictionary<string, UIElement> target)
        {
            target.Clear();

            var stack = new Stack<UIElement>(32);
            stack.Push(root);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                target[current.Id] = current;

                if (current is ContainerElement container)
                {
                    var children = container.Children;
                    for (int i = 0; i < children.Count; i++)
                        stack.Push(children[i]);
                }
            }
        }
    }
}
