using Newtonsoft.Json;
using System;

namespace UIFrameworkDotNet
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

        private string _activeTabId;
        [JsonIgnore]
        public string ActiveTabId
        {
            get => _activeTabId;
            set 
            { 
                if (_activeTabId == value) return;
                _activeTabId = value;
                States["activeTabId"] = value; 
                OnPropertyChanged(nameof(ActiveTabId)); 
            } 
        } 
    }

    public class TabControlActiveTabChangedCommand : ICommand
    {
        private readonly UITabControl _tabControl;

        public TabControlActiveTabChangedCommand(UITabControl tabControl)
        {
            _tabControl = tabControl;
        }

        public void Execute(object newValue)
        {
            _tabControl.ActiveTabId = newValue.ToString();
        }
    }

    public class UITab : ContainerElement
    {
        public UITab(int rows, int cols)
        {
            Grid = new Grid(rows, cols);
        }

        public UITab(string tag, int rows, int cols)
        {
            Tag = tag;
            Grid = new Grid(rows, cols);
        }

        private Grid _grid;
        [JsonIgnore]
        public Grid Grid
        {
            get => _grid;
            set
            {
                if (_grid != null && _grid.Equals(value)) return; 
                _grid = value;
                Props["grid"] = value;
                OnPropertyChanged(nameof(Grid));
            }
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

        public override bool Equals(object obj)
        {
            return obj is Grid grid &&
                   Rows == grid.Rows &&
                   Cols == grid.Cols;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Rows.GetHashCode();
                hash = hash * 23 + Cols.GetHashCode();
                return hash;
            }
        }
    }
}
