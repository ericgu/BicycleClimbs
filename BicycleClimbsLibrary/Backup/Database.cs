using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace BicycleClimbsLibrary
{
	public class Database
	{
        static OleDbConnection connection = null;

		public static OleDbConnection Connection
		{
			get
			{
				if (connection == null)
				{
					string computerName = System.Environment.MachineName;
					string databaseServerName;

					if (computerName == "ERICGU01")
					{
						databaseServerName = "localhost";
					}
					else
					{
                        databaseServerName = "sql324.mysite4now.com";
					}
                    //databaseServerName = "sql324.mysite4now.com";
                    databaseServerName = "faster.arvixe.com";

					//string databaseServerName = System.Configuration.ConfigurationSettings.AppSettings["DatabaseServerName"];
					//string databaseServerName = System.Configuration.ConfigurationManager.AppSettings["DatabaseServerName"];

					string connect = String.Format("Provider=SQLOLEDB;Data Source={0};User Id=Climber;Password=climb1bike2;Initial Catalog=BicycleClimbs",
													databaseServerName);

					connection = new OleDbConnection(connect);
					connection.Open();
                    while (connection.State == ConnectionState.Connecting)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
				}

				return connection;
			}
		}

		public static void ExecuteNonQuery(string query)
		{
			OleDbCommand command = new OleDbCommand(query, Database.Connection);
			command.ExecuteNonQuery();
            command.Dispose();
		}

		public static object ExecuteScalar(string query)
		{
			OleDbCommand fetch = new OleDbCommand(query, Database.Connection);
            object result = fetch.ExecuteScalar();
			return result;
		}

		public static DataReader ExecuteReader(string query)
		{
			OleDbCommand command = new OleDbCommand(query, Database.Connection);

			return new DataReader(command.ExecuteReader());
		}

		public void AddRole(string roleName)
		{
			OleDbCommand command = new OleDbCommand("aspnet_Roles_CreateRole", Connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.AddWithValue("@ApplicationName", "BicycleClimbs");
			command.Parameters.AddWithValue("@RoleName", roleName);
			command.ExecuteNonQuery();
		}

		public void AddUserRole(string userName, string roleName)
		{
			OleDbCommand command = new OleDbCommand("aspnet_UsersInRoles_AddUsersToRoles", Connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.AddWithValue("@ApplicationName", "BicycleClimbs");
			command.Parameters.AddWithValue("@UserNames", userName);
			command.Parameters.AddWithValue("@RoleNames", roleName);
			command.Parameters.AddWithValue("@TimeZoneAdjustment", 0);
			command.ExecuteNonQuery();
		}
	}
}
