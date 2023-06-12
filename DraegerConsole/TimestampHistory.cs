using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole
{
    public class TimestampHistoryUnit
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    public class TimestampHistory
    {
        public List <TimestampHistoryUnit> Units { get; set; } = new List<TimestampHistoryUnit>();
    }
}
