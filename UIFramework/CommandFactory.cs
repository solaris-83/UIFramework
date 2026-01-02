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

            return element switch
            {
                UITabControl tabControl when evt?.EventType == "change"
                    => new TabControlChangedCommand(
                        tabControl,
                        evt.Payload["selectedTabId"]?.ToString()),

                UIButton btn when evt?.EventType == "change"
                    => new ButtonChangedCommand(
                        btn,
                        Convert.ToBoolean(evt.Payload["enabled"])),

                UICheckbox cb when evt?.EventType == "change"
                    => new CheckboxChangedCommand(
                        cb,
                        Convert.ToBoolean(evt.Payload["checked"])),

                UITextbox tb when evt?.EventType == "change"
                    => new TextChangedCommand(
                        tb,
                        evt.Payload["value"]?.ToString()),

                UIDropDown dd when evt?.EventType == "change"
                     => new DropDownChangedCommand(dd, evt.Payload["selected"]?.ToString()),

                _ => null
            };
        }
    }
}
