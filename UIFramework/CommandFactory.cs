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
                UITabControl tabControl when evt?.EventType == "selectedTabChanged"
                    => new TabControlChangedCommand(
                        tabControl,
                        evt.Payload["selectedTabId"]?.ToString()),

                UIButton btn when evt?.EventType == "enabledChanged"
                    => new ButtonChangedCommand(
                        btn,
                        Convert.ToBoolean(evt.Payload["enabled"])),

                UICheckbox cb when evt?.EventType == "checked"
                    => new CheckboxChangedCommand(
                        cb,
                        Convert.ToBoolean(evt.Payload["checked"])),

                UITextbox tb when evt?.EventType == "valueChanged"
                    => new TextChangedCommand(
                        tb,
                        evt.Payload["value"]?.ToString()),

                UIDropDown dd when evt?.EventType == "selectedValueChanged"
                     => new DropDownChangedCommand(
                         dd, 
                         evt.Payload["selected"]?.ToString()),

                _ => null
            };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
