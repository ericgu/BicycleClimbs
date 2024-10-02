using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml;

// The web service claims to return XML, but the XML isn't well formed, so we just use a regex to pull out the result...

namespace BicycleClimbsLibrary
{
	public class ElevationWebService
	{
        BicycleClimbsLibrary.net.usgs.gisdata.Elevation_Service elevationService;

        public ElevationWebService()
        {
            elevationService = new BicycleClimbsLibrary.net.usgs.gisdata.Elevation_Service();
            elevationService.Proxy = new WebProxy();
        }


		public double FetchElevation(double latitude, double longitude, bool srtm)
		{
			int retry = 5;

            XmlNode output = null;
			while (retry-- > 0)
			{
				try
				{
					string dataSource = srtm ? "SRTM.US_1_ELEVATION" : "-1";
                    output = elevationService.getElevation(longitude.ToString(), latitude.ToString(), "FEET", dataSource, "");
					break;
				}
				catch (Exception)
				{
					if (retry == 0)
					{
						return -100.0;
					}
				}
			}

			Regex regex = new Regex("<Elevation>(?<Value>.*?)</Elevation>");
			Match match = regex.Match(output.InnerXml);
			if (!match.Success)
			{
				return double.NaN;
			}
			string elevationString = match.Groups["Value"].Value;
			return Double.Parse(elevationString);
		}
	}
}

