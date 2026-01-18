using Newtonsoft.Json;

namespace UIFrameworkDotNet
{
    public class GridContainerElement : ContainerElement
    {
        private Grid _grid;
        [JsonIgnore]
        public Grid Grid
        {
            get  => _grid;
            set => SetPropsProperty(ref _grid, value, nameof(Grid));
        }
    }
}
