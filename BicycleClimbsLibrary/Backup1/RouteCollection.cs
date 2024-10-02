using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data.OleDb;

namespace BicycleClimbsLibrary
{
    public class RouteCollection : List<Route>
    {
        static public void DeleteRoute(Route route)
        {
            string query = string.Format("delete from route where Id={0} and name='{1}'", route.Id, route.Name);
            Database.ExecuteNonQuery(query);
        }

        public Route Loadroute(int routeID)
        {
            Populate("where id=" + routeID.ToString());
            return this[0];
        }

        readonly string columnList = "Id, Name, UserId, Closed";

        private void Load(string query)
        {
            DataReader reader = Database.ExecuteReader(query);

            while (reader.Read())
            {
                Route route = new Route(reader);
                Add(route);
            }
        }

        public void Populate(string queryEnd)
        {
            string query = @"select " + columnList + " from route " + queryEnd;
            Load(query);
        }

        public void LoadForRegion(int regionId)
        {
            Populate(" where regionid=" + regionId.ToString());
        }

  
        public void Populate(int routeId)
        {
            Populate("where id=" + routeId.ToString());
        }
    }
}



