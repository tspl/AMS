<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Room Allocation Policy.aspx.cs" 
Inherits="Room_Allocation_Policy" Title="Tsunami ARMS - Room Allocation Policy" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server"> 
        <asp:Panel ID="Panel3" runat="server" GroupingText="User tips" Width="100%">
            <br />
            This policy used for set policy for different allocation type.<br />
            <br />
            common policy is common to all policy type.<br />
            <br />
            save button is used to save the policy details.<br />
            <br />
            report button has two link buttons. One &nbsp;for current policy and another one
            for policy history.<br />
            <br />
            Select all in combo, display whole details. If select each allocation type, report
            generated for selected type.</asp:Panel>
    
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    

<div>
    <table>
       <tr>
            <td colspan="2" valign="top" style="width: 711px">
                <asp:ScriptManager id="ScriptManager1" runat="server">
                </asp:ScriptManager><asp:UpdatePanel id="UpdatePanel1" runat="server"><contenttemplate>
<TABLE><TBODY><TR><TD style="TEXT-ALIGN: center" vAlign=top colSpan=2>&nbsp;<asp:Label id="Label1" runat="server" Width="244px" ForeColor="DarkBlue" Text="Room Allocation Policy" Font-Bold="True" Font-Size="16pt"></asp:Label></TD></TR><TR><TD vAlign=top rowSpan=3><asp:Panel id="Panel4" runat="server" Width="100%" GroupingText="Request Details"><TABLE><TBODY><TR><TD style="WIDTH: 104px"><asp:Label id="lblallocationrequest" runat="server" Width="188px" Font-Bold="False" Font-Size="Small" Text="Allocation Request:" ForeColor="#0000C0"></asp:Label></TD><TD style="WIDTH: 127px"><asp:DropDownList id="cmbAllocationRequest" runat="server" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="cmbAllocationRequest_SelectedIndexChanged1"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem>Common</asp:ListItem>
<asp:ListItem>Donor Free Allocation</asp:ListItem>
<asp:ListItem>Donor Paid Allocation</asp:ListItem>
<asp:ListItem>Donor multiple pass</asp:ListItem>
<asp:ListItem>General Allocation</asp:ListItem>
<asp:ListItem>TDB Allocation</asp:ListItem>
</asp:DropDownList></TD></TR></TBODY></TABLE><asp:Panel id="pnlrequest" runat="server" Width="100%"><TABLE><TBODY><TR><TD><asp:Label id="lblrequestseniority" runat="server" Width="178px" Text="Request Seniority"></asp:Label></TD><TD><asp:DropDownList id="cmbRequestSeniority" runat="server" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="cmbRequestSeniority_SelectedIndexChanged1"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem>0</asp:ListItem>
<asp:ListItem>1</asp:ListItem>
<asp:ListItem>2</asp:ListItem>
<asp:ListItem>3</asp:ListItem>
<asp:ListItem>4</asp:ListItem>
<asp:ListItem>5</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD><asp:Label id="lblmaxallocation" runat="server" Width="187px" Text="Maximum no of days allocation possible"></asp:Label></TD><TD><asp:TextBox id="txtMaxAllocation" runat="server" Width="146px"></asp:TextBox></TD></TR><TR><TD vAlign=top>
        <asp:Label id="lblmultipleroom" runat="server" Width="157px" 
            Text="Duration Type"></asp:Label></TD><TD vAlign=top>
        <asp:DropDownList id="cmbMultipleRoom" runat="server" Width="150px" 
            AutoPostBack="True" 
            OnSelectedIndexChanged="cmbMultipleRoom_SelectedIndexChanged1" 
            DataTextField="unitname" DataValueField="service_unit_id" Enabled="False"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
<asp:ListItem Value="0">No</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="HEIGHT: 26px"><asp:Label id="lblnoofrooms" 
                runat="server" Width="157px" Text="Duration"></asp:Label></TD><TD style="HEIGHT: 26px">
            <asp:TextBox id="txtNoofRooms" runat="server" Width="146px" 
                OnTextChanged="txtNoofRooms_TextChanged1" ReadOnly="True"></asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel><TABLE><TBODY><TR><TD style="WIDTH: 188px" vAlign=top><asp:Label id="lblallocationcancellation" runat="server" Width="142px" Text="Allocation cancellation"></asp:Label></TD><TD style="HEIGHT: 24px" vAlign=top><asp:DropDownList id="cmbAllocationCancellation" runat="server" Width="150px"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
<asp:ListItem Value="0">No</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 188px"><asp:Label id="lblexecutiveoverride" runat="server" Width="120px" Text="Execute over-ride"></asp:Label></TD><TD style="HEIGHT: 22px"><asp:DropDownList id="cmbExecutiveOverride" runat="server" Width="150px"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
<asp:ListItem Value="0">No</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 188px"><asp:Label id="lblwaitingcriteria" runat="server" Width="115px" Text="Waiting criteria"></asp:Label></TD><TD><asp:DropDownList id="cmbWaitingCriteria" runat="server" Width="150px"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem>Hours</asp:ListItem>
<asp:ListItem>No of Allocation</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 188px" vAlign=top><asp:Label id="Label3" runat="server" Width="115px" Text="No of units">
</asp:Label></TD><TD style="HEIGHT: 3px"><asp:TextBox id="txtNoofUnits" runat="server" Width="146px" Height="18px"></asp:TextBox></TD></TR>
    <tr>
        <td style="WIDTH: 188px" valign="top">
            <asp:Label ID="Label6" runat="server" Text="Past Allocation Check"></asp:Label>
        </td>
        <td style="HEIGHT: 3px">
            <asp:DropDownList ID="ddlpastalloc" runat="server" Width="146px">
                <asp:ListItem Value="0">--select--</asp:ListItem>
                <asp:ListItem Value="1">Yes</asp:ListItem>
                <asp:ListItem Value="2">No</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="WIDTH: 188px" valign="top">
            <asp:Label ID="lblmaxwaitinglist" runat="server" __designer:wfdid="w7" 
                Height="21px" Text="Max graceperiod for check out" Width="183px"></asp:Label>
        </td>
        <td style="HEIGHT: 3px">
            <asp:TextBox ID="txtMaxwaitingList" runat="server" __designer:wfdid="w8" 
                Height="16px" Width="146px"></asp:TextBox>
        </td>
    </tr>
        <tr>
            <td style="WIDTH: 188px" valign="top">
                <asp:Label ID="Label7" runat="server" Text="Grace time"></asp:Label>
            </td>
            <td style="HEIGHT: 3px">
                <asp:TextBox ID="txtgrace_time" runat="server" Height="16px" Width="146px"></asp:TextBox>
            </td>
        </tr>
    <tr>
        <td style="WIDTH: 188px" valign="top">
            Default time</td>
        <td style="HEIGHT: 3px">
            <asp:TextBox ID="txtdtime" runat="server" Height="16px" Width="146px"></asp:TextBox>
        </td>
    </tr>
    </TBODY></TABLE></asp:Panel> <asp:Panel id="pnlcommon" runat="server" Width="100%" GroupingText="Common Policy"><TABLE><TBODY><TR><TD style="WIDTH: 183px" vAlign=top><asp:Label id="Label4" runat="server" Width="154px" ForeColor="#0000C0" Visible="False" Font-Bold="False" Font-Size="Small" Text="Allocation request:" __designer:wfdid="w1"></asp:Label></TD><TD vAlign=top><asp:DropDownList id="cmbAllocation" runat="server" Width="150px" Visible="False" __designer:wfdid="w2"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem>Common</asp:ListItem>
<asp:ListItem>Donor Free Allocation</asp:ListItem>
<asp:ListItem>Donor Paid Allocation</asp:ListItem>
<asp:ListItem>Donor multiple pass</asp:ListItem>
<asp:ListItem>General Allocation</asp:ListItem>
<asp:ListItem>TDB Allocation</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 183px; HEIGHT: 39px" vAlign=top><asp:Label id="lblhousekeeping" runat="server" Width="191px" Height="19px" Text="Should house keeping rooms be shown on room vacant list" __designer:wfdid="w3"></asp:Label></TD><TD vAlign=top><asp:DropDownList id="cmbHouseKeeping" runat="server" Width="150px" __designer:wfdid="w4"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
<asp:ListItem Value="0">No</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 183px; HEIGHT: 18px" vAlign=top><asp:Label id="lblcheckintime" runat="server" Width="144px" Text="Inputting check in time" __designer:wfdid="w5"></asp:Label> </TD><TD style="HEIGHT: 18px"><asp:DropDownList id="cmbCheckinTime" runat="server" Width="150px" __designer:wfdid="w6"><asp:ListItem></asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
<asp:ListItem Value="0">No</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 183px" vAlign=top><asp:Label id="lblextraamount" runat="server" Width="136px" Text="Extra Amount" __designer:wfdid="w9"></asp:Label></TD><TD style="WIDTH: 104px"><asp:DropDownList id="cmbExtraAmount" runat="server" Width="150px" __designer:wfdid="w10"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
<asp:ListItem Value="0">No</asp:ListItem>
</asp:DropDownList></TD></TR></TBODY></TABLE></asp:Panel></TD><TD vAlign=top colSpan=1><asp:Panel id="pnlperiod" runat="server" Width="100%" GroupingText="Policy Period"><TABLE><TBODY><TR><TD style="WIDTH: 139px"><asp:Label id="lblpolicyseasonfrom" runat="server" Width="133px" Text="Applicable Season"></asp:Label></TD><TD><asp:ListBox id="lstSeasons" runat="server" Width="148px" Height="50px"></asp:ListBox></TD></TR><TR><TD style="WIDTH: 139px; HEIGHT: 1px; TEXT-ALIGN: left" vAlign=top><asp:Label id="lblpolicyperiodfrom" runat="server" Width="135px" Height="22px" Text="Policy applicable from"></asp:Label></TD><TD style="FONT-SIZE: 100%; WIDTH: 97px; HEIGHT: 1px" vAlign=top><asp:TextBox id="txtPolicyperiodFrom" runat="server" Width="146px" OnTextChanged="txtPolicyperiodFrom_TextChanged" AutoPostBack="True"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 139px"><asp:Label id="lblpolicyperiodto" runat="server" Width="132px" Text="Policy applicable to"></asp:Label></TD><TD><asp:TextBox id="txtPolicyperiodTo" runat="server" Width="146px" OnTextChanged="txtPolicyperiodTo_TextChanged" AutoPostBack="True"></asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="WIDTH: 195px" vAlign=top></TD></TR><TR><TD vAlign=top><asp:Panel id="pnlrent" runat="server" Width="100%" GroupingText="Rent Details"><TABLE><TBODY><TR><TD vAlign=top><asp:Label id="lblrentapplicable" runat="server" Width="131px" Text="Rent applicable or not"></asp:Label> </TD><TD vAlign=top><asp:DropDownList id="cmbRentApplicable" runat="server" Width="150px"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
<asp:ListItem Value="0">No</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD vAlign=top><asp:Label id="lblreturnrent" runat="server" Width="122px" Text="Return rent">
</asp:Label></TD><TD vAlign=top><asp:DropDownList id="cmbReturnRent" runat="server" Width="150px"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem Value="0">No</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD vAlign=top><asp:Label id="lblsecurityamount" runat="server" Width="142px" Text="Security deposit amount">
</asp:Label></TD><TD vAlign=top><asp:DropDownList id="cmbSecurityDeposit" runat="server" Width="150px"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
<asp:ListItem Value="0">No</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD vAlign=top><asp:Label id="lblreturnsecuritydeposit" runat="server" Width="147px" Text="Return  security deposit"></asp:Label></TD><TD vAlign=top><asp:DropDownList id="cmbReturnsecurityDeposit" runat="server" Width="150px"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
<asp:ListItem Value="0">No</asp:ListItem>
</asp:DropDownList></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="TEXT-ALIGN: center" vAlign=top colSpan=2><TABLE><TBODY><TR><TD><asp:Button id="btnSave" onclick="btnSave_Click" runat="server" Height="25px" Text="Save" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 46px">
        <asp:Button id="btnEdit" onclick="btnEdit_Click" 
            runat="server" Text="Edit" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 39px"><asp:Button id="btnDelete" onclick="btnDelete_Click" runat="server" Height="25px" Text="Delete" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 64px"><asp:Button id="btnClear" onclick="btnClear_Click" runat="server" CausesValidation="False" Text="Clear" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 64px"><asp:Button id="btnPolicy" onclick="btnPolicy_Click1" runat="server" CausesValidation="False" Text="Report" CssClass="btnStyle_small"></asp:Button></TD>
        <td style="WIDTH: 64px">
            <asp:Button id="btnView" runat="server" CssClass="btnStyle_medium" 
                Text="View Grid" CausesValidation="False" 
                onclick="btnView_Click" />
        </td>
        </TR></TBODY></TABLE></TD></TR><TR><TD vAlign=top colSpan=2><asp:Panel id="pnlrep" runat="server">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<BR /><TABLE><TBODY><TR>
    <TD style="HEIGHT: 125px" vAlign=top colSpan=2><asp:Panel id="Panel8" runat="server" GroupingText="Policy History" __designer:wfdid="w14"><TABLE><TBODY><TR><TD><asp:LinkButton id="lnkPolicy" onclick="lnkPolicy_Click" runat="server" Width="127px" CausesValidation="False" __designer:wfdid="w38">Policy history report</asp:LinkButton></TD><TD style="WIDTH: 162px">&nbsp;<asp:DropDownList id="cmbPhistory" runat="server" Width="155px" __designer:wfdid="w39"><asp:ListItem Value="-1">-- Select --</asp:ListItem>
<asp:ListItem Value="All">All</asp:ListItem>
<asp:ListItem>Common</asp:ListItem>
<asp:ListItem>Donor Free Allocation</asp:ListItem>
<asp:ListItem>Donor Paid Allocation</asp:ListItem>
<asp:ListItem>Donor multiple pass</asp:ListItem>
<asp:ListItem>General Allocation</asp:ListItem>
<asp:ListItem>TDB Allocation</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD><asp:Label id="lblfd" runat="server" Width="118px" Height="24px" Text="From date" __designer:wfdid="w40"></asp:Label></TD><TD><asp:TextBox id="txtReportFrom" runat="server" __designer:wfdid="w41"></asp:TextBox></TD></TR><TR><TD><asp:Label id="lbltt" runat="server" Width="112px" Height="25px" Text="To date" __designer:wfdid="w42"></asp:Label></TD><TD><asp:TextBox id="txtReportTo" runat="server" __designer:wfdid="w43"></asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel></TD><TD style="TEXT-ALIGN: left" vAlign=top><asp:Panel id="Panel6" runat="server" Width="275px" GroupingText="Current Policy" __designer:wfdid="w21"><TABLE><TBODY><TR><TD style="WIDTH: 54px; TEXT-ALIGN: right" colSpan=2 rowSpan=1><asp:LinkButton id="lnkCurrent" onclick="lnkCurrent_Click" runat="server" Width="86px" CausesValidation="False" __designer:wfdid="w48">Current policy</asp:LinkButton></TD><TD style="WIDTH: 3px; HEIGHT: 1px" colSpan=1 rowSpan=1><asp:DropDownList id="cmbCurrentPolicy" runat="server" Width="155px" __designer:wfdid="w49"><asp:ListItem Value="-1">-- Select--</asp:ListItem>
<asp:ListItem>All</asp:ListItem>
<asp:ListItem>Common</asp:ListItem>
<asp:ListItem>Donor Free Allocation</asp:ListItem>
<asp:ListItem>Donor Paid Allocation</asp:ListItem>
<asp:ListItem>Donor multiple pass</asp:ListItem>
<asp:ListItem>General Allocation</asp:ListItem>
<asp:ListItem>TDB Allocation</asp:ListItem>
</asp:DropDownList></TD></TR></TBODY></TABLE></asp:Panel>&nbsp;&nbsp;</TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD vAlign=top colSpan=2><asp:Panel id="Panel5" runat="server" Width="100%" GroupingText="Policy Grid" Wrap="False">
        <asp:GridView id="dtgRoomAllocationgrid" runat="server" 
            Width="100%" Height="100%" ForeColor="#333333" 
            OnSelectedIndexChanged="dtgRoomAllocationgrid_SelectedIndexChanged" 
            OnSorting="dtgRoomAllocationgrid_Sorting" 
            OnPageIndexChanging="dtgRoomAllocationgrid_PageIndexChanging" 
            AllowSorting="True" AllowPaging="True" 
            OnRowCreated="dtgRoomAllocationgrid_RowCreated" 
            CellPadding="4" GridLines="None" Visible="False">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel></TD></TR><TR><TD vAlign=top colSpan=2><cc1:CalendarExtender id="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtpolicyperiodfrom"></cc1:CalendarExtender> <cc1:CalendarExtender id="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtpolicyperiodto"></cc1:CalendarExtender> <cc1:CalendarExtender id="CalendarExtender3" runat="server" Format="dd/MM/yyyy" TargetControlID="txtreportfrom"></cc1:CalendarExtender> <cc1:CalendarExtender id="CalendarExtender4" runat="server" Format="dd/MM/yyyy" TargetControlID="txtreportto"></cc1:CalendarExtender><BR /><BR /><asp:Button style="DISPLAY: none" id="btnHidden" onclick="btnHidden_Click" runat="server" Text="Hidden"></asp:Button> <asp:TextBox id="TextBox1" runat="server" Visible="False" OnTextChanged="TextBox1_TextChanged" AutoPostBack="True"></asp:TextBox> <asp:Label id="Label2" runat="server" Text="Label" Visible="False"></asp:Label><BR /><BR /><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel7" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" ForeColor="MediumBlue" Text="Tsunami ARMS -" Font-Bold="True"></asp:Label></asp:Panel> <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel><BR /><cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize">
                </cc1:ModalPopupExtender><BR /><BR /></TD></TR></TBODY></TABLE><asp:Panel id="Panel1" runat="server" Width="125px" Height="50px">&nbsp;&nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RegularExpressionValidator1">
                    </cc1:ValidatorCalloutExtender> <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" Width="188px" ForeColor="White" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$" ErrorMessage="DD/MM/YYYY" ControlToValidate="txtPolicyperiodFrom"></asp:RegularExpressionValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator6">
                    </cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" Width="286px" ForeColor="White" Font-Bold="True" Font-Size="Small" ErrorMessage="Must select a season" ControlToValidate="lstSeasons"></asp:RequiredFieldValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator4">
                    </cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" Width="306px" ForeColor="White" Font-Bold="True" Font-Size="Small" ErrorMessage="must enter from date" ControlToValidate="txtPolicyperiodFrom"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2"></cc1:ValidatorCalloutExtender> <asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" Width="327px" ForeColor="White" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$" ErrorMessage="DD/YY/MMMM" ControlToValidate="txtPolicyperiodTo"></asp:RegularExpressionValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredFieldValidator5">
                    </cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" Width="321px" ForeColor="White" Font-Bold="True" Font-Size="Small" Enabled="False" ErrorMessage="must enter" ControlToValidate="cmbWaitingCriteria"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" TargetControlID="RequiredFieldValidator13">
                    </cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator13" runat="server" Width="263px" ForeColor="White" Font-Bold="True" Font-Size="Small" Enabled="False" ErrorMessage="must enter" ControlToValidate="txtNoofUnits" ValidationGroup="pnlcancellation"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" TargetControlID="RegularExpressionValidator3">
                    </cc1:ValidatorCalloutExtender> <asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" Width="117px" ForeColor="White" ValidationExpression="\d{1,2}" ErrorMessage="Number" ControlToValidate="txtMaxAllocation"></asp:RegularExpressionValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender8" runat="server" TargetControlID="RequiredFieldValidator2">
                    </cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Width="262px" ForeColor="White" Font-Bold="False" Font-Size="Small" ErrorMessage="must enter" ControlToValidate="txtMaxAllocation"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender9" runat="server" TargetControlID="RequiredFieldValidator9">
                    </cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator9" runat="server" Width="255px" ForeColor="White" Font-Bold="True" Font-Size="Large" ErrorMessage="must enter" ControlToValidate="cmbHouseKeeping" SetFocusOnError="True"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender10" runat="server" TargetControlID="RequiredFieldValidator10">
                    </cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator10" runat="server" Width="238px" ForeColor="White" Font-Bold="True" Font-Size="Large" ErrorMessage="must enter" ControlToValidate="txtNoofRooms"></asp:RequiredFieldValidator><BR /><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender12" runat="server" TargetControlID="RequiredFieldValidator11">
                    </cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator11" runat="server" Width="217px" ForeColor="White" Font-Bold="True" Font-Size="Small" Enabled="False" ErrorMessage="must enter" ControlToValidate="cmbRentApplicable"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender13" runat="server" TargetControlID="RequiredFieldValidator3">
                    </cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Width="269px" ForeColor="White" Font-Bold="True" Font-Size="Small" Enabled="False" ErrorMessage="must enter" ControlToValidate="cmbSecurityDeposit" SetFocusOnError="True"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender14" runat="server" TargetControlID="RequiredFieldValidator12">
                    </cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator12" runat="server" Width="230px" ForeColor="White" Font-Bold="True" Font-Size="Small" Enabled="False" ErrorMessage="must enter" ControlToValidate="cmbReturnsecurityDeposit"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender15" runat="server" TargetControlID="RequiredFieldValidator1">
                    </cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="328px" ForeColor="White" Font-Bold="True" Font-Size="Small" ErrorMessage="Select a request type" ControlToValidate="cmbAllocationRequest"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender16" runat="server" Enabled="False" TargetControlID="RequiredFieldValidator8"></cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator8" runat="server" Width="88px" ForeColor="White" Font-Size="Small" Visible="False" ErrorMessage="must enter" ControlToValidate="txtPolicyperiodTo"></asp:RequiredFieldValidator></asp:Panel> 
</contenttemplate>


<Triggers>
    <asp:PostBackTrigger ControlID="lnkpolicy" />
    <asp:PostBackTrigger ControlID="lnkcurrent" />
  </Triggers>
                </asp:UpdatePanel>
                </td>
       </tr>
        <tr>
            <td valign="top" colspan="2" style="width: 711px">
            </td>
        </tr>
        <tr>
            <td colspan="2"  valign="top" style="width: 711px">
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top" style="width: 711px" >
                <asp:Panel ID="Panel2" runat="server" Height="100%" Width="100%">
                    </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top" style="width: 711px">
               
            </td>
        </tr>
    </table>
    
    </div>

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

