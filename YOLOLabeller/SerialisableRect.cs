using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YOLOLabeller
{
    [XmlType("box")]
    public class SerialisableRect
    {
        public SerialisableRect()
        {            
        }
        public SerialisableRect(double t,double l,double w, double h)
        {
            top = (int)t;
            left = (int)l;
            width = (int)w;
            height = (int)h;
        }

        int top, left, width, height;
        [XmlAttribute("top")]
        public int Top { get => top; set => top = value; }
        [XmlAttribute("left")]
        public int Left { get => left; set => left = value; }
        [XmlAttribute("width")]
        public int Width { get => width; set => width = value; }
        [XmlAttribute("height")]
        public int Height { get => height; set => height = value; }
    }
}
