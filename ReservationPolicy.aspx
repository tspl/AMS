<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReservationPolicy.aspx.cs" Inherits="ReservationPolicy" Title="Tsunami ARMS- Reservation Policy" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%--<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
<TABLE><TBODY><TR><TD style="TEXT-ALIGN: center" colSpan=2><asp:Label id="lblheading" runat="server" Font-Size="Larger" Font-Bold="True" Text="Reservation Policy"></asp:Label></TD></TR><TR><TD><asp:Panel id="Panel1" runat="server" Width="100%" GroupingText="Reservation policy details" Height="100%"><TABLE><TBODY><TR><TD style="HEIGHT: 39px"><asp:Label id="lblreservationtype" runat="server" Width="106px" Font-Bold="False" Text="Reservation Type"></asp:Label></TD><TD style="HEIGHT: 39px"><asp:DropDownList id="cmbtype" runat="server" Width="140px" __designer:wfdid="w11"><asp:ListItem Value="-1">--Select--</asp:ListItem>
<asp:ListItem>Donor Free</asp:ListItem>
<asp:ListItem>Donor Paid</asp:ListItem>
<asp:ListItem>Tdb</asp:ListItem>
<asp:ListItem>General</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD><asp:Label id="lblrcharge" runat="server" Width="56px" Font-Bold="False" Text="Amount"></asp:Label></TD><TD><asp:TextBox id="txtRCamount" runat="server" Width="112px" MaxLength="3">0</asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel> </TD><TD><asp:Panel id="Panel2" runat="server" Width="100%" GroupingText="No of days of reservation" Height="100%"><TABLE><TBODY><TR><TD><asp:Label id="lblrmaxdays" runat="server" Width="64px" Text="Max days" Font-Bold="False"></asp:Label> </TD><TD><asp:Label id="Label1" runat="server" Width="64px" Text="Min days" Font-Bold="False"></asp:Label></TD><TD><asp:Label id="Label2" runat="server" Width="112px" Text="Max stay possible" Font-Bold="False"></asp:Label></TD></TR><TR><TD><asp:TextBox id="txtmaxdays" runat="server" Width="47px" MaxLength="3" OnTextChanged="txtmaxdays_TextChanged"></asp:TextBox></TD><TD><asp:TextBox id="txtmindays" runat="server" Width="47px" MaxLength="2" OnTextChanged="txtmindays_TextChanged"></asp:TextBox></TD><TD><asp:TextBox id="txtmaxstay" runat="server" Width="47px" MaxLength="2" OnTextChanged="txtmaxstay_TextChanged"></asp:TextBox></TD></TR></TBODY></TABLE><BR /></asp:Panel> </TD></TR><TR><TD><asp:Panel id="Panel3" runat="server" Width="100%" GroupingText="Postponement details" Height="100%"><TABLE><TBODY><TR><TD><asp:Label id="lblpostponeyn" runat="server" Width="104px" Font-Bold="False" Text="Postpone"></asp:Label> </TD><TD style="WIDTH: 7px"><asp:Label id="lblRCpostAmt" runat="server" Text="Amount"></asp:Label></TD><TD style="WIDTH: 7px"><asp:Label id="lblpostno" runat="server" Width="40px" Font-Bold="False" Text="Count"></asp:Label></TD></TR><TR><TD colSpan=1><asp:DropDownList id="cmbpostpon" runat="server" Width="140px" __designer:wfdid="w8" OnSelectedIndexChanged="cmbpostpon_SelectedIndexChanged" AutoPostBack="True"><asp:ListItem Value="-1">--Select--</asp:ListItem>
<asp:ListItem Value="1">Allowed</asp:ListItem>
<asp:ListItem Value="0">Not Allowed</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 7px"><asp:TextBox id="txtpostamt" runat="server" Width="40px" MaxLength="3">0</asp:TextBox></TD><TD style="WIDTH: 7px"><asp:TextBox id="txtpostno" runat="server" Width="32px" MaxLength="2">0</asp:TextBox></TD></TR><TR><TD colSpan=2><asp:Label id="lblpostnoofdays" runat="server" Width="228px" Font-Bold="False" Text="No: of days in which the postponement information should be given"></asp:Label></TD><TD style="WIDTH: 7px"><asp:TextBox id="txtpostnoofdys" runat="server" Width="40px" MaxLength="2">0</asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="Panel4" runat="server" Width="100%" GroupingText="Preponement Details" Height="100%" __designer:wfdid="w6"><TABLE><TBODY><TR><TD><asp:Label id="lblprepone" runat="server" Width="104px" Font-Bold="False" Text="Prepone" __designer:wfdid="w7"></asp:Label></TD><TD style="WIDTH: 7px"><asp:Label id="lblpreamt" runat="server" Width="51px" Text="Amount" __designer:wfdid="w8"></asp:Label></TD><TD style="WIDTH: 7px"><asp:Label id="lblpreno" runat="server" Width="32px" Font-Bold="False" Text="Count" __designer:wfdid="w9"></asp:Label></TD></TR><TR><TD colSpan=1><asp:DropDownList id="cmbprepon" runat="server" Width="140px" __designer:wfdid="w9" OnSelectedIndexChanged="cmbprepon_SelectedIndexChanged" AutoPostBack="True"><asp:ListItem Value="-1">--Select--</asp:ListItem>
<asp:ListItem Value="1">Allowed</asp:ListItem>
<asp:ListItem Value="0">Not Allowed</asp:ListItem>
</asp:DropDownList></TD><TD><asp:TextBox id="txtpreamt" runat="server" Width="40px" __designer:wfdid="w11" MaxLength="3">0</asp:TextBox></TD><TD><asp:TextBox id="txtpreno" runat="server" Width="40px" __designer:wfdid="w12" MaxLength="2">0</asp:TextBox></TD></TR><TR><TD colSpan=2><asp:Label id="lblprenoofdays" runat="server" Width="224px" Font-Bold="False" Text="No: of days in which the preponement information should be given" __designer:wfdid="w13"></asp:Label></TD><TD style="WIDTH: 7px"><asp:TextBox id="txtprenoofdys" runat="server" Width="40px" __designer:wfdid="w14" MaxLength="2">0</asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="Panel5" runat="server" Width="100%" GroupingText="Cancellation Details" Height="100%" __designer:wfdid="w15"><TABLE><TBODY><TR><TD style="WIDTH: 120px; HEIGHT: 18px"><asp:Label id="lblcancelyn" runat="server" Width="104px" Font-Bold="False" Text="Cancellation" __designer:wfdid="w16"></asp:Label></TD><TD style="HEIGHT: 18px"><asp:Label id="lblcancelcharge" runat="server" Width="47px" Font-Bold="False" Text="Amount" __designer:wfdid="w17"></asp:Label></TD><TD style="HEIGHT: 18px"><asp:Label id="lblcancelno" runat="server" Width="40px" Font-Bold="False" Text="Count" __designer:wfdid="w18"></asp:Label></TD></TR><TR><TD><asp:DropDownList id="cmbcanc" runat="server" Width="140px" __designer:wfdid="w10" OnSelectedIndexChanged="cmbcanc_SelectedIndexChanged" AutoPostBack="True"><asp:ListItem Value="-1">--Select--</asp:ListItem>
<asp:ListItem Value="1">Allowed</asp:ListItem>
<asp:ListItem Value="0">Not Allowed</asp:ListItem>
</asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp; </TD><TD><asp:TextBox id="txtcanclamt" runat="server" Width="48px" __designer:wfdid="w20" MaxLength="3">0</asp:TextBox></TD><TD><asp:TextBox id="txtcanclno" runat="server" Width="48px" __designer:wfdid="w21" MaxLength="2">0</asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel></TD><TD><asp:Panel id="Panel9" runat="server" Width="100%" GroupingText="Policy period" __designer:wfdid="w1"><TABLE><TBODY><TR><TD style="WIDTH: 120px"><asp:Label id="Label11" runat="server" Width="132px" Text="Policy applicable From" Font-Bold="False" __designer:wfdid="w2"></asp:Label></TD><TD colSpan=2><asp:TextBox id="txtfrmdate" runat="server" Width="120px" __designer:wfdid="w3" OnTextChanged="txtfrmdate_TextChanged"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 120px"><asp:Label id="Label12" runat="server" Width="129px" Text="Policy applicable To" Font-Bold="False" __designer:wfdid="w4"></asp:Label></TD><TD colSpan=2><asp:TextBox id="txttodate" runat="server" Width="120px" __designer:wfdid="w5" OnTextChanged="txttodate_TextChanged" AutoPostBack="True"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 120px"></TD><TD colSpan=2></TD></TR><TR><TD style="WIDTH: 120px"></TD><TD colSpan=2></TD></TR><TR><TD style="WIDTH: 120px"><asp:Label id="Label6" runat="server" Width="136px" Text="Applicable Seasons" __designer:wfdid="w26"></asp:Label></TD><TD colSpan=2 rowSpan=2><asp:ListBox id="lstseason" runat="server" Width="125px" Height="128px" __designer:wfdid="w27" SelectionMode="Multiple"></asp:ListBox></TD></TR><TR><TD style="WIDTH: 120px"><asp:TextBox id="txtpolicyid" runat="server" Width="10px" Height="10px" Visible="False" __designer:wfdid="w28"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 120px"></TD><TD colSpan=2></TD></TR><TR><TD style="WIDTH: 120px; HEIGHT: 18px"></TD><TD colSpan=2></TD></TR></TBODY></TABLE><BR /></asp:Panel> </TD></TR><TR><TD colSpan=2><asp:Panel id="pnlrent" runat="server" Width="100%" GroupingText="Rent Details"><TABLE><TBODY><TR><TD vAlign=top><asp:Label id="lblrentapplicable" runat="server" Width="131px" Text="Rent applicable or not"></asp:Label> </TD><TD vAlign=top><asp:DropDownList id="cmbRentApplicable" runat="server" Width="150px"><asp:ListItem>-- Select --</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
<asp:ListItem Value="0">No</asp:ListItem>
</asp:DropDownList></TD>
    <td valign="top">
        &nbsp;</td>
    <td valign="top">
        <asp:Label ID="lblsecurityamount0" runat="server" 
            Text="Security deposit amount" Width="142px">
        </asp:Label>
    </td>
    <td valign="top">
        <asp:DropDownList ID="cmbSecurityDeposit" runat="server" 
            Width="150px">
            <asp:ListItem>-- Select --</asp:ListItem>
            <asp:ListItem Value="1">Yes</asp:ListItem>
            <asp:ListItem Value="0">No</asp:ListItem>
        </asp:DropDownList>
    </td>
    </TR></TBODY></TABLE></asp:Panel></TD></TR>
    <tr>
        <td colspan="2">
            &nbsp;<table>
                <tbody>
                    <tr>
                        <td colspan="6" style="HEIGHT: 27px">
                            <asp:Label ID="Label13" runat="server" 
                                Text="Pre Reservation days"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtPre" runat="server" Width="112px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="HEIGHT: 27px">
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                        </td>
                        <td style="HEIGHT: 27px">
                            <asp:Button ID="btnsave" runat="server" 
                                CssClass="btnStyle_small" onclick="btnsave_Click" 
                                Text="Save" Width="70px" />
                        </td>
                        <td style="HEIGHT: 27px">
                            <asp:Button ID="btnedit" runat="server" 
                                CssClass="btnStyle_small" onclick="btnedit_Click" 
                                Text="Edit" Width="70px" />
                        </td>
                        <td style="HEIGHT: 27px">
                            <asp:Button ID="btnclr" runat="server" 
                                CausesValidation="False" CssClass="btnStyle_small" 
                                onclick="btnclr_Click" OnClientClick="btnclr_Click" 
                                Text="Clear" Width="70px" />
                        </td>
                        <td style="WIDTH: 69px; HEIGHT: 27px">
                            <asp:Button ID="btndelete" runat="server" 
                                CssClass="btnStyle_small" Enabled="False" 
                                onclick="btndelete_Click" Text="Delete" Width="70px" />
                        </td>
                        <td style="WIDTH: 68px; HEIGHT: 27px">
                            <asp:Button ID="btnreport" runat="server" 
                                CausesValidation="False" CssClass="btnStyle_small" 
                                onclick="btnreport_Click" Text="Report" Width="70px" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <TR><TD colSpan=2><asp:GridView id="gdrespolicy" runat="server" Width="100%" Caption="List of Policy details" OnSelectedIndexChanged="gdrespolicy_SelectedIndexChanged" OnPageIndexChanging="gdrespolicy_PageIndexChanging" CellPadding="4" OnRowCreated="gdrespolicy_RowCreated" AllowPaging="True" PageSize="5" ForeColor="#333333" AutoGenerateColumns="False" OnSelectedIndexChanging="gdrespolicy_SelectedIndexChanging" DataKeyNames="Serialno" GridLines="None">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="Serialno" Visible="False" HeaderText="id"></asp:BoundField>
<asp:BoundField DataField="TYPE" HeaderText="Type"></asp:BoundField>
<asp:BoundField DataField="AMOUNT" HeaderText="Amount"></asp:BoundField>
<asp:BoundField DataField="Max days" HeaderText="Max days"></asp:BoundField>
<asp:BoundField DataField="Max days" HeaderText="Min days"></asp:BoundField>
<asp:BoundField DataField="Max stay" HeaderText="Max stay"></asp:BoundField>
<asp:BoundField DataField="Res From" HeaderText="From"></asp:BoundField>
<asp:BoundField DataField="Res To" HeaderText="To"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True" VerticalAlign="Top"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR><TR><TD colSpan=2>&nbsp;<asp:Panel id="pnlreport" runat="server" Width="100%" GroupingText="Report" Height="50px"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblreporttype" runat="server" Width="104px" Text="Reservation type"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbRep" runat="server" Width="140px" __designer:wfdid="w13"><asp:ListItem Value="All">All</asp:ListItem>
<asp:ListItem>Donor Free</asp:ListItem>
<asp:ListItem>Donor Paid</asp:ListItem>
<asp:ListItem>Tdb</asp:ListItem>
<asp:ListItem>General</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px">&nbsp;</TD><TD style="WIDTH: 100px"><asp:Button id="btnhide" onclick="btnhide_Click" runat="server" CausesValidation="False" Text="Close Report" CssClass="btnStyle_large"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblreportfrom" runat="server" Text="From date"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtreportfrom" runat="server" Width="104px"></asp:TextBox></TD><TD>&nbsp;&nbsp; </TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblreportto" runat="server" Width="56px" Text="To date"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtreportto" runat="server" Width="104px"></asp:TextBox></TD><TD colSpan=2><asp:Label id="lblmessage" runat="server" Width="160px" Visible="False" Text="Select all essential fields" ForeColor="Red"></asp:Label> </TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 18px"></TD><TD style="WIDTH: 100px; HEIGHT: 18px"></TD><TD style="HEIGHT: 18px" colSpan=2></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="btnfetchpolicy" onclick="btnfetchpolicy_Click" runat="server" CausesValidation="False" Text="Fetch policy" CssClass="btnStyle_large"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="btnpolicyhis" onclick="btnpolicyhis_Click" runat="server" CausesValidation="False" Text="Policy history" CssClass="btnStyle_large"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="btncurrentpolicy" onclick="btncurrentpolicy_Click" runat="server" CausesValidation="False" Text="Current policy" CssClass="btnStyle_large"></asp:Button></TD><TD style="WIDTH: 100px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:Button id="btnreportclear" onclick="btnreportclear_Click" runat="server" CausesValidation="False" Text="Clear" CssClass="btnStyle_small"></asp:Button></TD></TR><TR><TD colSpan=2></TD><TD colSpan=2></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=2></TD></TR><TR><TD><asp:Panel id="Panel10" runat="server" Width="125px" Height="50px"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="208px" ForeColor="White" SetFocusOnError="True" ControlToValidate="cmbtype" ErrorMessage="Please select a Reservation type"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Width="208px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtmaxdays" ErrorMessage="Maximum days before which reservation can be made is required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" Width="240px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtmindays" ErrorMessage="Minimum days before which reservation has to be made is required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" Width="272px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtmaxstay" ErrorMessage="Maximum days of stay allowed for the user is required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" Width="264px" ForeColor="White" SetFocusOnError="True" ControlToValidate="cmbpostpon" ErrorMessage="Please select postponemnet allowed or not"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator7" runat="server" Width="248px" ForeColor="White" SetFocusOnError="True" ControlToValidate="cmbprepon" ErrorMessage="Please select preponemnet allowed or not"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="Required" runat="server" Width="248px" ForeColor="White" SetFocusOnError="True" ControlToValidate="cmbcanc" ErrorMessage="Please select cancellation  allowed or not"></asp:RequiredFieldValidator><asp:RequiredFieldValidator id="RequiredFieldValidator8" runat="server" Width="168px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtfrmdate" ErrorMessage="Policy from date is required"></asp:RequiredFieldValidator>&nbsp; <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" Width="184px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtRCamount" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,3}"></asp:RegularExpressionValidator><asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" Width="184px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtmaxdays" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,3}"></asp:RegularExpressionValidator> <asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" Width="176px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtmindays" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,2}"></asp:RegularExpressionValidator> <asp:RegularExpressionValidator id="RegularExpressionValidator4" runat="server" Width="176px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtmaxstay" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,2}"></asp:RegularExpressionValidator><asp:RegularExpressionValidator id="revpostamt" runat="server" Width="176px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtpostamt" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,3}"></asp:RegularExpressionValidator> <asp:RegularExpressionValidator id="revpostno" runat="server" Width="168px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtpostno" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,2}"></asp:RegularExpressionValidator> <asp:RegularExpressionValidator id="revpostnoofdays" runat="server" Width="184px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtpostnoofdys" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,2}"></asp:RegularExpressionValidator> <asp:RegularExpressionValidator id="revpreamt" runat="server" Width="192px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtpreamt" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,3}"></asp:RegularExpressionValidator> <asp:RegularExpressionValidator id="revprenoofdays" runat="server" Width="184px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtpreno" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,2}"></asp:RegularExpressionValidator> <asp:RegularExpressionValidator id="revpreno" runat="server" Width="184px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtprenoofdys" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,2}"></asp:RegularExpressionValidator> <asp:RegularExpressionValidator id="revcancelamt" runat="server" Width="184px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtcanclamt" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,3}"></asp:RegularExpressionValidator><asp:RegularExpressionValidator id="revcancelno" runat="server" Width="176px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtcanclno" ErrorMessage="Only Numbers are allowed" ValidationExpression="[0-9]{1,2}"></asp:RegularExpressionValidator><asp:RequiredFieldValidator id="RequiredFieldValidator22" runat="server" Width="173px" ForeColor="White" SetFocusOnError="True" ControlToValidate="lstseason" ErrorMessage="Select atleast one season"></asp:RequiredFieldValidator> <asp:RegularExpressionValidator id="RegularExpressionValidator13" runat="server" Width="208px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txtfrmdate" ErrorMessage="Date format should be dd/mm/yyyy" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator> <asp:RegularExpressionValidator id="RegularExpressionValidator14" runat="server" Width="208px" ForeColor="White" SetFocusOnError="True" ControlToValidate="txttodate" ErrorMessage="Date format should be dd/mm/yyyy" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator></asp:Panel><cc1:calendarextender id="calextfrm" runat="server" targetcontrolid="txtfrmdate" Format="dd/MM/yyyy"></cc1:calendarextender> <cc1:calendarextender id="calextto" runat="server" targetcontrolid="txttodate" Format="dd/MM/yyyy"></cc1:calendarextender> <cc1:CalendarExtender id="cereportfrom" runat="server" Format="dd/MM/yyyy" TargetControlID="txtreportfrom"></cc1:CalendarExtender> <cc1:CalendarExtender id="cereportto" runat="server" Format="dd/MM/yyyy" TargetControlID="txtreportto"></cc1:CalendarExtender>&nbsp;&nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator1"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RegularExpressionValidator1"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator3"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator4"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredFieldValidator5"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" TargetControlID="RegularExpressionValidator3"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" TargetControlID="RegularExpressionValidator2"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender8" runat="server" TargetControlID="RegularExpressionValidator4"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender9" runat="server" TargetControlID="RequiredFieldValidator6"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender10" runat="server" TargetControlID="RequiredFieldValidator7"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender11" runat="server" TargetControlID="Required"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender12" runat="server" TargetControlID="revpostamt"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender13" runat="server" TargetControlID="revpostno"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender14" runat="server" TargetControlID="revpostnoofdays"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender15" runat="server" TargetControlID="revpreamt"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender16" runat="server" TargetControlID="revpreno"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender17" runat="server" TargetControlID="revprenoofdays"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender18" runat="server" TargetControlID="revcancelamt"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender19" runat="server" TargetControlID="revcancelno"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender20" runat="server" TargetControlID="RegularExpressionValidator13"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender21" runat="server" TargetControlID="RegularExpressionValidator14"></cc1:ValidatorCalloutExtender>&nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender23" runat="server" TargetControlID="RequiredFieldValidator8"></cc1:ValidatorCalloutExtender>&nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender25" runat="server" TargetControlID="RequiredFieldValidator22"></cc1:ValidatorCalloutExtender></TD><TD vAlign=top><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel11" runat="server" BorderStyle="Outset" BackColor="LightSteelBlue">
            <asp:Label ID="lblHead" runat="server" Font-Bold="True" ForeColor="MediumBlue" Text="Tsunami ARMS - Confirmation"></asp:Label></asp:Panel> <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" Font-Size="Small" Text="Do you want to save?" ForeColor="Black"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD style="HEIGHT: 22px"></TD><TD style="HEIGHT: 22px" align=center><asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="HEIGHT: 22px" align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" Font-Size="Small" Text="Do you want to ?" ForeColor="Black"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center><asp:Button id="btnOk" onclick="btnOk_Click1" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel> &nbsp;<asp:Button style="DISPLAY: none" id="Button1" runat="server" Text="Hidden"></asp:Button><cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" TargetControlID="Button1" RepositionMode="RepositionOnWindowResize" PopupControlID="pnlMessage">
    </cc1:ModalPopupExtender> </TD></TR></TBODY></TABLE>
</ContentTemplate>
<Triggers>
    <asp:PostBackTrigger ControlID="btncurrentpolicy" />
    <asp:PostBackTrigger ControlID="btnpolicyhis" />
    <asp:PostBackTrigger ControlID="btnfetchpolicy" />
  </Triggers>

    </asp:UpdatePanel>
    
    
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel6" runat="server" GroupingText="User Tips" Height="50px" Width="100%">
        <br />
        User can edit or delete the selected policy 
        <br />
        <br />
        Click on grid row to select a existing policy<br />
        <br />
        Button to save edited policy as new policy 
        <br />
        <br />
        Button to edit the selected policy
        <br />
        <br />
        Button to delete the policy selected.<br />
    </asp:Panel>
    
</asp:Content>

