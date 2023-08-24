using DraegerConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsoleTests
{
    internal class NowUpdaterTests
    {
        [Test]
        public void Create_WithSomeStartTimestampAndZeroOffset_PastTimestampEquals()
        {
            NowTimestampUpdater updater = new NowTimestampUpdater(0, new DateTime(1990, 3, 5, 12, 25, 0));
            Assert.That(updater.PastTimestamp, Is.EqualTo(new DateTime(1990, 3, 5, 12, 25, 0)));

        }
        [Test]
        public void UpdateTimestamps_OneUpdate_PastEqualsToPreviousCurrent()
        {
            NowTimestampUpdater updater = new NowTimestampUpdater(0, new DateTime(1995, 3, 5, 12, 25, 0));
            var currentTimestamp = updater.CurrentTimestamp;
            updater.UpdateTimestamps();
            Assert.That(updater.PastTimestamp,Is.EqualTo(currentTimestamp));

        }
       
    }
}
