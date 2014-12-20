<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="roomallocation.aspx.cs" Inherits="roomallocation" Title="Room Allocation" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentplaceholder3" Runat="Server">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        <p>
            Use&nbsp; <strong>Tab Key </strong>or <strong>Mouse Click</strong>,
        To go to the Next Field. &nbsp;</p>
        <p>
            Use <strong>Mouse </strong>&nbsp;to select Data from the grid.</p>
        <p>
            Press <strong>Alloc Type</strong> Button for selecting the allocation 
            type.(General/Donor/TDB)</p>
        <p>
            Use <strong>Edit &nbsp;</strong>button for editting cashier details, check in 
            date and time).</p>
        <p>
            Use <strong>Report </strong>button to view Reports.</p>
        <p>
            Press <strong>View Alloc </strong>button to View Allocation.</p>
        <p>
            Press <strong>Allocate </strong>button for saving an allocation and print 
            receipt after enter all mandatory fields.</p>
        <p>
            Use <strong>Add </strong>button for allocate multiple room.</p>
        <p>
            Use<strong> AltRoom </strong>button for changing room in case of donor allocation.</p>
        <p>
            Use<strong> Reallocate</strong> button for Reallocation/Changing room in general
            allocation.</p>
    </asp:Panel>
    <strong><span style="text-decoration: underline"></span></strong>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE><TBODY><TR align=center><TD colSpan=3><asp:Panel id="pnlcash" runat="server" GroupingText="Cashier liability" Enabled="False"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="Label3" runat="server" Width="69px" Text="Receipt no:"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtreceiptno1" tabIndex=51 runat="server" Width="66px" OnTextChanged="txtreceiptno1_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 100px"><asp:Label id="Label10" runat="server" Width="97px" Text="Balance Receipt"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtreceiptno2" tabIndex=52 runat="server" Width="66px" OnTextChanged="txtreceiptno2_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 100px"><asp:Label id="Label4" runat="server" Width="93px" Text="Cashier Liability"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtcashierliability" tabIndex=53 runat="server" Width="66px" Font-Bold="True" Font-Size="Small"></asp:TextBox></TD><TD style="WIDTH: 100px"><asp:Label id="Label18" runat="server" Width="103px" Text="Security Deposit"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txttotsecurity" tabIndex=54 runat="server" Width="66px" Font-Bold="True" Font-Size="Small"></asp:TextBox></TD><TD style="WIDTH: 100px"><asp:Label id="Label1" runat="server" Width="122px" Text="No of Transactions"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtnooftrans" tabIndex=55 runat="server" Width="66px"></asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR align=center><TD id="TD1" colSpan=3>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label id="lblhead" runat="server" Text="ALLOCATION" Font-Bold="True" Font-Size="Medium" CssClass="heading"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:CheckBox id="chkplainpaper" runat="server" Width="153px" Text="Old receipt" AutoPostBack="True" OnCheckedChanged="chkplainpaper_CheckedChanged"></asp:CheckBox> <asp:GridView id="donorgrid" runat="server" Width="840px" ForeColor="#333333" CellPadding="4" GridLines="None" OnSelectedIndexChanged="donorgrid_SelectedIndexChanged">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
</Columns>

<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True" VerticalAlign="Top"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> &nbsp; </TD></TR><TR align=center><TD colSpan=3><asp:Panel id="donorallocpanel" runat="server" GroupingText="Donor Allocation"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="Label2" runat="server" Width="60px" Text="Pass type"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbPassType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbPassType_SelectedIndexChanged"><asp:ListItem Value="-1">--Select--</asp:ListItem>
<asp:ListItem Value="0">Free Pass</asp:ListItem>
<asp:ListItem Value="1">Paid Pass</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px"><asp:Label id="Label17" runat="server" Text="B.C"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtdonortype" tabIndex=1 runat="server" Width="70px" AutoPostBack="True" OnTextChanged="txtdonortype_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 100px"><asp:Label id="lbldonorpass" runat="server" Width="56px" Text="Pass no"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtdonorpass" tabIndex=2 runat="server" Width="70px" AutoPostBack="True" OnTextChanged="txtdonorpass_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 100px"><asp:Label id="lblstatus" runat="server" Width="106px" ForeColor="Gold" Font-Bold="True" BackColor="Red" Font-Size="Small"></asp:Label></TD><TD style="WIDTH: 100px"><asp:Label id="Label5" runat="server" Width="42px" Text="Donor"></asp:Label></TD><TD style="WIDTH: 100px">
        <asp:TextBox id="txtdonorname" tabIndex=4 runat="server" Width="190px"             ></asp:TextBox></TD><TD><asp:Button id="btnpass" onclick="btnpass_Click" runat="server" CausesValidation="False" Text="Add pass" Font-Bold="True" BackColor="#8080FF" UseSubmitBehavior="False"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> <asp:Label id="lblreceipt" runat="server" Text="Receipt No"></asp:Label><asp:TextBox id="txtreceipt" runat="server" Width="103px" AutoPostBack="True" OnTextChanged="txtreceipt_TextChanged"></asp:TextBox></TD></TR><TR><TD><asp:Panel id="swamipanel" runat="server" Height="1%" GroupingText="Swami Details"><TABLE><TBODY><TR><TD style="WIDTH: 162px; HEIGHT: 2px"><asp:Label id="lblswaminame" runat="server" Width="78px" Text="Swami name" Font-Bold="False"></asp:Label></TD><TD style="WIDTH: 284px; HEIGHT: 2px"><asp:TextBox id="txtswaminame" tabIndex=5 runat="server" Width="200px" Height="17px" CssClass="UpperCaseFirstLetter" AutoPostBack="True" OnTextChanged="txtswaminame_TextChanged1"></asp:TextBox></TD><TD></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 23px"><asp:Label id="Label12" runat="server" Width="82px" Text="Place"></asp:Label></TD><TD style="WIDTH: 284px; HEIGHT: 23px"><asp:TextBox id="txtplace" tabIndex=6 runat="server" Width="200px" Height="17px" CssClass="UpperCaseFirstLetter" AutoPostBack="True" OnTextChanged="txtplace_TextChanged"></asp:TextBox></TD><TD><BR /></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 14px"><asp:Label id="lblstate" runat="server" Width="79px" Text="State"></asp:Label></TD><TD style="WIDTH: 284px; HEIGHT: 14px"><asp:DropDownList id="cmbState" tabIndex=20 runat="server" Width="205px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbState_SelectedIndexChanged" DataValueField="state_id" DataTextField="statename" AppendDataBoundItems="True"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD><BR /></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 26px"><asp:Label id="Label11" runat="server" Width="80px" Text="District"></asp:Label><BR /></TD><TD style="WIDTH: 284px; HEIGHT: 26px"><asp:DropDownList id="cmbDists" tabIndex=20 runat="server" Width="205px" Height="22px" OnSelectedIndexChanged="cmbDists_SelectedIndexChanged" DataValueField="district_id" DataTextField="districtname"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD style="HEIGHT: 26px"><asp:LinkButton id="lnkdistrict" onclick="lnkdistrict_Click" runat="server" CausesValidation="False">New</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 162px"><asp:Label id="lblphone" runat="server" Width="79px" Text="Phone"></asp:Label></TD><TD style="WIDTH: 284px"><asp:TextBox id="txtphone" tabIndex=20 runat="server" Width="200px" Height="17px" OnTextChanged="txtphone_TextChanged"></asp:TextBox></TD><TD><BR /></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 31px"><asp:Label id="lblidproof" runat="server" Width="79px" Text="Identity proof"></asp:Label></TD><TD style="WIDTH: 284px; HEIGHT: 31px">
    <asp:DropDownList id="cmbIDp" tabIndex=20 runat="server" Width="205px" 
        Height="22px"><asp:ListItem>--Select--</asp:ListItem>
<asp:ListItem>Election ID</asp:ListItem>
<asp:ListItem>Driving License</asp:ListItem>
<asp:ListItem>Pass Port</asp:ListItem>
<asp:ListItem>Other</asp:ListItem>
</asp:DropDownList></TD><TD><BR /></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 20px"><asp:Label id="Label8" runat="server" Width="83px" Text="Identity ref: no"></asp:Label></TD><TD style="WIDTH: 284px; HEIGHT: 20px"><asp:TextBox id="txtidrefno" tabIndex=20 runat="server" Width="200px" Height="17px" OnTextChanged="txtidrefno_TextChanged" EnableTheming="True"></asp:TextBox></TD><TD></TD></TR><TR><TD style="WIDTH: 162px; HEIGHT: 9px"></TD><TD style="WIDTH: 284px; HEIGHT: 9px"></TD><TD></TD></TR></TBODY></TABLE></asp:Panel> </TD><TD><asp:Panel id="roomallocationpanel" runat="server" Height="1%" GroupingText="Room Allocation Details"><TABLE><TBODY><TR><TD><asp:Label id="lblnoofinmates" runat="server" Width="85px" Text="No: of Inmates"></asp:Label></TD><TD><asp:TextBox id="txtnoofinmates" tabIndex=12 runat="server" Width="170px" Height="17px" AutoPostBack="True" OnTextChanged="TextBox5_TextChanged"></asp:TextBox></TD><TD><BR /></TD></TR><TR><TD><asp:Label id="lblbuildingname" runat="server" Width="87px" Text="Building name"></asp:Label></TD><TD><asp:DropDownList id="cmbBuild" tabIndex=13 runat="server" Width="175px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbBuild_SelectedIndexChanged" DataTextField="buildingname" DataValueField="build_id"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD><BR /></TD></TR><TR><TD><asp:Label id="lbroomno" runat="server" Width="74px" Text="Room no"></asp:Label></TD><TD><asp:DropDownList id="cmbRooms" tabIndex=14 runat="server" Width="175px" AutoPostBack="True" OnSelectedIndexChanged="cmbRooms_SelectedIndexChanged" DataTextField="roomno" DataValueField="room_id"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD><BR /></TD></TR><TR><TD><asp:Label style="LEFT: 2px; POSITION: relative; TOP: 0px" id="lblcheckindate" runat="server" Width="85px" Text="Check in date" __designer:wfdid="w6"></asp:Label></TD><TD><asp:TextBox style="LEFT: 0px; POSITION: relative" id="txtcheckindate" tabIndex=57 runat="server" Width="170px" Height="17px" Enabled="False" AutoPostBack="True" OnTextChanged="txtcheckindate_TextChanged"></asp:TextBox></TD><TD></TD></TR><TR><TD><asp:Label style="POSITION: relative" id="lblcheckintime" runat="server" Width="85px" Text="Check in time"></asp:Label></TD><TD><asp:TextBox style="POSITION: relative" id="txtcheckintime" tabIndex=56 runat="server" Width="170px" Height="17px" Enabled="False" AutoPostBack="True" OnTextChanged="txtcheckintime_TextChanged"></asp:TextBox></TD><TD></TD></TR><TR><TD><asp:Label style="POSITION: relative" id="lblProposedCheckOutDate" runat="server" Width="92px" Text="Check out date"></asp:Label></TD><TD><asp:TextBox style="POSITION: relative" id="txtcheckout" tabIndex=16 runat="server" Width="170px" Height="17px" AutoPostBack="True" OnTextChanged="txtcheckout_TextChanged"></asp:TextBox></TD><TD><BR /></TD></TR><TR><TD><asp:Label style="POSITION: relative" id="lblcheckouttime" runat="server" Width="90px" Text="Check out time"></asp:Label></TD><TD><asp:TextBox style="LEFT: 0px; POSITION: relative" id="txtcheckouttime" tabIndex=17 runat="server" Width="170px" Height="17px" AutoPostBack="True" OnTextChanged="txtcheckouttime_TextChanged"></asp:TextBox></TD><TD>&nbsp;</TD></TR><TR><TD><asp:Label style="LEFT: 0px; POSITION: relative; TOP: 3px" id="Label100" runat="server" Width="95px" Text="No of days(hrs)" __designer:wfdid="w2"></asp:Label></TD><TD><asp:TextBox style="LEFT: 0px; POSITION: relative; TOP: 0px" id="txtnoofdays" tabIndex=15 runat="server" Width="170px" Height="17px" AutoPostBack="True" OnTextChanged="txtnoofdays_TextChanged" __designer:wfdid="w1" ReadOnly="True"></asp:TextBox></TD><TD></TD></TR></TBODY></TABLE></asp:Panel> </TD><TD><asp:Panel id="rentpanel" runat="server" Height="1%" GroupingText="Rent"><TABLE><TBODY><TR><TD style="WIDTH: 94px; HEIGHT: 26px"><asp:Label id="lblroomrent" runat="server" Width="73px" Text="Room rent"></asp:Label></TD><TD style="WIDTH: 92px; HEIGHT: 26px"><asp:TextBox id="txtroomrent" tabIndex=16 runat="server" Width="100px" Height="17px" Font-Bold="True" Enabled="False" OnTextChanged="txtroomrent_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 92px; HEIGHT: 26px"><BR /></TD></TR><TR><TD style="WIDTH: 94px"><asp:Label id="lblsecuritydeposit" runat="server" Width="97px" Text="Security deposit"></asp:Label></TD><TD style="WIDTH: 92px"><asp:TextBox id="txtsecuritydeposit" tabIndex=17 runat="server" Width="100px" Height="17px" Font-Bold="True" Enabled="False"></asp:TextBox></TD><TD style="WIDTH: 92px"><BR /></TD></TR><TR><TD style="WIDTH: 94px; HEIGHT: 26px"><asp:Label id="Label7" runat="server" Width="77px" Text="Other charge"></asp:Label></TD><TD style="WIDTH: 92px; HEIGHT: 26px"><asp:TextBox id="txtothercharge" tabIndex=19 runat="server" Width="100px" Height="17px" Font-Bold="True" AutoPostBack="True" OnTextChanged="txtothercharge_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 92px; HEIGHT: 26px"><BR /></TD></TR><TR><TD style="WIDTH: 94px; HEIGHT: 18px"><asp:Label id="Label14" runat="server" Text="Reason"></asp:Label></TD><TD style="WIDTH: 92px; HEIGHT: 18px"><asp:TextBox id="txtreson" tabIndex=19 runat="server" Width="100px" Height="17px" OnTextChanged="TextBox2_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 92px; HEIGHT: 18px"><BR /></TD></TR><TR><TD style="WIDTH: 94px; HEIGHT: 18px">Advance</TD><TD style="WIDTH: 92px; HEIGHT: 18px"><asp:TextBox id="txtadvance" tabIndex=20 runat="server" Width="100px" Height="17px" OnTextChanged="txtadvance_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 92px; HEIGHT: 18px"></TD></TR><TR><TD style="WIDTH: 94px; HEIGHT: 18px"><asp:Label id="lbltotalamount" runat="server" Width="80px" Text="Total amount"></asp:Label></TD><TD style="WIDTH: 92px; HEIGHT: 18px"><asp:TextBox id="txttotalamount" tabIndex=21 runat="server" Width="100px" Height="33px" ForeColor="OliveDrab" Font-Bold="True" Enabled="False" Font-Size="X-Large" Wrap="False"></asp:TextBox></TD><TD style="WIDTH: 92px; HEIGHT: 18px"></TD></TR><TR><TD style="WIDTH: 94px; HEIGHT: 17px"><asp:Label id="Label6" runat="server" Text="Grant total" Visible="False"></asp:Label></TD><TD style="WIDTH: 92px; HEIGHT: 17px"><asp:TextBox id="txtgranttotal" tabIndex=22 runat="server" Width="100px" Height="33px" ForeColor="OliveDrab" Font-Bold="True" Enabled="False" Font-Size="X-Large" Visible="False"></asp:TextBox></TD><TD style="WIDTH: 92px; HEIGHT: 17px"></TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD align=center colSpan=3><TABLE><TBODY><TR><TD><asp:Button id="btnallocate" tabIndex=18 onclick="btnallocate_Click" runat="server" Text="Allocate" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button></TD><TD><asp:Button id="btnadd" tabIndex=20 onclick="btnadd_Click" runat="server" Text="Add" Font-Bold="True" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnclear" tabIndex=19 onclick="btnclear_Click" runat="server" CausesValidation="False" Text="Clear" Font-Bold="True" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnaltroom" tabIndex=21 onclick="btnaltroom_Click" runat="server" CausesValidation="False" Text="Alt room" Font-Bold="True" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnreallocate" tabIndex=22 onclick="btnreallocate_Click" runat="server" CausesValidation="False" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button></TD><TD><asp:Button id="btncancel" tabIndex=23 onclick="btncancel_Click" runat="server" CausesValidation="False" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button></TD><TD><asp:Button id="btnreport" tabIndex=24 onclick="btnreport_Click" runat="server" CausesValidation="False" Text="Report View" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button></TD><TD><asp:Button id="btntype" tabIndex=25 onclick="btntype_Click" runat="server" CausesValidation="False" Text="Alloc type" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button></TD><TD><asp:Button id="btneditcash" tabIndex=26 onclick="btneditcash_Click" runat="server" CausesValidation="False" Text="Edit" Font-Bold="True" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnsave" tabIndex=58 onclick="btnsave_Click2" runat="server" CausesValidation="False" Text="Save" Font-Bold="True" CssClass="btnStyle_small"></asp:Button></TD></TR></TBODY></TABLE><TABLE><TBODY><TR><TD><asp:Panel id="pnlalternate" runat="server" Width="100%" GroupingText="Alternate Room"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="Label9" runat="server" Width="82px" Text="New building"></asp:Label></TD><TD style="WIDTH: 99px"><asp:DropDownList id="cmbaltbulilding" tabIndex=28 runat="server" Width="150px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbaltbulilding_SelectedIndexChanged" DataTextField="buildingname" DataValueField="build_id"></asp:DropDownList></TD><TD></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 3px"><asp:Label id="Label13" runat="server" Text="New room"></asp:Label></TD><TD style="WIDTH: 99px; HEIGHT: 3px"><asp:DropDownList id="cmbaltroom" tabIndex=28 runat="server" Width="150px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbaltroom_SelectedIndexChanged" DataTextField="roomno" DataValueField="room_id"></asp:DropDownList></TD><TD></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 3px"><asp:Label id="lblreason" runat="server" Text="Reason"></asp:Label></TD><TD style="WIDTH: 99px; HEIGHT: 3px"><asp:DropDownList id="cmbReason" runat="server" Width="150px" DataTextField="reason" DataValueField="reason_id"></asp:DropDownList></TD><TD></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 99px"></TD><TD><asp:Button id="btnchangeroom" tabIndex=29 onclick="btnchangeroom_Click" runat="server" CausesValidation="False" Text="Change room" Font-Bold="True"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlletter" runat="server" Width="100%" GroupingText="CEO Letter" Visible="False" __designer:wfdid="w3"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblletterbuilding" runat="server" Text="Building name" __designer:wfdid="w8"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbletterbuilding" runat="server" Width="150px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbletterbuilding_SelectedIndexChanged" DataTextField="buildingname" DataValueField="build_id" __designer:wfdid="w10"></asp:DropDownList></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblletterroom" runat="server" Text="Room no" __designer:wfdid="w9"></asp:Label></TD><TD style="WIDTH: 100px">
    <asp:DropDownList id="cmbletterroom" runat="server" Width="150px" Height="22px" 
        DataTextField="roomno" DataValueField="room_id" __designer:wfdid="w11"></asp:DropDownList></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:Button id="btnletterdetails" runat="server" CausesValidation="False" Text="Show details" Font-Bold="True" __designer:wfdid="w13" OnClick="btnletterdetails_Click"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel>&nbsp;&nbsp; </TD><TD><asp:Panel id="pnlalloctype" runat="server" Width="100%" GroupingText="Alloc Type" BackColor="Olive"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Button id="btngeneralallocation" tabIndex=30 onclick="btngeneralallocation_Click" runat="server" Width="100px" CausesValidation="False" Text="GENERAL" Font-Bold="True" __designer:wfdid="w4"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="btntdballocation" tabIndex=31 onclick="btntdballocation_Click" runat="server" Width="100px" CausesValidation="False" Text="TDB" Font-Bold="True" __designer:wfdid="w5"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="btndonorallocation" tabIndex=32 onclick="btndonorallocation_Click" runat="server" Width="100px" CausesValidation="False" Text="DONOR" Font-Bold="True" __designer:wfdid="w6"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="btnletter" onclick="btnletter_Click" runat="server" Width="100px" CausesValidation="False" Text="CEO Letter" Font-Bold="True" Enabled="False" __designer:wfdid="w7"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="userpanel" runat="server" Width="100%" GroupingText="User Allocation Panel" BackColor="#C0C0FF"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:Label id="Label15" runat="server" Width="66px" Text="User name"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:TextBox id="txtuname" tabIndex=33 runat="server"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:Label id="Label16" runat="server" Text="Password"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:TextBox id="txtupass" tabIndex=34 runat="server" TextMode="Password"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 26px"></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:Button id="btnsubmit" tabIndex=35 onclick="btnsubmit_Click" runat="server" Width="100px" CausesValidation="False" Font-Bold="True" Text="SUBMIT"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> </TD></TR></TBODY></TABLE></TD></TR><TR><TD align=center colSpan=3><asp:GridView id="gdroomallocation" runat="server" Width="840px" ForeColor="#333333" CellPadding="4" GridLines="None" OnSelectedIndexChanged="gdroomallocation_SelectedIndexChanged" OnSorting="gdroomallocation_Sorting" OnRowCreated="gdroomallocation_RowCreated" OnPageIndexChanging="gdroomallocation_PageIndexChanging" Caption="gridview" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="id" PageSize="5">
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
</asp:GridView> <asp:GridView id="gdDonor" runat="server" Width="840px" ForeColor="#333333" CellPadding="4" GridLines="None" OnSelectedIndexChanged="gdDonor_SelectedIndexChanged" OnRowCreated="gdDonor_RowCreated" OnPageIndexChanging="gdDonor_PageIndexChanging" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="id" PageSize="5">
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
</asp:GridView><asp:GridView id="gdtdb" runat="server" Width="840px" ForeColor="#333333" CellPadding="4" GridLines="None" OnSelectedIndexChanged="gdtdb_SelectedIndexChanged" OnRowCreated="gdtdb_RowCreated" OnPageIndexChanging="gdtdb_PageIndexChanging" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="resid" PageSize="5">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="resid" Visible="False" HeaderText="resid"></asp:BoundField>
<asp:BoundField DataField="resid" HeaderText="Reserve No"></asp:BoundField>
<asp:BoundField DataField="Swami Name" HeaderText="Swami Name"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="Room" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="ReserveDate" HeaderText="ReserveDate"></asp:BoundField>
<asp:BoundField DataField="VacateDate" HeaderText="VacateDate"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True" VerticalAlign="Middle"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView><asp:GridView id="gdalloc" runat="server" Width="840px" ForeColor="#333333" CellPadding="4" GridLines="None" OnSelectedIndexChanged="gdalloc_SelectedIndexChanged" OnRowCreated="gdalloc_RowCreated" OnPageIndexChanging="gdalloc_PageIndexChanging" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="id">
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
</asp:GridView> <asp:GridView id="gdletter" runat="server" Width="840px" ForeColor="#333333" CellPadding="4" GridLines="None" __designer:wfdid="w14">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView>&nbsp;</TD></TR><TR><TD style="HEIGHT: 744px" vAlign=top align=center colSpan=3>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Panel id="Panel1" runat="server" Width="100%" Height="50px"><TABLE width="100%"><TBODY><TR><TD style="WIDTH: 111px"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="86px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Name required" ControlToValidate="txtswaminame"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator1">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" Width="84px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Only alphabet" ControlToValidate="txtswaminame" ValidationExpression="[a-z A-Z . ]{1,25}"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RegularExpressionValidator3">
                                </cc1:ValidatorCalloutExtender> <cc1:CalendarExtender id="CalendarExtender2" runat="server" TargetControlID="txtcheckindate" Format="dd/MM/yyyy">
                </cc1:CalendarExtender> <cc1:CalendarExtender id="CalendarExtender1" runat="server" TargetControlID="txtcheckout" Format="dd/MM/yyyy">
                </cc1:CalendarExtender> </TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 41px"><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Width="141px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Building name required" ControlToValidate="cmbBuild"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px; HEIGHT: 41px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator2">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Width="114px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Room no required" ControlToValidate="cmbRooms"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator3">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RegularExpressionValidator id="RegularExpressionValidator4" runat="server" Width="125px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Only Numbers(1-10)" ControlToValidate="txtphone" ValidationExpression="[0-9]{1,10}"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" TargetControlID="RegularExpressionValidator4">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" ForeColor="White" SetFocusOnError="True" ErrorMessage="No days required" ControlToValidate="txtnoofdays"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredFieldValidator5">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px">&nbsp;<asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" ForeColor="White" ErrorMessage="No of inmates required" ControlToValidate="txtnoofinmates"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" TargetControlID="RequiredFieldValidator6"></cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 18px"><asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ForeColor="White" SetFocusOnError="True" ErrorMessage="DD/MM/YYYY" ControlToValidate="txtcheckindate" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator> </TD><TD style="WIDTH: 111px; HEIGHT: 18px"><cc1:ListSearchExtender id="ListSearchExtender1" runat="server" TargetControlID="cmbState" IsSorted="True"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender2" runat="server" TargetControlID="cmbDists"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender3" runat="server" TargetControlID="cmbBuild"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender4" runat="server" TargetControlID="cmbRooms"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender5" runat="server" TargetControlID="cmbaltbulilding"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender6" runat="server" TargetControlID="cmbaltroom"></cc1:ListSearchExtender></TD></TR><TR><TD style="WIDTH: 111px"><asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" Width="82px" ForeColor="White" SetFocusOnError="True" ErrorMessage="DD/MM/YYYY" ControlToValidate="txtcheckout" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 111px"></TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 17px"><asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" Width="107px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Enter no of days" ControlToValidate="txtnoofdays" Display="Dynamic"></asp:RequiredFieldValidator> </TD><TD style="WIDTH: 111px; HEIGHT: 17px">&nbsp;</TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 17px"><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel8" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="Label22" runat="server" Width="238px" ForeColor="MediumBlue" Text="Tsunami ARMS - Confirmation" Font-Bold="True"></asp:Label></asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize"></ajaxToolkit:ModalPopupExtender>&nbsp; <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button>&nbsp; <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD style="WIDTH: 227px; HEIGHT: 18px" align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 15px"></TD><TD style="WIDTH: 208px; HEIGHT: 15px"></TD><TD style="WIDTH: 13px; HEIGHT: 15px"></TD></TR><TR><TD style="HEIGHT: 26px"></TD><TD style="WIDTH: 208px; HEIGHT: 26px" align=center>&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="WIDTH: 13px; HEIGHT: 26px" align=center>&nbsp;</TD></TR><TR><TD style="HEIGHT: 18px"></TD><TD style="WIDTH: 208px; HEIGHT: 18px" align=center></TD><TD style="WIDTH: 13px; HEIGHT: 18px" align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD style="WIDTH: 224px" align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel></TD><TD style="WIDTH: 111px; HEIGHT: 17px"></TD></TR></TBODY></TABLE></asp:Panel>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<BR />&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE><IFRAME style="WIDTH: 200px; HEIGHT: 200px" id="frame1" runat="server" visible="true"></IFRAME>
</contenttemplate>

  <Triggers>
  <asp:PostBackTrigger ControlID="btnOk" />
   </Triggers>


    </asp:UpdatePanel>
    
    <br />
    <br />
 <script type="text/javascript"> function ClearLastMessage(elem) 
{ 
$get(elem).innerHTML = ''; 
} 
</script>
    <br />
    <br />
</asp:Content>

