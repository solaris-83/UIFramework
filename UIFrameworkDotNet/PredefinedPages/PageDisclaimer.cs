using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

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
            //Set eventi custom per la section del disclaimer: voglio che quando si arriva a fine scroll si abiliti il bottone CONTINUE
            var section = GetSection();
            Debug.Assert(section != null, "Section instance cannot be null by design.");
            section.SetCustomEvents("onScrollToEnd", 
                new List<UIEvent>() {
                    new UIEvent( section.Id, _buttonContinue.Id, UIEventType.OnScrollToEnd, true, new Dictionary<string, object>() { { "Enabled", true } })
                    //{
                    //    SourceId = section.Id,
                    //    TargetId = _buttonContinue.Id,
                    //    NewStates = new Dictionary<string, object>() { { "enabled", true } }
                    //}
                });
        }

        // TODO Direi che non serve più gestire l'evento di lettura del disclaimer. E' il JS che mi manda un messaggio di update della page
        //public override void ReceiveUpdate(object message)
        //{
        //    if (message != null)
        //    {
        //        //if (message.jsData[0].Equals("DISCLAIMER_READ"))
        //{
        //    BasicLibraries.GUI.DisableControl("CONTINUE", false);
        //}
        //    }
        //}
    }
}
