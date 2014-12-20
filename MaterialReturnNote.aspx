<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MaterialReturnNote.aspx.cs" Inherits="MaterialReturnNote" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 627px; HEIGHT: 276px"><TBODY><TR><TD vAlign=top><asp:Panel id="Panel1" runat="server"><TABLE><TBODY><TR><TD style="HEIGHT: 26px; TEXT-ALIGN: center" vAlign=top colSpan=2><SPAN style="FONT-SIZE: 16pt"><STRONG>Material Return Note</STRONG></SPAN></TD></TR><TR><TD vAlign=top colSpan=1><asp:Panel id="PnlReturn" runat="server" GroupingText="Return Details"><TABLE><TBODY><TR><TD style="WIDTH: 193px"><asp:Label id="lblType" runat="server" Width="82px" Text="Return Type"></asp:Label></TD><TD style="WIDTH: 160px"><asp:DropDownList id="cmbType" runat="server" Width="154px"><asp:ListItem>Stock Return</asp:ListItem>
<asp:ListItem></asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 193px"><asp:Label id="lblRetrunNo" runat="server" Width="67px" Text="Ret  No"></asp:Label></TD><TD style="WIDTH: 160px"><asp:TextBox id="txtRetrun" runat="server" Width="150px"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 193px"><asp:Label id="lblDate" runat="server" Width="32px" Text="Date"></asp:Label></TD><TD style="WIDTH: 160px"><asp:TextBox id="txtDate" runat="server" Width="150px"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 193px; HEIGHT: 26px"><asp:Label id="lblReturningOfficer" runat="server" Width="101px" Text="Returning Officer"></asp:Label></TD><TD style="WIDTH: 160px; HEIGHT: 26px"><asp:TextBox id="txtReturningOfficer" runat="server" Width="150px"></asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel><BR /></TD><TD vAlign=top><asp:Panel id="PnlItemReturn" runat="server" GroupingText="Return Items"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblItem" runat="server" Width="68px" Text="Item Name"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbItemName" runat="server" Width="154px" AutoPostBack="True" OnSelectedIndexChanged="cmbItemName_SelectedIndexChanged" DataValueField="item_id" DataTextField="itemname"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblItemCode" runat="server" Text="Item Code"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtItemCode" runat="server" Width="150px"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblQty" runat="server" Text="Quantity"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtQty" runat="server" Width="150px" Enabled="False"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblRequest" runat="server" Width="89px" Text="Request Office" Visible="False"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtRequestOffice" runat="server" Width="150px" Visible="False"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblReceivingStore" runat="server" Width="133px" Text="Issuing Office/ Store"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbReceivingStore" runat="server" Width="154px" DataValueField="office_issue" DataTextField="storename"></asp:DropDownList></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD vAlign=top colSpan=1></TD><TD vAlign=top><asp:Button id="btnReturn" runat="server" Text="Return" Visible="False" CssClass="btnStyle_small"></asp:Button> <asp:Button id="btnClear" onclick="btnClear_Click" runat="server" Text="Clear" CssClass="btnStyle_small"></asp:Button> <asp:Button id="btnReport" onclick="btnReport_Click" runat="server" Text="Report" CssClass="btnStyle_small"></asp:Button></TD></TR><TR><TD vAlign=top colSpan=2><BR /><asp:Panel id="pnlReport" runat="server" GroupingText="Retuned Items List" Visible="False"><TABLE style="WIDTH: 309px"><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblFromDate" runat="server" Width="80px" Text="  Date"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtFromDate" runat="server"></asp:TextBox></TD><TD><asp:LinkButton id="lnkReturnNote" onclick="lnkReturnNote_Click" runat="server" Width="85px" Font-Bold="True">Return Note</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblToDate" runat="server" Width="46px" Text="To Date" Visible="False"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="TextBox2" runat="server" Visible="False"></asp:TextBox></TD><TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=2><asp:Panel id="pnlItem" runat="server" Width="100%" GroupingText="Item Details" Visible="False"><asp:GridView id="dtgReturnItems" runat="server" Width="100%" ForeColor="#333333" Visible="False" CellPadding="4" GridLines="None" AutoGenerateColumns="False" DataKeyNames="item_id,office_request,req_from,grnno" OnSelectedIndexChanged="dtgReturnItems_SelectedIndexChanged" OnRowCreated="dtgReturnItems_RowCreated" OnPageIndexChanging="dtgReturnItems_PageIndexChanging">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:TemplateField><ItemTemplate>
<asp:CheckBox id="chkSelect" runat="server" OnCheckedChanged="chkSelect_CheckedChanged"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="itemname" HeaderText="Item Name"></asp:BoundField>
<asp:BoundField DataField="itemcode" HeaderText="Item Code"></asp:BoundField>
<asp:BoundField DataField="storename" HeaderText="Issuing Office"></asp:BoundField>
<asp:BoundField DataField="balance" HeaderText="Balance Qty"></asp:BoundField>
<asp:BoundField DataField="item_id" Visible="False" HeaderText="item_id"></asp:BoundField>
<asp:BoundField DataField="office_request" Visible="False" HeaderText="office_request"></asp:BoundField>
<asp:BoundField DataField="req_from" Visible="False" HeaderText="req_from"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel></TD></TR></TBODY></TABLE><asp:Button id="btnReturnItem" onclick="btnReturnItem_Click" runat="server" Text="Return" Visible="False" CssClass="btnStyle_small"></asp:Button></asp:Panel></TD></TR><TR><TD vAlign=top><asp:Panel id="pnlReceive" runat="server" Width="100%" GroupingText="Receive Details"><asp:GridView id="dtgReceiveDetails" runat="server" Width="100%" ForeColor="#333333" OnSelectedIndexChanged="dtgReceiveDetails_SelectedIndexChanged" OnPageIndexChanging="dtgReceiveDetails_PageIndexChanging" OnRowCreated="dtgReceiveDetails_RowCreated" DataKeyNames="refno" AutoGenerateColumns="False" GridLines="None" CellPadding="4" AllowPaging="True" PageSize="5">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="grnno" HeaderText="Receiving  No"></asp:BoundField>
<asp:BoundField DataField="receivedon" HeaderText="Receiving  Date"></asp:BoundField>
<asp:BoundField DataField="refno" Visible="False"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel></TD></TR><TR><TD style="WIDTH: 279px" vAlign=top><ajaxToolkit:CalendarExtender id="CalendarExtender1" runat="server" Format="dd-MM-yyyy" TargetControlID="txtDate"></ajaxToolkit:CalendarExtender><BR /><cc1:CalendarExtender id="CalendarExtender2" runat="server" Format="dd-MM-yyyy" TargetControlID="txtFromDate"></cc1:CalendarExtender><BR /><BR /><asp:Button style="DISPLAY: none" id="btnHidden" onclick="btnHidden_Click" runat="server" Text="Hidden"></asp:Button><asp:TextBox id="TextBox1" runat="server" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged" Visible="False"></asp:TextBox><asp:Label id="Label2" runat="server" Text="Label" Visible="False"></asp:Label><BR /><BR /><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel7" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Width="233px" ForeColor="MediumBlue" Text="Tsunami ARMS -" Font-Bold="True"></asp:Label></asp:Panel> <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" Width="173px" Height="25px" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="144px" Height="90px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 26px"></TD><TD align=center>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize">
    </cc1:ModalPopupExtender> </asp:Panel><BR /><BR /><BR /><BR /></TD></TR></TBODY></TABLE>
</contenttemplate>

<Triggers>
    
    <asp:PostBackTrigger ControlID="lnkReturnNote" />
       
  </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        &nbsp;<br />
        This form is used for return items to issuing store at the end of the season.</asp:Panel>
</asp:Content>

