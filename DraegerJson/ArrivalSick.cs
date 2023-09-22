using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public class ArrivalSick:IArrivalSick
    {
        public string Id { get; set; } = "";
        public string AufnahmeNR { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Location { get; set; } = "";
        public DateTime OPDate { get; set; } 
        public DateTime AdmissionToWardDate { get; set; }
        public List <Operation> Procedures { get; set; } = new List<Operation>();
        public ArrivalSickChanges GetDifference(ArrivalSick prototype)
        {
            return new ArrivalSickChanges(this, prototype);
        }
        
    }
}
