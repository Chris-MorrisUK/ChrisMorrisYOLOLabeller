using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YOLOLabeller
{
    [Serializable]
    [XmlType("image")]
    public class AnnotatedImage
    {
        public AnnotatedImage()
        {
            boundingBoxes = new List<SerialisableRect>();
            
        }
        public AnnotatedImage(string fName)
        {
            boundingBoxes = new List<SerialisableRect>();
            this.fName = Path.GetFileName(fName);
        }

        string fName;
        readonly List<SerialisableRect>  boundingBoxes;

        [XmlAttribute("file")]
        public string FileName {
            get { return  fName; }
            set { throw new NotImplementedException("Just here for XML"); }
        }
        [XmlElement("box")]
        public List<SerialisableRect> BoundingBoxes { get => boundingBoxes;  }
    }
}
