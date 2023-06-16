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

namespace DraegerJson
{
   

    //Serialze Objects
    public class ConverterJson : IConverterJson
    {
        private Hospital? currentHospital;
        public Dictionary<string, string> Convert(Hospital hospital)
        {
            Dictionary<string,string> result = new Dictionary<string,string>();
            currentHospital = hospital;
            
            foreach (ArrivalSick pat in hospital.Patients)
            {
                Bundle bundle = CreateNewBundle(hospital);
                AddPatientToBundle(bundle, pat);
                result.Add(pat.Id, CreateJsonFromBundle(bundle));
                
            }
            return result;        
        }

        private string CreateJsonFromBundle(Bundle bundle)
        {
            var options = new JsonSerializerOptions().ForFhir(typeof(Bundle).Assembly).Pretty();
            return JsonSerializer.Serialize(bundle, options);
        }

        private void AddPatientToBundle(Bundle bundle, ArrivalSick pat)
        {
            AddListOfProcedures(bundle, pat);
            foreach (Operation op in pat.Procedures)
            {
                if (op.Exist)
                    AddProcedureToBundle(bundle, pat, op);
            }
        }

        private void AddListOfProcedures(Bundle bundle, ArrivalSick pat)
        {
            List list = CreateEmptyList(pat);
            FillingListOfProcedures(pat, list);
            bundle.Entry.Add(new Bundle.EntryComponent { Resource = list, FullUrl = "https://srv-orchestra/List/1" });

        }


        private static void FillingListOfProcedures(ArrivalSick pat, List list)
        {
            foreach (Operation op in pat.Procedures)
            {
                if (op.Exist)
                    list.Entry.Add(
                        new List.EntryComponent
                        {
                            Item = new ResourceReference
                            {
                                Reference = "Procedure/" + op.Id
                            }
                        }
                    );
            }
        }

        private List CreateEmptyList(ArrivalSick pat)
        {
            List list = new List();
            list.Id = "example";
            list.Status = List.ListStatus.Current;
            list.Mode = ListMode.Snapshot;
            

            list.Code = CreateMillestoneProcedureConcept();
            list.Subject = new ResourceReference
            {
                Reference = "Patient/" + pat.Id

            };
            list.Source = new ResourceReference
            {
                Reference = "Practitioner/EXTICM"
            };
            return list;

        }
        private static  CodeableConcept CreateMillestoneProcedureConcept()
        {
            
            CodeableConcept codeableConcept = new CodeableConcept();
            codeableConcept.Coding.Add(
                new Coding
                {
                    System = "http://snomed.info/sct",
                    Code = "397788003",
                    Display = "Procedure milestone (observable entity)"
                }
            );
            return codeableConcept;
        }

        private void AddProcedureToBundle(Bundle bundle, ArrivalSick pat, Operation op)
        {  
            foreach (Parameter param in op.Params)
            {
                AddParamToProcedure(param, pat, op, bundle);
            }  
        }

        private void AddParamToProcedure(Parameter param, ArrivalSick pat, Operation op,Bundle bundle)
        {
            Procedure p = CreateProcedure(op);
            CreateSubject(pat, p);
            CreatePeriod(param, p);
            CreateCoding(param, p);
            CreateRecorder(param, p);
            AddProcedureToBundle(bundle, p);
        }

        private static void AddProcedureToBundle(Bundle bundle, Procedure p)
        {
            var first_entry = new Bundle.EntryComponent();
            first_entry.Resource = p;
            first_entry.FullUrl = "https://srv-orchestra/Procedure/" + p.Id;
            bundle.Entry.Add(first_entry);
            
        }

        private static Procedure CreateProcedure(Operation op)
        {
            Procedure p = new Procedure();
            p.Id = "Procedure/"+op.Id;
            //p.Category = new CodeableConcept();
            p.Code = new CodeableConcept();
            
            return p;
        }
      

        private static void CreateSubject(ArrivalSick pat, Procedure p)
        {
            ResourceReference sub = new ResourceReference();
            sub.Reference = $"Patient/{pat.Id}";
            sub.Display = pat.FullName ;
            p.Subject = sub;
            
            
        }

        private static void CreateCoding(Parameter param, Procedure p)
        {
            Coding coding = new Coding();
            coding.System = "http://snomed.info/sct";
            coding.Code = param.Id;
            coding.Display = param.Name;
            p.Code.Coding.Add(coding);
            p.Category = CreateMillestoneProcedureConcept();
            
            
        }

        private static void CreatePeriod(Parameter param, Procedure p)
        {
            Period period = new Period();
            period.Start = param.Date.ToString("yyyy-MM-ddTHH:mm:sszzz");
            period.End = param.Date.ToString("yyyy-MM-ddTHH:mm:sszzz");
            
            p.Performed = period;
            
        }
        private static void CreateRecorder(Parameter param, Procedure p)
        {
            ResourceReference sub = new ResourceReference();
            sub.Reference = "Practitioner/EXTICM";
            p.Recorder = sub;
        }
        private Bundle CreateNewBundle(Hospital hospital)
        {
            var bundle = new Bundle();
            bundle.Timestamp = new DateTimeOffset(hospital.Timestamp);
            bundle.Type = Bundle.BundleType.Collection;
            return bundle;
        }
        
    }
}
