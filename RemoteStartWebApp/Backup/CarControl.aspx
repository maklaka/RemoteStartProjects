<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarControl.aspx.cs" Inherits="RemoteStartWebApp.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 337px">
    <form id="form1" runat="server">
    <div>
    
        <strong>Mark Jones Ford Focus 2012 Remote Start</strong><br />
        <br />
        <asp:Button ID="btnStartCar" runat="server" Height="44px" Text="Start the Car" 
            Width="288px" />
        <br />
        <strong>
        <asp:Panel ID="Panel1" runat="server" BackColor="#FFCC00" 
            HorizontalAlign="Center" Width="289px">
            <asp:Label ID="lblCommandStatus" runat="server" Text="No active command"></asp:Label>
        </asp:Panel>
        </strong>
        <br />
        <strong>Car Info:</strong></div>
    <asp:Panel ID="panCarInfo" runat="server">
        <asp:Table ID="Table1" runat="server" Height="96px" Width="294px">
            <asp:TableRow ID="ping" runat="server">
                <asp:TableCell runat="server">Most recent ping:</asp:TableCell>
                <asp:TableCell ID="cellPing" runat="server"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="cmd" runat="server">
                <asp:TableCell runat="server">Most recent cmd:</asp:TableCell>
                <asp:TableCell ID="cellCMD" runat="server"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="ipadd" runat="server">
                <asp:TableCell runat="server">IP address:</asp:TableCell>
                <asp:TableCell ID="cellIP" runat="server"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
    </asp:Panel>
    </form>
</body>
</html>
