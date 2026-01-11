using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UIFrameworkDotNet
{
    public class UITab : GridContainerElement  // TODO su Tab ci vuole Title
    {

        //private UISection _currentSection;

        //[JsonIgnore]
        //public UISection CurrentSection
        //{
        //    get => _currentSection; 
        //    set => _currentSection = value; 
        //}

        #region Props

        

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
