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

public partial class RunTests : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		TestUserInit();
		TestUserCreate();

		TestClimbInit();
		TestClimbCreate();

		
		
		p_message.InnerHtml = "<B>All Tests Passed</b>";
	}

	void TestUserInit()
	{
		User user = UserCollection.LoadUser("TDUser");
		if (user != null)
		{
			UserCollection.DeleteUser(user);
		}
	}

	void Assert(bool condition, string message)
	{
		if (!condition)
		{
			throw new Exception(message);
		}
	}

	void TestUserCreate()
	{
		User user = new User();
		user.Username = "TDUser";
		user.Password = "Password";
		user.RegionId = -1;
		user.Email = "Email";
		user.CanCreateClimb = true;
		user.CanEditUsers = true;
		UserCollection.InsertUser(user);

		user = UserCollection.LoadUser("TDUser");

		Assert(user != null, "Could not load Saved user");

		Assert(user.Username == "TDUser", "Username");
		Assert(user.Password == "Password", "Password");
		Assert(user.RegionId == -1, "Region Id");
		Assert(user.Email == "Email", "Email");
		Assert(user.CanCreateClimb == true, "CanCreateClimb");
		Assert(user.CanEditUsers == true, "CanEditUsers");

		int newId = UserCollection.InsertUser(user);
		Assert(newId == -1, "No error when saving duplicate username");

		user.Password = "Password2";
		user.RegionId = -2;
		user.Email = "Email2";
		user.CanCreateClimb = false;
		user.CanEditUsers = false;
		UserCollection.SaveUser(user);

		user = UserCollection.LoadUser("TDUser");

		Assert(user.Password == "Password2", "Password");
		Assert(user.RegionId == -2, "Region Id");
		Assert(user.Email == "Email2", "Email");
		Assert(user.CanCreateClimb == false, "CanCreateClimb");
		Assert(user.CanEditUsers == false, "CanEditUsers");

	}

	void TestClimbInit()
	{
		Climb climb = ClimbCollection.LoadClimb("TDClimb");
		if (climb != null)
		{
			ClimbCollection.DeleteClimb(climb);
		}
	}

	void TestClimbCreate()
	{
		Climb climb = new Climb();

		climb.Name = "TDClimb";
		climb.Location = "Location";
		climb.LatitudeStart = 1.0;
		climb.LongitudeStart = 2.0;
		climb.Length = 3.0;
		climb.ElevationGain = 4.0;
		climb.Description = "Description";
		climb.MaxGradient = 5.0;
		climb.Rating = "Rating";
		climb.RegionId = -5;
		climb.UserId = -6;

		climb.InsertIntoTable();

		climb = ClimbCollection.LoadClimb("TDClimb");

		Assert(climb.Name == "TDClimb", "Name");
		Assert(climb.Location == "Location", "Location");
		Assert(climb.LatitudeStart == 1.0, "Latitude");
		Assert(climb.LongitudeStart == 2.0, "Longitude");
		Assert(climb.Length == 3.0, "Length");
		Assert(climb.ElevationGain == 4.0, "ElevationGain");
		Assert(climb.Description == "Description", "Description");
		Assert(climb.MaxGradient == 5.0, "Max Gradient");
		Assert(climb.Rating == "Rating", "Rating");
		Assert(climb.RegionId == -5, "Region Id");
		Assert(climb.UserId == -6, "User Id");

		climb.Location = "Location2";
		climb.LatitudeStart = 11.0;
		climb.LongitudeStart = 12.0;
		climb.Length = 13.0;
		climb.ElevationGain = 14.0;
		climb.Description = "Description2";
		climb.MaxGradient = 15.0;
		climb.Rating = "Rating2";
		climb.RegionId = -15;
		climb.UserId = -16;

		climb.Update();

		climb = ClimbCollection.LoadClimb("TDClimb");

		Assert(climb.Location == "Location2", "Location");
		Assert(climb.LatitudeStart == 11.0, "Latitude");
		Assert(climb.LongitudeStart == 12.0, "Longitude");
		Assert(climb.Length == 13.0, "Length");
		Assert(climb.ElevationGain == 14.0, "ElevationGain");
		Assert(climb.Description == "Description2", "Description");
		Assert(climb.MaxGradient == 15.0, "Max Gradient");
		Assert(climb.Rating == "Rating2", "Rating");
		Assert(climb.RegionId == -15, "Region Id");
		Assert(climb.UserId == -16, "User Id");
	}

}
