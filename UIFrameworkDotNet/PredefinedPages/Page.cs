
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using UIFrameworkDotNet.Helpers;

namespace UIFrameworkDotNet.PredefinedPages
{
    public class Page : ContainerElement, IDisposable
    {
        private UITabControl _tabControl;
        private UITab _currentTab;
        private const string STOP_BUTTON_TEXT = "STOP";

        public Page()
        {
            _tabControl = new UITabControl();
            Add(_tabControl);
        }

        private string _title;
        [JsonIgnore]
        public string Title
        {
            get => _title;
            set
            {
                if (value == null)
                    value = "Information";
                SetTitle(value, "info");
                _title = value;
            }
        }

        [JsonIgnore]
        public UITabControl TabControl
        {
            get { 
                if (_tabControl == null)
                    _tabControl = new UITabControl();
                 return _tabControl;
             }
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
        
        private void AttachElement(UIElement element)
        {
            element.PropertyChanged += OnPropertyChanged;

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
           // SendToJs(_diff.Add(parent.Id, child));
            AttachElement(child);
            _dataChanged?.Invoke(this, child.GetType());
        }

        private void OnItemRemoved(ContainerElement parent, UIElement child)
        {
            // SendToJs(_diff.Remove(child.Id));
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

        // Protected helper to allow derived classes to raise the event
        //protected virtual void OnUpdated(Type updatedType)
        //{
        //    _updated?.Invoke(this, updatedType);
        //}

        public UILabel SetTitle(string idStr, string style)
        {
            var titleLabel = new UILabel(idStr);
            var titleStyle = new Style() { Appearance = style };
            titleLabel.Style = titleStyle; // da qui evinco che è un title e quindi usa uno stile particolare
            Add(titleLabel);
            // OnUpdated(titleLabel.GetType());
            return titleLabel;
        }

        //// Per creare il titolo di una page creo una sezione e ci aggiungo una label
        //public bool SetTitle(string idStr, string style)
        //{
        //    var section = new UISection();
        //    var titleStyle = new Style() { Layout = style };
        //    section.SetStyle(titleStyle);
        //    var titleLabel = new UILabel(idStr);
        //    titleLabel.SetStyle(titleStyle); // da qui evinco che è un title e quindi usa uno stile particolare
        //    section.Add(titleLabel);
        //    Add(section);
        //    return true;
        //}

        // TODO UITab o bool?? Devo creare section?
        // TODO Il tab deve avere come primo parametro lo stile.
        // TODO non esiste la possibilità di mettere l'header?
        public UITab AddTab(string title, int rows, int cols)  // TODO UITab o bool?? Devo creare section?
        {
            _currentTab = new UITab(title, rows, cols);
            _tabControl.Add(_currentTab);
            _tabControl.ActiveTabId = _currentTab.Id;
            // non so se serve l'evento, per il momento non lo mettiamo
            return _currentTab;
        }
        
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
            _currentTab.Add(image);
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
            // OnUpdated(img.GetType());
            return true;
        }

        public UILabel AddBulletedItem(string idStr)
        {
            UILabel label = new UILabel(idStr); // TranslationsService.Instance.CurrentTranslations.GetLocalOrDefault(idStr);
            label.Style = new Style() { Appearance = "list-item-unordered" };
            _currentTab.Add(label);
            // OnUpdated(label.GetType());
            return label;
        }

        public bool UpdateBulletedItem(string itemId, string newIdStr)
        {
            var item = this.FindById(itemId);
            if (item == null || !(item is UILabel label))
                return false;
            label.Text = newIdStr;
            // OnUpdated(label.GetType());
            return true;
        }

        // "list-item-ordered" è in Style.Layout
        // index è in Tag
        public UILabel AddOrderedItem(string idStr, int index)
        {
            UILabel label = new UILabel(idStr); // TranslationsService.Instance.CurrentTranslations.GetLocalOrDefault(idStr);
            label.Style = new Style() { Appearance = "list-item-ordered" };
            label.Tag = index;
            _currentTab.Add(label);
            // OnUpdated(label.GetType());
            return label;
        }

        public UILabel AddOrderedItem(string idStr, string style, int index)
        {
            UILabel label = new UILabel(idStr); // TranslationsService.Instance.CurrentTranslations.GetLocalOrDefault(idStr);
            label.Style = new Style() { Appearance = style };
            label.Tag = index;
            _currentTab.Add(label);
            // OnUpdated(label.GetType());
            return label;
        }

        public UILabel AddOrderedItem(string idStr, string style)
        {
            UILabel label = new UILabel(idStr); // TranslationsService.Instance.CurrentTranslations.GetLocalOrDefault(idStr);
            label.Style = new Style() { Appearance = style };
           // label.Tag = index;
            _currentTab.Add(label);
            // OnUpdated(label.GetType());
            return label;
        }

        public UILabel AddOrderedItem(string idStr)
        {
            return AddOrderedItem(idStr, "");
        }

        public UILabel AddParagraph(string idStr)
        {
            return AddParagraph(idStr, "paragraph", "");
        }

        public UILabel AddParagraph(string idStr, string style, string color) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        {
            var label = new UILabel(idStr);
            label.Style = new Style() { Appearance = style, ForegroundColor = color };
            _currentTab.Add(label);
            // OnUpdated(label.GetType());
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
            label.Text = newIdStr;
            // OnUpdated(label.GetType());
            return true;
        }

        #region ADD BUTTON 
        public UIButton AddButton(string id)
        {
            var button = new UIButton(id, false, ""); // non c'è uno stile per il default??
            _currentTab.Add(button);
            // OnUpdated(button.GetType());
            return button;
        }

        public UIButton AddButton(string id, bool isEnabled, string style)
        {
            var button = new UIButton(id, isEnabled, style);
            _currentTab.Add(button);
            // OnUpdated(button.GetType());
            return button;
        }

        public UIButton AddButton(string id, bool isEnabled)
        {
            var button = new UIButton(id, isEnabled, "");
            _currentTab.Add(button);
            // OnUpdated(button.GetType());
            return button;
        }

        public UIButton AddButton(string id, string text)
        {
            var button = new UIButton(id, false, "", text);
            _currentTab.Add(button);
            // OnUpdated(button.GetType());
            return button;
        }

        public UIButton AddButton(string id, string text, bool isEnabled)
        {
            var button = new UIButton(id, isEnabled, "standard", text);
            _currentTab.Add(button);
            // OnUpdated(button.GetType());
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
            //button.Enabled = true;
           
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


        #region FEEDBACK
        public UIFeedback AddFeedbackCountdown(int ms)
        {
            return AddFeedbackCountdown(ms, true); // default is manual
        }
        public UIFeedback AddFeedbackCountdown(int ms, bool isManual)
        {
            var feedback = new UIFeedback(FeedbackMode.Countdown, "countdown", (ms * 1000).ToString() /* BasicLibraries.UTILITY.FormatDuration(ms * 1000)*/, ms, isManual);
            _currentTab.Add(feedback);
            // OnUpdated(feedback.GetType());
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
            // OnUpdated(uIFeedback.GetType());
            return true;
        }

        private void Feedback_TickElapsed(object sender, int e)
        {
            // OnUpdated(sender.GetType());
        }

        public UIFeedback AddFeedbackProgress(int perc)
        {
            var feedback = new UIFeedback(FeedbackMode.ProgressBar, "progress", "", perc);
            _currentTab.Add(feedback);
            // OnUpdated(feedback.GetType());
            return feedback;
        }

        public bool UpdateFeedbackProgress(string feedbackId, int perc, string msg)
        {
            var feedback = this.FindById(feedbackId);
            if (feedback == null || !(feedback is UIFeedback uIFeedback))
                return false;

            uIFeedback.Text = msg;
            uIFeedback.Percentage = perc;
            // OnUpdated(uIFeedback.GetType());
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
            var feedback = new UIFeedback(FeedbackMode.Message, "message", msg);
            _currentTab.Add(feedback);
            // OnUpdated(feedback.GetType());
            return feedback;
        }

        public bool UpdateFeedbackMessage(string feedbackId, string msg)
        {
            var feedback = this.FindById(feedbackId);
            if (feedback == null || !(feedback is UIFeedback uIFeedback))
                return false;

            uIFeedback.Text = msg;
            // OnUpdated(uIFeedback.GetType());
            return true;
        }

        #endregion

        // TODO sostituire con il tipo corretto di messaggio
        public virtual void ReceiveUpdate(object message) { }

        public void Dispose()
        {
            DetachContainer(TabControl);
        }
    }
}
