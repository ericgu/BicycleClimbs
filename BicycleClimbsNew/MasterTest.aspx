<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MasterTest.aspx.cs" Inherits="MasterTest" Title="Untitled Page" %>
<%@ MasterType VirtualPath="MasterPage.master" %>

<%@ Register TagPrefix="BC" Namespace="BicycleClimbsLibrary" Assembly="BicycleClimbsLibrary" %>
<%@ Register TagPrefix="BC" Src="~/MyHeaderControl.ascx" TagName="MyHeaderControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <BC:MyTable id="detailTable" BorderWidth="0" GridLines="Both" runat="server" />

    <h3>Description</h3>
    <div id="p_description" runat=server></div>

    <br />
    <h3>Map</h3>
    <asp:Image ID="p_map" runat="server" />
    <br />
    <h3>Profile</h3>
    <asp:Image ID="p_gradient" runat="server" />
    <br />
    <a href="http://www.delorme.com"><img src="http://www.bicycleclimbs.com/climbData/sponsor_delorme.gif" /></a>Maps and gradients (c) 2004 DeLorme (www.delorme.com) Topo USA
    
</asp:Content>

