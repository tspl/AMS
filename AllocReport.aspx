<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AllocReport.aspx.cs" Inherits="AllocReport" Title="" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE><TBODY><TR><TD style="WIDTH: 100px"><TABLE><TBODY><TR><TD vAlign=middle align=center><asp:Label id="Label4" runat="server" Width="139px" ForeColor="Gold" Text="Report View" Font-Bold="True" BackColor="Blue" BorderColor="Cornsilk" BorderStyle="Solid" __designer:wfdid="w7" Font-Size="Medium"></asp:Label>&nbsp;</TD></TR><TR><TD vAlign=middle align=center><TABLE><TBODY><TR><TD><asp:Button id="btnledger" onclick="btnledger_Click" runat="server" Width="200px" Height="30px" Text="Ledger Report" Font-Bold="True" BackColor="InactiveCaptionText" __designer:wfdid="w8" Font-Size="Small" Font-Strikeout="False" Font-Underline="False" Font-Overline="False"></asp:Button></TD><TD><asp:Button id="btnroomreport" onclick="btnroomreport_Click" runat="server" Width="200px" Height="30px" Text="Room Report" Font-Bold="True" BackColor="InactiveCaptionText" __designer:wfdid="w9" Font-Size="Small" Font-Strikeout="False" Font-Underline="False" Font-Overline="False"></asp:Button></TD><TD><asp:Button id="btndonorreport" onclick="btndonorreport_Click" runat="server" Width="200px" Height="30px" Text="Donor Report" Font-Bold="True" BackColor="InactiveCaptionText" __designer:wfdid="w10" Font-Size="Small" Font-Strikeout="False" Font-Underline="False" Font-Overline="False"></asp:Button></TD><TD><asp:Button id="btnotherreport" onclick="btnotherreport_Click" runat="server" Width="200px" Height="30px" Text="Other Report" Font-Bold="True" BackColor="InactiveCaptionText" __designer:wfdid="w11" Font-Size="Small" Font-Strikeout="False" Font-Underline="False" Font-Overline="False"></asp:Button></TD></TR></TBODY></TABLE></TD></TR><TR><TD style="WIDTH: 100px" vAlign=middle align=center><asp:Panel id="pnlledger" runat="server" Width="100%" GroupingText="Ledger Reports" __designer:wfdid="w12"><TABLE width="100%"><TBODY><TR><TD><asp:Label id="Label19" runat="server" Width="60px" Text="From date" __designer:wfdid="w1"></asp:Label></TD><TD><asp:TextBox id="txtfromd" tabIndex=41 runat="server" Width="90px" __designer:wfdid="w2"></asp:TextBox></TD><TD><asp:Label id="Label20" runat="server" Width="48px" Text="To date" __designer:wfdid="w3"></asp:Label></TD><TD><asp:TextBox id="txttod" tabIndex=42 runat="server" Width="90px" __designer:wfdid="w4"></asp:TextBox></TD><TD style="WIDTH: 278px"><asp:LinkButton id="lnktotalallocseasonreport" tabIndex=43 onclick="lnktotalallocseasonreport_Click" runat="server" Width="301px" Height="20px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w5" Font-Size="Small">Accommodation ledger report between dates</asp:LinkButton></TD><TD><asp:LinkButton id="lnkAccLedBetExcel" onclick="lnkAccLedBetExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w4">.Excel</asp:LinkButton></TD></TR><TR><TD style="HEIGHT: 26px"><asp:Label id="Label7" runat="server" Width="60px" Text="Counter" __designer:wfdid="w1"></asp:Label></TD><TD><asp:DropDownList id="cmbcounter" runat="server" Width="100px" __designer:wfdid="w2" DataValueField="counter_id" DataTextField="counter_ip"></asp:DropDownList></TD><TD><asp:Label id="Label21" runat="server" Width="59px" Text="Date" __designer:wfdid="w6"></asp:Label></TD><TD><asp:TextBox id="txtdate" tabIndex=39 runat="server" Width="90px" __designer:wfdid="w7"></asp:TextBox></TD><TD style="WIDTH: 278px">
    <asp:LinkButton id="lnkDonorPaidRoomAllocationReport" tabIndex=40 
        onclick="lnkDonorPaidRoomAllocationReport_Click" runat="server" Width="265px" 
        CausesValidation="False" Font-Bold="True" __designer:wfdid="w8" 
        Font-Size="Small">Accommodation ledger  on  current date</asp:LinkButton></TD><TD><asp:LinkButton id="lnkAccLedExcel" onclick="lnkAccLedExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w3">.Excel</asp:LinkButton></TD></TR><TR><TD style="HEIGHT: 26px">
    <asp:Label id="Label8" runat="server" Width="60px" Text="User" 
        __designer:wfdid="w1"></asp:Label></TD><TD><asp:DropDownList id="cmbuser" 
            runat="server" Width="100px" __designer:wfdid="w2" DataValueField="user_id" 
            DataTextField="username" AutoPostBack="True"></asp:DropDownList></TD><TD colSpan=2></TD><TD><asp:LinkButton id="lnkunclaimeddeposit" onclick="lnkunclaimeddeposit_Click" runat="server" Font-Bold="True" __designer:wfdid="w9">Unclaimed Security Deposit</asp:LinkButton></TD><TD><asp:LinkButton id="lnkUnClSecLedExcel" onclick="lnkUnClSecLedExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w5">.Excel</asp:LinkButton></TD></TR><TR>
    <TD style="HEIGHT: 22px" align="left">
        <asp:Label ID="Label26" runat="server" Text="From time"></asp:Label>
    </TD>
    <td align="left" style="HEIGHT: 22px">
        <asp:TextBox ID="txtfromTime" runat="server" Width="90px"></asp:TextBox>
    </td>
    <TD style="HEIGHT: 22px">
        <asp:Label ID="Label27" runat="server" Text="To time"></asp:Label>
    </TD>
    <td style="HEIGHT: 22px">
        <asp:TextBox ID="txttoTime" runat="server" Width="90px"></asp:TextBox>
    </td>
    <TD style="HEIGHT: 22px"><asp:LinkButton id="lnksecurityledger" onclick="lnksecurityledger_Click" runat="server" Width="169px" Font-Bold="True" __designer:wfdid="w10">Security Deposit Ledger</asp:LinkButton></TD><TD style="HEIGHT: 22px"><asp:LinkButton id="lnkSecLedExcel" onclick="lnkSecLedExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w6">.Excel</asp:LinkButton></TD></TR><TR>
    <TD align="left">
        <asp:Label ID="lblbuildstattusroom0" runat="server" __designer:wfdid="w111" Text="Building name" 
            Width="86px"></asp:Label>
    </TD>
    <td align="left">
        <asp:DropDownList ID="cmbbuildroomstat0" runat="server" __designer:wfdid="w112" AutoPostBack="True" 
            DataTextField="buildingname" DataValueField="build_id" Height="22px" 
            OnSelectedIndexChanged="cmbbuildroomstat0_SelectedIndexChanged" Width="150px">
        </asp:DropDownList>
    </td>
    <TD colSpan=2>&nbsp;</TD><TD><asp:LinkButton id="lnkoverstayledgerreport" onclick="lnkoverstayledgerreport_Click" runat="server" Width="141px" Font-Bold="True" __designer:wfdid="w11">Over Stay Ledger</asp:LinkButton></TD><TD><asp:LinkButton id="lnkOverStayledExcel" onclick="lnkOverStayledExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w7">.Excel</asp:LinkButton></TD></TR><TR>
    <TD>
        <asp:Label ID="Label28" runat="server" __designer:wfdid="w74" Text="Room no" Width="59px"></asp:Label>
    </TD>
    <td>
        <asp:DropDownList ID="cmbRoom0" runat="server" __designer:wfdid="w92" DataTextField="roomno" 
            DataValueField="room_id" Height="22px" Width="150px">
        </asp:DropDownList>
    </td>
    <TD></TD>
    <td>
    </td>
    <TD><asp:LinkButton id="lnkroomdamageledger" onclick="lnkroomdamageledger_Click" runat="server" Width="168px" Font-Bold="True" __designer:wfdid="w12">Room Damage Ledger</asp:LinkButton></TD><TD><asp:LinkButton id="lnkDamLedExcel" onclick="lnkDamLedExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w8">.Excel</asp:LinkButton></TD></TR><TR><TD colSpan=2></TD><TD colSpan=2></TD><TD><asp:LinkButton id="lnkkweylostledger" onclick="lnkkweylostledger_Click" runat="server" Font-Bold="True" __designer:wfdid="w13">Key Lost Charge Large</asp:LinkButton></TD><TD><asp:LinkButton id="keyLostExcel" onclick="keyLostExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w9">.Excel</asp:LinkButton></TD></TR>
    <tr>
        <td colspan="2">
            &nbsp;</td>
        <td colspan="2">
            &nbsp;</td>
        <td>
            <asp:LinkButton ID="lnkAccLedgerTime" runat="server" Font-Bold="true" 
                onclick="lnkAccLedgerTime_Click">Accommodation Ledger with time</asp:LinkButton>
        </td>
        <td>
            <asp:LinkButton ID="lnktimeExcel" runat="server" Font-Bold="true" onclick="lnktimeExcel_Click">.Excel</asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;</td>
        <td colspan="2">
            &nbsp;</td>
        <td>
            <asp:LinkButton ID="lnkAccLedgerBuild" runat="server" Font-Bold="true" 
                onclick="lnkAccLedgerBuild_Click">Accommodation Ledger Room wise</asp:LinkButton>
        </td>
        <td>
            <asp:LinkButton ID="lnkroomExcel" runat="server" Font-Bold="true" onclick="lnkroomExcel_Click">.Excel</asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;</td>
        <td colspan="2">
            &nbsp;</td>
        <td>
            <asp:LinkButton ID="LinkButton9" runat="server" Font-Bold="True" 
                onclick="LinkButton8_Click">Datewise Ledger</asp:LinkButton>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;</td>
        <td colspan="2">
            &nbsp;</td>
        <td>
            <asp:LinkButton ID="lb_ledger" runat="server" Font-Bold="True" 
                onclick="lb_ledger_Click"> Ledger</asp:LinkButton>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    </TBODY></TABLE></asp:Panel></TD></TR>
    <tr>
        <td align="center" valign="middle" width="100%">
            <asp:Panel ID="Panel9" runat="server" GroupingText="Online Reports " 
                Width="100%">
                <table style="width: 100%">
                    <tr>
                        <td width="10%">
                            <asp:Label ID="Label23" runat="server" Text="Date   "></asp:Label>
                        </td>
                        <td width="15%">
                            <asp:TextBox ID="txtonldate" runat="server" __designer:wfdid="w7" tabIndex="39" 
                                Width="120px" Wrap="False"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtonldate_CalendarExtender" runat="server" 
                                Format="dd/MM/yyyy" TargetControlID="txtonldate">
                            </cc1:CalendarExtender>
                        </td>
                        <td colspan="2">
                            &nbsp;</td>
                        <td width="50%">
                            <asp:LinkButton ID="LinkButton3" runat="server" onclick="LinkButton3_Click">Pending 
                            online reservations</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" width="25%">
                            &nbsp;</td>
                        <td colspan="2">
                            &nbsp;</td>
                        <td width="50%">
                            <asp:LinkButton ID="LinkButton2" runat="server" onclick="LinkButton2_Click">Completed 
                            online reservation</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%">
                            <asp:Label ID="Label24" runat="server" Text="From Date   "></asp:Label>
                        </td>
                        <td style="width: 12%" width="25%">
                            <asp:TextBox ID="txtfromonldate" runat="server" __designer:wfdid="w7" 
                                tabIndex="39" Width="120px" Wrap="False"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtfromonldate_CalendarExtender" runat="server" 
                                Format="dd/MM/yyyy" TargetControlID="txtfromonldate">
                            </cc1:CalendarExtender>
                        </td>
                        <td width="10%">
                            <asp:Label ID="Label25" runat="server" Text="To Date   "></asp:Label>
                        </td>
                        <td style="width: 15%">
                            <asp:TextBox ID="txttoonldate" runat="server" __designer:wfdid="w7" 
                                tabIndex="39" Width="120px" Wrap="False"></asp:TextBox>
                            <cc1:CalendarExtender ID="txttoonldate_CalendarExtender" runat="server" 
                                Format="dd/MM/yyyy" TargetControlID="txttoonldate">
                            </cc1:CalendarExtender>
                        </td>
                        <td width="50%">
                            <asp:LinkButton ID="LinkButton4" runat="server" onclick="LinkButton4_Click">Online 
                            reservation Ledger</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" style="height: 24px">
                            </td>
                        <td style="width: 12%; height: 24px;" width="25%">
                            </td>
                        <td width="10%" style="height: 24px">
                            </td>
                        <td style="height: 24px; width: 15%;">
                            </td>
                        <td width="50%" style="height: 24px">
                            <asp:LinkButton ID="LinkButton5" runat="server" onclick="LinkButton5_Click">Donor Free Online
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" style="height: 24px">
                            </td>
                        <td style="width: 12%; height: 24px;" width="25%">
                            </td>
                        <td width="10%" style="height: 24px">
                            </td>
                        <td style="height: 24px; width: 15%;">
                            </td>
                        <td width="50%" style="height: 24px">
                            <asp:LinkButton ID="LinkButton6" runat="server" onclick="LinkButton6_Click">Donor Paid Online</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%">
                            &nbsp;</td>
                        <td style="width: 12%" width="25%">
                            &nbsp;</td>
                        <td width="10%">
                            &nbsp;</td>
                        <td style="width: 15%">
                            &nbsp;</td>
                        <td width="50%">
                            <asp:LinkButton ID="LinkButton7" runat="server">RoomAllotted List Online</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <TR><TD><asp:Panel id="pnlroom" runat="server" Width="100%" GroupingText="Room Report" __designer:wfdid="w26"><TABLE width="100%"><TBODY><TR><TD colSpan=5><asp:Panel id="Panel4" runat="server" GroupingText="Room Status" __designer:wfdid="w90"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblbuildstattusroom" runat="server" Width="86px" Text="Building name" __designer:wfdid="w111"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbbuildroomstat" runat="server" Width="150px" Height="22px" __designer:wfdid="w112" AutoPostBack="True" OnSelectedIndexChanged="cmbbuildroomstat_SelectedIndexChanged" DataValueField="build_id" DataTextField="buildingname"></asp:DropDownList></TD><TD></TD><TD style="WIDTH: 100px"></TD><TD>&nbsp;&nbsp; </TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="Label1" runat="server" Width="59px" Text="Room no" __designer:wfdid="w74"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbRoom" runat="server" Width="150px" Height="22px" __designer:wfdid="w92" DataValueField="room_id" DataTextField="roomno"></asp:DropDownList></TD><TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD><TD style="WIDTH: 100px"><asp:Button id="btnroomstatus" onclick="btnroomstatus_Click" runat="server" Width="200px" Text="Current Room Status" Font-Bold="True" BackColor="#C0C0FF" __designer:wfdid="w93"></asp:Button></TD><TD></TD><TD style="WIDTH: 100px"><asp:Button id="btnroomhistory" onclick="btnroomhistory_Click" runat="server" Width="200px" Text="Room Occupancy History" Font-Bold="True" BackColor="#C0C0FF" __designer:wfdid="w94" Font-Names="Arial"></asp:Button></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD></TD><TD style="WIDTH: 100px"><BR /></TD><TD></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD><asp:Label id="lblToDate" runat="server" Text="To Date" __designer:wfdid="w5"></asp:Label></TD><TD><asp:TextBox id="txtTo" runat="server" Width="145px" __designer:wfdid="w6"></asp:TextBox></TD><TD></TD><TD style="WIDTH: 100px"><asp:Button id="btnRoomStatusHistory" onclick="btnRoomStatusHistory_Click" runat="server" Width="200px" Text="Previous 3  Room Status " Font-Bold="True" BackColor="#C0C0FF" __designer:wfdid="w4"></asp:Button></TD><TD></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD></TR>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                From Date</td>
            <td>
                <asp:TextBox ID="txtFrom1" runat="server" __designer:wfdid="w6" Width="145px"></asp:TextBox>
                <cc1:CalendarExtender ID="txtFrom1_CalendarExtender" runat="server" 
                    __designer:wfdid="w7" Format="dd-MM-yyyy" TargetControlID="txtFrom1">
                </cc1:CalendarExtender>
            </td>
            <td>
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                To Date</td>
            <td>
                <asp:TextBox ID="txtTo1" runat="server" __designer:wfdid="w6" Width="145px"></asp:TextBox>
                <cc1:CalendarExtender ID="txtTo1_CalendarExtender" runat="server" 
                    __designer:wfdid="w7" Format="dd-MM-yyyy" TargetControlID="txtTo1">
                </cc1:CalendarExtender>
            </td>
            <td>
                &nbsp;</td>
            <td style="WIDTH: 100px">
                <asp:Button ID="btnRoomStatusHistory0" runat="server" __designer:wfdid="w4" 
                    BackColor="#C0C0FF" Font-Bold="True" onclick="btnRoomStatusHistory0_Click" 
                    Text=" Room Status " Width="200px" />
            </td>
            <td>
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="7">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:GridView ID="gdroomstatus0" runat="server" __designer:wfdid="w15" 
                                AllowPaging="True" CellPadding="4" ForeColor="#333333" 
                                OnPageIndexChanging="gdroomstatus0_PageIndexChanging" 
                                onrowcreated="gdroomstatus0_RowCreated" 
                                onrowdatabound="gdroomstatus0_RowDataBound" PageSize="5" Width="100%">
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
                                <EditRowStyle BackColor="#2461BF" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                    HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btndown" runat="server" __designer:wfdid="w4" 
                                BackColor="#C0C0FF" Font-Bold="True" onclick="btndown_Click" Text=" Download" 
                                Visible="False" Width="200px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        </TBODY></TABLE><asp:GridView id="gdroomstatus" runat="server" Width="100%" ForeColor="#333333" __designer:wfdid="w15" CellPadding="4" OnPageIndexChanging="gdroomstatus_PageIndexChanging" AllowPaging="True" PageSize="5">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> <asp:GridView id="dtgRoomStatusHistory" runat="server" ForeColor="#333333" __designer:wfdid="w9" CellPadding="4" GridLines="None" AutoGenerateColumns="False">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:BoundField DataField="Slno" HeaderText="Acc_Slno"></asp:BoundField>
<asp:BoundField DataField="Building Name" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="Room No" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="Status" HeaderText="Status">
<ItemStyle Font-Bold="True"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="From Date" HeaderText="From Date"></asp:BoundField>
<asp:BoundField DataField="To Date" HeaderText="To Date"></asp:BoundField>
<asp:BoundField DataField="ADV_Receiptno" HeaderText="Adv_Receipt No"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView>&nbsp;</asp:Panel></TD></TR><TR><TD colSpan=5><asp:Panel id="Panel5" runat="server" GroupingText="Room Current Status Reports" __designer:wfdid="w97"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblbuildingnamereport" runat="server" Width="99px" Text="Building name" __designer:wfdid="w122"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbBuild" runat="server" Width="150px" Height="22px" __designer:wfdid="w123" OnSelectedIndexChanged="cmbBuild_SelectedIndexChanged" DataValueField="build_id" DataTextField="buildingname"></asp:DropDownList></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px"></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkVacantRoomRepor" tabIndex=38 onclick="lnkVacantRoomRepor_Click" runat="server" Width="169px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w3" Font-Size="Small">Vacant Room Report</asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkVacantExcel" onclick="lnkVacantExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w1">.Excel</asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkocccuroomreport" onclick="lnkocccuroomreport_Click" runat="server" Width="140px" Font-Bold="True" __designer:wfdid="w125" Font-Size="Small">Occupy Room Report</asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkOverStauExcel" onclick="lnkOverStauExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w5">.Excel</asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnknonvecating" onclick="lnknonvecating_Click" runat="server" Width="157px" Font-Bold="True" __designer:wfdid="w126" Font-Size="Small">Non vacating Report</asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkNonVacatExcel" onclick="lnkNonVacatExcel_Click1" runat="server" Font-Bold="True" __designer:wfdid="w9">.Excel</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px"></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkcompletestatus" onclick="lnkcompletestatus_Click" runat="server" Width="169px" Font-Bold="True" __designer:wfdid="w127" Font-Size="Small">Complete Room Status</asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkExtendreport" onclick="lnkExtendreport_Click" runat="server" Width="142px" Font-Bold="True" __designer:wfdid="w128" Font-Size="Small">Extend Rooms Report</asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkExtendExcel" onclick="lnkExtendExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w6" Visible="False">.Excel</asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkblockedroom" onclick="lnkblockedroom_Click" runat="server" Width="158px" Font-Bold="True" __designer:wfdid="w129" Font-Size="Small">Blocked Room Report</asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkBlockExcel" onclick="lnkBlockExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w10">.Excel</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkoverstayedreport" onclick="lnkoverstayedreport_Click" runat="server" Width="168px" Font-Bold="True" __designer:wfdid="w130" Font-Size="Small">Over Stayed Room Report</asp:LinkButton></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkOverStayExcel" onclick="lnkOverStayExcel_Click1" runat="server" Font-Bold="True" __designer:wfdid="w13">.Excel</asp:LinkButton></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnksecuritydepositledger" onclick="lnksecuritydepositledger_Click" runat="server" Width="143px" Font-Bold="True" __designer:wfdid="w131" Font-Size="Small">List of rooms vecated </asp:LinkButton></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkVacateRoomExcel" onclick="lnkVacateRoomExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w7">.Excel</asp:LinkButton></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnknonoccureserverooms" onclick="lnknonoccureserverooms_Click" runat="server" Width="164px" Font-Bold="True" __designer:wfdid="w2" Font-Size="Small" Visible="False">Non Occ Reserve Rooms</asp:LinkButton></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkVac24Excel" onclick="lnkVac24Excel_Click" runat="server" Font-Bold="True" __designer:wfdid="w11" Visible="False">.Excel</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkmutiallocatereport" onclick="lnkmutiallocatereport_Click" runat="server" Width="141px" Font-Bold="True" __designer:wfdid="w133" Font-Size="Small">Mul Days Allot Rooms</asp:LinkButton></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkMultiExcel" onclick="lnkMultiExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w8">.Excel</asp:LinkButton></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkvacant24hour" onclick="lnkvacant24hour_Click" runat="server" Width="166px" Font-Bold="True" __designer:wfdid="w3" Font-Size="Small" Visible="False">Vacant Rooms for 24 hour</asp:LinkButton></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkNonOccResExcel" runat="server" Font-Bold="True" __designer:wfdid="w12" Visible="False">.Excel</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD><asp:Panel id="pnldonor" runat="server" Width="100%" GroupingText="Donor Report" __designer:wfdid="w45"><TABLE width="100%"><TBODY><TR><TD colSpan=3><asp:Panel id="Panel6" runat="server" Width="100%" GroupingText="Donor Pass Status" __designer:wfdid="w31"><TABLE width="100%"><TBODY><TR><TD><asp:Label id="lblpType" runat="server" Width="65px" Text="Pass Type" __designer:wfdid="w32"></asp:Label></TD><TD><asp:DropDownList id="cmbdPtype" runat="server" Width="100px" Height="22px" __designer:wfdid="w33"><asp:ListItem Value="-1">--Select--</asp:ListItem>
<asp:ListItem Value="1">Paid Pass</asp:ListItem>
<asp:ListItem Value="0">Free Pass</asp:ListItem>
</asp:DropDownList></TD><TD><asp:Label id="lblpNo" runat="server" Width="54px" Text="Pass No" __designer:wfdid="w34"></asp:Label></TD><TD><asp:TextBox id="txtdPass" runat="server" Width="100px" __designer:wfdid="w35"></asp:TextBox></TD><TD><asp:Button id="btnPassStatus" onclick="btnPassStatus_Click" runat="server" Width="175px" Text="Pass Status" Font-Bold="True" BackColor="#C0C0FF" __designer:wfdid="w36"></asp:Button></TD><TD><asp:Button id="Button1" onclick="Button1_Click" runat="server" Width="175px" Text="Detailed Pass Status" Font-Bold="True" BackColor="#C0C0FF" __designer:wfdid="w37"></asp:Button></TD></TR><TR><TD colSpan=6><asp:GridView id="gdPassStatus" runat="server" Width="100%" ForeColor="#333333" __designer:wfdid="w38" CellPadding="4">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR><TR><TD colSpan=6><asp:GridView id="gdpassaddtionalStatus" runat="server" Width="100%" ForeColor="#333333" __designer:wfdid="w39" OnSelectedIndexChanged="gdpassaddtionalStatus_SelectedIndexChanged" CellPadding="4">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=3><asp:Panel id="Panel1" runat="server" GroupingText="Donor Pass Report Donor Wise" __designer:wfdid="w40"><TABLE><TBODY><TR><TD><asp:Label id="Label2" runat="server" Width="70px" Text="Donor name" __designer:wfdid="w41"></asp:Label></TD><TD><asp:DropDownList id="cmbrepDonor" runat="server" Width="150px" Height="22px" __designer:wfdid="w42" DataValueField="donor_id" DataTextField="donor_name"></asp:DropDownList></TD><TD></TD><TD></TD></TR><TR><TD><asp:Label id="Label3" runat="server" Text="season" __designer:wfdid="w43"></asp:Label></TD><TD><asp:DropDownList id="cmbrepSeason" runat="server" Width="150px" Height="22px" __designer:wfdid="w44" DataValueField="season_sub_id" DataTextField="seasonname"></asp:DropDownList></TD><TD><asp:LinkButton id="lnkDonorFreeRoomAllocationReport" tabIndex=44 onclick="lnkDonorFreeRoomAllocationReport_Click" runat="server" Width="224px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w45" Font-Size="Small">Donor pass report Donor Wise</asp:LinkButton></TD><TD><asp:LinkButton id="lnkDonPass" runat="server" Font-Bold="True" __designer:wfdid="w1" OnClick="lnkDonPass_Click">.Excel</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=3><asp:Panel id="Panel2" runat="server" GroupingText="Donor Pass Report Building & Room Wise" __designer:wfdid="w46"><TABLE><TBODY><TR><TD><asp:Label id="Label5" runat="server" Width="83px" Text="Building name" __designer:wfdid="w47"></asp:Label></TD><TD><asp:DropDownList id="cmbDonBuilding" runat="server" Width="150px" Height="22px" __designer:wfdid="w48" AutoPostBack="True" OnSelectedIndexChanged="cmbDonBuilding_SelectedIndexChanged" DataValueField="build_id" DataTextField="buildingname"></asp:DropDownList></TD><TD><asp:LinkButton id="lnkunutilizedpass" onclick="lnkunutilizedpass_Click" runat="server" Width="155px" Font-Bold="True" __designer:wfdid="w49">Unutilized Donor Pass</asp:LinkButton></TD><TD></TD></TR><TR><TD><asp:Label id="Label6" runat="server" Width="56px" Text="Room no" __designer:wfdid="w50"></asp:Label></TD><TD><asp:DropDownList id="cmbDonRoom" runat="server" Width="150px" Height="22px" __designer:wfdid="w51" DataValueField="room_id" DataTextField="roomno"></asp:DropDownList></TD><TD><asp:LinkButton id="lnkpassutilization" onclick="lnkpassutilization_Click" runat="server" Width="192px" Font-Bold="True" __designer:wfdid="w1">Donor Pass Utilization Report</asp:LinkButton></TD><TD></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD><asp:LinkButton id="lnkDonorDetails" onclick="lnkDonorDetails_Click" runat="server" Width="162px" Font-Bold="True" __designer:wfdid="w2">Donor Details with pass</asp:LinkButton></TD><TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=3><asp:Panel id="Panel3" runat="server" GroupingText="Donor Pass Allocation Details Day wise" __designer:wfdid="w53"><TABLE><TBODY><TR><TD><asp:Label id="lbldondate" runat="server" Text="Date" __designer:wfdid="w54"></asp:Label></TD><TD><asp:TextBox id="txtdondate" runat="server" Width="100px" __designer:wfdid="w55"></asp:TextBox> </TD><TD style="WIDTH: 3px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD><TD><asp:Label id="lbldondaybuild" runat="server" Width="82px" Text="Building name" __designer:wfdid="w56"></asp:Label></TD><TD><asp:DropDownList id="cmbdondaybuild" runat="server" Width="150px" Height="22px" __designer:wfdid="w57" DataValueField="build_id" DataTextField="buildingname"></asp:DropDownList></TD><TD><asp:LinkButton id="lnkdonreportbuildingwise" onclick="lnkdonreportbuildingwise_Click" runat="server" Width="150px" Font-Bold="True" __designer:wfdid="w58">Pass Allocation Report</asp:LinkButton></TD></TR><TR><TD colSpan=5><asp:LinkButton id="lnlpasstilldate" onclick="lnlpasstilldate_Click" runat="server" Width="262px" Font-Bold="True" __designer:wfdid="w59">Donor Wise pass utlilization till this date</asp:LinkButton></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkpassutilizationdate" onclick="lnkpassutilizationdate_Click" runat="server" Width="180px" Font-Bold="True" __designer:wfdid="w60">Pass Utilization for this date</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlDonorwithpass" runat="server" Width="100%" GroupingText="Donor with Pass Details" __designer:wfdid="w1" Visible="False"><asp:GridView id="dtgDonorName" runat="server" Width="100%" ForeColor="#333333" __designer:wfdid="w6" CellPadding="4" GridLines="None">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView><asp:GridView id="dtgDonorPassDetails" runat="server" Width="100%" ForeColor="#333333" __designer:wfdid="w8" CellPadding="4" GridLines="None">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel></TD></TR>             
    <TR><TD><asp:Panel id="pnlotherreport" runat="server" Width="100%" GroupingText="Other Report" __designer:wfdid="w67"><TABLE style="WIDTH: 723px"><TBODY><TR>
        <TD width="20%">Building Name</TD><TD width="20%">
        <asp:DropDownList ID="ddlbilding" runat="server" __designer:wfdid="w112" 
            AutoPostBack="True" DataTextField="buildingname" DataValueField="build_id" 
            Height="22px" OnSelectedIndexChanged="cmbbuildroomstat_SelectedIndexChanged" 
            Width="150px">
        </asp:DropDownList>
        </TD><TD width="5%">Date</TD><TD>
        <asp:TextBox ID="txtbdate" runat="server" __designer:wfdid="w6" Width="145px"></asp:TextBox>
        <cc1:CalendarExtender ID="txtbdate_CalendarExtender" runat="server" 
            __designer:wfdid="w7" Format="dd-MM-yyyy" TargetControlID="txtbdate">
        </cc1:CalendarExtender>
        <asp:LinkButton ID="lnkroomstatus" runat="server" onclick="lnkroomstatus_Click">Room 
        status</asp:LinkButton>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkroomstatus0" runat="server" 
            onclick="lnkroomstatus0_Click">.Excel</asp:LinkButton>
        </TD></TR>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td width="5%">
                &nbsp;</td>
            <td>
                <asp:LinkButton ID="lnkPlainPaperReceiptReport" runat="server" 
                    __designer:wfdid="w68" CausesValidation="False" Font-Bold="True" 
                    Font-Size="Small" onclick="lnkPlainPaperReceiptReport_Click" tabIndex="45" 
                    Visible="False" Width="176px">Plain paper receipt report</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td width="5%">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td width="5%">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td width="5%">
            </td>
            <td>
                &nbsp;</td>
        </tr>
        </TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="WIDTH: 100px"></TD></TR><TR><TD><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Button id="btnBack" onclick="btnBack_Click" runat="server" Text="Back" Font-Bold="True" __designer:wfdid="w135" CssClass="btnStyle_small"></asp:Button>&nbsp;</TD><TD style="WIDTH: 100px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD><TD style="WIDTH: 100px"><asp:Button id="btnhide" onclick="btnhide_Click" runat="server" Text="Hide Report" __designer:wfdid="w138" CssClass="btnStyle_medium"></asp:Button></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:Button id="btnclear" onclick="btnclear_Click" runat="server" Text="Clear" Font-Bold="True" __designer:wfdid="w70" CssClass="btnStyle_medium"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE><BR /><BR /><BR /><BR /><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel8" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="Label22" runat="server" Text="Tsunami ARMS - Confirmation" Font-Bold="True" ForeColor="MediumBlue"></asp:Label></asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize"></ajaxToolkit:ModalPopupExtender>&nbsp; <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button>&nbsp; <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD style="WIDTH: 227px; HEIGHT: 18px" align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 15px"></TD><TD style="WIDTH: 208px; HEIGHT: 15px"></TD><TD style="WIDTH: 13px; HEIGHT: 15px"></TD></TR><TR><TD style="HEIGHT: 26px"></TD><TD style="WIDTH: 208px; HEIGHT: 26px" align=center>&nbsp; &nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="WIDTH: 13px; HEIGHT: 26px" align=center>&nbsp;</TD></TR><TR><TD style="HEIGHT: 18px"></TD><TD style="WIDTH: 208px; HEIGHT: 18px" align=center></TD><TD style="WIDTH: 13px; HEIGHT: 18px" align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD style="WIDTH: 224px" align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp; &nbsp; &nbsp;<asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel> <ajaxToolkit:CalendarExtender id="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtfromd"></ajaxToolkit:CalendarExtender> <ajaxToolkit:CalendarExtender id="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txttod"></ajaxToolkit:CalendarExtender> <ajaxToolkit:CalendarExtender id="CalendarExtender3" runat="server" Format="dd/MM/yyyy" TargetControlID="txtdate"></ajaxToolkit:CalendarExtender> <ajaxToolkit:CalendarExtender id="CalendarExtender4" runat="server" __designer:wfdid="w33" Format="dd/MM/yyyy" TargetControlID="txtdondate"></ajaxToolkit:CalendarExtender> <ajaxToolkit:CalendarExtender id="CalendarExtender5" runat="server" __designer:wfdid="w7" Format="dd-MM-yyyy" TargetControlID="txtTo"></ajaxToolkit:CalendarExtender> <asp:TextBox id="txtFrom" runat="server" __designer:wfdid="w8" Visible="False"></asp:TextBox> 
</contenttemplate>
 <Triggers>
  <asp:PostBackTrigger ControlID="lnksecuritydepositledger" />
    <asp:PostBackTrigger ControlID="lnkDonorFreeRoomAllocationReport" />
    <asp:PostBackTrigger ControlID="btnYes" />
    <asp:PostBackTrigger ControlID="btnNo" />   
    <asp:PostBackTrigger ControlID="lnkVacantRoomRepor" />
    <asp:PostBackTrigger ControlID="lnktotalallocseasonreport" />
    <asp:PostBackTrigger ControlID="lnkDonorPaidRoomAllocationReport" />
    <asp:PostBackTrigger ControlID="lnkPlainPaperReceiptReport" />
     <asp:PostBackTrigger ControlID="lnkpassutilization" />
      <asp:PostBackTrigger ControlID="lnkunutilizedpass" />       
     <asp:PostBackTrigger ControlID="lnkocccuroomreport" />
     <asp:PostBackTrigger ControlID="lnkblockedroom" />
      <asp:PostBackTrigger ControlID="lnkvacant24hour" />
      <asp:PostBackTrigger ControlID="lnknonoccureserverooms" />
        <asp:PostBackTrigger ControlID="lnkoverstayedreport" />
        <asp:PostBackTrigger ControlID="lnkmutiallocatereport" />
        <asp:PostBackTrigger ControlID="lnkExtendreport" />        
     <asp:PostBackTrigger ControlID="lnknonvecating" /> 
     <asp:PostBackTrigger ControlID="lnksecurityledger" /> 
      <asp:PostBackTrigger ControlID="lnkunclaimeddeposit" /> 
      <asp:PostBackTrigger ControlID="lnkoverstayledgerreport" /> 
      <asp:PostBackTrigger ControlID="lnkcompletestatus" /> 
       <asp:PostBackTrigger ControlID="lnkdonreportbuildingwise" /> 
       <asp:PostBackTrigger ControlID="lnkpassutilizationdate" /> 
        <asp:PostBackTrigger ControlID="lnlpasstilldate" /> 
      <asp:PostBackTrigger ControlID="lnkroomdamageledger" /> 
       <asp:PostBackTrigger ControlID="lnkkweylostledger" /> 
     <asp:PostBackTrigger ControlID="lnkDonorDetails" /> 
          <asp:PostBackTrigger ControlID="lnkVacantExcel" /> 
          <asp:PostBackTrigger ControlID="lnkOverStayExcel" /> 
           <asp:PostBackTrigger ControlID="lnkOverStauExcel" /> 
          <asp:PostBackTrigger ControlID="lnkExtendExcel" /> 
         <asp:PostBackTrigger ControlID="lnkVacateRoomExcel" />
         <asp:PostBackTrigger ControlID="lnkMultiExcel" />
         <asp:PostBackTrigger ControlID="lnkNonVacatExcel" />
          <asp:PostBackTrigger ControlID="lnkBlockExcel" />
          <asp:PostBackTrigger ControlID="lnkDonPass" />          
          <asp:PostBackTrigger ControlID="lnkAccLedBetExcel" />
          <asp:PostBackTrigger ControlID="lnkAccLedExcel" />
          <asp:PostBackTrigger ControlID="lnkUnClSecLedExcel" />
          <asp:PostBackTrigger ControlID="lnkSecLedExcel" />
          <asp:PostBackTrigger ControlID="lnkOverStayledExcel" />
          <asp:PostBackTrigger ControlID="lnkDamLedExcel" />
           <asp:PostBackTrigger ControlID="keyLostExcel" />   
            <asp:PostBackTrigger ControlID="LinkButton3" />
             <asp:PostBackTrigger ControlID="LinkButton2" />
              <asp:PostBackTrigger ControlID="LinkButton4" />   
               <asp:PostBackTrigger ControlID="LinkButton5"/>    
               <asp:PostBackTrigger ControlID="btndown"/>       
               <asp:PostBackTrigger ControlID="lnkAccLedgerTime" />   
                <asp:PostBackTrigger ControlID="lnktimeExcel" />    
                  <asp:PostBackTrigger ControlID="lnkAccLedgerBuild" />   
                    <asp:PostBackTrigger ControlID="lnkroomExcel" />   
                    <asp:PostBackTrigger ControlID="lb_ledger" />
                     <asp:PostBackTrigger ControlID="lnkroomstatus" />
                      <asp:PostBackTrigger ControlID="lnkroomstatus0" />
                                                 
  </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

