using UIFramework.Helpers;
using UIFramework.PredefinedPages;
using static UIFramework.UIButton;
using static UIFramework.UITabControl;

namespace UIFramework
{
    public class CommandFactory
    {
        private readonly Page _page;

        public CommandFactory(Page page)
        {
            _page = page;
        }

        public ICommand Create(UiEvent evt)
        {
            var element = _page.FindById(evt.ElementId);

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8604 // Possible null reference argument.
            return element switch
            {
                UITabControl tabControl when evt?.EventType == "selectedActiveTabChanged"
                    => new TabControlChangedCommand(
                        tabControl,
                        //evt.Payload["selectedActiveTabId"]?.ToString()
                        evt.Payload),

                UIButton btn when evt?.EventType == "enabledChanged"
                    => new ButtonChangedCommand(
                        btn,
                        evt.Payload
                        //Convert.ToBoolean(evt.Payload["enabled"])
                        ),

                UIButton btn when evt?.EventType == "visibilityChanged"
                    => new ButtonChangedCommand(
                        btn,
                        evt.Payload
                        //Convert.ToBoolean(evt.Payload["visible"])
                        ),

                UICheckbox cb when evt?.EventType == "checked"
                    => new CheckboxChangedCommand(
                        cb,
                        // Convert.ToBoolean(evt.Payload["checked"])
                        evt.Payload),

                UITextbox tb when evt?.EventType == "valueChanged"
                    => new TextChangedCommand(
                        tb,
                        // evt.Payload["value"]?.ToString()
                        evt.Payload),

                UIDropDown dd when evt?.EventType == "selectedValueChanged"
                     => new DropDownChangedCommand(
                         dd,
                         // evt.Payload["selected"]?.ToString()
                         evt.Payload),

                _ => throw new InvalidOperationException($"Unsupported event type '{evt?.EventType}' for element '{element?.GetType().Name}'.")
            };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
