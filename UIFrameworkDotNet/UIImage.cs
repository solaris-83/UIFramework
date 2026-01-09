using Newtonsoft.Json;
using System;

namespace UIFrameworkDotNet
{
    public class UIImage : UIElement
    {
        public UIImage(string src)
        {
            Source = $"/img/{src}";
        }

        private string _source;
        [JsonIgnore]
        public string Source
        {
            get => _source;
            set
            {
                if (_source == value) return;
                _source = value;
                States["source"] = value;
                OnPropertyChanged(nameof(Source));
            }
        }
    }

    public class ImageSourceChangedCommand : ICommand
    {
        private readonly UIImage _image;

        public ImageSourceChangedCommand(UIImage image)
        {
            _image = image;
        }

        public void Execute(object newValue)
        {
            _image.Source = newValue.ToString();
        }
    }
}
