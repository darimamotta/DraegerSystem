using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public class ArrivalSick
    {
        public string Id { get; set; } = "";
        public string AufnahmeNR { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Location { get; set; } = "";
        public DateTime OPDate { get; set; } 
        public List <Operation> Procedures { get; set; } = new List<Operation>();
        
    }
}
