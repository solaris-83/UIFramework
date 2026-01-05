
namespace UIFramework
{
    internal class UICheckBoxGroup : ContainerElement
    {
        public IEnumerable<UICheckbox> AllCheckboxes => Children.OfType<UICheckbox>();
       
        public IEnumerable<UICheckbox> SelectedCheckboxes
            => AllCheckboxes.Where(c => c.Checked);

        public UICheckbox SelectedCheckbox
            => AnySelected ? SelectedCheckboxes.Single() : null;

        public IEnumerable<int> SelectedIndexes
            => AllCheckboxes.Select((item, index) => new { item, index })
                            .Where(x => x.item.Checked)
                            .Select(x => x.index);

        public int SelectedIndex
            => AnySelected ? SelectedIndexes.Single() : -1;

        public bool AnySelected => SelectedCheckboxes.Any();
    }
}
