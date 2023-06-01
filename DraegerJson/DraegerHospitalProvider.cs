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
                tempToTimestamp = tempFromTimestamp.AddMinutes(tempMinutes);
                Console.WriteLine("{0} {1}", tempFromTimestamp, tempToTimestamp);
                PatientsList pList = clapp.GetPatientsList(); 
                foreach (var p in pList.PatientList) 
                {
                    //ArrivalSick patient = BuildPatient(clapp, p);
                    ArrivalSick patient = Temporary_request_05102023(clapp, p);
                    hospital.Patients.Add(patient);
                }
            }
            tempFromTimestamp = tempToTimestamp;
            return hospital;
        }
        static DateTime tempFromTimestamp = new DateTime(2023, 5, 10, 9, 50, 0);
        static DateTime tempToTimestamp = new DateTime();
        static int tempMinutes = 10;
            
        private ArrivalSick Temporary_request_05102023(CLAPP clapp, Patient p)
        {
            
            clapp.SetPatient(p.CaseID);
            ArrivalSick patient = new ArrivalSick { Id = p.CaseID };
            var proc = BuildProcedure(patient, clapp, p);

            foreach (var snomedID in snomedIDs)
            {
                string template = CreateParamsTemplate(snomedID.Id, tempFromTimestamp, tempToTimestamp);
                var pt = clapp.ParseTemplate(
                    p.CaseID,
                    template,
                    tempFromTimestamp,
                    tempToTimestamp
                );
                BuildParameterFromTemplate(proc, pt, snomedID);


            }

            clapp.ReleasePatient();
          
            return patient;

        }
        private ArrivalSick BuildPatient(CLAPP clapp, Patient p)
        {
            clapp.SetPatient(p.CaseID);
            ArrivalSick patient = new ArrivalSick { Id = p.CaseID };
            var proc = BuildProcedure(patient, clapp, p);
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
            
            clapp.ReleasePatient();
            return patient;
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
            new SnomedParameter { Id = "363788007", Name = "Eintritt erfolgt" },
            new SnomedParameter { Id = "419126006", Name = "Beginn Anästhesiebetreuung" },
            new SnomedParameter { Id = "441765008", Name = "Beginn Einleitung" },
            new SnomedParameter { Id = "442335003", Name = "Ende Einleitung, Freigabe" },
            new SnomedParameter { Id = "442272006", Name = "Beginn op. Vorb.(Lagerung) nichtärztl." },
            new SnomedParameter { Id = "442385007", Name = "Saaleinfahrt" },
            new SnomedParameter { Id = "442126001", Name = "Beginn op. Vorb. (Desinfektion) ärztl" },
            new SnomedParameter { Id = "442371002", Name = "Beginn Hautschnitt (Schnitt)" },
            new SnomedParameter { Id = "442137000", Name = "Ende Hautnaht (Naht)" },
            new SnomedParameter { Id = "442273001", Name = "Ende op. Nachbereit." },
            new SnomedParameter { Id = "398164008", Name = "Ende Ausleitung" },
            new SnomedParameter { Id = "441969007", Name = "Saalausfahrt" },
            new SnomedParameter { Id = "397927004", Name = "Ende Anästhesiebetreuung" },
            new SnomedParameter { Id = "442431006", Name = "Ausfahrt Aufwachraum" }
        };

        private string CreateParamsTemplate( string snomedID)
        {
            string t = 
                $"[Orders:Records=First; " +
                $"Range=Context; " +
                $"ExternalIDType=SNOMED; " +
                $"ExternalID={snomedID}; " +
                $"Format=!({{Begin}})~];";
            
            return t;

        }
        private string CreateParamsTemplate(string snomedID, DateTime from, DateTime to)
    
        {
            string t =
                $"[Orders:Records=First; " +
                $"Range=NOW-22d@{from.ToString("HH:mm")}...NOW-22d@{to.ToString("HH:mm")}; " +                           
                $"ExternalIDType=SNOMED; " +
                $"ExternalID={snomedID}; " +
                $"Format=!({{Begin}})~];";
          
            return t;

        } 
        //$"Range=NOW-21d@{from.ToString("HH:mm")}...NOW-2d@{to.ToString("HH:mm")}; " + 

        private string CreateProcedureTemplate ()
        {
            return "[PreOP: Format=!({OP_ID})]";
        }
    }
}
