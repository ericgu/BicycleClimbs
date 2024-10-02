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
	public class User
	{
		int m_id;
		string m_username;
		string m_password;
		int m_regionId;
		string m_email;
		bool m_canCreateClimb;
		bool m_canEditUsers;

		public User()
		{
		}

		public User(IDataReader reader)
		{
			m_id = (int) reader[0];
			m_username = (string) reader[1];
			m_password = (string) reader[2];
			m_regionId = (int) reader[3];
			m_email = (string) reader[4];
			m_canCreateClimb = (bool) reader[5];
			m_canEditUsers = (bool) reader[6];
		}

		public int Id
		{
			get { return m_id; }
			set { m_id = value; }
		}

		public string Username
		{
			get { return m_username; }
			set { m_username = value; }
		}

		public string Password
		{
			get { return m_password; }
			set { m_password = value; }
		}

		public int RegionId
		{
			get { return m_regionId; }
			set { m_regionId = value; }
		}

		public string Email
		{
			get { return m_email; }
			set { m_email = value; }
		}

		public bool CanCreateClimb
		{
			get
			{
				return m_canCreateClimb;
			}
			set
			{
				m_canCreateClimb = value;
			}
		}

		public bool CanEditUsers
		{
			get
			{
				return m_canEditUsers;
			}
			set
			{
				m_canEditUsers = value;
			}
		}

        public int ClimbsCompleted
        {
            get{
                int completed = (int) Database.ExecuteScalar("select count(*) from ClimbsCompleted where userid=" + m_id.ToString());
                return completed;
            }
        }

        public int ClimbsTotal
        {
            get
            {
                int total = (int) Database.ExecuteScalar("select count(*) from Climbs where regionid=" + m_regionId.ToString());
                return total;
            }
        }
    }
}

