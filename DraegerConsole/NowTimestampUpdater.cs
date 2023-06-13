using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole
{
    public class NowTimestampUpdater : ITimestampUpdater
    {
        public DateTime CurrentTimestamp { get; set; }

        public DateTime PastTimestamp { get; set; }
        private int offsetInSeconds;
        public NowTimestampUpdater(int offsetInSeconds, DateTime firstTimestamp)
        {
            this.offsetInSeconds = offsetInSeconds;
            CurrentTimestamp = DateTime.Now.AddSeconds(offsetInSeconds);
            PastTimestamp = firstTimestamp;
        }


        public void UpdateTimestamps()
        {
            PastTimestamp = CurrentTimestamp;
            CurrentTimestamp = DateTime.Now.AddSeconds(offsetInSeconds);

        }
    }
}
