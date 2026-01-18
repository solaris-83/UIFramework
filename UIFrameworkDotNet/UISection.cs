using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UIFrameworkDotNet.Helpers;

namespace UIFrameworkDotNet
{
    public class UISection : GridPositionContainerElement
    {

        public UISection(int rows, int columns)
        {
            Grid = new Grid(rows, columns);
            GridPosition = new GridPosition(0, 0, 1, 1);
        }

        public UISection() : this (1, 1)
        {
            
        }
        public UISection(string title) : this(1, 1)
        {
           Title = title;
        }

        public void Add(UIElement child, int row, int column)
        {
            //if (_layout == null)
            //    _layout = new Dictionary<string, GridPosition>();
            base.Add(child);
            //_layout[child.Id] = new GridPosition(row, column);
        }

        #region Props

        //private int _rows;
        //[JsonIgnore]
        //public int Rows
        //{
        //    get => _rows;
        //    set => SetPropsProperty(ref _rows, value, nameof(Rows));
        //}

        //private int _columns;
        //[JsonIgnore]
        //public int Columns
        //{
        //    get => _columns;
        //    set => SetPropsProperty(ref _columns, value, nameof(Columns));
        //}

        //private int _rowSpan = 1;
        //[JsonIgnore]
        //public int RowSpan
        //{
        //    get => _rows;
        //    set => SetPropsProperty(ref _rowSpan, value, nameof(RowSpan));
        //}

        //private int _columnSpan = 1;
        //[JsonIgnore]
        //public int ColumnSpan
        //{
        //    get => _columnSpan;
        //    set => SetPropsProperty(ref _columnSpan, value, nameof(ColumnSpan));
        //}

        private string _title;
        [JsonIgnore]
        public string Title
        {
            get => _title;
            set 
            {
                SetPropsProperty(ref _title, value, nameof(Title));
            }
        }

        #endregion



        #region SHORTCUT METHODS FOR ESLX

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
            this.Add(image);
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
            this.Add(label);
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
            return AddOrderedItem(idStr, style: "list-item-ordered", index);
        }

        public UILabel AddOrderedItem(string idStr, string style, int index)
        {
            UILabel label = new UILabel(idStr);
            label.Style = new Style() { Appearance = style };
            if (index > -1)
                label.Tag = index;
            this.Add(label);
            return label;
        }

        public UILabel AddOrderedItem(string idStr, string style)
        {
            return AddOrderedItem(idStr, style, index: -1);
        }

        public UILabel AddOrderedItem(string idStr)
        {
            return AddOrderedItem(idStr, style: "");
        }

        public UILabel AddParagraph(string idStr)
        {
            return AddParagraph(idStr, "paragraph", "");
        }

        public UILabel AddParagraph(string idStr, string style, string color) // TODO capire dove inserire l'informazione "paragraph" utile per il JS
        {
            var label = new UILabel(idStr);
            label.Style = new Style() { Appearance = style, ForegroundColor = color };
            this.Add(label);
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

        #region ADD BUTTON  
        
        public UIButton AddButton(string id)
        {
            var button = new UIButton(id, false, ""); // non c'è uno stile per il default??
            this.Add(button);
            return button;
        }

        public UIButton AddButton(string id, bool isEnabled, string style)
        {
            var button = new UIButton(id, isEnabled, style);
            this.Add(button);
            return button;
        }

        public UIButton AddButton(string id, bool isEnabled)
        {
            var button = new UIButton(id, isEnabled, "");
            this.Add(button);
            return button;
        }

        public UIButton AddButton(string id, string text)
        {
            var button = new UIButton(id, false, "", text);
            this.Add(button);
            return button;
        }

        public UIButton AddButton(string id, string text, bool isEnabled)
        {
            var button = new UIButton(id, isEnabled, "standard", text);
            this.Add(button);
            return button;
        }
        #endregion

        #endregion
    }

    //public class UISectionChangedCommand : ICommand
    //{
    //    public readonly UISection _section;
    //    public UISectionChangedCommand(UISection section)
    //    {
    //        _section = section;
    //    }

    //    public void Execute(object newValue)
    //    {
    //        // Trovo il primo UILateralArea salendo a ritroso da section e cerco il bottone continue e  
    //        throw new System.NotImplementedException();
    //    }
    //}
}
