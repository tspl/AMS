<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Room Management.aspx.cs" Inherits="Roommanagement" Title="Tsunami ARMS - Room Management" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentplaceholder3" Runat="Server">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        <p>
        <br />
        This form is used for vacate , block, release the rooms.<br />
        <br />
            When &nbsp;select block, details of vacant room details are displayed in grid.<br />
        <br />
            When select Release, blocked room details are &nbsp;displayed in grid.
        <br />
        <br />
            When select
        force release, the details of room, that are occupied after completion of vecating
        time.<br />
        <br />
            When select nonoccupied reserved room&nbsp; such type of rooms are shown in grid.<br />
        <br />
        Different types of reports are available.</p>
    </asp:Panel>
    <br />

</asp:Content>


<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

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
                        Visible="False" Width="125px">Complaint master</asp:HyperLink><asp:HyperLink ID="hlteammaster"
                            runat="server" Height="32px" NavigateUrl="~/TeamMaster.aspx" Visible="False"
                            Width="128px">Team master</asp:HyperLink><asp:HyperLink ID="hlinvmaster" runat="server"
                                Height="32px" NavigateUrl="~/inventorymaster.aspx" Visible="False" Width="127px">Inventory master</asp:HyperLink><asp:HyperLink
                                    ID="hlseasonmstr" runat="server" Height="32px" NavigateUrl="~/SeasonMaster.aspx"
                                    Visible="False" Width="126px">Season master</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                                        ID="hlsubmaster" runat="server" Height="32px" NavigateUrl="~/Submasters.aspx"
                                        Visible="False" Width="128px">Submasters</asp:HyperLink><asp:HyperLink ID="hlreservpol"
                                            runat="server" Height="32px" NavigateUrl="~/ReservationPolicy.aspx" Visible="False" Width="127px">reservation Policy</asp:HyperLink><asp:HyperLink
                                                ID="hlroolallocpol" runat="server" Height="32px" NavigateUrl="~/Room Allocation Policy.aspx"
                                                Visible="False" Width="127px">Room Alloc Policy</asp:HyperLink><asp:HyperLink ID="hlbillpolicy"
                                                    runat="server" Height="32px" NavigateUrl="~/Billing and Service charge policy.aspx"
                                                    Visible="False" Width="128px">Bill n service policy</asp:HyperLink><asp:HyperLink
                                                        ID="hlbankpolicy" runat="server" Height="32px" NavigateUrl="~/Cashier and Bank Remittance Policy.aspx"
                                                        Visible="False" Width="127px">Bank policy</asp:HyperLink><asp:HyperLink ID="hlroomallocation"
                                                            runat="server" Height="32px" NavigateUrl="~/roomallocation.aspx" Visible="False"
                                                            Width="128px">Room allocation</asp:HyperLink><asp:HyperLink ID="hlroomreservation"
                                                                runat="server" Height="32px" NavigateUrl="~/Room Reservation.aspx" Visible="False"
                                                                Width="126px">Room reservation</asp:HyperLink><asp:HyperLink ID="hlvacating" runat="server"
                                                                    Height="32px" NavigateUrl="~/vacating and billing.aspx" Visible="False" Width="128px">Room Vacating</asp:HyperLink><asp:HyperLink
                                                                        ID="hldonorpass" runat="server" Height="32px" NavigateUrl="~/donorpassfinal.aspx"
                                                                        Visible="False" Width="128px">Donor pass</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                                                                            ID="hlcmplntrgstr" runat="server" Height="32px" NavigateUrl="~/Complaint Register.aspx"
                                                                            Visible="False" Width="129px">Complaint register</asp:HyperLink><span style="font-size: 9pt">
                                                                            </span>
        <asp:HyperLink ID="hlchellanentry" runat="server" Height="32px" NavigateUrl="~/Chellan Entry.aspx"
            Visible="False" Width="128px">Chellan entry</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                ID="hlroomrsrce" runat="server" Height="32px" NavigateUrl="~/Room Resource Register.aspx"
                Visible="False" Width="128px">Room resource</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                    ID="hlusercrtn" runat="server" Height="32px" NavigateUrl="~/User Account Information.aspx"
                    Visible="False" Width="128px">User creation</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                        ID="hluserprvlge" runat="server" Height="32px" NavigateUrl="~/UserPrivilegeSettings.aspx"
                        Visible="False" Width="128px">User privilege</asp:HyperLink><asp:HyperLink ID="hlprinter"
                            runat="server" Height="32px" NavigateUrl="~/PlainPreprintedSettings.aspx" Visible="False"
                            Width="127px">Printer settings</asp:HyperLink><asp:HyperLink ID="hldayclose" runat="server"
                                Height="32px" NavigateUrl="~/DayClosing.aspx" Visible="False" Width="126px">Day close</asp:HyperLink><asp:HyperLink
                                    ID="hlroommgmnt" runat="server" Height="32px" NavigateUrl="~/Room Management.aspx"
                                    Visible="False" Width="125px">Room management</asp:HyperLink><asp:HyperLink ID="hlinvmngmnt"
                                        runat="server" Height="32px" NavigateUrl="~/Room Inventory Management.aspx" Visible="False"
                                        Width="128px">Inventory mangement</asp:HyperLink><asp:HyperLink ID="hlhkmagmnt" runat="server"
                                            Height="32px" NavigateUrl="~/HK management.aspx" Visible="False" Width="128px">HK management</asp:HyperLink><br />
        <asp:HyperLink ID="hlnonvacatingalert" runat="server" Height="31px" NavigateUrl="~/Nonvecatingroomalert.aspx"
            Width="127px">Non vacating alert</asp:HyperLink></asp:Panel>



</asp:Content>--%>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<TABLE><TBODY><TR><TD style="WIDTH: 750px; HEIGHT: 21px" align=center colSpan=1><STRONG><SPAN style="FONT-WEIGHT: bold; FONT-SIZE: 14pt; COLOR: #003399">Room&nbsp;&nbsp; Manangement</SPAN></STRONG></TD></TR><TR><TD><asp:Panel id="Panel1" runat="server" Width="100%" GroupingText="Room Management"><TABLE id="TABLE2" onclick="return TABLE1_onclick()"><TBODY><TR><TD><asp:Label id="Lblcriteria" runat="server" Width="97px" Text="Operations"></asp:Label> </TD>
    <TD><asp:DropDownList id="cmbSelectCriteria" runat="server" Width="146px" OnSelectedIndexChanged="cmbSelectCriteria_SelectedIndexChanged1" AutoPostBack="True"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem>Room Blocking</asp:ListItem>
<asp:ListItem>Release Blocked Rooms</asp:ListItem>
<asp:ListItem>Release Overstayed Rooms</asp:ListItem>
<asp:ListItem>Release Unoccupied Reserved Rooms</asp:ListItem>
<asp:ListItem>Release Reserved Rooms</asp:ListItem>
<asp:ListItem>TDB Reservation</asp:ListItem>
</asp:DropDownList></TD><TD colSpan=1></TD><TD><asp:Label id="Lblfromdate" runat="server" Width="70px" Text="From  Date"></asp:Label> </TD><TD><asp:TextBox id="txtFromDate" tabIndex=4 runat="server" Width="69px" AutoPostBack="True" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox> </TD><TD><asp:Label id="lblfromtime" runat="server" Width="67px" Text="From Time"></asp:Label> </TD><TD><asp:TextBox id="txtFromTime" tabIndex=5 runat="server" Width="55px" AutoPostBack="True" OnTextChanged="txtFromTime_TextChanged"></asp:TextBox> </TD></TR><TR><TD>
    Category</TD><TD>
        <asp:DropDownList ID="ddl_catgry" runat="server" AutoPostBack="True" 
            OnSelectedIndexChanged="cmbSelectCriteria_SelectedIndexChanged1" Width="146px">
            <asp:ListItem>-- Select --</asp:ListItem>
            <asp:ListItem Value="1">Department staff</asp:ListItem>
            <asp:ListItem Value="2">Other blocking</asp:ListItem>
        </asp:DropDownList>
    </TD><TD colSpan=1>&nbsp;</TD><TD>
    <asp:Label ID="lbltodate" runat="server" Text="To Date" Width="64px"></asp:Label>
    </TD><TD>
        <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="True" 
            OnTextChanged="txtToDate_TextChanged" tabIndex="6" Width="69px"></asp:TextBox>
    </TD><TD>
        <asp:Label ID="lbltotime" runat="server" Text="To Time" Width="65px"></asp:Label>
    </TD><TD>
        <asp:TextBox ID="txtToTime" runat="server" tabIndex="7" Width="55px"></asp:TextBox>
    </TD></TR><TR><TD><asp:Label id="Lb1selectbuilding" runat="server" 
            Text="Building Name" Width="96px"></asp:Label> </TD><TD>
            <asp:DropDownList id="cmbSelectBuilding" runat="server" Width="146px" 
                OnSelectedIndexChanged="cmbSelectBuilding_SelectedIndexChanged1" 
                DataValueField="build_id" DataTextField="buildingname" AutoPostBack="True"></asp:DropDownList></TD>
        <TD colSpan=1></TD><TD>&nbsp;</TD><TD>&nbsp;</TD><TD>&nbsp;</TD><TD>&nbsp;</TD></TR><TR><TD>
    <asp:Label id="lblroom" runat="server" Text="Room No"></asp:Label> </TD>
    <TD style="HEIGHT: 23px" valign="top"><asp:DropDownList id="cmbSelectRoom" 
            runat="server" Width="146px" 
            OnSelectedIndexChanged="cmbSelectRoom_SelectedIndexChanged2" 
            DataValueField="roomno" DataTextField="roomno"></asp:DropDownList> </TD>
    <TD colSpan=1 valign="top"></TD><TD style="HEIGHT: 23px"></TD>
    <TD style="HEIGHT: 23px"></TD><TD style="HEIGHT: 23px"></TD>
    <TD style="HEIGHT: 23px"></TD></TR><TR><TD><asp:Label id="lblreason" 
        runat="server" Text="Reason"></asp:Label></TD><TD>
        <asp:DropDownList ID="cmbReason" runat="server" DataTextField="reason" 
            DataValueField="reason_id" 
            OnSelectedIndexChanged="cmbReason_SelectedIndexChanged" Width="146px">
        </asp:DropDownList>
        &nbsp; </TD><TD colSpan=1>
        <asp:LinkButton ID="lnkReason" runat="server" __designer:wfdid="w4" 
            CausesValidation="False" ForeColor="Blue" onclick="lnkReason_Click">New</asp:LinkButton>
    </TD><TD colSpan=1></TD><TD></TD><TD>&nbsp;</TD><TD></TD></TR><TR><TD>
    <asp:Label id="lblOfficerName" runat="server" Text="Officer Name" 
        __designer:wfdid="w5" Visible="False"></asp:Label></TD><TD>
        <asp:TextBox id="txtOfficer" runat="server" Width="141px" __designer:wfdid="w7" 
            Visible="False"></asp:TextBox></TD><TD colSpan=1></TD><TD colSpan=1></TD><TD></TD><TD></TD><TD></TD></TR><TR><TD>
    <asp:Label ID="lblSwami" runat="server" __designer:wfdid="w6" 
        Text="Inmates Name" Visible="False"></asp:Label>
    </TD><TD>
        <asp:TextBox ID="txtSwami" runat="server" __designer:wfdid="w8" Visible="False" 
            Width="141px"></asp:TextBox>
    </TD><TD colSpan=1></TD><TD colspan="1"></TD><TD></TD><TD></TD><TD></TD></TR><TR>
    <TD></TD>
    <td>
        &nbsp;<asp:Button ID="btnStatus" runat="server" __designer:wfdid="w6" 
            CausesValidation="False" CssClass="btnStyle_large" onclick="btnStatus_Click" 
            Text="Room Status" />
    </td>
    <td colspan="1">
    </td>
    <td>
        <asp:Button ID="btnSave" runat="server" CssClass="btnStyle_small" 
            onclick="btnSave_Click" tabIndex="8" ValidationGroup="VGroup" />
    </td>
    <td>
        <asp:Button ID="btnClear" runat="server" CausesValidation="False" 
            CssClass="btnStyle_small" onclick="btnClear_Click" tabIndex="9" Text="Clear" />
    </td>
    <td>
        <asp:Button ID="btnReport" runat="server" CausesValidation="False" 
            CssClass="btnStyle_small" onclick="btnReport_Click" tabIndex="10" Text="Report" 
            ValidationGroup="vreport" />
    </td>
    <td>
        <asp:Button ID="btnRoomAllocation" runat="server" __designer:wfdid="w19" 
            CausesValidation="False" CssClass="btnStyle_large" 
            onclick="btnRoomAllocation_Click" Text="Room Allocation" />
    </td>
    </TR>
    <tr>
        <td colspan="7">
            <table>
                <tbody>
                    <tr>
                        <td colspan="6">
                            <asp:Panel ID="pnlRoomStatusReport" runat="server" __designer:wfdid="w46" 
                                GroupingText="Room Status " Visible="False" Width="40%">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBuilding" runat="server" __designer:wfdid="w51" 
                                                    Text="Building Name" Visible="False" Width="87px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbBuilding" runat="server" __designer:wfdid="w52" 
                                                    AutoPostBack="True" DataTextField="buildingname" DataValueField="build_id" 
                                                    OnSelectedIndexChanged="cmbBuilding_SelectedIndexChanged" Visible="False" 
                                                    Width="115px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRoomNo" runat="server" __designer:wfdid="w53" Text="Room No" 
                                                    Visible="False" Width="61px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbRoomNo" runat="server" __designer:wfdid="w54" 
                                                    DataTextField="roomno" DataValueField="room_id" Visible="False" Width="113px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTo" runat="server" __designer:wfdid="w55" Text="To Date" 
                                                    Visible="False" Width="60px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTo" runat="server" __designer:wfdid="w56" 
                                                    AutoPostBack="True" OnTextChanged="txtTo_TextChanged" Visible="False" 
                                                    Width="98px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <asp:LinkButton ID="lnkStatusHistory" runat="server" __designer:wfdid="w57" 
                                                    CausesValidation="False" Font-Bold="True" onclick="lnkStatusHistory_Click" 
                                                    Visible="False" Width="133px">Room History Status</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:Label ID="lblRoomDetails" runat="server" __designer:wfdid="w21" 
                Text="Label" Visible="False"></asp:Label>
        </td>
    </tr>
    </TBODY></TABLE></asp:Panel> </TD></TR><TR><TD colSpan=1><asp:Panel id="Panel3" runat="server" Width="100%" GroupingText="Report"><TABLE><TBODY><TR>
        <TD style="TEXT-ALIGN: right" colSpan=4><asp:Panel id="pnlVacantAtAyTime" runat="server" Width="100%" GroupingText="Vacant At any Time " __designer:wfdid="w3"><TABLE><TBODY><TR><TD style="HEIGHT: 30px"><asp:Label id="lblVDate" runat="server" Width="42px" Text="Date" __designer:wfdid="w9"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 30px"><asp:TextBox id="txtVDate" runat="server" Width="113px" __designer:wfdid="w10"></asp:TextBox></TD><TD style="HEIGHT: 30px"><asp:Label id="lblVTime" runat="server" Text="Time" Visible="False" __designer:wfdid="w11"></asp:Label></TD><TD style="HEIGHT: 30px"><asp:TextBox id="txtVTime" runat="server" Width="113px" Visible="False" __designer:wfdid="w12"></asp:TextBox></TD><TD style="HEIGHT: 30px"><asp:LinkButton id="lnkVacantAnyTime" onclick="lnkVacantAnyTime_Click" runat="server" Width="181px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w13">Vacant Room At any Time</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD>&nbsp;<asp:Label id="lbldate" runat="server" Width="36px" Text="Date"></asp:Label></TD>
        <TD style="WIDTH: 172px; TEXT-ALIGN: right"><asp:TextBox id="txtDate" runat="server" OnTextChanged="txtDate_TextChanged"></asp:TextBox></TD><TD colSpan=1>&nbsp;<asp:Label id="lbltime" runat="server" Width="35px" Text="Time"></asp:Label>&nbsp; <asp:TextBox id="txtTime" runat="server"></asp:TextBox></TD><TD colSpan=1></TD></TR><TR>
        <TD colSpan=2><asp:LinkButton id="lnkOccupy" onclick="lnkOccupy_Click" runat="server" Width="153px" Font-Bold="True" __designer:wfdid="w51" ValidationGroup="VReport">Occupying Room report</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:LinkButton id="lnkExcOccupy" onclick="lnkExcOccupy_Click" runat="server" Font-Bold="True" __designer:wfdid="w3" ValidationGroup="VReport">Excel</asp:LinkButton></TD><TD colSpan=1><asp:LinkButton id="lnkVacant" onclick="lnkVacant_Click" runat="server" Width="138px" Font-Bold="True" ValidationGroup="VReport">Vacant  Room report</asp:LinkButton>&nbsp;&nbsp; &nbsp; <asp:LinkButton id="lnkExcVacant" onclick="lnkExcVacant_Click" runat="server" Font-Bold="True" __designer:wfdid="w4" ValidationGroup="VReport">Excel</asp:LinkButton></TD><TD colSpan=1><asp:LinkButton id="lnkBlocked" onclick="lnkBlocked_Click" runat="server" Width="146px" Font-Bold="True" ValidationGroup="Bblock">Blocked Room report</asp:LinkButton>&nbsp;&nbsp;&nbsp; <asp:LinkButton id="lnkEXBlock" onclick="lnkEXBlock_Click" runat="server" Font-Bold="True" __designer:wfdid="w2" ValidationGroup="Bblock">Excel</asp:LinkButton></TD></TR><TR>
        <TD colSpan=2><asp:LinkButton id="lnkOverStay" onclick="lnkOverStay_Click" runat="server" Width="145px" Font-Bold="True" __designer:wfdid="w52" ValidationGroup="VReport">Overstayed Room List</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:LinkButton id="lnkExOverstay" onclick="lnkExOverstay_Click" runat="server" Font-Bold="True" __designer:wfdid="w5" ValidationGroup="VReport">Excel</asp:LinkButton></TD><TD colSpan=1><asp:LinkButton id="lnkExtended" onclick="lnkExtended_Click" runat="server" Width="145px" Font-Bold="True" __designer:wfdid="w2" ValidationGroup="VReport">Extended Stay Period</asp:LinkButton>&nbsp;&nbsp;&nbsp;</TD><TD colSpan=1><asp:LinkButton id="lnkCancelledPass" onclick="lnkCancelledPass_Click" runat="server" Width="181px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w11">Unoccupied Cancelled Pass</asp:LinkButton>&nbsp;&nbsp; <asp:LinkButton id="lbkExcUnoccup" onclick="lbkExcUnoccup_Click" runat="server" CausesValidation="False" Font-Bold="True" __designer:wfdid="w7">Excel</asp:LinkButton></TD></TR><TR>
        <TD colSpan=2><asp:LinkButton id="lnknonoccupReserve" onclick="lnknonoccupReserve_Click1" runat="server" Width="248px" Font-Bold="True" __designer:wfdid="w53" ValidationGroup="VReport">NonOccupying Reserved Room Report</asp:LinkButton> <asp:LinkButton id="lnkExcNonOcc" onclick="lnkExcNonOcc_Click" runat="server" Font-Bold="True" __designer:wfdid="w1" ValidationGroup="VReport">Excel</asp:LinkButton></TD><TD colSpan=1><asp:LinkButton id="lnkDelayed" onclick="lnkDelayed_Click" runat="server" Width="187px" Font-Bold="True" __designer:wfdid="w56" ValidationGroup="VReport">Delayed Occupied Room List</asp:LinkButton>&nbsp; <asp:LinkButton id="lnkExcDelay" onclick="lnkExcDelay_Click" runat="server" Font-Bold="True" __designer:wfdid="w2" ValidationGroup="VReport">Excel</asp:LinkButton></TD><TD style="HEIGHT: 22px" colSpan=1><asp:LinkButton id="lnkVacant24" onclick="lnkVacant24_Click" runat="server" Width="219px" Font-Bold="True" __designer:wfdid="w2" ValidationGroup="VReport">Vacant Room More than 24 hours</asp:LinkButton></TD></TR><TR>
        <TD colSpan=2><asp:LinkButton id="lnkMultiDaysStay" onclick="lnkMultiDaysStay_Click" runat="server" Width="253px" Font-Bold="True" __designer:wfdid="w1" ValidationGroup="VReport">Rooms allotted for more than two days</asp:LinkButton> <asp:LinkButton id="lnkExcRmAll" onclick="lnkExcRmAll_Click" runat="server" Font-Bold="True" __designer:wfdid="w3">Excel</asp:LinkButton></TD><TD colSpan=1><asp:LinkButton id="lnkMultiple" onclick="lnkMultiple_Click" runat="server" Width="261px" Font-Bold="True" ValidationGroup="VReport">Multiple Days Allotted Room List</asp:LinkButton></TD><TD style="HEIGHT: 22px" colSpan=1><asp:LinkButton id="lnkDoubleRent" onclick="lnkDoubleRent_Click" runat="server" Width="197px" Font-Bold="True" __designer:wfdid="w13" ValidationGroup="VReport">Room Allotted for Double Rent</asp:LinkButton>&nbsp; <asp:LinkButton id="lnkExRmDoubleRent" onclick="lnkExRmDoubleRent_Click" runat="server" Font-Bold="True" __designer:wfdid="w4" ValidationGroup="VReport">Excel</asp:LinkButton> </TD></TR><TR>
        <TD colspan="2">&nbsp;</TD><TD colSpan=1>&nbsp;</TD><TD colSpan=1 style="HEIGHT: 22px">
        <asp:LinkButton ID="lnk_blk" runat="server" onclick="lnk_blk_Click" 
            CausesValidation="False">Blocked room 
        report for department staff 
        </asp:LinkButton>
        </TD></TR><TR><TD style="WIDTH: 22px">
            <asp:Label ID="lblbuildnamereport" runat="server" Text="Building name" 
                Visible="False" Width="88px"></asp:Label>
            </TD><TD style="WIDTH: 172px">
                <asp:DropDownList ID="cmbReportBuildingname" runat="server" Visible="False" 
                    Width="203px">
                </asp:DropDownList>
            </TD>
            <td colspan="1">
                <asp:Label ID="lblTimeto" runat="server" Text="To Time" Visible="False" 
                    Width="59px"></asp:Label>
                <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" 
                    onclick="LinkButton5_Click" Visible="False" Width="138px">Released Room 
                report</asp:LinkButton>
            </td>
            <td colspan="1">
                <asp:TextBox ID="txtTimeto" runat="server" Visible="False" Width="236px"></asp:TextBox>
            </td>
        </TR><TR><TD colSpan=3>
            <asp:Panel ID="pnlRoomHistory" runat="server" __designer:wfdid="w4" 
                GroupingText="Reservation History Report" Visible="False">
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <asp:Label ID="lblFromDate1" runat="server" __designer:wfdid="w14" 
                                    Text="From  Date" Width="65px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate1" runat="server" __designer:wfdid="w15"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbldate1" runat="server" __designer:wfdid="w16" Text="To Date" 
                                    Width="56px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDateto" runat="server" __designer:wfdid="w17"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="TEXT-ALIGN: center">
                                <asp:LinkButton ID="lnkReserOccupy" runat="server" __designer:wfdid="w18" 
                                    Font-Bold="True" ForeColor="Blue" onclick="lnkReserOccupy_Click" 
                                    ValidationGroup="RHistory" Width="310px">Donor reservation and occupancy 
                                history report</asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="lnkDonorROHistory" runat="server" __designer:wfdid="w6" 
                                    Font-Bold="True" onclick="lnkDonorROHistory_Click" ValidationGroup="RHistory">Excel</asp:LinkButton>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            </TD>
            <td colspan="1">
                &nbsp;&nbsp;
            </td>
        </TR><TR><TD colSpan=4>
            <table style="WIDTH: 738px">
                <tbody>
                    <tr>
                        <td style="WIDTH: 100px; HEIGHT: 22px">
                            <asp:LinkButton ID="lnkUnoccupiedRoomAt4PM" runat="server" 
                                __designer:wfdid="w1" CausesValidation="False" Font-Bold="True" 
                                onclick="lnkUnoccupiedRoomAt4PM_Click" Width="181px">Unoccupied Room at 4 PM</asp:LinkButton>
                        </td>
                        <td style="WIDTH: 116px; HEIGHT: 22px">
                            <asp:LinkButton ID="lnkExcel1" runat="server" __designer:wfdid="w2" 
                                CausesValidation="False" Font-Bold="True" onclick="lnkExcel1_Click">Excel</asp:LinkButton>
                        </td>
                        <td style="WIDTH: 186px">
                            <asp:LinkButton ID="lnkUnoccupiedRoomsat10pm" runat="server" 
                                __designer:wfdid="w3" CausesValidation="False" Font-Bold="True" 
                                onclick="lnkUnoccupiedRoomsat10pm_Click">Unoccupied Rooms at 10 PM</asp:LinkButton>
                        </td>
                        <td style="WIDTH: 100px; HEIGHT: 22px">
                            <asp:LinkButton ID="lnkExcel2" runat="server" __designer:wfdid="w4" 
                                CausesValidation="False" Font-Bold="True" onclick="lnkExcel2_Click">Excel</asp:LinkButton>
                        </td>
                    </tr>
                </tbody>
            </table>
            </TD></TR>
        <tr>
            <td colspan="4">
                <asp:Button ID="btnReservation" runat="server" __designer:wfdid="w4" 
                    CausesValidation="False" CssClass="btnStyle_large" 
                    onclick="btnReservation_Click" Text="Reservation Chart" Visible="False" />
                <asp:Button ID="btnCancelledPass" runat="server" __designer:wfdid="w12" 
                    CausesValidation="False" CssClass="btnStyle_large" 
                    onclick="btnCancelledPass_Click" Text="Cancelled Pass" Visible="False" />
                <asp:Panel ID="pnlReservation" runat="server" __designer:wfdid="w5" 
                    GroupingText="Reservation Chart" Visible="False">
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    <asp:Label ID="lblType" runat="server" __designer:wfdid="w6" 
                                        Text="Reservation Type" Width="107px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbReservation" runat="server" __designer:wfdid="w7" 
                                        Width="120px">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                        <asp:ListItem>Donor Free</asp:ListItem>
                                        <asp:ListItem>Donor Paid</asp:ListItem>
                                        <asp:ListItem Value="tdb">TDB</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblResDate" runat="server" __designer:wfdid="w8" 
                                        Text="Reserve  Date" Width="85px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtResDate" runat="server" __designer:wfdid="w9"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;<asp:Button ID="btnChart" runat="server" __designer:wfdid="w10" 
                                        CausesValidation="False" CssClass="btnStyle_small" onclick="btnChart_Click" 
                                        Text="Chart" />
                                    &nbsp;&nbsp;&nbsp;</td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        </TBODY></TABLE><asp:Button id="btnHide" onclick="btnHide_Click" runat="server" CausesValidation="False" Text="Hide Report" CssClass="btnStyle_large"></asp:Button></asp:Panel> </TD></TR><TR><TD colSpan=1><asp:Panel id="pnlHistory" runat="server" Width="100%" GroupingText="Status History" __designer:wfdid="w13" Wrap="False" Visible="False"><asp:GridView id="dtgHistory" runat="server" Width="100%" __designer:wfdid="w3" ForeColor="#333333" GridLines="None" CellPadding="4" AutoGenerateColumns="False">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:BoundField DataField="room_id" Visible="False" HeaderText="Room_id"></asp:BoundField>
<asp:BoundField DataField="buildingname" HeaderText="Building Name"></asp:BoundField>
<asp:BoundField DataField="roomno" HeaderText="Room No"></asp:BoundField>
<asp:BoundField DataField="Status" HeaderText="Remark"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel></TD></TR><TR><TD colSpan=1><asp:Panel id="pnlTransaction" runat="server" Width="100%" GroupingText="Transaction History" __designer:wfdid="w1" Visible="False"><asp:GridView id="dtgTransaction" runat="server" Width="100%" __designer:wfdid="w2" ForeColor="#333333" GridLines="None" CellPadding="4" AutoGenerateColumns="False">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:BoundField DataField="Slno" HeaderText="Acc_Slno"></asp:BoundField>
<asp:BoundField DataField="Building Name" HeaderText="Building "></asp:BoundField>
<asp:BoundField DataField="Room No" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="Status" HeaderText="Status">
<ItemStyle Font-Bold="True"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="From Date" HeaderText="From Date"></asp:BoundField>
<asp:BoundField DataField="To Date" HeaderText="To Date"></asp:BoundField>
<asp:BoundField DataField="ADV_ReceiptNo" HeaderText="Adv_Receiptno"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel></TD></TR><TR><TD colSpan=1><asp:Panel id="pnlRChart" runat="server" Width="100%" GroupingText="Todays Reservation Chart for TDB" __designer:wfdid="w9" Visible="False"><asp:GridView id="dtgReservationChart" runat="server" Width="100%" ForeColor="#333333" __designer:wfdid="w1" AutoGenerateColumns="False" CellPadding="4" GridLines="None">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:BoundField DataField="buildingname" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="roomno" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="reservedate" HeaderText="Check In Time"></asp:BoundField>
<asp:BoundField DataField="expvacdate" HeaderText="Exp Vec Time"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView><BR /><asp:Label id="lblPage" runat="server" Text="Page No" __designer:wfdid="w3"></asp:Label>&nbsp;&nbsp; &nbsp;<asp:TextBox id="txtPage" runat="server" Width="53px" __designer:wfdid="w1" Enabled="False">0</asp:TextBox>&nbsp;&nbsp; <asp:Button id="btnNext" onclick="btnNext_Click" runat="server" Text="Next >>" __designer:wfdid="w2" CssClass="btnStyle_large"></asp:Button></asp:Panel> </TD></TR><TR><TD vAlign=top align=left colSpan=1><asp:Panel id="Roomdetailpanel" runat="server" Width="100%" GroupingText="Room Details"><asp:CheckBox id="chkSelectall" runat="server" Text="Select All" Visible="False" __designer:wfdid="w18" AutoPostBack="True" OnCheckedChanged="chkSelectall_CheckedChanged"></asp:CheckBox><asp:GridView id="dtgRoomManagement" tabIndex=30 runat="server" Width="100%" ForeColor="#333333" __designer:wfdid="w16" OnSelectedIndexChanged="dtgRoomManagement_SelectedIndexChanged" CellPadding="4" GridLines="None" OnRowCreated="dtgRoomManagement_RowCreated" AllowPaging="True" Caption="Grid view" OnPageIndexChanging="dtgRoomManagement_PageIndexChanging" OnSorting="dtgRoomManagement_Sorting" PageSize="25">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:TemplateField><ItemTemplate>
<asp:CheckBox id="chkselect" runat="server" OnCheckedChanged="chkselect_CheckedChanged"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView><asp:GridView id="dtgBlocked" runat="server" Width="100%" ForeColor="#333333" OnSelectedIndexChanged="dtgBlocked_SelectedIndexChanged" AutoGenerateColumns="False" CellPadding="4" GridLines="None" OnRowCreated="dtgBlocked_RowCreated" AllowPaging="True" OnPageIndexChanging="dtgBlocked_PageIndexChanging" PageSize="25" DataKeyNames="No">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:TemplateField><ItemTemplate>
<asp:CheckBox id="chkselect" runat="server" __designer:wfdid="w2" OnCheckedChanged="chkselect_CheckedChanged1"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Building" HeaderText="Building Name"></asp:BoundField>
<asp:BoundField DataField="roomno" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
<asp:BoundField DataField="No" Visible="False" HeaderText="No"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView><asp:GridView id="dtgRelease" runat="server" Width="100%" ForeColor="#333333" OnSelectedIndexChanged="dtgRelease_SelectedIndexChanged" AutoGenerateColumns="False" CellPadding="4" GridLines="None" OnRowCreated="dtgRelease_RowCreated" AllowPaging="True" OnPageIndexChanging="dtgRelease_PageIndexChanging" PageSize="25" DataKeyNames="No">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:TemplateField><ItemTemplate>
<asp:CheckBox id="chkselect" runat="server"></asp:CheckBox>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Building" HeaderText="Building Name"></asp:BoundField>
<asp:BoundField DataField="Room" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="fromd" HeaderText="Blocked Date"></asp:BoundField>
<asp:BoundField DataField="tod" HeaderText="Exp_Release Date"></asp:BoundField>
<asp:BoundField DataField="reason" HeaderText="Reason"></asp:BoundField>
<asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
<asp:BoundField DataField="No" Visible="False" HeaderText="No"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView><asp:GridView id="dtgForceRelease" runat="server" Width="100%" ForeColor="#333333" OnSelectedIndexChanged="dtgForceRelease_SelectedIndexChanged" AutoGenerateColumns="False" CellPadding="4" GridLines="None" OnRowCreated="dtgForceRelease_RowCreated" AllowPaging="True" OnPageIndexChanging="dtgForceRelease_PageIndexChanging" PageSize="25" DataKeyNames="No">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="buildingname" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="roomno" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="Vecatedate" HeaderText="Exp vec Time"></asp:BoundField>
<asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
<asp:BoundField DataField="No" Visible="False" HeaderText="No"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView><asp:GridView id="dtgNonOccupiedReserved" runat="server" Width="100%" ForeColor="#333333" OnSelectedIndexChanged="dtgNonOccupiedReserved_SelectedIndexChanged" AutoGenerateColumns="False" CellPadding="4" GridLines="None" OnRowCreated="dtgNonOccupiedReserved_RowCreated" AllowPaging="True" OnPageIndexChanging="dtgNonOccupiedReserved_PageIndexChanging" PageSize="25" DataKeyNames="No,room_id">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:TemplateField><ItemTemplate>
<asp:CheckBox id="chkselect" runat="server" __designer:wfdid="w1"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="buildingname" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="roomno" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="Reserve_Date" HeaderText="Res Date"></asp:BoundField>
<asp:BoundField DataField="reserve_mode" HeaderText="Type"></asp:BoundField>
<asp:BoundField DataField="passno" HeaderText="Pass No"></asp:BoundField>
<asp:BoundField DataField="swaminame" HeaderText="Name"></asp:BoundField>
<asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView><asp:GridView id="dtgReleaseReserved" runat="server" Width="100%" ForeColor="#333333" __designer:wfdid="w1" OnSelectedIndexChanged="dtgReleaseReserved_SelectedIndexChanged" AutoGenerateColumns="False" CellPadding="4" GridLines="None" OnRowCreated="dtgReleaseReserved_RowCreated" AllowPaging="True" OnPageIndexChanging="dtgReleaseReserved_PageIndexChanging" PageSize="25" DataKeyNames="No,room_id">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:TemplateField><ItemTemplate>
<asp:CheckBox id="chkselect" runat="server" __designer:wfdid="w6"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="buildingname" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="roomno" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="reservedate" HeaderText="Res Date"></asp:BoundField>
<asp:BoundField DataField="expvacdate" HeaderText="Exp Vec Date"></asp:BoundField>
<asp:BoundField DataField="ResType" HeaderText="Type"></asp:BoundField>
<asp:BoundField DataField="passno" HeaderText="Pass No"></asp:BoundField>
<asp:BoundField DataField="swaminame" HeaderText="Name"></asp:BoundField>
<asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView><asp:GridView id="dtgTdbReserve" runat="server" Width="100%" ForeColor="#333333" __designer:wfdid="w7" OnSelectedIndexChanged="dtgTdbReserve_SelectedIndexChanged" AutoGenerateColumns="False" CellPadding="4" GridLines="None" OnRowCreated="dtgTdbReserve_RowCreated" AllowPaging="True" OnPageIndexChanging="dtgTdbReserve_PageIndexChanging" PageSize="25" DataKeyNames="No">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:TemplateField><ItemTemplate>
<asp:CheckBox id="chkselect" runat="server" __designer:wfdid="w8"></asp:CheckBox>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="buildingname" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="roomno" HeaderText="Room No"></asp:BoundField>
<asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel> </TD></TR><TR><TD align=left colSpan=1><asp:Panel id="pnlvalid" runat="server"><TABLE><TBODY><TR><TD style="WIDTH: 101px"><asp:RequiredFieldValidator id="Requiredselectcriteria" runat="server" Width="228px" Height="18px" ForeColor="White" ValidationGroup="VGroup" SetFocusOnError="True" ErrorMessage="Selection criteria required" ControlToValidate="cmbSelectCriteria" InitialValue="-1"></asp:RequiredFieldValidator> </TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" targetcontrolid="Requiredselectcriteria">
                                    </cc1:ValidatorCalloutExtender> </TD><TD style="WIDTH: 100px">&nbsp;<cc1:CalendarExtender id="CalendarExtender9" runat="server" __designer:wfdid="w11" Format="dd-MM-yyyy" TargetControlID="txtFrom"></cc1:CalendarExtender></TD></TR><TR><TD><asp:RequiredFieldValidator id="RequiredBuilding" runat="server" Width="133px" ForeColor="White" ValidationGroup="VGroup" SetFocusOnError="True" ErrorMessage="Building name required" ControlToValidate="cmbSelectBuilding" InitialValue="-1"></asp:RequiredFieldValidator> </TD><TD><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" targetcontrolid="RequiredBuilding">
                                    </cc1:ValidatorCalloutExtender> </TD><TD>&nbsp;<cc1:CalendarExtender id="CalendarExtender10" runat="server" __designer:wfdid="w12" Format="dd-MM-yyyy" TargetControlID="txtTo"></cc1:CalendarExtender></TD></TR><TR><TD style="WIDTH: 101px"><asp:RequiredFieldValidator id="RequiredRoomno" runat="server" Width="134px" ForeColor="White" ValidationGroup="VGroup" SetFocusOnError="True" ErrorMessage="Roomno required" ControlToValidate="cmbSelectRoom" InitialValue="-1"></asp:RequiredFieldValidator> </TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" targetcontrolid="RequiredRoomno">
                                    </cc1:ValidatorCalloutExtender> </TD><TD></TD></TR><TR><TD style="WIDTH: 101px"><asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" Width="206px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Enter Correctly(DD/MM/YYYY)" ControlToValidate="txtFromDate" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator> </TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" targetcontrolid="RegularExpressionValidator1">
                                    </cc1:ValidatorCalloutExtender> </TD><TD style="WIDTH: 100px"><cc1:CalendarExtender id="CalendarExtender1" runat="server" Format="dd-MM-yyyy" TargetControlID="txtFromDate"></cc1:CalendarExtender> </TD></TR><TR><TD style="WIDTH: 101px; HEIGHT: 34px"><asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" Width="196px" Height="16px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Enter Correctly(DD/MM/YYYY)" ControlToValidate="txtToDate" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator> </TD><TD style="WIDTH: 100px; HEIGHT: 34px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" targetcontrolid="RegularExpressionValidator2">
                                    </cc1:ValidatorCalloutExtender> </TD><TD style="WIDTH: 100px; HEIGHT: 34px"><cc1:CalendarExtender id="CalendarExtender2" runat="server" Format="dd-MM-yyyy" TargetControlID="txtDate"></cc1:CalendarExtender> </TD></TR><TR><TD>&nbsp;<asp:RequiredFieldValidator id="Requiredtodate" runat="server" Width="115px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Required" ControlToValidate="txtToDate"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" targetcontrolid="Requiredtodate">
                                    </cc1:ValidatorCalloutExtender> </TD><TD style="WIDTH: 100px">&nbsp;<cc1:CalendarExtender id="CalendarExtender4" runat="server" Format="dd-MM-yyyy" TargetControlID="txtToDate"></cc1:CalendarExtender></TD></TR><TR><TD><asp:RequiredFieldValidator id="Requiredtotime" runat="server" Width="111px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Required" ControlToValidate="txtToTime"></asp:RequiredFieldValidator> </TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" targetcontrolid="Requiredtotime">
                                    </cc1:ValidatorCalloutExtender> </TD><TD style="WIDTH: 100px">&nbsp;<cc1:CalendarExtender id="CalendarExtender6" runat="server" __designer:wfdid="w12" Format="dd-MM-yyyy" TargetControlID="txtFromDate1"></cc1:CalendarExtender></TD></TR><TR><TD style="WIDTH: 101px"><asp:RequiredFieldValidator id="ReqReason" runat="server" Width="201px" ForeColor="White" ValidationGroup="Block" ErrorMessage="Must select a reason" ControlToValidate="cmbReason" InitialValue="-1"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender12" runat="server" TargetControlID="ReqReason"></cc1:ValidatorCalloutExtender></TD><TD style="WIDTH: 100px"><cc1:CalendarExtender id="CalendarExtender7" runat="server" __designer:wfdid="w13" Format="dd-MM-yyyy" TargetControlID="txtDateto"></cc1:CalendarExtender></TD></TR><TR><TD style="WIDTH: 101px"><asp:RequiredFieldValidator id="ReqFdate" runat="server" Width="189px" ForeColor="White" __designer:wfdid="w14" ValidationGroup="RHistory" ErrorMessage="Must select a from date" ControlToValidate="txtFromDate1"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender18" runat="server" __designer:wfdid="w16" TargetControlID="ReqFdate"></cc1:ValidatorCalloutExtender></TD><TD style="WIDTH: 100px"><cc1:CalendarExtender id="CalendarExtender8" runat="server" __designer:wfdid="w11" Format="dd-MM-yyyy" TargetControlID="txtResDate"></cc1:CalendarExtender></TD></TR><TR><TD style="WIDTH: 101px"><asp:RequiredFieldValidator id="ReqTDate" runat="server" Width="195px" ForeColor="White" __designer:wfdid="w15" ValidationGroup="RHistory" ErrorMessage="Must select a To Date " ControlToValidate="txtDateto"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender19" runat="server" __designer:wfdid="w17" TargetControlID="ReqTDate"></cc1:ValidatorCalloutExtender></TD><TD><cc1:CalendarExtender id="CalendarExtender11" runat="server" __designer:wfdid="w8" Format="dd-MM-yyyy" TargetControlID="txtVDate"></cc1:CalendarExtender></TD></TR><TR><TD><asp:RegularExpressionValidator id="RegularExpressionValidator5" runat="server" ForeColor="White" __designer:wfdid="w1" ErrorMessage="Minimum character length is 3 and only alphabets and dot is accepted." ControlToValidate="txtOfficer" ValidationExpression="[A-Z a-z .  ]{3,100}"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender20" runat="server" __designer:wfdid="w2" TargetControlID="RegularExpressionValidator5"></cc1:ValidatorCalloutExtender></TD><TD></TD></TR><TR><TD style="WIDTH: 101px"><asp:RegularExpressionValidator id="RegularExpressionValidator6" runat="server" Width="223px" ForeColor="White" __designer:wfdid="w4" ErrorMessage="minimum character length is 3 and only alphabets and dot is accepted" ControlToValidate="txtSwami" ValidationExpression="[A-Z a-z .  ]{3,100}"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender21" runat="server" __designer:wfdid="w3" TargetControlID="RegularExpressionValidator6"></cc1:ValidatorCalloutExtender></TD><TD></TD></TR><TR><TD colSpan=2><BR /><asp:Button style="DISPLAY: none" id="btnhidden1" onclick="btnhidden1_Click" runat="server" CausesValidation="False" Text="hidden"></asp:Button> <asp:Button style="DISPLAY: none" id="btnHidden" onclick="btnHidden_Click" runat="server" Text="Hidden"></asp:Button> <asp:TextBox id="TextBox1" runat="server" Visible="False" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox> <asp:Label id="Label3" runat="server" Text="Label" Visible="False"></asp:Label><BR /><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel7" runat="server" Width="99%" Height="31px" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Width="231px" ForeColor="MediumBlue" Text="  Tsunami ARMS -  Confirmation" Font-Bold="True"></asp:Label></asp:Panel> <BR /><asp:Panel id="pnlYesNo" runat="server"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD></TD><TD></TD></TR><TR><TD></TD><TD align=center>&nbsp;<asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="WIDTH: 7px; HEIGHT: 18px" align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="WIDTH: 7px"></TD><TD></TD><TD></TD></TR><TR><TD style="WIDTH: 7px"></TD><TD align=center>&nbsp;&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD style="WIDTH: 7px"></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel></asp:Panel> <BR /><cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize">
                </cc1:ModalPopupExtender><BR /><cc1:ModalPopupExtender id="ModalPopupExtender1" runat="server" TargetControlID="btnhidden1" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize"></cc1:ModalPopupExtender></TD><TD></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlrre" runat="server" Width="125px" Visible="False"><asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" Width="171px" ForeColor="White" ErrorMessage="in dd/mm/yyyy format" ControlToValidate="txtDate" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender8" runat="server" TargetControlID="RegularExpressionValidator3"></cc1:ValidatorCalloutExtender> <asp:RegularExpressionValidator id="RegularExpressionValidator4" runat="server" Width="343px" ForeColor="White" ErrorMessage="hh:mm format( hour between 1-12 & minute  between  0-60)" ControlToValidate="txtTime" ValidationExpression="[0-9]{1,2}[:]{1}[ 0-6]{1}[0-9]{1}[ ]{1,3}[PM,AM,pm,am]{1,2}"></asp:RegularExpressionValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender9" runat="server" TargetControlID="RegularExpressionValidator4"></cc1:ValidatorCalloutExtender> <cc1:CalendarExtender id="CalendarExtender3" runat="server" Format="dd-MM-yyyy" TargetControlID="txtdate"></cc1:CalendarExtender><BR /><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Width="179px" ForeColor="White" ErrorMessage="enter date " ControlToValidate="txtDate"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender10" runat="server" TargetControlID="RequiredFieldValidator1"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender11" runat="server" TargetControlID="RequiredFieldValidator2"></cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="243px" ForeColor="White" ErrorMessage="Enter time in hh:mm format" ControlToValidate="txtTime"></asp:RequiredFieldValidator><BR /><BR /><cc1:CalendarExtender id="CalendarExtender5" runat="server" Format="dd-MM-yyyy" TargetControlID="txtDateto"></cc1:CalendarExtender><BR /><TABLE><TBODY><TR><TD style="WIDTH: 101px"><asp:RequiredFieldValidator id="ReqReportdate" runat="server" Width="172px" ForeColor="White" ValidationGroup="VReport" ErrorMessage="Must enter date" ControlToValidate="txtDate"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender14" runat="server" TargetControlID="ReqReportdate"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 101px"><asp:RequiredFieldValidator id="ReqreportTime" runat="server" Width="154px" ForeColor="White" ValidationGroup="VReport" ErrorMessage="Must enter time" ControlToValidate="txtTime"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender15" runat="server" TargetControlID="ReqreportTime"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 101px"><asp:RequiredFieldValidator id="ReqRTime" runat="server" ForeColor="White" ValidationGroup="RoomRep" ErrorMessage="Must Enter Time" ControlToValidate="txtTime"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender16" runat="server" TargetControlID="ReqRTime"></cc1:ValidatorCalloutExtender></TD></TR></TBODY></TABLE><TABLE><TBODY><TR><TD style="WIDTH: 101px"><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Width="144px" ForeColor="White" ValidationGroup="vreport" ErrorMessage="Must enter time" ControlToValidate="txtTime"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender13" runat="server" TargetControlID="RequiredFieldValidator3"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 101px"><asp:RequiredFieldValidator id="ReqBlock" runat="server" Width="142px" ForeColor="White" __designer:wfdid="w2" ValidationGroup="Bblock" ErrorMessage="Must Select date" ControlToValidate="txtDate"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender17" runat="server" __designer:wfdid="w3" TargetControlID="ReqBlock"></cc1:ValidatorCalloutExtender></TD></TR></TBODY></TABLE><asp:CheckBox id="CheckBox1" runat="server" Text="Select All" __designer:wfdid="w17" Visible="False" AutoPostBack="True" OnCheckedChanged="chkSelectall_CheckedChanged"></asp:CheckBox>&nbsp;<asp:Label id="lblFrom" runat="server" Text="From Date" __designer:wfdid="w19" Visible="False"></asp:Label><asp:LinkButton id="lnkRoomHistoryReport" onclick="lnkRoomHistoryReport_Click" runat="server" Width="128px" __designer:wfdid="w1" Visible="False" ValidationGroup="RoomRep">Room History Report</asp:LinkButton><asp:LinkButton id="lnkRoomHistory" onclick="lnkRoomHistory_Click" runat="server" Width="186px" ForeColor="Blue" Font-Bold="True" __designer:wfdid="w2" Visible="False" ValidationGroup="RHistory">Room Status History Report</asp:LinkButton><asp:TextBox id="txtFrom" runat="server" Width="113px" __designer:wfdid="w20" Visible="False"></asp:TextBox><asp:Button id="Button1" onclick="Button1_Click" runat="server" CausesValidation="False" Text="Button" __designer:wfdid="w19" Visible="False"></asp:Button> <asp:LinkButton id="lnkVacantRoom" onclick="lnkVacantRoom_Click" runat="server" Width="299px" Font-Bold="True" __designer:wfdid="w13" Visible="False" ValidationGroup="VReport">Allocated Room Vacant for more than 24 hours</asp:LinkButton></asp:Panel>&nbsp;&nbsp;&nbsp;&nbsp; 
        <asp:LinkButton ID="lnkMultipleDays" runat="server" __designer:wfdid="w3" 
            onclick="lnkMultipleDays_Click" ValidationGroup="RoomRep" Visible="False" 
            Width="172px">Multiple days alloted room</asp:LinkButton>
        </TD></TR><TR><TD align=left colSpan=1></TD></TR></TBODY></TABLE>
</ContentTemplate>
<Triggers>
    <asp:PostBackTrigger ControlID="lnkBlocked" /> 
     <asp:PostBackTrigger ControlID="lnknonoccupReserve" /> 
      <asp:PostBackTrigger ControlID="lnkOccupy" /> 
      <asp:PostBackTrigger ControlID="lnkVacant" />
     <asp:PostBackTrigger ControlID="lnkOverStay" /> 
     <asp:PostBackTrigger ControlID="lnkDelayed" /> 
     <asp:PostBackTrigger ControlID="lnkVacantRoom" /> 
     <asp:PostBackTrigger ControlID="lnkRoomHistoryReport" /> 
     <asp:PostBackTrigger ControlID="lnkRoomHistory" /> 
     <asp:PostBackTrigger ControlID="lnkExtended" /> 
     <asp:PostBackTrigger ControlID="lnkMultipleDays" /> 
     <asp:PostBackTrigger ControlID="lnkMultiple" /> 
      <asp:PostBackTrigger ControlID="lnkVacant24" /> 
     <asp:PostBackTrigger ControlID="lnkReserOccupy" /> 
     <asp:PostBackTrigger ControlID="btnChart" /> 
     <asp:PostBackTrigger ControlID="lnkDoubleRent" /> 
     <asp:PostBackTrigger ControlID="lnkVacantAnyTime" /> 
     <asp:PostBackTrigger ControlID="lnkCancelledPass" /> 
     <asp:PostBackTrigger ControlID="lnkMultiDaysStay" /> 
     <asp:PostBackTrigger ControlID="lnkStatusHistory" /> 
     <asp:PostBackTrigger ControlID="lnkEXBlock" /> 
     <asp:PostBackTrigger ControlID="lnkExcOccupy" /> 
     <asp:PostBackTrigger ControlID="lnkExcVacant" /> 
     <asp:PostBackTrigger ControlID="lnkExOverstay" /> 
     <asp:PostBackTrigger ControlID="lbkExcUnoccup" />
     <asp:PostBackTrigger ControlID="lnkExcNonOcc" /> 
     <asp:PostBackTrigger ControlID="lnkExcDelay" /> 
     <asp:PostBackTrigger ControlID="lnkExcRmAll" /> 
     <asp:PostBackTrigger ControlID="lnkExRmDoubleRent" /> 
     <asp:PostBackTrigger ControlID="lnkDonorROHistory" />      
      <asp:PostBackTrigger ControlID="lnkUnoccupiedRoomAt4PM" /> 
     <asp:PostBackTrigger ControlID="lnkUnoccupiedRoomsat10pm" /> 
     <asp:PostBackTrigger ControlID="lnkExcel1" /> 
     <asp:PostBackTrigger ControlID="lnkExcel2" />   
     <asp:PostBackTrigger ControlID="lnk_blk" />   
  </Triggers>
    </asp:UpdatePanel>
</asp:Content>

