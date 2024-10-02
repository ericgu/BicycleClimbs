using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BicycleClimbsLibrary;

public partial class Wattage : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string climbId = Context.Request.QueryString["ClimbId"];

		if (climbId == null)
		{
			p_error.InnerText = "Please enter a climb - ?ClimbId=<num>";
			return;
		}
		int climbIdInt = Int32.Parse(climbId);

		ClimbCollection climbs = new ClimbCollection();
		climbs.Populate("where id=" + climbId);

		if (climbs.Count == 0)
		{
			return;
		}
		Climb climb = climbs[0];

        Master.Title = "BicycleClimbs.com Wattage Estimate for " + climb.Name;
        Master.PageHeading = "Wattage Estimate for " + climb.Name;

        if (!IsPostBack)
		{
		}
		else
		{
			double massRider;
			double massBike;

			if (!double.TryParse(p_riderWeight.Text, out massRider))
			{
				p_error.InnerText = "Incorrect rider weight";
				return;
			}

			if (!double.TryParse(p_bikeWeight.Text, out massBike))
			{
				p_error.InnerText = "Incorrect rider weight";
				return;
			}
			double mass = massBike / 2.204 + massRider / 2.204;

            int timeMinutes = 0;
            int timeSeconds = 0;

            if (p_wattage.Text != "")
            {
                int wattage;
                if (!Int32.TryParse(p_wattage.Text, out wattage))
                {
                    p_error.InnerText = "Incorrect wattage";
                    return;
                }
                timeSeconds = GetSecondsForWattage(climb.ElevationGain, climb.Length, mass, wattage);
            }
            else
            {

                if (!Int32.TryParse(p_minutes.Text, out timeMinutes))
                {
                    p_error.InnerText = "Incorrect minutes";
                    return;
                }

                if (!Int32.TryParse(p_seconds.Text, out timeSeconds))
                {
                    p_error.InnerText = "Incorrect seconds";
                    return;
                }
                timeSeconds += timeMinutes * 60;
            }

			double watts = ClimbingWatts(climb.ElevationGain, mass, timeSeconds);
            double rollingWattage = RollingWatts(climb.Length, timeSeconds, mass);
            double aeroWattage = AeroWatts(climb.Length, timeSeconds);
            double totalWattage = watts + rollingWattage + aeroWattage;
            double kJ = totalWattage * timeSeconds / 1000;

            timeMinutes = timeSeconds / 60;
            timeSeconds = timeSeconds - timeMinutes * 60;

            p_result.InnerHtml = String.Format("Time {0}:{1:D2} ({2})", timeMinutes, timeSeconds, timeMinutes * 60 + timeSeconds);

			p_result.InnerHtml += "<br><br>Wattage(Climb): " + ((int)watts).ToString() + " watts";
			p_result.InnerHtml += "<br>Wattage(Rolling): " + ((int)rollingWattage).ToString() + " watts";
			p_result.InnerHtml += "<br>Wattage(Aero Drag): " + ((int)aeroWattage).ToString() + " watts";
            p_result.InnerHtml += "<br>Wattage(Total): " + ((int) totalWattage).ToString() + " watts";

			p_result.InnerHtml += "<br><br>Estimated Calories Burned: " + ((int)kJ).ToString();
		}
	}

    private double ClimbingWatts(double elevationGain, double mass, int seconds)
    {
        return (mass * 9.81 * (elevationGain * 0.3048)) / seconds;

    }
    
    private double RollingWatts(double length, int seconds, double mass)
    {
        double velocity = (length * 0.3048) / seconds;
        return (0.003 * mass * 9.81 * velocity);
    }
    
    private double AeroWatts(double length, int seconds)
    {
        double velocity = (length * 0.3048) / seconds;
        return (0.5 * 0.35 * 1.225 * velocity * velocity * velocity);
    }

    private int GetSecondsForWattage(double elevationGain, double length, double mass, int wattage)
    {
        for (int timeSeconds = 1; timeSeconds < 36000; timeSeconds++)
        {
            double watts = ClimbingWatts(elevationGain, mass, timeSeconds);
            double rollingWattage = RollingWatts(length, timeSeconds, mass);
            double aeroWattage = AeroWatts(length, timeSeconds);
            double totalWattage = watts + rollingWattage + aeroWattage;

            if (totalWattage <= wattage)
            {
                return timeSeconds;
            }
        }

        return 0;
    }
}
