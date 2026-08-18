using BaseCustomApp.Helpers;
using log4net;
using Newtonsoft.Json;
using ScriptLibraries.Data.Interfaces;
using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using UIFramework.Helpers;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    enum OrientationEnum
    {
        [Description("vertical")]
        Vertical,
        [Description("horizontal")]
        Horizontal
    }

    enum WrapEnum
    {
        [Description("overflow")]
        Overflow,
        [Description("noscroll")]
        Noscroll
    }

    public class UISection : GridContainerElement
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UISection));
        private readonly ISpecialAppDirectories _directories;

        public UISection(int rows, int columns, IUIContext context) : base(context)
        {
            Grid = new Grid();
            Grid.GridChanged += OnGridChanged;
            Grid.Rows = rows;
            Grid.Columns = columns;
            GridPosition = new GridPosition(0, 0, 1, 1);
            Orientation = OrientationEnum.Vertical.GetDescription();
            _directories = context.GlobalVariables?.Directories;
        }

        private void OnGridChanged(object sender, EventArgs e)
        {
            if (sender is Grid grid)
            {
                if (grid.Rows > 1 || grid.Columns > 1)
                    Wrap = WrapEnum.Noscroll.GetDescription();
                else
                    Wrap = WrapEnum.Overflow.GetDescription();
            };
        }

        public UISection(IUIContext context) : this (1, 1, context) { }

        public bool Add(UIElement child, int rowIndex, int columnIndex)
        {
            if (rowIndex < 0 || columnIndex < 0)
            {
                throw new ArgumentOutOfRangeException("RowIndex and ColumnIndex must be non-negative.");
            }
            base.Add(child ?? throw new ArgumentNullException(nameof(child)));
            child.GridPosition.RowIndex = rowIndex;
            child.GridPosition.ColumnIndex = columnIndex;

            return true;
        }

        override public void Dispose()
        {
            base.Dispose();
            Grid.GridChanged -= OnGridChanged;
        }

        #region Props

        private string _orientation;
        [JsonIgnore]
        public string Orientation
        {
            get => _orientation;
            set
            {
                SetPropsProperty(ref _orientation, value, nameof(Orientation));
            }
        }

        private string _wrap;
        [JsonIgnore]
        public string Wrap  // previste valori: "overflow", "noscroll"
        {
            get => _wrap;
            set
            {
                SetPropsProperty(ref _wrap, value, nameof(Wrap));
            }
        }

        #endregion

        #region States

        private string _isScrolledToEnd;
        [JsonIgnore]
        public string IsScrolledToEnd
        {
            get => _isScrolledToEnd;
            set
            {
                SetStatesProperty(ref _isScrolledToEnd, value, nameof(IsScrolledToEnd));
            }
        }
        #endregion

        #region SHORTCUT METHODS FOR ESLX

        public UIImage AddImage(string imageName)
        {
            var image = new UIImage(imageName);
            string imagePath = Path.Combine(_directories.ModuleFolder, "Resources", "img", imageName);
            if (!File.Exists(imagePath))
            {
                 _logger.Error($"Image not found in the Working Unit {image.Source}");
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

        public UILabel AddOrderedItem(string idStr, string style, string letter)
        {
            UILabel label = new UILabel(idStr);
            label.Style = new Style() { Appearance = style };
            label.Tag = letter;
            this.Add(label);
            return label;
        }

        public UILabel AddOrderedItem(string idStr, string letter)
        {
            return AddOrderedItem(idStr, "list-item-ordered", letter);
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

        public UILabel AddOrderedItem(string idStr)
        {
            return AddOrderedItem(idStr, style: "list-item-ordered", -1);
        }

        public bool UpdateOrderedItem(string itemId, string newIdStr)
        {
            var item = this.FindById(itemId);
            if (item == null || !(item is UILabel label))
                return false;
            label.UpdateText(newIdStr);
            return true;
        }

        public UILabel AddParagraph(string idStr)
        {
            return AddParagraph(idStr, css: ElementStatusEnum.Valid.GetDescription(), "");
        }

        public UILabel AddParagraph(string idStr, string css, string color)
        {
            var label = CreateParagraph(idStr, css, color);
            this.Add(label);
            return label;
        }

        public bool UpdateParagraph(string paragraphId, string newIdStr)
        {
            // return UpdateParagraph(paragraphId, newIdStr, "", "");
            var paragraph = this.FindById(paragraphId);
            if (paragraph == null || !(paragraph is UILabel label))
                return false;
            //if (!string.IsNullOrEmpty(style))
            //    label.Style.Appearance = style;
            //if (!string.IsNullOrEmpty(color))
            //    label.Style.ForegroundColor = color;
            label.UpdateText(newIdStr);
            return true;
        }

        // Questo non serve, Style è immodificabile (è una Props al momento)
        //public bool UpdateParagraph(string paragraphId, string newIdStr, string style, string color)
        //{
        //    var paragraph = this.FindById(paragraphId);
        //    if (paragraph == null || !(paragraph is UILabel label))
        //        return false;
        //    if (!string.IsNullOrEmpty(style))
        //        label.Style.Appearance = style;
        //    if (!string.IsNullOrEmpty(color))
        //        label.Style.ForegroundColor = color;
        //    label.UpdateText(newIdStr);
        //    return true;
        //}

        // I buttons sono peculiari nella CommandArea, non è possibile aggiungerli in una section
        //#region ADD BUTTON  
        
        //public UIButton AddButton(string id)
        //{
        //    var button = new UIButton(id, true, "", id); // non c'è uno stile per il default??
        //    this.Add(button);
        //    return button;
        //}

        //public UIButton AddButton(string id, bool isEnabled, string style)
        //{
        //    var button = new UIButton(id, isEnabled, style, id);
        //    this.Add(button);
        //    return button;
        //}

        //public UIButton AddButton(string id, bool isEnabled)
        //{
        //    var button = new UIButton(id, isEnabled, "", id);
        //    this.Add(button);
        //    return button;
        //}

        //public UIButton AddButton(string id, string text)
        //{
        //    var button = new UIButton(id, true, "", text);
        //    this.Add(button);
        //    return button;
        //}

        //public UIButton AddButton(string id, string text, bool isEnabled)
        //{
        //    var button = new UIButton(id, isEnabled, "standard", text);
        //    this.Add(button);
        //    return button;
        //}
        //#endregion

        #endregion

        #region Private methods

        public UILabel CreateParagraph(string idStr, string css, string color)
        {
            var label = new UILabel(idStr)
            {
                Style = new Style() { Appearance = "paragraph", CssClassName = css, ForegroundColor = color }
            };
            return label;
        }

        #endregion
    }
}
