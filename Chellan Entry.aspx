<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Chellan Entry.aspx.cs" Inherits="Chellan_Entry" Title="ARMS Chellan entry" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentplaceholder3" Runat="Server">
    <p>
        <asp:Panel ID="Panel4" runat="server" GroupingText="User Tips" Height="50px">
            <strong><span style="text-decoration: underline"></span></strong>
            <p>
                <span><strong><span style="text-decoration: underline"></span></strong>
            </p>
            <p>
        <span>use<strong> Enter Key</strong> or <strong>Tab Key</strong> or <strong>Mouse Click</strong>,
            To go to the Next Field. </span>
    </p>
    <p>
        <span></span><span>Use <strong>Mouse</strong> to select Data from the<strong> grid.</strong></span></p>
    <p>
        Press <strong>Save</strong> button to save the details</p>
    <p>
        Press <strong>Clear </strong>button to clear all fields.</p>
            </asp:Panel>
        &nbsp;</p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE><TBODY><TR><TD style="HEIGHT: 29px" colSpan=2><FONT size=5><CENTER>Chellan Entry</CENTER></FONT>&nbsp;</TD></TR><TR><TD style="WIDTH: 235px"><asp:Panel id="pnlbank" runat="server" Width="85%" GroupingText="Liability Details">&nbsp;&nbsp; <BR /><TABLE style="WIDTH: 523px"><TBODY><TR><TD><asp:Label id="lblOfficerName" runat="server" Width="83px" Text="Officier Name" __designer:wfdid="w36"></asp:Label></TD><TD><asp:TextBox id="txtOfficerName" runat="server" Width="140px" Height="17px" __designer:wfdid="w37" CssClass="UpperCaseFirstLetter"></asp:TextBox></TD><TD><asp:Label id="lblDate" runat="server" Width="49px" Text="Date" __designer:wfdid="w38"></asp:Label></TD><TD><asp:TextBox id="txtDate" runat="server" Width="140px" Height="17px" __designer:wfdid="w39" Enabled="False"></asp:TextBox></TD></TR><TR><TD><asp:Label id="lblDesign" runat="server" Text="Designation" __designer:wfdid="w40"></asp:Label></TD><TD><asp:TextBox id="txtDesignation" runat="server" Width="140px" Height="17px" __designer:wfdid="w41"></asp:TextBox></TD><TD><asp:Label id="lblTotlliblty" runat="server" Width="99px" Text="Total Liability" __designer:wfdid="w42"></asp:Label></TD><TD><asp:TextBox id="txtTotlliability" runat="server" Width="140px" Height="17px" ForeColor="Red" __designer:wfdid="w43" Enabled="False"></asp:TextBox></TD></TR><TR><TD><asp:Label id="lblCashier" runat="server" Text="Cashier Name" __designer:wfdid="w44"></asp:Label></TD><TD><asp:TextBox id="txtCashier" runat="server" Width="139px" __designer:wfdid="w45" OnTextChanged="txtCashier_TextChanged" Enabled="False"></asp:TextBox></TD><TD><asp:Label id="lblamount" runat="server" Width="138px" Text="Total Amount To be Remitted" __designer:wfdid="w46"></asp:Label></TD><TD><asp:TextBox id="txtAmount" runat="server" Width="140px" Height="17px" __designer:wfdid="w47" AutoPostBack="True" OnTextChanged="txtAmount_TextChanged" Enabled="False"></asp:TextBox></TD></TR><TR><TD><asp:Label id="lblChellanno" runat="server" Width="77px" Text="Chellan No" __designer:wfdid="w48"></asp:Label></TD><TD><asp:TextBox id="txtChellan" runat="server" Width="140px" Height="17px" __designer:wfdid="w49"></asp:TextBox></TD><TD><asp:Label id="lblBalance" runat="server" Width="117px" Text="Balance amount" __designer:wfdid="w50"></asp:Label></TD><TD><asp:TextBox id="txtBalance" runat="server" Width="140px" Height="17px" ForeColor="Red" __designer:wfdid="w51" Enabled="False"></asp:TextBox></TD></TR></TBODY></TABLE><BR /><BR /></asp:Panel> </TD><TD><asp:Panel id="pnlliability" runat="server" Width="100%" GroupingText="Bank Details"><TABLE style="HEIGHT: 104px"><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblbankname" runat="server" Width="73px" Text="Bank Name" __designer:wfdid="w52"></asp:Label> </TD><TD style="WIDTH: 130px"><asp:DropDownList id="ddlBankName" runat="server" Width="110px" AutoPostBack="True" DataTextField="bankname" DataValueField="bankname" OnSelectedIndexChanged="ddlBankName_SelectedIndexChanged" __designer:wfdid="w53"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblbranchname" runat="server" Width="84px" Text="Branch Name" __designer:wfdid="w54"></asp:Label> </TD><TD style="WIDTH: 130px"><asp:DropDownList id="ddlBranchName" runat="server" Width="110px" AutoPostBack="True" DataTextField="branchname" DataValueField="branchname" OnSelectedIndexChanged="ddlBranchName_SelectedIndexChanged1" __designer:wfdid="w55"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblaccountno" runat="server" Width="82px" Text="Account No" __designer:wfdid="w56"></asp:Label> </TD><TD style="WIDTH: 130px"><asp:DropDownList id="ddlAcNo" runat="server" Width="110px" AutoPostBack="True" DataTextField="accountno" DataValueField="bankid" __designer:wfdid="w57"></asp:DropDownList></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="Panel3" runat="server" Width="100%" GroupingText="Remitance Details" __designer:wfdid="w12"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="Label2" runat="server" Width="98px" Text="Bank Remitance Number" __designer:wfdid="w9"></asp:Label></TD><TD><asp:TextBox id="txtBankremNo" runat="server" Width="110px" __designer:wfdid="w11"></asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=2><asp:Panel id="Panel2" runat="server" Width="100%" Height="100%">&nbsp;<TABLE style="WIDTH: 774px; HEIGHT: 1px"><TBODY><TR><TD style="WIDTH: 100px; TEXT-ALIGN: right"><asp:Button id="Btnsubmit" onclick="Btnsubmit_Click" runat="server" Text="Submit" CssClass="btnStyle_medium" Enabled="False"></asp:Button></TD><TD style="WIDTH: 100px; TEXT-ALIGN: right"><asp:Button id="btnApprove" onclick="btnApprove_Click" runat="server" CausesValidation="False" Text="Approve" __designer:wfdid="w2" CssClass="btnStyle_medium" Enabled="False"></asp:Button></TD><TD style="WIDTH: 100px; TEXT-ALIGN: right"><asp:Button id="Btnreject" onclick="Btnreject_Click" runat="server" CausesValidation="False" Text="Reject" CssClass="btnStyle_medium" Enabled="False"></asp:Button></TD><TD style="WIDTH: 100px; TEXT-ALIGN: right"><asp:Button id="btnConfirm" onclick="btnConfirm_Click" runat="server" CausesValidation="False" Text="Confirm" __designer:wfdid="w4" CssClass="btnStyle_medium" Enabled="False"></asp:Button></TD><TD style="WIDTH: 100px; TEXT-ALIGN: right"><asp:Button id="btnClear" onclick="btnClear_Click" runat="server" CausesValidation="False" Text="Clear" CssClass="btnStyle_medium"></asp:Button></TD><TD style="WIDTH: 100px; TEXT-ALIGN: right"><asp:Button id="btnReport" onclick="btnReport_Click" runat="server" CausesValidation="False" Text="Report" __designer:wfdid="w1" CssClass="btnStyle_medium"></asp:Button></TD><TD style="WIDTH: 100px; TEXT-ALIGN: right"><asp:Button id="btnView" onclick="btnView_Click1" runat="server" CausesValidation="False" Text="View" __designer:wfdid="w10" CssClass="btnStyle_medium"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel>&nbsp;<asp:Panel id="pnllogin" runat="server" Width="125px" GroupingText="Login" HorizontalAlign="Left" Visible="False"><asp:Login id="Login1" runat="server" Width="346px" OnAuthenticate="Login1_Authenticate"></asp:Login> </asp:Panel> <asp:Panel id="pnlreport" runat="server" Width="45%" Height="100%" GroupingText="Report"><TABLE><TBODY><TR><TD style="WIDTH: 3px"><asp:LinkButton id="lnklblbank" onclick="lnklblbank_Click" runat="server" Width="189px" CausesValidation="False" __designer:wfdid="w3">Daily Bank Remittence report</asp:LinkButton></TD><TD style="WIDTH: 3px; TEXT-ALIGN: center"><asp:Label id="lblDaydendate" runat="server" Width="77px" __designer:wfdid="w36" Text="Select Date"></asp:Label></TD><TD style="WIDTH: 3px; TEXT-ALIGN: center"><asp:TextBox id="txtDayEndDate" runat="server" Width="125px" __designer:wfdid="w9"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 3px"><asp:LinkButton id="lnklbldaily" onclick="lnklbldaily_Click" runat="server" Width="144px" CausesValidation="False" Visible="False" __designer:wfdid="w2">Liability Register</asp:LinkButton></TD><TD style="WIDTH: 3px"></TD><TD style="WIDTH: 3px"></TD></TR></TBODY></TABLE></asp:Panel><BR /><BR /></TD></TR><TR><TD colSpan=2><asp:Panel id="Panel1" runat="server" Width="100%" Height="100%"><TABLE><TBODY><TR><TD style="WIDTH: 1px; HEIGHT: 1px"><asp:CheckBox id="chkselectall" runat="server" Width="99px" Text="Selectall" __designer:wfdid="w3" Visible="False" AutoPostBack="True" OnCheckedChanged="chkselectall_CheckedChanged"></asp:CheckBox></TD><TD style="WIDTH: 22px; HEIGHT: 1px" colSpan=2></TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=3><asp:GridView id="dtgLiability" runat="server" Width="788px" ForeColor="#333333" __designer:wfdid="w2" OnSelectedIndexChanged="dtgLiability_SelectedIndexChanged" OnPageIndexChanging="dtgLiability_PageIndexChanging" AllowPaging="True" PageSize="5" DataKeyNames="liable_id" CellPadding="4" Caption="Liability List" AutoGenerateColumns="False" GridLines="None">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:TemplateField><EditItemTemplate>
<asp:CheckBox runat="server" id="CheckBox1"></asp:CheckBox>
</EditItemTemplate>
<ItemTemplate>
<asp:CheckBox id="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" __designer:wfdid="w31"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="date" HeaderText="Date"></asp:BoundField>
<asp:BoundField DataField="ledgername" HeaderText="Ledgername"></asp:BoundField>
<asp:BoundField DataField="total" HeaderText="Total  Amount"></asp:BoundField>
<asp:TemplateField HeaderText="Amount to be Remitted"><ItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# bind("Amount_to_be_remitted") %>' AutoPostBack="True" OnTextChanged="TextBox1_TextChanged" ReadOnly="True"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Remitted_Amount" HeaderText="Remitted/Submitted"></asp:BoundField>
<asp:TemplateField HeaderText="Balance"><ItemTemplate>
<asp:Label id="Label1" runat="server" Text='<%# bind("balance") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="liable_id" Visible="False" HeaderText="Id"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR><TR><TD colSpan=3><asp:Panel id="pnlView" runat="server" Width="138px" Height="28px" GroupingText="Click to view" __designer:wfdid="w14" Visible="False"><asp:RadioButtonList id="RadioButtonList1" runat="server" Width="777px" Height="6px" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" __designer:wfdid="w8" RepeatDirection="Horizontal"><asp:ListItem Value="0">Submitted</asp:ListItem>
<asp:ListItem Value="1">Approved</asp:ListItem>
<asp:ListItem Value="2">Rejected</asp:ListItem>
<asp:ListItem Value="3">Confirmed</asp:ListItem>
</asp:RadioButtonList></asp:Panel></TD></TR><TR><TD style="HEIGHT: 40px" colSpan=3></TD></TR><TR><TD colSpan=3><asp:GridView id="gdchelanentry" runat="server" Width="791px" ForeColor="#333333" Font-Bold="False" __designer:wfdid="w9" OnSelectedIndexChanged="gdchelanentry_SelectedIndexChanged" OnPageIndexChanging="gdchelanentry_PageIndexChanging" CellPadding="4" OnRowCreated="gdchelanentry_RowCreated" GridLines="None">
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
</asp:GridView></TD></TR><TR><TD style="HEIGHT: 5px" colSpan=3></TD></TR><TR><TD colSpan=3><asp:GridView id="gdDetailed" runat="server" Width="792px" ForeColor="#333333" Font-Bold="False" __designer:wfdid="w11" OnPageIndexChanging="gdDetailed_PageIndexChanging" CellPadding="4" GridLines="None">
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
</asp:GridView></TD></TR></TBODY></TABLE></asp:Panel> &nbsp;&nbsp; <TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" Width="72px" ForeColor="White" Font-Bold="False" ControlToValidate="txtdate" ErrorMessage="dd-mm-yyyy" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RegularExpressionValidator1"></cc1:ValidatorCalloutExtender></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 41px"><asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" Width="120px" ForeColor="White" Font-Bold="False" ControlToValidate="txtamount" ErrorMessage="Amount required"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px; HEIGHT: 41px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator6"></cc1:ValidatorCalloutExtender></TD><TD style="WIDTH: 100px; HEIGHT: 41px"></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 68px"><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Width="144px" ForeColor="White" Font-Bold="False" ControlToValidate="ddlBankName" ErrorMessage="Bank name required" Font-Italic="False"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px; HEIGHT: 68px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator2"></cc1:ValidatorCalloutExtender></TD><TD style="WIDTH: 100px; HEIGHT: 68px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ForeColor="White" Font-Bold="False" ControlToValidate="ddlBranchName" ErrorMessage="Branch name required" Font-Italic="False" SetFocusOnError="True"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator3"></cc1:ValidatorCalloutExtender></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ForeColor="White" Font-Bold="False" ControlToValidate="ddlAcNo" ErrorMessage="Account no required" Font-Italic="False"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredFieldValidator4"></cc1:ValidatorCalloutExtender></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:RequiredFieldValidator id="RequiredFieldValidator7" runat="server" ForeColor="White" Font-Bold="False" ControlToValidate="txtofficername" ErrorMessage="Officername required"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" TargetControlID="RequiredFieldValidator7"></cc1:ValidatorCalloutExtender></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ForeColor="White" Font-Bold="False" ControlToValidate="txtchellan" ErrorMessage="Chellanno required" Font-Italic="False"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" TargetControlID="RequiredFieldValidator1"></cc1:ValidatorCalloutExtender></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><cc1:FilteredTextBoxExtender id="FilteredTextBoxExtender1" runat="server" TargetControlID="txtamount" FilterType="Numbers"></cc1:FilteredTextBoxExtender></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><cc1:CalendarExtender id="CalendarExtender1" runat="server" __designer:wfdid="w7" TargetControlID="txtDayEndDate"></cc1:CalendarExtender></TD><TD style="WIDTH: 100px"></TD></TR></TBODY></TABLE><BR /><asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button><asp:TextBox id="TextBox1" runat="server" Visible="False" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox><asp:Label id="Label3" runat="server" Text="Label" Visible="False"></asp:Label> <BR /><asp:Panel id="pnlMessage" runat="server" _="" _designer:dtid="562958543355909" CssClass="ModalWindow"><asp:Panel id="Panel7" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Width="229px" ForeColor="MediumBlue" Text="Tsunami ARMS -" Font-Bold="True"></asp:Label></asp:Panel> &nbsp; <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center><asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD style="HEIGHT: 18px"></TD><TD style="HEIGHT: 18px" align=center></TD><TD style="HEIGHT: 18px" align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel> <BR /><cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize">
                </cc1:ModalPopupExtender> <TABLE style="WIDTH: 182px"><TBODY><TR><TD style="WIDTH: 100px"><asp:Button id="Button4" onclick="Button4_Click" runat="server" CausesValidation="False" Text="Save & Approve" Visible="False" CssClass="btnStyle_large"></asp:Button></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:Button id="btnAuthentication" onclick="btnAuthentication_Click" runat="server" Width="100px" CausesValidation="False" Text="Authentication" Visible="False" CssClass="btnStyle_large"></asp:Button></TD><TD style="WIDTH: 141px"><asp:Button id="btnSave" onclick="btnSave_Click" runat="server" Width="88px" Text="Print Chellan" Visible="False" CssClass="btnStyle_large"></asp:Button></TD></TR></TBODY></TABLE><BR /><BR /></TD></TR></TBODY></TABLE>
</contenttemplate>

 <Triggers>
 
    <asp:PostBackTrigger ControlID="lnklbldaily" />
    
    <asp:PostBackTrigger ControlID="lnklblbank" />
  </Triggers>

    </asp:UpdatePanel>
    &nbsp;
</asp:Content>

