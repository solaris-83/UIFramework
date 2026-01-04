using System;
using System.Collections.Generic;
using UIFramework.PredefinedPages;

namespace UIFramework.Helpers
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

        public static IEnumerable<T> FindAllByType<T>(this Page page) where T : UIElement
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            var results = new List<T>();

            foreach (var el in page.Children)
            {
                CollectRecursive(el, results);
            }

            return results;
        }

        private static void CollectRecursive<T>(UIElement element, List<T> results) where T : UIElement
        {
            if (element is T matched)
                results.Add(matched);

            if (element is ContainerElement container)
            {
                foreach (var child in container.Children)
                {
                    CollectRecursive(child, results);
                }
            }
        }
    }
}
