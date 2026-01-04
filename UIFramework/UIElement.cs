
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
        }

        public object Tag
        {
            get => Props["tag"];
            set => Props["tag"] = value;
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
