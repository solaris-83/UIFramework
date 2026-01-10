
using System.Collections.Generic;
using UIFrameworkDotNet.Helpers;
using UIFrameworkDotNet.PredefinedPages;

namespace UIFrameworkDotNet
{
    public class UiCommandDispatcher
    {
        private readonly Page _page;
        private UISnapshot _lastSnapshot;
        private readonly DiffEngine _diffEngine;
        private readonly CommandRegistry _registry;

        public UiCommandDispatcher(Page page, CommandRegistry registry)
        {
            _page = page;
            _registry = registry;
            _lastSnapshot = SnapshotBuilder.From(page);
            _diffEngine = new DiffEngine();
        }

        // Usato per rispondere agli eventi generati dal JS
        public void HandleEvent(UIEvent events)
        {
            // 1. ricavo UIElement tramite Id
            // 2. per ogni nuova property States aggiornata inviata dal JS cerco il cmd che applica l'aggiornamento della istanza UIElement lato C#
            var element = _page.FindById(events.ElementId);
            if (element == null)
                return;
            foreach(var ev in events.NewStates)
            {
                var cmd = _registry.Resolve(element, ev.Key);
                if (cmd != null)
                    cmd.Execute(ev.Value);
            }
        }

        // Usato per mandare al JS solo le parti modificate dal C#
        public List<DiffOperation> EvaluateDiff()
        {
            var newSnapshot = SnapshotBuilder.From(_page);
            var diffs = _diffEngine.Compute(_lastSnapshot, newSnapshot);
            _lastSnapshot = newSnapshot;
            return diffs;
        }
    }
}
