<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Wattage.aspx.cs" Inherits="Wattage" Title="Untitled Page" %>
<%@ MasterType VirtualPath="MasterPage.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<p>
Enter time to calculate wattage, or wattage to calculate time.</p>

<div id="p_error" runat="server"></div>
<table>
<tr>
<td>Rider Weight (pounds)</td>
<td><asp:TextBox ID="p_riderWeight" runat="server" Columns="3"></asp:TextBox></td>
</tr>
    
<tr>
<td>Bike Weight (pounds)</td>
<td><asp:TextBox ID="p_bikeWeight" runat="server" Columns="3"></asp:TextBox></td>
</tr>

<tr>
<td>Climb Time (MM:SS)</td>
<td><asp:TextBox ID="p_minutes" runat="server" Columns="3"></asp:TextBox>&nbsp;:&nbsp;<asp:TextBox ID="p_seconds" runat="server" Columns="3"></asp:TextBox></td>
</tr>

<tr>
<td>Wattage</td>
<td><asp:TextBox ID="p_wattage" runat="server" Columns="3"></asp:TextBox></td>
</tr>
</table>

<asp:Button ID="p_calculate" runat="server" Text="Calculate" />

<br />
<h3>Results</h3>
<table border="1"><tr><td>
<div ID="p_result" runat="server"></div>
</td></tr></table>

<h1>Notes</h1>

<p>
    There are three wattages calculated by this page - climbing wattage, rolling wattage,
    and aero wattage. Your time should be from the lowest to the highest point, which isn't necessarily the beginning and end of the climb. The best way to find this
    is with a Polar heart rate computer that also measures altitude - it's very easy
    to figure out the time after your ride.</p>

        <h3>Climbing Wattage</h3>
    <p>
        Climbing wattage is calculated by dividing the increase in potential energy by the time.
        First, the potential energy is given by PE (joules) = mgh, where m = mass in Kg,
        g (gravity) is 9.81 m/sec^2, and h is the height in meters. One watt is one joule
        per second, so wattage = PE / time in seconds.
    </p>
    <p>
        The calculated climbing wattage should be very close to the actual value. Since
        both the aero drag and rolling resistance are proportional to the velocity, steeper
        hills will produce the best wattage estimates.</p>

        <h3>Rolling Resistance Wattage</h3>
    <p>
        This wattage is calculated using a typical value for rolling resistance for clincher
        tires on a racing bike. If you are running tubular tires, the wattage here is lower.
        If you are running wider tires or a lower inflation pressure, this value is likely
        higher. If you are running knobbies, you should probably be on a different website.</p>

        <h3>Aerodynamic Drag Wattage</h3>
    <p>
        This section uses some published values for a 155-165lb rider with his hands on the hoods. If you have a smaller frontal area or are climbing on your drops (or,
        for the truly disturbed, on an aero bar), the wattage due to drag will be less.
        If you have a larger frontal area or you like to stand up a lot, the wattage to
        drag will be greater.
    </p>

        <h3>Frictional Losses</h3>
    <p>
        This page doesn't consider frictional losses. A well adjusted bike is somewhere
        around 98% efficient, so factor that in if you'd like.
    </p>
        
        <h3>Reference Pages</h3>
        <p>
            A couple of pages
        I used to develop this page:&nbsp;</p>
        http://www.wolfgang-menn.de/motion.htm
    <br />
        http://www.cervelo.com/tech/articles/article5.html
 </asp:Content>

