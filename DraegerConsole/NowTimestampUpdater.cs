using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole
{
    public class NowTimestampUpdater : ITimestampUpdater
    {
        public DateTime CurrentTimestamp { get; private set; }

        public DateTime PastTimestamp { get; private set; }
        private int offsetInSeconds;
        public NowTimestampUpdater(int offsetInSeconds)
        {
            this.offsetInSeconds = offsetInSeconds;
            CurrentTimestamp = DateTime.Now.AddSeconds(offsetInSeconds);
            PastTimestamp = CurrentTimestamp;
        }


        public void UpdateTimestamps()
        {
            PastTimestamp = CurrentTimestamp;
            CurrentTimestamp = DateTime.Now;

        }
    }
}
