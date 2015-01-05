<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="RemoteStartWebApp._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <p>
        <strong>Username:</strong><br />
        <asp:TextBox ID="txtUN" runat="server"></asp:TextBox>
        <br />
        <strong>Password:<br />
        <asp:TextBox ID="txtPW" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        <asp:Label ID="lblResponse" runat="server" Font-Bold="True" ForeColor="Red" 
            Text="Invalid credentials - try again mr hax" Visible="False"></asp:Label>
        <br />
        <asp:Button ID="btnLogIn" runat="server" onclick="btnLogIn_Click" 
            Text="Log in" Font-Size="Large" Height="45px" Width="160px" />
        </strong>
    </p>
    </asp:Content>
