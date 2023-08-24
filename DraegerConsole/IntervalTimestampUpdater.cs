using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole
{
    public class IntervalTimestampUpdater : ITimestampUpdater
    {
        public IntervalTimestampUpdater(int intervalInSeconds,DateTime startTimestamp, DateTime firstIntervalPoint)
        {
            IntervalInSeconds = intervalInSeconds;
            PastTimestamp = startTimestamp;
            CurrentTimestamp = firstIntervalPoint;
        }

        public DateTime CurrentTimestamp { get;private set; }

        public DateTime PastTimestamp { get; private set; }

        public bool CanUpdate => true;
        public int IntervalInSeconds { get; private set; }
        public void UpdateTimestamps()
        {
            PastTimestamp = CurrentTimestamp;
            CurrentTimestamp = CurrentTimestamp.AddSeconds(IntervalInSeconds);
           
        }
    }
}
