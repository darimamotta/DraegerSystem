using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public interface IConverterJson
    {
        List<PatientJson> Convert(Hospital hospital);
    }
}
