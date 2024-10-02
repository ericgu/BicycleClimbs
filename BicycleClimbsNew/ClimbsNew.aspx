<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ClimbsNew.aspx.cs" Inherits="ClimbsNew" %>
<%@ MasterType VirtualPath="MasterPage.master" %>

<%@ Register TagPrefix="BC" Namespace="BicycleClimbsLibrary" Assembly="BicycleClimbsLibrary" %>
<%@ Register TagPrefix="BC" Src="~/MyHeaderControl.ascx" TagName="MyHeaderControl" %>
<%@ Register TagPrefix="BC" Src="~/Gmap.ascx" TagName="GMapIncluder" %>
<%@ Register TagPrefix="BC" Src="~/ShowMap.ascx" TagName="ShowMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <a href="ReleaseNotes.htm">Version 1.1</a>&nbsp; <a id="EnterClimb" onclick="EnterClimb();">
        <span style="color: #3300ff; text-decoration: underline">Enter Climb</span></a>  
    
	<table border="1" bordercolor="#ccffff"><tr>
		<td valign="top" bordercolor="#000000" style="height: 600px"><div id="map" style="width:600px; height:600px"></div></td>
		<td valign="top">
		<table border="0">
        <tr><td>Region: </td></tr>
        <tr><td><asp:DropDownList ID="p_regionList" runat="server" EnableViewState="true"></asp:DropDownList></td></tr>
        <tr><td>Climb: </td></tr>
 		<tr><td valign="top">
            <select id="climbList" onchange="javascript:popup(climbList.selectedIndex);">
            </select>
            <asp:DropDownList ID="p_completedFilter" runat="server" AutoPostBack="true"></asp:DropDownList>
		    <div id="panel" style="height:600 px">
			Loading...</div>
		</td>
		</tr>
		<tr><td>Click map to obtain elevation. Click save to set base elevation</td></tr>
		<tr><td id="output">Elevation: </td></tr>
		<tr><td>&nbsp;</td></tr>
		<tr><td><input id="" type="button" value="Save" onclick="setAsBase();" /></td></tr>
		</table>
		</td>
	</tr></table>

    <div id="output2"></div> 
    Default Zoom Level&nbsp;<asp:Button ID="p_zoomIn" runat="server" Text="+" />&nbsp;<asp:Button ID="p_zoomOut" runat="server" Text="-" />
    
    
       <script type="text/javascript">
    //<![CDATA[

		function EnterClimb()
		{
		    var link = document.getElementById("EnterClimb");
			
			var centerPt = map.getCenter();
		    var zoom = map.getZoom();
		    
    	    link.href = "EnterClimb.aspx?" + centerPt.toUrlValue() + "&" + zoom.toString();
    	    //alert(link.href);
		    
		}

    //]]>
    </script>
    </asp:Content>

