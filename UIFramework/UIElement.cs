
using Newtonsoft.Json;

namespace UIFramework
{

    public class UIElement
    {
        public UIElement()
        {
            Props["tag"] = null;
            Props["type"] = GetType().Name;
            Props["style"] = new Style();
            States["visible"] = true;
            States["enabled"] = true;
        }

        [JsonIgnore]
        public object Tag
        {
            get => Props["tag"];
            set => Props["tag"] = value;
        }

        [JsonIgnore]
        public bool Enabled
        {
            get => States.TryGetValue("enabled", out var v) && (bool)v;
            set => States["enabled"] = value;
        }

        [JsonIgnore]
        public bool Visible
        {
            get => States.TryGetValue("visible", out var v) && (bool)v;
            set => States["visible"] = value;
        }

        [JsonIgnore]
        public Style Style
        {
            get
            {
                Props.TryGetValue("style", out var style);
                if (style is Style s)
                    return s;
                return null;
            }
            set => Props["style"] = value;
        }

        public string Id { get; init; } = Guid.NewGuid().ToString();

        public void SetStyle(Style style)
        {
            Props["style"] = style;
        }

        public Dictionary<string, object> Props { get; } = new();
        public Dictionary<string, object> States { get; } = new();

        public virtual void UpdateStates(Dictionary<string, object> newStates)
        {
            if (newStates == null)
                return;

            UpdateStates(states =>
            {
                foreach (var kv in newStates)
                    States[kv.Key] = kv.Value;
            });
        }

        public virtual void UpdateStates(Action<Dictionary<string, object>> updater)
        {
            updater(States);
        }

        public virtual void UpdateProps(Dictionary<string, object> newProps)
        {
            if (newProps == null)
                return;

            UpdateProps(props =>
            {
                foreach (var kv in newProps)
                    props[kv.Key] = kv.Value;
            });
        }

        public virtual void UpdateProps(Action<Dictionary<string, object>> updater)
        {
            updater(Props);
        }
    }

    public class Style
    {
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public string FontFamily { get; set; }
        public int FontSize { get; set; }
        public string Layout { get; set; }

        public Style()
        {
            Layout = "";
            BackgroundColor = ""; // "#FFFFFF";
            ForegroundColor = ""; //"#000000";
            FontFamily = ""; //"Arial";
            FontSize = 12;
        }
    }
}
