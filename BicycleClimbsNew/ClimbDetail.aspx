<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ClimbDetail.aspx.cs" Inherits="ClimbDetail" Title="Untitled Page" %>
<%@ MasterType VirtualPath="MasterPage.master" %>

<%@ Register TagPrefix="BC" Namespace="BicycleClimbsLibrary" Assembly="BicycleClimbsLibrary" %>
<%@ Register TagPrefix="BC" Src="~/MyHeaderControl.ascx" TagName="MyHeaderControl" %>
<%@ Register TagPrefix="BC" Src="~/Gmap.ascx" TagName="GMapIncluder" %>
<%@ Register TagPrefix="BC" Src="~/ShowMap.ascx" TagName="ShowMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id="p_editLink" runat="server"></div>
    Completed: <asp:CheckBox ID="p_completed" runat="server" AutoPostBack="true"/><br />
    <BC:MyTable id="detailTable" BorderWidth="0" GridLines="Both" runat="server" />
    <div id="p_wattage" runat="server"/>

    <h3>Description</h3>
    <div id="p_description" runat=server></div>
    <h3>Map</h3>
    <div id="map" style="width: 800px; height: 800px"></div><br />
    Default Zoom Level&nbsp;<asp:Button ID="p_zoomIn" runat="server" Text="+" />&nbsp;<asp:Button ID="p_zoomOut" runat="server" Text="-" />

    <div id="p_oldmap" runat="server" />
    <br />
    <h3>Profile</h3>
    <div id="p_gradient" runat="server" />
    
    <br />
    
    <div id="p_log" runat="server" />
</asp:Content>

