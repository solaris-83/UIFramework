using System;
using System.Collections.Generic;
using System.Linq;
using UIFramework.Interfaces;

namespace UIFramework.UIElements.Base
{
    // Un ContainerElement è un UIElement che può contenere altri UIElement come figli che si chiamano Children.
    public class ContainerElement : UIElement
    {
        private readonly IUIContext _context;
        public ContainerElement(IUIContext context) 
        {
            _context = context ?? throw new ArgumentNullException("UIContext cannot be NULL");
        }

        private readonly List<UIElement> _children = new List<UIElement>();
        public List<UIElement> Children => _children;

        public event Action<ContainerElement, UIElement> ItemAdded;
        public event Action<ContainerElement, UIElement> ItemRemoved;

        public void Add(UIElement element)
        {
            element.ParentId = Id;
            if (element is IAttachableContext attachableElement)
                attachableElement.AttachContext(_context);
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
    }
}
