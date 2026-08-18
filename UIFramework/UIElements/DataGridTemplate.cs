using System;
using System.Collections.Generic;

namespace UIFramework.UIElements
{
    public class DataGridTemplate
    {
        public List<DataGridColumn> Columns { get; set; }

        public DataGridTemplate()
        {
            Columns = new List<DataGridColumn>();
        }

        public DataGridColumn AddColumn(string headerKey, /*string elementType, string bindingProperty,*/ string cssClassName, string appearance)
        {
            if (Columns.Find(col => col.Name == headerKey) != null)
            {
                throw new Exception($"Column {headerKey} has been already added");
            }
            var column = new DataGridColumn(name: headerKey, header: headerKey, cssClassName, appearance);
            Columns.Add(column);
            return column;
        }

        public DataGridColumn AddColumn(string headerKey)
        {
            return AddColumn(headerKey, "", "");
        }
    }
}
