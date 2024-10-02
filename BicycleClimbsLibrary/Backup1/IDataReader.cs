using System;
using System.Collections.Generic;
using System.Text;

namespace BicycleClimbsLibrary
{
    public interface IDataReader
    {
        object this[int index]
        {
            get;
            set;
        }

		bool Read();

		int Count
		{
			get;
		}
    }

}
