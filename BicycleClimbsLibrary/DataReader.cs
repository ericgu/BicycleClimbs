using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace BicycleClimbsLibrary
{
	public class DataReader : IDataReader
	{
		OleDbDataReader dataReader;

		public DataReader(OleDbDataReader dataReader)
		{
			this.dataReader = dataReader;
		}

		public object this[int index]
		{
			get
			{
				return dataReader[index];
			}
			set
			{
				throw new Exception();
			}
		}

		public int Count
		{
			get
			{
				return dataReader.FieldCount;
			}
		}

		public bool Read()
		{
			return dataReader.Read();
		}
	}
}
