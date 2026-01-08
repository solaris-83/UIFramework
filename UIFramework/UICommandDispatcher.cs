
using Microsoft.Win32;
using UIFramework.Helpers;
using UIFramework.PredefinedPages;
using static UIFramework.UIElement;

namespace UIFramework
{
    public class UiCommandDispatcher
    {
        private readonly Page _page;
        private UISnapshot _lastSnapshot;
        private readonly DiffEngine _diffEngine;
        private readonly CommandRegistry _registry;

        public UiCommandDispatcher(Page page)
        {
            _page = page;
            _registry = new CommandRegistry();
            _lastSnapshot = SnapshotBuilder.From(page);
            _diffEngine = new DiffEngine();
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            _registry.Register<UIElement>(
                nameof(UIElement.Enabled),
                (el) => new UIElementEnabledChangedCommand(el)
            );
            _registry.Register<UIElement>(
                nameof(UIElement.Visible),
                (chk) => new UIElementVisibleChangedCommand(chk)
            );
            _registry.Register<UICheckbox>(
                nameof(UICheckbox.Checked),
                (chk) => new CheckboxCheckedChangedCommand(chk)
            );
        }

        // Usato per rispondere agli eventi generati dal JS
        public void HandleEvent(UIEvent events)
        {
            // 1. ricavo UIElement tramite Id
            // 2. per ogni nuova property States aggiornata cerco il cmd che applica l'aggiornamento della istanza UIElement
            var element = _page.FindById(events.ElementId);
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
