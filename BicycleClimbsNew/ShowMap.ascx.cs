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

using System.Data.OleDb;
using BicycleClimbsLibrary;

public partial class ShowMap : System.Web.UI.UserControl
{
	enum RenderType
	{
		None,
		MapWithPath,
		ManyMarkers,
	};

	Climb m_climb;
	ClimbCollection m_climbCollection;
	RenderType m_renderType = RenderType.None;
	int m_regionId = 1;
	int m_zoom = 7;
    int m_userId = -1;
    int m_completedFilter = -1;

	protected override void Render(HtmlTextWriter output)
	{
		switch (m_renderType)
		{
			case RenderType.MapWithPath:
				output.Write(CreateMapWithPath());
				break;

			case RenderType.None:
				output.Write(CreateEmptyScript());
				break;

			case RenderType.ManyMarkers:
				output.Write(CreateManyMarkers2());
				break;
		}
	}

	public void LoadClimb(int climbID)
	{
		m_renderType = RenderType.MapWithPath;

		ClimbCollection climbs = new ClimbCollection();
		m_climb = climbs.LoadClimb(climbID);
	}

	public void LoadClimbs(int regionId, int userId, int completedFilter)
	{
		m_regionId = regionId;
		m_renderType = RenderType.ManyMarkers;
        m_userId = userId;
        m_completedFilter = completedFilter;

		m_climbCollection = new ClimbCollection();
		m_climbCollection.Populate("where regionid=" + regionId.ToString());

		Region region = RegionCollection.Load(regionId);
		m_zoom = region.Zoom;
	}

	protected string CreateEmptyScript()
	{
		string script = @"
   <script type=""text/javascript"">
    //<![CDATA[

		function LoadMap()
		{
		}

    //]]>
    </script>

		";

		return script;
    }

    #region MapWithPath

    protected string CreateMapWithPath()
	{
		string startPoint = String.Format("{0}, {1}", m_climb.LongitudeStart, m_climb.LatitudeStart);

		string pathString = "";
		PathCollection path = new PathCollection();
		path.FetchPathForClimb(m_climb.Id);
		if (path.Count != 0)
		{
			foreach (PathPoint point in path)
			{
				pathString += String.Format("\t\tpolypts.push(new GPoint({0}, {1}));\n",
											point.Longitude, point.Latitude);
			}
		}

		Location centerPoint = path.CenterPoint;
		if (centerPoint.Latitude == 0)
		{
			centerPoint.Latitude = m_climb.LatitudeStart;
			centerPoint.Longitude = m_climb.LongitudeStart;
		}
		string centerPointZoom = String.Format("new GLatLng({0}, {1}), {2}", centerPoint.Latitude, centerPoint.Longitude, 17 - m_climb.Zoom);

		string script = @"
   <script type=""text/javascript"">
    //<![CDATA[

		var polypts = [];
		var map;

		function CreateMarker(point)
		{
			var icon = new GIcon();
			icon.image = ""$$IconFilename$$"";
			icon.iconSize = new GSize(20, 34);
			icon.shadowSize = new GSize(37, 34);
			icon.shadow = ""http://www.google.com/mapfiles/shadow50.png"";
			icon.iconAnchor = new GPoint(9, 34);
			icon.infoWindowAnchor = new GPoint(9, 2);
			icon.infoShadowAnchor = new GPoint(18, 25);
  

			var marker = new GMarker(point, icon);
			return marker;
		}

		function LoadMap()
		{
			map = new GMap2(document.getElementById(""map""));
	        map.addControl(new GLargeMapControl()); 
	        map.addControl(new GMapTypeControl());
		    map.addControl(new GScaleControl());
			map.setCenter($$centerPointZoom$$);

			var point = new GPoint($$centerPoint$$);
			var marker = CreateMarker(point);

			GEvent.addListener(marker, ""click"", function() {
				marker.openInfoWindowHtml(""<h1>Testing</h1>""); 
			});
			map.addOverlay(marker);

			$$pathString$$

			var PolyLine = new GPolyline(polypts,""#ff00ff"", 6);
			map.addOverlay(PolyLine);


GEvent.addListener( map, ""wheelup"", function(p){ 
  if ( map.getZoomLevel() > 0 ) { 
    map.centerAndZoom( 
      p.scaleRelative( map.getCenterLatLng()), 
      map.getZoomLevel() - 1 
    ); 
  } 
}); 
  
GEvent.addListener( map, ""wheeldown"", function(p){ 
  if ( map.getZoomLevel() <= 16 ) 
    map.centerAndZoom( 
      p.scaleRelative( map.getCenterLatLng(), -1 ), 
      map.getZoomLevel() + 1 
    ); 
});
		
		}



    //]]>
    </script>

		";

		script = script.Replace("$$centerPoint$$", startPoint);
		script = script.Replace("$$centerPointZoom$$", centerPointZoom);
		script = script.Replace("$$IconFilename$$", m_climb.IconFilename);
		script = script.Replace("$$pathString$$", pathString);

		return script;
    }

    #endregion

     protected string CreateManyMarkers2()
    {
        if (m_climbCollection.Count == 0)
        {
            Region region = RegionCollection.Load(m_regionId);
            m_climb = new Climb();
            m_climb.LatitudeStart = region.Latitude;
            m_climb.LongitudeStart = region.Longitude;
        }
        else
        {
            m_climb = m_climbCollection[0];
        }

        string startPoint = String.Format("{0}, {1}", m_climb.LongitudeStart, m_climb.LatitudeStart);
        string centerPointZoom = String.Format("new GPoint({0}, {1}), {2}", m_climb.LongitudeStart, m_climb.LatitudeStart, 6);

        string markerString = "";
        string formatString = @"map.addOverlay(CreateMarker(new GPoint({0}, {1}), ""{2}"", ""<h1>{3}</h1>""));";

        string xmlUrl = String.Format("mapxml.aspx?RegionId={0}&UserId={1}&CompletedFilter={2}", m_regionId, m_userId, m_completedFilter);

        foreach (Climb climb in m_climbCollection)
        {
            markerString +=
                String.Format(formatString, climb.LongitudeStart, climb.LatitudeStart,
                                climb.IconFilename, climb.Name);
        }

        string script = @"
   <script type=""text/javascript"">
    //<![CDATA[

var markerArr;
var infoStorage;
var map;

// Creates a marker whose info window displays the given number
function createMarker(point, number, xmlMarker, info) {

  var iconTags = xmlMarker.getElementsByTagName(""icon"");
  var iconTag = iconTags[0];
  var filename = iconTag.getAttribute(""image"");

  var icon = new GIcon();
  icon.image = filename;
  icon.iconSize = new GSize(20, 34);
  icon.shadowSize = new GSize(37, 34);
  icon.shadow = ""http://www.google.com/mapfiles/shadow50.png"";
  icon.iconAnchor = new GPoint(9, 34);
  icon.infoWindowAnchor = new GPoint(9, 2);
  icon.infoShadowAnchor = new GPoint(18, 25);
  
  //var html = info

  var marker = new GMarker(point, icon);

  GEvent.addListener(marker, ""click"", function() {
    //marker.openInfoWindowXslt(info, ""climbs.xsl""); 
   // marker.openInfoWindowHtml(info.text); 
    //marker.openInfoWindowHtml('<h1>Hello</h1>'); 
    MakeInfoRequest(number - 1);


  });

  return marker;
}

var savedMarkerIndex = -1;

function MakeInfoRequest(markerIndex)
{
               if (window.XMLHttpRequest) { // Mozilla, Safari, ...
                    http_request = new XMLHttpRequest();
                } else if (window.ActiveXObject) { // IE
                    http_request = new ActiveXObject(""Microsoft.XMLHTTP"");
                }

                http_request.onreadystatechange = MarkerInfoBack; 
    savedMarkerIndex = markerIndex;

    http_request.open('POST', 'InfoWindow.aspx?RegionId=$$RegionId$$&MarkerId=' + markerIndex.toString(), true);
    http_request.send(null);
}

function MarkerInfoBack()
{
    if (http_request.readyState == 4)
    {
        markerArr[savedMarkerIndex].openInfoWindowHtml(http_request.responseText);
    } else 
    {
        // wait...
    } 
}
   
// Open the info box for the specified marker.
function popup( i )
{
    MakeInfoRequest(i);
//    alert('a');
//    markerArr[i].openInfoWindowHtml( infoStorage[i]);
//    markerArr[i].openInfoWindowXslt( infoStorage[i], ""climbs.xsl"" );
//    map.zoomTo(5);
}
 
function LoadMap() {
	      // Using XML and Asynchronous RPC (""AJAX"") with Maps
	      //
	      // In this example, we download a static file (""data.xml"") that contains a
	      // list of lat/lng coordinates in XML. When the download completes, we parse
	      // the XML and create a marker at each of those lat/lngs.
      
	      // Center the map on Palo Alto
	map = new GMap2(document.getElementById(""map""));
	map.addControl(new GLargeMapControl());
	map.addControl(new GMapTypeControl());
	map.addControl(new GScaleControl());
 
	var panel = document.getElementById( 'panel' );
     
      // Download the data in data.xml and load it on the map. The format we
      // expect is:
      // <markers>
      //   <marker lat=""37.441"" lng=""-122.141""/>
      //   <marker lat=""37.322"" lng=""-121.213""/>
      // </markers>
	var request = GXmlHttp.create();
		//request.open(""GET"", ""newxmlversion3.xml"", true);
	//request.open(""GET"", ""climbs.xml"", true);
	request.open(""GET"", ""$$xmlUrl$$"", true);
	request.onreadystatechange = function() 
	{
		if (request.readyState == 4) 
		{
			var xmlDoc = request.responseXML;
          
			var center = xmlDoc.documentElement.getElementsByTagName(""center""); 
 	    	var lng = parseFloat (center[0].getAttribute( ""lng""));
 	    	var lat = parseFloat (center[0].getAttribute( ""lat""));

        	map.setCenter( new GLatLng( lat, lng ), $$Zoom$$ ); 
          
        	var locations = xmlDoc.documentElement.getElementsByTagName(""location"");
        	infoStorage = new Array( locations.length );
			var panelText = """";
			var climbList = document.getElementById(""climbList"");

			markerArr = new Array(locations.length);

        	for (var i = 0; i < locations.length; i++)
        	{
				var points = locations[i].getElementsByTagName(""point"");
				lat = parseFloat(points[0].getAttribute(""lat""));
				lng = parseFloat(points[0].getAttribute(""lng""));
				var point = new GPoint(lng, lat);
			
				var info = locations[i].getElementsByTagName(""body"")[0];
				infoStorage[i] = info;
  
				var marker = createMarker(point, i + 1, locations[i], info);
			
				map.addOverlay(marker);
				markerArr[i] = marker;
    			
  				var titles = info.getElementsByTagName(""H1"");
  				
	    		var link = '<a href=""javascript:popup( ' + i + ' )"">';
  				  				
  				panelText += link;
  				
  				var title = titles[0];
  				//panelText += title.firstChild.nodeValue;
 				//panelText += ""</A><br>"";

				var oOption = document.createElement(""OPTION"");
				climbList.options.add(oOption);
				oOption.innerText = title.firstChild.nodeValue;
       		}
         	panel.innerHTML = panelText;

         //var page = xmlDoc.documentElement.getElementsByTagName(""page"");
          
         //var markers = xmlDoc.documentElement.getElementsByTagName(""marker"");
         //for (var i = 0; i < markers.length; i++) {
         // var point = new GPoint(parseFloat(markers[i].getAttribute(""lng"")),
         //                      parseFloat(markers[i].getAttribute(""lat"")));
   		 // var marker = createMarker(point, i + 1, markers[i]);
         //map.addOverlay(marker);
         //}
          
         	panelText += ""</table>"";
         	//panel.innerHTML = panelText;


        GEvent.addListener(map, ""click"", function(marker, point) { 
        
            if (point) { 
                document.getElementById(""output"").innerHTML = ""Elevation: Loading..."";

                if (window.XMLHttpRequest) { // Mozilla, Safari, ...
                    http_request = new XMLHttpRequest();
                } else if (window.ActiveXObject) { // IE
                    http_request = new ActiveXObject(""Microsoft.XMLHTTP"");
                }

                http_request.onreadystatechange = DataReady; 

                SendRequest(point);
            }
        } );


        }
    };
   	request.send(null);
}

function SendRequest(point)
{
    http_request.open('POST', 'FetchElevation.aspx?Latitude=' + point.y + '&Longitude=' + point.x, true);
    http_request.send(null);
}

var currentValue = -1
var baseValue = 0;

function DataReady()
{
    if (http_request.readyState == 4)
    {
        currentValue = parseInt(http_request.responseText);
        var delta = currentValue - baseValue
        document.getElementById(""output"").innerHTML = ""Elevation: "" + http_request.responseText + "" ("" + delta + "")"";

    } else 
    {
        // wait...
    } 
}

function setAsBase()
{
    baseValue = currentValue;
}




    //]]>
    </script>

		";

        script = script.Replace("$$centerPoint$$", startPoint);
        script = script.Replace("$$centerPointZoom$$", centerPointZoom);
        script = script.Replace("$$markerString$$", markerString);
        script = script.Replace("$$xmlUrl$$", xmlUrl);
        script = script.Replace("$$Zoom$$", m_zoom.ToString());
        script = script.Replace("$$RegionId$$", m_regionId.ToString());
        return script;
    }

}

#if fred
var polypts = [];

	var point = new GPoint(0, 0);
   polypts.push(point);


      var PolyLine = new GPolyline(polypts,"#ff00ff", 3);
        map.addOverlay(PolyLine);


    <div id="map" style="width: 500px; height: 400px"></div>
    <script type="text/javascript">
    //<![CDATA[
    
    var map = new GMap(document.getElementById("map"));
    map.addControl(new GLargeMapControl());
    map.centerAndZoom(new GPoint(-122.1419, 37.4419), 4);
    
 



   <script type="text/javascript">
    //<![CDATA[
     
   var xStr = '';
   var yStr = '';
   var ptStr = '';

      // puts the cumulative lng/lat center points in the message 
      // waits until the lng/lat changes to add
      // useful to determine route polyline points (copy & paste)
      // shows where new polyline will go

   var lastLng = -122.15149;
   var lastLat = 47.60616;

      var lastZoom = 6;   
      
      var ConfStr = '';   
      var trackFlag = 0;
      var d2r = Math.PI/180;


      var lastLatS = yStr;
      var lastLngS = xStr;

      var map;
      var centerPt;
      var centerIcon;
      
      init();
      
      function init()
      {
        map = new GMap(document.getElementById("map"));
        map.addControl(new GLargeMapControl()); 
        map.addControl(new GMapTypeControl());
        map.addControl(new GScaleControl());

        centerPt = new GPoint(lastLng,lastLat);
      
        fixPoint(centerPt);
        map.centerAndZoom(centerPt, lastZoom);

        GEvent.addListener(map, "moveend", echo);
        
        // create the marker icon for the + at the center
// you'll have to supply your own 17x17 images
center_icon = new GIcon();
center_icon.image = "http://www.mailbag.com/users/joe_/xhair.gif";
center_icon.shadow = "http://www.mailbag.com/users/joe_/blank.gif";
center_icon.iconSize = new GSize(17, 17);
center_icon.shadowSize = new GSize(17, 17);
center_icon.iconAnchor = new GPoint(9, 8);
center_icon.infoWindowAnchor = new GPoint(9, 8);

// since we're running this file, we also want to go ahead
// and place the initial +

   markCenter();

// Now, back on the main HTML page, include the following action handler
// such that every time the map moves the + is moved as well:

   GEvent.addListener(map, "move", function() {
      replaceCenter();
   }); 

      }

/////////////////////////////////////////////////////////////////////////////////////////
// var polypts = [new GPoint(lng0,lat0), ... ]

var polypts = [];
var PointList = [];  //use PointList.join()

function fixPoint(point){
   xStr = (parseFloat(point.x)).toFixed(5);
   yStr = (parseFloat(point.y)).toFixed(5);
   ptStr = '(' + xStr + ', ' + yStr + ')';
   point = new GPoint(parseFloat(xStr),parseFloat(yStr));
};

function addPoint(point){
   polypts.push(point);
   PointList.push(ptStr);
};

function delPoint(){
   polypts.pop();
   PointList.pop();
};
      
function echo(){
  replaceCenter();
  zoom = parseInt(map.getZoomLevel());
  if (lastZoom != zoom) {
    lastZoom = zoom;
    map.centerAtLatLng(centerPt);
  }else{
    
    centerPt = map.getCenterLatLng();
    fixPoint(centerPt);

   if (yStr != lastLatS || xStr != lastLngS) {

     if (trackFlag == -1 ){

        addPoint(centerPt);

        map.clearOverlays();
        markCenter();
        var PolyLine = new GPolyline(polypts,"#ff00ff", 3);
        map.addOverlay(PolyLine);

      document.getElementById("message").innerHTML = "Points: " + PointList.length;   
        document.getElementById("uploadText").value = PointList.join();
        
 
    } else { 
        document.getElementById("message").innerHTML = "Points: " + PointList.length;
    }; // end trackflag check

    lastLatS = yStr;
    lastLngS = xStr;
    }; // end of center check

  }; // end zoom check

}; // end function echo()
     
function startTrack(){
  if (trackFlag == 0){
      trackFlag = -1;
      polypts = [];
      PointList = [];
      addPoint(centerPt);
      
      document.getElementById("message").innerHTML = "Points: " + PointList.length;   
        document.getElementById("uploadText").value = PointList.join();
  }
};
function stopTrack(){
  if (trackFlag == -1){
      trackFlag = 0;
      map.clearOverlays();
      markCenter();
      polypts = [];
      PointList = [];
      document.getElementById("message").innerHTML = "Points: " + PointList.length;   
      document.getElementById("uploadText").value = PointList.join();
  }
};

function undo(){
  if (trackFlag == -1) {
      map.clearOverlays();
      markCenter();
      delPoint();
      centerPt = polypts[polypts.length-1];
      delPoint();
      fixPoint(centerPt);
      map.centerAtLatLng(centerPt); 
  }
};

function showInstruct(){
  alert(instruct);
};

// function getDistance of the polypts array

/////////////////////////////////////////////////////////////////////////////////////////



// a function to delete the old + mark,
// then place a new one
function markCenter() {
   map.centermarker = new GMarker(map.getCenterLatLng(), center_icon );
   map.addOverlay(map.centermarker);
}

function replaceCenter() {
   if (map.centermarker) {map.removeOverlay(map.centermarker)};
   map.centermarker = new GMarker(map.getCenterLatLng(), center_icon );
   map.addOverlay(map.centermarker);
}


    //]]>
    </script>
#endif