using DraegerConsole.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragerXML
{
    internal class AmpulaSizeDatabase
    {
        private List<AmpulaSizeDatabaseEntry> entries = new List<AmpulaSizeDatabaseEntry>();
        public IReadOnlyList<AmpulaSizeDatabaseEntry> Entries { get { return entries; } }
        void Load (string pathToDBfile)
        {
            entries = new List<AmpulaSizeDatabaseEntry>();
            using(StreamReader sr = new StreamReader(pathToDBfile))
            {
                while (!sr.EndOfStream)
                {
                    String? line = sr.ReadLine();
                    if (line == null)
                        throw new ReadDBException("Failed to read from Ampula size Database");
                    var tokens = line.Split(';');
                    if ((tokens.Length != 3)&&(tokens.Length >0))
                        throw new FileFormatException("Incorrect format of Ampula size Database ");
                    var entry = new AmpulaSizeDatabaseEntry
                    {
                        MedicamentName = tokens[0],
                        AmpulaSize = tokens[1],
                        MeasurementType = tokens[2]
                    };
                    entries.Add(entry);                                        
                }
            }
           

        }
    }
    internal class AmpulaSizeDatabaseEntry
    {
        public string MedicamentName { get; set; } = string.Empty;
        public string AmpulaSize { get; set; } = string.Empty;
        public string MeasurementType { get; set; } = string.Empty;

    }
}
