using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DragerXML.XMLUnits
{
    [XmlRoot(ElementName = "ParameterV40")]
    public class ParameterV40
    {

        [XmlElement(ElementName = "ParamValue")]
        public int ParamValue { get; set; }

        [XmlAttribute(AttributeName = "ParamTyp")]
        public string ParamTyp { get; set; } = string.Empty;

        [XmlText]
        public int Text { get; set; }
    }

}
