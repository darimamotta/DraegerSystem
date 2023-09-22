using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public interface IArrivalSick
    {
        public string Id { get; }
        public string AufnahmeNR { get; }
        public string FullName { get; } 
        public string Location { get; } 
        public DateTime OPDate { get; }
        public DateTime AdmissionToWardDate { get; }
        public List<Operation> Procedures { get;  }
    }
}
