
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

        public Page CreatePage()
        {
            return new Page();
        }

        public void ShowAndWait(Page page)
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

            _dispatcher = new UiCommandDispatcher(page);
            Console.WriteLine($"=== LOAD PAGE INIZIALE {page.GetType().Name} ===");
            Console.WriteLine(PageSerializer.Serialize(page));

            // Mi registro agli aggiornati degli UIElement della page triggerati dal C#
            page.Updated += (sender, args) =>
            {
                var diffs = _dispatcher.EvaluateDiff();
                Console.WriteLine("\n>>> EvaluateDiff");
                Console.WriteLine(PageSerializer.Serialize(diffs));
            };
        }
    
        private void ValidatePage(Page page)
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

            if (string.IsNullOrEmpty(tabControls.Single().SelectedActiveTabId))
            {
                throw new InvalidOperationException("UITabControl deve avere un UITab selezionato.");
            }
        }

        // Simula l'arrivo di un evento JS dall'interfaccia utente verso il dispatcher C# 
        // SERVE SOLO PER TEST
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
