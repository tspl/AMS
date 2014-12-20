<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="passview.aspx.cs" Inherits="passview" Title="Untitled Page" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 18px"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 24px"></TD><TD style="WIDTH: 100px; HEIGHT: 24px"></TD><TD style="HEIGHT: 24px" colSpan=1><asp:Label id="lblHeading" runat="server" Text="Donor Pass View" Font-Bold="True" Font-Size="Larger" __designer:wfdid="w459"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 24px" colSpan=1></TD><TD style="WIDTH: 158px; HEIGHT: 24px" colSpan=1></TD><TD style="WIDTH: 158px; HEIGHT: 24px" colSpan=1></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 24px"><asp:Label id="Label1" runat="server" Width="75px" Text="Filter by"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 24px"><asp:Label id="Label2" runat="server" Width="83px" Text="Buildingname"></asp:Label></TD><TD style="HEIGHT: 24px" colSpan=1><asp:DropDownList id="cmbBuildingPass" runat="server" Width="200px" Height="22px" __designer:wfdid="w454" DataTextField="buildingname" DataValueField="build_id" OnSelectedIndexChanged="cmbBuildingPass_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD><TD style="WIDTH: 100px; HEIGHT: 24px" colSpan=1><asp:Label id="Label5" runat="server" Width="93px" Text="Filter by status" __designer:wfdid="w10"></asp:Label></TD><TD style="WIDTH: 158px; HEIGHT: 24px" colSpan=1><asp:DropDownList id="cmbFilter" runat="server" Width="205px" Height="22px" __designer:wfdid="w460" OnSelectedIndexChanged="cmbFilter_SelectedIndexChanged" AutoPostBack="True"><asp:ListItem Value="-1">--Select--</asp:ListItem>
<asp:ListItem>Pass Not Printed</asp:ListItem>
<asp:ListItem>Address to Print</asp:ListItem>
<asp:ListItem>Not Dispatch</asp:ListItem>
<asp:ListItem>Dispatched</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 158px; HEIGHT: 24px" colSpan=1></TD></TR><TR><TD></TD><TD><asp:Label id="Label3" runat="server" Text="Roomno"></asp:Label></TD><TD><asp:DropDownList id="cmbRoomPass" runat="server" Width="200px" Height="22px" __designer:wfdid="w455" DataTextField="roomno" DataValueField="room_id" OnSelectedIndexChanged="cmbRoomPass_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD><TD style="WIDTH: 100px"><asp:Label id="lblPassStartNo" runat="server" Text="Pass Start No" __designer:wfdid="w2"></asp:Label></TD><TD style="WIDTH: 158px"><asp:TextBox id="txtPassStaetNo" runat="server" Width="200px" Height="17px" __designer:wfdid="w4"></asp:TextBox></TD><TD style="WIDTH: 158px"></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD><asp:Label id="Label4" runat="server" Text="Donorname"></asp:Label></TD><TD><asp:DropDownList id="cmbDonorPass" runat="server" Width="200px" Height="22px" __designer:wfdid="w456" DataTextField="donor_name" DataValueField="donor_id" OnSelectedIndexChanged="cmbDonorPass_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD><TD style="WIDTH: 100px"><asp:Label id="lblPassBalance" runat="server" Text="Pass Balance" __designer:wfdid="w3"></asp:Label></TD><TD style="WIDTH: 158px"><asp:TextBox id="txtPassBalance" runat="server" Width="200px" Height="17px" __designer:wfdid="w5"></asp:TextBox></TD><TD style="WIDTH: 158px"></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:Label id="lblPassType" runat="server" Text="PassType" __designer:wfdid="w1"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbPasstyp" runat="server" Width="200px" Height="22px" __designer:wfdid="w457" OnSelectedIndexChanged="cmbPasstyp_SelectedIndexChanged" AutoPostBack="True"><asp:ListItem Value="1">Paid Pass</asp:ListItem>
<asp:ListItem Value="0">Free Pass</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px"><asp:Button id="btnclear" onclick="btnclear_Click" runat="server" Text="Clear" __designer:wfdid="w4" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 158px"><asp:Button id="btnprint" onclick="btnprint_Click" runat="server" Text="Print" __designer:wfdid="w3" CssClass="btnStyle_large"></asp:Button></TD><TD style="WIDTH: 158px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="btnBack" onclick="btnBack_Click" runat="server" Text="Back" __designer:wfdid="w1" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 100px"></TD><TD></TD><TD colSpan=2>&nbsp;</TD><TD colSpan=1></TD></TR></TBODY></TABLE></TD></TR><TR><TD>&nbsp;<TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 27px"><asp:CheckBox id="chkSelectAll" runat="server" Width="85px" Text="Select All" __designer:wfdid="w3" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"></asp:CheckBox></TD><TD style="WIDTH: 100px; HEIGHT: 27px"></TD><TD style="WIDTH: 100px; HEIGHT: 27px"><asp:Label id="lblPfrom" runat="server" Width="88px" Text="Pass No From" __designer:wfdid="w455"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 27px"><asp:TextBox id="txtPassFrom" runat="server" Width="200px" Height="17px" __designer:wfdid="w454"></asp:TextBox></TD><TD style="WIDTH: 100px; HEIGHT: 27px"><asp:Label id="lblPTo" runat="server" Text="To" __designer:wfdid="w456"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 27px"><asp:TextBox id="txtPassTo" runat="server" Width="200px" Height="17px" __designer:wfdid="w453"></asp:TextBox></TD><TD style="WIDTH: 100px; HEIGHT: 27px"><asp:Button id="btnNext" onclick="btnNext_Click" runat="server" Text="Print" __designer:wfdid="w452" CssClass="btnStyle_small"></asp:Button></TD></TR></TBODY></TABLE><asp:GridView id="gdPassView" runat="server" Width="100%" ForeColor="#333333" __designer:wfdid="w1" OnSelectedIndexChanged="gdPassView_SelectedIndexChanged" PageSize="12" DataKeyNames="id,did,rid" AutoGenerateColumns="False" GridLines="None" OnRowCreated="gdPassView_RowCreated" OnPageIndexChanging="gdPassView_PageIndexChanging" CellPadding="4">
<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:TemplateField><ItemTemplate>
<asp:CheckBox id="CheckBox2" runat="server" __designer:wfdid="w1" OnCheckedChanged="CheckBox2_CheckedChanged"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="id" HeaderText="id" Visible="False"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="Room" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="Donor" HeaderText="Donor"></asp:BoundField>
<asp:BoundField DataField="PassCount" HeaderText="No of Pass"></asp:BoundField>
<asp:BoundField DataField="donorid" HeaderText="did" Visible="False"></asp:BoundField>
<asp:BoundField DataField="roomid" HeaderText="rid" Visible="False"></asp:BoundField>
</Columns>

<PagerStyle HorizontalAlign="Center" BackColor="#2461BF" ForeColor="White"></PagerStyle>

<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

<HeaderStyle HorizontalAlign="Left" BackColor="#507CD1" Font-Bold="True" ForeColor="White"></HeaderStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> <asp:GridView id="gdprint" runat="server" Width="100%" ForeColor="#333333" __designer:wfdid="w2" DataKeyNames="id,did,rid" AutoGenerateColumns="False" GridLines="None" OnPageIndexChanging="gdprint_PageIndexChanging" CellPadding="4">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="id" Visible="False" HeaderText="id"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="Room" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="Donor" HeaderText="Donor"></asp:BoundField>
<asp:BoundField DataField="PassCount" HeaderText="No of Pass"></asp:BoundField>
<asp:BoundField DataField="donorid" Visible="False" HeaderText="did"></asp:BoundField>
<asp:BoundField DataField="roomid" Visible="False" HeaderText="rid"></asp:BoundField>
<asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:Panel id="pnlMessage" runat="server" __designer:wfdid="w5" CssClass="ModalWindow" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel8" runat="server" BackColor="LightSteelBlue" __designer:wfdid="w6" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Text="Tsunami ARMS - Confirmation" __designer:wfdid="w7" Font-Bold="True" ForeColor="MediumBlue"></asp:Label></asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" __designer:wfdid="w8" RepositionMode="RepositionOnWindowResize" PopupControlID="pnlMessage" TargetControlID="btnHidden"></ajaxToolkit:ModalPopupExtender>&nbsp; <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden" __designer:wfdid="w9"></asp:Button>&nbsp; <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px" __designer:wfdid="w10"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD style="WIDTH: 227px; HEIGHT: 18px" align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small" __designer:wfdid="w11"></asp:Label></TD></TR><TR><TD style="HEIGHT: 15px"></TD><TD style="WIDTH: 208px; HEIGHT: 15px"></TD><TD style="WIDTH: 13px; HEIGHT: 15px"></TD></TR><TR><TD style="HEIGHT: 26px"></TD><TD style="WIDTH: 208px; HEIGHT: 26px" align=center>&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" __designer:wfdid="w12" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" __designer:wfdid="w13" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="WIDTH: 13px; HEIGHT: 26px" align=center>&nbsp;</TD></TR><TR><TD style="HEIGHT: 18px"></TD><TD style="WIDTH: 208px; HEIGHT: 18px" align=center></TD><TD style="WIDTH: 13px; HEIGHT: 18px" align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px" __designer:wfdid="w14"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD style="WIDTH: 224px" align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small" __designer:wfdid="w15"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" __designer:wfdid="w16" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel></TD></TR></TBODY></TABLE>&nbsp; 
</contenttemplate>
 <Triggers> 
 <asp:PostBackTrigger ControlID="btnYes" />
    <asp:PostBackTrigger ControlID="btnNext" />
  </Triggers>

    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        <p>
            &nbsp;</p>
    </asp:Panel>
</asp:Content>

