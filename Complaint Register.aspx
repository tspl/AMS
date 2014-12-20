<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Complaint Register.aspx.cs" Inherits="Complaint_Register" Title="Tsunami ARMS Complaint Register Page" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentplaceholder3" Runat="Server">
        <p>
            <asp:Panel ID="PnlTips" runat="server" GroupingText="User Tips" Height="50px" Width="100%">
                <br />
    User can assign urgency level for entered complaint.<p>
        This form is used for storing Complaint details in database</p>
                <p>
    User can also assign the team responsible to act for entered complaint. </p>
            <p>
    Authorised user can edit,delete or print complaint details.
<script language="javascript" type="text/javascript">
// <!CDATA[

function TABLE1_onclick() {

}

function TABLE2_onclick() {

}

// ]]>
</script>

            </p>
            <p>
    Click on Rectified button if the complaint is rectified.</p><p>Authorised user can see the report with average time taken for each work.</p>
            </asp:Panel>
            <br />
            &nbsp;</p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
  <script type="text/javascript">
function ClearLastMessage(Label7)
{

   $get(elem).innerHTML = '';
}
</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE id="TABLE2"><TBODY><TR><TD vAlign=top align=center colSpan=4>&nbsp;&nbsp;&nbsp; <asp:Label id="Label6" runat="server" ForeColor="#000099" Text="Complaint Register" Font-Bold="True" CssClass="heading" Font-Size="14pt" Font-Names="Arial"></asp:Label></TD></TR><TR style="FONT-SIZE: 12pt"><TD vAlign=top><asp:Panel id="pnlcomplaint" runat="server" Height="100%" GroupingText="Complaint"><TABLE id="TABLE1" onclick="return TABLE1_onclick()"><TBODY><TR><TD vAlign=top><asp:Label id="Label4" runat="server" Width="85px" Text="Building Name" __designer:wfdid="w17"></asp:Label></TD><TD vAlign=top><asp:DropDownList id="cmbBuilding" tabIndex=2 runat="server" Width="150px" Height="22px" OnSelectedIndexChanged="cmbBuilding_SelectedIndexChanged1" __designer:wfdid="w18" DataValueField="build_id" DataTextField="buildingname" AutoPostBack="True"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD vAlign=top></TD><TD vAlign=top><asp:Label id="Label3" runat="server" Width="82px" Text="Reciept No" __designer:wfdid="w12"></asp:Label></TD><TD style="WIDTH: 133px" vAlign=top><asp:TextBox id="txtReceipt" runat="server" Width="120px" Height="17px" __designer:wfdid="w13" AutoPostBack="True" OnTextChanged="txtReceipt_TextChanged1">0</asp:TextBox></TD><TD vAlign=top></TD></TR><TR><TD vAlign=top><asp:Label id="Label5" runat="server" Width="73px" Text="Room No" __designer:wfdid="w10"></asp:Label></TD><TD vAlign=top><asp:DropDownList id="cmbRoom" tabIndex=5 runat="server" Width="150px" Height="22px" OnSelectedIndexChanged="cmbRoom_SelectedIndexChanged" __designer:wfdid="w11" DataValueField="room_id" DataTextField="roomno" AutoPostBack="True"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD vAlign=top></TD><TD vAlign=top><asp:Label id="lblurgency" runat="server" Text="Urgency" __designer:wfdid="w14"></asp:Label></TD><TD vAlign=top><asp:TextBox id="txtUrgency" runat="server" __designer:wfdid="w27"></asp:TextBox></TD><TD vAlign=top><asp:LinkButton id="LinkButton1" onclick="LinkButton1_Click" runat="server" Width="28px" CausesValidation="False" __designer:wfdid="w16" Visible="False">New</asp:LinkButton></TD></TR><TR><TD style="HEIGHT: 26px" vAlign=top><asp:Label id="lblcmplntctgry" runat="server" Width="120px" Text="Complaint Category" __designer:wfdid="w1"></asp:Label></TD><TD style="HEIGHT: 26px" vAlign=top><asp:DropDownList id="cmbCategory" runat="server" Width="150px" Height="22px" OnSelectedIndexChanged="cmbCategory_SelectedIndexChanged" __designer:wfdid="w2" DataValueField="cmp_category_id" DataTextField="cmp_cat_name" AutoPostBack="True"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD style="HEIGHT: 26px" vAlign=top><asp:LinkButton id="LinkButton6" onclick="LinkButton5_Click1" runat="server" CausesValidation="False" __designer:wfdid="w3" Visible="False">New</asp:LinkButton></TD><TD style="HEIGHT: 26px" vAlign=top><asp:Label id="lblcmplntcode" runat="server" Width="92px" Text="Policy Type" __designer:wfdid="w19"></asp:Label></TD><TD style="WIDTH: 133px; HEIGHT: 26px" vAlign=top><asp:TextBox id="txtPolicy" runat="server" __designer:wfdid="w28"></asp:TextBox></TD><TD style="HEIGHT: 26px" vAlign=top></TD></TR><TR><TD><asp:Label id="lblcmplntname" runat="server" Width="98px" Height="11px" Text="Complaint Name" __designer:wfdid="w7"></asp:Label></TD><TD><asp:DropDownList id="cmbComplaint" tabIndex=1 runat="server" Width="150px" Height="22px" OnSelectedIndexChanged="cmbComplaint_SelectedIndexChanged" __designer:wfdid="w8" DataValueField="complaint_id" DataTextField="cmpname" AutoPostBack="True"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD><asp:LinkButton id="LinkButton2" onclick="LinkButton2_Click" runat="server" CausesValidation="False" __designer:wfdid="w9" Visible="False">New</asp:LinkButton></TD><TD vAlign=middle><asp:Label id="Label13" runat="server" Width="109px" Text="Reason for Delay" __designer:wfdid="w23" Visible="False"></asp:Label></TD><TD style="WIDTH: 133px" vAlign=middle><asp:DropDownList id="cmbReason" runat="server" Width="126px" Height="22px" __designer:wfdid="w24" DataValueField="reason_id" DataTextField="reason" AutoPostBack="True" Visible="False"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD vAlign=top></TD></TR><TR><TD><asp:Label id="Label7" runat="server" Text="Team Name" __designer:wfdid="w21"></asp:Label></TD><TD><asp:DropDownList id="cmbTeam" tabIndex=3 runat="server" Width="150px" Height="22px" OnSelectedIndexChanged="cmbTeam_SelectedIndexChanged2" __designer:wfdid="w22" DataValueField="team_id" DataTextField="teamname" AutoPostBack="True"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD></TD><TD vAlign=middle><asp:DropDownList id="cmbPolicy" tabIndex=8 runat="server" Width="126px" Height="22px" OnSelectedIndexChanged="cmbPolicy_SelectedIndexChanged" __designer:wfdid="w20" AutoPostBack="True" Visible="False"></asp:DropDownList></TD><TD style="WIDTH: 133px" vAlign=middle><asp:Label id="lblPolicyID" runat="server" Text="0" __designer:wfdid="w25" Visible="False"></asp:Label></TD><TD vAlign=top></TD></TR><TR><TD><asp:Label id="Label2" runat="server" Width="100px" Text="Task Action" __designer:wfdid="w4"></asp:Label></TD><TD><asp:DropDownList id="cmbAction" tabIndex=4 runat="server" Width="150px" Height="22px" OnSelectedIndexChanged="cmbAction_SelectedIndexChanged" __designer:wfdid="w5" DataValueField="task_id" DataTextField="taskname" AutoPostBack="True"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD><asp:LinkButton id="lnktask" onclick="lnktask_Click" runat="server" CausesValidation="False" __designer:wfdid="w6" Visible="False">New</asp:LinkButton></TD><TD vAlign=middle><asp:DropDownList id="cmbUrgency" tabIndex=7 runat="server" Width="126px" Height="22px" OnSelectedIndexChanged="cmbUrgency_SelectedIndexChanged1" __designer:wfdid="w15" DataValueField="urg_cmp_id" DataTextField="urgname" AutoPostBack="True" Visible="False"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD vAlign=middle><asp:Label id="lblUrgID" runat="server" Text="0" __designer:wfdid="w25" Visible="False"></asp:Label></TD><TD vAlign=top></TD></TR></TBODY></TABLE>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Panel></TD><TD vAlign=top colSpan=1><asp:Panel id="pnlbuldng" runat="server" Height="100%" GroupingText="Completion Details"><TABLE style="WIDTH: 255px; HEIGHT: 90px"><TBODY><TR><TD style="WIDTH: 99px; HEIGHT: 4px" vAlign=top><asp:Label id="Label16" runat="server" Width="102px" Text="Proposed  Date"></asp:Label></TD><TD style="TEXT-ALIGN: left" vAlign=top colSpan=2><asp:TextBox id="txtPropDate" runat="server" Width="95px"></asp:TextBox></TD></TR><TR><TD vAlign=top><asp:Label id="Label8" runat="server" Width="106px" Text="Proposed  Time"></asp:Label></TD><TD style="WIDTH: 143px" colSpan=2><asp:TextBox id="txtPropTime" tabIndex=6 runat="server" Width="95px"></asp:TextBox></TD></TR><TR><TD vAlign=top colSpan=3>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label id="lblwrk" runat="server" Text="Work Status" Font-Bold="True"></asp:Label><asp:RadioButtonList id="rblStatus" runat="server" Width="242px" OnSelectedIndexChanged="rblStatus_SelectedIndexChanged" AutoPostBack="True" RepeatDirection="Horizontal"><asp:ListItem Selected="True">Not Completed</asp:ListItem>
<asp:ListItem>Completed</asp:ListItem>
</asp:RadioButtonList></TD></TR><TR><TD style="HEIGHT: 18px" vAlign=top colSpan=3><asp:Label id="lblComplete" runat="server" Width="207px" ForeColor="#FF8000" Text="Work Completion Time" Font-Bold="True" Visible="False"></asp:Label></TD></TR><TR><TD vAlign=top><asp:Label id="Label1" runat="server" Width="104px" Text="Completed Date" Visible="False"></asp:Label></TD><TD style="WIDTH: 143px" colSpan=2><asp:TextBox id="txtComplete" runat="server" Width="95px" Visible="False" ReadOnly="True"></asp:TextBox></TD></TR><TR><TD vAlign=top><asp:Label id="Label14" runat="server" Text="Completed Time" Visible="False"></asp:Label></TD><TD style="WIDTH: 143px" colSpan=2><asp:TextBox id="TextBox1" runat="server" Width="95px" Visible="False"></asp:TextBox></TD></TR><TR><TD vAlign=top></TD><TD style="WIDTH: 143px" colSpan=2></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD vAlign=top align=center colSpan=2 rowSpan=1><asp:Button id="btnRegister" tabIndex=9 onclick="btnRegister_Click" runat="server" Text="Save" CssClass="btnStyle_small" OnClientClick=" "></asp:Button> <asp:Button id="btnDelete" onclick="btnDelete_Click" runat="server" Width="72px" Text="Delete" CssClass="btnStyle_small"></asp:Button> <asp:Button id="clear" onclick="clear_Click" runat="server" CausesValidation="False" Text="Clear" CssClass="btnStyle_small"></asp:Button> <asp:Button id="Button5" onclick="Button5_Click" runat="server" CausesValidation="False" Text="Report" CssClass="btnStyle_small"></asp:Button></TD></TR><TR><TD vAlign=top colSpan=2 rowSpan=1>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Panel id="pnlrprt" runat="server" Width="97%" Height="1px" GroupingText="Report Details"><TABLE><TBODY><TR><TD style="HEIGHT: 18px" vAlign=top><asp:Label id="Label11" runat="server">From date</asp:Label></TD><TD style="WIDTH: 134px; HEIGHT: 18px" vAlign=top colSpan=3><asp:TextBox id="txtreportfrom" runat="server" Width="132px" OnTextChanged="txtreportfrom_TextChanged"></asp:TextBox></TD><TD style="HEIGHT: 18px" vAlign=top colSpan=1><asp:Label id="Label12" runat="server" Width="58px" Text="To date"></asp:Label></TD><TD style="WIDTH: 158px" vAlign=top colSpan=1><asp:TextBox id="txtreportto" runat="server" Width="132px"></asp:TextBox></TD><TD style="WIDTH: 59px; HEIGHT: 18px" vAlign=top colSpan=1></TD><TD style="WIDTH: 81px; HEIGHT: 18px" vAlign=top colSpan=1></TD><TD style="WIDTH: 81px; HEIGHT: 18px" vAlign=top colSpan=1></TD></TR><TR><TD style="HEIGHT: 6px; TEXT-ALIGN: left">&nbsp;<asp:Label id="Label9" runat="server" Width="85px" Text="Building Name"></asp:Label></TD><TD colSpan=3><asp:DropDownList id="cmbReportBuilding" runat="server" Width="138px" DataValueField="build_id" DataTextField="buildingname" AutoPostBack="True"><asp:ListItem Value="-1">All</asp:ListItem>
</asp:DropDownList></TD><TD vAlign=top colSpan=1>Category</TD><TD vAlign=top><asp:DropDownList id="cmbReportcategory" runat="server" Width="137px" DataValueField="cmp_category_id" DataTextField="cmp_cat_name" AutoPostBack="True"><asp:ListItem Value="-1">All</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 59px; HEIGHT: 6px"></TD><TD style="WIDTH: 81px; HEIGHT: 6px"></TD><TD style="WIDTH: 81px; HEIGHT: 6px"></TD></TR><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: left" colSpan=4><asp:LinkButton id="LinkButton4" onclick="LinkButton4_Click" runat="server" Width="247px" CausesValidation="False">All  Pending  complaint Details</asp:LinkButton></TD><TD style="HEIGHT: 18px; TEXT-ALIGN: left" colSpan=1></TD><TD style="HEIGHT: 18px" colSpan=5><asp:LinkButton id="lnkCompleted" onclick="lnkCompleted_Click" runat="server" CausesValidation="False">Completed Works</asp:LinkButton></TD></TR><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: left" colSpan=4></TD><TD style="HEIGHT: 18px; TEXT-ALIGN: left" colSpan=1></TD><TD style="HEIGHT: 18px" colSpan=5>&nbsp;<asp:Button id="Button6" onclick="Button6_Click1" runat="server" CausesValidation="False" Text="Hide Report" CssClass="btnStyle_large"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel>&nbsp;&nbsp; </TD></TR><TR><TD vAlign=top align=center colSpan=4><asp:GridView id="dgcmpregister" runat="server" Width="828px" HorizontalAlign="Left" ForeColor="Blue" AutoGenerateColumns="False" DataKeyNames="complaint_no" GridLines="None" CellPadding="4" PageSize="6" OnRowCreated="dgcmpregister_RowCreated" AllowPaging="True" OnPageIndexChanging="dgcmpregister_PageIndexChanging" OnSelectedIndexChanged="dgcmpregister_SelectedIndexChanged" Caption="Complaint Details">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="complaint_no" Visible="False" HeaderText="complaint_no"></asp:BoundField>
<asp:BoundField DataField="Complaint Name" HeaderText="Complaint Name"></asp:BoundField>
<asp:BoundField DataField="Complaint Category" HeaderText="Complaint Category"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="Room No" HeaderText="Room No"></asp:BoundField>
<asp:BoundField DataField="Policy" HeaderText="Policy"></asp:BoundField>
<asp:BoundField DataField="Team" HeaderText="Team"></asp:BoundField>
<asp:BoundField DataField="Urgency" HeaderText="Urgency"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView>&nbsp; <BR /><BR /><BR /><BR /><BR /><BR /><BR /><BR /><BR /><BR /><BR /><BR /><BR /><BR /></TD></TR><TR><TD vAlign=middle colSpan=4></TD></TR></TBODY></TABLE>&nbsp; <TABLE><TBODY><TR><TD vAlign=top><BR /><cc1:FilteredTextBoxExtender id="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="txtReceipt"></cc1:FilteredTextBoxExtender> <BR /><asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" Width="222px" ForeColor="White" ErrorMessage="please select a complaint category" ControlToValidate="cmbCategory" InitialValue="-1"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" Width="215px" ForeColor="White" ErrorMessage="select a complaint name " ControlToValidate="cmbComplaint"></asp:RequiredFieldValidator> <BR /><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Width="221px" ForeColor="White" ErrorMessage="Enter the urgency" ControlToValidate="cmbUrgency" InitialValue="-1"></asp:RequiredFieldValidator>&nbsp;<BR />&nbsp; <cc1:CalendarExtender id="CalendarExtender1" runat="server" TargetControlID="txtreportfrom" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:CalendarExtender id="CalendarExtender2" runat="server" TargetControlID="txtreportto" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator4"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator5"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator2"></cc1:ValidatorCalloutExtender><BR />&nbsp;<asp:RequiredFieldValidator id="rfvBuild" runat="server" ForeColor="White" ErrorMessage="Building Required" ControlToValidate="cmbBuilding" InitialValue="-1"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="rfvRoom" runat="server" ForeColor="White" ErrorMessage="Room Required" ControlToValidate="cmbRoom"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="rfvTeam" runat="server" ForeColor="White" ErrorMessage="Team Required" ControlToValidate="cmbTeam" InitialValue="-1"></asp:RequiredFieldValidator> <cc1:ValidatorCalloutExtender id="Build" runat="server" TargetControlID="rfvBuild"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="room" runat="server" TargetControlID="rfvRoom"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="team" runat="server" TargetControlID="rfvTeam"></cc1:ValidatorCalloutExtender> <cc1:ListSearchExtender id="buildig" runat="server" TargetControlID="cmbBuilding"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="lroom" runat="server" TargetControlID="cmbRoom"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="lteam" runat="server" TargetControlID="cmbTeam"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="lcomplaint" runat="server" TargetControlID="cmbComplaint"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="lcategory" runat="server" TargetControlID="cmbCategory"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="lurgency" runat="server" TargetControlID="cmbUrgency"></cc1:ListSearchExtender></TD><TD style="WIDTH: 440px" vAlign=top><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel5" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Text="Tsunami ARMS - Confirmation" Font-Bold="True" ForeColor="MediumBlue"></asp:Label><BR /><asp:Label id="lblHead2" runat="server" Text="Tsunami ARMS - Warning" Font-Bold="True" ForeColor="MediumBlue"></asp:Label></asp:Panel> <asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" Width="132px" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" Font-Bold="True" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" Font-Bold="True" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center>&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" Width="174px" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD></TD><TD align=center>&nbsp; &nbsp;<asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" Font-Bold="True" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR></TBODY></TABLE></asp:Panel>&nbsp; <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button><BR /><cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" TargetControlID="btnHidden" RepositionMode="RepositionOnWindowResize" PopupControlID="pnlMessage">
                </cc1:ModalPopupExtender></asp:Panel>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<BR />&nbsp;&nbsp;&nbsp; 
</contenttemplate>
 <Triggers>
   
   
    <asp:PostBackTrigger ControlID="LinkButton4" />
    
  <asp:PostBackTrigger ControlID="lnkCompleted" />
   
  </Triggers>
    </asp:UpdatePanel>
    <br />
    &nbsp;
    
</asp:Content>

