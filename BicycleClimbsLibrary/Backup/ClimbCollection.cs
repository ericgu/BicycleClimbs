using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data.OleDb;

namespace BicycleClimbsLibrary
{
	public class ClimbCollection: List<Climb>
	{
		public double minLatitude = double.MaxValue;
		public double maxLatitude = double.MinValue;
		public double minLongitude = double.MaxValue;
		public double maxLongitude = double.MinValue;

		static public void DeleteClimb(Climb climb)
		{
			string query = string.Format("delete from climbs where Id={0} and name='{1}'", climb.Id, climb.Name);
			Database.ExecuteNonQuery(query);
		}

		public Climb LoadClimb(int climbID)
		{
			Populate("where id=" + climbID.ToString());
            if (Count == 1)
            {
                return this[0];
            }
            else
            {
                return null;
            }
		}

		static public Climb LoadClimb(string name)
		{
			ClimbCollection climbs = new ClimbCollection();
			climbs.Populate(String.Format("where name='{0}'", name));
			if (climbs.Count == 0)
			{
				return null;
			}
			else
			{
				return climbs[0];
			}
		}

		readonly string columnList = "Id, Name, Location, LatitudeStart, LongitudeStart, Length, ElevationGain, Description, MaxGradient, Rating, Date, RegionId, UserId, Zoom";

        private void Load(string query)
        {
            DataReader reader = Database.ExecuteReader(query);

            while (reader.Read())
            {
                Climb climb = new Climb(reader);
                Add(climb);
            }
        }

		public void Populate(string queryEnd)
		{
			string query = @"select " + columnList + " from climbs " + queryEnd;
            Load(query);
		}

        public void LoadForRegion(int regionId)
        {
            Populate(" where regionid=" + regionId.ToString());
        }

        public void FilterCompleted(bool completed)
        {
            List<Climb> filteredList =
                this.FindAll(
                    delegate(Climb climb) { return climb.Completed == completed; }
                );

            Clear();
            AddRange(filteredList);
        }

		public void LoadCompletedData(int userId)
		{
			string query = @"select ClimbId from ClimbsCompleted where UserId = " + userId.ToString();
			DataReader reader = Database.ExecuteReader(query);

			while (reader.Read())
			{
				Climb search = new Climb();
				search.Id = (int) reader[0];
				int index = IndexOf(search);
				if (index != -1)
				{
					Climb climb = this[index];
					climb.Completed = true;
				}
			}
        }

		public void Populate(int climbId)
		{
			Populate("where id=" + climbId.ToString());
		}

		public void FigureMinMax()
		{
			if (minLatitude != Double.MaxValue)
			{
				return;
			}

			foreach (Climb climb in this)
			{
				if (climb.LatitudeStart == 0.0)
				{
					continue;
				}

				if (climb.LatitudeStart < minLatitude)
				{
					minLatitude = climb.LatitudeStart;
				}
				if (climb.LatitudeStart > maxLatitude)
				{
					maxLatitude = climb.LatitudeStart;
				}

				if (climb.LongitudeStart < minLongitude)
				{
					minLongitude = climb.LongitudeStart;
				}
				if (climb.LongitudeStart > maxLongitude)
				{
					maxLongitude = climb.LongitudeStart;
				}
			}
		}

		public XmlElement GetXml(double latitude, double longitude)
		{
			FigureMinMax();

			XmlDocument document = new XmlDocument();

			XmlElement page = document.CreateElement("page");
			document.AppendChild(page);

			XmlElement title = document.CreateElement("title");
			title.InnerText = "Climbs of the Eastside";
			page.AppendChild(title);

			XmlElement query = document.CreateElement("query");
			query.InnerText = "Why?";
			page.AppendChild(query);

			XmlElement center = document.CreateElement("center");
			if (Count != 0)
			{
				center.SetAttribute("lat", ((minLatitude + maxLatitude) / 2).ToString());
				center.SetAttribute("lng", ((minLongitude + maxLongitude) / 2).ToString());
			}
			else
			{
				center.SetAttribute("lat", latitude.ToString());
				center.SetAttribute("lng", longitude.ToString());
			}
			page.AppendChild(center);

			XmlElement span = document.CreateElement("span");
			span.SetAttribute("lat", (maxLatitude - minLatitude).ToString());
			span.SetAttribute("lng", (maxLongitude - minLongitude).ToString());
			page.AppendChild(span);

			XmlElement overlay = document.CreateElement("overlay");
			overlay.SetAttribute("panelStyle", "/mapfiles/geocodepanel.xls");
			page.AppendChild(overlay);

			foreach (Climb climb in this)
			{
				climb.XmlDocument = document;
				XmlElement location = climb.LocationXml;
				overlay.AppendChild(location);
			}

			return page;
		}

		public XmlElement GetClimbRss(string regionId)
		{
			string regionName = "";
			if (regionId == null)
			{
				Populate("");
			}
			else
			{
				Populate("where regionid=" + regionId);

				int regiondIdInt = Int32.Parse(regionId);
				Region region = RegionCollection.Load(regiondIdInt);
				if (region != null)
				{
					regionName = " for " + region.Name;
				}
			}

			XmlDocument document = new XmlDocument();

			XmlElement feed = document.CreateElement("rss");
			feed.SetAttribute("version", "2.0");
			feed.SetAttribute("xmlns:dc", "http://purl.org/dc/elements/1.1/");
			feed.SetAttribute("xmlns:slash", "http://purl.org/rss/1.0/modules/slash/");
			feed.SetAttribute("xmlns:wfw", "http://wellformedweb.org/CommentAPI/");

			document.AppendChild(feed);

			XmlElement channel = document.CreateElement("channel");
			feed.AppendChild(channel);

			XmlElement title = document.CreateElement("title");
			title.InnerText = "BicycleClimbs.com feed" + regionName;
			channel.AppendChild(title);

			XmlElement link = document.CreateElement("link");
			link.InnerText = "http://www.bicycleclimbs.com/climbsnew.aspx";
			if (regionId != null)
			{
				link.InnerText += "?RegionId=" + regionId;
			}
			channel.AppendChild(link);

			XmlElement description = document.CreateElement("description");
			channel.AppendChild(description);

			XmlElement language = document.CreateElement("language");
			language.InnerText = "en-US";
			channel.AppendChild(language);

			XmlElement generator = document.CreateElement("generator");
			generator.InnerText = "ClimbMaster 3000";
			channel.AppendChild(generator);

			foreach (Climb climb in this)
			{
				climb.XmlDocument = document;
				channel.AppendChild(climb.RssXml);
			}

			return feed;
		}

        public void UpdateCompleted(int userId, int id, bool completed)
        {
            string query = String.Format("Select count(*) from ClimbsCompleted where UserId={0} and ClimbId={1}", userId, id);
            int count = (int) Database.ExecuteScalar(query);

            if (count == 1)
            {
                if (!completed)
                {
                    query = String.Format("delete from ClimbsCompleted where UserId={0} and ClimbId={1}", userId, id);
                    Database.ExecuteNonQuery(query);
                }
            }
            else
            {
                if (completed)
                {
                    query = String.Format("insert into ClimbsCompleted(UserId, ClimbId) values({0}, {1})", userId, id);
                    Database.ExecuteNonQuery(query);
                }
            }
        }
    }
}

#if fred

<rss version="2.0" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:slash="http://purl.org/rss/1.0/modules/slash/" xmlns:wfw="http://wellformedweb.org/CommentAPI/">
  <channel>
    <title>Climbs of Seattle All Climbs</title>
    <link>http://www.bicycleclimbs.com/climbs.htm</link>
    <description />
    <language>en-US</language>
    <generator>ClimbMaster 3000</generator>
    <item>
      <title>100th</title>
      <link>http://www.bicycleclimbs.com/climbData/Climb_57.htm</link>
      <pubDate>Fri, 08 Jul 2005 19:40:47 GMT</pubDate>
      <guid isPermaLink="false">ericgunnerson climb57</guid>
    </item>
    <item>
      <title>112th, 520 to downtown</title>
      <link>http://www.bicycleclimbs.com/climbData/Climb_25.htm</link>
      <pubDate>Mon, 04 Jul 2005 16:00:21 GMT</pubDate>
      <guid isPermaLink="false">ericgunnerson climb25</guid>
    </item>



#endif


