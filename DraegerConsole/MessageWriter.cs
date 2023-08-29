using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole
{
    internal class MessageWriter
    {
        private ITimestampUpdater timestampUpdater;

        public MessageWriter(ITimestampUpdater timestampUpdater)
        {
            this.timestampUpdater = timestampUpdater;
        }

        public void WriteProcessMessage()
        {
            Console.WriteLine(
               "Process from {0} to {1}...",
               timestampUpdater.PastTimestamp.ToString("yyyy.MM.dd_HH.mm.ss"),
               timestampUpdater.CurrentTimestamp.ToString("yyyy.MM.dd_HH.mm.ss")
           );
        }
        public void WriteOKMessage() 
        {
            Console.WriteLine("OK. Enter 'Exit' for Stop ");
        }
        public void WriteStartMessage()
        {
            Console.WriteLine("Application started at " + DateTime.Now);
        }
    }
}
