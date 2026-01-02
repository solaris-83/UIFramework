

namespace UIFramework.PredefinedPages
{
    public class Page : ContainerElement
    {
        //protected string Id { get; init; } = Guid.NewGuid().ToString();
       
        //protected string Type { get; private set; }

        protected event EventHandler OnUIElementUpdated;

        // Per creare il titolo di una page creo una sezione e ci aggiungo una label
        public bool SetTitle(string idStr, string style)
        {
            var section = new UISection();
            var titleStyle = new Style() { Layout = style };
            {
                Style = titleStyle;
            }
            section.Style = titleStyle;
            var titleLabel = new UILabel(idStr)
            {
                Style = titleStyle  // da qui evinco che è un title e quindi usa uno stile particolare
            };
            section.Add(titleLabel);
            Children.Add(section);
            return true;
        }

        //public Page()
        //{
        //    Type = "standard";
        //}

        //public Page(string type)
        //{
        //    Type = type;
        //}

        //public List<UIElement> Elements { get; } = new();

        //public void Add(UIElement element) => Elements.Add(element);

        //public bool Remove(string elementId)
        //{
        //    var el = Elements.FirstOrDefault(e => e.Id == elementId);
        //    if (el != null)
        //    {
        //        Elements.Remove(el);
        //        return true;
        //    }

        //    foreach (var container in Elements.OfType<ContainerElement>())
        //    {
        //        if (container.Remove(elementId))
        //            return true;
        //    }

        //    return false;
        //}
    }
}
