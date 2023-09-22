using DraegerJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public class ConverterJsonWithLimitedUpdateTime : ConverterJsonBase
    {
        public bool CanAddOldPatient { get; set; }
        public ConverterJsonWithLimitedUpdateTime(TimeSpan limitUpdateTime)
        {
            LimitUpdateTime = limitUpdateTime;
            patients = new Dictionary<string, ArrivalSick>();
            CanAddOldPatient = false;
        }

       
        public TimeSpan LimitUpdateTime { get; private set; }

        public override List<PatientJson> Convert(Hospital hospital)
        {
            RemoveOldPatients(hospital.Timestamp);
            AddNewPatients(hospital);
            CalculateChangesOfExistingPatients(hospital);
            List<PatientJson> result = CalculateResult(hospital);
            FixChanges(hospital);
            CanAddOldPatient = false;
            return result;
        }

        private List<PatientJson> CalculateResult(Hospital hospital)
        {
            List<PatientJson> result = new List<PatientJson>();

            foreach (ArrivalSick pat in newPatients.Values)
            {
                result.Add(CreatePatientJson(pat, hospital));
            }
            foreach (ArrivalSickChanges pat in patientsChanges.Values)
            {
                result.Add(CreatePatientJson(pat,hospital));
            }
            return result;
        }

        private void FixChanges(Hospital hospital)
        {
            foreach(var patient in newPatients.Values)
            {
                patients.Add(patient.Id, patient);
            }
            foreach(var changeId in patientsChanges.Keys)
            {
                patients[changeId] = hospital.Patients.Find(p=>p.Id==changeId)!;
            }
        }

        private void CalculateChangesOfExistingPatients(Hospital hospital)
        {
            patientsChanges = new Dictionary<string, ArrivalSickChanges>();
            foreach(var patient in hospital.Patients)
            {
                if (patients.ContainsKey(patient.Id))
                {
                    ArrivalSickChanges changes = new ArrivalSickChanges(patient, patients[patient.Id]);
                    if(changes.ContainsChange)
                        patientsChanges.Add(patient.Id, changes);
                }
            }
        }

        private void RemoveOldPatients(DateTime timestamp)
        {
            var keys = new List<string>(patients.Keys);
            foreach(var id in keys)
            {
                if (patients.ContainsKey(id) && 
                    patients[id].AdmissionToWardDate + LimitUpdateTime < timestamp)
                {
                    patients.Remove(id);
                }
            }   
        }
        private void AddNewPatients(Hospital hospital)
        {
            newPatients = new Dictionary<string, ArrivalSick>();
            foreach(var patient in hospital.Patients) 
            {
                if (!patients.ContainsKey(patient.Id) &&
                    (patient.AdmissionToWardDate + LimitUpdateTime >= hospital.Timestamp || CanAddOldPatient)) 
                { 
                    newPatients.Add(patient.Id, patient);                    
                }
            }
        }
    

        private Dictionary<string, ArrivalSick> patients;
        private Dictionary<string, ArrivalSickChanges> patientsChanges = null!;
        private Dictionary<string, ArrivalSick> newPatients = null!;
    }
}
