
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace UIFramework
{

    public class UIElement
    {
        public UIElement(string id)
        {
            Id = id;
        }

        public UIElement()
        {
        }
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string Type => GetType().Name;
        public bool Visible { get; set; } = true;
        public Style Style { get; set; } = new Style();

        public Dictionary<string, object> Props { get; } = new();
        public Dictionary<string, object> State { get; } = new();

        public virtual void ApplyState(Dictionary<string, object> newState)
        {
            foreach (var kv in newState)
                State[kv.Key] = kv.Value;
        }
        public virtual void Update(Action<Dictionary<string, object>> updater)
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
