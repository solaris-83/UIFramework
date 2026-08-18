using Newtonsoft.Json;
using UIFramework.Interfaces;

namespace UIFramework.UIElements
{
    public class UIChartAxis : UIHeadingElement
    {
        public UIChartAxis()
        {
            Title = "";
            SubTitle = "";
            Position = "";
            Min = 0;
            Max = 0;
            Step = 0;
            Unit = "";
            IsAutoScaling = false;
        }

        private string _position;
        [JsonIgnore]
        public string Position
        {
            get => _position;
            set => SetPropsProperty(ref _position, value, nameof(Position));
        }

        private double _min;
        [JsonIgnore]
        public double Min
        {
            get => _min;
            set => SetPropsProperty(ref _min, value, nameof(Min));
        }

        private double _max;
        [JsonIgnore]
        public double Max
        {
            get => _max;
            set => SetPropsProperty(ref _max, value, nameof(Max));
        }

        private double _step;
        [JsonIgnore]
        public double Step
        {
            get => _step;
            set => SetPropsProperty(ref _step, value, nameof(Step));
        }


        private string _unit;
        [JsonIgnore]
        public string Unit
        {
            get => _unit;
            set => SetPropsProperty(ref _unit, value, nameof(Unit));
        }

        private bool _isAutoScaling; // TODO MARCO mettere le 2 property in una classe ad hoc ChartXAxis?
        [JsonIgnore]
        public bool IsAutoScaling
        {
            get => _isAutoScaling;
            set => SetPropsProperty(ref _isAutoScaling, value, nameof(IsAutoScaling));
        }
    }
}
