<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:v="urn:schemas-microsoft-com:vml">
  <head>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8"/>
    <title>&nbsp;Bicycle Climbs of Seattle</title>
    
    <!-- #Include File="vml.inc" -->
    <script src="http://maps.google.com/maps?file=api&v=1&key=ABQIAAAAiZoAf08AX4ClzrJYaSSV9xTBqWC_ogAKx9eKDib7mMgLWMMJihSZyN8JqLbPnobFKQ6qF5XkAEZRDg" type="text/javascript"></script>

    <script type="text/javascript">
    //<![CDATA[

var map;
var polyline;
var polypts = [];

var PointList = [];  //use PointList.join()
var trackFlag = 0;

function fixPoint(point){
   xStr = (parseFloat(point.x)).toFixed(5);
   yStr = (parseFloat(point.y)).toFixed(5);
   ptStr = ' new GPoint(' + xStr + ', ' + yStr + ')';
   point = new GPoint(parseFloat(xStr),parseFloat(yStr));
   DrawnPath.Text += ptStr;
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
        var distravel = getDistance();

        map.clearOverlays();
        markCenter();
        var PolyLine = new GPolyline(polypts,"#ff00ff", 3);
        map.addOverlay(PolyLine);

  var mileS = ' cumulative miles tracked '+ (parseFloat(distravel)).toFixed(2) +'<br />';
       // document.getElementById("message").innerHTML = mileS + PointList.join();
 
    } else { 
        document.getElementById("message").innerHTML = ptStr;
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
      document.getElementById("message").innerHTML = mileS + PointList.join();   
  }
};
function stopTrack(){
  if (trackFlag == -1){
      trackFlag = 0;
      map.clearOverlays();
      markCenter();
      polypts = [];
      PointList = [];
      document.getElementById("message").innerHTML = ptStr;               
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


function onLoad() {
	      // Using XML and Asynchronous RPC ("AJAX") with Maps
	      //
	      // In this example, we download a static file ("data.xml") that contains a
	      // list of lat/lng coordinates in XML. When the download completes, we parse
	      // the XML and create a marker at each of those lat/lngs.
      
	document.getElementById("message").innerHTML = "<B>Test</b>";
     
	      // Center the map on Palo Alto
	map = new GMap(document.getElementById("map"));
	map.addControl(new GSmallMapControl());
	map.addControl(new GMapTypeControl());

	var panel = document.getElementById( 'panel' );
     
      // Download the data in data.xml and load it on the map. The format we
      // expect is:
      // <markers>
      //   <marker lat="37.441" lng="-122.141"/>
      //   <marker lat="37.322" lng="-121.213"/>
      // </markers>
	var request = GXmlHttp.create();
	request.open("GET", "climbPoly4a.xml", true);
	request.onreadystatechange = function() 
	{
		if (request.readyState == 4) 
		{
			var xmlDoc = request.responseXML;
          
			var center = xmlDoc.documentElement.getElementsByTagName("centerpoint"); 
 	    	var lng = parseFloat (center[0].getAttribute( "lng"));
 	    	var lat = parseFloat (center[0].getAttribute( "lat"));

        	map.centerAndZoom( new GPoint(-122.15149, 47.60616), 6 ); 
 
 		}
    };
   	request.send(null);

 // since we're running this file, we also want to go ahead
// and place the initial +

   markCenter();

// Now, back on the main HTML page, include the following action handler
// such that every time the map moves the + is moved as well:

   GEvent.addListener(map, "move", function() {
      replaceCenter();
   }); 
}


// create the marker icon for the + at the center
// you'll have to supply your own 17x17 images
var center_icon = new GIcon();
center_icon.image = "http://www.mailbag.com/users/joe_/xhair.gif";
center_icon.shadow = "http://www.mailbag.com/users/joe_/blank.gif";
center_icon.iconSize = new GSize(17, 17);
center_icon.shadowSize = new GSize(17, 17);
center_icon.iconAnchor = new GPoint(9, 8);
center_icon.infoWindowAnchor = new GPoint(9, 8);

// a function to delete the old + mark,
// then place a new one
function markCenter() {
      document.getElementById("message").innerHTML = "MarkCenter";               

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
    
        </head>
  <body onload="onLoad()" bgcolor="#CCFFFF">

		
	
	<table border="1" bordercolor="#CCFFFF"><tr>
		<td valign="top" bordercolor="#000000"><div id="map" style="width:600px; height:600px"></div></td>
	</tr></table>
	<div id="rightpanel">

    <div>
     &nbsp;<br />
    <input onclick="startTrack()" value="Start" type="button">
    <input onclick="undo()" value="Undo" type="button"> 
    <input onclick="stopTrack()" value="Stop" type="button"><br />&nbsp;<br />
    <input onclick="showInstruct()" value="Instruct" type="button"> <br />
    &nbsp;<br />
   </div>

    <div id="message">
    <script type="text/javascript">
     document.write(ptStr);
    </script>
    </div>

</div>


    <div id="message"></div>
  </body>
</html>
