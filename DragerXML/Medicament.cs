using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DragerXML
{
    public class Medicament
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Dose { get; set; } = "";

        public string AmpulaSize { get; set; } = "";
        public double UsedAmpulas 
        { 
            get
            {
                double dose = double.Parse(Dose, NumberFormatInfo.InvariantInfo);
                double ampulaSize = double.Parse(AmpulaSize, NumberFormatInfo.InvariantInfo);
                return dose/ampulaSize;
            } 
        }
       
    }
}
