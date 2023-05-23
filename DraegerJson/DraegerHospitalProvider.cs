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
            template = CreateTemplate();
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
                    ArrivalSick? patient = BuildPatient(clapp, p);
                    if (patient !=null) 
                        hospital.Patients.Add(patient);
                }
            } 
            return hospital;
        }

        private ArrivalSick? BuildPatient(CLAPP clapp, Patient p)
        {
            clapp.SetPatient(p.CaseID);
            var pt = clapp.ParseTemplate(
                p.CaseID, 
                template, 
                new DateTime(1990,1,1),
                DateTime.Now
            );
            ArrivalSick? patient = BuildPatientFromTemplate(pt);
            TEMPORARY_writeResultToFile(pt);
            clapp.ReleasePatient();
            return patient;
        }

        private void TEMPORARY_writeResultToFile(ParseTemplate pt)
        {
            if (File.Exists("result.txt"))
                return;
            File.AppendAllText("result.txt", pt.TextResult);
        }

        private ArrivalSick? BuildPatientFromTemplate(ParseTemplate pt)
        {
            return null;
        }

        private string certificate;
        private string certificateFilePassword;
        private string clappId;
        private string serverHostName;
        private string domainId;
        private int serverPort;
        private string template;

        private string CreateTemplate()
        {
            List<string> snomedID = new List<string>()
            {
                "363788007","419126006","441765008","442335003","442272006","442385007","442126001","442371002",
                "442137000","442273001","398164008","441969007","397927004","442431006"
            };
            string t = "";

            foreach (var id in snomedID) 
            {
                t += $"[Orders:Records=First; Range=All; ExternalIDType=SNOMED; ExternalID={id}; Format=!({{Begin}})~];";
            }
            t += "[PreOP: Format=!({OP_ID})]";
            return t;

        }
    }
}
