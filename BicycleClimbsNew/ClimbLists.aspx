<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ClimbLists.aspx.cs" Inherits="ClimbLists" %>
<%@ MasterType VirtualPath="MasterPage.master" %>

<%@ Register TagPrefix="BC" Namespace="BicycleClimbsLibrary" Assembly="BicycleClimbsLibrary" %>
<%@ Register TagPrefix="BC" Src="~/MyHeaderControl.ascx" TagName="MyHeaderControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:HyperLink ID="p_sorting" runat="server">Sorting</asp:HyperLink>
    <table>
        <tr><td>Region: </td></tr>
        <tr><td><asp:DropDownList ID="p_regionList" runat="server" EnableViewState="true"></asp:DropDownList></td></tr>
    </table>
    <asp:GridView ID="p_gridView" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Completed"> 
<HeaderStyle HorizontalAlign="Left"/> 
<ItemTemplate> 
<asp:CheckBox ID="Completed" Checked= '<%# Eval("Completed") %>' runat="server" /> 
<asp:HiddenField ID="ID" Value = '<%# Eval("Id") %>' runat="server" /> 
</ItemTemplate> 
</asp:TemplateField> 
</Columns>
    </asp:GridView>
<asp:Button ID="p_updateCompleted" runat="server" Text="Update Completed" />
</asp:Content>
