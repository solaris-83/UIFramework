
using UIFramework.PredefinedPages;

namespace UIFramework
{
    public class UiEvent
    {
        public string ElementId { get; set; }
        public string EventType { get; set; }   // "change", "click", ...
        public Dictionary<string, object> Payload { get; set; }
    }

    public enum DiffOperationType
    {
        Add = 0,
        Remove = 1,
        UpdateProps = 2,
        UpdateState = 3
    }

    public class UiDiff
    {
        public DiffOperationType Operation { get; set; }
        public string ElementId { get; set; }
        public string ParentId { get; set; } // serve??? (chatgpt dice per Add ma poi non lo usa)
        public Dictionary<string, object> Props { get; set; } 
        public Dictionary<string, object> States { get; set; }
    }

    public class PageSnapshot
    {
        public Dictionary<string, UiElementSnapshot> Elements { get; } = new();
    }

    public class UiElementSnapshot
    {
        public string Id { get; set; }
        public Dictionary<string, object> Props { get; set; }
        public Dictionary<string, object> States { get; set; }
    }

    public static class PageSnapshotBuilder
    {
        public static PageSnapshot Create(Page page)
        {
            var snapshot = new PageSnapshot();

            foreach (var el in page.Children)
                Collect(el, snapshot);

            return snapshot;
        }

        private static void Collect(UIElement el, PageSnapshot snapshot)
        {
            snapshot.Elements[el.Id] = new UiElementSnapshot
            {
                Id = el.Id,
                Props = new Dictionary<string, object>(el.Props),
                States = new Dictionary<string, object>(el.States)
            };

            if (el is ContainerElement container)
            {
                foreach (var child in container.Children)
                    Collect(child, snapshot);
            }
        }
    }

    public static class DiffEngine
    {
        public static List<UiDiff> Compute(PageSnapshot oldSnap, PageSnapshot newSnap)
        {
            var diffs = new List<UiDiff>();

            // REMOVED
            foreach (var id in oldSnap.Elements.Keys.Except(newSnap.Elements.Keys))
            {
                diffs.Add(new UiDiff
                {
                    Operation = DiffOperationType.Remove,
                    ElementId = id
                });
            }

            // ADDED
            foreach (var id in newSnap.Elements.Keys.Except(oldSnap.Elements.Keys))
            {
                diffs.Add(new UiDiff
                {
                    Operation = DiffOperationType.Add,
                    ElementId = id,
                    Props = newSnap.Elements[id].Props, 
                    States = newSnap.Elements[id].States,
                });
            }

            // UPDATED
            foreach (var id in newSnap.Elements.Keys.Intersect(oldSnap.Elements.Keys))
            {
                var oldEl = oldSnap.Elements[id];
                var newEl = newSnap.Elements[id];

                if (!DictionaryEquals(oldEl.Props, newEl.Props))
                {
                    diffs.Add(new UiDiff
                    {
                        Operation = DiffOperationType.UpdateProps,
                        ElementId = id,
                        Props = newEl.Props,
                    });
                }

                if (!DictionaryEquals(oldEl.States, newEl.States))
                {
                    diffs.Add(new UiDiff
                    {
                        Operation = DiffOperationType.UpdateState,
                        ElementId = id,
                        States = newEl.States
                    });
                }
            }

            return diffs;
        }

        private static bool DictionaryEquals(
            Dictionary<string, object> a,
            Dictionary<string, object> b)
        {
            if (a.Count != b.Count)
                return false;

            foreach (var kv in a)
            {
                if (!b.TryGetValue(kv.Key, out var v) || !Equals(v, kv.Value))
                    return false;
            }

            return true;
        }
    }

}
