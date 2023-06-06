using DragerXML.XMLUnits;
using System.Xml.Serialization;


namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Header header = new Header();
            header.ReceivingApplication = "SAP";
            header.ReceivingFacility = 608;
            header.ReceivingServiceCode = 1100;
            header.SendingApplication = "MEDORA";
            header.SendingFacility = "Radiologie";
            header.SendingServiceCode = "Radiologie";
            header.MessageControlID = 12208255;
            header.SoftwareReleaseNumber = 2.3;
            header.FileCreationDate = new DateTime(2023, 06, 06, 13, 28, 39);
            Visit visit = new Visit();
            visit.VisitNumber = 0009990002;
            visit.PatientName = "Shachter";
            visit.PatientGivenName = "Tim";
            visit.PatientBirthDate = new DateTime(1995, 07, 08);
            Service service = new Service();
            service.ServiceDate = new DateTime(2023, 06, 06, 13, 28, 39);
            service.SessionID = 1;
            service.ServiceType = "TARMED";
            service.ServiceItem = 39.0240;
            service.EnteredDateTime = new DateTime(2023, 06, 06, 13, 28, 39);
            service.ProviderID = 124000;
            service.ItemNumber = 2160484;
            service.Quantity = 1.0;
            PersonV40 p1 = new PersonV40();
            p1.PersonTyp = "ResponsiblePhysician";
            p1.PersonID = 0001107598;
            PersonV40 p2 = new PersonV40();
            p2.PersonTyp = "ProvidingPhysician";
            p2.PersonID = 0001107598;
            PersonV40 p3 = new PersonV40();
            p3.PersonTyp = "FeePhysician";
            p3.PersonID = 0001107598;
            ParameterV40 par40 = new ParameterV40();
            par40.ParamTyp = "Side";
            par40.ParamValue = 001;

        }
    }

    //XmlSerializer serializer = new XmlSerializer(typeof(Header));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (Header)serializer.Deserialize(reader);
    // }

 


}