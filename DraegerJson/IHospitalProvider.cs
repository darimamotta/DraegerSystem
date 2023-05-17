using Draft_Draeger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerJson
{
    public interface IHospitalProvider
    {
        public Hospital? GetHospital();
    }
}
