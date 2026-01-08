using Newtonsoft.Json;

namespace UIFrameworkDotNet
{
    // TODO capire se le sections servono e dove piazzarle come contenitori logici di altri elementi
    public class UISection : ContainerElement
    {
        public UISection()
        {
            
        }
        public UISection(string title)
        {
           Title = title;
        }

        private string _title;
        [JsonIgnore]
        public string Title
        {
            get => _title;
            set 
            { 
                if (_title == value) return;
                _title = value;
                Props["title"] = value;
                OnPropertyChanged(nameof(Title)); 
            }
        }
    }
}
