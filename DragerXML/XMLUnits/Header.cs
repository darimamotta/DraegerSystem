using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DragerXML.XMLUnits
{
    [XmlRoot(ElementName = "Header")]
    public class Header
    {
        [XmlElement(ElementName = "ReceivingApplication")]
        public string ReceivingApplication { get; set; } = string.Empty;
        
        [XmlElement(ElementName = "ReceivingFacility")]
        public int ReceivingFacility { get; set; }
        
        [XmlElement(ElementName = "ReceivingServiceCode")]
        public int ReceivingServiceCode { get; set; }
        
        [XmlElement(ElementName = "SendingApplication")]
        public string SendingApplication { get; set; } = string.Empty;
        
        [XmlElement(ElementName = "SendingFacility")]
        public string SendingFacility { get; set; } = string.Empty;
        
        [XmlElement(ElementName = "SendingServiceCode")]
        public string SendingServiceCode { get; set; } = string.Empty;

        [XmlElement(ElementName = "MessageControlID")]
        public int MessageControlID { get; set; }
        
        [XmlElement(ElementName = "SoftwareReleaseNumber")]
        public double SoftwareReleaseNumber { get; set; }
        
        [XmlElement(ElementName = "FileCreationDate")]
        public DateTime FileCreationDate { get; set; }
        
        [XmlElement(ElementName = "Visit")]
        public List<Visit> Visit { get; set; } = new List<Visit>();

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "noNamespaceSchemaLocation")]
        public string NoNamespaceSchemaLocation { get; set; } = string.Empty;

        [XmlText]
        public string Text { get; set; } = string.Empty;
    }
}
