<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Billing and Service charge policy.aspx.cs" Inherits="Building_and_Service_charge_policy" Title="Untitled Page" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <strong><span style="text-decoration: underline">
    </span></strong><p>
        <span>
            <asp:Panel ID="Panel1" runat="server" GroupingText="User Tips" Height="50px" HorizontalAlign="Left" Width="100%">
                <br />
                Use<strong> Enter Key</strong> or <strong>Tab Key</strong> or <strong>Mouse Click</strong>,
            To go to the Next Field. 
                <br />
                <br />
        <span></span><span>Use <strong>Mouse</strong> to select Data from the<strong> grid.</strong></span><p>
        &nbsp;</p>
            </asp:Panel>
        </span>
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        <span><strong></strong></span>&nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        <span><strong></strong></span>&nbsp;</p>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>

        <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<TABLE style="WIDTH: 100%; HEIGHT: 100%"><TBODY><TR><TD style="WIDTH: 943px; HEIGHT: 18px" align=center colSpan=1><asp:Label id="Label4" runat="server" Width="422px" Text="Billing and service charge policy" __designer:wfdid="w99" CssClass="heading" Font-Size="Large" Font-Names="Arial"></asp:Label></TD></TR><TR><TD style="WIDTH: 943px"><TABLE><TBODY><TR><TD vAlign=top><asp:Panel id="panelservice" runat="server" GroupingText="Service details" __designer:wfdid="w61"><TABLE><TBODY><TR><TD><asp:Label id="lblservicename" runat="server" Width="87px" Text=" Service name" __designer:wfdid="w58"></asp:Label></TD><TD><asp:DropDownList id="cmbService" runat="server" Width="141px" Height="22px" __designer:wfdid="w59" OnSelectedIndexChanged="cmbService_SelectedIndexChanged" DataValueField="bill_service_id" DataTextField="bill_service_name" AutoPostBack="True"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD vAlign=top><asp:LinkButton id="lnkroomcategory" onclick="lnkroomcategory_Click" runat="server" CausesValidation="False" __designer:wfdid="w60">New</asp:LinkButton></TD></TR><TR><TD vAlign=top><asp:Label id="Label5" runat="server" Text="Applicable to" __designer:wfdid="w61"></asp:Label></TD><TD><asp:DropDownList id="cmbApplicable" runat="server" Width="141px" Height="21px" __designer:wfdid="w62" OnSelectedIndexChanged="cmbApplicable_SelectedIndexChanged1" AutoPostBack="True"><asp:ListItem Value="-1">--Select--</asp:ListItem>
<asp:ListItem Value="0">Room Category</asp:ListItem>
<asp:ListItem Value="1">Single Room</asp:ListItem>
</asp:DropDownList></TD><TD vAlign=top></TD></TR><TR><TD><asp:Label id="lblroomtype" runat="server" Width="91px" Text="Room category" __designer:wfdid="w63"></asp:Label></TD><TD vAlign=top><asp:DropDownList id="cmbRoomcategory" runat="server" Width="141px" Height="22px" __designer:wfdid="w64" OnSelectedIndexChanged="cmbRoomcategory_SelectedIndexChanged1" DataValueField="room_cat_id" DataTextField="room_cat_name" AutoPostBack="True"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD style="HEIGHT: 16px" vAlign=top></TD></TR><TR><TD><asp:Label id="Label3" runat="server" Text="Building name" __designer:wfdid="w65"></asp:Label></TD><TD><asp:DropDownList id="cmbBuilding" runat="server" Width="141px" Height="22px" __designer:wfdid="w66" OnSelectedIndexChanged="cmbBuilding_SelectedIndexChanged" DataValueField="build_id" DataTextField="buildingname" AutoPostBack="True"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD></TD></TR><TR><TD><asp:Label id="lblroomno" runat="server" Width="72px" Text="Room no" __designer:wfdid="w67"></asp:Label></TD><TD><asp:DropDownList id="cmbRoom" runat="server" Width="141px" Height="22px" __designer:wfdid="w68" OnSelectedIndexChanged="cmbRoom_SelectedIndexChanged" DataValueField="room_id" DataTextField="roomno" AutoPostBack="True"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD style="HEIGHT: 24px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Panel></TD><TD vAlign=top><asp:Panel id="pnlservicecharge" runat="server" GroupingText="Servicecharge" __designer:wfdid="w72"><TABLE><TBODY><TR><TD vAlign=top><asp:Label id="lblserviceunit" runat="server" Width="138px" Text="Service measuring Unit" __designer:wfdid="w69"></asp:Label></TD><TD vAlign=top><asp:DropDownList id="cmbServicemeasure" runat="server" Width="134px" Height="22px" __designer:wfdid="w70" OnSelectedIndexChanged="cmbServicemeasure_SelectedIndexChanged" DataValueField="service_unit_id" DataTextField="unitname" AutoPostBack="True"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD><asp:Label id="lblminnoofunits" runat="server" Width="126px" Text="Minimum No of units" __designer:wfdid="w71"></asp:Label></TD><TD><asp:TextBox id="txtminnoofunits" tabIndex=6 runat="server" Width="130px" Height="17px" __designer:wfdid="w72" OnTextChanged="txtminnoofunits_TextChanged"></asp:TextBox></TD></TR><TR><TD><asp:Label id="Label1" runat="server" Width="132px" Text="Service Charge" __designer:wfdid="w73"></asp:Label></TD><TD><asp:TextBox id="txtservicecharge" tabIndex=7 runat="server" Width="130px" Height="27px" __designer:wfdid="w74" OnTextChanged="txtservicecharge_TextChanged"></asp:TextBox></TD></TR><TR><TD><asp:Label id="lbltaxrate" runat="server" Width="118px" Text="Tax Rate" __designer:wfdid="w75"></asp:Label></TD><TD><asp:TextBox id="txttaxrate" tabIndex=8 runat="server" Width="130px" Height="29px" __designer:wfdid="w76" ontextchanged="txttaxrate_TextChanged"></asp:TextBox></TD></TR></TBODY></TABLE><BR /></asp:Panel></TD><TD style="HEIGHT: 205px" vAlign=top><asp:Panel id="pnlperiod" runat="server" GroupingText="Policy Period" __designer:wfdid="w82"><TABLE style="HEIGHT: 153px"><TBODY><TR><TD><asp:Label id="lblpolicyfrom" runat="server" Width="131px" Text="Policy applicable from" __designer:wfdid="w77"></asp:Label></TD><TD><asp:TextBox id="txtpolicyfrom" tabIndex=9 runat="server" Width="125px" Height="17px" __designer:wfdid="w78" ontextchanged="txtpolicyfrom_TextChanged" MaxLength="10"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="lblpolicyto" runat="server" Width="131px" Text="Policy applicable to" __designer:wfdid="w79"></asp:Label></TD><TD><asp:TextBox id="txtpolicyto" tabIndex=10 runat="server" Width="125px" Height="17px" __designer:wfdid="w80" ontextchanged="txtpolicyto_TextChanged" MaxLength="10"></asp:TextBox></TD></TR><TR><TD><asp:Label id="Label2" runat="server" Width="114px" Text="Policy seasons" __designer:wfdid="w81"></asp:Label></TD><TD><asp:ListBox id="lstseasons" tabIndex=11 runat="server" Width="128px" __designer:wfdid="w82" SelectionMode="Multiple" onselectedindexchanged="lstseasons_SelectedIndexChanged"></asp:ListBox>&nbsp; </TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD align=right colSpan=2><TABLE><TBODY><TR><TD style="HEIGHT: 27px"><asp:Button id="btnadd" tabIndex=18 onclick="btnadd_Click" runat="server" Text="Save" __designer:wfdid="w20" CssClass="btnStyle_small"></asp:Button></TD><TD style="HEIGHT: 27px"></TD><TD style="WIDTH: 76px; HEIGHT: 27px"><asp:Button id="btndelete" tabIndex=20 onclick="btndelete_Click" runat="server" Width="75px" Text="Delete" __designer:wfdid="w22" CssClass="btnStyle_small"></asp:Button></TD><TD style="HEIGHT: 27px"><asp:Button id="Button1" tabIndex=21 onclick="Button1_Click" runat="server" CausesValidation="False" Text="Clear" __designer:wfdid="w23" CssClass="btnStyle_small"></asp:Button></TD><TD style="HEIGHT: 27px"><asp:Button id="btnreport" tabIndex=22 onclick="btnreport_Click" runat="server" CausesValidation="False" Text="Report" __designer:wfdid="w24" CssClass="btnStyle_small"></asp:Button></TD></TR></TBODY></TABLE>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD><TD>&nbsp; </TD></TR><TR><TD colSpan=3><asp:GridView id="dgservicepolicy" runat="server" Width="100%" ForeColor="#333333" GridLines="None" OnPageIndexChanging="dgservicepolicy_PageIndexChanging" AllowPaging="True" CellPadding="4" OnSelectedIndexChanged="dgservicepolicy_SelectedIndexChanged" OnRowCreated="dgservicepolicy_RowCreated" DataKeyNames="Policy Id" AutoGenerateColumns="False">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="Policy Id" Visible="False" HeaderText="Policy Id"></asp:BoundField>
<asp:BoundField DataField="Service Name" HeaderText="Service Name"></asp:BoundField>
<asp:BoundField DataField="Applicable To" HeaderText="Applicable To"></asp:BoundField>
<asp:BoundField DataField="Measurement Unit" HeaderText="Measurement Unit"></asp:BoundField>
<asp:BoundField DataField="Minimum Unit" HeaderText="Minimum Unit"></asp:BoundField>
<asp:BoundField DataField="From" HeaderText="From"></asp:BoundField>
<asp:BoundField DataField="To" HeaderText="To"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR></TBODY></TABLE><asp:Panel id="pnlreport" runat="server" Width="200px" Height="12%" GroupingText="Report" __designer:wfdid="w52" Visible="False"><TABLE><TBODY><TR><TD style="WIDTH: 253px; HEIGHT: 18px"><asp:LinkButton id="lnklblservicechargelist" onclick="lnklblservicechargelist_Click" runat="server" Width="139px" CausesValidation="False" __designer:wfdid="w55">Current policy report</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 253px; HEIGHT: 18px"><asp:LinkButton id="lnklblpolicyhistory" onclick="lnklblpolicyhistory_Click" runat="server" Width="145px" CausesValidation="False" __designer:wfdid="w56">Policy history report</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 253px; HEIGHT: 18px"><asp:Button id="btmclose" runat="server" CausesValidation="False" __designer:wfdid="w57" Text="Close Report" OnClick="btmclose_Click"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="WIDTH: 943px">&nbsp; <TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 201px" vAlign=top>&nbsp;<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Width="168px" ForeColor="White" __designer:wfdid="w15" ControlToValidate="cmbService" ErrorMessage="Service name required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Width="134px" ForeColor="White" __designer:wfdid="w17" ControlToValidate="cmbServicemeasure" ErrorMessage="Required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" Width="138px" ForeColor="White" __designer:wfdid="w16" ControlToValidate="txtpolicyfrom" ErrorMessage="From date required"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:RequiredFieldValidator id="rqdfldvalseason" runat="server" Width="155px" ForeColor="White" __designer:wfdid="w8" ControlToValidate="lstseasons" ErrorMessage="Season required"></asp:RequiredFieldValidator><BR />&nbsp;<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ForeColor="White" __designer:wfdid="w58" ControlToValidate="cmbRoomcategory" ErrorMessage="Required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" ForeColor="White" __designer:wfdid="w77" ControlToValidate="cmbApplicable" ErrorMessage="Required"></asp:RequiredFieldValidator>&nbsp; <cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" __designer:wfdid="w115" TargetControlID="RequiredFieldValidator1"></cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" __designer:wfdid="w134" TargetControlID="RequiredFieldValidator5"></cc2:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" ForeColor="White" __designer:wfdid="w1" ControlToValidate="txtminnoofunits" ErrorMessage="Required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator7" runat="server" ForeColor="White" __designer:wfdid="w1" ControlToValidate="txtservicecharge" ErrorMessage="Required"></asp:RequiredFieldValidator> <cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" __designer:wfdid="w19" TargetControlID="RequiredFieldValidator7"></cc2:ValidatorCalloutExtender></TD><TD style="WIDTH: 100px; HEIGHT: 201px"><asp:Panel id="pnlMessage" runat="server" __designer:wfdid="w28" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel8" runat="server" BackColor="LightSteelBlue" __designer:wfdid="w29" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" ForeColor="MediumBlue" Text="Tsunami ARMS - Confirmation" __designer:wfdid="w30" Font-Bold="True"></asp:Label></asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" __designer:wfdid="w31" TargetControlID="btnHidden" RepositionMode="RepositionOnWindowResize" PopupControlID="pnlMessage"></ajaxToolkit:ModalPopupExtender>&nbsp; <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden" __designer:wfdid="w32"></asp:Button>&nbsp; <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px" __designer:wfdid="w33"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD style="WIDTH: 227px; HEIGHT: 18px" align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" __designer:wfdid="w34" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 15px"></TD><TD style="WIDTH: 208px; HEIGHT: 15px"></TD><TD style="WIDTH: 13px; HEIGHT: 15px"></TD></TR><TR><TD style="HEIGHT: 26px"></TD><TD style="WIDTH: 208px; HEIGHT: 26px" align=center>&nbsp; &nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" __designer:wfdid="w35" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" __designer:wfdid="w36" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="WIDTH: 13px; HEIGHT: 26px" align=center>&nbsp;</TD></TR><TR><TD style="HEIGHT: 18px"></TD><TD style="WIDTH: 208px; HEIGHT: 18px" align=center></TD><TD style="WIDTH: 13px; HEIGHT: 18px" align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px" __designer:wfdid="w37"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD style="WIDTH: 224px" align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" __designer:wfdid="w38" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp; &nbsp; &nbsp;<asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" __designer:wfdid="w39" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel>&nbsp;<BR /><cc2:CalendarExtender id="CalendarExtender1" runat="server" __designer:wfdid="w20" TargetControlID="txtpolicyfrom" Format="dd-MM-yyyy"></cc2:CalendarExtender> <cc2:CalendarExtender id="CalendarExtender2" runat="server" __designer:wfdid="w39" TargetControlID="txtpolicyto" Format="dd-MM-yyyy"></cc2:CalendarExtender></TD><TD style="WIDTH: 100px; HEIGHT: 201px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <cc2:FilteredTextBoxExtender id="FilteredTextBoxExtender3" runat="server" __designer:wfdid="w4" TargetControlID="txtservicecharge" FilterType="Numbers"></cc2:FilteredTextBoxExtender> <cc2:FilteredTextBoxExtender id="FilteredTextBoxExtender4" runat="server" __designer:wfdid="w5" TargetControlID="txttaxrate" FilterType="Numbers"></cc2:FilteredTextBoxExtender> <cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" __designer:wfdid="w6" TargetControlID="rqdfldvalseason"></cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" __designer:wfdid="w12" TargetControlID="RequiredFieldValidator2"></cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" __designer:wfdid="w1" TargetControlID="RequiredFieldValidator4"></cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" __designer:wfdid="w19" TargetControlID="RequiredFieldValidator6"></cc2:ValidatorCalloutExtender> <cc2:FilteredTextBoxExtender id="FilteredTextBoxExtender1" runat="server" __designer:wfdid="w1" TargetControlID="txtminnoofunits" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" InvalidChars=":,00,' '"></cc2:FilteredTextBoxExtender></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>
</ContentTemplate>
 <Triggers>
    <asp:PostBackTrigger ControlID="lnklblservicechargelist" />
    <asp:PostBackTrigger ControlID="lnklblpolicyhistory" />
  </Triggers>
    </asp:UpdatePanel>
    
    </div>
</asp:Content>




