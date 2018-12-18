using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YOLOLabeller
{
    public class ImagesToAnnotate
    {
        public ImagesToAnnotate(string folderName)
        {
            ImgFld = new ImagesFolder(folderName);
       
        }

       

        public readonly ImagesFolder ImgFld;
    }
}
