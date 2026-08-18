
using BaseCustomApp.Helpers;

namespace UIFramework
{
    public class UIPropertyChange
    {
        public string ElementId { get; set; }
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
        public UIPropertyChange(string elementId, string propertyName, object propertyValue)
        {
            ElementId = elementId;
            PropertyName = propertyName.ToCamelCase();
            PropertyValue = propertyValue;
        }
    }
}
