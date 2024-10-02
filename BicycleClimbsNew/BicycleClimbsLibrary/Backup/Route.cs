using System;
using System.Collections.Generic;
using System.Text;

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
    public class Route
    {
        public int Id;
        public int RegionId;
        public string Name;
        public int UserId;
        public bool Closed;

        public Route(IDataReader reader)
        {
            int i = 0;
            Id = (int) reader[i++];
            RegionId = (int) reader[i++];
            Name = (string) reader[i++];
            UserId = (int) reader[i++];
            Closed = (bool) reader[i++];
        }

        public int InsertIntoTable()
        {
            int id = (int) Database.ExecuteScalar("select max(id) from route");

            int retry = 5;
            while (retry != 0)
            {
                id++;

                try
                {
                    string nameTemp = Name.Replace("'", "''");
                    string query = String.Format(
                        @"insert into Route(Id,  Name,     UserId, Closed, RegionId) " +
                                   @"values ({0}, '{1}',    '{2}',    {3}, {4})",
                                             Id, nameTemp, UserId, Closed ? 1 : 0, RegionId);
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
            string query = String.Format(
                @"update climbs set Name='{0}', UserId={1}, Closed={2}, RegionId={3} where id={4}",
                                        nameTemp, UserId, Closed ? 1 : 0, RegionId, Id);
            Database.ExecuteNonQuery(query);
        }
    }
}
