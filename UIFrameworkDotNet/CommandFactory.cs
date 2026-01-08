/*
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

        public ICommandOld Create(UiEvent evt)
        {
            var element = _page.FindById(evt.ElementId);

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8604 // Possible null reference argument.
            return element switch
            {
                UITabControl tabControl when evt?.EventType == "propertyChanged" // activeTabChanged
                    => new TabControlPropertyChangedCommand(
                        tabControl,
                        //evt.Payload["activeTabId"]?.ToString()
                        evt.States),

                UIButton btn when evt?.EventType == "propertyChanged" // "enabledChanged"
                    => new ButtonPropertyChangedCommand(
                        btn,
                        evt.States
                        //Convert.ToBoolean(evt.Payload["enabled"])
                        ),

                UIButton btn when evt?.EventType == "propertyChanged" // "visibilityChanged"
                    => new ButtonPropertyChangedCommand(
                        btn,
                        evt.States
                        //Convert.ToBoolean(evt.Payload["visible"])
                        ),

                UICheckbox cb when evt?.EventType == "propertyChanged" // "checked"
                    => new CheckboxPropertyChangedCommand(
                        cb,
                        // Convert.ToBoolean(evt.Payload["checked"])
                        evt.States),

                UITextbox tb when evt?.EventType == "propertyChanged" // "valueChanged"
                    => new TextPropertyChangedCommand(
                        tb,
                        // evt.Payload["value"]?.ToString()
                        evt.States),

                UIDropDown dd when evt?.EventType == "propertyChanged" // "selectedValueChanged"
                     => new DropDownPropertyChangedCommand(
                         dd,
                         // evt.Payload["selected"]?.ToString()
                         evt.States),

                _ => throw new InvalidOperationException($"Unsupported event type '{evt?.EventType}' for element '{element?.GetType().Name}'.")
            };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
*/