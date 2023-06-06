using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragerXML
{
    public class Hospital
    {
        public List<ArrivalSick> Patients { get; set; } = new List<ArrivalSick>();
        public DateTime Timestamp { get; set; }
    }
}
