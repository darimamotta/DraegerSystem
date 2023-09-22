using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole
{
    internal class FixedOffsetTimeStampUpdater:ITimestampUpdater
    {
        public TimeSpan Offset { get; private set; }

        public DateTime CurrentTimestamp { get; private set; }

        public DateTime PastTimestamp { get; private set; }

        public bool CanUpdate => true;
        public FixedOffsetTimeStampUpdater(TimeSpan offset) 
        {
            this.Offset = offset;
            this.CurrentTimestamp = DateTime.Now;
            this.PastTimestamp = CurrentTimestamp - Offset;
        }

        public void UpdateTimestamps()
        {
            this.CurrentTimestamp = DateTime.Now;
            this.PastTimestamp = CurrentTimestamp - Offset;
        }
    }
}
