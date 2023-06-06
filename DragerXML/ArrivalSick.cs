using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragerXML
{
    public class ArrivalSick
    {
        public string Id { get; set; } = "";
        public List <Operation> Procedures { get; set; } = new List<Operation>();
        
    }
}
