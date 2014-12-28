<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="RemoteStartWebApp._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        I am changing this header
        - nothing you can do about it, homey
    </h2>
    <p>
        <strong>Username:</strong><br />
        <asp:TextBox ID="txtUN" runat="server"></asp:TextBox>
    </p>
    <p>
        <strong>Password:</strong><br />
        <asp:TextBox ID="txtPW" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        <asp:Label ID="lblResponse" runat="server" Font-Bold="True" ForeColor="Red" 
            Text="Invalid credentials - try again mr hax" Visible="False"></asp:Label>
    </p>
    <p>
        <asp:Button ID="btnLogIn" runat="server" onclick="btnLogIn_Click" 
            Text="Log in" />
    </p>
    <p>
        &nbsp;</p>
    <p>
        <asp:Label ID="lblServerWrite" runat="server" 
            Text="This label gon' change when you press that there button"></asp:Label>
    </p>
    <p>
        &nbsp;</p>
    <p>
        <asp:Button ID="btnWriteBack" runat="server" onclick="btnWriteBack_Click" 
            Text="Change that label, son" />
    </p>
    <p>
        You can also find <a href="http://go.microsoft.com/fwlink/?LinkID=152368&amp;clcid=0x409"
            title="MSDN ASP.NET Docs">documentation on ASP.NET at MSDN</a>.
    </p>
</asp:Content>
