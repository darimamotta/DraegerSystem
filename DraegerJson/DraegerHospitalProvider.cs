using Draeger.Pdms.Services.Extensions;
using Draeger.Pdms.Services.Json;
using Draeger.Pdms.Services.Json.Entities;
using Draft_Draeger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public struct SnomedParameter
    {
        public string Id;
        public string Name;
    }
    //релизует интерфейс IObjectforJsonprovider
    public class DraegerHospitalProvider : IHospitalProvider
    {
        public DraegerHospitalProvider(
            string certificate,
            string certificateFilePassword,
            string clappId,
            string serverHostName,
            int serverPort,
            string domainId,
            DateTime fromTimestamp,
            DateTime toTimestamp
           
        ) 
        {
            this.certificate = certificate;
            this.certificateFilePassword = certificateFilePassword;
            this.clappId = clappId;
            this.serverHostName = serverHostName;
            this.domainId = domainId;
            this.serverPort = serverPort;
            this.fromTimestamp = fromTimestamp;
            this.toTimestamp = toTimestamp;
            
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
                    ArrivalSick patient = BuildPatient(clapp, p);
                    hospital.Patients.Add(patient);
                }
            }
            
            return hospital;
        }
       
        
          
        
        
        private ArrivalSick BuildPatient(CLAPP clapp, Patient p)
        {
            clapp.SetPatient(p.CaseID);
            ArrivalSick patient = new ArrivalSick { Id = p.CaseID };
            var proc = BuildProcedure(patient, clapp, p);
            if (proc.Exist)
                BuildTimestampsByProcedure(clapp, p, proc);
            clapp.ReleasePatient();
            return patient;
        }

        private void BuildTimestampsByProcedure(CLAPP clapp, Patient p, Operation proc)
        {
            foreach (var snomedID in snomedIDs)
            {
                string template = CreateParamsTemplate(snomedID.Id);
                var pt = clapp.ParseTemplate(
                    p.CaseID,
                    template,
                    fromTimestamp,
                    toTimestamp
                );
                BuildParameterFromTemplate(proc, pt, snomedID);

            }
        }

        private Operation BuildProcedure(ArrivalSick patient, CLAPP clapp, Patient p)
        {
            var pt = clapp.ParseTemplate(
                   p.CaseID,
                   CreateProcedureTemplate(),
                   new DateTime(1990, 1, 1),
                   DateTime.Now
               );
            Operation proc = new Operation();
            proc.Id = pt.TextResult;
            patient.Procedures.Add(proc);   
               
            return proc;
        }


        private void BuildParameterFromTemplate(Operation proc, ParseTemplate pt, SnomedParameter param)
        {
            var tokens = pt.TextResult.Split(';', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++) 
            {
                proc.Params.Add(
                    new Parameter
                    {
                        Id = param.Id,
                        Name = param.Name,
                        Date = DateTime.Parse(tokens[i])
                    }
                ); 
            }
          
        }


        private string certificate;
        private string certificateFilePassword;
        private string clappId;
        private string serverHostName;
        private string domainId;
        private int serverPort;
        private DateTime fromTimestamp;
        private DateTime toTimestamp;

        private static List <SnomedParameter> snomedIDs = new List<SnomedParameter>()
        {
            new SnomedParameter { Id = "363788007", Name = "Time of patient arrival in healthcare facility" },
            new SnomedParameter { Id = "419126006", Name = "Anesthesia preparation time" },
            new SnomedParameter { Id = "441765008", Name = "Time of induction of anesthesia" },
            new SnomedParameter { Id = "442335003", Name = "Time of establishment of adequate anesthesia" },
            new SnomedParameter { Id = "442272006", Name = "Time patient ready for transport" },
            new SnomedParameter { Id = "442385007", Name = "Time of patient arrival in procedure room" },
            new SnomedParameter { Id = "442126001", Name = "Start time for preparation of patient procedure room" },
            new SnomedParameter { Id = "442371002", Name = "Start time of procedure" },
            new SnomedParameter { Id = "442137000", Name = "Completion time of procedure" },
            new SnomedParameter { Id = "442273001", Name = "Time procedure room ready for next case" },
            new SnomedParameter { Id = "398164008", Name = "Anesthesia finish time" },
            new SnomedParameter { Id = "441969007", Name = "Time of patient departure from procedure room" },
            new SnomedParameter { Id = "397927004", Name = "Time ready for discharge from post anesthesia care unit" },
            new SnomedParameter { Id = "442431006", Name = "Time of discharge from post anesthesia care unit" }
        };

       private string CreateParamsTemplate( string snomedID)
       {
           string t = 
               $"[Orders:Admins=All; " +
               $"Range=CTX...CTX; " +
               $"ExternalIDType=SNOMED; " +
               $"ExternalID={snomedID}; " +
               $"Format=!({{AdminDate}})~];";
           
           return t;
       
       }
      //private string CreateParamsTemplate(string snomedID, DateTime from, DateTime to)    
      //{
      //    string t =
      //        $"[Orders:Admins=All; " +
      //        $"Range=NOW-29d@{from.ToString("HH:mm")}...NOW-29d@{to.ToString("HH:mm")}; " +                           
      //        $"ExternalIDType=SNOMED; " +
      //        $"ExternalID={snomedID}; " +
      //        $"Format=!({{Begin}})~];";
      //  
      //    return t;
      //
      //} 
        //$"Range=NOW-21d@{from.ToString("HH:mm")}...NOW-2d@{to.ToString("HH:mm")}; " + 

        private string CreateProcedureTemplate ()
        {
            return "[PreOP: Format=!({OP_ID})]";
        }
    }
}
