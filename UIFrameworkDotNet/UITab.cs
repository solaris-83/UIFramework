using Newtonsoft.Json;
using System;
using System.Linq;

namespace UIFrameworkDotNet
{
    public class UITab : GridContainerElement
    {

        #region Props

        private string _title;
        //[JsonIgnore]

        //public string Title
        //{
        //    get => _title;
        //    set => SetPropsProperty(ref _title, value, nameof(Title));
        //}

        [JsonIgnore]
        public string Title
        {
            get => _title;
            set
            {
                SetTitle("title", value, "info");
            }
        }
        #endregion

        public UITab(int rows, int columns)
        {
            Grid = new Grid(rows, columns);
        }

        public UITab(string tag, int rows, int columns) : this(rows, columns)
        {
            Tag = tag;
        }
        

        public void Add(UIElement element, int row, int column)
        {
           // Add(element, new GridPosition(row, column));

            if (element is UISection section)
            {
                //Add(element, position.RowIndex, position.ColumnIndex);
                section.GridPosition.RowIndex = row;
                section.GridPosition.ColumnIndex = column;
                Add(section);
            }
            else
            {
                throw new InvalidOperationException("Only UISection elements can be added to UITab.");
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

        private UILabel CreateOrUpdateTitle(string tag, string idStr, string style)
        {
            var foundElement = this.Children.FirstOrDefault(f => f.Tag.ToString() == "title");
            if (foundElement != null && foundElement is UILabel title)
            {
                title.UpdateText(idStr);
                title.Tag = tag;
                title.Style.Appearance = style;
                return title;
            }
            else 
            {
                var label = new UILabel(idStr);
                label.Tag = tag;
                label.Style = new Style() { Appearance = style };
                Add(label);
                return label;
            }
        }

        /*
        public void Add(UIElement element, GridPosition position)
        {
            if (element is UISection section)
            {
                //Add(element, position.RowIndex, position.ColumnIndex);
                Add(section);
            }
            else
            {
                throw new InvalidOperationException("Only UISection elements can be added to UITab.");
            }
           // base.Add(element);
        }
        */
    }
}
