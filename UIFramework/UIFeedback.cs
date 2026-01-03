using Newtonsoft.Json;

namespace UIFramework
{
    public enum FeedbackMode
    {
        Countdown,
        ProgressBar
    }
    public class UIFeedback : UIElement
    {
        public event EventHandler<int> TickElapsed;
        public UIFeedback(FeedbackMode feedbackMode, string text, int milliseconds)
        {
            Props["mode"] = feedbackMode.ToString();
            Props["text"] = feedbackMode == FeedbackMode.Countdown ? (int)Math.Round(milliseconds / 1000d) + " seconds remaining" : text;
            Props["totalSeconds"] = milliseconds / 1000;
            States["remaining"] = milliseconds / 1000;
            States["percentage"] = 0;
        }

        public UIFeedback(FeedbackMode feedbackMode, string text)
        {
            Props["mode"] = feedbackMode.ToString();
            Props["text"] = text;
        }

        public UIFeedback(FeedbackMode feedbackMode, string text, int milliseconds, bool isManual)
        {
            Props["mode"] = feedbackMode.ToString();
            Props["text"] = feedbackMode == FeedbackMode.Countdown ? (int)Math.Round(milliseconds / 1000d) + " seconds remaining" : text;
            States["remaining"] = milliseconds / 1000;
            Props["totalSeconds"] = milliseconds / 1000;
            States["isManual"] = isManual;
            States["percentage"] = 0;
        }

        private System.Timers.Timer timer;

        public void StartCountdown()
        {
            timer = new System.Timers.Timer(1000);
            var tickCommand = new FeedbackTickCommand(this);
            timer.Start();
            timer.Elapsed += (_, _) =>
            {
                if (Remaining > 0)
                {
                    tickCommand.Execute();
                    TickElapsed?.Invoke(this, Remaining);
                    
                }
                else
                {
                    timer?.Stop();
                }
            };
        }

        public void StopCountdown()
        {
            timer?.Stop();
        }

        [JsonIgnore]
        public int Percentage
        {
           get => (int)States["percentage"];
           set => States["percentage"] = 100 - Remaining * 100 / Totalseconds;
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
        public int Remaining
        {
            get => (int)States["remaining"];
            set => States["remaining"] = value;
        }

        [JsonIgnore]
        public bool IsActive
        {
            get => timer != null && timer.Enabled;
        }
    }

    public class FeedbackTickCommand : ICommand
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
}
