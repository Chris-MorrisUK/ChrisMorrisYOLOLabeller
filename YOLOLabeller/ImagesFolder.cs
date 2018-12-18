using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace YOLOLabeller
{
    [Serializable]
    [XmlType("dataset")]
    public class ImagesFolder
    {
        public ImagesFolder()
        {
            //this only gets save to xml, not restored from it, so this should never be used
        }
        public ImagesFolder(string folderName)
        {
            //If you need more file types, add them here.
            files = Directory.GetFiles(folderName,Properties.Settings.Default.fileType1,SearchOption.AllDirectories);

            if (files.Count() > 0)
            {
                List<AnnotatedImage> annotatedAsList = new List<AnnotatedImage>();
                FolderContainsFiles = true;
                foreach (string fName in files)                
                    annotatedAsList.Add(new AnnotatedImage(fName));
                annotatedImages = annotatedAsList.ToArray();
                Folder = folderName;
            }
            else
                FolderContainsFiles = false;
        }
              
                
        private int imgIdx = 0;
        private readonly string[] files;
        [XmlIgnore]
        public readonly bool FolderContainsFiles;
        
        private readonly AnnotatedImage[] annotatedImages;
        [XmlIgnore]
        public readonly string Folder;
        [XmlArray("images")]
        [XmlArrayItem("image")]
        public AnnotatedImage[] AnnotatedImages
        {
            get
            {
             return   annotatedImages;
            }
            set
            {
                throw new NotSupportedException("This setter is just for XML serialization, dont actually use it");
            }
        }

        public void GoNext()
        {
            if (files == null) return;
            if (imgIdx == (files.Count() - 1))
                imgIdx = 0;
            else
                imgIdx++;
        }

        public void GoPrev()
        {
            if (files == null) return;
            if (imgIdx == 0)
                imgIdx = (files.Count() - 1);
            else
                imgIdx--;
        }
        public void AddSelectionToCurrent(double top, double left, double height, double width)
        {
            AnnotatedImages[imgIdx].BoundingBoxes.Add(new SerialisableRect(top,left,width,height));
        }
        public string GetCurrentFile()
        {
            if (!FolderContainsFiles)
                return string.Empty;
            else
                return files[imgIdx];
        }

        public void SaveAnnotatedImages(string fName)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ImagesFolder));

            using (FileStream fs = new FileStream(fName, FileMode.Create))
            {
                ser.Serialize(fs, this);
            }
        }
    }
}
