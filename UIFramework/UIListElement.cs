namespace UIFramework
{
    public class UIListElement : ContainerElement
    {
        public UIListElement(ListKind kind)
        {
            Props["kind"] = kind.ToString().ToLower();
        }
    }

    public enum ListKind
    {
        Unordered,   // <ul>
        Ordered      // <ol>
    }
}
