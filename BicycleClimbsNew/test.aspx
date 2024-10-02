<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>My First ASP.NET Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    My first asp.net page
    <asp:Table id="table1" BorderWidth="1" GridLines="Both" runat="server" />

    </div>
    </form>
        <asp:Button ID="Button1" runat="server" Text="Button" />
</body>
</html>
