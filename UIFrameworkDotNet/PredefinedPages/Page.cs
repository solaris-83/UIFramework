using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using UIFrameworkDotNet.Helpers;

namespace UIFrameworkDotNet.PredefinedPages
{
    public class Page : ContainerElement, IDisposable
    {
        private readonly IUIContext _uiContext;
        private UITabControl _tabControl;
        private const string STOP_BUTTON_TEXT = "STOP";
        private UILateralArea _lateralArea;
        private UIBottomArea _bottomArea;
        private UITitleArea _titleArea;
        //private UITab _implicitTab;
        //private UISection _implicitSection;

        [JsonIgnore]
        public UILateralArea LateralArea
        {
            get => _lateralArea;
            set => _lateralArea = value;
        }

        [JsonIgnore]
        public UIBottomArea BottomArea
        {
            get => _bottomArea;
            set => _bottomArea = value;
        }

        [JsonIgnore]
        public UITitleArea TitleArea
        {
            get => _titleArea;
            set => _titleArea = value;// SetStatesProperty(ref _titleArea, value, nameof(TitleArea));
        }

        public Page(IUIContext uicontext)
        {
            _uiContext = uicontext;
            LateralArea = new UILateralArea();
            TabControl = new UITabControl();
            BottomArea = new UIBottomArea();
            TitleArea = new UITitleArea();
            Add(TabControl);
            Add(LateralArea);
            Add(BottomArea);
            Add(TitleArea);
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
                    value = "Information";
                SetTitle("title", value, "info");
            }
        }

        #endregion

        [JsonIgnore]
        public UITabControl TabControl
        {
            get  => _tabControl;
            set => _tabControl = value;
        }

        // Backing field for the event delegate
        private EventHandler<Type> _dataChanged;

        // Custom event with controlled add/remove
        public event EventHandler<Type> DataChanged
        {
            add
            {
                if (_dataChanged != null)
                {
                    throw new InvalidOperationException("Updated event already has a subscriber.");
                }
                _dataChanged = value;
            }
            remove
            {
                // Properly unsubscribe the delegate instead of overwriting it
                _dataChanged = value;
            }
        }

        public void AttachContainer(ContainerElement container)
        {
            container.ItemAdded += OnItemAdded;
            container.ItemRemoved += OnItemRemoved;

            foreach (var child in container.Children)
                AttachElement(child);   
        }

        public void AttachElement(UIElement element)
        {
            element.PropertyChanged += OnPropertyChanged;
            if (element is UITextElement textElement)
                textElement.AttachContext(_uiContext);

            if (element is ContainerElement c)
                AttachContainer(c);
        }

        public void DetachContainer(ContainerElement container)
        {
            // Detach children (each DetachElement will process its subtree from inner-most to outer-most)
            foreach (var child in container.Children)
                DetachElement(child);

            // Unsubscribe container-level events after children are detached
            container.ItemAdded -= OnItemAdded;
            container.ItemRemoved -= OnItemRemoved;
        }

        private void DetachElement(UIElement element)
        {
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
            element.PropertyChanged -= OnPropertyChanged;
        }

        private void OnItemAdded(ContainerElement parent, UIElement child)
        {
            AttachElement(child);
            _dataChanged?.Invoke(this, child.GetType());
        }

        private void OnItemRemoved(ContainerElement parent, UIElement child)
        {
            DetachElement(child);
            _dataChanged?.Invoke(this, child.GetType());
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var element = (UIElement)sender;
            _dataChanged?.Invoke(this, element.GetType());
            //SendToJs(
            //    _diff.Update(
            //        element.Id,
            //        new Dictionary<string, object?>
            //        {
            //            [e.PropertyName!.ToLower()] =
            //                element.GetState(e.PropertyName.ToLower())
            //        }
            //    )
            //);
        }

        public UILabel SetTitle(string tag, string idStr, string style)
        {
            return CreateOrUpdateTitle(tag, idStr, style);
        }

        public UILabel SetTitle(string idStr, string style)
        {
            return SetTitle("title", idStr, style);
        }

        private UILabel CreateOrUpdateTitle(string tag, string idStr, string style)
        {
            UILabel titlePresent = TitleArea.FindAllByType<UILabel>().FirstOrDefault();
            if (titlePresent == null)
            {
                var label = new UILabel(idStr)
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

        // TODO Il tab deve avere come primo parametro lo stile.
        // TODO non esiste la possibilità di mettere l'header?
        public UITab AddTab(string title, int rows, int cols)
        {
            //// Se esiste già il tab implicito lo riconfiguro
            //if (_implicitTab != null)
            //{
            //    _implicitTab.Tag = title;
            //    _implicitTab.Grid = new Grid(rows, cols);
            //    return _implicitTab;
            //}
            _tabControl.CurrentTab = new UITab(title, rows, cols);
            _tabControl.Add(_tabControl.CurrentTab);
            _tabControl.ActiveTabId = _tabControl.CurrentTab.Id;
            return _tabControl.CurrentTab;
        }

        //protected UISection EnsureImplicitSection()
        //{
        //    if (_implicitSection != null)
        //        return _implicitSection;
        //    // Creo tab implicito
        //    _implicitTab = new UITab("default", rows: 1, columns: 1);
        //    TabControl.Add(_implicitTab);
        //    TabControl.ActiveTabId = _implicitTab.Id;
        //    TabControl.CurrentTab = _implicitTab;
        //    // Crea section implicita
        //    _implicitSection = new UISection();
        //    _implicitTab.Add(_implicitSection, 0, 0);

        //    return _implicitSection;
        //}
        //public UIImage AddImage(string imageName)
        //{
        //    return EnsureImplicitSection().AddImage(imageName);
        //}

        //public bool UpdateImage(string imageId, string newImageName)
        //{
        //    return EnsureImplicitSection().UpdateImage(imageId, newImageName);
        //}

        //public UILabel AddBulletedItem(string idStr)
        //{
        //    return EnsureImplicitSection().AddBulletedItem(idStr);
        //}

        //public bool UpdateBulletedItem(string itemId, string newIdStr)
        //{
        //    return EnsureImplicitSection().UpdateBulletedItem(itemId, newIdStr);
        //}

        //// "list-item-ordered" è in Style.Appearance
        //// index è in Tag
        //public UILabel AddOrderedItem(string idStr, int index)
        //{
        //    return EnsureImplicitSection().AddOrderedItem(idStr, index);
        //}

        //public UILabel AddOrderedItem(string idStr, string style, int index)
        //{
        //    return EnsureImplicitSection().AddOrderedItem(idStr, style, index);
        //}

        //public UILabel AddOrderedItem(string idStr, string style)
        //{
        //    return EnsureImplicitSection().AddOrderedItem(idStr, style);
        //}

        //public UILabel AddOrderedItem(string idStr)
        //{
        //    return AddOrderedItem(idStr, "");
        //}

        //public UILabel AddParagraph(string idStr)
        //{
        //    return AddParagraph(idStr, "paragraph", "");
        //}

        //public UILabel AddParagraph(string idStr, string style, string color) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        //{
        //    return EnsureImplicitSection().AddParagraph(idStr, style, color);
        //}

        //public bool UpdateParagraph(string paragraphId, string newIdStr) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        //{
        //    return EnsureImplicitSection().UpdateParagraph(paragraphId, newIdStr);
        //}

        //public bool UpdateParagraph(string paragraphId, string newIdStr, string style, string color) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        //{
        //    return EnsureImplicitSection().UpdateParagraph(paragraphId, newIdStr, style, color);
        //}

        /*
        public UIImage AddImage(string imageName)
        {
            var image = new UIImage(imageName);
            //string imagePath = Path.Combine(ExecutionResultManager.Global.Directories.ModuleFolder, "Resources", "img", imageName);
            string imagePath = Path.Combine("Helpers", "imgs", imageName);
            if (!File.Exists(imagePath))
            {
                // _logger.Error($"Image not found in the Working Unit {element.Source}");
            }

            image.Source = ImageHelper.ConvertImageToBase64(imagePath);
            _tabControl.CurrentTab.Add(image);
            // OnUpdated(image.GetType());
            return image;
        }

        public bool UpdateImage(string imageId, string newImageName)
        {
            var image = this.FindById(imageId);
            if (image == null || !(image is UIImage img))
                return false;

            string imagePath = Path.Combine("Resources", "img", newImageName);
            if (!File.Exists(imagePath))
            {
                // _logger.Error($"Image not found in the Working Unit {element.Source}");
                return false;
            }
            img.Source = ImageHelper.ConvertImageToBase64(imagePath);
            return true;
        }

        public UILabel AddBulletedItem(string idStr)
        {
            UILabel label = new UILabel(idStr); // TranslationsService.Instance.CurrentTranslations.GetLocalOrDefault(idStr);
            label.Style = new Style() { Appearance = "list-item-unordered" };
            _tabControl.CurrentTab.Add(label);
            return label;
        }

        public bool UpdateBulletedItem(string itemId, string newIdStr)
        {
            var item = this.FindById(itemId);
            if (item == null || !(item is UILabel label))
                return false;
            label.UpdateText(newIdStr);
            return true;
        }

        // "list-item-ordered" è in Style.Appearance
        // index è in Tag
        public UILabel AddOrderedItem(string idStr, int index)
        {
            UILabel label = new UILabel(idStr); // TranslationsService.Instance.CurrentTranslations.GetLocalOrDefault(idStr);
            label.Style = new Style() { Appearance = "list-item-ordered" };
            label.Tag = index;
            _tabControl.CurrentTab.Add(label);
            return label;
        }

        public UILabel AddOrderedItem(string idStr, string style, int index)
        {
            UILabel label = new UILabel(idStr); // TranslationsService.Instance.CurrentTranslations.GetLocalOrDefault(idStr);
            label.Style = new Style() { Appearance = style };
            label.Tag = index;
            _tabControl.CurrentTab.Add(label);
            return label;
        }

        public UILabel AddOrderedItem(string idStr, string style)
        {
            UILabel label = new UILabel(idStr); // TranslationsService.Instance.CurrentTranslations.GetLocalOrDefault(idStr);
            label.Style = new Style() { Appearance = style };
            // label.Tag = index;
            _tabControl.CurrentTab.Add(label);
            return label;
        }

        public UILabel AddOrderedItem(string idStr)
        {
            return AddOrderedItem(idStr, "list-item-ordered");
        }

        public UILabel AddParagraph(string idStr)
        {
            return AddParagraph(idStr, "paragraph", "");
        }

        public UILabel AddParagraph(string idStr, string style, string color) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        {
            var label = new UILabel(idStr);
            label.Style = new Style() { Appearance = style, ForegroundColor = color };
            _tabControl.CurrentTab.Add(label);
            return label;
        }

        public bool UpdateParagraph(string paragraphId, string newIdStr) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        {
            return UpdateParagraph(paragraphId, newIdStr, "", "");
        }

        public bool UpdateParagraph(string paragraphId, string newIdStr, string style, string color) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        {
            var paragraph = this.FindById(paragraphId);
            if (paragraph == null || !(paragraph is UILabel label))
                return false;
            if (!string.IsNullOrEmpty(style))  // TODO vedere se migliorabile ( minimizzare le OnPropertyChanged e quindi la generazione di messaggi verso JS)
                label.Style.Appearance = style;
            if (!string.IsNullOrEmpty(color))
                label.Style.ForegroundColor = color;
            label.UpdateText(newIdStr);
            return true;
        }
        */

        #region ADD BUTTON  
        public UIButton AddButton(string id)
        {
            var button = new UIButton(id, false, ""); // non c'è uno stile per il default??
           LateralArea.Add(button);
            return button;
        }

        public UIButton AddButton(string id, bool isEnabled, string style)
        {
            var button = new UIButton(id, isEnabled, style);
            LateralArea.Add(button);
            return button;
        }

        public UIButton AddButton(string id, bool isEnabled)
        {
            var button = new UIButton(id, isEnabled, "");
            LateralArea.Add(button);
            return button;
        }

        public UIButton AddButton(string id, string text)
        {
            var button = new UIButton(id, false, "", text);
            LateralArea.Add(button);
            return button;
        }

        public UIButton AddButton(string id, string text, bool isEnabled)
        {
            var button = new UIButton(id, isEnabled, "standard", text);
            LateralArea.Add(button);
            return button;
        }
        #endregion

        #region STOP BUTTON (ABORT)
        public UIButton AddButtonStop()
        {
            return AddButton(STOP_BUTTON_TEXT, true, "danger");
        }

        public UIButton AddButtonStop(bool isEnabled)
        {
            return AddButton(STOP_BUTTON_TEXT, isEnabled, "danger");
        }

        public bool EnableStop()
        {
            var button = this.FindById(STOP_BUTTON_TEXT);
            if (button == null)
                return false;
            if (button is UIButton btn)
                btn.Enabled = true;
           
            return true;
        }

        public bool DisableStop()
        {
            var button = this.FindById(STOP_BUTTON_TEXT);
            if (button == null)
                return false;
            if (button is UIButton btn)
                btn.Enabled = false;
             
            return true;
        }
        #endregion

        // TODO capire come gestire lo shortcut e quindi il poterlo richiamare direttamente da Page come avviene purtroppo in ESLX. Ora si dovrebbe cercare la UITab e la section corrente. Se non esistono crearle prima di aggiungere l'elemento UIElement?
        #region BOTTOM AREA FEEDBACK
        public UIFeedback AddFeedbackCountdown(int ms)
        {
            return AddFeedbackCountdown(ms, true); // default is manual
        }
        public UIFeedback AddFeedbackCountdown(int ms, bool isManual)
        {
            var feedback = new UIFeedback(FeedbackMode.Countdown, "countdown", (ms * 1000).ToString() /* BasicLibraries.UTILITY.FormatDuration(ms * 1000)*/, ms, isManual);
            BottomArea.Add(feedback);
            //_tabControl.CurrentTab.Add(feedback);
            feedback.TickElapsed += Feedback_TickElapsed; // TODO Bisogna desottoscriversi
            if (!isManual)
                feedback.StartCountdown();
            return feedback;
        }

        public bool UpdateFeedbackCountdown(string feedbackId, int ms)
        {
            var feedback = this.FindById(feedbackId);
            if (feedback == null || !(feedback is  UIFeedback uIFeedback))
                return false;

            uIFeedback.Remaining = ms;
            return true;
        }

        private void Feedback_TickElapsed(object sender, int e)
        {
            // OnUpdated(sender.GetType());
        }

        public UIFeedback AddFeedbackProgress(int perc)
        {
            var feedback = new UIFeedback(FeedbackMode.ProgressBar, "progress", "", perc);
            //_tabControl.CurrentTab.Add(feedback);
            BottomArea.Add(feedback);
            return feedback;
        }

        public bool UpdateFeedbackProgress(string feedbackId, int perc, string msg)
        {
            var feedback = this.FindById(feedbackId);
            if (feedback == null || !(feedback is UIFeedback uIFeedback))
                return false;

            uIFeedback.UpdateText(msg);
            uIFeedback.Percentage = perc;
            return true;
        }

        public bool UpdateFeedbackProgress(string feedbackId, double perc)
        {
            return UpdateFeedbackProgress(feedbackId, perc, "");
        }

        public bool UpdateFeedbackProgress(string feedbackId, double perc, string msg)
        {
            return UpdateFeedbackProgress(feedbackId, perc, msg);
        }

        public bool UpdateFeedbackProgress(string feedbackId, int perc)
        {
            return UpdateFeedbackProgress(feedbackId, perc);
        }

        public UIFeedback AddFeedbackMessage()
        {
            return AddFeedbackMessage("");
        }

        public bool UpdateFeedbackMessage(string feedbackId)
        {
            return UpdateFeedbackMessage(feedbackId, "");
        }

        public UIFeedback AddFeedbackMessage(string msg)
        {
            //msg = TranslationsService.Instance.CurrentTranslations.GetLocalOrDefault(msg);
            var feedback = new UIFeedbackMessage(msg);
            //_tabControl.CurrentTab.Add(feedback);
            BottomArea.Add(feedback);
            return feedback;
        }

        public bool UpdateFeedbackMessage(string feedbackId, string msg)
        {
            var feedback = this.FindById(feedbackId);
            if (feedback == null || !(feedback is UIFeedback uIFeedback))
                return false;

            uIFeedback.UpdateText(msg);
            return true;
        }

        #endregion

        // TODO sostituire con il tipo corretto di messaggio
        //public virtual void ReceiveUpdate(object message) { }

        public void Dispose()
        {
            DetachContainer(this);
        }

        public void Attach()
        {
            AttachContainer(this);
        }
    }
}
