using Draeger.Pdms.Services.Extensions;
using Draeger.Pdms.Services.Json;
using Draeger.Pdms.Services.Json.Entities;
using Draft_Draeger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    //релизует интерфейс IObjectforJsonprovider
    public class DraegerHospitalProvider : IHospitalProvider
    {
        public DraegerHospitalProvider(
            string certificate,
            string certificateFilePassword,
            string clappId,
            string serverHostName,
            int serverPort,
            string domainId
           
        ) 
        {
            this.certificate = certificate;
            this.certificateFilePassword = certificateFilePassword;
            this.clappId = clappId;
            this.serverHostName = serverHostName;
            this.domainId = domainId;
            this.serverPort = serverPort;
        }
        public CLAPPConfiguration CreateConfig()
        {
            return new CLAPPConfiguration()
            {
                Certificate = certificate,
                CertificateFilePassword = certificateFilePassword.ToSecureString(),
                CLAPPID = clappId,
                DomainID = domainId,
                ServerHostname = serverHostName,
                ServerPort = serverPort
            };
        }
        public Hospital? GetHospital()
        {
            CLAPPConfiguration config = CreateConfig();
            try
            {
                return BuildHospital(config);
            }
            catch( Exception e) 
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        private Hospital BuildHospital(CLAPPConfiguration config)
        {
            Hospital hospital = new Hospital();

            using (CLAPP clapp = new CLAPP(config))
            {
                PatientsList pList = clapp.GetPatientsList(); 
                foreach (var p in pList.PatientList) 
                {
                    hospital.Patients.Add(BuildPatient(clapp, p));
                }
            } 
            return hospital;
        }

        private ArrivalSick BuildPatient(CLAPP clapp, Patient p)
        {
            clapp.SetPatient(p.CaseID);
            var pt = clapp.ParseTemplate(p.CaseID, template, new DateTime(1990,1,1),DateTime.Now);
            Console.WriteLine(pt.TextResult);
            clapp.ReleasePatient();
            return new ArrivalSick();
        }

        private string certificate;
        private string certificateFilePassword;
        private string clappId;
        private string serverHostName;
        private string domainId;
        private int serverPort;
        private string template = "[Orders:Records=First; Range=All; ExternalIDType=SNOMED; ExternalID=363788007; Format=!({Begin})~]; [Orders:Records=First; Range=All; ExternalIDType=SNOMED; ExternalID=441765008; Format=!({Begin})~]; [Orders:Records=First; Range=All; ExternalIDType=SNOMED; ExternalID=419126006; Format=!({Begin})~]";


    }
}
