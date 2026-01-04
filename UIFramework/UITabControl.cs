using Newtonsoft.Json;

namespace UIFramework
{
    public class UITabControl : ContainerElement
    {
        [JsonIgnore]
        public string SelectedTabId
        {
            get => States.TryGetValue("selectedTabId", out var v) ? v?.ToString() : null;
            set => States["selectedTabId"] = value;
        } 
        
        public class TabControlChangedCommand : ICommand
        { 
            private readonly UITabControl _tabControl;
            private readonly string _selectedTabId;

            public TabControlChangedCommand(UITabControl tabControl, string value)
            {
                _tabControl = tabControl;
                _selectedTabId = value;
            }

            public void Execute()
            {
                _tabControl.SelectedTabId = _selectedTabId;
            }
        }
    }

    public class UITab : ContainerElement
    {
        public UITab(int rows, int cols)
        {
            Props["grid"] = new Grid(rows, cols);
        }

        public UITab(string tag, int rows, int cols)
        {
            Props["tag"] = tag;
            Props["grid"] = new Grid(rows, cols);
        }

        //public UITab(string id, int rows, int cols) : this(id, rows, cols)
        //{
        //   // Props["title"] = title;
        //}

        //[JsonIgnore]
        //public string Title
        //{
        //    get => Props["title"]?.ToString();
        //    set => Props["title"] = value;
        //}

        [JsonIgnore]
        public Grid Grid
        {
            get  
            { 
                Props.TryGetValue("grid", out var grid);
                if (grid is Grid g)
                    return g;
                return null;
            }
            set => Props["grid"] = value;
        }
    }

    public class Grid
    {
        public Grid(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
        }

        public int Rows { get; set; }
        public int Cols { get; set; }
    }
}
