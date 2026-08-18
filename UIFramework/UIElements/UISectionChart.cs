using BaseCustomApp.Helpers;
using ScriptLibraries.Data.Interfaces;
using UIFramework.Interfaces;
using UIFramework.Interfaces.Adapters;

namespace UIFramework.UIElements
{
    public class UISectionChart : UISection, ISectionChartAdapter
    {
        private readonly IUIContext _uIContext;
        public UISectionChart(int rows, int columns, IUIContext context) : base(rows, columns, context)
        {
            _uIContext = context;
            Orientation = OrientationEnum.Horizontal.GetDescription();
            Wrap = WrapEnum.Noscroll.GetDescription();
        }

        public UISectionChart(IUIContext context) : this(1, 1, context) { }

        public UIChart AddChart()
        {
            return AddChart(0, 0);
        }

        public UIChart AddChart(int rowIndex, int columnIndex)
        {
            UIChart chart = new UIChart(_uIContext);
            Add(chart, rowIndex, columnIndex);
            return chart;
        }
    }
}
