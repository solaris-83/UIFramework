
using UIFramework.PredefinedPages;

namespace UIFramework
{
    public class UIEvent
    {
        public string ElementId { get; set; }  // elemento su cui è stato lanciato l'evento
        public string EventType { get; set; }   // tipo di evento "change", ...
        public Dictionary<string, object> NewStates { get; set; }
    }

    public sealed class DiffOperation
    {
        public DiffOperationType Operation { get; }
        public string TargetId { get; }
        public object? Payload { get; }

        public DiffOperation(
            DiffOperationType operation,
            string targetId,
            object? payload)
        {
            Operation = operation;
            TargetId = targetId;
            Payload = payload;
        }
    }


    public enum DiffOperationType
    {
        Add = 0,
        Remove = 1,
        UpdateProps = 2,
        UpdateState = 3, 
        Move = 4
    }

    public class UiDiff
    {
        public DiffOperationType Operation { get; set; }
        public string ElementId { get; set; }
        public string ParentId { get; set; } // serve per Add
        public Dictionary<string, object> Props { get; set; } 
        public Dictionary<string, object> States { get; set; }
    }

    public class PageSnapshot
    {
        public Dictionary<string, UIElementSnapshot> Elements { get; } = new();
    }

    public class UIElementSnapshot
    {
        public string Id { get; set; }
        public string Type { get; }
        public string ParentId { get; set; }
        public Dictionary<string, object> Props { get; set; }
        public Dictionary<string, object> States { get; set; }
        public UIElementSnapshot(
        string id,
        string type,
        Dictionary<string, object> props,
        Dictionary<string, object> states,
        string? parentId)
        {
            Id = id;
            Type = type;
            Props = props;
            States = states;
            ParentId = parentId;
        }
    }

    public sealed class UISnapshot
    {
        public UISnapshot()
        {
            
        }
        public IReadOnlyDictionary<string, UIElementSnapshot> Elements { get; }

        public UISnapshot(IEnumerable<UIElementSnapshot> elements)
        {
            Elements = elements.ToDictionary(e => e.Id);
        }
    }

    public static class SnapshotBuilder
    {
        public static UISnapshot From(Page page)
        {
            var list = new List<UIElementSnapshot>();

            // Root: TabControl
            Visit(
                element: page,
                parentId: null,
                list: list
            );

            return new UISnapshot(list);
        }


        private static void Visit(
        UIElement element,
        string? parentId,
        List<UIElementSnapshot> list)
        {
            list.Add(
                new UIElementSnapshot(
                    id: element.Id,
                    type: element.GetType().Name,
                    props: new Dictionary<string, object>(element.Props),
                    states: new Dictionary<string, object>(element.States),
                    parentId: parentId
                )
            );

            if (element is ContainerElement container)
            {
                foreach (var child in container.Children)
                {
                    Visit(child, element.Id, list);
                }
            }
        }
    }

    //public static class PageSnapshotBuilder
    //{
    //    public static UISnapshot Create(Page page)
    //    {
    //        var snapshot = new UISnapshot();

    //        foreach (var el in page.Children)
    //            Collect(el, snapshot);

    //        return snapshot;
    //    }

    //    private static void Collect(UIElement el, UISnapshot snapshot)
    //    {
    //        snapshot.Elements[el.Id] = new UIElementSnapshot(el.Id, el.GetType().Name, el.Props, el.States, el.ParentId);

    //        if (el is ContainerElement container)
    //        {
    //            foreach (var child in container.Children)
    //                Collect(child, snapshot);
    //        }
    //    }
    //}

    public class DiffEngine
    {
        public List<DiffOperation> Compute(
            UISnapshot oldSnapshot,
            UISnapshot newSnapshot)
        {
            var diffs = new List<DiffOperation>();

            var oldElements = oldSnapshot.Elements;
            var newElements = newSnapshot.Elements;

            // 1️⃣ REMOVED
            foreach (var oldId in oldElements.Keys)
            {
                if (!newElements.ContainsKey(oldId))
                {
                    diffs.Add(
                        new DiffOperation(
                            DiffOperationType.Remove,
                            oldId,
                            null
                        )
                    );
                }
            }

            // 2️⃣ ADDED + UPDATED + MOVED
            foreach (var (id, newEl) in newElements)
            {
                if (!oldElements.TryGetValue(id, out var oldEl))
                {
                    // ADD
                    diffs.Add(
                        new DiffOperation(
                            DiffOperationType.Add,
                            newEl.ParentId!,
                            newEl
                        )
                    );
                    continue;
                }

                // MOVE
                if (oldEl.ParentId != newEl.ParentId)
                {
                    diffs.Add(
                        new DiffOperation(
                            DiffOperationType.Move,
                            id,
                            new { parentId = newEl.ParentId }
                        )
                    );
                }

                // UPDATE (state diff)
                var stateDiff = ComputeStateDiff(
                    oldEl.States,
                    newEl.States);

                if (stateDiff.Count > 0)
                {
                    diffs.Add(
                        new DiffOperation(
                            DiffOperationType.UpdateState,
                            id,
                            stateDiff
                        )
                    );
                }

                // UPDATE (prop diff)
                var propDiff = ComputePropDiff(
                    oldEl.Props,
                    newEl.Props);

                if (propDiff.Count > 0)
                {
                    diffs.Add(
                        new DiffOperation(
                            DiffOperationType.UpdateProps,
                            id,
                            propDiff
                        )
                    );
                }
            }

            return diffs;
        }

        private Dictionary<string, object> ComputeStateDiff(
                IReadOnlyDictionary<string, object> oldState,
                IReadOnlyDictionary<string, object> newState)
        {
            Dictionary<string, object>? diff = null;

            foreach (var (key, newValue) in newState)
            {
                if (!oldState.TryGetValue(key, out var oldValue) ||
                    !Equals(oldValue, newValue))
                {
                    diff ??= new Dictionary<string, object>();
                    diff[key] = newValue;
                }
            }

            return diff ?? EmptyDictionary;
        }

        private Dictionary<string, object> ComputePropDiff(
                IReadOnlyDictionary<string, object> oldProp,
                IReadOnlyDictionary<string, object> newProp)
        {
            Dictionary<string, object>? diff = null;

            foreach (var (key, newValue) in newProp)
            {
                if (!oldProp.TryGetValue(key, out var oldValue) ||
                    !Equals(oldValue, newValue))
                {
                    diff ??= new Dictionary<string, object>();
                    diff[key] = newValue;
                }
            }

            return diff ?? EmptyDictionary;
        }

        private static readonly Dictionary<string, object> EmptyDictionary
            = new();

    }

}
