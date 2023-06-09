using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole
{
    public class FromArrayTimestampUpdater : ITimestampUpdater
    {
        public DateTime CurrentTimestamp { get { return dateTimes[index + 1]; } }

        public DateTime PastTimestamp { get { return dateTimes[index]; }}
        private int index;
        private DateTime[] dateTimes = new DateTime[0];
        
        public FromArrayTimestampUpdater(DateTime[] dateTimes) 
        {
            this.dateTimes = dateTimes;
            index = 0;
        }
        public void UpdateTimestamps()
        {
            if(index<dateTimes.Length-2)
            {
                index++;
            }
        }
    }
}
