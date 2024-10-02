using System;
using System.Collections.Generic;
using System.Text;

namespace BicycleClimbsLibrary
{
    public class DataReaderFake: IDataReader 
    {
        object[] values;

        public DataReaderFake()
        {
            values = new object[16];

            values[0] = 1;
            values[1] = "Fred";
            values[2] = "Seattle";
            values[3] = 47.55432;
            values[4] = -122.07827;
            values[5] = 33.0;
            values[6] = 44.0;
            values[7] = "Description";
            values[8] = "Author";
            values[9] = "ProfileSource";
            values[10] = 55.0;
            values[11] = 66.0;
            values[12] = "Rating";
            values[13] = new DateTime(1964, 4, 2);
			values[14] = 47.5417;
			values[15] = -122.09771;
        }

        public object this[int index]
        {
            get
            {
                return values[index];
            }
            set
            {
                values[index] = value;
            }
        }

		public bool Read()
		{
			return true;
		}

        public int Length
        {
            set
            {
                values = new object[value];
            }
        }

		public int Count
		{
			get
			{
				return values.Length;
			}
		}
    }
}
