using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System.Text.Json;
using FhirTypes = Hl7.Fhir.ElementModel.Types;

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


            Procedure p = new Procedure();
            p.Id = "OPERATION_123_1";
            p.Category = new CodeableConcept();
            ResourceReference resRef = new ResourceReference();
            resRef.Reference = "Procedure/OPERATION_123";
            p.PartOf.Add(resRef);
            Coding coding = new Coding();
            coding.System = "http://snomed.info/sct";
            coding.Code = "363788007";
            coding.Display = "Eintritt erfolgt";
            p.Category.Coding.Add(coding);
            ResourceReference sub = new ResourceReference();
            sub.Reference = "Patient/PATIENT_789";
            p.Subject = sub;
            p.Recorder = new ResourceReference() { Reference = "Practitioner/DAMUE" };
            Period period = new Period();
            period.Start = "2023-05-18T16:14:00";
            period.End = "2023-05-18T16:14:05";
            p.Performed = period;
            Operation p2 = new Operation();
            p2.Id = "OPERATION_125_2";
            p2.Category = new CodeableConcept();
            ResourceReference resRef2 = new ResourceReference();
            resRef2.Reference = "Procedure/OPERATION_126";
            p2.PartOf.Add(resRef2);
            Coding coding2 = new Coding();
            coding2.System = "http://snomed.info/sct";
            coding2.Code = "419126006";
            coding2.Display = "Beginn Anästhesiebetreuung";
            p2.Category.Coding.Add(coding2);
            ResourceReference sub2 = new ResourceReference();
            sub2.Reference = "Patient/PATIENT_678";
            p2.Subject = sub2;
            p2.Recorder = new ResourceReference() { Reference = "Practitioner/meda" };
            Period period2 = new Period();
            period2.Start = "2023-05-18T16:17:00";
            period2.End = "2023-05-18T16:17:05";

            p2.Performed = period2;


            //Patient p = new Patient();
            //p.Active = true;
            //p.Address.Add(new Address() { City = "Zurich" });
            //p.Name.Add(new HumanName() { Family = "Schwarz" });
            //
            var bundle = new Bundle();
            bundle.Timestamp = new DateTimeOffset(DateTime.Now);
            bundle.Type = Bundle.BundleType.Collection;
            var first_entry = new Bundle.EntryComponent();
            first_entry.Resource = p;
            var second_entry = new Bundle.EntryComponent();
            second_entry.Resource = p2;
            bundle.Entry.Add(first_entry);
            bundle.Entry.Add(second_entry);

            var options = new JsonSerializerOptions().ForFhir(typeof(Bundle).Assembly).Pretty();
            string patientJson = JsonSerializer.Serialize(bundle, options);

            //var options = new JsonSerializerOptions().ForFhir(typeof(Patient).Assembly).Pretty();
            //string patientJson = JsonSerializer.Serialize(p, options);




            //var serializer = new FhirJsonSerializer(new SerializerSettings()
            //{
            //    Pretty = true
            //});
            //
            //var json = serializer.SerializeToString(p, Hl7.Fhir.Rest.SummaryType.Text);

            File.WriteAllText("eitstempel.json", patientJson);
        
    }

        private string CreateJsonFromBundle(Bundle bundle)
        {
            throw new NotImplementedException();
        }

        private void AddPatientToBundle(Bundle bundle, ArrivalSick pat)
        {
            foreach (Operation op in pat.Procedures)
            {
                AddProcedureToBundle(bundle, pat, op);
            }
        }

        private void AddProcedureToBundle(Bundle bundle, ArrivalSick pat, Operation op)
        {
            Procedure p = new Procedure();
            p.Id = op.Id;
            p.Category = new CodeableConcept();
            foreach (Parameter param in op.Params)
            {
                AddParamToProcedure(param, p);
            }
            ResourceReference sub = new ResourceReference();
            sub.Reference = $"Patient/{pat.Id}";
            p.Subject = sub;
         
            Period period = new Period();
            period.Start = currentHospital!.Start.ToString("yyyy-MM-ddTHH:mm:ssZ");
            period.End = currentHospital!.End.ToString("yyyy-MM-ddTHH:mm:ssZ");
            p.Performed = period;

        }

        private void AddParamToProcedure(Parameter param, Procedure p)
        {
            throw new NotImplementedException();
        }

        private Bundle CreateNewBundle(Hospital hospital)
        {
            var bundle = new Bundle();
            bundle.Timestamp = new DateTimeOffset(DateTime.Now);
            bundle.Type = Bundle.BundleType.Collection;
            return bundle;
        }
        
    }
}
