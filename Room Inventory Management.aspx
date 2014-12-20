<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Room Inventory Management.aspx.cs" Inherits="Room_Inventory_Management" Title="Room Inventory Management" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<%--<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td style="width: 100%">
                <asp:Panel ID="pnlapprove" runat="server" Width="100%">
                    <table>
                        <tr>
                            <td colspan="2" style="width: 100%">
                                <asp:ScriptManager id="ScriptManager1" runat="server">
                                </asp:ScriptManager>
                                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                    <contenttemplate>
<TABLE><TBODY><TR><TD style="TEXT-ALIGN: center" colSpan=2><SPAN style="FONT-SIZE: 16pt; COLOR: mediumblue"><STRONG>Inventory Management</STRONG></SPAN></TD></TR><TR><TD vAlign=top><asp:Panel id="Panel1" runat="server" Width="99%" Height="81%" GroupingText="User Details"><TABLE><TBODY><TR><TD style="WIDTH: 148px; HEIGHT: 26px"><asp:Label id="lblreqno" runat="server" Width="72px" Text="Req. No"></asp:Label></TD><TD style="HEIGHT: 26px"><asp:TextBox id="txtRequestNo" runat="server" Width="147px" OnTextChanged="txtRequestNo_TextChanged"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 148px"><asp:Label id="lbld" runat="server" Width="38px" Text="Date"></asp:Label></TD><TD><asp:TextBox id="txtDate" runat="server" Width="147px" AutoPostBack="True"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 148px"><asp:Label id="lblreq" runat="server" Width="106px" Text="Requesting  Officer"></asp:Label></TD><TD><asp:TextBox id="txtRequestOfficer" runat="server" Width="147px"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 148px"><asp:Label id="lblreqof" runat="server" Width="158px" Text="Requesting Office/ Store"></asp:Label></TD><TD><asp:DropDownList id="cmbReqStore" runat="server" Width="152px" OnSelectedIndexChanged="cmbReqStore_SelectedIndexChanged" DataValueField="Id" DataTextField="Name"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 148px"><asp:Label id="lblst" runat="server" Width="154px" Text="Issuing  Store"></asp:Label></TD><TD><asp:DropDownList id="cmbIssueStore" runat="server" Width="151px" OnSelectedIndexChanged="cmbIssueStore_SelectedIndexChanged1" AutoPostBack="True" DataValueField="Id" DataTextField="Sname"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 148px"><asp:Label id="lblApprOfficer" runat="server" Text="Approving Officer" Visible="False"></asp:Label></TD><TD><asp:TextBox id="txtApprovingOfficer" runat="server" Width="147px" Visible="False"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 148px"><asp:Label id="lblIssueOfficer" runat="server" Text="Issuing Officer" Visible="False"></asp:Label></TD><TD><asp:TextBox id="txtIssueOfficer" runat="server" Width="147px" Visible="False"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 148px"><asp:Label id="lblIssueNo" runat="server" Width="62px" Text="Issue No" Visible="False"></asp:Label></TD><TD><asp:TextBox id="txtIssueNo" runat="server" Width="147px" Visible="False"></asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel> </TD><TD vAlign=top><asp:Panel id="Panel2" runat="server" Width="97%" Height="67%" GroupingText="Request Panel"><TABLE><TBODY><TR><TD><asp:Label id="lblit" runat="server" Width="96px" Text="Item Category"></asp:Label> </TD><TD style="WIDTH: 158px"><asp:DropDownList id="cmbItem" runat="server" Width="151px" Height="22px" OnSelectedIndexChanged="cmbItem_SelectedIndexChanged1" AutoPostBack="True" DataTextField="itemcatname" DataValueField="itemcat_id"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD><asp:Label id="lblna" runat="server" Text="Item Name"></asp:Label></TD><TD style="WIDTH: 158px"><asp:DropDownList id="cmbItemName" runat="server" Width="151px" Height="22px" OnSelectedIndexChanged="cmbItemName_SelectedIndexChanged1" AutoPostBack="True" DataTextField="itemname" DataValueField="item_id"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD><asp:Label id="lblcode" runat="server" Width="71px" Text="Item Code"></asp:Label></TD><TD style="WIDTH: 158px"><asp:TextBox id="txtCode" runat="server" Width="147px" Enabled="False"></asp:TextBox></TD></TR><TR><TD><asp:Label id="lblqn" runat="server" Text="Quantity"></asp:Label></TD><TD style="WIDTH: 158px"><asp:TextBox id="txtQuantity" runat="server" Width="147px" OnTextChanged="txtQuantity_TextChanged"></asp:TextBox></TD></TR><TR><TD><asp:Label id="lblUnit" runat="server" Width="125px" Text="Unit of Measurement"></asp:Label></TD><TD style="WIDTH: 158px"><asp:TextBox id="txtUnit" runat="server" Width="147px" Enabled="False"></asp:TextBox></TD><TD><asp:Button id="btnAddRequest" onclick="btnAddRequest_Click" runat="server" Text="Add Item" CssClass="btnStyle_small"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel>&nbsp; <BR /><cc1:CalendarExtender id="CalendarExtender1" runat="server" TargetControlID="txtDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> </TD></TR><TR><TD vAlign=top colSpan=2><asp:Panel id="Panel4" runat="server">&nbsp;<TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 17px; TEXT-ALIGN: center"><asp:Button id="btnRequest" onclick="btnRequest_Click" runat="server" CausesValidation="False" Text="Confirm Request" CssClass="btnStyle_large"></asp:Button></TD><TD><asp:Button id="btnClear" onclick="btnClear_Click" runat="server" CausesValidation="False" Text="Clear" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnReport" onclick="btnReport_Click" runat="server" CausesValidation="False" Text="Report" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 100px; HEIGHT: 17px"><asp:Button id="btnView" onclick="btnView_Click" runat="server" CausesValidation="False" Text="View Requested Items" CssClass="btnStyle_large"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=5><asp:Panel id="Panel3" runat="server" Visible="False" __designer:wfdid="w108"><TABLE><TBODY><TR><TD><asp:RadioButton id="RdoRequest" runat="server" Text="Requested" Visible="False" __designer:wfdid="w109" AutoPostBack="True" GroupName="a" OnCheckedChanged="RdoRequest_CheckedChanged"></asp:RadioButton></TD><TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:RadioButton id="RdoApprove" runat="server" Text="Approved" Visible="False" __designer:wfdid="w110" AutoPostBack="True" GroupName="a" OnCheckedChanged="RdoApprove_CheckedChanged"></asp:RadioButton></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=5><asp:GridView id="dtgItem" runat="server" Width="100%" ForeColor="#333333" OnRowCreated="dtgItem_RowCreated" GridLines="None" CellPadding="4">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR><TR><TD colSpan=5><BR /><asp:Panel id="Panel8" runat="server" Width="100%" GroupingText="Requested Item Details" Visible="False"><BR /><asp:GridView id="dtgItemDetails" runat="server" Width="100%" Height="100%" ForeColor="#333333" Font-Bold="False" OnSelectedIndexChanged="dtgItemDetails_SelectedIndexChanged" OnRowCreated="dtgItemDetails_RowCreated" GridLines="None" CellPadding="4" AutoGenerateColumns="False" OnPageIndexChanging="dtgItemDetails_PageIndexChanging" AllowPaging="True" PageSize="5" Font-Size="Small" AllowSorting="True" OnSorting="dtgItemDetails_Sorting" DataKeyNames="reqno,item_id">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:TemplateField><ItemTemplate>
<asp:CheckBox id="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" __designer:wfdid="w36"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="item_id" SortExpression="item_id" HeaderText="Item ID"></asp:BoundField>
<asp:BoundField DataField="itemname" SortExpression="itemname" HeaderText="Name"></asp:BoundField>
<asp:BoundField DataField="itemcatname" SortExpression="itemcatname" HeaderText="Category"></asp:BoundField>
<asp:BoundField DataField="req_qty" SortExpression="req_qty" HeaderText="Requested Quantity"></asp:BoundField>
<asp:TemplateField HeaderText="Approved  Quantity"><ItemTemplate>
<asp:TextBox id="TextBox3" runat="server" Text='<%# bind("req_qty") %>' OnTextChanged="TextBox3_TextChanged" __designer:wfdid="w37"></asp:TextBox> <BR /><asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" ErrorMessage="Enter Quantity" ControlToValidate="TextBox3" __designer:wfdid="w38"></asp:RequiredFieldValidator> 
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>
<EmptyDataTemplate>

<asp:TextBox id="TextBox2" runat="server" Text="(<%# Approved Quantity %>)"></asp:TextBox> 
</EmptyDataTemplate>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> <asp:Button id="btnApprove" onclick="btnApprove_Click" runat="server" CausesValidation="False" Visible="False" Text="Approve" CssClass="btnStyle_small"></asp:Button></asp:Panel></TD></TR><TR><TD colSpan=5><asp:GridView id="dtgRequestedItems" runat="server" Width="100%" HorizontalAlign="Left" ForeColor="#333333" OnRowCreated="dtgRequestedItems_RowCreated" GridLines="None" CellPadding="4" AutoGenerateColumns="False" OnPageIndexChanging="dtgRequestedItems_PageIndexChanging" AllowPaging="True" PageSize="5" OnSelectedIndexChanged="dtgRequestedItems_SelectedIndexChanged">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="reqno" HeaderText="Req No"></asp:BoundField>
<asp:BoundField DataField="Request_Officer" HeaderText="Request Officer"></asp:BoundField>
<asp:BoundField DataField="Date" HeaderText="Date"></asp:BoundField>
<asp:BoundField DataField="Store" HeaderText="Store Name"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR><TR><TD colSpan=5><asp:Panel id="pnlapprove1" runat="server" Width="100%" GroupingText="Approved Item Details"><asp:GridView id="dtgApproved" runat="server" Width="100%" ForeColor="#333333" OnRowCreated="dtgApproved_RowCreated" GridLines="None" CellPadding="4" AutoGenerateColumns="False" OnPageIndexChanging="dtgApproved_PageIndexChanging" AllowPaging="True" PageSize="5" OnSelectedIndexChanged="dtgApproved_SelectedIndexChanged" DataKeyNames="reqno,item_id">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:TemplateField><ItemTemplate>
<asp:CheckBox id="CheckBox2" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox2_CheckedChanged" __designer:wfdid="w5"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="item_id" SortExpression="item_id" HeaderText="Item ID"></asp:BoundField>
<asp:BoundField DataField="itemname" SortExpression="itemname" HeaderText=" Name"></asp:BoundField>
<asp:BoundField DataField="itemcatname" SortExpression="itemcatname" HeaderText="Category"></asp:BoundField>
<asp:BoundField DataField="approved_qty" SortExpression="approved_qty" HeaderText="Approve Quantity"></asp:BoundField>
<asp:TemplateField HeaderText="Issued Quantity"><ItemTemplate>
<asp:TextBox id="TextBox5" runat="server" Width="97px" Text='<%# bind("approved_qty") %>' OnTextChanged="TextBox5_TextChanged"></asp:TextBox><BR /><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Enter Quantity" ControlToValidate="TextBox5"></asp:RequiredFieldValidator>&nbsp; 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Start Slno"><ItemTemplate>
<asp:TextBox id="TextBox6" runat="server" Width="97px" AutoPostBack="True" OnTextChanged="TextBox6_TextChanged" __designer:wfdid="w6"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="End Slno"><ItemTemplate>
<asp:TextBox id="TextBox7" runat="server" Width="97px" AutoPostBack="True" OnTextChanged="TextBox7_TextChanged" __designer:wfdid="w7"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>
<EmptyDataTemplate>
<asp:TextBox id="TextBox4" runat="server" Text="(<%# Approved Quantity %>)"></asp:TextBox>
</EmptyDataTemplate>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView>&nbsp;<asp:Button id="btnIssue" onclick="btnIssue_Click" runat="server" CausesValidation="False" Text="Issue" Visible="False" CssClass="btnStyle_small"></asp:Button> </asp:Panel><BR /><asp:Panel id="Panel10" runat="server" Width="100%" GroupingText="Approve item details" Visible="False" Wrap="False"><asp:GridView id="dtgAItem" runat="server" Width="100%" ForeColor="#333333" OnSelectedIndexChanged="dtgAItem_SelectedIndexChanged" OnRowCreated="dtgAItem_RowCreated" GridLines="None" CellPadding="4" AutoGenerateColumns="False" OnPageIndexChanging="dtgAItem_PageIndexChanging" AllowPaging="True" PageSize="5">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="reqno" HeaderText="Req No"></asp:BoundField>
<asp:BoundField DataField="Request_Officer" HeaderText="Request Officer"></asp:BoundField>
<asp:BoundField DataField="Date" HeaderText="Date"></asp:BoundField>
<asp:BoundField DataField="Store" HeaderText="Store Name"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel></TD></TR><TR><TD colSpan=5><asp:Panel id="Panel11" runat="server" Width="100%" __designer:wfdid="w59">&nbsp;&nbsp;&nbsp;&nbsp;<BR /><TABLE><TBODY><TR><TD vAlign=top colSpan=2><asp:LinkButton id="lnkStoreManager" onclick="lnkStoreManager_Click" runat="server" CausesValidation="False" Font-Bold="True" Visible="False" __designer:wfdid="w97">Store Manager's Liability Ledger</asp:LinkButton>&nbsp;&nbsp;&nbsp; &nbsp; <asp:LinkButton id="lnkStock" onclick="lnkStock_Click" runat="server" Width="117px" CausesValidation="False" Font-Bold="True" Visible="False" __designer:wfdid="w98">Stock Register</asp:LinkButton> <asp:LinkButton id="lnkKeyStockLedger" onclick="lnkKeyStockLedger_Click" runat="server" CausesValidation="False" ForeColor="Blue" Font-Bold="True" Visible="False" __designer:wfdid="w101">Room Key's Stock Ledger</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:LinkButton id="lnkRol" onclick="lnkRol_Click" runat="server" Width="133px" CausesValidation="False" Font-Bold="True" Visible="False" __designer:wfdid="w99">Material below ROL</asp:LinkButton>&nbsp;<BR /><asp:LinkButton id="lnkAuthor" onclick="lnkAuthor_Click" runat="server" CausesValidation="False" Font-Bold="True" Visible="False" __designer:wfdid="w100">Authorised User's List to raise,approve SR, process PR</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton id="LnkDPStockLed" onclick="LnkDPStockLed_Click" runat="server" Width="218px" CausesValidation="False" Font-Bold="True" Visible="False" __designer:wfdid="w1">Donor Free Pass Stock Ledger</asp:LinkButton> <BR /><asp:LinkButton id="lnkPPSL" onclick="lnkPPSL_Click" runat="server" CausesValidation="False" Font-Bold="True" Visible="False" __designer:wfdid="w3">Donor Paid Pass Stock Ledger</asp:LinkButton></TD></TR><TR><TD colSpan=2><asp:Label id="lblStoreName" runat="server" Width="79px" Text="Store Name" Visible="False" __designer:wfdid="w65"></asp:Label>&nbsp; &nbsp;<asp:DropDownList id="cmbStockRegistry" runat="server" Width="120px" Visible="False" __designer:wfdid="w66" OnSelectedIndexChanged="cmbStockRegistry_SelectedIndexChanged" AutoPostBack="True" DataTextField="Sname" DataValueField="id"><asp:ListItem>--Select--</asp:ListItem>
</asp:DropDownList>&nbsp; &nbsp;<asp:Label id="lblItName" runat="server" Width="69px" Text="Item Name" Visible="False" __designer:wfdid="w67"></asp:Label>&nbsp; &nbsp;<asp:DropDownList id="cmbStockItem" runat="server" Width="120px" Visible="False" __designer:wfdid="w68" DataTextField="itemname" DataValueField="item_id"></asp:DropDownList>&nbsp;&nbsp;&nbsp; &nbsp;<asp:Button id="btnStock" onclick="btnStock_Click" runat="server" CausesValidation="False" Text="Stock Ledger" Visible="False" __designer:wfdid="w69" CssClass="btnStyle_large" ValidationGroup="Stock"></asp:Button>&nbsp; </TD></TR><TR><TD colSpan=2><asp:Label id="lblStaff" runat="server" Text="Staff Name" Visible="False" __designer:wfdid="w70"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList id="cmbStaff" runat="server" Width="120px" Visible="False" __designer:wfdid="w71" OnSelectedIndexChanged="cmbStaff_SelectedIndexChanged" AutoPostBack="True" DataTextField="staffname" DataValueField="staff_id"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label id="lblStore1" runat="server" Text="Store Name" Visible="False" __designer:wfdid="w72"></asp:Label>&nbsp; <asp:DropDownList id="cmbStore1" runat="server" Width="120px" Visible="False" __designer:wfdid="w73" DataTextField="storename" DataValueField="store_id"></asp:DropDownList>&nbsp;&nbsp;&nbsp; &nbsp;<asp:Button id="btnLed" onclick="btnLed_Click" runat="server" Text="Liability Ledger" Visible="False" __designer:wfdid="w74" CssClass="btnStyle_large" ValidationGroup="Liability"></asp:Button></TD></TR><TR><TD colSpan=2><asp:Label id="lblRStore" runat="server" Text="Store Name" Visible="False" __designer:wfdid="w75"></asp:Label>&nbsp;&nbsp; &nbsp; &nbsp;<asp:DropDownList id="cmbRStore" runat="server" Width="120px" Visible="False" __designer:wfdid="w76" DataTextField="storename" DataValueField="store_id"></asp:DropDownList>&nbsp;&nbsp; &nbsp;<asp:Button id="btnRol" onclick="btnRol_Click" runat="server" Text="ROL Report" Visible="False" __designer:wfdid="w77" CssClass="btnStyle_large" ValidationGroup="Rol"></asp:Button></TD></TR><TR><TD colSpan=2><asp:Label id="lblBuilding" runat="server" Width="58px" Text="Building" Visible="False" __designer:wfdid="w78"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; <asp:DropDownList id="cmbKBuilding" runat="server" Width="120px" Visible="False" __designer:wfdid="w79" OnSelectedIndexChanged="cmbKBuilding_SelectedIndexChanged" AutoPostBack="True" DataTextField="buildingname" DataValueField="build_id"></asp:DropDownList>&nbsp;&nbsp;&nbsp; <asp:Label id="lblRoomNO" runat="server" Width="39px" Text="Room" Visible="False" __designer:wfdid="w80"></asp:Label>&nbsp; <asp:DropDownList id="cmbKRoom" runat="server" Width="120px" Visible="False" __designer:wfdid="w81" DataTextField="roomno" DataValueField="room_id"></asp:DropDownList>&nbsp;<asp:Label id="lblKStore" runat="server" Text="Store" Visible="False" __designer:wfdid="w82"></asp:Label>&nbsp; <asp:DropDownList id="cmbKStore" runat="server" Width="120px" Visible="False" __designer:wfdid="w83" DataTextField="store" DataValueField="id"></asp:DropDownList>&nbsp;<asp:Button id="btnKeyStockLed" onclick="btnKeyStockLed_Click" runat="server" CausesValidation="False" Text="Key Ledger" Visible="False" __designer:wfdid="w84" CssClass="btnStyle_large"></asp:Button></TD></TR><TR><TD colSpan=2><asp:Label id="lblPStore" runat="server" Text="Store Name" Visible="False" __designer:wfdid="w85"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:DropDownList id="cmbPStore" runat="server" Width="120px" Visible="False" __designer:wfdid="w86" OnSelectedIndexChanged="cmbPStore_SelectedIndexChanged" AutoPostBack="True" DataTextField="Sname" DataValueField="id"></asp:DropDownList>&nbsp;&nbsp;&nbsp; <asp:Label id="lblPItem" runat="server" Text="Item Name" Visible="False" __designer:wfdid="w87"></asp:Label>&nbsp; <asp:DropDownList id="cmbPItem" runat="server" Width="120px" Visible="False" __designer:wfdid="w88" DataTextField="itemname" DataValueField="item_id"></asp:DropDownList>&nbsp; <asp:Button id="btnPass" onclick="btnPass_Click" runat="server" Text="Button" Visible="False" __designer:wfdid="w89" ValidationGroup="Stock"></asp:Button><BR /><asp:Panel id="pnlItems" runat="server" GroupingText="Request & Issue Items" Visible="False" __designer:wfdid="w90"><TABLE style="WIDTH: 358px"><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblFromDate" runat="server" Width="69px" __designer:wfdid="w102" Text="From Date"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtFromDate" runat="server" __designer:wfdid="w103"></asp:TextBox></TD><TD style="WIDTH: 63px"><asp:Label id="lblToDate" runat="server" Width="51px" __designer:wfdid="w104" Text="To  Date"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtToDate" runat="server" __designer:wfdid="w105"></asp:TextBox></TD></TR><TR><TD colSpan=2><asp:LinkButton id="lnkRequestItem" onclick="lnkRequestItem_Click" runat="server" CausesValidation="False" Font-Bold="True" __designer:wfdid="w106">Requested Item List</asp:LinkButton></TD><TD colSpan=2><asp:LinkButton id="lnkIssueDetail" onclick="lnkIssueDetail_Click" runat="server" CausesValidation="False" Font-Bold="True" __designer:wfdid="w107">Issued Items List</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=2><asp:Panel id="Panel5" runat="server"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RegularExpressionValidator1">
                                    </cc1:ValidatorCalloutExtender> <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" Width="168px" ForeColor="White" ControlToValidate="txtDate" ErrorMessage="Enter date in dd/MM/yyyy" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator>&nbsp;&nbsp;&nbsp;&nbsp;<BR /><asp:LinkButton id="lnkr" runat="server" Width="95px" Visible="False">Items Issued</asp:LinkButton> <asp:LinkButton id="lnkMaterial" onclick="lnkMaterial_Click" runat="server" CausesValidation="False" Visible="False">Material request form</asp:LinkButton> <asp:LinkButton id="lnkReceipt" onclick="lnkReceipt_Click" runat="server" Visible="False">Material receipt register</asp:LinkButton> <asp:LinkButton id="lnkIssue" runat="server" Visible="False">Material issue register</asp:LinkButton> <asp:RadioButtonList id="rdoViewRequest" runat="server" Visible="False" __designer:wfdid="w13" OnSelectedIndexChanged="rdoViewRequest_SelectedIndexChanged" AutoPostBack="True" RepeatDirection="Horizontal"><asp:ListItem>Requested</asp:ListItem>
<asp:ListItem>Approved</asp:ListItem>
</asp:RadioButtonList> <cc2:ComboBox id="cmbRequestOfficer" runat="server" Visible="False" __designer:wfdid="w8" DataTextField="staffname" EmptyText="-- select --"></cc2:ComboBox> <asp:Label id="lblt" runat="server" Width="85px" Text="Team/ Counter" Visible="False" __designer:wfdid="w12"></asp:Label> <cc2:ComboBox id="cmbTeamCounter" runat="server" Visible="False" __designer:wfdid="w11" MenuWidth></cc2:ComboBox> <cc2:ComboBox id="cmbIssueOfficer" runat="server" Visible="False" __designer:wfdid="w10" DataTextField="staffname"></cc2:ComboBox> <asp:Button id="Button1" onclick="Button1_Click" runat="server" Text="Button" Visible="False"></asp:Button> <asp:DropDownList id="DropDownList1" runat="server" Width="107px" Visible="False"></asp:DropDownList> <asp:Label id="lblStart" runat="server" Text="Start SlNo" Visible="False"></asp:Label><asp:TextBox id="txtStart" runat="server" Visible="False"></asp:TextBox><asp:Label id="lblEnd" runat="server" Text="End SlNo" Visible="False"></asp:Label><asp:TextBox id="txtEnd" runat="server" Visible="False"></asp:TextBox> <cc2:ComboBox id="cmbApprOfficer" runat="server" Visible="False" __designer:wfdid="w9" DataTextField="staffname"></cc2:ComboBox> <asp:LinkButton id="lnkStockregister" onclick="lnkstockregister_Click" runat="server" Visible="False">Stock register</asp:LinkButton> <asp:LinkButton id="lnkis" runat="server" Visible="False">Total Items Issued</asp:LinkButton> <asp:LinkButton id="lnkreq" runat="server" Visible="False">Requested Item's Report</asp:LinkButton></asp:Panel>&nbsp; <asp:Panel id="Panel6" runat="server" Width="125px" Height="50px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator3"></cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Width="177px" ForeColor="White" Font-Bold="True" Font-Size="Small" ControlToValidate="txtQuantity" ErrorMessage="Must enter quantity"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredFieldValidator4"></cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" Width="166px" ForeColor="White" Font-Bold="True" Font-Size="Small" ControlToValidate="cmbItemName" ErrorMessage="Must choose item Name"></asp:RequiredFieldValidator><BR /><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator1"></cc1:ValidatorCalloutExtender><BR /><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="246px" ForeColor="WhiteSmoke" ControlToValidate="cmbIssueStore" ErrorMessage="Must enter issuing store"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="Reqreqstore" runat="server" Width="152px" ForeColor="White" ControlToValidate="cmbReqStore" ErrorMessage="Must select a store"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="Reqreqstore"></cc1:ValidatorCalloutExtender> <cc1:FilteredTextBoxExtender id="FilteredTextBoxExtender1" runat="server" TargetControlID="txtQuantity" FilterType="Numbers"></cc1:FilteredTextBoxExtender><BR /><BR /><asp:RequiredFieldValidator id="ReqStore1" runat="server" Width="200px" ForeColor="White" ValidationGroup="Stock" __designer:wfdid="w7" ControlToValidate="cmbStockRegistry" ErrorMessage="Must select Store name"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="ReqItem" runat="server" Width="124px" ForeColor="White" ValidationGroup="Stock" __designer:wfdid="w8" ControlToValidate="cmbStockItem" ErrorMessage="Must Select Item"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="ReqStore2" runat="server" Width="119px" ForeColor="White" ValidationGroup="Liability" __designer:wfdid="w9" ControlToValidate="cmbStore1" ErrorMessage="Must select Store"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="ReqStaff" runat="server" Width="189px" ForeColor="White" ValidationGroup="Liability" __designer:wfdid="w10" ControlToValidate="cmbStaff" ErrorMessage="Must select staff"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" TargetControlID="ReqStore1" __designer:wfdid="w12"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" TargetControlID="ReqItem" __designer:wfdid="w13"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender8" runat="server" TargetControlID="ReqStore2" __designer:wfdid="w14"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender9" runat="server" TargetControlID="ReqStaff" __designer:wfdid="w15"></cc1:ValidatorCalloutExtender> <cc1:CalendarExtender id="CalendarExtender2" runat="server" TargetControlID="txtFromDate" Format="dd-MM-yyyy" __designer:wfdid="w9"></cc1:CalendarExtender> <cc1:CalendarExtender id="CalendarExtender3" runat="server" TargetControlID="txtToDate" Format="dd-MM-yyyy" __designer:wfdid="w10"></cc1:CalendarExtender> <asp:RequiredFieldValidator id="ReqRStore" runat="server" Width="187px" ForeColor="White" ValidationGroup="Rol" __designer:wfdid="w6" ControlToValidate="cmbRStore" ErrorMessage="Must Select a Store"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender10" runat="server" TargetControlID="ReqRStore" __designer:wfdid="w7"></cc1:ValidatorCalloutExtender><BR /></asp:Panel><BR /><asp:Button style="DISPLAY: none" id="btnHidden" onclick="btnHidden_Click" runat="server" Text="Hidden"></asp:Button> <asp:TextBox id="TextBox1" runat="server" Visible="False" OnTextChanged="TextBox1_TextChanged" AutoPostBack="True"></asp:TextBox><BR /><asp:Label id="Label3" runat="server" Text="Label" Visible="False"></asp:Label><BR /><BR /><asp:Panel id="pnlMessage" runat="server" _="" _designer:dtid="562958543355909" CssClass="ModalWindow"><asp:Panel id="Panel7" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Text="Tsunami ARMS -" Font-Bold="True" ForeColor="MediumBlue"></asp:Label></asp:Panel> <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="WIDTH: 13px; HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center><asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="WIDTH: 13px" align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD style="WIDTH: 13px" align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD style="HEIGHT: 18px"></TD><TD style="HEIGHT: 18px" align=center></TD><TD style="HEIGHT: 18px" align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel> <cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize">
                </cc1:ModalPopupExtender></TD></TR></TBODY></TABLE>
</contenttemplate>



<Triggers>
    <asp:PostBackTrigger ControlID="lnkmaterial" />
    <asp:PostBackTrigger ControlID="lnkstockregister" />
    <asp:PostBackTrigger ControlID="lnkreceipt" />
    <asp:PostBackTrigger ControlID="lnkissue" />
    <asp:PostBackTrigger ControlID="btnOk" />
     <asp:PostBackTrigger ControlID="btnRol" />
     <asp:PostBackTrigger ControlID="btnStock" />
     <asp:PostBackTrigger ControlID="btnLed" />
     <asp:PostBackTrigger ControlID="lnkRequestItem" />
     <asp:PostBackTrigger ControlID="lnkIssueDetail" />
     <asp:PostBackTrigger ControlID="lnkAuthor" />
     <asp:PostBackTrigger ControlID="btnKeyStockLed" />
     <asp:PostBackTrigger ControlID="btnPass" />
     <asp:PostBackTrigger ControlID="LnkDPStockLed" />  
     <asp:PostBackTrigger ControlID="lnkPPSL" />    
  </Triggers>
  
  
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel9" runat="server" GroupingText="User Tips" Height="50px" Width="100%">
        <br />
            This form is used for Request,approve and issue &nbsp;the request.<br />
        <br />
        Authorized user can approve the request.<br />
        <br />
        If the requested item is greater than requested item,&nbsp; only available number
        of item is approved.<br />
        <br />
        If there is no stock, request can not be accepted.<br />
        <br />
        Add button is used for add more than one items at a time.<br />
        <br />
                        Confirm Request button is used for submit the request.<br />
        <br />
        Approve button is used for approve the request but not issued.<br />
        <br />
        Issue button is used for issue the requested item if available</asp:Panel>
</asp:Content>

