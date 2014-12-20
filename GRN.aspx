<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GRN.aspx.cs" Inherits="GRN" Title="Tsunami ARMS - Material Receipt Note" %>


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
                                            runat="server" Height="32px" NavigateUrl="~/ReservationPolicy.aspx" Visible="False"
                                            Width="127px">reservation Policy</asp:HyperLink><asp:HyperLink ID="hlroolallocpol"
                                                runat="server" Height="32px" NavigateUrl="~/Room Allocation Policy.aspx" Visible="False"
                                                Width="127px">Room Alloc Policy</asp:HyperLink><asp:HyperLink ID="hlbillpolicy" runat="server"
                                                    Height="32px" NavigateUrl="~/Billing and Service charge policy.aspx" Visible="False"
                                                    Width="128px">Bill n service policy</asp:HyperLink><asp:HyperLink ID="hlbankpolicy"
                                                        runat="server" Height="32px" NavigateUrl="~/Cashier and Bank Remittance Policy.aspx"
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
    <asp:Panel ID="Panel1" runat="server">
        <asp:ScriptManager id="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table style="width: 529px">
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel id="UpdatePanel1" runat="server">
                        <contenttemplate>
<TABLE><TBODY><TR><TD colSpan=2><STRONG><SPAN style="FONT-SIZE: 16pt; COLOR: #0000cd">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<SPAN style="FONT-SIZE: 14pt; COLOR: #003399"> &nbsp;Material Receipt Note</SPAN></SPAN></STRONG></TD></TR><TR><TD vAlign=top><asp:Panel id="Panel2" runat="server" GroupingText="GRN  details"><TABLE><TBODY><TR><TD style="WIDTH: 116px" vAlign=top><asp:Label id="lbltype" runat="server" Width="100%" Text="Type"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbType" runat="server" Width="153px" AutoPostBack="True" OnSelectedIndexChanged="cmbType_SelectedIndexChanged"><asp:ListItem>-- Select--</asp:ListItem>
<asp:ListItem>Purchase Requisition</asp:ListItem>
<asp:ListItem>Stores Requisition</asp:ListItem>
<asp:ListItem>Stock Transfer</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 116px"><asp:Label id="lblGrn" runat="server" Text="Mr No"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtGrno" runat="server" Width="150px"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 116px"><asp:Label id="txtdate" runat="server" Width="44px" Text="Date"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtDate1" runat="server" Width="150px"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 116px"><asp:Label id="lblreceive" runat="server" Width="138px" Text="Receiving Officer"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtReceive" runat="server" Width="150px"></asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel></TD><TD vAlign=top>&nbsp;<asp:Panel id="Panel3" runat="server" GroupingText="Item Details" Visible="False"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lbliname" runat="server" Width="70px" Text="Item name" __designer:wfdid="w2"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtItemName" runat="server" Width="150px" __designer:wfdid="w3"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblicode" runat="server" Text="Item code" __designer:wfdid="w4"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtItemCode" runat="server" Width="150px" __designer:wfdid="w5"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblqnt" runat="server" Text="Quantity" __designer:wfdid="w6"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtQty" runat="server" Width="150px" __designer:wfdid="w7"></asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel>&nbsp; &nbsp; &nbsp;&nbsp; </TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=2><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Button id="btnClear" onclick="btnClear_Click" runat="server" Text="Clear" CssClass="btnStyle_small" __designer:wfdid="w26"></asp:Button></TD><TD style="WIDTH: 100px" vAlign=top><asp:Button id="btnReport" onclick="btnReport_Click" runat="server" CausesValidation="False" Text="Report" CssClass="btnStyle_small" __designer:wfdid="w27"></asp:Button></TD></TR></TBODY></TABLE><asp:Panel id="pnlReport" runat="server" Width="125px" Height="50px" GroupingText="Received Item Details" Visible="False" __designer:wfdid="w30"><TABLE style="WIDTH: 350px"><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblFromDate" runat="server" Width="94px" Text="From Date" __designer:wfdid="w31"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtFromDate" runat="server" Width="150px" __designer:wfdid="w32"></asp:TextBox></TD><TD style="WIDTH: 100px"><asp:Label id="lblToDate" runat="server" Width="79px" Text="To  Date" __designer:wfdid="w33"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtToDate" runat="server" Width="150px" __designer:wfdid="w34"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkReceive" onclick="lnkReceive_Click" runat="server" Width="151px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w35">Material Receipt Note</asp:LinkButton></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD></TR></TBODY></TABLE></asp:Panel>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR><TR><TD colSpan=2><asp:Panel id="pnlIssitem" runat="server" Visible="False"><asp:Panel id="Panel5" runat="server" Width="100%" GroupingText="Item Details" Visible="False"><asp:GridView id="dtgIssueDetails" runat="server" Width="100%" ForeColor="#333333" OnSelectedIndexChanged="dtgIssueDetails_SelectedIndexChanged" PageSize="5" OnRowCreated="dtgIssueDetails_RowCreated" AllowPaging="True" DataKeyNames="issueno,item_id,reqno" GridLines="None" CellPadding="4" AutoGenerateColumns="False">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:TemplateField><ItemTemplate>
<asp:CheckBox id="chkSelect" runat="server" OnCheckedChanged="chkSelect_CheckedChanged" __designer:wfdid="w1"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="itemname" HeaderText="Item Name"></asp:BoundField>
<asp:BoundField DataField="issued_qty" HeaderText="Issued Qty"></asp:BoundField>
<asp:TemplateField HeaderText="Receive Qty"><ItemTemplate>
            <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="True" OnTextChanged="TextBox2_TextChanged"
                Text='<%# bind("issued_qty") %>'></asp:TextBox>
        
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel><asp:Button id="btnSave" onclick="btnSave_Click" runat="server" Text="Receive" Visible="False" CssClass="btnStyle_small"></asp:Button><BR /></asp:Panel><asp:Panel id="pnlIssue" runat="server" GroupingText="Issue Details" Visible="False"><asp:GridView id="dtgIssue" runat="server" Width="100%" ForeColor="#333333" OnSelectedIndexChanged="dtgIssue_SelectedIndexChanged" AutoGenerateColumns="False" CellPadding="4" GridLines="None" DataKeyNames="reqno" AllowPaging="True" OnRowCreated="dtgIssue_RowCreated" PageSize="5" OnPageIndexChanging="dtgIssue_PageIndexChanging">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="issueno" HeaderText="Issue No"></asp:BoundField>
<asp:BoundField DataField="Date" HeaderText="Date"></asp:BoundField>
<asp:BoundField DataField="storename" HeaderText="Issuing Office"></asp:BoundField>
<asp:BoundField DataField="iss_officer" HeaderText="Issuing Officer"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel><BR /><BR /><BR /><asp:Button style="DISPLAY: none" id="btnHidden" onclick="btnHidden_Click" runat="server" Text="Hidden"></asp:Button> <asp:TextBox id="TextBox1" runat="server" Visible="False" OnTextChanged="TextBox1_TextChanged" AutoPostBack="True"></asp:TextBox> <asp:Label id="Label2" runat="server" Text="Label" Visible="False"></asp:Label><BR /><BR /><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel7" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset">
            <asp:Label ID="lblHead" runat="server" Font-Bold="True" ForeColor="MediumBlue" Text="Tsunami ARMS -"></asp:Label></asp:Panel> <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center><asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" TargetControlID="btnHidden" RepositionMode="RepositionOnWindowResize" PopupControlID="pnlMessage">
    </cc1:ModalPopupExtender> </asp:Panel>&nbsp;&nbsp;<BR /><cc1:CalendarExtender id="CalendarExtender3" runat="server" __designer:wfdid="w37" TargetControlID="txtFromDate" Format="dd-MM-yyyy"></cc1:CalendarExtender><BR /><cc1:CalendarExtender id="CalendarExtender2" runat="server" __designer:wfdid="w36" TargetControlID="txtToDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> <ajaxToolkit:CalendarExtender id="CalendarExtender1" runat="server" __designer:wfdid="w8" TargetControlID="txtDate1" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender></TD></TR></TBODY></TABLE>
</contenttemplate>




<Triggers>
    
    <asp:PostBackTrigger ControlID="btnOk" />
    <asp:PostBackTrigger ControlID="lnkReceive" /> 
   
  </Triggers>
  
                    </asp:UpdatePanel></td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="PnlTips" runat="server" GroupingText="User Tips" Height="50px" Width="100%">
        <br />
        *
        This form is used for Receive items to Requesting Office.
        <br />
        <br />
        * If user select&nbsp; Stores Requisition &nbsp;from Type combo box, entire issued&nbsp;
        details are shown in grid.<br />
        <br />
        * Then select a row from grid, item setails of that issue number is displayed in
        another grid.<br />
        <br />
        * User clicks on check box and enter quantity and press on Receive button.<br />
        <br />
        * Selected item is added to requesting office and also generate a PDF with entire
        details.<br />
    </asp:Panel>
</asp:Content>

