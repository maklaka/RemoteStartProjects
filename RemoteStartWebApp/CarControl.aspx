<%@ Page Title="CarControl" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="CarControl.aspx.cs" Inherits="RemoteStartWebApp.CarControl" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<asp:ScriptManager ID="ScriptManager1" runat="server"/>
<div>
    <asp:Timer ID="timUpdateMe" runat="server" Interval="3000" OnTick="timUpdateMe_Tick">
    </asp:Timer>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="timUpdateMe" EventName="Tick"/>
    </Triggers>
    <ContentTemplate>

        <asp:Button ID="btnStartTheCar" runat="server" Font-Size="X-Large" Height="57px" Text="Start The Car!"  Width="422px" OnClick="btnStartTheCar_Click" style="text-align: center; position: relative; top: 18px; left: 1px; z-index: 1;"  />
        <br />
        <br />
        <asp:Label ID="lblMessageStatus" runat="server" style="text-align:center" Text="No message sent..." Width="425px"/>
        <br />


        <asp:Label ID="lblSrvrStatus" runat="server" Font-Size="X-Large" Height ="37px" Text="Waiting For Server Connection" Width="425px" style="text-align:center" BackColor="#FFFFCC" ForeColor="Black"/>
        <br />
        <asp:Label ID="lblLastIHeard" runat="server" Text="Last I Heard from the RPI:"  style="text-align:center" Width="425px"/>
        <br />
        <asp:Label ID="lblCarStatus" runat="server" BackColor="#FFFFCC" Font-Size="X-Large" Height="37px" style="text-align:center" Text="Unknown car state"   Width="425px" ForeColor="Black" />
        <br />
        <br />
        <asp:Table ID="tabInfo" runat="server" HorizontalAlign="Center" style="margin-left: 0px; margin-top: 0px; text-align: center; position: relative; top: -1px; left: 33px;" Width="304px">
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" HorizontalAlign="Left">Last Message From RPI:</asp:TableCell>
                <asp:TableCell ID="LastRPIInfo" runat="server" HorizontalAlign="Right"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" HorizontalAlign="Left">IP Endpoint:</asp:TableCell>
                <asp:TableCell ID="IPEndPoint" runat="server" HorizontalAlign="Right"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <asp:Button ID="btnHalt" runat="server" OnClick="btnHalt_Click" Text="Debug Halt" />
        <br />
    </ContentTemplate>
</asp:UpdatePanel>





    
</asp:Content>


