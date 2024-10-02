<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageRegion.aspx.cs" Inherits="ManageRegion" Title="Untitled Page" %>
<%@ MasterType VirtualPath="MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div runat=server id="p_error"></div>
<asp:Repeater ID="p_repeater" runat="server">
<HeaderTemplate><table border=2 cellpadding=2>
<tr><th>Region</th><th>Longitude</th><th>Latitude</th><th>Zoom Level</th><th>Update</th><th>Delete</th></tr>
</HeaderTemplate>
<ItemTemplate>
<TR>
<td>
   <%# DataBinder.Eval(Container, "DataItem.Name") %>
</td>
<td align=center>
    <asp:TextBox ID="edit1" CommandName="Longitude" CommandArgument=<%# DataBinder.Eval(Container, "DataItem.Id") %> Text=<%# DataBinder.Eval(Container, "DataItem.Longitude") %> runat="server"></asp:TextBox>
</td>
<td align=center>
    <asp:TextBox ID="TextBox1" CommandName="Latitude" CommandArgument=<%# DataBinder.Eval(Container, "DataItem.Id") %> Text=<%# DataBinder.Eval(Container, "DataItem.Latitude") %> runat="server"></asp:TextBox>
</td>
<td align=center>
    <asp:TextBox ID="TextBox2" CommandName="Zoom" CommandArgument=<%# DataBinder.Eval(Container, "DataItem.Id") %> Text=<%# DataBinder.Eval(Container, "DataItem.Zoom") %> runat="server"></asp:TextBox>
</td>
<td align=center>
    <asp:Button ID="Button3" CommandName="Update" CommandArgument=<%# DataBinder.Eval(Container, "DataItem.Id") %> runat="server" Text="Update" />
</td>
<td align=center>
    <asp:Button ID="Button1" CommandName="Delete" CommandArgument=<%# DataBinder.Eval(Container, "DataItem.Id") %> runat="server" Text="Delete" />
</td>
</tr>
</ItemTemplate>
<FooterTemplate></table></FooterTemplate>
</asp:Repeater>

<h3>New Region</h3>
    <table>
        <tr>
            <td>
                Name</td>
            <td>
                <asp:TextBox ID="p_name" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Latitude</td>
            <td>
                <asp:TextBox ID="p_latitude" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Longitude</td>
            <td>
                <asp:TextBox ID="p_longitude" runat="server"></asp:TextBox>
            </td>
            <td>
                Zoom Level</td>
            <td>
                <asp:TextBox ID="p_zoom" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>  
    <asp:Button ID="p_createButton" runat="server" Text="Create" />
    
    <div id="p_error"></div>
</asp:Content>

