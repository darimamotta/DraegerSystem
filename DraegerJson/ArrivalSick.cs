using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public class ArrivalSick
    {
        public int Id { get; set; }
        public List <Procedure> Procedures { get; set; } = new List<Procedure>();
        
    }
}
