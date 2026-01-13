
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
        public void HandleEvent(UIEvent incomingEvent)
        {
            // 1. ricavo UIElement tramite Id
            // 2. per ogni nuova property States aggiornata inviata dal JS cerco il cmd che applica l'aggiornamento della istanza UIElement lato C#
            UIElement element = default;
            if (!string.IsNullOrEmpty(incomingEvent.TargetId))
            {
                if (incomingEvent.SourceId == incomingEvent.TargetId)
                    element = _page.FindById(incomingEvent.SourceId);
                else
                    element = _page.FindById(incomingEvent.TargetId);
                if (element == null)
                    return;
            }
            else
                element = _page.FindById(incomingEvent.SourceId);
            var cmd = _registry.Resolve(element, incomingEvent.EventType);
            if (cmd != null)
                cmd.Execute(incomingEvent.NewStates);
                //foreach (var ev in customEvent.Value)
                //{
                //    var cmd = _registry.Resolve(element, ev.EventType.ToString());
                //    if (cmd != null)
                //    cmd.Execute(ev.NewStates);
                //}
           
         
            //foreach(var ev in incomingEvent.NewStates)  // TODO MIGLIORARE
            //{
            //    var cmd = _registry.Resolve(element, ev.Key);
            //    if (cmd != null)
            //        cmd.Execute(incomingEvent.NewStates);
            //}
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
