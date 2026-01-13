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
            // Registro un command per la gestione dell'evento OnScrollToEnd relativo a un UIButton (target)
            _registry.Register<UIButton>(
                UIEventType.OnScrollToEnd.ToString(),// "onScrollToEnd"
                (el) => new UpdateValueCommand<UIButton>(el)
            );
            // Registro un command per gli update delle property relative ad un oggetto UIElement
            _registry.Register<UIElement>(
              UIEventType.OnPropertyChanged.ToString(), //  "onPropertyChanged",
               (el) => new UpdateValueCommand<UIElement>(el)
           );

            _registry.Register<UITabControl>(
               UIEventType.OnPropertyChanged.ToString(), // nameof(UITabControl.ActiveTabId),
                (tbc) => new UpdateValueCommand<UITabControl>(tbc)
            );
            // _registry.Register<UISection>(
            //     "onScrollToEnd",
            //     (el) => new UISectionChangedCommand(el)
            // );

            // _registry.Register<UIElement>(
            //     nameof(UIElement.Enabled),
            //     (el) => new UIElementEnabledChangedCommand(el)
            // );
            // _registry.Register<UITextElement>(
            //     nameof(UITextElement.Text),
            //     (el) => new UITextElementTextChangedCommand(el)
            // );
            // _registry.Register<UIElement>(
            //     nameof(UIElement.Visible),
            //     (chk) => new UIElementVisibleChangedCommand(chk)
            // );
            // _registry.Register<UIImage>(
            //    nameof(UIImage.Source),
            //    (img) => new ImageSourceChangedCommand(img)
            //);
            // _registry.Register<UITabControl>(
            //    nameof(UITabControl.ActiveTabId),
            //    (tbc) => new TabControlActiveTabChangedCommand(tbc)
            //);
            // _registry.Register<UIFeedback>(
            //     nameof(UIFeedback.Remaining),
            //     (fb) => new FeedbackTickChangedCommand(fb)
            // );
            // _registry.Register<UIFeedback>(
            //     nameof(UIFeedback.Percentage),
            //     (fb) => new FeedbackTickChangedCommand(fb)
            // );
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
            //page.AttachContainer(page.BottomArea);
            //page.AttachContainer(page.LateralArea);
            //page.AttachContainer(page.TabControl);
            _currentPage.Attach();
            //page.AttachContainer(page);

            _dispatcher = new UiCommandDispatcher(_currentPage, _registry);
            Console.WriteLine($"=== LOAD PAGE INIZIALE {_currentPage.GetType().Name} ===");
            Console.WriteLine(PageSerializer.Serialize(_currentPage));

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

            var lateralAreaElements = _currentPage.FindAllByType<UILateralArea>();
            if (lateralAreaElements.Count() != 1)
                throw new InvalidOperationException("La pagina deve contenere una sola LateralArea.");

            var lateralArea = lateralAreaElements.First();
            if (lateralArea.Children.All(c => c.Type == nameof(UIButton)) == false)
            {
                throw new InvalidOperationException("Nella LateralArea si possono aggiungere solo UIButton.");
            }

            var bottomAreaElements = _currentPage.FindAllByType<UIBottomArea>();
            if (bottomAreaElements.Count() > 1)
                throw new InvalidOperationException("La pagina può contenere una sola BottomArea.");

            if (bottomAreaElements.Any())
            {
                var bottomArea = bottomAreaElements.First();
                if (bottomArea.Children.Any(c => c.Type != nameof(UIFeedback)))
                {
                    throw new InvalidOperationException("Nella BottomArea si possono aggiungere solo UIFeedback.");
                }
            }

            var tabs = _currentPage.FindAllByType<UITab>();
            foreach (var tab in tabs)
            {
                if(tab.Children.Any(c => c.Type != nameof(UISection)))
                    throw new InvalidOperationException("Ad un tab si possono aggiungere solo UISection.");  // TOGLIERE LA POSSIBILITA' DI AGGIUNGERE SECCHE UNA UILABEL. USARE VIA TITLEAREA
            }

            foreach (var tab in tabs)
            {
                if (tab.Children.Any(c => c.Type != nameof(UISection)))
                    throw new InvalidOperationException("Ad un tab si possono aggiungere solo UISection.");  // TOGLIERE LA POSSIBILITA' DI AGGIUNGERE SECCHE UNA UILABEL. USARE VIA TITLEAREA
            }

            var titleAreaElements = _currentPage.FindAllByType<UITitleArea>();
            if (titleAreaElements.Count() > 1)
                throw new InvalidOperationException("La pagina può contenere una sola TitleArea.");

            if (titleAreaElements.Any())
            {
                var titleArea = titleAreaElements.First();
                if (titleArea.Children.Any(c => c.Type != nameof(UILabel)))
                {
                    throw new InvalidOperationException("Nella TitleArea si possono aggiungere solo UILabel.");
                }
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
        public void SyncModelAndNotifyUI(UIEvent uiEvent
                //string sourceId,
                //string targetId,
                //string eventType,
                //bool notifyBack,
                //Dictionary<string, object> states
            )  // TODO usare oggetto UIEvents
        {
            Console.WriteLine("\n>>> EVENTO JS -> C#");
            //Enum.TryParse(eventType, out UIEventType enumFound);
            //var evt = new UIEvent
            //{
            //    SourceId = sourceId,
            //    TargetId = targetId,
            //    EventType = enumFound,
            //    NotifyBack = notifyBack,
            //    NewStates = states
            //};
            Console.WriteLine(JsonConvert.SerializeObject(uiEvent, Formatting.Indented));

            _dispatcher.HandleEvent(uiEvent);
        }
    }
}
