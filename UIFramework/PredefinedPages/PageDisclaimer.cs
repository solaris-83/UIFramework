using Newtonsoft.Json;

namespace UIFramework.PredefinedPages
{
    public sealed class PageDisclaimer : Page
    {
        [JsonIgnore]
        public bool RequiresCompleteRead
        {
            get => Props.TryGetValue("requiresCompleteRead", out var v) && (bool)v;
            set => Props["requiresCompleteRead"] = value;
        }

        public PageDisclaimer() : base()
        {
            RequiresCompleteRead = false;
            SetTitle("Information", "info");
            AddTab("disclaimer", 1, 1);
            AddButton("EXIT_WITHOUT_REPORT", true, "danger");
            AddButton("CONTINUE", false);
        }

        // TODO gestire l'evento di lettura del disclaimer
        public override void ReceiveUpdate(object message)
        {
            if (message != null)
            {
                //if (message.jsData[0].Equals("DISCLAIMER_READ"))
                //{
                //    BasicLibraries.GUI.DisableControl("CONTINUE", false);
                //}
            }
        }
    }
}
