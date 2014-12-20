<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ComplaintMaster.aspx.cs" Inherits="ComplaintMaster" Title="Complaint Master Page" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%--<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        <br />
                    This form is used for storing complaint details 
        <br />
        <br />
        User can assign urgency level,frequency,policy applicable perid for entered 
                    complaint.<br />
        <br />
        User can also assign actions to be taken for entered complaint.<br />
        <br />
        User can also assign the team responsible to act for entered complaint. 
        <br />
        <br />
        Authorised user can edit,delete or print complaint details.<br />
        <br />
        User can see the report on policy history,current policy,policy between entered 
                    dates.</asp:Panel>
    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 100%"><TBODY><TR><TD align=center colSpan=2><asp:Label id="lblcomplaintmaster" runat="server" Width="371px" ForeColor="#000099" Text="Complaint Master & Policy" Font-Bold="True" __designer:wfdid="w1" CssClass="heading" Font-Size="14pt" Font-Names="Arial"></asp:Label> </TD></TR><TR><TD vAlign=top><asp:Panel id="Panel1" runat="server" Width="100%" GroupingText="Complaint Details" __designer:wfdid="w25"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblCat" runat="server" Width="137px" Text="Complaint Category" __designer:wfdid="w26"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbCategory" tabIndex=1 runat="server" Width="200px" Height="22px" __designer:wfdid="w73" OnSelectedIndexChanged="cmbCategory_SelectedIndexChanged4" AutoPostBack="True" DataTextField="cmp_cat_name" DataValueField="cmp_category_id"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px"><asp:LinkButton id="LinkButton1" onclick="LinkButton1_Click" runat="server" CausesValidation="False" ForeColor="Blue" __designer:wfdid="w74">New</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblComplaint" runat="server" Width="144px" Text="Complaint Name" __designer:wfdid="w27"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtComplaint" tabIndex=2 runat="server" Width="200px" Height="18px" __designer:wfdid="w76" CssClass="UpperCaseFirstLetter" AutoPostBack="True" MaxLength="20" OnTextChanged="txtComplaint_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblUrgency" runat="server" Text="Urgency Level" __designer:wfdid="w77"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbUrgency" tabIndex=3 runat="server" Width="200px" Height="22px" __designer:wfdid="w78" OnSelectedIndexChanged="cmbUrgency_SelectedIndexChanged" AutoPostBack="True" DataTextField="urgname" DataValueField="urg_cmp_id"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px"><asp:LinkButton id="LinkButton3" onclick="LinkButton3_Click" runat="server" CausesValidation="False" ForeColor="Blue" __designer:wfdid="w79">New</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="Label7" runat="server" Width="114px" Text="Time Required" __designer:wfdid="w96"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txttimereqforcompletetask" tabIndex=7 runat="server" Width="125px" Height="16px" __designer:wfdid="w97" AutoPostBack="True" OnTextChanged="txttimereqforcompletetask_TextChanged"></asp:TextBox>&nbsp;Hr</TD><TD style="WIDTH: 100px"><asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" __designer:wfdid="w98" ControlToValidate="txttimereqforcompletetask" ErrorMessage="hh:mm" ValidationExpression="^([01][0-9]|[0-9][0-9]):[0-5][0-9]$"></asp:RegularExpressionValidator></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="Panel9" runat="server" Width="100%" GroupingText="Policy Details" __designer:wfdid="w21"><TABLE><TBODY><TR><TD style="WIDTH: 128px; TEXT-ALIGN: left" vAlign=top><asp:Label id="txtpolicy" runat="server" Width="93px" Text="Policy Type" __designer:wfdid="w85"></asp:Label> </TD><TD style="WIDTH: 123px" vAlign=top><asp:DropDownList id="cmbPolicy" tabIndex=6 runat="server" Width="200px" Height="22px" __designer:wfdid="w86" OnSelectedIndexChanged="cmbPolicy_SelectedIndexChanged" DataValueField="policy_id" DataTextField="policy" AutoPostBack="True"></asp:DropDownList> </TD><TD style="WIDTH: 65px"></TD></TR><TR><TD style="WIDTH: 128px"><asp:Label id="lbl2" runat="server" Width="130px" Text="Policy ApplicableFrom " __designer:wfdid="w90"></asp:Label> </TD><TD style="WIDTH: 123px" vAlign=top><asp:TextBox id="txtfrmdate1" tabIndex=8 runat="server" Width="125px" Height="17px" __designer:wfdid="w91" AutoPostBack="True" OnTextChanged="txtfrmdate1_TextChanged"></asp:TextBox> </TD><TD style="WIDTH: 65px; TEXT-ALIGN: left" vAlign=top><asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" __designer:wfdid="w92" ErrorMessage="dd/MM/yyyy" ControlToValidate="txtfrmdate1" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator> </TD></TR><TR><TD style="WIDTH: 128px; HEIGHT: 24px"><asp:Label id="lbl" runat="server" Width="120px" Text="Policy Applicable To" __designer:wfdid="w93" Visible="False"></asp:Label> </TD><TD style="WIDTH: 123px; HEIGHT: 24px" vAlign=top><asp:TextBox id="txttodate" tabIndex=9 runat="server" Width="125px" Height="17px" __designer:wfdid="w94" Visible="False" AutoPostBack="True" OnTextChanged="txttodate_TextChanged"></asp:TextBox> </TD><TD style="WIDTH: 65px; HEIGHT: 24px; TEXT-ALIGN: left" vAlign=top><asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" __designer:wfdid="w95" ErrorMessage="dd/MM/yyyy" ControlToValidate="txttodate" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator> </TD></TR><TR><TD style="WIDTH: 128px"></TD><TD style="WIDTH: 123px" vAlign=top></TD><TD style="WIDTH: 65px; TEXT-ALIGN: left" vAlign=top></TD></TR></TBODY></TABLE></asp:Panel> </TD><TD vAlign=top rowSpan=2><asp:Panel id="Panel10" runat="server" GroupingText="Responsible Teams for Rectification"><TABLE style="WIDTH: 100%"><TBODY><TR><TD><asp:Label id="lblTeam0" runat="server" Width="111px" Text="Team Name" __designer:wfdid="w8"></asp:Label> </TD><TD><asp:DropDownList id="cmbTeam" tabIndex=4 runat="server" Width="200px" Height="22px" __designer:wfdid="w2" OnSelectedIndexChanged="cmbTeam_SelectedIndexChanged" ValidationGroup="task" AutoPostBack="True" DataTextField="teamname" DataValueField="team_id"><asp:ListItem></asp:ListItem>
</asp:DropDownList> </TD><TD>&nbsp;</TD></TR><TR><TD><asp:Label id="Label9" runat="server" Width="118px" Text="Task Action" __designer:wfdid="w3"></asp:Label> </TD><TD><asp:DropDownList id="cmbTask" tabIndex=5 runat="server" Width="200px" Height="22px" __designer:wfdid="w4" OnSelectedIndexChanged="cmbAction_SelectedIndexChanged" ValidationGroup="task" AutoPostBack="True" DataTextField="taskname" DataValueField="task_id"><asp:ListItem></asp:ListItem>
</asp:DropDownList> </TD><TD>&nbsp;<asp:LinkButton id="lnktask" onclick="lnktask_Click1" runat="server" CausesValidation="False" __designer:wfdid="w13" Visible="False">New</asp:LinkButton></TD></TR><TR><TD>&nbsp;</TD><TD><asp:Button id="btnAddTeam" onclick="btnAddTask_Click" runat="server" Text="Add Team" __designer:wfdid="w5" CssClass="btnStyle_medium" ValidationGroup="task"></asp:Button> </TD><TD>&nbsp;</TD></TR><TR><TD colSpan=3><asp:GridView id="dgTeam" runat="server" Width="100%" Height="100%" HorizontalAlign="Left" ForeColor="#333333" __designer:wfdid="w6" AutoGenerateColumns="False" AllowPaging="True" CellPadding="4" GridLines="None" OnPageIndexChanging="dgTeam_PageIndexChanging" DataKeyNames="task_id" PageSize="5">
<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>
<Columns>
<asp:BoundField DataField="team_id" HeaderText="TeamId" Visible="False"></asp:BoundField>
<asp:BoundField DataField="team" HeaderText="Team Name"></asp:BoundField>
<asp:BoundField DataField="task_id" HeaderText="Task Id" Visible="False"></asp:BoundField>
<asp:BoundField DataField="taskname" HeaderText="Task Name"></asp:BoundField>
<asp:TemplateField><ItemTemplate>
                                                        <asp:LinkButton ID="wrklnk" runat="server" CausesValidation="False" 
                                                            CommandArgument='<%#Eval("task_id").ToString()+","+Eval("team_id").ToString()%>' 
                                                            OnCommand="TeamDelete">Delete</asp:LinkButton>
                                                    
</ItemTemplate>
</asp:TemplateField>
</Columns>

<PagerStyle HorizontalAlign="Center" BackColor="#2461BF" ForeColor="White"></PagerStyle>

<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

<HeaderStyle HorizontalAlign="Left" BackColor="#507CD1" Font-Bold="True" ForeColor="White"></HeaderStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> </TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD vAlign=top><asp:Label id="lblCmpID" runat="server" Text="0" Visible="False"></asp:Label> </TD></TR><TR><TD align=center colSpan=2><asp:Button id="btnadd" tabIndex=10 onclick="btnadd_Click" runat="server" Width="70px" Text="Save" __designer:wfdid="w526" CssClass="btnStyle_small"></asp:Button> &nbsp;<asp:Button id="btndelete" onclick="btndelete_Click" runat="server" Width="70px" Text="Delete" __designer:wfdid="w527" CssClass="btnStyle_small"></asp:Button> &nbsp;<asp:Button id="Button1" onclick="Button1_Click" runat="server" Width="70px" CausesValidation="False" Text="Clear" __designer:wfdid="w529" CssClass="btnStyle_small"></asp:Button> &nbsp;<asp:Button id="Button2" onclick="Button2_Click" runat="server" Width="70px" CausesValidation="False" Text="Report" __designer:wfdid="w528" CssClass="btnStyle_small"></asp:Button> &nbsp; <asp:LinkButton id="backtoregister" onclick="backtoregister_Click" runat="server" CausesValidation="False" __designer:wfdid="w530" Visible="False">BACK 
                        TO FORM</asp:LinkButton> </TD></TR><TR><TD colSpan=2><asp:GridView id="dgcomplaint" runat="server" Width="728px" ForeColor="#333333" __designer:wfdid="w478" AutoGenerateColumns="False" AllowPaging="True" Caption="Complaint Details" CellPadding="4" GridLines="None" OnPageIndexChanging="dgcomplaint_PageIndexChanging" OnRowcreated="dgcomplaint_RowCreated" OnSelectedIndexChanged="dgcomplaint_SelectedIndexChanged1" DataKeyNames="complaint_id">
<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="complaint_id" HeaderText="Complaint ID"></asp:BoundField>
<asp:BoundField DataField="cmp_cat_name" HeaderText="Complaint Category"></asp:BoundField>
<asp:BoundField DataField="cmpname" HeaderText="Complaint Name"></asp:BoundField>
<asp:BoundField DataField="urgname" HeaderText="Urgency Level"></asp:BoundField>
<asp:BoundField DataField="policy" HeaderText="Policy Type"></asp:BoundField>
<asp:BoundField DataField="policy_id" HeaderText="Policy ID" Visible="False"></asp:BoundField>
</Columns>

<PagerStyle HorizontalAlign="Center" BackColor="#2461BF" ForeColor="White"></PagerStyle>

<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

<HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"></HeaderStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> </TD></TR><TR><TD colSpan=2><asp:Panel id="pnlreport" runat="server" Width="100%" GroupingText="Report" __designer:wfdid="w479" Visible="False"><TABLE style="WIDTH: 708px"><TBODY><TR><TD style="WIDTH: 629px; TEXT-ALIGN: center" colSpan=2><TABLE style="WIDTH: 687px"><TBODY><TR><TD style="WIDTH: 123px; HEIGHT: 18px"><asp:Button id="Button3" onclick="Button3_Click" runat="server" CausesValidation="False" Text="Hide panel" __designer:wfdid="w480" CssClass="btnStyle_large"></asp:Button></TD><TD style="WIDTH: 132px"><asp:Button id="Button4" onclick="Button4_Click" runat="server" CausesValidation="False" Text="Clear" __designer:wfdid="w481" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 43px"></TD><TD style="WIDTH: 36px; HEIGHT: 18px"></TD><TD style="WIDTH: 71px; HEIGHT: 18px"></TD></TR><TR><TD style="WIDTH: 123px; HEIGHT: 2px; TEXT-ALIGN: right" vAlign=top><asp:Label id="Label11" runat="server" Width="69px" Text="From Date" __designer:wfdid="w482"></asp:Label></TD><TD style="WIDTH: 132px; TEXT-ALIGN: left"><asp:TextBox id="txtreportfrom" runat="server" Width="120px" __designer:wfdid="w483"></asp:TextBox>&nbsp;&nbsp;</TD><TD style="WIDTH: 43px; TEXT-ALIGN: left"><asp:Label id="Label12" runat="server" Width="57px" Text="To Date" __designer:wfdid="w1"></asp:Label></TD><TD style="WIDTH: 36px; HEIGHT: 2px; TEXT-ALIGN: left"><asp:TextBox id="txtreportto" runat="server" Width="120px" __designer:wfdid="w2"></asp:TextBox></TD><TD style="WIDTH: 71px; HEIGHT: 2px"></TD><TD style="HEIGHT: 2px; TEXT-ALIGN: center" vAlign=top></TD></TR><TR><TD style="WIDTH: 123px; HEIGHT: 24px; TEXT-ALIGN: left"><asp:Label id="Label5" runat="server" Text="Complaint name" __designer:wfdid="w486" Font-Size="10pt"></asp:Label></TD><TD style="WIDTH: 132px; HEIGHT: 24px; TEXT-ALIGN: left"><asp:DropDownList id="cmbComplaint" runat="server" Width="128px" __designer:wfdid="w487" OnSelectedIndexChanged="cmbComplaint_SelectedIndexChanged" AutoPostBack="True" DataTextField="cmpname" DataValueField="complaint_id">
                                <asp:ListItem Value="-1">Select all</asp:ListItem>

                            </asp:DropDownList></TD><TD style="WIDTH: 43px; HEIGHT: 24px; TEXT-ALIGN: left"></TD><TD style="WIDTH: 36px; HEIGHT: 24px; TEXT-ALIGN: left"><asp:Label id="Label2" runat="server" Width="92px" Text="Policy Type" __designer:wfdid="w488"></asp:Label></TD><TD style="HEIGHT: 24px; TEXT-ALIGN: left" colSpan=2><asp:DropDownList id="cmbReportType" runat="server" Width="126px" __designer:wfdid="w489" OnSelectedIndexChanged="cmbReportType_SelectedIndexChanged" AutoPostBack="True"><asp:ListItem Value="All">All</asp:ListItem>
<asp:ListItem Value="1">Allot</asp:ListItem>
<asp:ListItem Value="2">Alarm &amp; Allot</asp:ListItem>
<asp:ListItem Value="3">Block</asp:ListItem>

                            </asp:DropDownList></TD></TR><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: left" colSpan=2><asp:Label id="Label10" runat="server" Width="259px" ForeColor="Red" Text="select Complaint name" Font-Bold="True" __designer:wfdid="w490"></asp:Label></TD><TD style="WIDTH: 43px; HEIGHT: 18px; TEXT-ALIGN: left" colSpan=1></TD><TD style="HEIGHT: 18px; TEXT-ALIGN: left" colSpan=4><asp:Label id="lblMessage" runat="server" Width="210px" ForeColor="Red" Text="select esential fields" Font-Bold="True" __designer:wfdid="w491"></asp:Label></TD></TR><TR><TD style="WIDTH: 123px; HEIGHT: 18px; TEXT-ALIGN: left"><asp:LinkButton id="LinkButton6" onclick="LinkButton6_Click" runat="server" Width="140px" CausesValidation="False" __designer:wfdid="w492">All complaintdetails</asp:LinkButton></TD><TD style="HEIGHT: 18px; TEXT-ALIGN: center" colSpan=2></TD><TD style="HEIGHT: 18px; TEXT-ALIGN: left"></TD><TD colSpan=2><asp:LinkButton id="LinkButton7" onclick="LinkButton7_Click" runat="server" CausesValidation="False" __designer:wfdid="w4">Policy history</asp:LinkButton>&nbsp; &nbsp;&nbsp;<asp:LinkButton id="LinkButton8" onclick="LinkButton8_Click" runat="server" CausesValidation="False" __designer:wfdid="w3">Current policy</asp:LinkButton>&nbsp;</TD><TD style="HEIGHT: 18px; TEXT-ALIGN: left" colSpan=1></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> </TD></TR></TBODY></TABLE><TABLE style="WIDTH: 430px; HEIGHT: 100%"><TBODY><TR><TD style="TEXT-ALIGN: center" colSpan=2>&nbsp;</TD></TR><TR><TD vAlign=top rowSpan=1>&nbsp;</TD><TD vAlign=top colSpan=1 rowSpan=1>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR><TR><TD vAlign=top>&nbsp;</TD><TD vAlign=top colSpan=1>&nbsp;</TD></TR><TR><TD style="HEIGHT: 58px; TEXT-ALIGN: center" vAlign=top colSpan=2><TABLE><TBODY><TR><TD style="HEIGHT: 30px" vAlign=middle>&nbsp;</TD><TD style="HEIGHT: 30px" vAlign=middle>&nbsp;</TD><TD style="HEIGHT: 30px" vAlign=middle>&nbsp;</TD><TD style="HEIGHT: 30px" vAlign=middle>&nbsp;</TD><TD style="HEIGHT: 30px; TEXT-ALIGN: left" vAlign=middle></TD><TD style="HEIGHT: 30px" vAlign=middle>&nbsp;&nbsp;&nbsp; </TD><TD vAlign=middle>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE><TABLE><TBODY><TR><TD style="WIDTH: 258px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtendercategory" runat="server" __designer:wfdid="w54" TargetControlID="RequiredFieldValidator1"></cc1:ValidatorCalloutExtender>&nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtenderUrgency" runat="server" __designer:wfdid="w56" TargetControlID="RequiredFieldValidator3"></cc1:ValidatorCalloutExtender>&nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtenderPolicy" runat="server" __designer:wfdid="w58" TargetControlID="RequiredFieldValidator5"></cc1:ValidatorCalloutExtender>&nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtenderfromdate" runat="server" __designer:wfdid="w60" TargetControlID="RequiredFieldValidator7"></cc1:ValidatorCalloutExtender>&nbsp; <cc1:FilteredTextBoxExtender id="FilteredTextBoxExtender2time" runat="server" __designer:wfdid="w48" TargetControlID="txttimereqforcompletetask" FilterType="Custom, Numbers" ValidChars=":"></cc1:FilteredTextBoxExtender> <cc1:ValidatorCalloutExtender id="vceTeam" runat="server" __designer:wfdid="w62" TargetControlID="rfvTeam"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="vceTask" runat="server" __designer:wfdid="w8" TargetControlID="rfvTask"></cc1:ValidatorCalloutExtender><BR /><cc1:FilteredTextBoxExtender id="FilteredTextBoxExtendercomplaint" runat="server" __designer:wfdid="w47" TargetControlID="txtComplaint" FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars=". " InvalidChars="Numbers"></cc1:FilteredTextBoxExtender><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender10" runat="server" __designer:wfdid="w49" TargetControlID="CompareValidator2"></cc1:ValidatorCalloutExtender> <cc1:CalendarExtender id="CalendarExtender1" runat="server" __designer:wfdid="w1" TargetControlID="txtfrmdate1" Format="dd-MM-yyyy"></cc1:CalendarExtender> <cc1:CalendarExtender id="CalendarExtender2" runat="server" __designer:wfdid="w2" TargetControlID="txttodate" Format="dd-MM-yyyy"></cc1:CalendarExtender> <cc1:CalendarExtender id="CalendarExtender3" runat="server" __designer:wfdid="w6" TargetControlID="txtreportfrom" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:CalendarExtender id="CalendarExtender4" runat="server" __designer:wfdid="w7" TargetControlID="txtreportto" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:ListSearchExtender id="lstcategory" runat="server" __designer:wfdid="w5" TargetControlID="cmbCategory"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="lsturgency" runat="server" __designer:wfdid="w6" TargetControlID="cmbUrgency"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="lstteam" runat="server" __designer:wfdid="w7" TargetControlID="cmbTeam"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="lstaction" runat="server" __designer:wfdid="w8" TargetControlID="cmbTask"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="lstpolicy" runat="server" __designer:wfdid="w9" TargetControlID="cmbPolicy"></cc1:ListSearchExtender></TD><TD style="WIDTH: 382px" vAlign=top rowSpan=2><asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden" __designer:dtid="562958543355909" __designer:wfdid="w1"></asp:Button> <asp:Panel id="pnlMessage" runat="server" __designer:dtid="562958543355914" __designer:wfdid="w27" CssClass="ModalWindow" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel3" runat="server" BackColor="LightSteelBlue" __designer:dtid="562958543355915" __designer:wfdid="w28" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" __designer:dtid="562958543355916" Text="Tsunami ARMS - Confirmation" __designer:wfdid="w29" Font-Bold="True" ForeColor="MediumBlue"></asp:Label><BR /><asp:Label id="lblHead2" runat="server" Text="Tsunami ARMS - Warning" __designer:wfdid="w55" Font-Bold="True" ForeColor="MediumBlue"></asp:Label></asp:Panel> <asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="100%" __designer:dtid="562958543355918" __designer:wfdid="w30"><TABLE style="WIDTH: 237px" __designer:dtid="562958543355919"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3></TD></TR><TR __designer:dtid="562958543355920"><TD align=center colSpan=1 __designer:dtid="562958543355921"></TD><TD align=center colSpan=3 __designer:dtid="562958543355922"><asp:Label id="lblMsg" runat="server" Width="179px" ForeColor="Black" Text="Do you want to save?" __designer:dtid="562958543355923" __designer:wfdid="w31" Font-Size="Small"></asp:Label></TD></TR><TR __designer:dtid="562958543355928"><TD __designer:dtid="562958543355929"></TD><TD align=center __designer:dtid="562958543355930">&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" Font-Bold="True" __designer:dtid="562958543355931" __designer:wfdid="w32" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" Font-Bold="True" __designer:dtid="562958543355932" __designer:wfdid="w33" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center __designer:dtid="562958543355933">&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="100%" __designer:dtid="562958543355938" __designer:wfdid="w34"><TABLE style="WIDTH: 237px" __designer:dtid="562958543355939"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3></TD></TR><TR __designer:dtid="562958543355940"><TD align=center colSpan=1 __designer:dtid="562958543355941"></TD><TD align=center colSpan=3 __designer:dtid="562958543355942"><asp:Label id="lblOk" runat="server" Width="216px" ForeColor="Black" Text="Do you want to ?" __designer:dtid="562958543355943" __designer:wfdid="w35" Font-Size="Small"></asp:Label></TD></TR><TR __designer:dtid="562958543355948"><TD __designer:dtid="562958543355949"></TD><TD align=center __designer:dtid="562958543355950">&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" Font-Bold="True" __designer:dtid="562958543355951" __designer:wfdid="w36" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center __designer:dtid="562958543355952">&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> <cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" __designer:dtid="562958543355957" __designer:wfdid="w39" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize">
                </cc1:ModalPopupExtender></asp:Panel>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR><TR><TD style="WIDTH: 258px; HEIGHT: 113px" vAlign=top>&nbsp;<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ForeColor="White" __designer:wfdid="w68" ControlToValidate="cmbCategory" ErrorMessage="select a category" InitialValue="-1"></asp:RequiredFieldValidator> <BR /><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ForeColor="White" __designer:wfdid="w69" ControlToValidate="txtComplaint" ErrorMessage="Enter the complaint"></asp:RequiredFieldValidator> <BR /><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ForeColor="White" __designer:wfdid="w70" ControlToValidate="cmbUrgency" ErrorMessage="Select an urgency level or input a new using the New link" InitialValue="-1"></asp:RequiredFieldValidator> <BR /><BR /><asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" ForeColor="White" __designer:wfdid="w72" ControlToValidate="cmbPolicy" ErrorMessage="Select one" InitialValue="-1"></asp:RequiredFieldValidator> <BR /><BR /><asp:RequiredFieldValidator id="RequiredFieldValidator7" runat="server" ForeColor="White" __designer:wfdid="w74" ControlToValidate="txtfrmdate1" ErrorMessage="Enter the from date which you want to set the work"></asp:RequiredFieldValidator> <BR /><BR /><asp:RequiredFieldValidator id="rfvTeam" runat="server" ForeColor="White" __designer:wfdid="w76" ControlToValidate="cmbTeam" ErrorMessage="Select a team" InitialValue="-1" ValidationGroup="task"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="rfvTask" runat="server" ForeColor="White" __designer:wfdid="w7" ControlToValidate="cmbTask" ErrorMessage="Select task" InitialValue="-1" ValidationGroup="task"></asp:RequiredFieldValidator> <asp:CompareValidator id="CompareValidator2" runat="server" Width="239px" ForeColor="White" __designer:wfdid="w81" Visible="False" ControlToValidate="txttodate" ErrorMessage="To date should be greater than fromdate" ControlToCompare="txtfrmdate1" Operator="GreaterThan" Type="Date"></asp:CompareValidator> </TD></TR></TBODY></TABLE>&nbsp;&nbsp; 
</contenttemplate>
 <Triggers>
   
    <asp:PostBackTrigger ControlID="LinkButton8" />
    <asp:PostBackTrigger ControlID="LinkButton7" />
    <asp:PostBackTrigger ControlID="LinkButton6" />
   
  </Triggers>
    </asp:UpdatePanel>
  
</asp:Content>


                                              

