
using UIFramework.PredefinedPages;

namespace UIFramework
{
    public class UiCommandDispatcher
    {
        private readonly Page _page;
        private PageSnapshot _lastSnapshot;
        private readonly CommandFactory _factory;

        public UiCommandDispatcher(Page page)
        {
            _page = page;
            _factory = new CommandFactory(page);
            _lastSnapshot = PageSnapshotBuilder.Create(page);
        }

        // Usato per rispondere agli eventi generati dal JS
        public List<UiDiff> HandleEvent(UiEvent evt)
        {
            var command = _factory.Create(evt);
            if (command == null)
                return [];

            command.Execute();

            var newSnapshot = PageSnapshotBuilder.Create(_page);
            var diffs = DiffEngine.Compute(_lastSnapshot, newSnapshot);

            _lastSnapshot = newSnapshot;
            return diffs;
        }

        // Usato per mandare al JS solo le parti modificate dal C#
        public List<UiDiff> EvaluateDiff()
        {
            var newSnapshot = PageSnapshotBuilder.Create(_page);
            var diffs = DiffEngine.Compute(_lastSnapshot, newSnapshot);

            _lastSnapshot = newSnapshot;
            return diffs;
        }
    }
}
