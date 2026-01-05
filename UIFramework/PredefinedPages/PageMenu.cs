

using Newtonsoft.Json;
using System.Linq;
using System.Reflection.Emit;
using UIFramework.Helpers;

namespace UIFramework.PredefinedPages
{
    public sealed class PageMenu : Page
    {
        private readonly UICheckBoxGroup _checkboxgroup;

        [JsonIgnore]
        public bool HasCheckboxes
        {
            get => Props.TryGetValue("hasCheckboxes", out var v) && (bool)v;
            set => Props["hasCheckboxes"] = value;
        }

        [JsonIgnore]
        public bool IsMultipleSelection
        {
            get => Props.TryGetValue("isMultipleSelection", out var v) && (bool)v;
            set => Props["isMultipleSelection"] = value;
        }

        [JsonIgnore]
        public int SelectedIndex => !IsMultipleSelection ?
             _checkboxgroup.SelectedIndex :
            throw new InvalidOperationException("You cannot retrieve SelectedIndex if IsMutipleSection is set to TRUE. Use SelectedIndexes instead.");


        [JsonIgnore]
        public string SelectedId => !IsMultipleSelection ?
            (_checkboxgroup?.SelectedCheckbox == null ? "" : _checkboxgroup.SelectedCheckbox.Tag.ToString()) :
            throw new InvalidOperationException("You cannot retrieve SelectedIndex if IsMutipleSection is set to TRUE. Use SelectedIds instead.");

        [JsonIgnore]
        public DataArray SelectedIndexes => new DataArray(_checkboxgroup.SelectedIndexes);
        

        [JsonIgnore]
        public DataArray SelectedIds => new DataArray(_checkboxgroup.SelectedCheckboxes.Select(chk => chk.Tag));


        public PageMenu() : base()
        {
            var tab = AddTab("menu", 1, 1);
            AddButton("EXIT", true, "danger");
            AddButton("CONTINUE", false);
            var section = new UISection();
            _checkboxgroup = new UICheckBoxGroup();
            section.Add(_checkboxgroup);
            tab.Add(section);   
        }

        public UICheckbox AddItem(string tag, string idStr)
        {
            var chkbox = new UICheckbox(idStr, isChecked: false, isEnabled: true, tag);
            _checkboxgroup.Add(chkbox);
            return chkbox;
        }

        public bool SetMessage(params string[] ids)
        {
            foreach (string idStr in ids)
            {
                AddParagraph(idStr);
            }

            return true;
        }
    }
}
