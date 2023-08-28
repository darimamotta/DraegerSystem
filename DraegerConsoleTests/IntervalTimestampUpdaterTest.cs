using DraegerConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsoleTests
{
    public class IntervalTimestampUpdaterTest
    {
        [Test]
        public void Create_FromSomeArguments_PastTimestampEqualsStartTimestamp()
        {
            DateTime startTimestamp = new DateTime(2023, 8, 28, 14, 20, 00);
            DateTime firstIntervalPoint = new DateTime(2023,8, 28, 14, 20,13,15);
            int intervalInSeconds = 5;
            IntervalTimestampUpdater intervalTimestampUpdater = new IntervalTimestampUpdater(intervalInSeconds, startTimestamp, firstIntervalPoint);
            Assert.That(intervalTimestampUpdater.PastTimestamp,Is.EqualTo(startTimestamp));          

        }
        [Test]
        public void Create_FromSomeArguments_CurrentTimestampEqualsFirstIntervalPoint()
        {
            DateTime startTimestamp = new DateTime(2023, 8, 28, 14, 20, 00);
            DateTime firstIntervalPoint = new DateTime(2023, 8, 28, 14, 20, 13, 15);
            int intervalInSeconds = 5;
            IntervalTimestampUpdater intervalTimestampUpdater = new IntervalTimestampUpdater(intervalInSeconds, startTimestamp, firstIntervalPoint);
            Assert.That(intervalTimestampUpdater.CurrentTimestamp, Is.EqualTo(firstIntervalPoint));
            

        }
        [Test]
        public void Create_FromSomeArguments_IntervalInSecondsEqualsInterval()
        {
            DateTime startTimestamp = new DateTime(2023, 8, 28, 14, 20, 00);
            DateTime firstIntervalPoint = new DateTime(2023, 8, 28, 14, 20, 13, 15);
            int intervalInSeconds = 5;
            IntervalTimestampUpdater intervalTimestampUpdater = new IntervalTimestampUpdater(intervalInSeconds, startTimestamp, firstIntervalPoint);
            Assert.That(intervalTimestampUpdater.IntervalInSeconds, Is.EqualTo(intervalInSeconds));

        }
        [Test]
        public void Update_OneTime_CurrentTimestampUpdated()
        {
            DateTime startTimestamp = new DateTime(2023, 8, 28, 14, 20, 00);
            DateTime firstIntervalPoint = new DateTime(2023, 8, 28, 14, 20, 13, 15);
            int intervalInSeconds = 5;
            IntervalTimestampUpdater intervalTimestampUpdater = new IntervalTimestampUpdater(intervalInSeconds, startTimestamp, firstIntervalPoint);
            intervalTimestampUpdater.UpdateTimestamps();
            
            Assert.That(intervalTimestampUpdater.CurrentTimestamp, Is.EqualTo(firstIntervalPoint.AddSeconds(intervalInSeconds)));
         }
        [Test]
        public void Update_OneTime_PastTimestampUpdated()
        {
            DateTime startTimestamp = new DateTime(2023, 8, 28, 14, 20, 00);
            DateTime firstIntervalPoint = new DateTime(2023, 8, 28, 14, 20, 13, 15);
            int intervalInSeconds = 5;
            IntervalTimestampUpdater intervalTimestampUpdater = new IntervalTimestampUpdater(intervalInSeconds, startTimestamp, firstIntervalPoint);
            intervalTimestampUpdater.UpdateTimestamps();

            Assert.That(intervalTimestampUpdater.PastTimestamp, Is.EqualTo(firstIntervalPoint));
        }
        public void Update_TwoTimes_CurrentTimestampUpdated()
        {
            DateTime startTimestamp = new DateTime(2023, 8, 28, 14, 20, 00);
            DateTime firstIntervalPoint = new DateTime(2023, 8, 28, 14, 20, 13, 15);
            int intervalInSeconds = 5;
            IntervalTimestampUpdater intervalTimestampUpdater = new IntervalTimestampUpdater(intervalInSeconds, startTimestamp, firstIntervalPoint);
            intervalTimestampUpdater.UpdateTimestamps();
            intervalTimestampUpdater.UpdateTimestamps();
            DateTime newCurrent = firstIntervalPoint.AddSeconds(intervalInSeconds);
            Assert.That(intervalTimestampUpdater.CurrentTimestamp, Is.EqualTo(newCurrent.AddSeconds(intervalInSeconds)));
        }
        [Test]
        public void Update_TwoTimes_PastTimestampUpdated()
        {
            DateTime startTimestamp = new DateTime(2023, 8, 28, 14, 20, 00);
            DateTime firstIntervalPoint = new DateTime(2023, 8, 28, 14, 20, 13, 15);
            int intervalInSeconds = 5;
            IntervalTimestampUpdater intervalTimestampUpdater = new IntervalTimestampUpdater(intervalInSeconds, startTimestamp, firstIntervalPoint);
            intervalTimestampUpdater.UpdateTimestamps();
            intervalTimestampUpdater.UpdateTimestamps();
            DateTime newPast =firstIntervalPoint.AddSeconds(intervalInSeconds);
            Assert.That(intervalTimestampUpdater.PastTimestamp, Is.EqualTo(newPast));
        }
    }
}
     