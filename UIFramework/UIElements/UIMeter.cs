using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIMeter : UIHeadingElement
    {
        public UIMeter(string tag)
        {
            Tag = tag;
            WarningRanges = new List<Range>();
            ErrorRanges = new List<Range>();
            ValidRanges = new List<Range>();
            Title = "";
            SubTitle = "";
            Precision = 0;
            Unit = "";
            Delta = 0;
            ConversionFactor = 1;
            Offset = 0;
            Point = new Point(0, 0);
            ValueStatus = ElementStatusEnum.Normal.GetDescription();
            Min = 0;
            Max = 100;
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
       
        private string _valueStatus;
        [JsonIgnore]
        public string ValueStatus
        {
            get => _valueStatus;
            set => SetStatesProperty(ref _valueStatus, value, nameof(ValueStatus));
        }

        private double _min = 0;
        [JsonIgnore]
        public double Min
        {
            get => _min;
            set => SetPropsProperty(ref _min, value, nameof(Min));
        }

        private double _max = 0;
        [JsonIgnore]
        public double Max
        {
            get => _max;
            set => SetPropsProperty(ref _max, value, nameof(Max));
        }

        private List<Range> _warningRanges;
        [JsonIgnore]
        public List<Range> WarningRanges
        {
            get => _warningRanges;
            set => SetPropsProperty(ref _warningRanges, value, nameof(WarningRanges));
        }

        private List<Range> _errorRanges;
        [JsonIgnore]
        public List<Range> ErrorRanges
        {
            get => _errorRanges;
            set => SetPropsProperty(ref _errorRanges, value, nameof(ErrorRanges));
        }

        private List<Range> _validRanges;
        [JsonIgnore]
        public List<Range> ValidRanges
        {
            get => _validRanges;
            set => SetPropsProperty(ref _validRanges, value, nameof(ValidRanges));
        }

        public bool AddWarningRange(double from, double to)
        {
            if (from >= to)
                throw new Exception("The 'from' value must be less than the 'to' value.");
            _warningRanges.Add(new Range() { From = from, To = to });
            WarningRanges = _warningRanges;
            return true;
        }

        public bool AddErrorRange(double from, double to)
        {
            if (from >= to)
                throw new Exception("The 'from' value must be less than the 'to' value.");
            _errorRanges.Add(new Range() { From = from, To = to });
            ErrorRanges = _errorRanges;
            return true;
        }

        public bool AddValidRange(double from, double to)
        {
            if (from >= to)
                throw new Exception("The 'from' value must be less than the 'to' value.");
            _validRanges.Add(new Range() { From = from, To = to });
            ValidRanges = _validRanges;
            return true;
        }

        private Point _point;
        [JsonIgnore]
        public Point Point
        {
            get => _point;
            set => SetProperty(ref _point, value, 
                () =>
                {
                    States["point"] = value;
                    ValueStatus = ErrorRanges.Exists(r => Point.Y >= r.From && Point.Y <= r.To) ? ElementStatusEnum.Error.GetDescription() : 
                                (WarningRanges.Exists(r => Point.Y >= r.From && Point.Y <= r.To) ? ElementStatusEnum.Warning.GetDescription() : 
                                (ValidRanges.Exists(r => Point.Y >= r.From && Point.Y <= r.To) ? ElementStatusEnum.Success.GetDescription() : ElementStatusEnum.Normal.GetDescription()));
                },
                nameof(Point));
        }

        public bool SendUpdate(double newValue)
        {
            Point = CreateNewPoint(0, newValue);
            return true;
        }

        public Point CreateNewPoint(double time, double newValue)
        {
            return new Point(time, Math.Round((newValue + Delta) * ConversionFactor + Offset, Precision));
        }
    }

    public class Range
    {
        public double From { get; set; }
        public double To { get; set; }
    }
}
