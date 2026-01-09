using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UIFrameworkDotNet
{
    public enum FeedbackMode
    {
        Countdown,
        ProgressBar,
        Message
    }

    // TODO createsectioncountdown si può rimuovere e usare AddFeedbackCountdown? diciamo di farlo?
    // TODO CreatePageCountdown si può rimuovere? Usata solo in GENERIC_ASK_WRITING(snippet)

    public class UIFeedback : UIElement
    {
        // Backing field for the event delegate
        private EventHandler<int> _tickElapsed;

        // Custom event with controlled add/remove
        public event EventHandler<int> TickElapsed
        {
            add
            {
                if (_tickElapsed != null)
                {
                    throw new InvalidOperationException("TickElapsed already has a subscriber.");
                }
                _tickElapsed = value;
            }
            remove
            {
                _tickElapsed = value;
            }
        }
        
        public UIFeedback(FeedbackMode feedbackMode, string style, string text, int ms)
        {
            Tag = $"feedback-{style}";
            Props["style"] = new Style
            {
                Layout = style
            };
            Mode = feedbackMode.ToString();
            Totalseconds = ms / 1000;
            Text = feedbackMode == FeedbackMode.Countdown ? (int)Math.Round(ms / 1000d) + " seconds remaining" : text;
            Remaining = ms / 1000;
            Percentage = 0;
        }

        public UIFeedback(FeedbackMode feedbackMode, string style, string text)
        {
            Props["style"] = new Style
            {
                Layout = style
            };
            Tag = $"feedback-{style}";
            Mode = feedbackMode.ToString();
            Text = text;
        }

        public UIFeedback(FeedbackMode feedbackMode, string style, string text, int ms, bool isManual)
        {
            Props["style"] = new Style
            {
                Layout = style
            };
            Tag = $"feedback-{style}";
            Mode = feedbackMode.ToString();
            Totalseconds = ms / 1000;
            IsManual = isManual;
            Text = feedbackMode == FeedbackMode.Countdown ? (int)Math.Round(ms / 1000d) + " seconds remaining" : text;
            Remaining = ms / 1000;
            Percentage = 0;
        }

        private System.Timers.Timer _timer;
        
        public bool StartCountdown()
        {
            _timer = new System.Timers.Timer(1000);
            var tickCommand = new FeedbackTickChangedCommand(this);
            _timer.Start();
            _timer.Elapsed += (_, __) => Timer_Elapsed(_, __, tickCommand);
            return true;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, FeedbackTickChangedCommand command = null)
        {
            if (Remaining > 0)
            {
                command?.Execute(default);
                _tickElapsed?.Invoke(this, Remaining);
                Console.WriteLine("Timer_Elapsed executed");
            }
            else
            {
                _timer?.Stop();
                Console.WriteLine("Timer_Elapsed stopped");
            }
        }

        public bool StopCountdown()
        {
            _timer.Stop();
            _timer.Elapsed -= (_, __) => Timer_Elapsed(_, __);
            return true;
        }

        public bool RestartCountdown()
        {
            StopCountdown();
            Remaining = Totalseconds;
            Percentage = 0;
            return StartCountdown();
        }

        private string _mode;
        [JsonIgnore]
        public string Mode
        {
            get => _mode;
            set
            {
                if (_mode == value) return;
                _mode = value;
                Props["mode"] = _mode;
                OnPropertyChanged(nameof(Mode));
            }
        }

        private double _percentage;
        [JsonIgnore]
        public double Percentage
        {
           get => _percentage;
           set 
           {
                if (_percentage == value) return;
                _percentage = value;
                States["percentage"] = 100 - Remaining * 100 / Totalseconds; 
                OnPropertyChanged(nameof(Percentage)); 
           }
        }

        [JsonIgnore]
        public int Milliseconds
        {
            get => (int)Props["totalSeconds"] * 1000;
        }

        private int _totalSeconds;
        [JsonIgnore]
        public int Totalseconds
        {
            get => _totalSeconds;
            set
            {
                if (_totalSeconds == value) return;
                _totalSeconds = value;
                Props["totalSeconds"] = value;
                OnPropertyChanged(nameof(Totalseconds));
            }
        }

        private bool _isManual;
        [JsonIgnore]
        public bool IsManual
        {
            get => _isManual;
            set
            {
                if (_isManual == value) return;
                _isManual = value;
                Props["isManual"] = _isManual;
                OnPropertyChanged(nameof(IsManual));
            }
        }

        private int _remaining;
        [JsonIgnore]
        public int Remaining
        {
            get => _remaining;
            set 
            { 
                if (_remaining == value) return;
                _remaining = value;
                States["remaining"] = value; 
                OnPropertyChanged(nameof(Remaining)); 
            }
        }

        [JsonIgnore]
        public bool IsActive
        {
            get => _timer != null && _timer.Enabled;
        }

        private string _text;
        [JsonIgnore]
        public string Text
        {
            get => _text;
            set 
            {
                if (_text == value) return;
                _text = value;
                States["text"] = value; 
                OnPropertyChanged(nameof(Text)); 
            }
        }
    }

    public class FeedbackTickChangedCommand : ICommand
    {
        private readonly UIFeedback _feedback;

        public FeedbackTickChangedCommand(UIFeedback feedback)
        {
            _feedback = feedback;
        }

        public void Execute(object newValue)
        {
            // Gli aggiornamenti delle percentuali devono sempre passare un newValue
            if (newValue != null)
            {
                switch (_feedback.Mode)
                {
                    case "Countdown":
                        _feedback.Remaining = Convert.ToInt32(newValue);
                        _feedback.Percentage = 100 - _feedback.Remaining * 100 / _feedback.Totalseconds;
                        break;
                    case "ProgressBar":
                        _feedback.Percentage = Convert.ToDouble(newValue);
                        break;
                    default:
                        throw new InvalidOperationException($"You can only use {nameof(FeedbackTickChangedCommand)} with \"Countdown\" or \"ProgressBar\" mode");
                }
            }
            else
            {   // Caso StartCountdown gestito da eslx/C#
                switch (_feedback.Mode)
                {
                    case "Countdown":
                        if (_feedback.Remaining <= 0)
                            return;
                        _feedback.Remaining--;
                        _feedback.Percentage = 100 - _feedback.Remaining * 100 / _feedback.Totalseconds;
                        break;
                    default:
                        throw new InvalidOperationException($"You can only use {nameof(FeedbackTickChangedCommand)} with \"Countdown\" mode");
                }
            }
        }
    }
}
