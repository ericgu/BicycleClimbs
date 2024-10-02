<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DrawRouteTest.aspx.cs" Inherits="DrawRouteTest" %>
<html>
  <body onload="onLoad()" bgcolor="#CCFFFF">
   
     <script type="text/javascript">
    //<![CDATA[
 
 function onLoad() {
    form1.DrawnRoute.value = '45,50,56,51,'; 
 }
 
    //]]>
    </script>
        
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="DrawnRoute" name="DrawnRoute" runat="server"></asp:TextBox>    
        <asp:Button ID="p_upload" runat="server" Text="Upload Route" />
    </div>
    </form>
     
    
</body>
    </html>


