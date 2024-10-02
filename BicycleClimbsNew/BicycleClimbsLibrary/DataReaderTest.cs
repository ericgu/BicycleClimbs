using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace BicycleClimbsLibrary
{
    [TestFixture]
    class DataReaderTest
    {
        [Test]
        public void TestClimbConstructor()
        {
            DataReaderFake fake = new DataReaderFake();
            Climb climb = new Climb(fake);

            Assert.AreEqual(climb.Id, 1);
            Assert.AreEqual(climb.Name, "Fred");
            Assert.AreEqual(climb.Location, "Seattle");
			Assert.AreEqual(climb.LatitudeStart, 1.0);
			Assert.AreEqual(climb.LongitudeStart, 2.0);
            Assert.AreEqual(climb.Length, 33.0);
            Assert.AreEqual(climb.ElevationGain, 44.0);
            Assert.AreEqual(climb.Description, "Description");
            Assert.AreEqual(climb.Gradient, 55);
            Assert.AreEqual(climb.MaxGradient, 66);
            Assert.AreEqual(climb.Rating, "Rating");
            Assert.AreEqual(climb.Date, new DateTime(1964, 4, 2));
        }

    }
}
