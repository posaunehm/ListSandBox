using System.Windows.Media.Imaging;
using Livet;

namespace ListboxSandboxApp.ViewModels
{
    public class ListBoxItemSampleViewModel : ViewModel
    {
        private BitmapSource _imageSource;
        private string _description;

        public ListBoxItemSampleViewModel(BitmapSource image)
        {
            ImageSource = image;
        }

        public BitmapSource ImageSource
        {
            get { return _imageSource; }
            private set
            {
                if (Equals(value, _imageSource)) return;
                _imageSource = value;
                RaisePropertyChanged("ImageSource");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                RaisePropertyChanged("Description");
            }
        }
    }
}