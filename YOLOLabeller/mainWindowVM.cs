using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace YOLOLabeller
{
    public class MainWindowVM: INotifyPropertyChanged
    {
        public MainWindowVM()
        {
            
        }

        ScaleTransform imageResize = new ScaleTransform();
        private TransformedBitmap currentImage;
        const int ZOOM_MULTIPLE = 20;
        public TransformedBitmap CurrentImage { get => currentImage; set => currentImage = value; }
        private BitmapImage fullSize;
        public double Zoom { get => zoom* ZOOM_MULTIPLE;
            set {
                zoom = value / ZOOM_MULTIPLE;
                imageResize.ScaleY = imageResize.ScaleX = zoom;
                if (fullSize != null)
                {
                    CurrentImage = new TransformedBitmap(fullSize, imageResize);
                    OnPropertyChanged("CurrentImage");
                }
                OnPropertyChanged("Zoom");
            } }

        private double zoom;


        private void OnPropertyChanged(string pName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void LoadBitmapFromFile(string fName)
        {                      
            Zoom = ZOOM_MULTIPLE;
            fullSize = new BitmapImage(new Uri(fName));
            CurrentImage = new TransformedBitmap(fullSize, imageResize);
            //CurrentImage = fName;
            OnPropertyChanged("CurrentImage");

        }


    }
}
