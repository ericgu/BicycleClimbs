using System;
using System.Collections.Generic;
using System.Text;

namespace BicycleClimbsLibrary
{
	class Limits
	{
		public float Minimum = Single.MaxValue;
		public float Maximum = Single.MinValue;

		public void UpdateMinMax(double dValue)
		{
			float value = (float) dValue;

			if (value < Minimum)
			{
				Minimum = value;
			}

			if (value > Maximum)
			{
				Maximum = value;
			}
		}

		public float Middle
		{
			get
			{
				return (Minimum + Maximum) / 2;
			}
		}
	}
}
