using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFrameworkDotNet
{
    public class GridPositionContainerElement : GridContainerElement
    {
        private GridPosition _gridPosition;
        [JsonIgnore]
        public GridPosition GridPosition
        {
            get => _gridPosition;
            set => SetPropsProperty(ref _gridPosition, value, nameof(GridPosition));
        }
    }
}
