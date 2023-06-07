using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DragerXML.XMLUnits
{
    [XmlRoot(ElementName = "PersonV40")]
    public class PersonV40
    {

        [XmlElement(ElementName = "PersonID")]
        public int PersonID { get; set; }

        [XmlAttribute(AttributeName = "PersonTyp")]
        public string PersonTyp { get; set; } = string.Empty;

        [XmlText]
        public string Text { get; set; }
    }
}
