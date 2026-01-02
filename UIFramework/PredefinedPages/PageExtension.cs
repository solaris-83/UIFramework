namespace UIFramework.PredefinedPages
{
    public static class PageExtensions
    {
        public static UIElement FindById(this Page page, string id)
        {
            foreach (var el in page.Children)
            {
                var found = FindRecursive(el, id);
                if (found != null)
                    return found;
            }
            return null;
        }

        private static UIElement FindRecursive(UIElement element, string id)
        {
            if (element.Id == id)
                return element;

            if (element is ContainerElement container)
            {
                foreach (var child in container.Children)
                {
                    var found = FindRecursive(child, id);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }
    }
}
