using System;
using System.Collections.Generic;
using System.Linq;

namespace UIFrameworkDotNet
{
    // Un ContainerElement è un UIElement che può contenere altri UIElement come figli che si chiamano Children.
    public class ContainerElement : UIElement
    {
        protected ContainerElement()
        {
        }
        private readonly List<UIElement> _children = new List<UIElement>();
        public List<UIElement> Children => _children;

        public event Action<ContainerElement, UIElement> ItemAdded;
        public event Action<ContainerElement, UIElement> ItemRemoved;

        public void Add(UIElement element)
        {
            element.ParentId = Id;
            _children.Add(element);
            ItemAdded?.Invoke(this, element);
        }

        public bool Remove(string id)
        {
            var element = Children.FirstOrDefault(e => e.Id == id);
            if (element != null)
            {
                Children.Remove(element);
                ItemRemoved?.Invoke(this, element);
                return true;
            }

            foreach (var child in Children.OfType<ContainerElement>())
            {
                if (child.Remove(id))
                    return true;
            }

            return false;
        }

        //public bool Update(string id, UIElement element)
        //{
        //    var foundElement = GetUIElement(id);
        //    if (foundElement == null)
        //        return false;
        //    foundElement.UpdateStates(element.States);
        //    return true;
        //}

        //public UIElement GetUIElement(string id)
        //{
        //    var element = Children.FirstOrDefault(e => e.Id == id);
        //    element ??= Children.OfType<ContainerElement>().FirstOrDefault(e => e.Id == id);
        //    return element;
        //}

        //internal virtual void OnChildPropertyChanged(UIElement source, string propertyName)
        //{
        //    Parent?.OnChildPropertyChanged(source, propertyName);
        //}
    }
}
