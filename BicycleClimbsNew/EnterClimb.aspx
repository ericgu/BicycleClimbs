<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EnterClimb.aspx.cs" Inherits="EnterClimb" %>
<%@ MasterType VirtualPath="MasterPage.master" %>

<%@ Register TagPrefix="BC" Namespace="BicycleClimbsLibrary" Assembly="BicycleClimbsLibrary" %>
<%@ Register TagPrefix="BC" Src="~/MyHeaderControl.ascx" TagName="MyHeaderControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Enter new climb</h1>
<p>
    Start entering your climb on this All you have to do is set the region and give
    the climb a name. You can set a max gradient if you have a good description, and
    you can also set a rating. Ratings are intended to be relative to your area, but
    generally an "A" class climb has to some like a 300' climb at 8% average or more.
</p>
    <p>
        You should leave the latitude, longitude, length, and elevation gain set to 0.</p>
    <p>
        After you've entered the data, enter your password, and you'll be taken to the page
        where you draw the route on the map.</p>
   <div id="p_error" runat=server></div>
   <table style="width: 848px">
   <tr>
        <td>Region: </td>
        <td style="width: 643px"><asp:DropDownList ID="p_regionList" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
        <td>Climb Name: </td>
        <td style="width: 643px"><asp:TextBox ID="p_name" runat="server" Columns="40"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                ControlToValidate="p_name"  
                ErrorMessage="Value required"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td id="p_locationText" runat="server">Location: </td>
        <td >
            <asp:TextBox ID="p_location" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td id="p_latitudeText" runat="server">Starting Latitude: </td>
        <td ><asp:TextBox ID="p_latitudeStart" runat="server"></asp:TextBox>
        <asp:RangeValidator ID="RangeValidator1" runat="server" 
             ControlToValidate="p_latitudeStart" MinimumValue="0" MaximumValue="90" Type=Double 
             ErrorMessage="The latitude must be between 0 and 90"></asp:RangeValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                ControlToValidate="p_latitudeStart"  
                ErrorMessage="Value required"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td id="p_longitudeText" runat="server">Starting Longitude: </td>
        <td ><asp:TextBox ID="p_longitudeStart" runat="server"></asp:TextBox>
        <asp:RangeValidator ID="RangeValidator3" runat="server" 
             ControlToValidate="p_longitudeStart" MinimumValue="-180" MaximumValue="180" Type=Double 
             ErrorMessage="The longitude must be between -180 and 180"></asp:RangeValidator>
       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                ControlToValidate="p_longitudeStart"  
                ErrorMessage="Value required"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td id="p_lengthText" runat="server">Length: (feet) </td>
        <td ><asp:TextBox ID="p_length" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                ControlToValidate="p_length"  
                ErrorMessage="Value required"></asp:RequiredFieldValidator>
        
        </td>
    </tr>
    <tr>
        <td id="p_elevationGainText" runat="server">Elevation Gain: </td>
        <td ><asp:TextBox ID="p_elevationGain" runat="server"></asp:TextBox></td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                ControlToValidate="p_elevationGain"  
                ErrorMessage="Value required"></asp:RequiredFieldValidator>
    </tr>
    <tr>
        <td>Max Gradient: </td>
        <td style="width: 643px"><asp:TextBox ID="p_maxGradient" runat="server"></asp:TextBox>
        <asp:RangeValidator ID="RangeValidator5" runat="server" 
             ControlToValidate="p_maxGradient" MinimumValue="0" MaximumValue="50" Type=Double 
             ErrorMessage="The gradient must be between 0 and 50"></asp:RangeValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                ControlToValidate="p_maxGradient"  
                ErrorMessage="Value required"></asp:RequiredFieldValidator>
        </td>
    </tr>
        <tr>
        <td>Rating: </td>
        <td style="width: 643px"><asp:DropDownList ID="p_rating" runat="server"></asp:DropDownList>A = Hardest
         </td>
    </tr>
    <tr>
        <td style="height: 202px">Description: </td>
        <td style="width: 643px; height: 202px"><asp:TextBox ID="p_description" runat="server" Height="208px" Width="656px" Wrap=true TextMode=MultiLine></asp:TextBox></td>
    </tr>
    <tr>
        <td>Password: </td>
        <td style="width: 643px"><asp:TextBox ID="p_password" runat="server"></asp:TextBox>
        <div id="p_passwordError" runat=server></div>
        </td>
    </tr>
    
</table>

<asp:button ID="p_submit" runat="server" text="Save" /></asp:Content>


