using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public class ArrivalSickChanges:IArrivalSick
    {
        public bool IdChanged { get; private set; }        
        public bool AufnahmeNRChanged { get; private set; }
        public bool FullNameChanged { get; private set; } 
        public bool LocationChanged { get; private set; } 
        public bool OPDateChanged { get; private set; }
        public bool AdmissionToWardDateChanged { get; private set; }
        public bool ProceduresChanged { get; private set; }
        public bool ContainsChange { get { return IdChanged || 
                                                  AufnahmeNRChanged || 
                                                  FullNameChanged || 
                                                  LocationChanged || 
                                                  OPDateChanged || 
                                                  AdmissionToWardDateChanged || 
                                                  ProceduresChanged; } }


        public string NewId { get; set; } = "";
        public string NewAufnahmeNR { get; set; } = "";
        public string NewFullName { get; set; } = "";
        public string NewLocation { get; set; } = "";
        public DateTime NewOPDate { get; set; }
        public DateTime NewAdmissionToWardDate { get; set; }
        public List<Operation> NewProcedures { get; set; } = new List<Operation>();

        public string Id => NewId;

        public string AufnahmeNR => NewAufnahmeNR;
        public string FullName => NewFullName;

        public string Location => NewLocation;

        public DateTime OPDate => NewOPDate;

        public DateTime AdmissionToWardDate => NewAdmissionToWardDate;

        public List<Operation> Procedures => new List<Operation>(NewProcedures);

        public ArrivalSickChanges(ArrivalSick patient, 
                                  ArrivalSick prototype) 
        { 
            if(patient.Id!=prototype.Id)            
                IdChanged = true;
            NewId= patient.Id;            
            if(patient.AufnahmeNR!=prototype.AufnahmeNR)            
                AufnahmeNRChanged = true;
            NewAufnahmeNR= patient.AufnahmeNR;
            
            if( patient.FullName!=prototype.FullName)
                FullNameChanged = true;
            NewFullName= patient.FullName;            
            if(patient.Location!=prototype.Location)
                LocationChanged = true;
            NewLocation= patient.Location;            
            if(patient.OPDate!=prototype.OPDate)
                OPDateChanged = true;
            NewOPDate= patient.OPDate;            
            if(patient.AdmissionToWardDate!=prototype.AdmissionToWardDate)
                AdmissionToWardDateChanged = true;
            NewAdmissionToWardDate= patient.AdmissionToWardDate;
            
            CreateProcedureChanges(prototype.Procedures, patient.Procedures);
        }

        private void CreateProcedureChanges(List<Operation> oldProcedures, List<Operation> newProcedures)
        {
            NewProcedures = new List<Operation>();
            foreach(Operation op in newProcedures) 
            {
                if (!oldProcedures.Contains(op))
                    NewProcedures.Add(GetChangedProcedure(oldProcedures, op));
            }
            if(NewProcedures.Count > 0)
                ProceduresChanged = true;
        }

        private Operation GetChangedProcedure(List<Operation> oldProcedures, Operation newOp)
        {
            Operation? chOp = oldProcedures.Find(o=>o.Id.Equals(newOp.Id));
            if (chOp == null)
                return newOp;
            Operation result = new Operation
            {
                Id = newOp.Id,
                Status = newOp.Status,
                ResourceType = newOp.ResourceType,
                Params = new List<Parameter>()
            };
            result.Params.AddRange(GetChangedParams(newOp,chOp));
            return result;

        }

        private IEnumerable<Parameter> GetChangedParams(Operation newOp, Operation op)
        {
            List<Parameter> chParams = new List<Parameter>();
            foreach(Parameter param in newOp.Params)
            {
                if(!op.Params.Contains(param))
                    chParams.Add(param);
            }
            return chParams;
        }
    }
}
