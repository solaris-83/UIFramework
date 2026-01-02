using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.PredefinedPages;

namespace UIFramework
{
    //public class UIEventDispatcher
    //{
    //    private readonly Page _page;

    //    public UIEventDispatcher(Page page)
    //    {
    //        _page = page;
    //    }

    //    public void Dispatch(UiEvent evt)
    //    {
    //        var element = _page.FindById(evt.ElementId);
    //        if (element == null)
    //            return;

    //        switch (evt.EventType)
    //        {
    //            case "change":
    //                element.ApplyState(evt.Payload);
    //                break;

    //            case "click":
    //                HandleClick(element);
    //                break;
    //        }
    //    }

    //    private void HandleClick(UIElement element)
    //    {
    //        // opzionale: eventi applicativi
    //    }
    //}


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
        public Dictionary<string, object> Data { get; set; }  // Contiene props o state
    }

    public class PageSnapshot
    {
        public Dictionary<string, UiElementSnapshot> Elements { get; } = new();
    }

    public class UiElementSnapshot
    {
        public string Id { get; set; }
        public Dictionary<string, object> Props { get; set; }
        public Dictionary<string, object> State { get; set; }
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
                State = new Dictionary<string, object>(el.State)
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
                    Data = newSnap.Elements[id].Props
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
                        Data = newEl.Props
                    });
                }

                if (!DictionaryEquals(oldEl.State, newEl.State))
                {
                    diffs.Add(new UiDiff
                    {
                        Operation = DiffOperationType.UpdateState,
                        ElementId = id,
                        Data = newEl.State
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
