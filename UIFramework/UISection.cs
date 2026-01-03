

namespace UIFramework
{
    // TODO capire se le sections servono e dove piazzarle come contenitori logici di altri elementi
    public class UISection : ContainerElement
    {
        public UISection()
        {
            
        }
        public UISection(string title)
        {
            Props["title"] = title;
        }
    }
}
