<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Donor Paid Room Allocation.aspx.cs" Inherits="Donor_Paid_Room_Allocation" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
<style>
         .modalBackground
{
background-color: Gray;
filter: alpha(opacity=80);
opacity: 0.8;
z-index: 10000;
}
</style>
 <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
 <p>
        <br />
        This form is used for room allocation for Donor paid pass.<br />
        <br />
        </asp:Panel>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE><TBODY><TR align=center><TD colSpan=3><asp:Panel id="pnlcash" runat="server" GroupingText="Counter Status" Enabled="False"><TABLE><TBODY><TR>
    <td>
        <asp:Label ID="lblstaff" runat="server" Text="Staff Name:" Width="69px"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="txtstaffname" runat="server" ReadOnly="true" Width="66px"></asp:TextBox>
    </td>
    <td>
        <asp:Label ID="Label4" runat="server" Text="Cashier Liability" Width="93px"></asp:Label>
    </td>
    <TD>
        <asp:TextBox ID="txtcashierliability" runat="server" Font-Bold="True" 
            Font-Size="Small" tabIndex="53" Width="66px"></asp:TextBox>
    </TD><TD style="WIDTH: 100px" align="left">
        <asp:Label ID="Label3" runat="server" Text="Receipt no:" Width="69px"></asp:Label>
    </td><td style="WIDTH: 100px">
        <asp:TextBox ID="txtreceiptno1" runat="server" 
            OnTextChanged="txtreceiptno1_TextChanged" tabIndex="51" Width="66px"></asp:TextBox>
    <TD style="WIDTH: 100px"><asp:Label id="Label1" runat="server" Width="122px" Text="No of Transactions"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtnooftrans" tabIndex=55 runat="server" Width="66px"></asp:TextBox></TD> <td style="WIDTH: 100px">
                        <asp:Label ID="Label19" runat="server" Text="Uncliamed " Width="103px"></asp:Label>
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:TextBox ID="txtunclaimed" runat="server" Font-Bold="True" 
                            Font-Size="Small" tabIndex="54" Width="66px"></asp:TextBox>
                    </td>
    </TR><tr><td style="WIDTH: 100px">
        <asp:Label ID="Label2" runat="server" Text="Login Time:" Width="69px"></asp:Label>
        </td><td style="WIDTH: 100px">
            <asp:TextBox ID="txtlogintime" runat="server" ReadOnly="true" Width="66px"></asp:TextBox>
        
        </td><td align="left">
        <asp:Label ID="Label10" runat="server" Text="Today&#39;s Collection" Width="69px"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtcounterliability" runat="server" Width="66px"></asp:TextBox>
        </td>
         <TD style="WIDTH: 100px">
             <asp:Label ID="Label101" runat="server" Text="Balance Receipt" Width="97px"></asp:Label>
        </td><td style="WIDTH: 100px">
            <asp:TextBox ID="txtreceiptno2" runat="server" 
                OnTextChanged="txtreceiptno2_TextChanged" tabIndex="52" Width="66px"></asp:TextBox>
        </td>      
        <td style="WIDTH: 100px"><asp:Label id="Label18" runat="server" Width="103px" Text="Security Deposit"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txttotsecurity" tabIndex=54 runat="server" Width="66px" Font-Bold="True" Font-Size="Small"></asp:TextBox></TD>
        <td style="WIDTH: 100px">
                        <asp:Label ID="Label20" runat="server" Text="Counter Deposit" Width="103px"></asp:Label>
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:TextBox ID="txtcounterdeposit" runat="server" Font-Bold="True" 
                            Font-Size="Small" tabIndex="54" Width="66px"></asp:TextBox>
                    </td>
    </tr></TBODY></TABLE></asp:Panel></TD></TR><TR align=center><TD id="TD1" colSpan=3>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label id="lblhead" runat="server" Text="DONOR PAID  ROOM ALLOCATION" Font-Bold="True" CssClass="heading" Font-Size="Medium"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:CheckBox id="chkplainpaper" runat="server" Width="153px" Text="Old receipt" OnCheckedChanged="chkplainpaper_CheckedChanged" AutoPostBack="True"></asp:CheckBox> <asp:GridView id="donorgrid" runat="server" Width="840px" ForeColor="#333333" Visible="False" OnSelectedIndexChanged="donorgrid_SelectedIndexChanged" GridLines="None" CellPadding="4">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns><asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField></Columns>
<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left"></RowStyle>
<EditRowStyle BackColor="#2461BF"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True" VerticalAlign="Top"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> &nbsp; </TD></TR><TR align=center><TD colSpan=3><asp:Panel id="donorallocpanel" runat="server" GroupingText="Donor Allocation"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="Label17" runat="server" Text="Barcode"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtdonortype" tabIndex=1 runat="server" Width="110px" AutoPostBack="True" OnTextChanged="txtdonortype_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 100px"><asp:Label id="lbldonorpass" runat="server" Width="56px" Text="Pass no"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtdonorpass" tabIndex=2 runat="server" Width="70px" AutoPostBack="True" OnTextChanged="txtdonorpass_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 100px"><asp:Label id="lblstatus" runat="server" Width="106px" ForeColor="Gold" Font-Bold="True" BackColor="Red" Font-Size="Small"></asp:Label></TD><TD style="WIDTH: 100px"><asp:Label id="Label5" runat="server" Width="90px" Text="Donor Name"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtdonorname" tabIndex=3 runat="server" Width="190px"></asp:TextBox></TD><TD><asp:Button id="btnpass" onclick="btnpass_Click" runat="server" CausesValidation="False" Text="Add pass" Font-Bold="True" BackColor="#8080FF" UseSubmitBehavior="False"></asp:Button></TD></TR><TR><TD colSpan=4><asp:Label id="Label102" runat="server" Text="Reserve No." __designer:wfdid="w4"></asp:Label> <asp:TextBox id="txtReserveNo" runat="server" Width="200px" Height="17px" AutoPostBack="True" __designer:wfdid="w3" ontextchanged="txtReserveNo_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD></TD></TR></TBODY></TABLE></asp:Panel> <asp:Label id="lblreceipt" runat="server" Text="Ledger No"></asp:Label><asp:TextBox id="txtreceipt" runat="server" Width="103px" AutoPostBack="True" OnTextChanged="txtreceipt_TextChanged"></asp:TextBox></TD></TR><TR><TD><asp:Panel id="swamipanel" runat="server" Height="1%" GroupingText="Swami Details"><TABLE><TBODY><TR><TD style="WIDTH: 162px; HEIGHT: 2px"><asp:Label id="lblswaminame" runat="server" Width="78px" Text="Swami name" Font-Bold="False"></asp:Label></TD><TD style="WIDTH: 284px; HEIGHT: 2px"><asp:TextBox id="txtswaminame" tabIndex=5 runat="server" Width="200px" Height="17px" CssClass="UpperCaseFirstLetter" AutoPostBack="True" OnTextChanged="txtswaminame_TextChanged1"></asp:TextBox></TD><TD></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 23px"><asp:Label id="Label12" runat="server" Width="82px" Text="Place"></asp:Label></TD><TD style="WIDTH: 284px; HEIGHT: 23px"><asp:TextBox id="txtplace" tabIndex=6 runat="server" Width="200px" Height="17px" CssClass="UpperCaseFirstLetter" AutoPostBack="True" OnTextChanged="txtplace_TextChanged"></asp:TextBox> </TD><TD><BR /></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 14px"><asp:Label id="lblstate" runat="server" Width="79px" Text="State"></asp:Label></TD><TD style="WIDTH: 284px; HEIGHT: 14px"><asp:DropDownList id="cmbState" tabIndex=20 runat="server" Width="205px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbState_SelectedIndexChanged" AppendDataBoundItems="True" DataTextField="statename" DataValueField="state_id"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD><BR /></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 26px"><asp:Label id="Label11" runat="server" Width="80px" Text="District"></asp:Label> <BR /></TD><TD style="WIDTH: 284px; HEIGHT: 26px"><asp:DropDownList id="cmbDists" tabIndex=20 runat="server" Width="205px" Height="22px" OnSelectedIndexChanged="cmbDists_SelectedIndexChanged" DataTextField="districtname" DataValueField="district_id">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                </asp:DropDownList> </TD><TD style="HEIGHT: 26px"><asp:LinkButton id="lnkdistrict" onclick="lnkdistrict_Click" runat="server" CausesValidation="False">New</asp:LinkButton> </TD></TR><TR><TD style="WIDTH: 162px"><asp:Label id="lblphone" runat="server" Width="79px" Text="Phone"></asp:Label></TD><TD style="WIDTH: 284px"><asp:TextBox id="txtphone" tabIndex=20 runat="server" Width="200px" Height="17px" OnTextChanged="txtphone_TextChanged"></asp:TextBox> </TD><TD><BR /></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 31px"><asp:Label id="lblidproof" runat="server" Width="79px" Text="Identity proof"></asp:Label></TD><TD style="WIDTH: 284px; HEIGHT: 31px"><asp:DropDownList id="cmbIDp" tabIndex=20 runat="server" Width="205px" Height="22px">
                <asp:ListItem>--Select--</asp:ListItem>
                <asp:ListItem>Election ID</asp:ListItem>
                <asp:ListItem>Driving License</asp:ListItem>
                <asp:ListItem>Pass Port</asp:ListItem>
                <asp:ListItem>Other</asp:ListItem>
            </asp:DropDownList> </TD><TD><BR /></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 20px"><asp:Label id="Label8" runat="server" Width="83px" Text="Identity ref: no"></asp:Label> </TD><TD style="WIDTH: 284px; HEIGHT: 20px"><asp:TextBox id="txtidrefno" tabIndex=20 runat="server" Width="200px" Height="17px" OnTextChanged="txtidrefno_TextChanged" EnableTheming="True"></asp:TextBox> </TD><TD></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 9px"></TD><TD style="WIDTH: 284px; HEIGHT: 9px"></TD><TD></TD></TR></TBODY></TABLE></asp:Panel> </TD><TD><asp:Panel id="roomallocationpanel" runat="server" Height="1%" GroupingText="Room Allocation Details"><TABLE><TBODY><TR><TD><asp:Label id="lblnoofinmates" runat="server" Width="85px" Text="No: of Inmates"></asp:Label></TD><TD><asp:TextBox id="txtnoofinmates" tabIndex=12 runat="server" Width="170px" Height="17px" AutoPostBack="True" OnTextChanged="TextBox5_TextChanged"></asp:TextBox></TD><TD><BR /></TD></TR><TR><TD><asp:Label style="POSITION: relative" id="lblProposedCheckOutDate0" runat="server" Width="92px" Text="Check out date"></asp:Label> </TD><TD><asp:TextBox style="POSITION: relative" id="txtcheckout" tabIndex=16 runat="server" Width="170px" Height="17px" AutoPostBack="True" OnTextChanged="txtcheckout_TextChanged"></asp:TextBox> <cc1:CalendarExtender id="txtcheckout_CalendarExtender" runat="server" TargetControlID="txtcheckout" Format="dd/MM/yyyy">
        </cc1:CalendarExtender> </TD><TD></TD></TR><TR><TD><asp:Label style="POSITION: relative" id="lblcheckouttime0" runat="server" Width="90px" Text="Check out time"></asp:Label> </TD><TD><asp:TextBox style="LEFT: 0px; POSITION: relative; TOP: 0px" id="txtcheckouttime" tabIndex=17 runat="server" Width="170px" Height="17px" AutoPostBack="True" OnTextChanged="txtcheckouttime_TextChanged"></asp:TextBox> </TD><TD></TD></TR><TR><TD><asp:Label style="LEFT: 0px; POSITION: relative; TOP: 3px" id="Label100" runat="server" Width="95px" Text="No of days(hrs)" __designer:wfdid="w2"></asp:Label></TD><TD><asp:TextBox style="LEFT: 0px; POSITION: relative; TOP: 5px" id="txtnoofdays" tabIndex=15 runat="server" Width="170px" Height="17px" AutoPostBack="True" OnTextChanged="txtnoofdays_TextChanged" __designer:wfdid="w1" ReadOnly="True"></asp:TextBox></TD><TD></TD></TR><TR><TD><asp:Label id="lblbuildingname" runat="server" Width="87px" Text="Building name"></asp:Label></TD><TD><asp:DropDownList id="cmbBuild" tabIndex=13 runat="server" Width="175px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbBuild_SelectedIndexChanged" DataTextField="buildingname" DataValueField="build_id"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD><BR /></TD></TR><TR><TD><asp:Label id="lbroomno" runat="server" Width="74px" Text="Room no"></asp:Label></TD><TD><asp:DropDownList id="cmbRooms" tabIndex=14 runat="server" Width="175px" AutoPostBack="True" OnSelectedIndexChanged="cmbRooms_SelectedIndexChanged" DataTextField="roomno" DataValueField="room_id"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD><BR /></TD></TR><TR><TD><asp:Label style="LEFT: 2px; POSITION: relative; TOP: 0px" id="lblcheckindate0" runat="server" Width="85px" Text="Check in date"></asp:Label> </TD><TD><asp:TextBox style="LEFT: 0px; POSITION: relative" id="txtcheckindate" tabIndex=57 runat="server" Width="170px" Height="17px" Enabled="False" AutoPostBack="True" OnTextChanged="txtcheckindate_TextChanged"></asp:TextBox> <cc1:CalendarExtender id="txtcheckindate_CalendarExtender" runat="server" TargetControlID="txtcheckindate" Format="dd/MM/yyyy">
            </cc1:CalendarExtender> </TD><TD>&nbsp;</TD></TR><TR><TD><asp:Label style="POSITION: relative" id="lblcheckintime1" runat="server" Width="85px" Text="Check in time"></asp:Label> </TD><TD><asp:TextBox style="POSITION: relative" id="txtcheckintime" tabIndex=56 runat="server" Width="170px" Height="17px" Enabled="False" AutoPostBack="True" OnTextChanged="txtcheckintime_TextChanged"></asp:TextBox> </TD><TD>&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> </TD><TD><asp:Panel id="rentpanel" runat="server" Height="1%" GroupingText="Rent"><TABLE><TBODY><TR><TD style="WIDTH: 115px; HEIGHT: 26px"><asp:Label id="lblroomrent" runat="server" Width="73px" Text="Room rent"></asp:Label></TD><TD style="WIDTH: 92px; HEIGHT: 26px"><asp:TextBox id="txtroomrent" tabIndex=16 runat="server" Width="100px" Height="17px" Font-Bold="True" Enabled="False" OnTextChanged="txtroomrent_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 92px; HEIGHT: 26px"><BR /></TD></TR><TR><TD style="WIDTH: 115px; HEIGHT: 24px"><asp:Label id="lblsecuritydeposit" runat="server" Width="97px" Text="Security deposit"></asp:Label></TD><TD style="WIDTH: 92px; HEIGHT: 24px"><asp:TextBox id="txtsecuritydeposit" tabIndex=17 runat="server" Width="100px" Height="17px" Font-Bold="True" Enabled="False"></asp:TextBox></TD><TD style="WIDTH: 92px; HEIGHT: 24px"><BR /></TD></TR><TR><TD style="WIDTH: 115px; HEIGHT: 26px"><asp:Label id="Label7" runat="server" Width="77px" Text="Other charge"></asp:Label></TD><TD style="WIDTH: 92px; HEIGHT: 26px"><asp:TextBox id="txtothercharge" tabIndex=19 runat="server" Width="100px" Height="17px" Font-Bold="True" AutoPostBack="True" OnTextChanged="txtothercharge_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 92px; HEIGHT: 26px"><BR /></TD></TR><TR><TD style="WIDTH: 115px; HEIGHT: 18px"><asp:Label id="lbltotalamount" runat="server" Width="80px" Text="Total amount"></asp:Label></TD><TD style="WIDTH: 92px; HEIGHT: 18px"><asp:TextBox id="txttotalamount" tabIndex=21 runat="server" Width="100px" Height="33px" ForeColor="OliveDrab" Font-Bold="True" Enabled="False" Font-Size="X-Large" Wrap="False"></asp:TextBox></TD><TD style="WIDTH: 92px; HEIGHT: 18px"></TD></TR><TR><TD style="WIDTH: 115px; HEIGHT: 18px">Amount Received</TD><TD style="WIDTH: 92px; HEIGHT: 18px"><asp:TextBox id="txtadvance" tabIndex=20 runat="server" Width="100px" Height="17px" OnTextChanged="txtadvance_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 92px; HEIGHT: 18px"></TD></TR><TR><TD style="WIDTH: 115px; HEIGHT: 18px"><B>Balance Payable</B></TD><TD style="WIDTH: 92px; HEIGHT: 18px"><asp:TextBox id="txtnetpayment" tabIndex=21 runat="server" Width="100px" Height="17px"></asp:TextBox> </TD><TD style="WIDTH: 92px; HEIGHT: 18px">&nbsp;</TD></TR><TR>
    <TD style="WIDTH: 115px; HEIGHT: 18px">Inmates charge</TD>
    <TD style="WIDTH: 92px; HEIGHT: 18px">
        <asp:TextBox ID="txtinmatecharge" runat="server" Height="17px" 
            OnTextChanged="txtadvance_TextChanged" tabIndex="20" Width="90px"></asp:TextBox>
    </TD><TD style="WIDTH: 92px; HEIGHT: 18px">&nbsp;</TD></TR>
    <tr>
        <td style="WIDTH: 115px; HEIGHT: 18px">
            <asp:Label ID="Label104" runat="server" Text="Inmate Deposit"></asp:Label>
        </td>
        <td style="WIDTH: 92px; HEIGHT: 18px">
            <asp:TextBox ID="txtinmatedeposit" runat="server" Height="17px" 
                OnTextChanged="txtadvance_TextChanged" tabIndex="20" Width="100px"></asp:TextBox>
        </td>
        <td style="WIDTH: 92px; HEIGHT: 18px">
            &nbsp;</td>
    </tr>
    <tr>
        <td style="WIDTH: 115px; HEIGHT: 17px">
            <asp:Label ID="Label6" runat="server" Text="Grant total"></asp:Label>
        </td>
        <td style="WIDTH: 92px; HEIGHT: 17px">
            <asp:TextBox ID="txtgranttotal" runat="server" Enabled="False" Font-Bold="True" 
                Font-Size="X-Large" ForeColor="#FF3300" Height="33px" tabIndex="22" 
                Width="100px"></asp:TextBox>
        </td>
        <td style="WIDTH: 92px; HEIGHT: 17px">
        </td>
    </tr>
    </TBODY></TABLE></asp:Panel> </TD></TR><TR><TD align=center colSpan=3><TABLE><TBODY><TR><TD><asp:Button id="btnallocate" tabIndex=18 onclick="btnallocate_Click" runat="server" Text="Allocate" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button></TD><TD><asp:Button id="btneditcash" tabIndex=26 onclick="btneditcash_Click" runat="server" CausesValidation="False" Text="Edit" Font-Bold="True" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnadd" tabIndex=20 onclick="btnadd_Click" runat="server" Text="Add" Font-Bold="True" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnclear" tabIndex=19 onclick="btnclear_Click" runat="server" CausesValidation="False" Text="Clear" Font-Bold="True" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnaltroom" tabIndex=21 onclick="btnaltroom_Click" runat="server" CausesValidation="False" Text="Change room" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button></TD><TD><asp:Button id="btncancel" tabIndex=23 onclick="btncancel_Click" runat="server" CausesValidation="False" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button></TD><TD>&nbsp;</TD><TD><asp:Button id="btnreport" tabIndex=24 onclick="btnreport_Click" runat="server" CausesValidation="False" Text="Report View" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button></TD></TR></TBODY></TABLE><TABLE><TBODY><TR><TD><asp:Panel id="pnlalternate" runat="server" Width="100%" GroupingText="Alternate Room"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="Label9" runat="server" Width="82px" Text="New building"></asp:Label></TD><TD style="WIDTH: 99px"><asp:DropDownList id="cmbaltbulilding" tabIndex=28 runat="server" Width="150px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbaltbulilding_SelectedIndexChanged" DataTextField="buildingname" DataValueField="build_id"></asp:DropDownList></TD><TD></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 3px"><asp:Label id="Label13" runat="server" Text="New room"></asp:Label></TD><TD style="WIDTH: 99px; HEIGHT: 3px"><asp:DropDownList id="cmbaltroom" tabIndex=28 runat="server" Width="150px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbaltroom_SelectedIndexChanged" DataTextField="roomno" DataValueField="room_id"></asp:DropDownList></TD><TD></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 3px"><asp:Label id="lblreason" runat="server" Text="Reason"></asp:Label></TD><TD style="WIDTH: 99px; HEIGHT: 3px"><asp:DropDownList id="cmbReason" runat="server" Width="150px" DataTextField="reason" DataValueField="reason_id"></asp:DropDownList></TD><TD></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 99px">&nbsp;</TD><TD><asp:Button id="btnchangeroom" tabIndex=29 onclick="btnchangeroom_Click" runat="server" CausesValidation="False" Text="Change room" Font-Bold="True"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlletter" runat="server" Width="100%" GroupingText="CEO Letter" Visible="False" __designer:wfdid="w3"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblletterbuilding" runat="server" Text="Building name" __designer:wfdid="w8"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbletterbuilding" runat="server" Width="150px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbletterbuilding_SelectedIndexChanged" DataTextField="buildingname" DataValueField="build_id" __designer:wfdid="w10"></asp:DropDownList></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblletterroom" runat="server" Text="Room no" __designer:wfdid="w9"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbletterroom" runat="server" Width="150px" Height="22px" DataTextField="roomno" DataValueField="room_id" __designer:wfdid="w11"></asp:DropDownList></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:Button id="btnletterdetails" runat="server" CausesValidation="False" Text="Show details" Font-Bold="True" __designer:wfdid="w13" OnClick="btnletterdetails_Click"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel>&nbsp;&nbsp; </TD><TD vAlign=top><asp:Panel id="userpanel" runat="server" Width="100%" GroupingText="User Allocation Panel" BackColor="#C0C0FF"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:Label id="Label15" runat="server" Width="66px" Text="User name"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:TextBox id="txtuname" tabIndex=33 runat="server"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:Label id="Label16" runat="server" Text="Password"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:TextBox id="txtupass" tabIndex=34 runat="server" TextMode="Password"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 26px"></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:Button id="btnsubmit" tabIndex=35 onclick="btnsubmit_Click" runat="server" Width="100px" CausesValidation="False" Font-Bold="True" Text="SUBMIT"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> </TD></TR></TBODY></TABLE></TD></TR><TR><TD align=center colSpan=3><asp:GridView id="gdroomallocation" runat="server" Width="840px" ForeColor="#333333" OnSelectedIndexChanged="gdroomallocation_SelectedIndexChanged" GridLines="None" CellPadding="4" PageSize="5" DataKeyNames="id" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Caption="gridview" OnPageIndexChanging="gdroomallocation_PageIndexChanging" OnRowCreated="gdroomallocation_RowCreated" OnSorting="gdroomallocation_Sorting">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="id" Visible="False" HeaderText="id"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="Room No" HeaderText="Room No"></asp:BoundField>
<asp:BoundField DataField="Inmates" HeaderText="Inmates"></asp:BoundField>
<asp:BoundField DataField="Area" HeaderText="Area"></asp:BoundField>
<asp:BoundField DataField="Rent" HeaderText="Rent"></asp:BoundField>
</Columns>
<RowStyle BackColor="#EFF3FB" Width="50px" BorderColor="Black" HorizontalAlign="Left"></RowStyle>
<EditRowStyle BackColor="#2461BF" BorderColor="Black"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True" VerticalAlign="Middle"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> <asp:GridView id="gdDonor" runat="server" Width="840px" ForeColor="#333333" OnSelectedIndexChanged="gdDonor_SelectedIndexChanged" GridLines="None" CellPadding="4" PageSize="5" DataKeyNames="id" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gdDonor_PageIndexChanging" OnRowCreated="gdDonor_RowCreated">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="id" Visible="False" HeaderText="id"></asp:BoundField>
<asp:BoundField DataField="Pass No" HeaderText="Pass No"></asp:BoundField>
<asp:BoundField DataField="PassType" HeaderText="PassType"></asp:BoundField>
<asp:BoundField DataField="Donor Name" HeaderText="Donor Name"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="Room" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="ResStatus" HeaderText="ResStatus"></asp:BoundField>
</Columns>
<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left"></RowStyle>
<EditRowStyle BackColor="#2461BF"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True" VerticalAlign="Top"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView><asp:GridView id="gdalloc" runat="server" Width="840px" ForeColor="#333333" OnSelectedIndexChanged="gdalloc_SelectedIndexChanged" GridLines="None" CellPadding="4" DataKeyNames="id" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gdalloc_PageIndexChanging" OnRowCreated="gdalloc_RowCreated">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="id" Visible="False" HeaderText="id"></asp:BoundField>
<asp:BoundField DataField="No" HeaderText="No"></asp:BoundField>
<asp:BoundField DataField="Reciept" HeaderText="Reciept"></asp:BoundField>
<asp:BoundField DataField="Swami Name" HeaderText="Swami Name"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="Room" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="Alloc Date" HeaderText="Alloc Date"></asp:BoundField>
<asp:BoundField DataField="Vecate Date" HeaderText="Vecate Date"></asp:BoundField>
<asp:BoundField DataField="Rent" HeaderText="Rent"></asp:BoundField>
<asp:BoundField DataField="Deposit" HeaderText="Deposit"></asp:BoundField>
<asp:BoundField DataField="Amt" HeaderText="Amt"></asp:BoundField>
</Columns>
<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left"></RowStyle>
<EditRowStyle BackColor="#2461BF"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> <asp:GridView id="gdletter" runat="server" Width="840px" ForeColor="#333333" GridLines="None" CellPadding="4" __designer:wfdid="w14">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<RowStyle BackColor="#EFF3FB"></RowStyle>
<EditRowStyle BackColor="#2461BF"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView>&nbsp;</TD></TR><TR><TD style="HEIGHT: 744px" vAlign=top align=center colSpan=3>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Panel id="Panel1" runat="server" Width="100%" Height="50px"><TABLE width="100%"><TBODY><TR><TD style="WIDTH: 111px"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="86px" ForeColor="White" ControlToValidate="txtswaminame" ErrorMessage="Name required" SetFocusOnError="True"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator1">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" Width="84px" ForeColor="White" ControlToValidate="txtswaminame" ErrorMessage="Only alphabet" SetFocusOnError="True" ValidationExpression="[a-z A-Z . ]{1,25}"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RegularExpressionValidator3">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 41px"><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Width="141px" ForeColor="White" ControlToValidate="cmbBuild" ErrorMessage="Building name required" SetFocusOnError="True"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px; HEIGHT: 41px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator2">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Width="114px" ForeColor="White" ControlToValidate="cmbRooms" ErrorMessage="Room no required" SetFocusOnError="True"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator3">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RegularExpressionValidator id="RegularExpressionValidator4" runat="server" Width="125px" ForeColor="White" ControlToValidate="txtphone" ErrorMessage="Only Numbers(1-10)" SetFocusOnError="True" ValidationExpression="[0-9]{1,10}"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" TargetControlID="RegularExpressionValidator4">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" ForeColor="White" ControlToValidate="txtnoofdays" ErrorMessage="No days required" SetFocusOnError="True"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredFieldValidator5">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px">&nbsp;<asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" ForeColor="White" ControlToValidate="txtnoofinmates" ErrorMessage="No of inmates required"></asp:RequiredFieldValidator> <asp:Panel id="Panel9" runat="server">
            </asp:Panel> </TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" TargetControlID="RequiredFieldValidator6"></cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 18px"><asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ForeColor="White" ControlToValidate="txtcheckindate" ErrorMessage="DD/MM/YYYY" SetFocusOnError="True" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator> </TD><TD style="WIDTH: 111px; HEIGHT: 18px"><cc1:ListSearchExtender id="ListSearchExtender1" runat="server" TargetControlID="cmbState" IsSorted="True"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender2" runat="server" TargetControlID="cmbDists"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender3" runat="server" TargetControlID="cmbBuild"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender4" runat="server" TargetControlID="cmbRooms"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender5" runat="server" TargetControlID="cmbaltbulilding"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender6" runat="server" TargetControlID="cmbaltroom"></cc1:ListSearchExtender></TD></TR><TR><TD style="WIDTH: 111px"><asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" Width="82px" ForeColor="White" ControlToValidate="txtcheckout" ErrorMessage="DD/MM/YYYY" SetFocusOnError="True" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 111px"></TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 17px">
        &nbsp;</TD><TD style="WIDTH: 111px; HEIGHT: 17px">&nbsp;</TD></TR>
        <tr>
            <td style="WIDTH: 111px; HEIGHT: 17px">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="txtnoofdays" Display="Dynamic" 
                    ErrorMessage="Enter no of days" ForeColor="White" SetFocusOnError="True" 
                    Width="107px"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="HEIGHT: 17px">
                <asp:UpdateProgress ID="UpdateProgress" runat="server">
                    <progresstemplate>
                        <asp:Image ID="Image1" runat="server" AlternateText="Processing" 
                            ImageUrl="Images/waiting.gif" />
                    </progresstemplate>
                </asp:UpdateProgress>
                <cc1:ModalPopupExtender ID="modalPopup" runat="server" 
                    BackgroundCssClass="modalBackground" PopupControlID="UpdateProgress" 
                    TargetControlID="UpdateProgress" />
            </td>
        </tr>
        <TR><TD style="WIDTH: 111px; HEIGHT: 17px"><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel8" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="Label22" runat="server" Width="238px" ForeColor="MediumBlue" Text="Tsunami ARMS - Confirmation" Font-Bold="True"></asp:Label>
            </asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" TargetControlID="btnHidden" RepositionMode="RepositionOnWindowResize" PopupControlID="pnlMessage"></ajaxToolkit:ModalPopupExtender>&nbsp; <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button>&nbsp; <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD style="WIDTH: 227px; HEIGHT: 18px" align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 15px"></TD><TD style="WIDTH: 208px; HEIGHT: 15px"></TD><TD style="WIDTH: 13px; HEIGHT: 15px"></TD></TR><TR><TD style="HEIGHT: 26px"></TD><TD style="WIDTH: 208px; HEIGHT: 26px" align=center>&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="WIDTH: 13px; HEIGHT: 26px" align=center>&nbsp;</TD></TR><TR><TD style="HEIGHT: 18px"></TD><TD style="WIDTH: 208px; HEIGHT: 18px" align=center></TD><TD style="WIDTH: 13px; HEIGHT: 18px" align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD style="WIDTH: 224px" align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlAbnormal" runat="server" Width="100%" GroupingText="Abnormal history" BackColor="#C0C0FF" __designer:wfdid="w3"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="Label21" runat="server" Text="Inmates name" __designer:wfdid="w8"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtAbnormal" runat="server" Width="160px"></asp:TextBox> </TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="Label107" runat="server" Text="Abnormal Type"></asp:Label> </TD><TD style="WIDTH: 100px"><asp:DropDownList id="ddlAbnormal" runat="server" Width="162px" DataTextField="abnormal_type" DataValueField="id">
                    </asp:DropDownList> </TD><TD style="WIDTH: 100px">&nbsp;</TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="Label23" runat="server" Text="Remarks" __designer:wfdid="w9"></asp:Label> </TD><TD style="WIDTH: 100px"><asp:TextBox id="txtRemarks" runat="server" Width="162px" TextMode="MultiLine"></asp:TextBox> </TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px" align=center colSpan=3><asp:Button id="btnAb" onclick="btnAb_Click" runat="server" CausesValidation="False" Text="Save Abnormality" CssClass="btnStyle"></asp:Button> </TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel> <asp:Label id="Label14" runat="server" Text="Reason" Visible="False"></asp:Label> <asp:TextBox id="txtreson" tabIndex=19 runat="server" Width="100px" Height="17px" Visible="False" OnTextChanged="TextBox2_TextChanged" Wrap="False"></asp:TextBox> <asp:Button id="btnreallocate" tabIndex=22 onclick="btnreallocate_Click" runat="server" CausesValidation="False" Font-Bold="True" CssClass="btnStyle_medium" Visible="False"></asp:Button> </TD><TD style="WIDTH: 111px; HEIGHT: 17px"></TD></TR></TBODY></TABLE></asp:Panel>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<BR />&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE><IFRAME style="WIDTH: 200px; HEIGHT: 200px" id="frame1" runat="server" visible="true"></IFRAME>
</contenttemplate>
 <Triggers>
  <asp:PostBackTrigger ControlID="btnOk" />
   </Triggers>
  </asp:UpdatePanel>    
    <br />
    <br />
 <script type="text/javascript">     function ClearLastMessage(elem) {
         $get(elem).innerHTML = '';
     } 
</script>
    <br />
    <br />
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
        prm.add_beginRequest(BeginRequestHandler);
        // Raised after an asynchronous postback is finished and control has been returned to the browser.
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            //Shows the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();

            }
        }

        function EndRequestHandler(sender, args) {
            //Hide the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
        function openNewWindows1() {

            window.open("onlinehelp.aspx");
        }
        function openNewWindows2() {

            window.open("important steps.aspx");
        }
        $(function () {
            $(document).tooltip({
                track: true
            });
        });
  

</script>  
</asp:Content>

