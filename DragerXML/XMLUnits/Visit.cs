using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DragerXML.XMLUnits
{
    [XmlRoot(ElementName = "Visit")]
    public class Visit
    {

        [XmlElement(ElementName = "VisitNumber")]
        public int VisitNumber { get; set; }

        [XmlElement(ElementName = "PatientName")]
        public string PatientName { get; set; } = string.Empty;

        [XmlElement(ElementName = "PatientGivenName")]
        public string PatientGivenName { get; set; } = string.Empty;

        [XmlElement(ElementName = "PatientBirthDate")]
        public DateTime PatientBirthDate { get; set; }

        [XmlElement(ElementName = "Service")]
        public List<Service> Service { get; set; } = new List<Service>();
    }
}
