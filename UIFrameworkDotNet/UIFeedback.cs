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
            Props["tag"] = $"feedback-{style}";
            Props["style"] = new Style
            {
                Layout = style
            };
            Props["mode"] = feedbackMode.ToString();
            States["text"] = feedbackMode == FeedbackMode.Countdown ? (int)Math.Round(ms / 1000d) + " seconds remaining" : text;
            Props["totalSeconds"] = ms / 1000;
            States["remaining"] = ms / 1000;
            States["percentage"] = 0;
        }

        public UIFeedback(FeedbackMode feedbackMode, string style, string text)
        {
            Props["style"] = new Style
            {
                Layout = style
            };
            Props["tag"] = $"feedback-{style}";
            Props["mode"] = feedbackMode.ToString();
            States["text"] = text;
        }

        public UIFeedback(FeedbackMode feedbackMode, string style, string text, int ms, bool isManual)
        {
            Props["style"] = new Style
            {
                Layout = style
            };
            Props["tag"] = $"feedback-{style}";
            Props["mode"] = feedbackMode.ToString();
            States["text"] = feedbackMode == FeedbackMode.Countdown ? (int)Math.Round(ms / 1000d) + " seconds remaining" : text;
            States["remaining"] = ms / 1000;
            Props["totalSeconds"] = ms / 1000;
            Props["isManual"] = isManual;
            States["percentage"] = 0;
        }

        private System.Timers.Timer _timer;
        
        public bool StartCountdown()
        {
            _timer = new System.Timers.Timer(1000);
            var tickCommand = new FeedbackTickCommand(this);
            _timer.Start();
            _timer.Elapsed += (_, __) => Timer_Elapsed(_, __, tickCommand);
            return true;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, FeedbackTickCommand command = null)
        {
            Console.WriteLine("Timer_Elapsed called");
            if (Remaining > 0)
            {
                command?.Execute();
                _tickElapsed?.Invoke(this, Remaining);
            }
            else
            {
                _timer?.Stop();
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


        [JsonIgnore]
        public double Percentage
        {
           get => (double)States["percentage"];
           set { States["percentage"] = 100 - Remaining * 100 / Totalseconds; OnPropertyChanged(nameof(Percentage)); }
        }

        [JsonIgnore]
        public int Milliseconds
        {
            get => (int)Props["totalSeconds"] * 1000;
        }

        [JsonIgnore]
        public int Totalseconds
        {
            get => (int)Props["totalSeconds"];
        }

        [JsonIgnore]
        public bool IsManual
        {
            get => Props.TryGetValue("isManual", out var v) && (bool)v;
        }

        [JsonIgnore]
        public int Remaining
        {
            get => (int)States["remaining"];
            set { States["remaining"] = value; OnPropertyChanged(nameof(Remaining)); }
        }

        [JsonIgnore]
        public bool IsActive
        {
            get => _timer != null && _timer.Enabled;
        }

        [JsonIgnore]
        public string Text
        {
            get => States.ContainsKey("text") ? States["text"].ToString() : "";
            set { States["text"] = value; OnPropertyChanged(nameof(Text)); }
        }
    }

    public class FeedbackTickCommand : ICommandOld
    {
        private readonly UIFeedback _feedback;

        public FeedbackTickCommand(UIFeedback feedback)
        {
            _feedback = feedback;
        }

        public void Execute()
        {
            if (_feedback.Remaining <= 0)
                return;
            _feedback.Remaining--;
            _feedback.Percentage = 100 - _feedback.Remaining * 100 / _feedback.Totalseconds;
        }
    }

    public class FeedbackChangedCommand : ICommandOld
    {
        private readonly UIFeedback _feedback;
        private readonly Dictionary<string, object> _states;

        public FeedbackChangedCommand(UIFeedback feedback, Dictionary<string, object> states)
        {
            _feedback = feedback;
            _states = states;
        }

        public void Execute()
        {
            _feedback.UpdateStates(_states);
        }
    }
}
