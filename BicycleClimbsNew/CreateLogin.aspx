<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateLogin.aspx.cs" Inherits="CreateLogin" Title="Untitled Page" %>

<%@ MasterType VirtualPath="MasterPage.master" %>

<%@ Register TagPrefix="BC" Namespace="BicycleClimbsLibrary" Assembly="BicycleClimbsLibrary" %>
<%@ Register TagPrefix="BC" Src="~/MyHeaderControl.ascx" TagName="MyHeaderControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <table>
        <tr>
            <td>
                Username</td>
            <td>
                <asp:TextBox ID="p_username" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Password</td>
            <td>
                <asp:TextBox ID="p_password" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Password</td>
            <td>
                <asp:TextBox ID="p_password2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Email Address</td>
            <td>
                <asp:TextBox ID="p_email" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Default Region</td>
            <td>
                <asp:DropDownList ID="p_defaultRegion" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan=2>
                <div id="p_error" runat=server></div></td>
         </tr>
    </table>  
    <asp:Button ID="p_createButton" runat="server" Text="Create" />
</asp:Content>
