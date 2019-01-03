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
using System.Windows;

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
            showingSelections = new List<Rectangle>();
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
            scrlZoom.Value = MainWindowVM.ZOOM_MULTIPLE;
            resetSelectangle();
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


       
        bool dragging = false;
        Point startPoint;
        private void ScrlViewerImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            dragging = true;
            startPoint = e.GetPosition(cnvSelectanglePos);
            Canvas.SetLeft(selectangle, startPoint.X);
            Canvas.SetTop(selectangle, startPoint.Y);
            initSelectRectangle();
            selectangle.Visibility = Visibility.Visible;
        }

        private void ScrlViewerImage_MouseMove(object sender, MouseEventArgs e)
        {
            if((dragging)&&(startPoint != null))
            {
                Point mousePoint = e.GetPosition(cnvSelectanglePos);
             
                if (startPoint.X < mousePoint.X)
                {
                    Canvas.SetLeft(selectangle, startPoint.X);
                    selectangle.Width = mousePoint.X - startPoint.X;
                }
                else
                {
                    Canvas.SetLeft(selectangle, mousePoint.X);
                    selectangle.Width = startPoint.X - mousePoint.X;
                }

                if (startPoint.Y < mousePoint.Y)
                {
                    Canvas.SetTop(selectangle, startPoint.Y);
                    selectangle.Height = mousePoint.Y - startPoint.Y;
                }
                else
                {
                    Canvas.SetTop(selectangle, mousePoint.Y);
                    selectangle.Height = startPoint.Y - mousePoint.Y;
                }
            }
        }

        private void ScrlViewerImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Released)
                return;
            dragging = false;
            setSelectRectangleSemiVis();
        }

        private void initSelectRectangle()
        {
            selectangle.Fill = Brushes.Transparent;
            selectangle.Opacity = 1;
        }
        private void setSelectRectangleSemiVis()
        {
            selectangle.Fill = Brushes.ForestGreen;
            selectangle.Opacity = 0.3;
        }

        private void resetSelectangle()
        {
            if (selectangle == null) return;
            selectangle.Visibility = Visibility.Hidden;
            initSelectRectangle();
        }

        private void ScrlZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            resetSelectangle();
        }
        readonly List<Rectangle> showingSelections;

        private void BtnAddRegion_Click(object sender, RoutedEventArgs e)
        {
            if((selectangle.ActualWidth > 0)&&
                (selectangle.ActualHeight > 0))
            {
                double scroll = 1 / (theVM.Zoom /  MainWindowVM.ZOOM_MULTIPLE);
                double top = Canvas.GetTop(selectangle);
                double left = Canvas.GetLeft(selectangle);
                double width = selectangle.Width * scroll;
                double height = selectangle.Height * scroll; 
                images.ImgFld.AddSelectionToCurrent(top, left, height, width);
                Rectangle prevSelection = new Rectangle();
                prevSelection.Width = width * scroll;
                prevSelection.Height = height * scroll;
                prevSelection.Fill = Brushes.Blue;
                prevSelection.Opacity = 0.3;
                showingSelections.Add(prevSelection);
                prevSelection.RenderTransform = theVM.ImageResize;
                cnvSelectanglePos.Children.Add(prevSelection);
                Canvas.SetLeft(prevSelection, left);
                Canvas.SetTop(prevSelection, top);
            }
        }

        private void BtnSaveAnnotations_Click(object sender, RoutedEventArgs e)
        {
            CommonSaveFileDialog cfd = new CommonSaveFileDialog
            {
                InitialDirectory = images.ImgFld.Folder,
                Title = "Images Labels file",
                DefaultExtension = "xml",
                AddToMostRecentlyUsedList = true,
                OverwritePrompt = true,
                IsExpandedMode = true
            };
            if(cfd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                images.ImgFld.SaveAnnotatedImages(cfd.FileName);
            }
        }
    }
}
