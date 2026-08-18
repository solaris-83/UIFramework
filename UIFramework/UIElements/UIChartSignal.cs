using Newtonsoft.Json;
using System;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIChartSignal : UIHeadingElement
    {
        public UIChartSignal(string tag, string title, string color)
        {
            Tag = tag;
            Title = title;
            SubTitle = "";
            Style.ForegroundColor = color;
            Precision = 0;
            Unit = "";
            Delta = 0;
            ConversionFactor = 1;
            XConversionFactor = 1;
            Offset = 0;
            Point = new Point(0, 0);
        }

        private int _precision;
        [JsonIgnore]
        public int Precision
        {
            get => _precision;
            set => SetPropsProperty(ref _precision, value, nameof(Precision));
        }

        private double _delta;
        [JsonIgnore]
        public double Delta
        {
            get => _delta;
            set => SetPropsProperty(ref _delta, value, nameof(Delta));
        }

        private double _conversionFactor;
        [JsonIgnore]
        public double ConversionFactor
        {
            get => _conversionFactor;
            set => SetPropsProperty(ref _conversionFactor, value, nameof(ConversionFactor));
        }

        private string _unit;
        [JsonIgnore]
        public string Unit
        {
            get => _unit;
            set => SetPropsProperty(ref _unit, value, nameof(Unit));
        }

        private double _offset;
        [JsonIgnore]
        public double Offset
        {
            get => _offset;
            set => SetPropsProperty(ref _offset, value, nameof(Offset));
        }

        private string _xId;
        [JsonIgnore]
        public string XId
        {
            get => _xId;
            set => SetPropsProperty(ref _xId, value, nameof(XId));
        }

        private string _yId;
        [JsonIgnore]
        public string YId
        {
            get => _yId;
            set => SetPropsProperty(ref _yId, value, nameof(YId));
        }



        private double _xConversionFactor;
        [JsonIgnore]
        public double XConversionFactor
        {
            get => _xConversionFactor;
            set => SetPropsProperty(ref _xConversionFactor, value, nameof(XConversionFactor));
        }

        public bool SetYAxis(UIChartAxis axis)
        {
            YId = axis.Id;
            return true;
        }

        private Point _point;
        [JsonIgnore]
        public Point Point
        {
            get => _point;
            set => SetStatesProperty(ref _point, value, nameof(Point));
        }

        public bool SendUpdate(double time, double newValue)
        {
            Point = CreateNewPoint(time, newValue);
            return true;
        }

        public Point CreateNewPoint(double time, double newValue)
        {
            return new Point(XConversionFactor != 1 ? Math.Round(time * XConversionFactor, 2) : time, Math.Round((newValue + Delta) * ConversionFactor + Offset, Precision));
        }
    }
}
