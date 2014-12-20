<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="User Privilege settings.aspx.cs" Inherits="User" Title="Tsunami ARMS- User Privilege Settings" %>

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
                        Visible="False">Complaint master</asp:HyperLink><asp:HyperLink ID="hlteammaster"
                            runat="server" Height="32px" NavigateUrl="~/TeamMaster.aspx" Visible="False"
                            Width="128px">Team master</asp:HyperLink><asp:HyperLink ID="hlinvmaster" runat="server"
                                Height="32px" NavigateUrl="~/inventorymaster.aspx" Visible="False">Inventory master</asp:HyperLink><asp:HyperLink
                                    ID="hlseasonmstr" runat="server" Height="32px" NavigateUrl="~/SeasonMaster.aspx"
                                    Visible="False" Width="136px">Season master</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                                        ID="hlsubmaster" runat="server" Height="32px" NavigateUrl="~/Submasters.aspx"
                                        Visible="False" Width="128px">Submasters</asp:HyperLink><asp:HyperLink ID="hlreservpol"
                                            runat="server" Height="32px" NavigateUrl="~/ReservationPolicy.aspx" Visible="False">reservation Policy</asp:HyperLink><asp:HyperLink
                                                ID="hlroolallocpol" runat="server" Height="32px" NavigateUrl="~/Room Allocation Policy.aspx"
                                                Visible="False" Width="144px">Room Alloc Policy</asp:HyperLink><asp:HyperLink ID="hlbillpolicy"
                                                    runat="server" Height="32px" NavigateUrl="~/Billing and Service charge policy.aspx"
                                                    Visible="False" Width="152px">Bill & service policy</asp:HyperLink><asp:HyperLink
                                                        ID="hlbankpolicy" runat="server" Height="32px" NavigateUrl="~/Cashier and Bank Remittance Policy.aspx"
                                                        Visible="False" Width="136px">Bank policy</asp:HyperLink><asp:HyperLink ID="hlroomallocation"
                                                            runat="server" Height="32px" NavigateUrl="~/roomallocation.aspx" Visible="False"
                                                            Width="128px">Room allocation</asp:HyperLink><asp:HyperLink ID="hlroomreservation"
                                                                runat="server" Height="32px" NavigateUrl="~/Room Reservation.aspx" Visible="False"
                                                                Width="136px">Room reservation</asp:HyperLink><asp:HyperLink ID="hlvacating" runat="server"
                                                                    Height="32px" NavigateUrl="~/vacating and billing.aspx" Visible="False" Width="128px">Room Vacating</asp:HyperLink><asp:HyperLink
                                                                        ID="hldonorpass" runat="server" Height="32px" NavigateUrl="~/donorpassfinal.aspx"
                                                                        Visible="False" Width="128px">Donor pass</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                                                                            ID="hlcmplntrgstr" runat="server" Height="32px" NavigateUrl="~/Complaint Register.aspx"
                                                                            Visible="False" Width="144px">Complaint register</asp:HyperLink><span style="font-size: 9pt">
                                                                            </span>
        <asp:HyperLink ID="hlchellanentry" runat="server" Height="32px" NavigateUrl="~/Chellan Entry.aspx"
            Visible="False" Width="128px">Chellan entry</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                ID="hlroomrsrce" runat="server" Height="32px" NavigateUrl="~/Room Resource Register.aspx"
                Visible="False" Width="128px">Room resource</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                    ID="hlusercrtn" runat="server" Height="32px" NavigateUrl="~/User Account Information.aspx"
                    Visible="False" Width="136px">User creation</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                        ID="hluserprvlge" runat="server" Height="32px" NavigateUrl="~/UserPrivilegeSettings.aspx"
                        Visible="False" Width="136px">User privilege</asp:HyperLink><asp:HyperLink ID="hlprinter"
                            runat="server" Height="32px" NavigateUrl="~/PlainPreprintedSettings.aspx" Visible="False"
                            Width="136px">Printer settings</asp:HyperLink><asp:HyperLink ID="hldayclose" runat="server"
                                Height="32px" NavigateUrl="~/DayClosing.aspx" Visible="False" Width="136px">Day close</asp:HyperLink><asp:HyperLink
                                    ID="hlroommgmnt" runat="server" Height="32px" NavigateUrl="~/Room Management.aspx"
                                    Visible="False" Width="152px">Room management</asp:HyperLink><asp:HyperLink ID="hlinvmngmnt"
                                        runat="server" Height="32px" NavigateUrl="~/Room Inventory Management.aspx" Visible="False"
                                        Width="153px">Inventory mangement</asp:HyperLink><asp:HyperLink ID="hlhkmagmnt" runat="server"
                                            Height="32px" NavigateUrl="~/HK management.aspx" Visible="False" Width="152px">HK management</asp:HyperLink></asp:Panel>
</asp:Content>--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 568px; HEIGHT: 216px"><TBODY><TR><TD style="HEIGHT: 653px" vAlign=top colSpan=3 rowSpan=3><asp:Panel id="Panel1" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 100%"><TBODY><TR><TD style="TEXT-ALIGN: center" colSpan=3><SPAN style="FONT-SIZE: 14pt; COLOR: #003399"><STRONG>User Privilege Settings</STRONG></SPAN></TD></TR><TR><TD style="HEIGHT: 368px" colSpan=3><asp:Panel id="pnluserprivilege" runat="server" Width="111%" Height="285px" GroupingText="User Privilege"><TABLE style="WIDTH: 414px"><TBODY><TR><TD><asp:Label id="lbluserlevel" runat="server" Width="97px" Text="User level"></asp:Label></TD><TD colSpan=2><asp:TextBox id="txtUserlevel" runat="server" Width="155px" AutoPostBack="True" OnTextChanged="txtUserlevel_TextChanged"></asp:TextBox></TD></TR><TR><TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Form List</TD><TD></TD><TD>&nbsp; &nbsp;&nbsp; Selected Forms</TD></TR><TR><TD style="WIDTH: 73px; HEIGHT: 73px"><asp:ListBox id="lstSelectform" tabIndex=1 runat="server" Width="179px" OnSelectedIndexChanged="lstSelectform_SelectedIndexChanged" SelectionMode="Multiple"></asp:ListBox></TD><TD style="WIDTH: 59px; HEIGHT: 73px"><asp:Button id="btnAdd" tabIndex=2 onclick="btnAdd_Click" runat="server" Width="51px" CausesValidation="False" Text=">>" BackColor="CornflowerBlue"></asp:Button><BR /><asp:Button id="btnRemove" tabIndex=3 onclick="btnRemove_Click" runat="server" Width="52px" CausesValidation="False" Text="<<" BackColor="CornflowerBlue"></asp:Button></TD><TD><asp:ListBox id="lstSelectedform" tabIndex=4 runat="server" Width="186px" SelectionMode="Multiple"></asp:ListBox></TD></TR><TR><TD><asp:Label id="lbldefault" runat="server" Width="118px" Text="Default Home Page"></asp:Label></TD><TD colSpan=2><cc2:ComboBox id="cmbDefault1" runat="server" Visible="False" OnSelectedIndexChanged="cmbDefault_SelectedIndexChanged" SelectedValue="form_id" SelectedText="displayname" EnableViewState="False" EmptyText="-- select --" DataSourceID="SqlDataSource1" MenuWidth DataTextField="displayname" DataValueField="form_id" FilterType="StartsWith"></cc2:ComboBox><asp:SqlDataSource id="SqlDataSource1" runat="server" SelectCommand="SELECT form_id, displayname FROM m_sub_form" ProviderName="<%$ ConnectionStrings:tdbnewConnectionString3.ProviderName %>" ConnectionString="<%$ ConnectionStrings:tdbnewConnectionString3 %>"><SelectParameters>
<asp:Parameter DefaultValue="-1" Name="name"></asp:Parameter>
</SelectParameters>
</asp:SqlDataSource> <asp:DropDownList id="cmbDefault" tabIndex=5 runat="server" Width="146px" Height="22px" DataTextField="displayname" DataValueField="defaultform_id"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD>&nbsp;<asp:Label id="lblexecute" runat="server" Width="104px" Text="Execute Override"></asp:Label></TD><TD colSpan=2><asp:DropDownList id="cmbExecute" runat="server" Width="144px" Height="22px"><asp:ListItem></asp:ListItem>
<asp:ListItem>Yes</asp:ListItem>
<asp:ListItem>No</asp:ListItem>
</asp:DropDownList></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=3><asp:Panel id="Panel2" runat="server" Width="100%"><TABLE><TBODY><TR><TD><asp:Button id="btnSave" onclick="btnSave_Click1" runat="server" Text="Save" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnDelete" onclick="btnDelete_Click" runat="server" CausesValidation="False" Text="Delete" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 72px"><asp:Button id="btnClear" onclick="btnClear_Click" runat="server" CausesValidation="False" Text="Clear" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 72px"><asp:Button id="btnEdit" runat="server" CausesValidation="False" Enabled="False" Text="Edit" CssClass="btnStyle_small" OnClick="btnEdit_Click"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="HEIGHT: 230px" colSpan=3><asp:Panel id="pnlgrid" runat="server" Width="100%" GroupingText="User Level and Home page"><asp:GridView id="dtgUsergrid" runat="server" Width="100%" ForeColor="#333333" OnSelectedIndexChanged="dtgUsergrid_SelectedIndexChanged" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" OnSorting="dtgUsergrid_Sorting" OnPageIndexChanging="dtgUsergrid_PageIndexChanging" OnRowCreated="dtgUsergrid_RowCreated1" CellPadding="4" GridLines="None" PageSize="5">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="Level" HeaderText="Prev Level"></asp:BoundField>
<asp:BoundField DataField="HomePage" HeaderText="Home Page"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel></TD></TR><TR><TD></TD><TD style="WIDTH: 289px" colSpan=2><asp:Panel id="pnlvalidation" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 100%"><TBODY><TR><TD><asp:RequiredFieldValidator id="userlevel" runat="server" ForeColor="White" ErrorMessage="User level required" ControlToValidate="txtUserlevel"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="listboxselection" runat="server" ForeColor="White" ErrorMessage="List box selection required" ControlToValidate="lstSelectedform"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="homepage" runat="server" Width="197px" ForeColor="White" ErrorMessage="Home page selection required" ControlToValidate="cmbDefault"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="execute" runat="server" ForeColor="White" ErrorMessage="Execute override required" ControlToValidate="cmbExecute"></asp:RequiredFieldValidator><BR /><BR /><asp:RegularExpressionValidator id="reguserlevel" runat="server" ForeColor="White" ErrorMessage="Only numbers allowed" ControlToValidate="txtUserlevel" ValidationExpression="[0-9]{1,10}"></asp:RegularExpressionValidator><BR /><asp:RegularExpressionValidator id="Reguselevel" runat="server" ForeColor="White" Visible="False" ErrorMessage="Enter only Number" ControlToValidate="txtUserlevel"></asp:RegularExpressionValidator></TD><TD colSpan=2><cc1:ValidatorCalloutExtender id="vuserlevel" runat="server" TargetControlID="userlevel"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="vlistbox" runat="server" TargetControlID="listboxselection"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="vdefault" runat="server" TargetControlID="homepage"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="vexecute" runat="server" TargetControlID="execute"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="valuserlevel" runat="server" TargetControlID="userlevel"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="Reguselevel" Enabled="False"></cc1:ValidatorCalloutExtender> <cc1:FilteredTextBoxExtender id="FilteredTextBoxExtender1" runat="server" TargetControlID="txtUserlevel" FilterType="Numbers"></cc1:FilteredTextBoxExtender></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=3><BR /><asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button> <asp:TextBox id="TextBox1" runat="server" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged" Visible="False"></asp:TextBox><BR /><asp:Label id="Label3" runat="server" Text="Label" Visible="False"></asp:Label><BR /><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel7" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Font-Bold="True" Text="Tsunami ARMS -" ForeColor="MediumBlue"></asp:Label></asp:Panel> <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center><asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel><BR /><cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize" TargetControlID="btnHidden">
                </cc1:ModalPopupExtender> <TABLE style="WIDTH: 414px"><TBODY><TR><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"></TD></TR></TBODY></TABLE><BR /></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR></TR><TR></TR></TBODY></TABLE><BR /><BR />
</contenttemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="pnlusertip" runat="server" GroupingText="User tips" Width="100%">
        <br />
        Set User level<br />
            <br />
        Select forms from list box for setting user previllage.<br />
            <br />
        Use &gt;&gt; and &lt;&lt; buttons to select and remove data from listbox.<br />
            <br />
        Set Home page for user by selecting it from dropdown list.<br />
            <br />
        Set Yes or No for provide user execute override facility.<br />
            <br />
            Use single click to select data from grid.<br />
            <br />
            For edit, select data from grid.<br />
    </asp:Panel>
</asp:Content>

