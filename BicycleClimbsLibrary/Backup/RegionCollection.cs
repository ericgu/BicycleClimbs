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
    public class RegionCollection: List<Region>
    {
		public void Load()
		{
			string query = @"select RegionId, RegionName, Latitude, Longitude, Zoom from regions ";

			DataReader reader = Database.ExecuteReader(query);

			while (reader.Read())
			{
				Region region = new Region(reader);
				Add(region);
			}
        }

		public static Region Load(int regionId)
		{
			string query = @"select RegionId, RegionName, Latitude, Longitude, Zoom from regions where RegionId=" + regionId.ToString();

			DataReader reader = Database.ExecuteReader(query);
			Region region = null;
			while (reader.Read())
			{
				region = new Region(reader);
				break;
			}

			return region;
		}

		public static void DeleteRegion(Region region)
		{
			int count = (int) Database.ExecuteScalar("Select count(*) from regions where RegionId=" + region.Id.ToString());

			if (count == 0)
			{
				return;
			}

			string query = String.Format("delete from regions where RegionId={0}", region.Id);
			Database.ExecuteNonQuery(query);
		}

		public static void SaveRegion(Region region)
		{
			string query = String.Format("Update regions set RegionName='{1}', Latitude={2}, Longitude={3}, Zoom={4} where RegionId={0}",
				region.Id, region.Name, region.Latitude, region.Longitude, region.Zoom);
			Database.ExecuteNonQuery(query);
		}

		public static int InsertRegion(Region region)
		{
			int count = (int) Database.ExecuteScalar(
						String.Format("select count(*) from regions where RegionName='{0}'", region.Name));
			if (count != 0)
			{
				return -1;
			}

			region.Id = 1;
			object objectId = Database.ExecuteScalar("select max(RegionId) from regions");
			if (objectId != null && objectId != System.DBNull.Value)
			{
				region.Id = (int) objectId;
			}

			int retry = 5;
			while (retry != 0)
			{
				region.Id++;

				try
				{
					string query = String.Format(
						@"insert into regions(RegionId, RegionName, Latitude, Longitude, Zoom) " +
						@"values ({0}, '{1}', {2}, {3})",
						region.Id, region.Name, region.Latitude, region.Longitude, region.Zoom);
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
			return region.Id;
		}

 	}
}
