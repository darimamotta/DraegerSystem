using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DragerXML.XMLUnits
{
    [XmlRoot(ElementName = "Service")]
    public class Service
    {

        [XmlElement(ElementName = "ServiceDate")]
        public DateTime ServiceDate { get; set; }

        [XmlElement(ElementName = "SessionID")]
        public int SessionID { get; set; }

        [XmlElement(ElementName = "ServiceType")]
        public string ServiceType { get; set; } = string.Empty;

        [XmlElement(ElementName = "ServiceItem")]
        public double ServiceItem { get; set; }

        [XmlElement(ElementName = "EnteredDateTime")]
        public DateTime EnteredDateTime { get; set; }

        [XmlElement(ElementName = "ProviderID")]
        public int ProviderID { get; set; }

        [XmlElement(ElementName = "ItemNumber")]
        public int ItemNumber { get; set; }

        [XmlElement(ElementName = "RefIItemNumber")]
        public int RefIItemNumber { get; set; }

        [XmlElement(ElementName = "Quantity")]
        public double Quantity { get; set; }

        [XmlElement(ElementName = "PersonV40")]
        public List<PersonV40> PersonV40 { get; set; } = new List<PersonV40>();

        [XmlElement(ElementName = "ParameterV40")]
        public List<ParameterV40> ParameterV40 { get; set; } =  new List<ParameterV40>();

    }
}
