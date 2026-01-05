
namespace UIFramework
{
    internal class UICheckBoxGroup : ContainerElement
    {
        public IList<UICheckbox> AllCheckboxes => Children.OfType<UICheckbox>().ToList();
       
        public IList<UICheckbox> SelectedCheckboxes
            => AllCheckboxes.Where(c => c.Checked).ToList();

        public UICheckbox SelectedCheckbox
            => AnySelected ? SelectedCheckboxes.Single() : null;

        public IList<int> SelectedIndexes
            => AllCheckboxes.Select((item, index) => new { item, index })
                            .Where(x => x.item.Checked)
                            .Select(x => x.index)
                            .ToList();

        public int SelectedIndex
            => AnySelected ? SelectedIndexes.Single() : -1;

        public bool AnySelected => SelectedCheckboxes.Any();
    }
}
