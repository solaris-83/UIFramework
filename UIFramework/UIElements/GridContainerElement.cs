using Newtonsoft.Json;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class GridContainerElement : ContainerElement
    {
        public GridContainerElement(IUIContext context) : base(context)
        {

        }
        private Grid _grid;
        [JsonIgnore]
        public Grid Grid
        {
            get  => _grid;
            set => SetPropsProperty(ref _grid, value, nameof(Grid));
        }
    }
}
