<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Nonvecatingroomalert.aspx.cs" Inherits="Nonvecatingroomalert" Title="Non Vecating Room Alert Page" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentplaceholder3" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<br />
    <br />
    <asp:Panel ID="pnluserlink" runat="server" GroupingText="Quick links" Height="100%"
        Width="150px">
        <br />
        <asp:HyperLink ID="hlstaffmaster" runat="server" Height="32px" NavigateUrl="~/StaffMaster.aspx"
            Visible="False" Width="128px">Staff master</asp:HyperLink><asp:HyperLink ID="hlroommaster"
                runat="server" Height="32px" NavigateUrl="~/roommaster1.aspx" Visible="False"
                Width="128px">Room master</asp:HyperLink><asp:HyperLink ID="hldonormaster" runat="server"
                    Height="32px" NavigateUrl="~/DonorMaster.aspx" Visible="False" Width="128px">Donor master</asp:HyperLink><asp:HyperLink
                        ID="hlcomplaintmaster" runat="server" Height="32px" NavigateUrl="~/ComplaintMaster.aspx"
                        Visible="False">Complaint master</asp:HyperLink><asp:HyperLink ID="hlteammaster"
                            runat="server" Height="32px" NavigateUrl="~/TeamMaster.aspx" Visible="False"
                            Width="128px">Team master</asp:HyperLink><asp:HyperLink ID="hlinvmaster" runat="server"
                                Height="32px" NavigateUrl="~/inventorymaster.aspx" Visible="False">Inventory master</asp:HyperLink><asp:HyperLink
                                    ID="hlseasonmstr" runat="server" Height="32px" NavigateUrl="~/SeasonMaster.aspx"
                                    Visible="False" Width="130px">Season master</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                                        ID="hlsubmaster" runat="server" Height="32px" NavigateUrl="~/Submasters.aspx"
                                        Visible="False" Width="128px">Submasters</asp:HyperLink><asp:HyperLink ID="hlreservpol"
                                            runat="server" Height="32px" NavigateUrl="~/ReservationPolicy.aspx" Visible="False">reservation Policy</asp:HyperLink><asp:HyperLink
                                                ID="hlroolallocpol" runat="server" Height="32px" NavigateUrl="~/Room Allocation Policy.aspx"
                                                Visible="False" Width="144px">Room Alloc Policy</asp:HyperLink><asp:HyperLink ID="hlbillpolicy"
                                                    runat="server" Height="32px" NavigateUrl="~/Billing and Service charge policy.aspx"
                                                    Visible="False" Width="140px">Bill n service policy</asp:HyperLink><asp:HyperLink
                                                        ID="hlbankpolicy" runat="server" Height="32px" NavigateUrl="~/Cashier and Bank Remittance Policy.aspx"
                                                        Visible="False" Width="136px">Bank policy</asp:HyperLink><asp:HyperLink ID="hlroomallocation"
                                                            runat="server" Height="32px" NavigateUrl="~/roomallocation.aspx" Visible="False"
                                                            Width="128px">Room allocation</asp:HyperLink><asp:HyperLink ID="hlroomreservation"
                                                                runat="server" Height="32px" NavigateUrl="~/Room Reservation.aspx" Visible="False"
                                                                Width="136px">Room reservation</asp:HyperLink><asp:HyperLink ID="hlvacating" runat="server"
                                                                    Height="32px" NavigateUrl="~/vacating and billing.aspx" Visible="False" Width="128px">Room Vacating</asp:HyperLink><asp:HyperLink
                                                                        ID="hldonorpass" runat="server" Height="32px" NavigateUrl="~/donorpassfinal.aspx"
                                                                        Visible="False" Width="128px">Donor pass</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                                                                            ID="hlcmplntrgstr" runat="server" Height="32px" NavigateUrl="~/Complaint Register.aspx"
                                                                            Visible="False" Width="126px">Complaint register</asp:HyperLink><span style="font-size: 9pt">
                                                                            </span>
        <asp:HyperLink ID="hlchellanentry" runat="server" Height="32px" NavigateUrl="~/Chellan Entry.aspx"
            Visible="False" Width="128px">Chellan entry</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                ID="hlroomrsrce" runat="server" Height="32px" NavigateUrl="~/Room Resource Register.aspx"
                Visible="False" Width="128px">Room resource</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                    ID="hlusercrtn" runat="server" Height="32px" NavigateUrl="~/User Account Information.aspx"
                    Visible="False" Width="136px">User creation</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                        ID="hluserprvlge" runat="server" Height="32px" NavigateUrl="~/UserPrivilegeSettings.aspx"
                        Visible="False" Width="136px">User privilege</asp:HyperLink><asp:HyperLink ID="hlprinter"
                            runat="server" Height="32px" NavigateUrl="~/PlainPreprintedSettings.aspx" Visible="False"
                            Width="136px">Printer settings</asp:HyperLink><asp:HyperLink ID="hldayclose" runat="server"
                                Height="32px" NavigateUrl="~/DayClosing.aspx" Visible="False" Width="136px">Day close</asp:HyperLink><asp:HyperLink
                                    ID="hlroommgmnt" runat="server" Height="32px" NavigateUrl="~/Room Management.aspx"
                                    Visible="False" Width="143px">Room management</asp:HyperLink><asp:HyperLink ID="hlinvmngmnt"
                                        runat="server" Height="32px" NavigateUrl="~/Room Inventory Management.aspx" Visible="False"
                                        Width="139px">Inventory mangement</asp:HyperLink><asp:HyperLink ID="hlhkmagmnt" runat="server"
                                            Height="32px" NavigateUrl="~/HK management.aspx" Visible="False" Width="135px">HK management</asp:HyperLink></asp:Panel>

 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 721px; HEIGHT: 243px"><TBODY><TR><TD style="WIDTH: 3px"></TD><TD style="TEXT-ALIGN: center" colSpan=2><STRONG>Non Vacating Room alert</STRONG></TD></TR><TR><TD style="WIDTH: 3px; HEIGHT: 110px"></TD><TD vAlign=top><asp:Panel id="Panel1" runat="server" Width="100%" Height="100%" GroupingText="Alert"><TABLE><TBODY><TR><TD style="WIDTH: 54px; HEIGHT: 32px"></TD><TD style="WIDTH: 31px; HEIGHT: 32px"><asp:Label id="Label1" runat="server" Width="59px" __designer:wfdid="w10" Text="Date"></asp:Label></TD><TD style="WIDTH: 204px; HEIGHT: 32px"><asp:TextBox id="txtdate" runat="server" Width="125px" CausesValidation="True" __designer:wfdid="w11" AutoPostBack="True"></asp:TextBox></TD><TD style="WIDTH: 204px; HEIGHT: 32px"><asp:LinkButton id="LinkButton1" onclick="LinkButton1_Click" runat="server" Width="198px" __designer:wfdid="w5">Building Wise Report based on Proposed Check Out time</asp:LinkButton></TD><TD style="WIDTH: 61px" rowSpan=3></TD></TR><TR><TD style="WIDTH: 54px; HEIGHT: 32px"></TD><TD style="WIDTH: 31px; HEIGHT: 32px"><asp:Label id="lblsearchtime" runat="server" Width="55px" __designer:wfdid="w16" Text="Time"></asp:Label></TD><TD style="WIDTH: 204px; HEIGHT: 32px"><asp:TextBox id="txtsearchtime" runat="server" Width="122px" __designer:wfdid="w17"></asp:TextBox></TD><TD style="WIDTH: 204px; HEIGHT: 32px"><asp:LinkButton id="LinkButton2" onclick="LinkButton2_Click" runat="server" Width="200px" __designer:wfdid="w6">List of  rooms which are to be vecated  with in 1 hour</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 54px; HEIGHT: 34px"></TD><TD style="WIDTH: 31px; HEIGHT: 34px"></TD><TD style="HEIGHT: 34px"><asp:Button id="btnprint" runat="server" __designer:wfdid="w18" Text="Print" CssClass="btnStyle_small"></asp:Button> </TD><TD style="HEIGHT: 34px"><asp:LinkButton id="LinkButton5" onclick="LinkButton5_Click" runat="server" Visible="False" __designer:wfdid="w7">List Of OverStayed Rooms</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel> </TD><TD style="WIDTH: 100px; HEIGHT: 110px" vAlign=top>&nbsp;</TD></TR><TR><TD style="WIDTH: 3px; HEIGHT: 173px"></TD><TD style="HEIGHT: 173px" vAlign=top colSpan=2><asp:Panel id="Panel4" runat="server" Width="100%" Height="100%" GroupingText="Data grid showing the list of rooms which have not vacated after the expected vacating time" __designer:wfdid="w20" Visible="False"><asp:GridView id="GridView2" runat="server" Width="100%" Height="100%" __designer:wfdid="w21" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" ForeColor="#333333" CellPadding="4" GridLines="None">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel> <BR /><BR /><asp:Panel id="Panel3" runat="server" Width="100%" GroupingText="rooms due for vacating in the next one hour" __designer:wfdid="w29" Visible="False"><asp:GridView id="GridView1" runat="server" Width="100%" Height="100%" Visible="False" __designer:wfdid="w42" ForeColor="#0000FF" CellSpacing="1"><Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
</Columns>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:GridView></asp:Panel></TD></TR><TR><TD style="WIDTH: 3px; HEIGHT: 173px"></TD><TD style="HEIGHT: 173px" vAlign=top colSpan=2><asp:LinkButton id="LinkButton3" onclick="LinkButton3_Click" runat="server" __designer:wfdid="w2" Visible="False">LinkButton</asp:LinkButton> <asp:LinkButton id="LinkButton4" onclick="LinkButton4_Click" runat="server" __designer:wfdid="w3" Visible="False">LinkButton</asp:LinkButton><BR />l<BR __designer:dtid="1970333426909188" />&nbsp;&nbsp;<asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden" __designer:dtid="562958543355909" __designer:wfdid="w4"></asp:Button> <asp:Panel id="pnlMessage" runat="server" __designer:dtid="562958543355914" __designer:wfdid="w5" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel5" runat="server" Width="240px" BackColor="LightSteelBlue" __designer:dtid="562958543355915" __designer:wfdid="w6" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" __designer:dtid="562958543355916" __designer:wfdid="w8" Text="Tsunami ARMS - Confirmation" ForeColor="MediumBlue" Font-Bold="True"></asp:Label><BR /><asp:Label id="lblHead2" runat="server" __designer:wfdid="w8" Text="Tsunami ARMS - Warning" ForeColor="MediumBlue" Font-Bold="True"></asp:Label></asp:Panel> <asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="100%" __designer:dtid="562958543355918" __designer:wfdid="w9"><TABLE style="WIDTH: 237px" __designer:dtid="562958543355919"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3></TD></TR><TR __designer:dtid="562958543355920"><TD align=center colSpan=1 __designer:dtid="562958543355921"></TD><TD align=center colSpan=3 __designer:dtid="562958543355922"><asp:Label id="lblMsg" runat="server" Width="132px" ForeColor="Black" Text="Do you want to save?" __designer:dtid="562958543355923" __designer:wfdid="w10" Font-Size="Small"></asp:Label></TD></TR><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3></TD></TR><TR __designer:dtid="562958543355928"><TD __designer:dtid="562958543355929"></TD><TD align=center __designer:dtid="562958543355930">&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" Font-Bold="True" __designer:dtid="562958543355931" __designer:wfdid="w11" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" Font-Bold="True" __designer:dtid="562958543355932" __designer:wfdid="w12" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center __designer:dtid="562958543355933">&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="100%" __designer:dtid="562958543355938" __designer:wfdid="w13"><TABLE style="WIDTH: 237px" __designer:dtid="562958543355939"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3></TD></TR><TR __designer:dtid="562958543355940"><TD align=center colSpan=1 __designer:dtid="562958543355941"></TD><TD align=center colSpan=3 __designer:dtid="562958543355942"><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" __designer:dtid="562958543355943" __designer:wfdid="w14" Font-Size="Small"></asp:Label></TD></TR><TR __designer:dtid="562958543355948"><TD __designer:dtid="562958543355949"></TD><TD align=center __designer:dtid="562958543355950">&nbsp; &nbsp;&nbsp; <asp:Button id="btnOk" runat="server" Width="50px" CausesValidation="False" Text="OK" Font-Bold="True" __designer:dtid="562958543355951" __designer:wfdid="w15" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center __designer:dtid="562958543355952">&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> <cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" __designer:dtid="562958543355957" __designer:wfdid="w16" TargetControlID="btnHidden" RepositionMode="RepositionOnWindowResize" PopupControlID="pnlMessage">
                </cc1:ModalPopupExtender></asp:Panel></TD></TR></TBODY></TABLE>
</contenttemplate>
<Triggers>
<asp:PostBackTrigger ControlID="LinkButton1"></asp:PostBackTrigger>
<asp:PostBackTrigger ControlID="LinkButton2"></asp:PostBackTrigger>
<asp:PostBackTrigger ControlID="LinkButton5"></asp:PostBackTrigger>

</Triggers>
    </asp:UpdatePanel>
</asp:Content>

