
using Newtonsoft.Json;
using UIFramework.Helpers;
using UIFramework.PredefinedPages;

namespace UIFramework
{
    internal class LibraryUI
    {
        UiCommandDispatcher _dispatcher = null;
        public PageDisclaimer CreatePageDisclaimer()
        {
            return new PageDisclaimer();
        }

        public PageResult CreatePageResult()
        {
            return new PageResult();
        }

        public PageMenu CreatePageMenu()
        {
            return new PageMenu();
        }

        public Page CreatePage()
        {
            return new Page();
        }

        public UISequence CreateSequence()
        {
            return new UISequence();
        }

        public void ShowAndWait(Page page)
        {
            Validate(page);

            _dispatcher = new UiCommandDispatcher(page);
            Console.WriteLine($"=== LOAD PAGE INIZIALE {page.GetType().Name} ===");
            Console.WriteLine(PageSerializer.Serialize(page));

            // Mi registro agli aggiornamenti degli UIElement della page triggerati dal C#
            page.Updated += Page_Updated;
            // TODO deregistrarsi da tutti gli eventi tramite dispose alla fine
        }

        private void Validate(Page page)
        {
            var tabControls = page.FindAllByType<UITabControl>();
            if (!tabControls.Any())
            {
                throw new InvalidOperationException("La pagina deve contenere almeno un UITabControl.");
            }

            if (tabControls.Count() > 1)
            {
                throw new InvalidOperationException("La pagina deve contenere un solo UITabControl.");
            }
            // TODO (è limitazione?) Un solo UIFeedback (se continuiamo ad usare il metodo BCA.UI.UpdateFeedbackProgress / Countdown non stiamo specificando per quale UIElement altrimenti 
            // potremmo poi dire di modificare gli eslx lanciando gli update sull'istanza di UIFeedback.

            var uIFeedbacks = page.FindAllByType<UIFeedback>();

            if (uIFeedbacks.Count() > 1)
            {
                throw new InvalidOperationException("La pagina può contenere un solo UIFeedback.");
            }
        }

        private void Page_Updated(object? sender, Type e)
        {
            var diffs = _dispatcher.EvaluateDiff();
            Console.WriteLine("\n>>> EvaluateDiff");
            Console.WriteLine(PageSerializer.Serialize(diffs));
        }

        // Simula l'arrivo di un evento JS dall'interfaccia utente verso il dispatcher C# 
        // ======== A T T E N Z I O N E =================
        // SERVE SOLO PER TEST
        // ==============================================
        public void SimulateJsEvent(
                string elementId,
                string eventType,
                Dictionary<string, object> payload)
        {
            Console.WriteLine("\n>>> EVENTO JS");
            var evt = new UiEvent
            {
                ElementId = elementId,
                EventType = eventType,
                Payload = payload
            };

            var diffs = _dispatcher.HandleEvent(evt);

            Console.WriteLine(">>> DIFF PRODOTTI:");
            Console.WriteLine(JsonConvert.SerializeObject(diffs, Formatting.Indented));
        }
    }
}
