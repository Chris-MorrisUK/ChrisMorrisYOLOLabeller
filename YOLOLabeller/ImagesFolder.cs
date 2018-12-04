using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YOLOLabeller
{
    public class ImagesFolder
    {
        public ImagesFolder(string folderName)
        {
            //If you need more file types, add them here.
            files = Directory.GetFiles(folderName,Properties.Settings.Default.fileType1,SearchOption.AllDirectories);
            if (files.Count() > 0)
                FolderContainsFiles = true;
            else
                FolderContainsFiles = false;
        }
              
                
        private int imgIdx = 0;
        private readonly string[] files;
        public readonly bool FolderContainsFiles;

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

        public string GetCurrentFile()
        {
            if (!FolderContainsFiles)
                return string.Empty;
            else
                return files[imgIdx];
        }
    }
}
