using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework
{
    public class UITabControl : ContainerElement
    {
        public string SelectedTabId
        {
            get => State.TryGetValue("selectedTabId", out var v) ? v?.ToString() : null;
            set => State["selectedTabId"] = value;
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

    public class Tab : ContainerElement
    {
        public Tab(string header)
        {
            Props["header"] = header;
        }
    }
}
