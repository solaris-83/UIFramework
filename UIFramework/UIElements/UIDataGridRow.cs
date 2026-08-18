using System.Collections.Generic;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIDataGridRow : ContainerElement
    {
        private readonly Dictionary<string, UIElement> _cells = new Dictionary<string, UIElement>();
        
        public UIDataGridRow(IUIContext context) : base(context)
        {
        }

        public bool AddCell(string columnName, UIElement element)
        {
            _cells[columnName] = element;
            Add(element);
            return true;
        }

        public UIElement GetCell(string columnName)
        {
            return _cells.TryGetValue(columnName, out var el) ? el : throw new System.Exception($"No UIElement found for column {columnName}.");
        }
    }

    public class UIDataGridHeaderRow : UIDataGridRow
    {
        public UIDataGridHeaderRow(IUIContext context) : base(context)
        {
        }
    }
}
