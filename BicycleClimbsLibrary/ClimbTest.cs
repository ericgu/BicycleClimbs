using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Xml;

namespace BicycleClimbsLibrary
{
    [TestFixture]
    class ClimbTest
    {
        [Test]
        public void TestClimbXml()
        {
            DataReaderFake fake = new DataReaderFake();
            XmlDocument xmlDocument = new XmlDocument();
            Climb climb = new Climb(fake);
            climb.XmlDocument = xmlDocument;

            Assert.AreEqual(@"<point lat=""47.55432"" lng=""-122.07827"" />", climb.PointXml.OuterXml);

            string s = climb.IconXml.OuterXml;
            Assert.AreEqual(@"<icon image=""http://www.bicycleclimbs.com/marker_Rating.png"" class=""local"" />", climb.IconXml.OuterXml);

            Assert.AreEqual(@"<title xml:space=""preserve"">Fred</title>", climb.TitleXml.OuterXml);

			Assert.AreEqual(@"<image>http://www.bicycleclimbs.com/marker_Rating.png</image>", climb.ImageXml.OuterXml);
			Assert.AreEqual(@"<url>climbData/Climb_1.htm</url>", climb.UrlXml.OuterXml);
			Assert.AreEqual(@"<length>0.0</length>", climb.LengthXml.OuterXml);
			Assert.AreEqual(@"<elevationgain>44</elevationgain>", climb.ElevationGainXml.OuterXml);
			Assert.AreEqual(@"<gradient>133.3</gradient>", climb.GradientXml.OuterXml);
			Assert.AreEqual(@"<maxgradient>66</maxgradient>", climb.MaxGradientXml.OuterXml);
			Assert.AreEqual(@"<elevationgain>44</elevationgain>", climb.ElevationGainXml.OuterXml);

			ClimbCollection climbs = new ClimbCollection();
			climbs.Add(climb);

			string full = climbs.GetXml(0, 0).OuterXml;
			Console.WriteLine(full);

			MyTable table = new MyTable();
			climb.PopulateDetailTable(table);

			Assert.AreEqual("Location", table[0, 0]);
			Assert.AreEqual("Seattle", table[0, 1]);
			Assert.AreEqual("Latitude", table[1, 0]);
			Assert.AreEqual("47.55432", table[1, 1]);
			Assert.AreEqual("Longitude", table[2, 0]);
			Assert.AreEqual("-122.07827", table[2, 1]);
			Assert.AreEqual("Length", table[3, 0]);
			Assert.AreEqual("33", table[3, 1]);
			Assert.AreEqual("Altitude Gain", table[4, 0]);
			Assert.AreEqual("44 feet", table[4, 1]);
			Assert.AreEqual("Gradient", table[5, 0]);
			Assert.AreEqual("133.3 %", table[5, 1]);
			Assert.AreEqual("Max Gradient", table[6, 0]);
			Assert.AreEqual("66 %", table[6, 1]);
		}

		[Test]
		public void TestElevationWebService()
		{
			DataReaderFake fake = new DataReaderFake();
			Climb climb = new Climb(fake);

			//climb.FetchElevationData();
		}
    }
}
