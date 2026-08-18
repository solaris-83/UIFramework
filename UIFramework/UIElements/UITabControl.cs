using Newtonsoft.Json;
using System;
using System.Linq;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UITabControl : ContainerElement
    {
        private UITab _currentTab;
        [JsonIgnore]
        public UITab CurrentTab
        {
            get
            {
                //if (_currentTab == null)
                //    _currentTab = new UITab("mytab", 1, 1);
                return _currentTab;
            }
            set => _currentTab = value;
        }

        public UITabControl(IUIContext context) : base(context) { }

        public new void Add(UIElement element)
        {             
            if (element is UITab tab)
            {
                base.Add(tab);
                ActiveTabId = this.Children.FirstOrDefault(t => t.Type == nameof(UITab))?.Id; // Se aggiungo un nuovo tab rimane attivo il primo tab
            }
            else
            {
                throw new InvalidOperationException("Only UITab elements can be added to UITabControl.");
            }
        }

        #region States
        private string _activeTabId;
        [JsonIgnore]
        public string ActiveTabId
        {
            get => _activeTabId;
            set 
            { 
                SetStatesProperty(ref _activeTabId, value, nameof(ActiveTabId));
            } 
        }

        #endregion
    }

    //public class TabControlActiveTabChangedCommand : ICommand
    //{
    //    private readonly UITabControl _tabControl;

    //    public TabControlActiveTabChangedCommand(UITabControl tabControl)
    //    {
    //        _tabControl = tabControl;
    //    }

    //    public void Execute(object newValue)
    //    {
    //        _tabControl.ActiveTabId = newValue.ToString();
    //    }
    //}

    

}
