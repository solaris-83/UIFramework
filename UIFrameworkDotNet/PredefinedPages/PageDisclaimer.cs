using Newtonsoft.Json;

namespace UIFrameworkDotNet.PredefinedPages
{
    public sealed class PageDisclaimer : PredefinedPage
    {
        private UIButton _buttonContinue;

        private bool _requiresCompleteRead;
        [JsonIgnore]
        public bool RequiresCompleteRead
        {
            get => _requiresCompleteRead;
            set
            {
                SetProperty(ref _requiresCompleteRead, value, () =>
                    {
                        _buttonContinue.Enabled = !_requiresCompleteRead;
                        Props["requiresCompleteRead"] = _requiresCompleteRead;
                    },
                nameof(RequiresCompleteRead));
            }
        }

        public PageDisclaimer(IUIContext uicontext) : base("disclaimer", uicontext)
        {
            SetTitle("title", "Information", "info");
          //  AddTab("disclaimer", 1, 1);
            AddButton("EXIT_WITHOUT_REPORT", true, "danger");
            _buttonContinue = AddButton("CONTINUE", true);
            RequiresCompleteRead = false;
        }

        // TODO Direi che non serve più gestire l'evento di lettura del disclaimer. E' il JS che mi manda un messaggio di update della page
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
