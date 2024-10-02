#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Drawing;

namespace BicycleClimbsLibrary
{
    public class Climb
    {
        XmlDocument xmlDocument;

        int m_id;
        string m_name;
        string m_location;
        public double LatitudeStart;
        public double LongitudeStart;
        public double Length;
        double m_elevationGain;
        public string Description;
        double m_maxGradient;
        public string Rating;
        public DateTime Date;
		public int RegionId;
		public int UserId;
		public int Zoom;
		bool m_completed;

		public Climb()
		{
		}

        public Climb(IDataReader reader)
        {
			int i = 0;
            Id = (int) reader[i++];
            m_name = (string) reader[i++];
            m_location = (string) reader[i++];
			LatitudeStart = (double) reader[i++];
			LongitudeStart = (double) reader[i++];
			Length = (double) reader[i++];
			m_elevationGain = (double) reader[i++];
			Description = (string) reader[i++];
			m_maxGradient = (double) reader[i++];
			Rating = (string) reader[i++];
			Date = (DateTime) reader[i++];
			RegionId = (int) reader[i++];
			UserId = (int) reader[i++];
			Zoom = (int) reader[i++];
			if (reader.Count > 14)
			{
				Completed = reader[i++] == null ? false : true;
			}
		}

		public override bool Equals(object obj)
		{
			return Id == ((Climb) obj).Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public int Id
		{
			get { return m_id; }
			set { m_id = value; }
		}

		public string Name
		{
			get { return m_name; }
			set {m_name = value; }
		}

        public string Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

		public double MaxGradient
		{
			get { return m_maxGradient; }
			set	{ m_maxGradient = value; }
		}

		public double ElevationGain
		{
			get { return m_elevationGain; }
			set { m_elevationGain = value; }
		}

		public bool Completed
		{
			get { return m_completed; }
			set { m_completed = value; }
		}

		public double Gradient
		{
			get
			{
				return Math.Round(ElevationGain * 100 / Length, 1);
			}
		}

        public string LengthInMiles
        {
            get
            {
                return (Length / 5280.0).ToString("F1");
            }
        }

        public XmlDocument XmlDocument
        {
            get
            {
                return xmlDocument;
            }
            set
            {
                xmlDocument = value;
            }
        }

        public XmlElement PointXml
        {
            get
            {
                XmlElement point = xmlDocument.CreateElement("point");
				point.SetAttribute("lat", LatitudeStart.ToString());
				point.SetAttribute("lng", LongitudeStart.ToString());

                return point;
            }
        }

        public XmlElement IconXml
        {
            get
            {
                XmlElement icon = xmlDocument.CreateElement("icon");
                icon.SetAttribute("image", IconFilename);
                icon.SetAttribute("class", "local");

                return icon;
            }
        }

		public string IconFilename
		{
			get
			{
				return "http://www.bicycleclimbs.com/marker_" + Rating + ".png";
			}
		}

        public XmlElement TitleXml
        {
            get
            {
                XmlElement title = xmlDocument.CreateElement("H1");
                title.SetAttribute("xml:space", "preserve");
                title.InnerText = Name;
  
                return title;
            }
        }

        public XmlElement UrlXml
        {
            get
            {
                XmlElement url = xmlDocument.CreateElement("url");
				url.InnerText = "ClimbDetail.aspx?ClimbId=" + Id.ToString();

				return url;
            }
        }

        public XmlElement LengthXml
        {
            get
            {
			    XmlElement length = xmlDocument.CreateElement("length");
			    length.InnerText = LengthInMiles;
				return length;
            }
        }

        public XmlElement ElevationGainXml
        {
            get
            {
				XmlElement elevationGain = xmlDocument.CreateElement("elevationgain");
				elevationGain.InnerText = ElevationGain.ToString();
                return elevationGain;
            }
        }

       public XmlElement GradientXml
        {
            get
            {
	            XmlElement gradient = xmlDocument.CreateElement("gradient");
		        gradient.InnerText = Gradient.ToString();
				return gradient;
            }
        }

       public XmlElement MaxGradientXml
        {
            get
            {
				XmlElement maxGradient = xmlDocument.CreateElement("maxgradient");
				maxGradient.InnerText = MaxGradient.ToString();
				return maxGradient;
            }
        }

       public XmlElement ImageXml
        {
            get
            {
				XmlElement image = xmlDocument.CreateElement("image");
				image.InnerText = "http://www.bicycleclimbs.com/marker_" + Rating + ".png";
                return image;
            }
        }

		public string DetailUrl
		{
			get
			{
				return @"ClimbDetail.Aspx?ClimbID=" + Id.ToString();
			}
		}

		public string ToggleCompletedUrl
		{
			get { return @"ClimbLists.aspx?ClimbId=" + Id.ToString(); }
		}

        public XmlElement InfoXml
        {
            get
            {
                XmlElement info = xmlDocument.CreateElement("info");
                info.AppendChild(TitleXml);
				info.AppendChild(ImageXml);
				info.AppendChild(UrlXml);
				info.AppendChild(LengthXml);
				info.AppendChild(ElevationGainXml);
				info.AppendChild(GradientXml);
				info.AppendChild(MaxGradientXml);
				info.AppendChild(ElevationGainXml);
                return info;
            }
        }

		public XmlElement LocationXml
        {
            get
            {
				XmlElement location = xmlDocument.CreateElement("location");
				location.SetAttribute("infoStyle", "http://www.bicycleclimbs.com/climbs.xsl");
				location.SetAttribute("id", Id.ToString());
                location.AppendChild(PointXml);
                location.AppendChild(IconXml);

                XmlElement header = xmlDocument.CreateElement("h1");
                header.InnerText = "Popup";
                location.AppendChild(header);

                XmlElement info = xmlDocument.CreateElement("body");
                info.AppendChild(TitleXml);
                info.AppendChild(UrlXml);

#if fred
                info.AppendChild(ImageXml);

				info.AppendChild(LengthXml);
				info.AppendChild(ElevationGainXml);
				info.AppendChild(GradientXml);
				info.AppendChild(MaxGradientXml);
				info.AppendChild(ElevationGainXml);
#endif

                location.AppendChild(info);

                return location;
            }
        }

		public XmlElement RssXml
		{
			get
			{
				XmlElement item = xmlDocument.CreateElement("item");

				XmlElement title = xmlDocument.CreateElement("title");
				title.InnerText = Name;
				item.AppendChild(title);

				XmlElement link = xmlDocument.CreateElement("link");
				link.InnerText = "http://www.bicycleclimbs.com/ClimbDetail.aspx?ClimbId=" + Id.ToString();
				item.AppendChild(link);

				XmlElement pubData = xmlDocument.CreateElement("pubDate");
				pubData.InnerText = Date.ToString();
				item.AppendChild(pubData);

				XmlElement guid = xmlDocument.CreateElement("guid");
				guid.SetAttribute("isPermaLink", "true");
				guid.InnerText = Id.ToString();
				item.AppendChild(guid);

				return item;
			}
		}
#if fred
    <item>
      <title>100th</title>
      <link>http://www.bicycleclimbs.com/climbData/Climb_57.htm</link>
      <pubDate>Fri, 08 Jul 2005 19:40:47 GMT</pubDate>
      <guid isPermaLink="false">ericgunnerson climb57</guid>
    </item>
#endif


		public void PopulateDetailTable(MyTable table)
		{
			table.Add("Location", Location);
			table.Add("Latitude", LatitudeStart.ToString());
			table.Add("Longitude", LongitudeStart.ToString());
			table.Add("Length", Length.ToString());
			table.Add("Altitude Gain", ElevationGain.ToString() + " feet");
			table.Add("Gradient", Gradient.ToString() + " %");
			table.Add("Max Gradient", MaxGradient.ToString() + " %");
		}

		public int InsertIntoTable()
		{
			int id = (int) Database.ExecuteScalar("select max(id) from climbs");

			int retry = 5;
			while (retry != 0)
			{
				id++;
				
				try
				{
					string nameTemp = Name.Replace("'", "''");
					string descriptionTemp = Description.Replace("'", "''");
					string query = String.Format(
						@"insert into climbs(Id,  Name,     Location,   LatitudeStart, LongitudeStart, Length, ElevationGain, Description,     MaxGradient, Rating, RegionId, UserId, Zoom) " +
						           @"values ({0}, '{1}',    '{2}',      {3},           {4},            {5},    {6},           '{7}',           {8},        '{9}',   {10},     {11},   {12})",
						                     id,  nameTemp, m_location, LatitudeStart, LongitudeStart, Length, ElevationGain, descriptionTemp, MaxGradient, Rating, RegionId, UserId, Zoom);
					Database.ExecuteNonQuery(query);
					retry = 0;
				}
				catch (OleDbException)
				{
					retry--;
					if (retry == 0)
					{
						throw;
					}
				}
			}
			return id;
		}

		public void Update()
		{
			string nameTemp = Name.Replace("'", "''");
			string descriptionTemp = Description.Replace("'", "''");
			string query = String.Format(
				@"update climbs set Name='{0}', Location='{1}', LatitudeStart={2}, LongitudeStart={3}, Length={4}, ElevationGain={5}, Description='{6}', MaxGradient={7}, Rating='{8}', RegionId={9}, UserId={10}, Zoom={11} where id={12}",
									nameTemp,   m_location,     LatitudeStart,     LongitudeStart,     Length,     ElevationGain,     descriptionTemp,   MaxGradient,     Rating,       RegionId,      UserId,     Zoom,            Id);
			Database.ExecuteNonQuery(query);
		}
#if fred
		public void FetchElevationData()
		{
			BicycleClimbsLibrary.net.usgs.gisdata.TNM_Elevation_Service elevationService = new BicycleClimbsLibrary.net.usgs.gisdata.TNM_Elevation_Service();


			ElevationWebService elevationWebService = new ElevationWebService();

			double elevationStart = elevationWebService.FetchElevation(LatitudeStart, LongitudeStart, false);
		}
#endif

		public void FetchElevations(object state)
		{
			PathElevationCollection pathElevation = new PathElevationCollection();

			pathElevation.LoadPathForClimb(Id);

			pathElevation.UpdateElevation();
			pathElevation.UpdateClimbFromElevation(Id);
			System.Threading.Thread.Sleep(50000);
		}

		public Bitmap CreateGraph()
		{
			PathCollection paths = new PathCollection();
			paths.CreateClimbPathElevation(Id);

			PathElevationCollection pathElevation = new PathElevationCollection();

			pathElevation.LoadPathForClimb(Id);
			return pathElevation.CreateGraph(Name);
		}
	}
}
