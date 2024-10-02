<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DrawRoute.aspx.cs" Inherits="DrawRoute" %>

<%@ Register TagPrefix="BC" Namespace="BicycleClimbsLibrary" Assembly="BicycleClimbsLibrary" %>
<%@ Register TagPrefix="BC" Src="~/Gmap.ascx" TagName="GMapIncluder" %>

<%@ Register TagPrefix="BC" Src="~/MyHeaderControl.ascx" TagName="MyHeaderControl" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:v="urn:schemas-microsoft-com:vml">
  <head>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8"/>
    <title runat=server id="p_titleControl">DrawRoute</title>

    <!-- #Include File="vml.inc" -->

     <BC:GMapIncluder id="p_includer" runat="server"/>

 
 </head>


<body bgcolor="#CCFFFF">
    <BC:MyHeaderControl id="MyHeaderControl1" runat="server"/>
    <h2 id="PageHeadingControl" runat="server"></h2>
    <h1 id="p_title" runat=server>
        Draw Route for Climb</h1>
    <p>
        This page is used to draw the route for the climb. The system will tell you how
        many points are currently entered - if there are points entered, there is no need
        to do it again.
    </p>
    <p>
        There are instructions at the bottom. I find that it's easiest if I zoom all the
        way in and try to keep the lines mostly within the route.
    </p>
    <p>
        When you are done, enter upload, and you'll be taken to the detail page. It will
        take a while for the elevation data to be fetched, but when it's done, the climb
        data will show up.</p>
   
    <div id="map" style="width: 800px; height: 500px"></div>


    <div id="p_setClimbData" runat=server>
   </div>

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

      var lastZoom = 15;   
      
      var ConfStr = '';   
      var trackFlag = 0;
      var d2r = Math.PI/180;


      var lastLatS = yStr;
      var lastLngS = xStr;

      var map;
      var centerPt;
      var centerIcon;
      
      SetClimbData();
      init();
      
      function init()
      {
        map = new GMap2(document.getElementById("map"), {draggableCursor:"crosshair"});
        map.addControl(new GLargeMapControl()); 
        map.addControl(new GMapTypeControl());
        map.addControl(new GScaleControl());

        centerPt = new GLatLng(lastLat, lastLng);
      
        fixPoint(centerPt);
        map.setCenter(centerPt, lastZoom);

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
  zoom = parseInt(map.getZoom());
  if (lastZoom != zoom) {
    lastZoom = zoom;
    map.setCenter(centerPt);
  }else{
    
    centerPt = map.getCenter();
    fixPoint(centerPt);

   if (yStr != lastLatS || xStr != lastLngS) {

     if (trackFlag == -1 ){

        addPoint(centerPt);

        map.clearOverlays();
        markCenter();
        var PolyLine = new GPolyline(polypts,"#ff00ff", 3);
        map.addOverlay(PolyLine);

      document.getElementById("message").innerHTML = "Points: " + PointList.length;   
        document.getElementById("p_uploadText").value = PointList.join();
        
 
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
        document.getElementById("p_uploadText").value = PointList.join();
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
      document.getElementById("p_uploadText").value = PointList.join();
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
   map.centermarker = new GMarker(map.getCenter(), center_icon );
   map.addOverlay(map.centermarker);
}

function replaceCenter() {
   if (map.centermarker) {map.removeOverlay(map.centermarker)};
   map.centermarker = new GMarker(map.getCenter(), center_icon );
   map.addOverlay(map.centermarker);
}


    //]]>
    </script>

<div id="rightpanel">

    <div>
     &nbsp;<br />
    <form name="p_uploadForm" id="p_uploadForm" method="POST" runat=server>
   <input onclick="startTrack()" value="Start" type="button">
    <input onclick="undo()" value="Undo" type="button"> 
    <input onclick="stopTrack()" value="Clear" type="button">

      <asp:Button ID="p_upload" runat="server" Text="Upload" />
     <asp:TextBox ID="p_uploadText" runat="server" Text=""></asp:TextBox>
	</form>
   </div>


    <div id="message">
	
    <script type="text/javascript">
     document.write('');
    </script>
    </div>



</div>
Double click to center text, hit start to begin tracing the route, and then double-click along the route.<br />

Hit Upload when you are done.
</body></html>


 
