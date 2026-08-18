using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIFeedbackCountdown : UIFeedbackProgress
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UIFeedbackCountdown));

        public UIFeedbackCountdown(int ms, bool isManual) : base(ms)
        {
            Milliseconds = ms;
            IsManual = isManual;
            Remaining = ms;
            ProgressValue = new ProgressValue(100, FormatDuration(Remaining / 1000));
        }

        #region Props

        [JsonIgnore]
        public bool IsActive
        {
            get => _timer != null && _timer.Enabled;
        }

        private bool isManual;
        [JsonIgnore]
        public bool IsManual
        {
            get => isManual;
            set
            {
                SetPropsProperty(ref isManual, value, nameof(IsManual));
            }
        }

        private int _milliseconds;
        [JsonIgnore]
        public int Milliseconds
        {
            get => _milliseconds;
            set => SetPropsProperty(ref _milliseconds, value, nameof(Milliseconds));
        }

        #endregion

        #region States

        private int _remaining;
        [JsonIgnore]
        public int Remaining
        {
            get => _remaining;
            set 
            {
                if (_remaining != value)
                {
                    _remaining = value;
                    States["remaining"] = value;
                    SendUpdate(Remaining * 100 / Milliseconds, FormatDuration(Remaining / 1000));

                }
            }
        }
        #endregion


        private System.Timers.Timer _timer;

        public bool StartCountdown()
        {
            _timer = new System.Timers.Timer(1000);
            _timer.Start();
            _timer.Elapsed += (_, __) => Timer_Elapsed(_, __);

            return true;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Remaining <= 0)
            {
                StopCountdown();
                return;
            }
            Remaining -= 1000;
        }   

        public bool StopCountdown()
        {
            if (_timer != null)
            {
                _logger.Debug($"Stopping countdown with Id {Id}");
                _timer.Stop();
                _timer.Elapsed -= (_, __) => Timer_Elapsed(_, __);
                _timer = null;
                // Sblocco il thread che è fermo nella ShowAndWait
                Context.WaitForUserEvent.Set();
                _logger.Debug($"Unblocked WaitForUserEvent in order to let ESLX proceed");
            }

            return true;
        }

        public bool RestartCountdown(int ms)
        {
            StopCountdown();
            Milliseconds = ms;
            Remaining = Milliseconds;
            StartCountdown();
            return true;
        }

        public bool RestartCountdown()
        {
            StopCountdown();
            Remaining = Milliseconds;
            StartCountdown();
            return true;
        }

        public override void Dispose()
        {
            StopCountdown();
            base.Dispose();
            _logger.Debug($"{nameof(UIFeedbackCountdown)} has been properly disposed");
        }

        private string FormatDuration(double seconds)
        {
            int totalSeconds = (int)Math.Round(seconds);
            int minutes = totalSeconds / 60;
            int secs = totalSeconds % 60;
            var parts = new List<string>();
            if (minutes > 0)
            {
                if (minutes == 1)
                {
                    parts.Add(minutes.ToString());
                    parts.Add("BCA_MINUTE");
                }
                else
                {
                    parts.Add(minutes.ToString());
                    parts.Add("BCA_MINUTES");
                }
            }
            if (secs > 0 || minutes == 0)
            {
                parts.Add(secs.ToString());
                parts.Add("BCA_SECONDS");
            }
            return string.Join("#<br/>#", parts);
        }
    }
}
