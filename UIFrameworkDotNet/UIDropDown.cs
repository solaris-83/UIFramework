using Newtonsoft.Json;
using ScriptLibraries.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace UIFrameworkDotNet
{
    public class UIDropDown : UIElement
    {
        public UIDropDown(IEnumerable<DropDownOption> options, string selected = null)
        {
            Props["options"] = options.ToList();
            States["selected"] = selected;
            States["enabled"] = true;
        }

        [JsonIgnore]
        public string Selected
        {
            get => States.ContainsKey("selected")? States["selected"]?.ToString() : "";
            set { States["selected"] = value; OnPropertyChanged(nameof(Selected)); }
        }

        [JsonIgnore]
        public bool Enabled
        {
            get => States.TryGetValue("enabled", out var v) && (bool)v;
            set { States["enabled"] = value; OnPropertyChanged(nameof(Enabled)); }
        }
    }

    public class DropDownOption
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public DropDownOption(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }

    //public class DropDownPropertyChangedCommand : ICommand
    //{
    //    private readonly UIDropDown _dropDown;
    //    private readonly ITranslationService _translationService;

    //    public DropDownPropertyChangedCommand(UIDropDown dropDown, ITranslationService translationService)
    //    {
    //        _translationService = translationService;
    //        _dropDown = dropDown;
    //    }

    //    public void Execute(object newValue)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
}
