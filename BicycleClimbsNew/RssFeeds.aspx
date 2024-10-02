<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RssFeeds.aspx.cs" Inherits="RssFeeds" Title="Untitled Page" %>

<%@ MasterType VirtualPath="MasterPage.master" %>

<%@ Register TagPrefix="BC" Namespace="BicycleClimbsLibrary" Assembly="BicycleClimbsLibrary" %>
<%@ Register TagPrefix="BC" Src="~/MyHeaderControl.ascx" TagName="MyHeaderControl" %>
<%@ Register TagPrefix="BC" Src="~/Gmap.ascx" TagName="GMapIncluder" %>
<%@ Register TagPrefix="BC" Src="~/ShowMap.ascx" TagName="ShowMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<p>The following RSS feeds are available:</p>

<p><a href="http://www.bicycleclimbs.com/ClimbRss.aspx">Feed for all climbs</a></p> 

<p>Feeds by Region</p>
    
<div ID="p_regionFeedList" runat="server">
</div>    

</asp:Content>

