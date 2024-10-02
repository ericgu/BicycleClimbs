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
var polypts;

function onLoad() {
	      // Using XML and Asynchronous RPC ("AJAX") with Maps
	      //
	      // In this example, we download a static file ("data.xml") that contains a
	      // list of lat/lng coordinates in XML. When the download completes, we parse
	      // the XML and create a marker at each of those lat/lngs.
      
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

        	map.centerAndZoom( new GPoint( lng, lat), 3 ); 
        	map.addOverlay(new GMarker(new GPoint( lng, lat)));

        	var pointElements = xmlDoc.documentElement.getElementsByTagName("point");

        	var polypts = [];
        	for (var i = 0; i < pointElements.length; i++)
        	{
        	 	var lng = parseFloat (pointElements[i].getAttribute( "lng"));
 	    		var lat = parseFloat (pointElements[i].getAttribute( "lat"));
 	    		
	        	polypts.push(new GPoint( lng, lat));
	        }
        	polyline = new GPolyline( polypts, "#ff0000", 3, 1.0);
        	map.addOverlay(polyline );
		}
    };
   	request.send(null);
}

    //]]>
    </script>
    
        </head>
  <body onload="onLoad()" bgcolor="#CCFFFF">

		
	
	<table border="1" bordercolor="#CCFFFF"><tr>
		<td valign="top" bordercolor="#000000"><div id="map" style="width:600px; height:600px"></div></td>
	</tr></table>
    <div id="message"></div>
  </body>
</html>
