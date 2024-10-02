<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditUser.aspx.cs" Inherits="EditUser" Title="Untitled Page" %>
<%@ MasterType VirtualPath="MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div runat=server id="p_error"></div>
<asp:Repeater ID="p_repeater" runat="server">
<HeaderTemplate><table border=2 cellpadding=2>
<tr><th>User</th><th>Can Create Climb</th><th>Can Edit Role</th></tr>
</HeaderTemplate>
<ItemTemplate>
<TR>
<td>
   <%# DataBinder.Eval(Container, "DataItem.Username") %>
</td>
<td align=center>
   <%# DataBinder.Eval(Container, "DataItem.CanCreateClimb") %>
    <asp:Button CommandName="CanCreateClimb" CommandArgument=<%# DataBinder.Eval(Container, "DataItem.Id") %> runat="server" Text="X" />
</td>
<td align=center>
   <%# DataBinder.Eval(Container, "DataItem.CanEditUsers") %>
    <asp:Button ID="Button1" CommandName="CanEditUsers" CommandArgument=<%# DataBinder.Eval(Container, "DataItem.Id") %> runat="server" Text="X" />
</td>
</tr>
</ItemTemplate>
<FooterTemplate></table></FooterTemplate>
</asp:Repeater>

</asp:Content>

