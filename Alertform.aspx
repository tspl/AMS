<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Alertform.aspx.cs" Inherits="Alertform" Title="Untitled Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 807px; HEIGHT: 383px"><TBODY><TR><TD align=center colSpan=5><asp:Label id="lblheading" runat="server" Text="Alert Form" Font-Bold="True" Font-Size="X-Large"></asp:Label></TD></TR><TR><TD><asp:ImageButton id="ImageButton3" onclick="ImageButton3_Click" runat="server" ImageUrl="~/Images/ReservedButNotOccupiedRoomAlertButton.gif" Visible="False"></asp:ImageButton></TD><TD><asp:ImageButton id="ImageButton7" onclick="ImageButton7_Click" runat="server" ImageUrl="~/Images/RoomsVacentFor24hrsB.gif" Visible="False"></asp:ImageButton></TD><TD>&nbsp;<asp:ImageButton id="ImageButton1" onclick="ImageButton1_Click" runat="server" ImageUrl="~/Images/NonVacatinButton.gif"></asp:ImageButton></TD><TD><asp:ImageButton id="ImageButton4" onclick="ImageButton4_Click" runat="server" ImageUrl="~/Images/HouseKeeping&MaintenanceAlertButton.gif" Visible="False"></asp:ImageButton></TD><TD><asp:ImageButton id="ImageButton8" onclick="ImageButton8_Click" runat="server" ImageUrl="~/Images/ExtendedRoomAlertB.gif" Visible="False"></asp:ImageButton></TD></TR><TR><TD colSpan=5><asp:Panel id="pnlreport" runat="server" GroupingText="Reports" Visible="False" __designer:wfdid="w6">&nbsp; <TABLE><TBODY><TR><TD><asp:Label id="LabelLiability" runat="server" ForeColor="Blue" Text="Liability" Font-Bold="True" Font-Size="Larger" Visible="False" __designer:wfdid="w17"></asp:Label>&nbsp;<asp:LinkButton id="Lnkrep1" onclick="Lnkrep1_Click" runat="server" Font-Bold="True" __designer:wfdid="w18">Report</asp:LinkButton></TD></TR><TR><TD><asp:LinkButton style="POSITION: static" id="Lnkrep2" onclick="Lnkrep2_Click" runat="server" Font-Bold="True" __designer:wfdid="w19">Report</asp:LinkButton></TD></TR><TR><TD><asp:LinkButton style="POSITION: static" id="Lnkrep3" runat="server" Font-Bold="True" __designer:wfdid="w20">Report</asp:LinkButton></TD></TR><TR><TD><asp:LinkButton id="Lnkrep4" runat="server" Font-Bold="True" __designer:wfdid="w21">Report</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD><asp:ImageButton id="ImageButton6" onclick="ImageButton6_Click1" runat="server" ImageUrl="~/Images/CashierLiabilityAlertButton.gif" Visible="False"></asp:ImageButton></TD><TD><asp:ImageButton id="ImageButton5" onclick="ImageButton5_Click" runat="server" ImageUrl="~/Images/ReceiptBalanceAlertButton.gif" Visible="False"></asp:ImageButton></TD><TD></TD><TD></TD><TD><asp:ImageButton id="ImageButton2" onclick="ImageButton2_Click" runat="server" ImageUrl="~/Images/InventoryItemROLButton.gif" Visible="False"></asp:ImageButton></TD></TR><TR></TR></TBODY></TABLE><TABLE style="WIDTH: 804px; HEIGHT: 255px"><TBODY><TR><TD style="WIDTH: 200px"></TD><TD style="WIDTH: 510px; TEXT-ALIGN: center">&nbsp;&nbsp;<BR /><asp:Panel id="PanelGrid" runat="server" GroupingText="Inventory Item List" Visible="False"><asp:GridView id="GridInvItemList" runat="server" Width="450px" Height="100%" ForeColor="#333333" Visible="False" OnPageIndexChanging="GridInvItemList_PageIndexChanging" EnableViewState="False" CellPadding="4" AllowPaging="True" GridLines="None">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
</Columns>

<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel> </TD><TD style="WIDTH: 160px"></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px; TEXT-ALIGN: center"><asp:Panel id="pnlMessage" runat="server" _="" _designer:dtid="562958543355909" CssClass="ModalWindow"><asp:Panel id="Panel8" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Text="Tsunami ARMS - Confirmation" Font-Bold="True" ForeColor="MediumBlue"></asp:Label></asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" TargetControlID="btnHidden" RepositionMode="RepositionOnWindowResize" PopupControlID="pnlMessage">
                </ajaxToolkit:ModalPopupExtender>&nbsp; <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button>&nbsp; <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD style="WIDTH: 227px; HEIGHT: 18px" align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="WIDTH: 208px; HEIGHT: 15px"></TD><TD style="WIDTH: 13px; HEIGHT: 10px"></TD></TR><TR><TD style="HEIGHT: 26px"></TD><TD style="WIDTH: 208px; HEIGHT: 26px" align=center><asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="WIDTH: 13px; HEIGHT: 26px" align=center>&nbsp;</TD></TR><TR><TD style="HEIGHT: 18px"></TD><TD style="WIDTH: 208px; HEIGHT: 18px" align=center></TD><TD style="WIDTH: 13px; HEIGHT: 18px" align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD style="WIDTH: 224px; TEXT-ALIGN: center" align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD style="TEXT-ALIGN: center" align=center><asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel></asp:Panel> </TD><TD style="WIDTH: 100px"></TD></TR></TBODY></TABLE>
</contenttemplate>
    
   <Triggers>
    <asp:PostBackTrigger ControlID="Lnkrep1" />
    <asp:PostBackTrigger ControlID="Lnkrep2" />
    <asp:PostBackTrigger ControlID="Lnkrep3" />
    <asp:PostBackTrigger ControlID="btnYes" />
    <asp:PostBackTrigger ControlID="btnOk" />

  </Triggers>
    
    
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <strong><span style="font-size: 13pt; text-decoration: underline">
    </span></strong>
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        <br />
        1. The Alert form provides the user with alerts like nonvacating alert, delayed
        housekeeping alert etc.<br />
        <br />
        2. When the alert is active, the button color changes from blue to red.<br />
        <br />
        3. The report or information about the alert can be taken from the report panel
        that appears when the alert button is clicked.</asp:Panel>
</asp:Content>