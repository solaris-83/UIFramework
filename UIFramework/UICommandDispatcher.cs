
using UIFramework.PredefinedPages;

namespace UIFramework
{
    public class UiCommandDispatcher
    {
        private readonly Page _page;
        private UISnapshot _lastSnapshot;
        private readonly CommandFactory _factory;

        public UiCommandDispatcher(Page page)
        {
            _page = page;
            _factory = new CommandFactory(page);
            _lastSnapshot = SnapshotBuilder.From(page);
        }

        // Usato per rispondere agli eventi generati dal JS
        public List<DiffOperation> HandleEvent(UiEvent evt)
        {
            var command = _factory.Create(evt);
            if (command == null)
                return [];

            command.Execute();

            UISnapshot newSnapshot = SnapshotBuilder.From(_page);
            var diffs = DiffEngine.Compute(_lastSnapshot, newSnapshot);

            _lastSnapshot = newSnapshot;
            return diffs;
        }

        // Usato per mandare al JS solo le parti modificate dal C#
        public List<DiffOperation> EvaluateDiff()
        {
            var newSnapshot = SnapshotBuilder.From(_page);
            var diffs = DiffEngine.Compute(_lastSnapshot, newSnapshot);

            _lastSnapshot = newSnapshot;
            return diffs;
        }
    }
}
