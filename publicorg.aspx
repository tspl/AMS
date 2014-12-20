<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="publicorg.aspx.cs" Inherits="publicorg" Title="Tsunami ARMS-Public display" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server"><asp:Panel ID="pnluserlink" runat="server" GroupingText="Quick links" Height="100%"
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
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 326px; HEIGHT: 155px"><TBODY><TR><TD style="WIDTH: 558px; HEIGHT: 175px" colSpan=3><asp:Panel id="Panel1" runat="server"><TABLE><TBODY><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: center" colSpan=5><STRONG><SPAN style="FONT-SIZE: 14pt">Public&nbsp; Display &nbsp;System</SPAN></STRONG></TD></TR><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: right" align=right colSpan=5><asp:Button id="btnNewInsructions" runat="server" Width="131px" Text="New Instructions" Font-Bold="True" BackColor="#C0C0FF" __designer:wfdid="w4" OnClick="btnNewInsructions_Click"></asp:Button></TD></TR><TR><TD align=center colSpan=3><BR /><asp:Label id="Label1" runat="server" Width="98px" Text="Select Report" Font-Bold="True"></asp:Label> </TD><TD style="WIDTH: 53px" align=center colSpan=1></TD><TD align=center colSpan=1><BR /><BR /><asp:Label id="Label2" runat="server" Text="Selected Reports" Font-Bold="True"></asp:Label><BR /></TD></TR><TR><TD align=center colSpan=3></TD><TD style="WIDTH: 53px" align=center colSpan=1></TD><TD align=center colSpan=1></TD></TR><TR><TD style="HEIGHT: 11px; TEXT-ALIGN: center" colSpan=3><asp:GridView id="dtgReports" runat="server" Width="456px" ForeColor="#333333" OnSelectedIndexChanged="dtgReports_SelectedIndexChanged" AutoGenerateColumns="False" OnPageIndexChanging="dtgReports_PageIndexChanging" AllowPaging="True" CellPadding="4" OnRowCreated="dtgReports_RowCreated" DataKeyNames="report_id">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="report_id" Visible="False" HeaderText="Id"></asp:BoundField>
<asp:BoundField DataField="reportname" HeaderText="Reports"></asp:BoundField>
<asp:BoundField DataField="reporttype" HeaderText="Type"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView>&nbsp; </TD><TD style="WIDTH: 53px; HEIGHT: 11px; TEXT-ALIGN: center" colSpan=1><asp:Button id="btnadd" onclick="Button1_Click" runat="server" Width="50px" ForeColor="White" Text=">>" Font-Bold="True" BackColor="CornflowerBlue" CssClass=" " BorderStyle="Groove" ToolTip="<<"></asp:Button><BR /><asp:Button id="btndelete" onclick="Button2_Click" runat="server" Width="50px" CausesValidation="False" ForeColor="White" Text="<<" Font-Bold="True" BackColor="CornflowerBlue"></asp:Button></TD><TD style="HEIGHT: 11px" vAlign=top colSpan=1><asp:GridView id="dtgSelectedReports" runat="server" Width="350px" Height="48px" ForeColor="#333333" AutoGenerateColumns="False" OnPageIndexChanging="dtgSelectedReports_PageIndexChanging" AllowPaging="True" CellPadding="4" OnRowCreated="dtgSelectedReports_RowCreated" DataKeyNames="display_id" PageSize="20" OnRowDataBound="dtgSelectedReports_RowDataBound">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:BoundField DataField="display_id" Visible="False" HeaderText="id"></asp:BoundField>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="Slno" HeaderText="Slno"></asp:BoundField>
<asp:BoundField DataField="displayname" HeaderText="Reports"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=5><asp:Label id="lblscrolltxt" runat="server" Text="Scrolling   Message" Font-Bold="True"></asp:Label></TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=5><asp:TextBox id="txtScrollMessage" runat="server" Width="811px" Height="45px" ForeColor="#404040" OnTextChanged="txtscrolltext_TextChanged"></asp:TextBox></TD></TR><TR><TD colSpan=3></TD><TD style="WIDTH: 53px; HEIGHT: 20px; TEXT-ALIGN: center" colSpan=1></TD><TD colSpan=1></TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=5><asp:Button id="btnsubmit" onclick="Button3_Click" runat="server" Text="Submit Report" CssClass="btnStyle_large"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel>&nbsp;&nbsp;&nbsp; <asp:Panel id="pnlMessage" runat="server" _="" _designer:dtid="562958543355909" CssClass="ModalWindow"><asp:Panel id="Panel2" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" ForeColor="MediumBlue" Text="Tsunami ARMS - Confirmation" Font-Bold="True"></asp:Label></asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize" TargetControlID="btnHidden">
                </ajaxToolkit:ModalPopupExtender> <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button> <asp:Label id="Label3" runat="server" Text="Label"></asp:Label> <asp:TextBox id="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged" AutoPostBack="True"></asp:TextBox><BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center><asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center><asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel> </TD></TR></TBODY></TABLE>
</contenttemplate>

 <Triggers>
  
    <asp:PostBackTrigger ControlID="btnsubmit" />
  </Triggers>

    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel3" runat="server" GroupingText="User Tips" Height="50px" Width="100%">
        <br />
    This form is used to display the status of rooms and various instructions
    to public.<br />
    </asp:Panel>
</asp:Content>

