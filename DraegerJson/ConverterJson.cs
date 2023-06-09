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

namespace DraegerJson
{
   

    //Serialze Objects
    public class ConverterJson : IConverterJson
    {
        private Hospital? currentHospital;
        public string Convert(Hospital hospital)
        {
            currentHospital = hospital;
            Bundle bundle = CreateNewBundle(hospital);
            foreach (ArrivalSick pat in hospital.Patients)
            {
                AddPatientToBundle(bundle, pat);
            }
            return CreateJsonFromBundle(bundle);        
        }

        private string CreateJsonFromBundle(Bundle bundle)
        {
            var options = new JsonSerializerOptions().ForFhir(typeof(Bundle).Assembly).Pretty();
            return JsonSerializer.Serialize(bundle, options);
        }

        private void AddPatientToBundle(Bundle bundle, ArrivalSick pat)
        {
            foreach (Operation op in pat.Procedures)
            {
                if (op.Exist)
                    AddProcedureToBundle(bundle, pat, op);
            }
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
            bundle.Entry.Add(first_entry);
        }

        private static Procedure CreateProcedure(Operation op)
        {
            Procedure p = new Procedure();
            p.Id = "Procedure/"+op.Id;
            p.Category = new CodeableConcept();
            return p;
        }

        private static void CreateSubject(ArrivalSick pat, Procedure p)
        {
            ResourceReference sub = new ResourceReference();
            sub.Reference = $"Patient/{pat.Id}";
            p.Subject = sub;
        }

        private static void CreateCoding(Parameter param, Procedure p)
        {
            Coding coding = new Coding();
            coding.System = "http://snomed.info/sct";
            coding.Code = param.Id;
            coding.Display = param.Name;
            p.Category.Coding.Add(coding);
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
            sub.Reference = "Technical User/EXTMON";
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
