using System;
using System.Collections.Generic;
using UIFramework.UIElements.Base;

namespace UIFramework.Helpers
{

    public static class ContainerElementExtensions
    {
        public static UIElement FindById(this ContainerElement containerElement, string id)
        {
            foreach (var el in containerElement.Children)
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

        private static UIElement FindRecursive(UIElement element, Type typeToSearch)
        {
            if (element.Type == typeToSearch.Name)
                return element;

            if (element is ContainerElement container)
            {
                foreach (var child in container.Children)
                {
                    var found = FindRecursive(child, typeToSearch);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }

        public static T FindFirstByType<T>(this ContainerElement containerElement) where T : UIElement
        {
            foreach (var el in containerElement.Children)
            {
                var found = FindRecursive(el, typeof(T));
                if (found != null && found is T firstElement)
                    return firstElement;
            }
            return null;
        }

        public static IEnumerable<T> FindAllByType<T>(this ContainerElement container) where T : UIElement
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            var results = new List<T>();

            foreach (var el in container.Children)
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
