using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIChart : ContainerElement  // I figli di uno UIChart sono UIChartAxis e UIChartSignal
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UIChart));
        public UIChart(IUIContext context) : base(context) { }

        [JsonIgnore]
        public IEnumerable<UIChartAxis> AllXAxes => Children.OfType<UIChartAxis>().Where(x => x.Tag.ToString().StartsWith("x"));
        [JsonIgnore]
        public IEnumerable<UIChartAxis> AllYAxes => Children.OfType<UIChartAxis>().Where(x => x.Tag.ToString().StartsWith("y"));
        [JsonIgnore]
        public IEnumerable<UIChartSignal> AllSignals => Children.OfType<UIChartSignal>();


        public UIChartAxis AddXAxis(string title, string unit, double min, double max)
        {
            UIChartAxis axis = new UIChartAxis
            {
                Tag = $"x{AllXAxes.Count()}",
                Title = title,
                Unit = unit,
                Min = min,
                Max = max, 
                Position = "bottom"
            };

            if (AllXAxes.Count() < 2)
            {
                Add(axis);
            }
            else
            {
                _logger.Error("There is a maximum of 2 X axes in a chart");
                throw new Exception("There is a maximum of 2 X axes in a chart");
            }
            return axis;
        }

        public UIChartAxis AddYAxis(string title, string unit, double min, double max)
        {
            UIChartAxis axis = new UIChartAxis
            {
                Tag = $"y{AllYAxes.Count()}",
                Title = title,
                Unit = unit,
                Min = min,
                Max = max, 
                Position = "left"
            }; 

            Add(axis);
            return axis;
        }

        public UIChartSignal AddSignal(string tag, string title)
        {
            return AddSignal(tag, title, null);
        }

        public UIChartSignal AddSignal(string tag, string title, string color)
        {
            if (AllSignals.Count() >= 8)
            {
                _logger.Error("Max 8 signal are allowed for each chart");
                throw new Exception("Max 8 signal are allowed for each chart");
            }

            UIChartSignal signal = new UIChartSignal(tag, title, color);
            if (AllXAxes.Count() > 0)
                signal.XId = AllXAxes.First().Id;
            else
                throw new InvalidOperationException("You must create a X axis before adding a Signal.");
            Add(signal);
            return signal;
        }
        

        public bool SendUpdate(string signalTag, double time, double newValue)
        {
            var signal = AllSignals.FirstOrDefault(x => x.Tag.ToString() == signalTag);
            if (signal != null)
            {
                signal.SendUpdate(time, newValue);
                return true;
            }
            else
            {
                _logger.Error($"Signal with tag {signalTag} could not be found and SendUpdate was not performed");
                return false;
            }
        }
    }
}
