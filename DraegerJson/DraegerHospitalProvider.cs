using Draeger.Pdms.Services.Extensions;
using Draeger.Pdms.Services.Json;
using Draeger.Pdms.Services.Json.Entities;
using Patient = Draeger.Pdms.Services.Json.Entities.Patient;

namespace DraegerJson
{
    public struct SnomedParameter
    {
        public string Id;
        public string Name;
        public string Milestone;
    }
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
            catch (Exception e)
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
                CreatePatientList(hospital, clapp);
            }
            hospital.Timestamp = toTimestamp;
            return hospital;
        }
        private void CreatePatientList(Hospital hospital, CLAPP clapp)
        {
            PatientsList pList = clapp.GetPatientsList();
            foreach (var p in pList.PatientList)
            {
                ArrivalSick patient = BuildPatient(clapp, p);
                if (PatientContainsParameters(patient))
                    hospital.Patients.Add(patient);
            }
        }
        private bool PatientContainsParameters(ArrivalSick arrivalSick)
        {
            foreach (var procedure in arrivalSick.Procedures)
                if (procedure.Params.Count > 0)
                    return true;
            return false;
        }
        private ArrivalSick BuildPatient(CLAPP clapp, Patient p)
        {
            clapp.SetPatient(p.CaseID);
            ArrivalSick patient = new ArrivalSick { AufnahmeNR = p.AdmissionNumber };
            var proc = BuildProcedure(patient, clapp, p);
            if (proc.Exist)
            {
                BuildPatientId(clapp, p, patient);
                BuildPatientFullName(clapp, p, patient);
                BuildPatientOP(clapp, p, patient);
                BuildPatientAdmissionWardDate(clapp, p, patient);
                BuildPatientLocation(clapp, p, patient);
                BuildTimestampsByProcedure(clapp, p, proc, patient);
            }
            clapp.ReleasePatient();
            return patient;
        }
        private void BuildPatientId(CLAPP clapp, Patient p, ArrivalSick patient)
        {
            string template = CreatePatientIdTemplate();
            var pt = clapp.ParseTemplate(
                p.CaseID,
                template,
                fromTimestamp,
                toTimestamp
            );
            patient.Id = pt.TextResult;
        }
        private void BuildPatientOP(CLAPP clapp, Patient p, ArrivalSick patient)
        {
            string template = CreateOPDate();
            var pt3 = clapp.ParseTemplate(
                p.CaseID,
                template,
                fromTimestamp,
                toTimestamp
            );
            string opdate = pt3.TextResult.ToString();
            patient.OPDate = DateTime.Parse(opdate);
        }
        private void BuildPatientAdmissionWardDate(
            CLAPP clapp, Patient p, ArrivalSick patient
        )
        {
            string template = CreateOPDate();
            var pt4 = clapp.ParseTemplate(
                p.CaseID,
                template,
                fromTimestamp,
                toTimestamp
            );
            string opdate = pt4.TextResult.ToString();
            patient.AdmissionToWardDate = DateTime.Parse(opdate);
        }
        private void BuildPatientFullName(CLAPP clapp, Patient p, ArrivalSick patient)
        {
            PatientDemographics pd = clapp.GetPatientDemographics(p.CaseID);
            patient.FullName = pd.PatientName;
        }
        private void BuildPatientLocation(CLAPP clapp, Patient p, ArrivalSick patient)
        {
            string template = CreatePatientLocationTemplate();
            var pt1 = clapp.ParseTemplate(
                p.CaseID,
                template,
                patient.OPDate,
                toTimestamp
            );
            patient.Location = pt1.TextResult;
        }
        private string CreatePatientLocationTemplate()
        {
            return "[PreOP: Format=!({OP_Location})]";
        }
        private void BuildTimestampsByProcedure(
            CLAPP clapp, Patient p, Operation proc, ArrivalSick patient
        )
        {
            foreach (var snomedID in snomedIDs)
            {
                string template = CreateParamsTemplate(snomedID.Id);
                var pt = clapp.ParseTemplate(
                    p.CaseID,
                    template,
                    patient.OPDate,
                    toTimestamp
                );
                BuildParameterFromTemplate(proc, pt, snomedID, patient);
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
        private void BuildParameterFromTemplate(
            Operation proc,
            ParseTemplate pt,
            SnomedParameter param,
            ArrivalSick patient
        )
        {
            var tokens = pt.TextResult.Split(
                ';', StringSplitOptions.RemoveEmptyEntries
            );
            for (int i = 0; i < tokens.Length; i++)
            {
                proc.Params.Add(
                    new Parameter
                    {
                        Id = param.Id,
                        Name = param.Name,
                        Date = DateTime.Parse(tokens[i]),
                        PatientId = patient.Id,
                        Milestone = param.Milestone
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

        private static List<SnomedParameter> snomedIDs = new List<SnomedParameter>()
        {
            new SnomedParameter { Milestone ="milestone1", Id = "363788007", Name = "Time of patient arrival in healthcare facility" },
            new SnomedParameter { Milestone ="milestone2", Id = "419126006", Name = "Anesthesia preparation time" },
            new SnomedParameter { Milestone ="milestone3", Id = "441765008", Name = "Time of induction of anesthesia" },
            new SnomedParameter { Milestone ="milestone4", Id = "442335003", Name = "Time of establishment of adequate anesthesia" },
            new SnomedParameter { Milestone ="milestone5", Id = "442272006", Name = "Time patient ready for transport" },
            new SnomedParameter { Milestone ="milestone6", Id = "442385007", Name = "Time of patient arrival in procedure room" },
            new SnomedParameter { Milestone ="milestone7", Id = "442126001", Name = "Start time for preparation of patient in procedure room" },
            new SnomedParameter { Milestone ="milestone8", Id = "442371002", Name = "Start time of procedure" },
            new SnomedParameter { Milestone ="milestone9", Id = "442137000", Name = "Completion time of procedure" },
            new SnomedParameter { Milestone ="milestone10", Id = "442273001", Name = "Time procedure room ready for next case" },
            new SnomedParameter { Milestone ="milestone11", Id = "398164008", Name = "Anesthesia finish time" },
            new SnomedParameter { Milestone ="milestone12", Id = "441969007", Name = "Time of patient departure from procedure room" },
            new SnomedParameter { Milestone ="milestone13", Id = "397927004", Name = "Time ready for discharge from post anesthesia care unit" },
            new SnomedParameter { Milestone ="milestone14", Id = "442431006", Name = "Time of discharge from post anesthesia care unit" },
            new SnomedParameter { Milestone ="milestone15", Id = "441968004", Name = "Time of patient arrival in healthcare facility"},
            new SnomedParameter { Milestone ="milestone16", Id = "441927008", Name = "Time of patient arrival to location for anesthesia"},
            new SnomedParameter { Milestone ="milestone18", Id = "442126001", Name = "Start time for preparation of patient" },
            new SnomedParameter { Milestone ="milestone19", Id = "442336002", Name = "Completion time for preparation of patient in procedure room" },
            new SnomedParameter { Milestone ="milestone20", Id = "442272006", Name = "Time patient ready for transport" },
            new SnomedParameter { Milestone ="milestone21", Id = "442463000", Name = "Time ready for discharge from post anesthesia care unit" }
        };
        private string CreateParamsTemplate(string snomedID)
        {
            return
                $"[Orders:Admins=All; " +
                $"Range=CTX...CTX; " +
                $"ExternalIDType=SNOMED; " +
                $"ExternalID={snomedID}; " +
                $"Format=!({{AdminDate}};)~];";
        }
        private string CreateProcedureTemplate()
        {
            return "[PreOP: Format=!({OP_ID})]";
        }
        private string CreatePatientIdTemplate()
        {
            return "[CurrentHISPatientID]";
        }
        private string CreatePatientAufnahmeNrTemplate()
        {
            return "[D: Format=!({CurrentHISCaseID})]";
        }
        private string CreateOPDate()
        {
            return "[Pat:Property=AufnahmeDatum]";
        }
    }
}