using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public class Patient
    {
        public int Id { get; set; }
        public Procedure Procedure { get; set; }
        public List<Parameter> Params { get; set; } = new List<Parameter>();
    }
}
