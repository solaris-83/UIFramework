using Newtonsoft.Json;
using System;
using UIFrameworkDotNet.Helpers;

namespace UIFrameworkDotNet.PredefinedPages
{
    public sealed class PageMenu : PredefinedPage
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
        public DataArray SelectedIndexes
        {
            get
            {
                var da = new DataArray();
                foreach (var idx in _checkboxgroup.SelectedIndexes)
                    da.Add(idx);
                return da;
            }
        }
        

        [JsonIgnore]
        public DataArray SelectedIds
        {
            get
            {
                var da = new DataArray();
                foreach (var chb in _checkboxgroup.SelectedCheckboxes)
                    da.Add(chb.Tag);
                return da;
            }
        }


        public PageMenu(IUIContext uicontext) : base("menu", uicontext)
        {
            AddButton("EXIT", true, "danger");
            AddButton("CONTINUE", false);
            var section = GetSection();
            _checkboxgroup = new UICheckBoxGroup();
            section.Add(_checkboxgroup);
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
