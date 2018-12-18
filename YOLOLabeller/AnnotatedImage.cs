using System;
using System.Collections.Generic;
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
            this.fName = fName;
        }

        string fName;
        readonly List<SerialisableRect>  boundingBoxes;

        [XmlAttribute("file")]
        public string FileName { get => fName;  }
        [XmlElement("box")]
        public List<SerialisableRect> BoundingBoxes { get => boundingBoxes;  }
    }
}
