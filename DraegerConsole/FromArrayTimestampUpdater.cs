using DraegerConsole.Exceptions;
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

        public DateTime PastTimestamp { get { return dateTimes[index]; } }
        public bool CanUpdate { get { return index < dateTimes.Length - 2; } }
        private int index;
        private DateTime[] dateTimes = new DateTime[0];
        
        public FromArrayTimestampUpdater(DateTime[] dateTimes) 
        {
            if (dateTimes.Length < 2)
            {
                throw new TimestampUpdaterException("The number of timestamps must be greater than 1.");
            }
            this.dateTimes = dateTimes;
            index = 0;
        }
        public void UpdateTimestamps()
        {
            if(index<dateTimes.Length-2)
            {
                index++;
            }
            else
            { throw new TimestampUpdaterException("The array of timestamps is exceeded!"); }
        }
    }
}
