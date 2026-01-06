

using Newtonsoft.Json;

namespace UIFramework
{
    public class UICheckBoxGroup : ContainerElement
    {
        [JsonIgnore]
        public IList<UICheckbox> Checkboxes => Children.OfType<UICheckbox>().ToList();

        [JsonIgnore]
        public IList<UICheckbox> SelectedCheckboxes
            => Checkboxes.Where(c => c.Checked).ToList();

        [JsonIgnore]
        public UICheckbox SelectedCheckbox
            => AnySelected ? SelectedCheckboxes.Single() : null;

        [JsonIgnore]
        public IList<int> SelectedIndexes
            => Checkboxes.Select((item, index) => new { item, index })
                            .Where(x => x.item.Checked)
                            .Select(x => x.index)
                            .ToList();

        [JsonIgnore]
        public int SelectedIndex
            => AnySelected ? SelectedIndexes.Single() : -1;

        [JsonIgnore]
        public bool AnySelected => SelectedCheckboxes.Any();
    }
}
