using BaseCustomApp.Helpers;
using log4net;
using Newtonsoft.Json;
using ScriptLibraries.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UIFramework.Helpers;
using UIFramework.Interfaces;
using UIFramework.Interfaces.Adapters;
using UIFramework.Interfaces.Reactive;
using UIFramework.Reactive;
using UIFramework.SpecializedPopups;
using UIFramework.UIElements;
using UIFramework.UIElements.Base;

namespace UIFramework.SpecializedPages
{
    public class Page : ContainerElement, IDisposable, IPageAdapter
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Page));

        [JsonIgnore]
        public IUIContext Context { get; private set; }
        private UITabControl _tabControl;
        private const string STOP_BUTTON_TEXT = "STOP";
        private UICommandArea _commandArea;
        private UIFeedbackArea _feedbackArea;
        private UITitleArea _titleArea;
        private UIOverlay _overlay;
        private bool _isFullScreen;
        /// <summary>
        /// List of actions that need to be executed after page is loaded
        /// </summary>
        [JsonIgnore]
        public List<Action> ActivableActions { get; private set; }

        /// <summary>
        /// List of actions that need to be stopped before page disposal
        /// </summary>
        [JsonIgnore]
        public List<Action> StoppableActions { get; private set; }

        [JsonIgnore]
        public string CommandName { get; private set; }

        [JsonIgnore]
        public UICommandArea CommandArea
        {
            get => _commandArea;
            set => _commandArea = value;
        }

        [JsonIgnore]
        public UIFeedbackArea FeedbackArea
        {
            get => _feedbackArea;
            set => _feedbackArea = value;
        }

        [JsonIgnore]
        public UITitleArea TitleArea
        {
            get => _titleArea;
            set => _titleArea = value;
        }

        [JsonIgnore]
        public UIOverlay Overlay
        {
            get => _overlay;
            set => _overlay = value;
        }

        [JsonIgnore]
        public UITabControl TabControl
        {
            get => _tabControl;
            set => _tabControl = value;
        }

        [JsonIgnore]
        public bool IsFullScreen
        {
            get => _isFullScreen;
            set
            {
                SetStatesProperty(ref _isFullScreen, value, nameof(IsFullScreen));
            }
        }

        public Page(IUIContext uicontext) : base(uicontext)
        {
            Context = uicontext;
            ActivableActions = new List<Action>();
            StoppableActions = new List<Action>();
            CommandArea = new UICommandArea(Context);
            TabControl = new UITabControl(Context);
            FeedbackArea = new UIFeedbackArea(Context);
            TitleArea = new UITitleArea(Context);
            Overlay = new UIOverlay(Context);
            Add(TabControl);
            Add(CommandArea);
            Add(FeedbackArea);
            Add(TitleArea);
            Add(Overlay);
        }

        #region States

        private string _title;
        [JsonIgnore]
        public string Title
        {
            get => _title;
            set
            {
                if (value == null)
                    value = "BCA_INFORMATION";
                SetTitle("title", value, ElementStatusEnum.Info.GetDescription());
            }
        }

        #endregion

        #region  EVENTS

        // Backing field for the event delegate
        private EventHandler<DiffOperation> _dataChanged;

        // Custom event with controlled add/remove
        public event EventHandler<DiffOperation> DataChanged
        {
            add
            {
                if (_dataChanged != null)
                {
                    throw new InvalidOperationException("DataChanged event already has a subscriber.");
                }
                _dataChanged += value;
            }
            remove
            {
                // Properly unsubscribe the delegate instead of overwriting it
                _dataChanged -= value;
            }
        }

        #endregion

        private void AttachContainer(ContainerElement container)
        {
            container.ItemAdded += OnItemAdded;
            container.ItemRemoved += OnItemRemoved;
            _logger.Info($"[UI] Attached OnItemAdded/OnItemRemoved for ContainerElement {container.Type} with Id {container.Id}");

            foreach (var child in container.Children)
                AttachElement(child);   
        }

        private void AttachElement(UIElement element)
        {
            element.PropertyChanged += Element_PropertyChanged;
            _logger.Info($"[UI] Attached PropertyChanged for UIElement {element.Type} with Id {element.Id}");
            // Se lo UIElement è attachable lo agganciamo
            //if (element is IAttachableContext attachableElement)
            //    attachableElement.AttachContext(Context);

            if (element is ContainerElement c)
                AttachContainer(c);
        }

        private void DetachContainer(ContainerElement container)
        {
            _logger.Info($"[UI] Disposing {container.Type} with Id {container.Id}");
            // Detach children (each DetachElement will process its subtree from inner-most to outer-most)
            foreach (var child in container.Children)
                DetachElement(child);

            // Unsubscribe container-level events after children are detached
            container.ItemAdded -= OnItemAdded;
            container.ItemRemoved -= OnItemRemoved;
        }

        private void DetachElement(UIElement element)
        {
            _logger.Info($"[UI] Disposing {element.Type} with Id {element.Id}");
            if (element is ContainerElement c)
            {
                // First detach all children (deepest elements are detached first)
                foreach (var child in c.Children)
                    DetachElement(child);

                // Then unsubscribe container events
                c.ItemAdded -= OnItemAdded;
                c.ItemRemoved -= OnItemRemoved;
            }

            // Finally unsubscribe property-change handler for this element
            element.PropertyChanged -= Element_PropertyChanged;
            element.Dispose();
        }

        private void OnItemAdded(ContainerElement parent, UIElement child)
        {
            _dataChanged?.Invoke(this, new DiffOperation(DiffOperationType.Add, parent.Id, child));
            AttachElement(child);
        }

        private void OnItemRemoved(ContainerElement parent, UIElement child)
        {
            DetachElement(child);
            _dataChanged?.Invoke(this, new DiffOperation(DiffOperationType.Remove, child.Id, null));
        }

        protected void Element_PropertyChanged(object sender, UIPropertyChange args)
        {
            _dataChanged?.Invoke(this, new DiffOperation(DiffOperationType.UpdateState, args.ElementId, payload: new { args.PropertyName, args.PropertyValue }));
        }

        public bool AddExitingPopup()
        {
            var waitPopup = new UIWaitPopup("BCA_CLOSING_IN_PROGRESS", Context); // stringId 51003
            Overlay.Add(waitPopup);

            return true;
        }

        public void Validate()
        {
            if (IsFullScreen && this is SpecializedPage)
                throw new InvalidOperationException("SpecializedPages cannot have a fullscreen view");

            var tabControls = this.FindAllByType<UITabControl>();
            if (!tabControls.Any() || tabControls.Count() > 1)
            {
                throw new InvalidOperationException("Page must have one UITabControl.");
            }

            var tabControl = tabControls.Single();
            if (tabControl.Children.OfType<UITab>().Any() && string.IsNullOrEmpty(tabControl.ActiveTabId))
                throw new InvalidOperationException("TabControl must provide ActiveTabId info.");

            var lateralAreaElements = this.FindAllByType<UICommandArea>();
            if (lateralAreaElements.Count() != 1)
                throw new InvalidOperationException("Page must have one CommandArea.");

            var lateralArea = lateralAreaElements.First();
            if (lateralArea.Children.All(c => c.Type == nameof(UIButton)) == false)
            {
                throw new InvalidOperationException("You can add only UIButton to CommandArea.");
            }

            var allButtons = lateralArea.Children.Where(c => c is UIButton);
            if (!IsFullScreen && !allButtons.Any(b => b.Style.Appearance == ElementStatusEnum.Danger.GetDescription()))
            {
                throw new InvalidOperationException("You can't create a page without a danger button.");
            }

            var bottomAreaElements = this.FindAllByType<UIFeedbackArea>();
            if (bottomAreaElements.Count() > 1)
                throw new InvalidOperationException("Page must have one FeedbackArea.");

            if (bottomAreaElements.Any())
            {
                var bottomArea = bottomAreaElements.First();
                if (bottomArea.Children.Any(c => c.Type != nameof(UIFeedbackCountdown) && c.Type != nameof(UIFeedbackMessage) && c.Type != nameof(UIFeedbackProgress)))
                {
                    throw new InvalidOperationException("You can add only UIFeedbackCountdown, UIFeedbackProgress and UIFeedbackMessage to FeedbackArea.");
                }
            }

            var tabs = this.FindAllByType<UITab>();
            foreach (var tab in tabs)
            {
                if (tab.Children.Any(c => !(c is UISection) &&!(c is UILabel)))
                    throw new InvalidOperationException("You can only add UISection and one single UILabel (title) to a UITab.");

                if (tab.Children.Count(c => c.Type == nameof(UILabel)) > 1)
                {
                    throw new InvalidOperationException("A Tab must have at most one UILabel (title)");
                }
            }

            var titleAreaElements = this.FindAllByType<UITitleArea>();
            if (titleAreaElements.Count() > 1)
                throw new InvalidOperationException("Page must have one TitleArea.");

            if (titleAreaElements.Any())
            {
                var titleArea = titleAreaElements.First();
                if (titleArea.Children.Any(c => c.Type != nameof(UILabel)))
                {
                    throw new InvalidOperationException("You can add only UILabel to TitleArea.");
                }
            }
            ////// TODO MARCO verificare meglio
            //var sections = this.FindAllByType<UISection>();
            //foreach (var item in sections)
            //{
            //    // Validate indexes
            //    if (item.GridPosition.RowIndex < 0 || item.GridPosition.RowIndex >= item.Grid.Rows)
            //        throw new ArgumentOutOfRangeException($"RowIndex {item.GridPosition.RowIndex} for UISection {item.Id} is not compatible with the configured rows number {item.Grid.Rows}");
            //    if (item.GridPosition.ColumnIndex < 0 || item.GridPosition.ColumnIndex >= item.Grid.Columns)
            //        throw new ArgumentOutOfRangeException($"ColumnIndex {item.GridPosition.ColumnIndex} for UISection {item.Id} is not compatible with the configured columns number {item.Grid.Columns}");
            //    // Validate Span
            //    if (item.GridPosition.RowSpan <= 0)
            //        throw new ArgumentOutOfRangeException($"RowSpan {item.GridPosition.RowSpan} for UISection {item.Id} must be greater than 0");
            //    if (item.GridPosition.ColumnSpan <= 0)
            //        throw new ArgumentOutOfRangeException($"ColumnSpan {item.GridPosition.ColumnSpan} for UISection {item.Id} must be greater than 0");
            //    if (item.GridPosition.RowIndex + item.GridPosition.RowSpan > item.Grid.Rows)
            //        throw new InvalidOperationException($"Considering RownIndex {item.GridPosition.RowIndex} RowSpan {item.GridPosition.RowSpan} exceeds grid rows {item.Grid.Rows}");
            //    if (item.GridPosition.ColumnIndex + item.GridPosition.ColumnSpan > item.Grid.Columns)
            //        throw new InvalidOperationException($"Considering ColumnIndex {item.GridPosition.ColumnIndex} ColumnSpan {item.GridPosition.ColumnSpan} exceeds grid columns {item.Grid.Columns}");
            //    // Validate Overlap
            //    //bool Intersects(GridPosition a, GridPosition b)
            //    //{
            //    //    return !(a.RowIndex + a.RowSpan < b.RowIndex &&
            //    //                b.RowIndex + b.RowSpan > a.RowIndex &&
            //    //                a.ColumnIndex + a.ColumnSpan < b.ColumnIndex &&
            //    //                b.ColumnIndex + b.ColumnSpan > a.ColumnIndex);
            //    //}
            //    //foreach (var other in sections)
            //    //{
            //    //    if (other.Id == item.Id)
            //    //        continue;
            //    //    if (Intersects(other.GridPosition, item.GridPosition))
            //    //        throw new InvalidOperationException(
            //    //            $"GridPosition overlap between sections {other.Id} and {item.Id}");
            //    //}
            //}

            var charts = this.FindAllByType<UIChart>();
            foreach (var chart in charts)
            {
                chart.Children.ForEach((s) =>
                {
                    if (s is UIChartAxis axis)
                    {
                        if (string.IsNullOrEmpty(axis.Position))
                            throw new InvalidOperationException("UIChartAxis must have Position property set.");
                    }
                    if (s is UIChartSignal signal)
                    {
                        if (string.IsNullOrEmpty(signal.XId))
                            throw new InvalidOperationException("UIChartSignal must be set to a X axis.");
                        if (string.IsNullOrEmpty(signal.YId))
                            throw new InvalidOperationException("UIChartSignal must be set to a Y axis.");
                    }
                });
            }
        }

        public UILabel SetTitle(string tag, string idStr, string style)
        {
            return CreateOrUpdateTitle(tag, idStr, style);
        }

        public UILabel SetTitle(string idStr, string style)
        {
            return SetTitle("title", idStr, style);
        }

        public UILabel SetTitle(string idStr)
        {
            return SetTitle("title", idStr, ElementStatusEnum.Info.GetDescription());
        }

        private UILabel CreateOrUpdateTitle(string tag, string idStr, string style)
        {
            UILabel titlePresent = TitleArea.FindAllByType<UILabel>().FirstOrDefault();
            if (titlePresent == null)
            {
                // qui non è proprio corretto. Per il momento va bene perché Title non è aggiornabile.
                // Se in futuro lo diventerà dovrò salvare il Title dentro States["text"] della UILabel.
                var label = new UILabel(idStr, readOnly: true)
                {
                    Tag = tag,
                    Style = new Style() { Appearance = style }
                };
                TitleArea.Add(label);
                return label;
            }
            else
            {
                titlePresent.UpdateText(idStr);
                titlePresent.Tag = tag;
                titlePresent.Style.Appearance = style;
                return titlePresent;
            }
        }
      
        public UITab AddTab(string tag, int rows, int cols)
        {
            _tabControl.CurrentTab = new UITab(tag, rows, cols, Context);
            _tabControl.Add(_tabControl.CurrentTab);
            // Non serve, è già nella Add del tabControl
           // _tabControl.ActiveTabId = _tabControl.CurrentTab.Id;
            return _tabControl.CurrentTab;
        }

        public override void Dispose()
        {
            DetachContainer(this);
        }

        public void Attach()
        {
            AttachContainer(this);
        }

        #region ADD BUTTON  

        public UIButton AddButton(string idStr, bool isEnabled, string style, string text)
        {
            var button = new UIButton(idStr, isEnabled, style, text);
            button.Clicked += OnButtonClicked;
            CommandArea.Add(button);
            return button;
        }

        public UIButton AddButton(string idStr)
        {
            return AddButton(idStr, isEnabled: true, ElementStatusEnum.Primary.GetDescription(), idStr);
        }

        public UIButton AddButton(string idStr, bool isEnabled)
        {
            return AddButton(idStr, isEnabled, ElementStatusEnum.Primary.GetDescription(), idStr);
        }

        public UIButton AddButton(string idStr, string text)
        {
            return AddButton(idStr, isEnabled: true, ElementStatusEnum.Primary.GetDescription(), text);
        }

        public UIButton AddButton(string idStr, bool isEnabled, string style)
        {
            return AddButton(idStr, isEnabled, style, idStr);
        }

        public UIButton AddButton(string idStr, string text, bool isEnabled)
        {
            return AddButton(idStr, isEnabled, ElementStatusEnum.Primary.GetDescription(), text);
        }

        #endregion

        #region STOP BUTTON (ABORT)
        public UIButton AddButtonStop()
        {
            return AddButton(STOP_BUTTON_TEXT, isEnabled: true, ElementStatusEnum.Danger.GetDescription(), STOP_BUTTON_TEXT);
        }

        public UIButton AddButtonStop(bool isEnabled)
        {
            return AddButton(STOP_BUTTON_TEXT, isEnabled, ElementStatusEnum.Danger.GetDescription(), STOP_BUTTON_TEXT);
        }

        public UIButton AddButtonExit()
        {
            return AddButton("EXIT", isEnabled: true, ElementStatusEnum.Danger.GetDescription(), "EXIT");
        }

        public UIButton AddButtonExit(bool isEnabled)
        {
            return AddButton("EXIT", isEnabled, ElementStatusEnum.Danger.GetDescription(), "EXIT");
        }

        public bool EnableButton(string idOrTag)
        {
            var button = CommandArea.FindById(idOrTag) ?? CommandArea.FindAllByType<UIButton>().FirstOrDefault(btn => btn.Tag.ToString() == idOrTag);
            if (button != null)
            {
                button.Enabled = true;
                return true;
            }
            return false;
        }

        public bool DisableButton(string idOrTag)
        {
            var button = CommandArea.FindById(idOrTag) ?? CommandArea.FindAllByType<UIButton>().FirstOrDefault(btn => btn.Tag.ToString() == idOrTag);
            if (button != null)
            {
                button.Enabled = false;
                return true;
            }
            return false;
        }

        public bool EnableStop()
        {
            var button = CommandArea.FindAllByType<UIButton>().FirstOrDefault(btn => btn.Tag.ToString() == STOP_BUTTON_TEXT);
            if (button != null)
            {
                button.Enabled = true;
                return true;
            }
            return false;
        }

        public bool DisableStop()
        {
            var button = CommandArea.FindAllByType<UIButton>().FirstOrDefault(btn => btn.Tag.ToString() == STOP_BUTTON_TEXT);
            if (button != null)
            {
                button.Enabled = false;
                return true;
            }
            return false;
        }

        public bool ShowButton(string idOrTag)
        {
            var button = CommandArea.FindById(idOrTag) ?? CommandArea.FindAllByType<UIButton>().FirstOrDefault(btn => btn.Tag.ToString() == idOrTag);
            if (button != null)
            {
                button.Visible = true;
                return true;
            }
            return false;
        }

        public bool HideButton(string idOrTag)
        {
            var button = CommandArea.FindById(idOrTag) ?? CommandArea.FindAllByType<UIButton>().FirstOrDefault(btn => btn.Tag.ToString() == idOrTag);
            if (button != null)
            {
                button.Visible = false;
                return true;
            }
            return false;
        }

        internal virtual void OnButtonClicked(object sender, EventArgs e)
        {
            if (sender is UIButton button)
            {
                CommandName = button.Tag.ToString();
            }
            Context.WaitForUserEvent.Set();
            _logger.Debug($"Unblocked WaitForUserEvent in order ESLX to proceed");
        }
        #endregion

        #region BOTTOM AREA FEEDBACK
        public UIFeedbackCountdown AddFeedbackCountdown(int ms)
        {
            return AddFeedbackCountdown(ms, true); // default is manual
        }
        public UIFeedbackCountdown AddFeedbackCountdown(int ms, bool isManual)
        {
            var feedback = new UIFeedbackCountdown(ms, isManual);
            FeedbackArea.Add(feedback);
            ActivableActions.Add(() =>
            {
                if (!feedback.IsManual) 
                    feedback.StartCountdown();
            });
            StoppableActions.Add(() =>
            {
               if (feedback != null && feedback.IsActive)
                  feedback?.StopCountdown();
            });
            return feedback;
        }

        public UIFeedbackProgress AddFeedbackProgress(int perc)
        {
            var feedback = new UIFeedbackProgress(perc);
            FeedbackArea.Add(feedback);
            return feedback;
        }

        public UIFeedbackProgress AddFeedbackProgress(double perc)
        {
            var feedback = new UIFeedbackProgress(perc);
            FeedbackArea.Add(feedback);
            return feedback;
        }

        //public bool UpdateFeedbackProgress(string feedbackId, int perc, string msg)
        //{
        //    var feedback = FeedbackArea.FindById(feedbackId);
        //    if (feedback == null || !(feedback is UIFeedbackProgress uIFeedback))
        //        return false;

        //    uIFeedback.UpdateText(msg);
        //    uIFeedback.Percentage = perc;
        //    return true;
        //}

        public bool UpdateFeedback(string feedbackId, string msg)
        {
            var feedback = FeedbackArea.FindById(feedbackId);
            if (feedback is UIFeedbackMessage uIFeedbackMessage)
            {
                uIFeedbackMessage.UpdateText(msg);
                return true;
            }
            if (feedback is UIFeedbackCountdown uIFeedbackCountdown)
            {
                uIFeedbackCountdown.SendUpdate(msg);
                return true;
            }
            if (feedback is UIFeedbackProgress uIFeedbackProgress)
            {
                uIFeedbackProgress.SendUpdate(msg);
                return true;
            }
            return false;
        }

        public bool UpdateFeedback(string feedbackId, double msOrPercentage)
        {
            var feedback = FeedbackArea.FindById(feedbackId);
            if (feedback is UIFeedbackCountdown uIFeedbackCountdown)
            {
                uIFeedbackCountdown.Remaining = (int)msOrPercentage;
                return true;
            }
            if (feedback is UIFeedbackProgress uIFeedbackProgress)
            {
                uIFeedbackProgress.SendUpdate(msOrPercentage);
                return true;
            }
            return false;
        }

        public bool UpdateFeedback(string feedbackId, double msOrPercentage, string msg)
        {
            var feedback = FeedbackArea.FindById(feedbackId);
            if (feedback is UIFeedbackCountdown uIFeedbackCountdown)
            {
                uIFeedbackCountdown.Remaining = (int)msOrPercentage;
                return true;
            }
            if (feedback is UIFeedbackProgress uIFeedbackProgress)
            {
                uIFeedbackProgress.SendUpdate(msOrPercentage, msg);
                return true;
            }
            return false;
        }

        public UIFeedbackMessage AddFeedbackMessage(string msg)
        {
            var feedback = new UIFeedbackMessage(msg);
            FeedbackArea.Add(feedback);
            return feedback;
        }

        #endregion

        #region BINDING
        public bool CreateBinding(ICondition condition, IReaction reaction)
        {
            var binding = new Binding(condition, reaction);
            if (condition is CompositeCondition compositeCondition)
            {
               compositeCondition.Conditions.ForEach(c => c.GetTargetElement().AddBinding(binding));
            }
            else 
                condition.GetTargetElement().AddBinding(binding);
            return true;
        }

        #endregion
    }
}
