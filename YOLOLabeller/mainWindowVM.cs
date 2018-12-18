using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace YOLOLabeller
{
    public class MainWindowVM: INotifyPropertyChanged
    {
        public MainWindowVM()
        {
            imageResize = new ScaleTransform();
           
        }

        ScaleTransform imageResize;
        
        private BitmapImage currentImage;
        public const int ZOOM_MULTIPLE = 20;
        public BitmapImage CurrentImage { get => currentImage; set => currentImage = value; }
        private BitmapImage fullSize;
        public double Zoom { get => zoom* ZOOM_MULTIPLE;
            set {
                zoom = value / ZOOM_MULTIPLE;
                imageResize.ScaleY = imageResize.ScaleX = zoom;
                if (fullSize != null)
                {
                    //In theory there's a better way to do this
                  //  CurrentImage = new TransformedBitmap(fullSize, imageResize);
                    Width = CurrentImage.Width;
                    Height = CurrentImage.Height;
                    OnPropertyChanged("CurrentImage");
                    OnPropertyChanged("Width");
                    OnPropertyChanged("Height");
                }

                OnPropertyChanged("Zoom");
            } }

        public ScaleTransform ImageResize { get => imageResize; set => imageResize = value; }
        public double Width { get => width; set => width = value; }
        public double Height { get => height; set => height = value; }

        private double zoom;

        private double height;
        private double width;
        private void OnPropertyChanged(string pName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void LoadBitmapFromFile(string fName)
        {                      
            Zoom = ZOOM_MULTIPLE;
            fullSize = new BitmapImage(new Uri(fName));
           
            CurrentImage =  fullSize;// new TransformedBitmap(fullSize, /*imageResize*/);
           // CurrentImage = fName;
            OnPropertyChanged("CurrentImage");

        }


    }
}
