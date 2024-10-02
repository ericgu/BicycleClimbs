using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data.OleDb;

namespace BicycleClimbsLibrary
{
	public class UserCollection : List<User>
	{
		public void LoadUsers()
		{
			LoadUsers("");
		}

		public void LoadUsers(string query)
		{
			string fullQuery = @"select Id, Username, Password, RegionId, Email, CanCreateClimb, CanEditUsers from users " + query;

			DataReader reader = Database.ExecuteReader(fullQuery);

			while (reader.Read())
			{
				User user = new User(reader);
				Add(user);
			}
        }

		public static User LoadUser(int userId)
		{
			UserCollection users = new UserCollection();
			users.LoadUsers("where id=" + userId.ToString());

			if (users.Count == 1)
			{
				return users[0];
			}
			else
			{
				return null;
			}
		}

		public static User LoadUser(string username)
		{
			UserCollection users = new UserCollection();
			users.LoadUsers("where username='" + username + "'");

			if (users.Count == 1)
			{
				return users[0];
			}
			else
			{
				return null;
			}
		}

		public static void DeleteUser(User user)
		{
			int count = (int) Database.ExecuteScalar("Select count(*) from users where Id=" + user.Id.ToString());

			if (count == 0)
			{
				return;
			}

			string query = String.Format("delete from users where Id={0}", user.Id);
			Database.ExecuteNonQuery(query);
		}

		public static void SaveUser(User user)
		{
			string query = String.Format("Update users set Password='{1}', RegionId={2}, Email='{3}', CanCreateClimb={4}, CanEditUsers={5} where Id={0}",
				user.Id, user.Password, user.RegionId, user.Email, user.CanCreateClimb ? 1 : 0, user.CanEditUsers ? 1 : 0);
			Database.ExecuteNonQuery(query);
		}

		public static int InsertUser(User user)
		{
			int count = (int) Database.ExecuteScalar(
						String.Format("select count(*) from users where username='{0}'", user.Username));
			if (count != 0)
			{
				return -1;
			}

			user.Id = 1;
			object objectId = Database.ExecuteScalar("select max(Id) from users");
			if (objectId != null && objectId != System.DBNull.Value)
			{
				user.Id = (int) objectId;
			}

			int retry = 5;
			while (retry != 0)
			{
				user.Id++;

				try
				{
					string query = String.Format(
						@"insert into users(Id, Username, Password, RegionId, Email, CanCreateClimb, CanEditUsers) " +
						@"values ({0}, '{1}', '{2}', {3}, '{4}', {5}, {6})",
						user.Id, user.Username, user.Password, user.RegionId, user.Email, user.CanCreateClimb ? 1: 0, user.CanEditUsers ? 1 : 0);
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
			return user.Id;
		}
	}
}
