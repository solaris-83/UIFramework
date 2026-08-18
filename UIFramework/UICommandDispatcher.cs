using log4net;
using ScriptLibraries.Data.Interfaces;
using System;
using System.Collections.Generic;
using UIFramework.Commands;
using UIFramework.Interfaces;
using UIFramework.SpecializedPages;
using UIFramework.UIElements;
using UIFramework.UIElements.Base;

namespace UIFramework
{
    public class UICommandDispatcher : IUICommandDispatcher
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UICommandDispatcher));

        private readonly Page _page;
        private Dictionary<string, UIElement> _lastSnapshot;
        private readonly CommandRegistry _registry;
        private Dictionary<string, UIElement> _lastSnapshotWithTagAsKeys;
        private object lockObj = new object ();

        public Dictionary<string, UIElement> LastSnapshot 
        { 
            get => _lastSnapshot; 
            set 
            {
                _lastSnapshot = value;
                BuildAdditionalSnapshotWithTagAsKeys();
            }
        }

        public UICommandDispatcher(Page page)
        {
            _page = page;
            _registry = new CommandRegistry();
            // Registro un command per la gestione dell'evento OnScrollToEnd relativo a un UIButton (target)
           /* _registry.Register<UIButton>(
                UIEventType.OnScrollToEnd,
                (btn) => new UpdateValueCommand<UIButton>(btn)
            );*/
            //_registry.Register<UIGauge>(
            //    UIEventType.OnBackwardCompatibility,
            //    (g) => new UpdateValueCommand<UIGauge>(g)
            //);
            //_registry.Register<UIThermometer>(
            //    UIEventType.OnBackwardCompatibility,
            //    (th) => new UpdateValueCommand<UIGauge>(th)
            //);
            _registry.Register<UIButton>(
                UIEventType.OnButtonClicked,
                (btn) => new ButtonCommand(btn)
            );
            _registry.Register<UIFileInputBox>(
                UIEventType.OnFileSelected,
                (fileInput) => new FileInputBoxCommand(fileInput)
            );
            _registry.Register<UISecureTextBox>(
                UIEventType.OnInputChanged,
                (secureTextBox) => new SecureTextBoxCommand(secureTextBox)
            );
            // Registro un command per gli update delle property relative ad un oggetto UIElement
            _registry.Register<UIElement>(
              UIEventType.OnPropertyChanged,
               (el) => new UpdateValueCommand<UIElement>(el)
           );
           // _registry.Register<UIElement>(
           //   UIEventType.OnBackwardCompatibility,
           //    (el) => new UpdateValueCommand<UIElement>(el)
           //);

            // Alla istanziazione creo lo snapshot della page
            LastSnapshot?.Clear();
            LastSnapshot = SnapshotBuilder.Init(_page);
        }

        public void ConvertToUIEventAndResolve(IObservableUIUpdate data)
        {
            _lastSnapshotWithTagAsKeys.TryGetValue(data.Id, out var element);
            UIEvent ev = null;
            // Questa logica deve valere per un oggetto che implementa IObservableUpdate. Se in futuro fossero necessarie altre logiche raffineremo.
            if (element is UIMeter meter)
            {
                ev = new UIEvent(meter.Id, meter.Id, UIEventType.OnPropertyChanged, new Dictionary<string, object>
                        {
                            { "point", meter.CreateNewPoint(0, Convert.ToDouble(data.Y)) }
                        }, notifyBack: true);
            }
            else if (element is UIChartSignal signal)
            {
                ev = new UIEvent(signal.Id, signal.Id, UIEventType.OnPropertyChanged, new Dictionary<string, object>
                        {
                            { "point", signal.CreateNewPoint(Convert.ToDouble(data.X), Convert.ToDouble(data.Y)) }
                        }, notifyBack: true);
            }

            if (ev != null)
                HandleUIEvent(ev);
        }
        
        public void DequeueParameters(IObserverUpdateCollection items)
        {
            try
            {
                foreach (var data in items.Observables)
                {
                    if (data is IObservableUIUpdate observable)
                        ConvertToUIEventAndResolve(observable);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error processing IObservableUpdate: {ex.Message}", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Cerca lo UIElement o nel _lastSnapshotWithTagAsKeys o nel LastSnapshot e ci applica il command di update states
        /// </summary>
        /// <param name="ev"></param>
        private void HandleUIEvent(UIEvent ev)
        {
            lock (lockObj)
            {
                //UIElement element;
                //if (ev.EventType == UIEventType.OnBackwardCompatibility)
                //{
                //    _lastSnapshotWithTagAsKeys.TryGetValue(ev.TargetId, out element);
                //}
                //else
                //{
                //    LastSnapshot.TryGetValue(ev.TargetId, out element);
                //}

                LastSnapshot.TryGetValue(ev.TargetId, out UIElement element);

                if (element == null)
                {
                    _logger.Error($"No UIElement found for event related to element with Id {ev.TargetId}");
                    return;
                }

                var cmd = _registry.Resolve(element, ev.EventType);
                if (cmd != null)
                {
                    // Eseguo la modifica degli states. Lo snapshot si aggiorna da solo perché agiamo sulla reference direttamente
                    cmd.Execute(ev.NewStates);
                    // Scommentare se necessario
                    //_logger.Info($"Executed command for {element.Id} and {ev.EventType}");
                }
                else
                {
                    _logger.Error($"No command found for element {element.Id} with type {element.Type} given the event {ev.EventType}");
                }
            }
        }

        /// <summary>
        /// Metodo usato per gestire retrocompatibilità con la chiamata SendUpdate della BCA.UI dove si usa come key il Tag invece che l'elementId.
        /// I controlli grafici coinvolti sono: UIGauge, UIThermometer, UIChart
        /// </summary>
        private void BuildAdditionalSnapshotWithTagAsKeys()
        {
            _lastSnapshotWithTagAsKeys = new Dictionary<string, UIElement>();
            foreach (var elementId in LastSnapshot.Keys)
            {
                var element = LastSnapshot[elementId];
                if ((element.Type == "UIGauge" || element.Type == "UIThermometer" || element.Type == "UIChartSignal") && element.Tag != null)
                {
                    _lastSnapshotWithTagAsKeys[element.Tag.ToString()] = element;
                }
            }
        }

        /// <summary>
        /// Add/modify an element in the current snapshot
        /// </summary>
        /// <param name="element">Element already modified</param>
        //public void UpdateSnapshot(UIElement element)
        //{
        //    LastSnapshot[element.Id] = element;
        //}

        public void AddToSnapshot(UIElement element)
        {
           // LastSnapshot[element.Id] = element;
            SnapshotBuilder.FlattenAndUpdate(element, LastSnapshot);
            // Tengo sincronizzato anche l'additionalSnapshot
            BuildAdditionalSnapshotWithTagAsKeys();
        }

        public void RemoveFromSnapshot(string elementId)
        {
            LastSnapshot.TryGetValue(elementId, out var element);
            if (element != null)
            {
                // LastSnapshot.Remove(elementId);
                SnapshotBuilder.FlattenAndRemove(element, LastSnapshot);
                // Tengo sincronizzato anche l'additionalSnapshot
                BuildAdditionalSnapshotWithTagAsKeys();
            }
        }

        // Usato per rispondere agli eventi generati dal JS
        public void HandleUIEvents(UIEventList incomingEvents)
        {
            foreach (UIEvent ev in incomingEvents)
            {
                HandleUIEvent(ev);
            }
        }
    }
}
