
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
            _registry.Register<UIImage>(
               nameof(UIImage.Source),
               (img) => new ImageSourceChangedCommand(img)
           ); 
            _registry.Register<UILabel>(
               nameof(UILabel.Text),
               (label) => new LabelTextChangedCommand(label)
           ); 
            _registry.Register<UITabControl>(
               nameof(UITabControl.ActiveTabId),
               (tbc) => new TabControlActiveTabChangedCommand(tbc)
           );
            _registry.Register<UITextbox>(
              nameof(UITextbox.Text),
              (txt) => new TextboxTextChangedCommand(txt)
          );
            _registry.Register<UIFeedback>(
              nameof(UIFeedback.Remaining),
              (fb) => new FeedbackTickChangedCommand(fb)
          );
            _registry.Register<UIFeedback>(
             nameof(UIFeedback.Percentage),
             (fb) => new FeedbackTickChangedCommand(fb)
         );
        }

        // Usato per rispondere agli eventi generati dal JS
        public void HandleEvent(UIEvent events)
        {
            // 1. ricavo UIElement tramite Id
            // 2. per ogni nuova property States aggiornata inviata dal JS cerco il cmd che applica l'aggiornamento della istanza UIElement lato C#
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
