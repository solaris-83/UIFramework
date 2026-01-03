

using System.Xml.Linq;
using UIFramework.Helpers;

namespace UIFramework.PredefinedPages
{
    public class Page : ContainerElement
    {
        private UITabControl _tabControl;
        private UITab _currentTab;
        private const string STOP_BUTTON_TEXT = "STOP";
        public Page() 
        {
            _tabControl = new UITabControl();
            Add(_tabControl);
        }

        //protected string Id { get; init; } = Guid.NewGuid().ToString();

        //protected string Type { get; private set; }

        public event EventHandler<Page> UIElementUpdated;

        // Per creare il titolo di una page creo una sezione e ci aggiungo una label
        public bool SetTitle(string idStr, string style)
        {
            var section = new UISection();
            var titleStyle = new Style() { Layout = style };
            section.SetStyle(titleStyle);
            var titleLabel = new UILabel(idStr);
            titleLabel.SetStyle(titleStyle); // da qui evinco che è un title e quindi usa uno stile particolare
            section.Add(titleLabel);
            Add(section);
            return true;
        }

        public UITab AddTab(string title, int rows, int cols)  // TODO UITab o bool?? Devo creare section?
        {
            _currentTab = new UITab(title, rows, cols);
            _tabControl.Add(_currentTab);
            _tabControl.SelectedTabId = _currentTab.Id;
            return _currentTab;
        }

        public bool AddImage(string imageName)
        {
            var image = new UIImage(imageName);
            //string imagePath = Path.Combine(ExecutionResultManager.Global.Directories.ModuleFolder, "Resources", "img", imageName);
            string imagePath = Path.Combine("Resources", "img", imageName);
            if (!File.Exists(imagePath))
            {
               // _logger.Error($"Image not found in the Working Unit {element.Source}");
            }

            image.Src = ImageHelper.ConvertImageToBase64(imagePath);
            _currentTab.Add(image);
            return true;
        }

        public bool AddParagraph(string idStr)
        {
            return AddParagraph(idStr, "", "");
        }

        public bool AddParagraph(string idStr, string style, string color) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        {
            var label = new UILabel(idStr);
            label.SetStyle(new Style() { Layout = style, ForegroundColor = color });
            _currentTab.Add(label);
            return true;
        }

        #region ADD BUTTON 
        public bool AddButton(string id)
        {
            var button = new UIButton(id, false, ""); // non c'è uno stile per il default??
            _currentTab.Add(button);
            return true;
        }

        public bool AddButton(string id, bool isEnabled, string style)
        {
            var button = new UIButton(id, isEnabled, style);
            _currentTab.Add(button);
            return true;
        }

        public bool AddButton(string id, bool isEnabled)
        {
            var button = new UIButton(id, isEnabled, "");
            _currentTab.Add(button);
            return true;
        }

        public bool AddButton(string id, string text)
        {
            var button = new UIButton(id, false, "", text);
            _currentTab.Add(button);
            return true;
        }

        public bool AddButton(string id, string text, bool isEnabled)
        {
            var button = new UIButton(id, isEnabled, "standard", text);
            _currentTab.Add(button);
            return true;
        }
        #endregion

        #region STOP BUTTON (ABORT)
        public bool AddButtonStop()
        {
            return AddButton(STOP_BUTTON_TEXT, true, "danger");
        }

        public bool AddButtonStop(bool isEnabled)
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
            // TODO Mettere un warning se non ci si è registrati all'evento
            UIElementUpdated?.Invoke(button, this);
            return true;
        }

        public bool DisableStop()
        {
            var button = this.FindById(STOP_BUTTON_TEXT);
            if (button == null)
                return false;
            if (button is UIButton btn)
                btn.Enabled = false;
            // TODO Mettere un warning se non ci si è registrati all'evento
            UIElementUpdated?.Invoke(button, this);
            return true;
        }
        #endregion
    }
}
