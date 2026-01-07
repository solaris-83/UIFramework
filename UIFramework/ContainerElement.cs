using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework
{
    // Un ContainerElement è un UIElement che può contenere altri UIElement come figli che si chiamano Children.
    public class ContainerElement : UIElement
    {
        public ContainerElement()
        {
        }
        public List<UIElement> Children { get; } = new List<UIElement>();

        public void Add(UIElement element) => Children.Add(element);

        public bool Remove(string id)
        {
            var element = Children.FirstOrDefault(e => e.Id == id);
            if (element != null)
            {
                Children.Remove(element);
                return true;
            }

            foreach (var child in Children.OfType<ContainerElement>())
            {
                if (child.Remove(id))
                    return true;
            }

            return false;
        }

        public bool Update(string id, UIElement element)
        {
            var foundElement = GetUIElement(id);
            if (foundElement == null)
                return false;
            foundElement.UpdateStates(element.States);
            return true;
        }

        public UIElement GetUIElement(string id)
        {
            var element = Children.FirstOrDefault(e => e.Id == id);
            element ??= Children.OfType<ContainerElement>().FirstOrDefault(e => e.Id == id);
            return element;
        }
    }
}
