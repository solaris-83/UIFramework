using BaseCustomApp.Helpers;
using log4net;
using Newtonsoft.Json;
using System;
using UIFramework.Validation;

namespace UIFramework.UIElements
{
    public sealed class UINumericBox : UIInputBox
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UINumericBox));

        public UINumericBox(double val) : base(false)
        {
            Value = val;
            StepSize = 1;
        }

        public UINumericBox() : this(val: 0)
        { 
        }

        #region Props

        private bool _showSpinners;
        [JsonIgnore]
        public bool ShowSpinners
        {
            get => _showSpinners;
            set => SetPropsProperty(ref _showSpinners, value, nameof(ShowSpinners));
        }

        private int _stepSize;
        [JsonIgnore]
        public int StepSize
        {
            get => _stepSize;
            set => SetPropsProperty(ref _stepSize, value, nameof(StepSize));
        }

        private double _minValue = double.MinValue;
        [JsonIgnore]
        public double MinValue
        {
            get => _minValue;
            set
            {
                if (_minValue != value)
                {
                    _minValue = value;
                    var minValidationRule = CheckIfValidationRuleExists(nameof(MinValueValidationRule));
                    if (minValidationRule != null)
                    {
                        if (minValidationRule is MinValueValidationRule minValRule)
                        {
                            minValRule.MinValue = _minValue;
                        }
                        else
                        {
                            _logger.Error("ValidationRule cannot be casted to MinLengthValidationRule");
                            throw new Exception("ValidationRule cannot be casted to MinLengthValidationRule");
                        }
                    }
                    else
                    {
                        AddValidationRule(new MinValueValidationRule(_minValue));
                    }
                    Props["minValue"] = _minValue;
                }
            }
        }

        private double _maxValue = double.MaxValue;
        [JsonIgnore]
        public double MaxValue
        {
            get => _maxValue;
            set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;
                    var maxValidationRule = CheckIfValidationRuleExists(nameof(MaxValueValidationRule));
                    if (maxValidationRule != null)
                    {
                        if (maxValidationRule is MaxValueValidationRule maxValRule)
                        {
                            maxValRule.MaxValue = _maxValue;
                        }
                        else
                        {
                            _logger.Error("ValidationRule cannot be casted to MaxLengthValidationRule");
                            throw new Exception("ValidationRule cannot be casted to MaxLengthValidationRule");
                        }
                    }
                    else
                    {
                        AddValidationRule(new MaxValueValidationRule(_maxValue));
                    }
                    Props["maxValue"] = _maxValue;
                }
            }
        }

        #endregion

        #region States

        private double _value = double.MinValue;
        [JsonIgnore] 
        public double Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;

                    if (IsReadOnly)
                        Props["value"] = _value;
                    else
                    {
                        States["value"] = _value;
                        OnPropertyChanged(Id, nameof(Value), _value);
                    }
                    ApplyValidationRules(_value);
                }
            }
        }



        #endregion

        public string ToHex()
        {
            return Value.ToString("X2");
        }

        public byte[] ToBytes()
        {
            return Conversions.FromHexToBytes(ToHex());
        }
    }
}
