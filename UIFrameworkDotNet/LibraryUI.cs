using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UIFrameworkDotNet.Helpers;
using UIFrameworkDotNet.PredefinedPages;

namespace UIFrameworkDotNet
{
    public class LibraryUI
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LibraryUI));

        private UiCommandDispatcher _dispatcher = null;
        private Page _currentPage = null;
        private CommandRegistry _registry;
        private IUIContext _uiContext;
        public LibraryUI() 
        {
            _uiContext = new UIContext(new FakeTranslationService());
            _registry = new CommandRegistry();
        
            _registry.Register<UIElement>(
                nameof(UIElement.Enabled),
                (el) => new UIElementEnabledChangedCommand(el)
            );
            _registry.Register<UITextElement>(
                nameof(UITextElement.Text),
                (el) => new UITextElementTextChangedCommand(el, _uiContext.Translator)
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
            _registry.Register<UITabControl>(
               nameof(UITabControl.ActiveTabId),
               (tbc) => new TabControlActiveTabChangedCommand(tbc)
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

        public PageDisclaimer CreatePageDisclaimer() => new PageDisclaimer(_uiContext);

        public PageResult CreatePageResult() => new PageResult(_uiContext);

        // TODO Putroppo che anche questa firma (nello SCROF e qualche altra app)
        [Obsolete]
        public PageResult CreateResult() => CreatePageResult();

        public PageMenu CreatePageMenu() => new PageMenu(_uiContext);

        public Page CreatePage() => new Page(_uiContext);

        public UISection CreateSectionDocument() => new UISection();

        public UISection CreateSection() => new UISection();


        public UISequence CreateSequence() => new UISequence();

        public void ShowAndWait(Page page)
        {
            // Mi deregistro agli aggiornamenti degli UIElement della precedente page triggerati dal C# e dispose
            if (_currentPage != null) _currentPage.DataChanged -= Page_DataChanged;
            _currentPage?.Dispose();
            _currentPage = null;
            _currentPage = page;
            Validate();
            page.AttachContainer(page.LateralArea);
            page.AttachContainer(page.TabControl);

            _dispatcher = new UiCommandDispatcher(page, _registry);
            Console.WriteLine($"=== LOAD PAGE INIZIALE {page.GetType().Name} ===");
            Console.WriteLine(PageSerializer.Serialize(page));

            // Mi registro agli aggiornamenti degli UIElement della page triggerati dal C#
            _currentPage.DataChanged += Page_DataChanged;
        }

        private void Validate()
        {
            var tabControls = _currentPage.FindAllByType<UITabControl>();
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

            var uIFeedbacks = _currentPage.FindAllByType<UIFeedback>();

            if (uIFeedbacks.Count() > 1)
            {
                throw new InvalidOperationException("La pagina può contenere un solo UIFeedback.");
            }
        }

        private void Page_DataChanged(object sender, Type e)
        {
            var diffs = _dispatcher.EvaluateDiff();
            Console.WriteLine("\n>>> MESSAGGIO C# -> JS");
            Console.WriteLine(PageSerializer.Serialize(diffs));
        }

        // Simula l'arrivo di un evento JS dall'interfaccia utente verso il dispatcher C# 
        // ======== A T T E N Z I O N E =================
        // SERVE SOLO PER TEST
        // ==============================================
        public void SimulateJsEvent(
                string elementId,
                string eventType,
                Dictionary<string, object> states)
        {
            Console.WriteLine("\n>>> EVENTO JS -> C#");
            var evt = new UIEvent
            {
                ElementId = elementId,
                EventType = eventType,
                NewStates = states
            };
            Console.WriteLine(JsonConvert.SerializeObject(evt, Formatting.Indented));

            _dispatcher.HandleEvent(evt);
        }
    }
}
