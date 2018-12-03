using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace YOLOLabeller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            hidenavbtns();
            theVM = (MainWindowVM)this.DataContext;
        }
        ImagesToAnnotate images = null;
        readonly MainWindowVM theVM;

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CommonOpenFileDialog fldDlg = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                EnsurePathExists = true,
                EnsureFileExists = false,
                AllowNonFileSystemItems = false,
                DefaultFileName = "Select Folder",
                Title = "Select Folder Containing Images to Label"
            };
            
            if (fldDlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                images = new ImagesToAnnotate(fldDlg.FileName);
                if (images.ImgFld.FolderContainsFiles)
                {
                    showImageControls();
                    LoadCurrentImage();
                }
                else
                {
                    hidenavbtns();
                }
            }
            

        }
        private void hidenavbtns()
        {
           spImageControls.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void showImageControls()
        {
            spImageControls.Visibility = System.Windows.Visibility.Visible;
        }



      
        private void LoadCurrentImage()
        {
            theVM.LoadBitmapFromFile(images.ImgFld.GetCurrentFile());
            scrlZoom.Value = 1;
        }

       

        private void BtnPrev_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            images.ImgFld.GoPrev();
            LoadCurrentImage();
            
        }

        private void BtnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            images.ImgFld.GoNext();
            LoadCurrentImage();
        }
    }
}
