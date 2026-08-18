using log4net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public sealed class UIDataGrid : ContainerElement
    {
        private readonly IUIContext _context;

        [JsonIgnore]
        public DataGridTemplate Template { get; set; }

        [JsonIgnore]
        public IEnumerable<UIDataGridRow> Rows => Children.Where(r => r.GetType() == typeof(UIDataGridRow)).Cast<UIDataGridRow>();


        public UIDataGrid(DataGridTemplate template, IUIContext context) : base(context)
        {
            _context = context;
            Template = template;
            CreateHeader();
        }

        public UIDataGridHeaderRow GetHeader()
        {
            return Children.OfType<UIDataGridHeaderRow>().SingleOrDefault();
        }

        public UIDataGridRow GetRow(int index)
        {
            if (index < Rows.Count())
                return Rows.ElementAt(index);
            else
                throw new System.Exception($"Index {index} is greater than the number of rows in the UIDataGrid");
        }

        public bool AddRow(UIDataGridRow row)
        {
            base.Add(row);
            return true;
        }

        /*public UIDataGridRow AddRow(params object[] rowValues)
        {
            int i = 0;
            var row = new UIDataGridRow(_context);
            foreach (var column in Template.Columns)
            {
                object item = null;
                UIElement element = null;
                if (i < rowValues.Length)
                {
                    //var type = System.Type.GetType("UIFramework.UIElements."+column.ElementType);
                    //var element = (UIElement)Activator.CreateInstance(type);
                    item = rowValues[i];
                    element = CreateInstance(column.ElementType, item);
                    if (!string.IsNullOrEmpty(column.BindingProperty))
                    {
                        var currentValue = PropertyPathResolver.GetPropertyValue(element, column.BindingProperty) ?? throw new InvalidOperationException($"Property '{column.BindingProperty}' not found on element {element.Type} {element.Id}");
                        var convertedValue = Convert.ChangeType(currentValue, item.GetType());

                        SetElementValue(element, column.BindingProperty, convertedValue);
                    }
                }
                else
                {
                    element = CreateInstance(column.ElementType, string.Empty);
                }

                row.AddCell(column.Name, element);
                i++;
            }
            
            Add(row);

            return row;
        }

        
        private UIElement CreateInstance(string elementType, params object[] parameters)
        {
            UIElement element;
            switch (elementType)
            {
                case "UILabel":
                    element = new UILabel(parameters[0].ToString());
                    break;
                case "UITextbox":
                    element = new UITextbox(parameters[0].ToString());
                    break;
                case "UIStatus":
                    element = new UIStatus();
                    break;
                case "UIButton":
                    element = new UIButton(parameters[0].ToString(), true, ElementStatusEnum.Primary.GetDescription(), parameters[0].ToString());
                    break;
                default: 
                    throw new InvalidOperationException($"{elementType} is not supported.");
            }
            return element;
        }

        private void SetElementValue(UIElement element, string bindingProperty, object value)
        {
            //var prop = element.GetType().GetProperty(bindingProperty);

            //if (prop != null)
            //    prop.SetValue(element, value?.ToString());

            var prop = element.GetType().GetProperty(bindingProperty) ?? throw new InvalidOperationException($"Property '{bindingProperty}' not found");
            if (prop != null && prop.CanWrite)
            {
                try
                {
                    var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                    prop.SetValue(element, convertedValue);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Property {bindingProperty} could not be updated with value {value}", ex);
                }
            }
            else
            {
                _logger.Error($"Property {bindingProperty} is either NULL or cannot be written");
            }
        }*/

        private void CreateHeader()
        {
            var header = new UIDataGridHeaderRow(_context);

            foreach (var column in Template.Columns)
            {
                var label = new UILabel(column.Header);
                if (!string.IsNullOrWhiteSpace(column.Appearance) || !string.IsNullOrWhiteSpace(column.CssClassName))
                {
                    if (!string.IsNullOrWhiteSpace(column.CssClassName))
                        label.Style.CssClassName = column.CssClassName;
                    if (!string.IsNullOrWhiteSpace(column.Appearance))
                        label.Style.Appearance = column.Appearance;
                }
                else
                    label.SetAppearance("header");
                header.AddCell(column.Name, label);
            }

            Add(header);
        }
    }
}
