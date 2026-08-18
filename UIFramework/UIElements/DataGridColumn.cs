using System;

namespace UIFramework.UIElements
{
    public class DataGridColumn
    {
        public DataGridColumn()
        {
            
        }
        public DataGridColumn(string name, string header, /*string elementType, string bindingProperty,*/ string cssClassName, string appearance)
        {
            Name = name;
            Header = header;
            //ElementType = elementType;
            //BindingProperty = bindingProperty;
            CssClassName = cssClassName;
            Appearance = appearance;
        }

        public string Name { get; }
        public string Header { get; set; }
        //public string ElementType { get; set; }
        //public string BindingProperty { get; set; }
        public string CssClassName { get; set; }
        public string Appearance { get; set; }
    }
}
