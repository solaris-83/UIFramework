using Newtonsoft.Json;

namespace UIFramework
{
    public class UITabControl : ContainerElement
    {
        public UITabControl()
        {
            
        }

        public new void Add(UIElement element)
        {             
            if (element is UITab tab)
            {
                base.Add(tab);
                ActiveTabId = tab.Id;
            }
            else
            {
                throw new InvalidOperationException("Only UITab elements can be added to UITabControl.");
            }
        }

        [JsonIgnore]
        public string ActiveTabId
        {
            get => States.ContainsKey("activeTabId")? States["activeTabId"].ToString() : "";
            set => States["activeTabId"] = value;
        } 
        
        public class TabControlPropertyChangedCommand : ICommand
        { 
            private readonly UITabControl _tabControl;
            private readonly Dictionary<string, object> _states;

            public TabControlPropertyChangedCommand(UITabControl tabControl, Dictionary<string, object> states)
            {
                _tabControl = tabControl;
                _states = states;
            }

            public void Execute()
            {
                _tabControl.UpdateStates(_states);
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
