using Draeger.Pdms.Services.Extensions;
using Draeger.Pdms.Services.Json;
using Draeger.Pdms.Services.Json.Entities;
using DragerXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DraegerXML
{
    public struct UsedMedicament
    {
        public string Id;
        public string Name;
        public string Dose;
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

        private ArrivalSick BuildPatient(CLAPP clapp, Draeger.Pdms.Services.Json.Entities.Patient p)
        {
            clapp.SetPatient(p.CaseID);
            ArrivalSick patient = new ArrivalSick { Id = p.CaseID };
            var proc = BuildProcedure(patient, clapp, p);
            foreach (var medicamID in medicamIDs)
            {
                string template = CreateParamsTemplate(medicamID.Name);
                var pt = clapp.ParseTemplate(
                    p.CaseID,
                    template,
                    fromTimestamp,
                    toTimestamp
                );
                BuildParameterFromTemplate(proc, pt, medicamID);
                TEMPORARY_writeResultToFile(pt);

            }
            
            clapp.ReleasePatient();
            return patient;
        }

        private Operation BuildProcedure(ArrivalSick patient, CLAPP clapp, Draeger.Pdms.Services.Json.Entities.Patient p)
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

        private void TEMPORARY_writeResultToFile(ParseTemplate pt)
        {
            if (File.Exists("result.txt"))
                return;
            File.AppendAllText("result.txt", pt.TextResult);
        }

        private void BuildParameterFromTemplate(Operation proc, ParseTemplate pt, UsedMedicament medicament)
        {
            var tokens = pt.TextResult.Split(';', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++) 
            {
                proc.Medicaments.Add(
                    new Medicament
                    {
                        Id = medicament.Id,
                        Name = medicament.Name,
                        Dose = medicament.Dose
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

        private static List <UsedMedicament> medicamIDs = new List< UsedMedicament >()
        {
            new UsedMedicament { Id = "700734,1", Name = "Propofol 1% Propofol 20 ml 1 AMP." },
            new UsedMedicament { Id = "700277,1", Name = "Fentanyl Fentanyl 0.1mg 1 AMP."},
            new UsedMedicament { Id = "700278,1", Name = "Fentanyl Fentanyl 0.5mg 1 AMP."},
            new UsedMedicament { Id = "701130,1", Name = "Ultiva Remifentanil 1 mg 1 VIAL"},
            new UsedMedicament { Id = "", Name = "Esmeron Rocuronium 50mg 1 AMP."},
            new UsedMedicament { Id = "", Name = "Lysthenon Suxamethon 5% 100mg/2ml 1 Amp."},
            new UsedMedicament { Id = "", Name = "Dormicum Midazolam 5 mg/5 ml 1 Amp."},
            new UsedMedicament { Id = "", Name = "Droperidol Droperidol 1 mg/2 ml 1 AMP."},
            new UsedMedicament { Id = "", Name = "Morphium Morphin 10mg 1 AMP."},
            new UsedMedicament { Id = "", Name = "Catapresan Clonidin 0.15mg 1 Amp."},
            new UsedMedicament { Id = "", Name = "Robinul-Neostigmin Glycopyrrolat 1 Amp."},
            new UsedMedicament { Id = "", Name = "Mepivacain 1% 50 ml 1 Amp."},
            new UsedMedicament { Id = "", Name = "Atropin Atropinsulfat 0.5mg/ml1 Amp."},
            new UsedMedicament { Id = "", Name = "Nalbuphin 20 mg 1 Amp." },
            new UsedMedicament { Id = "", Name = "Ketalar 10 mg/ml 20 ml 1 Amp."},
            new UsedMedicament { Id = "", Name = "Ultiva Remifentanil 2 mg 1 VIAL"},
            new UsedMedicament { Id = "", Name = "Anexate 0.5 mg Flumazenil 1 AMP."},
            new UsedMedicament { Id = "", Name = "Rapifen Alfentanil 1mg 1 Amp." },
            new UsedMedicament { Id = "", Name = "Etomidat Etomidat 20mg 1 Amp." },
            new UsedMedicament { Id = "", Name = "Bupivacain 0.5% 20 ml 1 Amp." },
            new UsedMedicament { Id = "", Name = "Ebrantil Urapidil 50mg 1 AMP." },
            new UsedMedicament { Id = "", Name = "Carbostesin 0.5% h'bar Bupivacain 1 Amp." },

        };

        private string CreateParamsTemplate( string medicamName)
        {           
            string t2 =
               $" orders: treatmentname = {medicamName}; " +
               $" Range = All;" +
               $" admins = all;" +
               " format = !({admindate};" +
               "{treatmentname};" +
               "{SubstanceIdent};" +
               "{ct_dose};" +
               "{SubstanceUnit}\\LF)]";


            return t2;
        }
        private string CreateProcedureTemplate ()
        {
            return "[PreOP: Format=!({OP_ID})]";
        }
    }
}
