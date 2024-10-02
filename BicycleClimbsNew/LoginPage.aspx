<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage"  %>
<%@ MasterType VirtualPath="MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="p_error" runat=server></div>
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
    </table>
    <asp:HiddenField ID="p_referrer" runat="server" /> 
    <asp:Button ID="p_loginButton" runat="server" Text="Login" />
    <a href="CreateLogin.aspx">Create Login</a>
    </asp:Content>
    

