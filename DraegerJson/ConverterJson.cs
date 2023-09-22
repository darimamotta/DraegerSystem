using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System.Text.Json;
using FhirTypes = Hl7.Fhir.ElementModel.Types;
using System.IO;
using Hl7.Fhir.Utility;
using Hl7.FhirPath.Sprache;

namespace DraegerJson
{
   
    public class PatientJson
    {
        public string PatientId { get; set; } = string.Empty;
        public string AufnahmeNr { get; set;} = string.Empty;
        public string Json { get; set; } = string.Empty;
    }
    //Serialze Objects
    public class ConverterJson : ConverterJsonBase
    {
        
        private ParameterHistory history;
      

        public override List<PatientJson> Convert(Hospital hospital)
        {
            List<PatientJson> result = new List<PatientJson>();            

            foreach (ArrivalSick pat in hospital.Patients)
            {
              
                result.Add(CreatePatientJson(pat, hospital));                
            }
            return result;        
        }
        protected override void AddProcedureToBundle(Bundle bundle, IArrivalSick pat, Operation op, List list)
        {
            foreach (Parameter param in op.Params)
            {
                if (!history.Contains(param))
                {
                    AddParamToProcedure(param, pat, op, bundle, list);
                    history.Add(param);
                }
            }
        }
        public ConverterJson(ParameterHistory history)
        {
            this.history = history;

        }

      
       // private static  CodeableConcept CreateMilestoneProcedureConcept()
       // {
       //     
       //     CodeableConcept codeableConcept = new CodeableConcept();
       //     codeableConcept.Coding.Add(
       //         new Coding
       //         {
       //             System = "http://snomed.info/sct",
       //             Code = "397788003",
       //             Display = "Procedure milestone (observable entity)"
       //         }
       //     );
       //     return codeableConcept;
       // }
       //
       //
       // private void AddParamToProcedure(Parameter param, ArrivalSick pat, Operation op,Bundle bundle, List list)
       // {
       //     Procedure p = CreateProcedure(list, param);
       //     CreateSubject(pat, p);
       //     CreatePeriod(param, p);
       //     CreateCoding(param, p);
       //     CreateRecorder(param, p);
       //     CreatePartOf(op, p);
       //     CreateStatus(p);
       //     AddProcedureToBundle(bundle, p,op,param);
       // }
       //
       // private void CreateStatus(Procedure p)
       // {
       //     p.Status = EventStatus.Completed;
       // }
       //
       // private void CreatePartOf(Operation op, Procedure p)
       // {
       //     p.PartOf = new List<ResourceReference>();
       //     p.PartOf.Add(new ResourceReference() { Reference = "Procedure/"+op.Id });
       // }
       //
       // private static void AddProcedureToBundle(Bundle bundle, Procedure p, Operation op, Parameter param)
       // {
       //     var first_entry = new Bundle.EntryComponent();
       //     first_entry.Resource = p;
       //     first_entry.FullUrl = "https://srv-orchestra/Procedure/" + op.Id + "/Milestone/" + param.Milestone;
       //         //param.Id;
       //     bundle.Entry.Add(first_entry);
       //     
       // }
       //
       // private static Procedure CreateProcedure(List list, Parameter param)
       // {
       //     Procedure p = new Procedure();
       //     p.Id = param.Milestone;
       //     p.Category = new CodeableConcept();
       //     p.Code = new CodeableConcept();
       //
       //     AddProcedureToList(p, list);
       //     return p;
       // }
       //
       // private static void AddProcedureToList(Procedure p, List list)
       // {
       //     list.Entry.Add(
       //         new List.EntryComponent
       //         {
       //             Item = new ResourceReference
       //             {
       //                 Reference = p.Id
       //             }
       //         }
       //     );
       // }
       //
       // private static void CreateSubject(ArrivalSick pat, Procedure p)
       // {
       //     ResourceReference sub = new ResourceReference();
       //     sub.Reference = $"Patient/{pat.Id}";
       //     sub.Display = pat.FullName ;
       //     p.Subject = sub;
       //   
       //     
       //     
       // }
       //
       // private static void CreateCoding(Parameter param, Procedure p)
       // {
       //     Coding coding = new Coding();
       //     coding.System = "http://snomed.info/sct";
       //     coding.Code = param.Id;
       //     coding.Display = param.Name;
       //     p.Code.Coding.Add(coding);
       //     p.Category = CreateMilestoneProcedureConcept();
       //     
       //     
       //     
       // }
       //
       //
       // private static void CreatePeriod(Parameter param, Procedure p)
       // {
       //     Period period = new Period();
       //     period.Start = param.Date.ToString("yyyy-MM-ddTHH:mm:sszzz");
       //     period.End = param.Date.ToString("yyyy-MM-ddTHH:mm:sszzz");
       //
       //     //FhirTypes.DateTime datatimenew = FhirTypes.DateTime.Parse(param.Date.ToString("yyyy-MM-ddTHH:mm:sszzz"));
       //     p.Performed = period;
       //     
       //     
       // }
       //
       // private static void CreateRecorder(Parameter param, Procedure p)
       // {
       //     ResourceReference sub = new ResourceReference();
       //     sub.Reference = "Practitioner/EXTICM";
       //     p.Recorder = sub;
       // }
       //
        
    }
}
