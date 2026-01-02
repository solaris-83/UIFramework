using System;
using System.Collections.Generic;
using System.Linq;

//namespace ProvaFiltroInteri
//{
//    // ========== INTERFACCE ==========

//    public interface IRenderable
//    {
//        string Render();
//    }

//    public interface IContainer
//    {
//        void Add(Component component);
//        void Remove(Component component);
//        IEnumerable<Component> GetComponents();
//    }

//    public interface IClickable
//    {
//        event EventHandler Click;
//        void OnClick();
//    }

//    // ========== ENUMERAZIONI ==========

//    public enum LayoutType
//    {
//        Vertical,
//        Horizontal,
//        Grid,
//        Absolute
//    }

//    public enum ComponentType
//    {
//        Label,
//        Button,
//        TextBox,
//        Tab,
//        Panel,
//        Image,
//        CheckBox
//    }

//    // ========== CLASSI BASE ==========

//    public abstract class Component : IRenderable
//    {
//        public string Id { get; set; }
//        public Position Position { get; set; }
//        public Size Size { get; set; }
//        public bool Visible { get; set; }
//        public Style Style { get; set; }
//        public ComponentType Type { get; protected set; }

//        protected Component(string id)
//        {
//            Id = id ?? Guid.NewGuid().ToString();
//            Position = new Position(0, 0);
//            Size = new Size(100, 30);
//            Visible = true;
//            Style = new Style();
//        }

//        public abstract string Render();
//    }

//    // ========== STRUTTURE DATI ==========

//    public class Position
//    {
//        public int X { get; set; }
//        public int Y { get; set; }

//        public Position(int x, int y)
//        {
//            X = x;
//            Y = y;
//        }
//    }

//    public class Size
//    {
//        public int Width { get; set; }
//        public int Height { get; set; }

//        public Size(int width, int height)
//        {
//            Width = width;
//            Height = height;
//        }
//    }

//    public class Style
//    {
//        public string BackgroundColor { get; set; }
//        public string ForegroundColor { get; set; }
//        public string FontFamily { get; set; }
//        public int FontSize { get; set; }
//        public string BorderColor { get; set; }
//        public int BorderWidth { get; set; }

//        public Style()
//        {
//            BackgroundColor = "#FFFFFF";
//            ForegroundColor = "#000000";
//            FontFamily = "Arial";
//            FontSize = 12;
//            BorderColor = "#CCCCCC";
//            BorderWidth = 1;
//        }
//    }

//    // ========== COMPONENTI CONCRETI ==========

//    public class Label : Component
//    {
//        public string Text { get; set; }

//        public Label(string id, string text = "") : base(id)
//        {
//            Type = ComponentType.Label;
//            Text = text;
//        }

//        public override string Render()
//        {
//            return $"<Label Id='{Id}' Position='({Position.X},{Position.Y})' Size='({Size.Width}x{Size.Height})'>{Text}</Label>";
//        }
//    }

//    public class Button : Component, IClickable
//    {
//        public string Text { get; set; }
//        public event EventHandler Click;

//        public Button(string id, string text = "Button") : base(id)
//        {
//            Type = ComponentType.Button;
//            Text = text;
//        }

//        public void OnClick()
//        {
//            Click?.Invoke(this, EventArgs.Empty);
//        }

//        public override string Render()
//        {
//            return $"<Button Id='{Id}' Position='({Position.X},{Position.Y})' Size='({Size.Width}x{Size.Height})'>{Text}</Button>";
//        }
//    }

//    public class TextBox : Component
//    {
//        public string Value { get; set; }
//        public string Placeholder { get; set; }

//        public TextBox(string id, string placeholder = "") : base(id)
//        {
//            Type = ComponentType.TextBox;
//            Placeholder = placeholder;
//            Value = "";
//        }

//        public override string Render()
//        {
//            return $"<TextBox Id='{Id}' Position='({Position.X},{Position.Y})' Size='({Size.Width}x{Size.Height})' Placeholder='{Placeholder}' Value='{Value}'/>";
//        }
//    }

//    public class CheckBox : Component
//    {
//        public string Text { get; set; }
//        public bool IsChecked { get; set; }
//        public string Group { get; set; }
//        public event EventHandler CheckedChanged;

//        public CheckBox(string id, string text = "", string group = null) : base(id)
//        {
//            Type = ComponentType.CheckBox;
//            Text = text;
//            IsChecked = false;
//            Group = group;
//        }

//        public void Toggle()
//        {
//            IsChecked = !IsChecked;
//            OnCheckedChanged();
//        }

//        public void SetChecked(bool value)
//        {
//            if (IsChecked != value)
//            {
//                IsChecked = value;
//                OnCheckedChanged();
//            }
//        }

//        protected virtual void OnCheckedChanged()
//        {
//            CheckedChanged?.Invoke(this, EventArgs.Empty);
//        }

//        public override string Render()
//        {
//            var checkedAttr = IsChecked ? "Checked='true'" : "Checked='false'";
//            var groupAttr = !string.IsNullOrEmpty(Group) ? $" Group='{Group}'" : "";
//            return $"<CheckBox Id='{Id}' Position='({Position.X},{Position.Y})' {checkedAttr}{groupAttr}>{Text}</CheckBox>";
//        }
//    }

//    public class CheckBoxGroup
//    {
//        public string Name { get; set; }
//        private List<CheckBox> _checkBoxes;

//        public CheckBoxGroup(string name)
//        {
//            Name = name;
//            _checkBoxes = new List<CheckBox>();
//        }

//        public void Add(CheckBox checkBox)
//        {
//            checkBox.Group = Name;
//            _checkBoxes.Add(checkBox);
//        }

//        public IEnumerable<CheckBox> GetChecked()
//        {
//            return _checkBoxes.Where(cb => cb.IsChecked);
//        }

//        public IEnumerable<CheckBox> GetUnchecked()
//        {
//            return _checkBoxes.Where(cb => !cb.IsChecked);
//        }

//        public IEnumerable<CheckBox> GetAll()
//        {
//            return _checkBoxes;
//        }

//        public void CheckAll()
//        {
//            foreach (var cb in _checkBoxes)
//            {
//                cb.SetChecked(true);
//            }
//        }

//        public void UncheckAll()
//        {
//            foreach (var cb in _checkBoxes)
//            {
//                cb.SetChecked(false);
//            }
//        }

//        public int CountChecked()
//        {
//            return _checkBoxes.Count(cb => cb.IsChecked);
//        }

//        public List<string> GetCheckedTexts()
//        {
//            return _checkBoxes.Where(cb => cb.IsChecked).Select(cb => cb.Text).ToList();
//        }

//        public List<string> GetCheckedIds()
//        {
//            return _checkBoxes.Where(cb => cb.IsChecked).Select(cb => cb.Id).ToList();
//        }
//    }

//    public class TabControl : Component, IContainer
//    {
//        private List<TabPage> _tabs;

//        public TabControl(string id) : base(id)
//        {
//            Type = ComponentType.Tab;
//            _tabs = new List<TabPage>();
//        }

//        public void AddTab(TabPage tab)
//        {
//            _tabs.Add(tab);
//        }

//        public void Add(Component component)
//        {
//            if (component is TabPage tab)
//                _tabs.Add(tab);
//        }

//        public void Remove(Component component)
//        {
//            if (component is TabPage tab)
//                _tabs.Remove(tab);
//        }

//        public IEnumerable<Component> GetComponents()
//        {
//            return _tabs;
//        }

//        public override string Render()
//        {
//            var tabsContent = string.Join("\n  ", _tabs.Select(t => t.Render()));
//            return $"<TabControl Id='{Id}'>\n  {tabsContent}\n</TabControl>";
//        }
//    }

//    public class TabPage : Component, IContainer
//    {
//        public string Title { get; set; }
//        private List<Component> _components;

//        public TabPage(string id, string title) : base(id)
//        {
//            Title = title;
//            _components = new List<Component>();
//        }

//        public void Add(Component component)
//        {
//            _components.Add(component);
//        }

//        public void Remove(Component component)
//        {
//            _components.Remove(component);
//        }

//        public IEnumerable<Component> GetComponents()
//        {
//            return _components;
//        }

//        public override string Render()
//        {
//            var content = string.Join("\n    ", _components.Select(c => c.Render()));
//            return $"<TabPage Title='{Title}'>\n    {content}\n  </TabPage>";
//        }
//    }

//    // ========== SECTION ==========

//    public class Section : IContainer, IRenderable
//    {
//        public string Id { get; set; }
//        public string Title { get; set; }
//        public LayoutType Layout { get; set; }
//        private List<Component> _components;

//        public Section(string id, string title = "")
//        {
//            Id = id ?? Guid.NewGuid().ToString();
//            Title = title;
//            Layout = LayoutType.Vertical;
//            _components = new List<Component>();
//        }

//        public void Add(Component component)
//        {
//            _components.Add(component);
//        }

//        public void Remove(Component component)
//        {
//            _components.Remove(component);
//        }

//        public IEnumerable<Component> GetComponents()
//        {
//            return _components;
//        }

//        public string Render()
//        {
//            var content = string.Join("\n    ", _components.Select(c => c.Render()));
//            return $"<Section Id='{Id}' Title='{Title}' Layout='{Layout}'>\n    {content}\n  </Section>";
//        }
//    }

//    // ========== PAGE ==========

//    public class Page : IContainer, IRenderable
//    {
//        public string Id { get; set; }
//        public string Title { get; set; }
//        public Size Size { get; set; }
//        private List<Section> _sections;

//        public Page(string id, string title = "Untitled Page")
//        {
//            Id = id ?? Guid.NewGuid().ToString();
//            Title = title;
//            Size = new Size(800, 600);
//            _sections = new List<Section>();
//        }

//        public void AddSection(Section section)
//        {
//            _sections.Add(section);
//        }

//        public void RemoveSection(Section section)
//        {
//            _sections.Remove(section);
//        }

//        public void Add(Component component)
//        {
//            // Crea una sezione di default se si aggiunge un componente direttamente
//            var defaultSection = new Section("default", "Default Section");
//            defaultSection.Add(component);
//            _sections.Add(defaultSection);
//        }

//        public void Remove(Component component)
//        {
//            foreach (var section in _sections)
//            {
//                section.Remove(component);
//            }
//        }

//        public IEnumerable<Component> GetComponents()
//        {
//            return _sections.SelectMany(s => s.GetComponents());
//        }

//        public string Render()
//        {
//            var content = string.Join("\n  ", _sections.Select(s => s.Render()));
//            return $"<Page Id='{Id}' Title='{Title}' Size='({Size.Width}x{Size.Height})'>\n  {content}\n</Page>";
//        }
//    }

//    // ========== DOCUMENT ==========

//    public class Document : IRenderable
//    {
//        public string Title { get; set; }
//        private List<Page> _pages;
//        private int _currentPageIndex;

//        public Document(string title = "Untitled Document")
//        {
//            Title = title;
//            _pages = new List<Page>();
//            _currentPageIndex = 0;
//        }

//        public void AddPage(Page page)
//        {
//            _pages.Add(page);
//        }

//        public void RemovePage(Page page)
//        {
//            _pages.Remove(page);
//        }

//        public Page GetCurrentPage()
//        {
//            return _currentPageIndex >= 0 && _currentPageIndex < _pages.Count
//                ? _pages[_currentPageIndex]
//                : null;
//        }

//        public bool NextPage()
//        {
//            if (_currentPageIndex < _pages.Count - 1)
//            {
//                _currentPageIndex++;
//                return true;
//            }
//            return false;
//        }

//        public bool PreviousPage()
//        {
//            if (_currentPageIndex > 0)
//            {
//                _currentPageIndex--;
//                return true;
//            }
//            return false;
//        }

//        public void GoToPage(int index)
//        {
//            if (index >= 0 && index < _pages.Count)
//                _currentPageIndex = index;
//        }

//        public int GetPageCount()
//        {
//            return _pages.Count;
//        }

//        public string Render()
//        {
//            var content = string.Join("\n", _pages.Select(p => p.Render()));
//            return $"<Document Title='{Title}' Pages='{_pages.Count}'>\n{content}\n</Document>";
//        }
//    }

//    // ========== BUILDER PATTERN ==========

//    public class PageBuilder
//    {
//        private Page _page;
//        private Section _currentSection;

//        public PageBuilder(string title = "New Page")
//        {
//            _page = new Page(null, title);
//        }

//        public PageBuilder WithSize(int width, int height)
//        {
//            _page.Size = new Size(width, height);
//            return this;
//        }

//        public PageBuilder AddSection(string title, LayoutType layout = LayoutType.Vertical)
//        {
//            _currentSection = new Section(null, title) { Layout = layout };
//            _page.AddSection(_currentSection);
//            return this;
//        }

//        public PageBuilder AddLabel(string text)
//        {
//            EnsureSection();
//            _currentSection.Add(new Label(null, text));
//            return this;
//        }

//        public PageBuilder AddButton(string text, EventHandler onClick = null)
//        {
//            EnsureSection();
//            var button = new Button(null, text);
//            if (onClick != null)
//                button.Click += onClick;
//            _currentSection.Add(button);
//            return this;
//        }

//        public PageBuilder AddTextBox(string placeholder = "")
//        {
//            EnsureSection();
//            _currentSection.Add(new TextBox(null, placeholder));
//            return this;
//        }

//        public PageBuilder AddCheckBox(string text, bool isChecked = false, EventHandler onCheckedChanged = null)
//        {
//            EnsureSection();
//            var checkBox = new CheckBox(null, text) { IsChecked = isChecked };
//            if (onCheckedChanged != null)
//                checkBox.CheckedChanged += onCheckedChanged;
//            _currentSection.Add(checkBox);
//            return this;
//        }

//        public PageBuilder AddCheckBoxGroup(string groupName, params string[] items)
//        {
//            EnsureSection();
//            foreach (var item in items)
//            {
//                var checkBox = new CheckBox(null, item, groupName);
//                _currentSection.Add(checkBox);
//            }
//            return this;
//        }

//        public PageBuilder AddTab(Action<TabBuilder> configure)
//        {
//            EnsureSection();
//            var tabBuilder = new TabBuilder();
//            configure(tabBuilder);
//            _currentSection.Add(tabBuilder.Build());
//            return this;
//        }

//        private void EnsureSection()
//        {
//            if (_currentSection == null)
//            {
//                AddSection("Default Section");
//            }
//        }

//        public Page Build()
//        {
//            return _page;
//        }
//    }

//    public class TabBuilder
//    {
//        private TabControl _tabControl;

//        public TabBuilder()
//        {
//            _tabControl = new TabControl(null);
//        }

//        public TabBuilder AddTab(string title, Action<TabPageBuilder> configure)
//        {
//            var tabPageBuilder = new TabPageBuilder(title);
//            configure(tabPageBuilder);
//            _tabControl.AddTab(tabPageBuilder.Build());
//            return this;
//        }

//        public TabControl Build()
//        {
//            return _tabControl;
//        }
//    }

//    public class TabPageBuilder
//    {
//        private TabPage _tabPage;

//        public TabPageBuilder(string title)
//        {
//            _tabPage = new TabPage(null, title);
//        }

//        public TabPageBuilder AddLabel(string text)
//        {
//            _tabPage.Add(new Label(null, text));
//            return this;
//        }

//        public TabPageBuilder AddButton(string text)
//        {
//            _tabPage.Add(new Button(null, text));
//            return this;
//        }

//        public TabPage Build()
//        {
//            return _tabPage;
//        }
//    }
//}