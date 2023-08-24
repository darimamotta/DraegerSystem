using DraegerConsole;
using DraegerConsole.Exceptions;

namespace DraegerConsoleTests
{
    public class FromArrayUpdaterTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Create_WithSomeArray_PastEqualsZeroIndexOfElement()
        {
            FromArrayTimestampUpdater updater = new FromArrayTimestampUpdater(
                new DateTime[]
                {
                    new DateTime(2023,8,18,13,15,0),
                    new DateTime(2023,9,3,14,25,15),
                    new DateTime(2023,7,30,15,32,08),
                    new DateTime(2023,8,10,14,52,43)
                }
            );
            Assert.That(updater.PastTimestamp, Is.EqualTo(new DateTime(2023, 8, 18, 13, 15, 0)));
        }
        [Test]
        public void Create_WithSomeArray_CurrentEqualsOneIndexOfElement()
        {
            FromArrayTimestampUpdater updater = new FromArrayTimestampUpdater(
                new DateTime[]
                {
                    new DateTime(2023,8,18,13,15,0),
                    new DateTime(2023,9,3,14,25,15),
                    new DateTime(2023,7,30,15,32,08),
                    new DateTime(2023,8,10,14,52,43)
                }
            );
            Assert.That(updater.CurrentTimestamp, Is.EqualTo(new DateTime(2023, 9, 3, 14, 25, 15)));
        }

        [Test]
        public void UpdateTimestamps_OneTimeUpdate_PastEqualsOneIndexOfElement()
        {
            FromArrayTimestampUpdater updater = new FromArrayTimestampUpdater(
                new DateTime[]
                {
                    new DateTime(2023,8,18,13,15,0),
                    new DateTime(2023,9,3,14,25,15),
                    new DateTime(2023,7,30,15,32,08),
                    new DateTime(2023,8,10,14,52,43)
                }

            );
            updater.UpdateTimestamps();
            Assert.That(updater.PastTimestamp, Is.EqualTo(new DateTime(2023, 9, 3, 14, 25, 15)));
        }
        [Test]
        public void UpdateTimestamps_OneTimeUpdate_CurrentEqualsTwoIndexOfElement()
        {
            FromArrayTimestampUpdater updater = new FromArrayTimestampUpdater(
                new DateTime[]
                {
                    new DateTime(2023,8,18,13,15,0),
                    new DateTime(2023,9,3,14,25,15),
                    new DateTime(2023,7,30,15,32,08),
                    new DateTime(2023,8,10,14,52,43)
                }

            );
            updater.UpdateTimestamps();
            Assert.That(updater.CurrentTimestamp, Is.EqualTo(new DateTime(2023, 7, 30, 15, 32, 08)));
        }
        [Test]
        public void UpdateTimestamps_ThreeTimestampsOneUpdate_CanUpdateReturnFalse()
        {
            FromArrayTimestampUpdater updater = new FromArrayTimestampUpdater(
                new DateTime[]
                {
                    new DateTime(2023,8,18,13,15,0),
                    new DateTime(2023,9,3,14,25,15),
                    new DateTime(2023,7,30,15,32,08)
                }

            );
            updater.UpdateTimestamps();
            Assert.False(updater.CanUpdate);
        }
        [Test]
        public void CanUpdate_ThreeTimestampsZeroUpdate_CanUpdateReturnTrue()
        {
            FromArrayTimestampUpdater updater = new FromArrayTimestampUpdater(
                new DateTime[]
                {
                    new DateTime(2023,8,18,13,15,0),
                    new DateTime(2023,9,3,14,25,15),
                    new DateTime(2023,7,30,15,32,08)
                }

            );
            
            Assert.True(updater.CanUpdate);
        }
        [Test]
        public void Create_CreateWithEmptyArray_ThrowsException()
        {
           
            var exception = Assert.Throws<TimestampUpdaterException>(
                () => new FromArrayTimestampUpdater(new DateTime[] {})
            );

            Assert.True(exception.Message.ToLower().Contains("the number of timestamps must be greater than 1"));
        }
        [Test]
        public void Create_CreateWithArrayOfOneElement_ThrowsException()
        {

            var exception = Assert.Throws<TimestampUpdaterException>(
                () => new FromArrayTimestampUpdater(new DateTime[1])
            );

            Assert.True(exception.Message.ToLower().Contains("the number of timestamps must be greater than 1"));
        }
        [Test]
        public void UpdateTimestamps_ThreeTimestampsTwoUpdates_ThrowsException()
        {
            FromArrayTimestampUpdater updater = new FromArrayTimestampUpdater(
                new DateTime[]
                {
                    new DateTime(2023,8,18,13,15,0),
                    new DateTime(2023,9,3,14,25,15),
                    new DateTime(2023,7,30,15,32,08)
                }

            );
            updater.UpdateTimestamps();

            var exception = Assert.Throws<TimestampUpdaterException>(
                () => updater.UpdateTimestamps()
            );

            Assert.True(exception.Message.ToLower().Contains("the array of timestamps is exceeded"));
        }
    }
}