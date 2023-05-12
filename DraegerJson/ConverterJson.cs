using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DraegerJson
{
   

    //Serialze Objects
    public class ConverterJson : IConverterJson
    {
        public string Convert(Hospital hospital)
        {
            return JsonConvert.SerializeObject(hospital, Formatting.Indented);
        }
    }
}
