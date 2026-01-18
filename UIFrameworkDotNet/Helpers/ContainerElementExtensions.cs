using System;
using System.Collections.Generic;

namespace UIFrameworkDotNet.Helpers
{
    public enum RecursiveSearchDirection
    {
        Forward, Backward
    }

    public static class StringExtensions
    {
        public static string ToCamelCase(this string s) => char.ToLowerInvariant(s[0]) + s.Substring(1);
        public static string ToPascalCase(this string s) => char.ToUpperInvariant(s[0]) + s.Substring(1);
    }

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
