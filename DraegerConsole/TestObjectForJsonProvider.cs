using DraegerJson;
using Draft_Draeger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole
{
    internal class TestObjectForJsonProvider : IHospitalProvider
    {
        public Hospital GetHospital()
        {
            ArrivalSick patient1 = new ArrivalSick();
            patient1.Id = 1;
            
            Operation procedure1 = new Operation();
            procedure1.ResourceType = "Procedure";
            procedure1.Id = "OPERATION_123";
            procedure1.Status = "completed";

            patient1.Procedures.Add(procedure1);

            Parameter parameter1 = new Parameter();
            parameter1.Id = "442385007";
            parameter1.Name = "Time of patient arrival in procedure room";
            parameter1.Date = new DateTime(2023, 7, 10);

            Parameter parameter2 = new Parameter();
            parameter2.Id = "442126001";
            parameter2.Name = "Start time for preparation of patient in procedure room";
            parameter2.Date = new DateTime(2023, 7, 11);

            procedure1.Params.Add(parameter1);
            procedure1.Params.Add(parameter2);

            ArrivalSick patient2 = new ArrivalSick();
            patient2.Id = 2;

            Operation procedure2 = new Operation();
            procedure2.ResourceType = "Procedure";
            procedure2.Id = "OPERATION_123_1";
            procedure2.Status = "completed";

            patient2.Procedures.Add(procedure2);

            Parameter parameter3 = new Parameter();
            parameter3.Id = "442273001";
            parameter3.Name = "Time procedure room ready for next case";
            parameter3.Date = new DateTime(2023, 6, 17);

            procedure2.Params.Add(parameter3);

            Hospital hospital = new Hospital();
            hospital.Patients.Add(patient1);
            hospital.Patients.Add(patient2);
            return hospital;
        }
    }
}
