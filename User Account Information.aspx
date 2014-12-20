<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="User Account Information.aspx.cs" Inherits="User_Account_Information" Title="User Account Information Page" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <asp:ScriptManager id="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel id="UpdatePanel1" runat="server">
            <contenttemplate>
<TABLE><TBODY><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: center" colSpan=2><asp:Label id="Label4" runat="server" Text="User Account Information" __designer:wfdid="w1" Font-Size="X-Large"></asp:Label></TD></TR><TR><TD style="HEIGHT: 248px" vAlign=top colSpan=1><asp:Panel id="Panel3" runat="server" Height="50px" GroupingText="Staff"><TABLE><TBODY><TR><TD style="HEIGHT: 21px" vAlign=top><asp:Label id="LblStaffName" runat="server" Width="74px" Text="Staff Name" __designer:wfdid="w47"></asp:Label></TD><TD style="WIDTH: 134px; HEIGHT: 21px" vAlign=middle><asp:DropDownList id="DropDownList1" runat="server" Width="127px" __designer:wfdid="w46" DataValueField="staff_id" DataTextField="staffname" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"></asp:DropDownList></TD><TD style="WIDTH: 6px; HEIGHT: 21px" colSpan=2></TD></TR><TR><TD style="WIDTH: 97px; HEIGHT: 18px"><asp:Label id="Label3" runat="server" Text="Staff Id" __designer:wfdid="w49"></asp:Label></TD><TD style="WIDTH: 134px"><asp:TextBox id="txtstaffid" runat="server" Width="125px" __designer:wfdid="w50" Enabled="False"></asp:TextBox></TD><TD style="WIDTH: 6px; HEIGHT: 18px" colSpan=2></TD></TR><TR><TD style="WIDTH: 97px"><asp:Label id="LblUserNam" runat="server" Width="76px" Text="User Name"></asp:Label></TD><TD style="WIDTH: 134px"><asp:TextBox id="TextUserName" tabIndex=1 runat="server" Width="125px" AutoPostBack="True" OnTextChanged="TextUserName_TextChanged" MaxLength="10"></asp:TextBox></TD><TD style="WIDTH: 6px" colSpan=2>&nbsp;</TD></TR><TR><TD style="WIDTH: 97px; HEIGHT: 24px"><asp:Label id="LblPswd" runat="server" Width="63px" Text="Password"></asp:Label></TD><TD style="WIDTH: 134px"><asp:TextBox id="TextPsword" tabIndex=2 runat="server" Width="125px" OnTextChanged="TextPsword_TextChanged" MaxLength="10" TextMode="Password"></asp:TextBox></TD><TD style="WIDTH: 6px; HEIGHT: 24px" colSpan=2></TD></TR><TR><TD style="WIDTH: 97px"><asp:Label id="LblRepswd" runat="server" Width="108px" Text="Retype Password"></asp:Label> </TD><TD style="WIDTH: 134px"><asp:TextBox id="TextRetypePswd" tabIndex=3 runat="server" Width="125px" CausesValidation="True" OnTextChanged="TextRetypePswd_TextChanged" TextMode="Password"></asp:TextBox> </TD><TD style="WIDTH: 6px" colSpan=2></TD></TR><TR><TD style="WIDTH: 97px">&nbsp;</TD><TD colSpan=3><asp:CompareValidator id="CompareValidator1" runat="server" Width="137px" __designer:wfdid="w6" ErrorMessage="Password missmatch" ControlToValidate="TextRetypePswd" ControlToCompare="TextPsword"></asp:CompareValidator></TD></TR></TBODY></TABLE></asp:Panel>&nbsp;&nbsp; </TD><TD style="WIDTH: 297px; HEIGHT: 248px" vAlign=top colSpan=1><asp:Panel id="Panel2" runat="server" Width="349px" Height="72%" GroupingText="Details"><TABLE><TBODY><TR><TD style="WIDTH: 147px; HEIGHT: 18px" vAlign=middle><asp:Label id="Label1" runat="server" Width="111px" Text="User Rights Level"></asp:Label></TD><TD style="TEXT-ALIGN: left" vAlign=top><asp:DropDownList id="DropDownList2" tabIndex=4 runat="server" Width="127px" __designer:wfdid="w47" DataValueField="prev_level" DataTextField="prev_level" AutoPostBack="True" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged"></asp:DropDownList></TD><TD vAlign=top>&nbsp;</TD></TR><TR><TD style="WIDTH: 147px"><asp:Label id="Label6" runat="server" Width="100px" Text="User Privilages"></asp:Label></TD><TD style="WIDTH: 120px"><asp:ListBox id="ListUserPrivlegs" runat="server" Width="129px" Height="62px" AutoPostBack="True" OnSelectedIndexChanged="ListUserPrivlegs_SelectedIndexChanged"></asp:ListBox></TD></TR><TR><TD style="WIDTH: 147px; HEIGHT: 1px" vAlign=top><asp:Label id="defaultform" runat="server" Width="82px" Text="Default Form"></asp:Label></TD><TD style="WIDTH: 120px; HEIGHT: 1px" vAlign=top><asp:TextBox id="txtdefaultform" runat="server" Width="125px" Enabled="False"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 147px; HEIGHT: 18px"><asp:Label id="LblExectvPolicy" runat="server" Width="158px" Height="22px" Text="Executive Override Policy" __designer:wfdid="w74"></asp:Label></TD><TD style="WIDTH: 120px; HEIGHT: 18px" vAlign=top><asp:TextBox id="txtexecuteop" runat="server" Width="125px" AutoPostBack="True" Enabled="False"></asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel>&nbsp; </TD></TR><TR><TD style="TEXT-ALIGN: left" vAlign=top colSpan=2><asp:Panel id="Panel1" runat="server" Width="642px" GroupingText="Buttons" HorizontalAlign="Center"><TABLE><TBODY><TR><TD style="WIDTH: 85px; HEIGHT: 26px"><asp:Button id="BtnSave" tabIndex=5 onclick="BtnSave_Click" runat="server" Width="72px" Text="Save" __designer:wfdid="w30" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 85px; HEIGHT: 26px"><asp:Button id="btndelete" onclick="btndelete_Click" runat="server" Width="73px" Text="Delete" __designer:wfdid="w31" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 85px; HEIGHT: 26px"><asp:Button id="clear" onclick="clear_Click" runat="server" Width="73px" CausesValidation="False" Text="Clear" __designer:wfdid="w32" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 85px; HEIGHT: 26px"><asp:Button id="Button1" onclick="Button1_Click" runat="server" Width="73px" CausesValidation="False" Text="Report" __designer:wfdid="w33" CssClass="btnStyle_small"></asp:Button> </TD></TR></TBODY></TABLE></asp:Panel>&nbsp; </TD></TR><TR><TD style="TEXT-ALIGN: center" vAlign=top colSpan=2><asp:Label id="Label2" runat="server" Text="USER DETAILS GRID" Font-Bold="True" __designer:wfdid="w2"></asp:Label></TD></TR><TR><TD style="HEIGHT: 294px" vAlign=top colSpan=2><asp:Panel id="panel" runat="server" Width="640px" Height="65px" GroupingText="System User Details" ForeColor="Blue" BorderColor="White">&nbsp;<asp:GridView id="dguseraccount" runat="server" Width="621px" Height="61px" ForeColor="#333333" __designer:wfdid="w3" OnSelectedIndexChanged="dguseraccount_SelectedIndexChanged1" GridLines="None" CellPadding="4" PageSize="7" OnPageIndexChanging="dguseraccount_PageIndexChanging" OnRowCreated="dguseraccount_RowCreated" AllowPaging="True">
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
</asp:GridView> </asp:Panel> <asp:Panel id="Panel4" runat="server" Width="640px" Height="50px" GroupingText="Report" __designer:wfdid="w5"><TABLE style="WIDTH: 617px"><TBODY><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: left" colSpan=2><asp:LinkButton id="LinkButton1" onclick="LinkButton1_Click1" runat="server" CausesValidation="False" __designer:wfdid="w6">User profile report</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:LinkButton id="LnkBtnRepot" onclick="LnkBtnRepot_Click" runat="server" Width="133px" CausesValidation="False" __designer:wfdid="w7">User audit trial report</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="Button2" onclick="Button2_Click" runat="server" CausesValidation="False" Text="Hide Report" __designer:wfdid="w8" CssClass="btnStyle_large"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 69px"></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="155px" ForeColor="White" __designer:wfdid="w76" ErrorMessage="Select a staff" ControlToValidate="DropDownList1"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ForeColor="White" __designer:wfdid="w81" ErrorMessage="enter a username" ControlToValidate="TextUserName"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" __designer:wfdid="w77" TargetControlID="RequiredFieldValidator1"></cc1:ValidatorCalloutExtender><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" __designer:wfdid="w82" TargetControlID="RequiredFieldValidator2"></cc1:ValidatorCalloutExtender><BR />&nbsp;<BR /><asp:Panel id="pnlMessage" runat="server" __designer:dtid="562958543355914" __designer:wfdid="w186" _="" _designer:dtid="562958543355909" CssClass="ModalWindow"><asp:Panel id="Panel6" runat="server" Width="99%" Height="35px" BackColor="LightSteelBlue" __designer:dtid="562958543355915" __designer:wfdid="w187" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Width="238px" ForeColor="MediumBlue" Text="Tsunami ARMS - Confirmation" Font-Bold="True" __designer:dtid="562958543355916" __designer:wfdid="w5"></asp:Label><asp:Label id="lblHead2" runat="server" Width="239px" ForeColor="MediumBlue" Text="Tsunami ARMS - Warning" Font-Bold="True" __designer:wfdid="w6"></asp:Label></asp:Panel> <asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px" __designer:dtid="562958543355918" __designer:wfdid="w190"><TABLE style="WIDTH: 237px" __designer:dtid="562958543355919"><TBODY><TR __designer:dtid="562958543355920"><TD align=center colSpan=1 __designer:dtid="562958543355921"></TD><TD align=center colSpan=3 __designer:dtid="562958543355922"><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" __designer:dtid="562958543355923" __designer:wfdid="w191" Font-Size="Small"></asp:Label></TD></TR><TR __designer:dtid="562958543355928"><TD __designer:dtid="562958543355929"></TD><TD align=center __designer:dtid="562958543355930">&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" Font-Bold="True" __designer:dtid="562958543355931" __designer:wfdid="w192" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" Font-Bold="True" __designer:dtid="562958543355932" __designer:wfdid="w193" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center __designer:dtid="562958543355933">&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px" __designer:dtid="562958543355938" __designer:wfdid="w194"><TABLE style="WIDTH: 237px" __designer:dtid="562958543355939"><TBODY><TR __designer:dtid="562958543355940"><TD align=center colSpan=1 __designer:dtid="562958543355941"></TD><TD align=center colSpan=3 __designer:dtid="562958543355942"><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" __designer:dtid="562958543355943" __designer:wfdid="w195" Font-Size="Small"></asp:Label></TD></TR><TR __designer:dtid="562958543355948"><TD __designer:dtid="562958543355949"></TD><TD align=center __designer:dtid="562958543355950">&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" ForeColor="Black" Text="OK" Font-Bold="True" __designer:dtid="562958543355951" __designer:wfdid="w196" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center __designer:dtid="562958543355952">&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> <cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" __designer:dtid="562958543355957" __designer:wfdid="w197" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize"></cc1:ModalPopupExtender> <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden" __designer:dtid="562958543355909" __designer:wfdid="w198"></asp:Button></asp:Panel><BR __designer:dtid="562958543355912" /><BR />
</contenttemplate>
<Triggers>
    <asp:PostBackTrigger ControlID="LinkButton1" />
    <asp:PostBackTrigger ControlID="LnkBtnRepot" />
    </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="ContentPlaceHolder3">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        <p>
            &nbsp;</p>
        <p>
            Select the level which&nbsp; you want</p>
        <p>Select staff name and&nbsp; enter the Username and Password</p>
        <p>The list of forms accessible to the level ara listed&nbsp; in user privileges.</p><p>According to the level selected the default form&nbsp; and executive override policy
                get loaded in corresponding tesxt area which cannot be edited.&nbsp; </p>
    </asp:Panel>
</asp:Content>

