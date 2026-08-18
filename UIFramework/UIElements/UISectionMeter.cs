using BaseCustomApp.Helpers;
using ScriptLibraries.Data.Interfaces;
using UIFramework.Interfaces;
using UIFramework.Interfaces.Adapters;

namespace UIFramework.UIElements
{
    public class UISectionMeter : UISection, ISectionMeterAdapter
    {
        public UISectionMeter(int rows, int columns, IUIContext context) : base(rows, columns, context)
        {
            Orientation = OrientationEnum.Horizontal.GetDescription();
            Wrap = WrapEnum.Noscroll.GetDescription();
        }

        public UISectionMeter(IUIContext context) : this(1, 1, context) { }

        public UIGauge AddGauge(string tag)
        {
           return AddGauge(tag, 0, 0);
        }

        public UIGauge AddGauge(string tag, int rowIndex, int columnIndex)
        {
            UIGauge gauge = new UIGauge(tag);
            Add(gauge, rowIndex, columnIndex);
            return gauge;
        }

        public UIThermometer AddThermometer(string tag)
        {
            return AddThermometer(tag, 0, 0);
        }

        public UIThermometer AddThermometer(string tag, int rowIndex, int columnIndex)
        {
            UIThermometer thermo = new UIThermometer(tag);
            Add(thermo, rowIndex, columnIndex);
            return thermo;
        }
    }
}
