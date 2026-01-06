using Newtonsoft.Json;

namespace UIFramework
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
            set => States["selected"] = value;
        }

        [JsonIgnore]
        public bool Enabled
        {
            get => States.TryGetValue("enabled", out var v) && (bool)v;
            set => States["enabled"] = value;
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

    public class DropDownPropertyChangedCommand : ICommand
    {
        private readonly UIDropDown _dropDown;
        private readonly Dictionary<string, object> _states;

        public DropDownPropertyChangedCommand(UIDropDown dropDown, Dictionary<string, object> states)
        {
            _dropDown = dropDown;
            _states = states;
        }

        public void Execute()
        {
            _dropDown.UpdateStates(_states);
        }
    }
}
