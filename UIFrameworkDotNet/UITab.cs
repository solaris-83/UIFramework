using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UIFrameworkDotNet
{
    public class UITab : ContainerElement  // TODO su Tab ci vuole Title
    {
        private readonly Dictionary<string, GridPosition> _layout = new Dictionary<string, GridPosition>();

        private UISection _currentSection;

        [JsonIgnore]
        public UISection CurrentSection
        {
            get => _currentSection; 
            set => _currentSection = value; 
        }

        #region Props

        private int _rows;
        [JsonIgnore]
        public int Rows
        {
            get => _rows;
            set => SetPropsProperty(ref _rows, value, nameof(Rows));
        }

        private int _columns;
        [JsonIgnore]
        public int Columns
        {
            get => _columns;
            set => SetPropsProperty(ref _columns, value, nameof(Columns));

        }

        #endregion

        public UITab(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        public UITab(string tag, int rows, int columns) : this(rows, columns)
        {
            Tag = tag;
        }
        

        public void Add(UIElement element, int row, int column)
        {
            Add(element, new GridPosition(row, column));
        }

        public void Add(UIElement element, GridPosition position)
        {
            if (element is UISection section)
            {
                base.Add(section);
                CurrentSection = section;
                _layout[element.Id] = position;
            }
            else
            {
                throw new InvalidOperationException("Only UISection elements can be added to UITab.");
            }
            base.Add(element);
        }
    }
}
