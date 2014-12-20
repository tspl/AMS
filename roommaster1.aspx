<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="roommaster1.aspx.cs" Inherits="roommaster1" Title="Tsunami ARMS- Room master" %>

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
    <asp:ScriptManager id="ScriptManager2" runat="server">
    </asp:ScriptManager>
    
       
    
     <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 369px; HEIGHT: 149px"><TBODY><TR><TD style="HEIGHT: 24px; TEXT-ALIGN: center" colSpan=2><STRONG><SPAN style="FONT-SIZE: 14pt">Room Master</SPAN></STRONG></TD></TR><TR><TD vAlign=top><asp:Panel id="pnlRoom" runat="server" Width="100%" Height="100%" GroupingText="Room Details" ForeColor="Blue" BorderColor="White"><TABLE id="Table5"><TBODY><TR><TD><asp:Label id="LblBldngName" runat="server" Width="89px" Text="Building Name"></asp:Label></TD><TD><asp:DropDownList id="cmbBuiildingName" runat="server" Width="136px" DataValueField="build_id" DataTextField="buildingname" AutoPostBack="True" OnSelectedIndexChanged="cmbBuiildingName_SelectedIndexChanged1"></asp:DropDownList>&nbsp;&nbsp;&nbsp; </TD><TD><asp:LinkButton id="lnkNewBuilding" onclick="LnkNewBuilding_Click1" runat="server" CausesValidation="False">New</asp:LinkButton></TD></TR><TR><TD><asp:Label id="LblFloorNmbr" runat="server" Width="85px" Text="Floor No"></asp:Label></TD><TD><asp:DropDownList id="cmbFloorNo" runat="server" Width="136px" DataValueField="floor_id" DataTextField="floor" AutoPostBack="True" OnSelectedIndexChanged="cmbFloorNo_SelectedIndexChanged1"></asp:DropDownList></TD><TD><asp:LinkButton id="lnkNewFloor" onclick="LinkButton3_Click1" runat="server" CausesValidation="False">New</asp:LinkButton></TD></TR><TR><TD><asp:Label id="LblRoomNumbr" runat="server" Width="84px" Text="Room No"></asp:Label></TD><TD><asp:TextBox id="txtRoomNo" runat="server" Width="129px" AutoPostBack="True" OnTextChanged="TxtRoomNo_TextChanged"></asp:TextBox></TD><TD>&nbsp;</TD></TR><TR><TD><asp:Label id="lblroomtype" runat="server" Width="98px" Text="Type of Room"></asp:Label></TD><TD><asp:DropDownList id="cmbRoomType" runat="server" Width="136px" DataValueField="room_cat_id" DataTextField="room_cat_name" AutoPostBack="True" OnSelectedIndexChanged="cmbRoomType_SelectedIndexChanged1"></asp:DropDownList>&nbsp; </TD><TD><asp:LinkButton id="lnkNewType" onclick="lnkNewType_Click" runat="server" CausesValidation="False">New</asp:LinkButton></TD></TR><TR><TD><asp:Label id="lblfacility" runat="server" Width="108px" Text="Facilities Available"></asp:Label></TD><TD><asp:ListBox id="lstFacility" runat="server" Width="136px" Height="68px" OnSelectedIndexChanged="lstfascilit_SelectedIndexChanged1" SelectionMode="Multiple"><asp:ListItem></asp:ListItem>
</asp:ListBox></TD><TD><asp:LinkButton id="lnkFacility" onclick="LinkButton1_Click" runat="server" CausesValidation="False">New</asp:LinkButton></TD></TR><TR><TD><asp:Label id="lblservice" runat="server" Width="87px" Text="Service Available"></asp:Label></TD><TD><asp:ListBox id="lstService" runat="server" Width="136px" Height="56px" OnSelectedIndexChanged="lstService_SelectedIndexChanged1" SelectionMode="Multiple" Rows="6"><asp:ListItem></asp:ListItem>
</asp:ListBox></TD><TD><asp:LinkButton id="lnkService" onclick="LinkButton2_Click" runat="server" CausesValidation="False">New</asp:LinkButton></TD></TR><TR><TD><asp:Label id="LblArea" runat="server" Width="92px" Text="Area of Room"></asp:Label></TD><TD><asp:TextBox id="txtRoomArea" runat="server" Width="95px" AutoPostBack="True" OnTextChanged="TextRoomArea_TextChanged"></asp:TextBox>&nbsp; Sq</TD><TD>&nbsp;</TD></TR><TR><TD style="WIDTH: 26px; HEIGHT: 20px"><asp:Label id="LblInmates" runat="server" Width="124px" Text="Maximum number of Inmates"></asp:Label></TD><TD><asp:TextBox id="txtInmatesNo" runat="server" Width="96px" AutoPostBack="True" OnTextChanged="TextInmatesNumber_TextChanged"></asp:TextBox></TD><TD>&nbsp;</TD></TR><TR><TD align=center colSpan=2><asp:Label id="lblMessage" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label></TD><TD></TD></TR></TBODY></TABLE></asp:Panel></TD><TD vAlign=top><asp:Panel id="pnlDonor" runat="server" Width="100%" Height="67%" GroupingText="Room & Donor Details" ForeColor="Blue" BorderColor="White"><TABLE id="Table6"><TBODY><TR><TD><asp:Label id="LblRent" runat="server" Width="47px" Text="Rent"></asp:Label></TD><TD style="WIDTH: 186px; HEIGHT: 5px"><asp:TextBox id="txtRoomRent" tabIndex=11 runat="server" Width="161px" AutoPostBack="True" OnTextChanged="TextRoomRent_TextChanged1" Enabled="False"></asp:TextBox></TD><TD>&nbsp;</TD></TR><TR><TD style="WIDTH: 49px; HEIGHT: 5px"><asp:Label id="LblSecurty" runat="server" Width="102px" Text="Security Deposit"></asp:Label></TD><TD style="WIDTH: 186px; HEIGHT: 5px"><asp:TextBox id="txtSecurityDeposit" tabIndex=12 runat="server" Width="161px" AutoPostBack="True" OnTextChanged="TextSecurtydposit_TextChanged" Enabled="False"></asp:TextBox> </TD><TD>&nbsp;</TD></TR><TR><TD style="WIDTH: 49px; HEIGHT: 5px"><asp:Label id="LblDonrName" runat="server" Width="89px" Text="Donor Name"></asp:Label></TD><TD style="WIDTH: 186px; HEIGHT: 5px"><asp:DropDownList id="cmbDonorName" runat="server" Width="166px" DataValueField="donor_id" DataTextField="donor_name" AutoPostBack="True" OnSelectedIndexChanged="cmbDonorName_SelectedIndexChanged2"></asp:DropDownList> </TD><TD><asp:LinkButton id="lnkNewDonor" tabIndex=11 onclick="lnkNewDonor_Click" runat="server" CausesValidation="False">New</asp:LinkButton></TD></TR><TR><TD><asp:Label id="lbldonorhousename" runat="server" Width="85px" Text="HouseName"></asp:Label></TD><TD style="WIDTH: 186px; HEIGHT: 26px"><asp:TextBox id="txtDonorHouseName" runat="server" Width="161px" Enabled="False" ValidationGroup="9"></asp:TextBox> </TD><TD style="HEIGHT: 26px"></TD></TR><TR><TD vAlign=top><asp:Label id="lbllsgnohousenodoorno" runat="server" Width="93px" Height="25px" Text="LSG no/House no/Door no"></asp:Label></TD><TD style="WIDTH: 186px; HEIGHT: 27px"><asp:TextBox id="txtDonorHouseNo" runat="server" Width="160px" Enabled="False"></asp:TextBox> </TD><TD></TD></TR><TR><TD><asp:Label id="lbladdress1" runat="server" Width="100px" Text="Address 1"></asp:Label></TD><TD style="WIDTH: 186px; HEIGHT: 26px"><asp:TextBox id="txtDonorAddress1" runat="server" Width="159px" Enabled="False"></asp:TextBox> </TD><TD style="HEIGHT: 26px"></TD></TR><TR><TD height:><asp:Label id="lbladdress2" runat="server" Width="64px" Text="Address 2"></asp:Label></TD><TD style="WIDTH: 186px"><asp:TextBox id="txtDonorAddress2" runat="server" Width="161px" Enabled="False"></asp:TextBox> </TD><TD></TD></TR><TR><TD><asp:Label id="lbldonordistrict" runat="server" Width="44px" Text="District"></asp:Label></TD><TD style="WIDTH: 186px; HEIGHT: 22px"><asp:TextBox id="txtDonorDistrict" runat="server" Width="161px" Enabled="False"></asp:TextBox> </TD><TD></TD></TR><TR><TD><asp:Label id="lbldonorstate" runat="server" Width="33px" Text="State"></asp:Label></TD><TD style="WIDTH: 186px"><asp:TextBox id="txtDonorState" runat="server" Width="161px" Enabled="False"></asp:TextBox> </TD><TD></TD></TR></TBODY></TABLE></asp:Panel>&nbsp; </TD></TR><TR><TD align=center colSpan=2>&nbsp;<asp:Button id="BtnSave" onclick="BtnSave_Click" runat="server" Text="Save" EnableViewState="False" CssClass="btnStyle_small" EnableTheming="True"></asp:Button> <asp:Button id="btnedit" onclick="btnedit_Click1" runat="server" Text="Edit" CssClass="btnStyle_small"></asp:Button> <asp:Button id="btnclear" onclick="btnclear_Click" runat="server" CausesValidation="False" Text="Clear" CssClass="btnStyle_small"></asp:Button> <asp:Button id="btndelete" onclick="btndelete_Click" runat="server" Text="Delete" CssClass="btnStyle_small"></asp:Button>&nbsp;&nbsp;<asp:Button id="Button1" onclick="Button1_Click" runat="server" CausesValidation="False" Text="Report" CssClass="btnStyle_small"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR><TR><TD vAlign=top colSpan=2><asp:Panel id="pnlReport" runat="server" GroupingText="Report" ForeColor="Blue" Visible="False">&nbsp;<TABLE><TBODY><TR><TD><asp:LinkButton id="LnkRoomList" onclick="LnkRoomList_Click" runat="server" Width="159px" CausesValidation="False" Font-Bold="True">Building wise roomlist</asp:LinkButton><BR /></TD><TD style="WIDTH: 100px">&nbsp;<asp:DropDownList id="cmbBuildReport" runat="server" Width="115px" DataValueField="build_id" DataTextField="buildingname"></asp:DropDownList>&nbsp; </TD><TD></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:LinkButton id="LnkDonorList" onclick="LinkButton3_Click" runat="server" Width="174px" CausesValidation="False" Font-Bold="True">Donor List with Room details</asp:LinkButton></TD><TD style="HEIGHT: 18px"><asp:Button id="btnCloseReport" onclick="btnCloseReport_Click" runat="server" Text="Close Report" CssClass="btnStyle_large"></asp:Button></TD><TD style="HEIGHT: 18px"></TD></TR></TBODY></TABLE><BR /><BR /><BR /></asp:Panel></TD></TR><TR><TD vAlign=top colSpan=2><asp:Panel id="pnlBuildingGrid" runat="server" Width="100%" Height="100%" ForeColor="Blue" BorderColor="White"><asp:Panel id="pnLRoomDetails" runat="server" Height="1px"><TABLE><TBODY><TR><TD><asp:GridView id="dtgRoomDetails" runat="server" ForeColor="#333333" OnSelectedIndexChanged="dtgRoomDetails_SelectedIndexChanged1" AutoGenerateColumns="False" PageSize="5" GridLines="None" CellPadding="4" AllowPaging="True" OnPageIndexChanging="dtgRoomDetails_PageIndexChanging1" OnRowCreated="dtgRoomDetails_RowCreated" OnSorting="dtgRoomDetails_Sorting1">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="room_id" HeaderText="Room Id"></asp:BoundField>
<asp:BoundField DataField="buildingname" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="floor" HeaderText="Floor"></asp:BoundField>
<asp:BoundField DataField="roomno" HeaderText="Room No"></asp:BoundField>
<asp:BoundField DataField="area" HeaderText="Area"></asp:BoundField>
<asp:BoundField DataField="rent" HeaderText="Rent"></asp:BoundField>
<asp:BoundField DataField="deposit" HeaderText="Deposit"></asp:BoundField>
<asp:BoundField DataField="room_cat_name" HeaderText="Room Category"></asp:BoundField>
<asp:BoundField DataField="donor_name" HeaderText="Donor Name"></asp:BoundField>
<asp:BoundField DataField="address1" HeaderText="Donor Address"></asp:BoundField>
<asp:BoundField DataField="maxinmates" HeaderText="Inmates"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR><TR><TD><asp:Panel id="pnlRoomGrid" runat="server" Width="100%" GroupingText="Room details" Height="11%"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:GridView id="dtgRoom" runat="server" Width="639px" ForeColor="#333333" OnSelectedIndexChanged="room_SelectedIndexChanged"  OnRowCreated="room_RowCreated" OnPageIndexChanging="room_PageIndexChanging" AllowPaging="True" CellPadding="4" GridLines="None">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="HEIGHT: 1px"><asp:Panel id="pnlFloorGrid" runat="server" Width="100%" GroupingText="room details" Height="19%"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:GridView id="dtgFloor" runat="server" Width="643px" ForeColor="#333333"  OnRowCreated="floor_RowCreated" OnPageIndexChanging="floor_PageIndexChanging" AllowPaging="True" CellPadding="4" GridLines="None">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD><asp:Panel id="pnlDonorDetails" runat="server" Visible="False"><TABLE><TBODY><TR><TD style="HEIGHT: 328px"><asp:GridView id="dtgDonorDetails" runat="server" Width="802px" ForeColor="#333333" AutoGenerateColumns="False" GridLines="None" CellPadding="4" AllowPaging="True" OnPageIndexChanging="dtgDonorDetails_PageIndexChanging" OnRowCreated="dtgDonorDetails_RowCreated">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:BoundField DataField="donor_name" HeaderText="Donor Name"></asp:BoundField>
<asp:BoundField DataField="housename" HeaderText="House Name"></asp:BoundField>
<asp:BoundField DataField="housenumber" HeaderText="House Number"></asp:BoundField>
<asp:BoundField DataField="address1" HeaderText="Address1"></asp:BoundField>
<asp:BoundField DataField="address2" HeaderText="Address2"></asp:BoundField>
<asp:BoundField DataField="districtname" HeaderText="District"></asp:BoundField>
<asp:BoundField DataField="statename" HeaderText="State"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE></asp:Panel></asp:Panel></TD></TR><TR><TD colSpan=2><cc1:ListSearchExtender id="ListSearchExtender1" runat="server" TargetControlID="cmbBuiildingName"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender2" runat="server" TargetControlID="cmbBuildReport"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender3" runat="server" TargetControlID="cmbDonorName"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender4" runat="server" TargetControlID="cmbFloorNo"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender5" runat="server" TargetControlID="cmbRoomType"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender6" runat="server" TargetControlID="lstFacility"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender7" runat="server" TargetControlID="lstService"></cc1:ListSearchExtender></TD></TR><TR><TD style="HEIGHT: auto" vAlign=top colSpan=2>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Panel id="Panel7" runat="server" Width="304px" Height="50px">&nbsp;<asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" Width="82px" ForeColor="Snow" ControlToValidate="txtSecurityDeposit" ErrorMessage="Enter deposit"></asp:RequiredFieldValidator> <asp:RegularExpressionValidator id="RegularExpressionValidator12" runat="server" Width="123px" ForeColor="Snow" Font-Bold="False" ControlToValidate="txtSecurityDeposit" ErrorMessage="enter valid deposit" ValidationExpression="[1-9]\d[0-9]{0,6}"></asp:RegularExpressionValidator>&nbsp;<asp:RegularExpressionValidator id="RegularExpressionValidator10" runat="server" Width="176px" ForeColor="Snow" Font-Bold="False" ControlToValidate="txtInmatesNo" ErrorMessage="Enter valid inmates number" ValidationExpression="[0-9]{1,2}"></asp:RegularExpressionValidator>&nbsp; <asp:RegularExpressionValidator id="RegularExpressionValidator8" runat="server" Width="126px" ForeColor="LightYellow" ControlToValidate="txtRoomNo" ErrorMessage="Enter valid room no" ValidationExpression="[1-9]\d{0,2}"></asp:RegularExpressionValidator>&nbsp; <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" Width="107px" ForeColor="Snow" Font-Bold="True" ControlToValidate="cmbBuiildingName" ErrorMessage="Select Building" InitialValue="-1"></asp:RequiredFieldValidator>&nbsp; <asp:RequiredFieldValidator id="RequiredFieldValidator7" runat="server" Width="82px" ForeColor="Snow" ControlToValidate="txtRoomNo" ErrorMessage="Enter roomno"></asp:RequiredFieldValidator> <asp:RegularExpressionValidator id="RegularExpressionValidator9" runat="server" Width="135px" ForeColor="Snow" Font-Bold="True" ControlToValidate="txtRoomArea" ErrorMessage="Enter valid area" ValidationExpression="[0-9]\d{0,10}"></asp:RegularExpressionValidator>&nbsp; <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="69px" ForeColor="Snow" Font-Bold="True" ControlToValidate="txtRoomArea" ErrorMessage="Enter Area"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ForeColor="Snow" Font-Bold="True" ControlToValidate="txtRoomRent" ErrorMessage="Enter room rent"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp; <asp:RequiredFieldValidator id="RequiredFieldValidator8" runat="server" ForeColor="WhiteSmoke" ControlToValidate="cmbRoomType" ErrorMessage="Select a Room Type" InitialValue="-1"></asp:RequiredFieldValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator8"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender13" runat="server" TargetControlID="RequiredFieldValidator2"></cc1:ValidatorCalloutExtender>&nbsp;&nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender8" runat="server" TargetControlID="RequiredFieldValidator4"></cc1:ValidatorCalloutExtender> &nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RegularExpressionValidator9"></cc1:ValidatorCalloutExtender></asp:Panel>&nbsp;&nbsp; <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ForeColor="Snow" Font-Bold="True" ControlToValidate="txtInmatesNo" ErrorMessage="Enter inmate no"></asp:RequiredFieldValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" TargetControlID="RegularExpressionValidator12"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" TargetControlID="RequiredFieldValidator6"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RegularExpressionValidator8"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" TargetControlID="RegularExpressionValidator10"></cc1:ValidatorCalloutExtender>&nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender12" runat="server" TargetControlID="RequiredFieldValidator1"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender10" runat="server" TargetControlID="RequiredFieldValidator7"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender11" runat="server" TargetControlID="RequiredFieldValidator3"></cc1:ValidatorCalloutExtender>&nbsp; <asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel8" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" ForeColor="MediumBlue" Text="Tsunami ARMS - Confirmation" Font-Bold="True"></asp:Label></asp:Panel> <asp:TextBox id="TextBox1" runat="server" AutoPostBack="True"></asp:TextBox> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize">
                </ajaxToolkit:ModalPopupExtender> <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button> <asp:Label id="Label1" runat="server" Text="Label"></asp:Label><BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center><asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD style="HEIGHT: 18px" align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 18px"></TD><TD></TD><TD></TD></TR><TR><TD style="HEIGHT: 26px"></TD><TD align=center>&nbsp;&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel> </TD></TR></TBODY></TABLE>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <BR /><BR />
</contenttemplate>
   
  <Triggers>
    <asp:PostBackTrigger ControlID="LnkRoomList" />
    <asp:PostBackTrigger ControlID="LnkDonorList" />
  </Triggers>

   
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <strong><span>
    </span></strong>
    <asp:Panel ID="Panel1" runat="server" GroupingText="User Tips" Height="50px" Width="100%">
        <br />
    This form is used to store room details such as buildingid, floor id, room area , number of inmates, deposit, rent etc.<br />
        <br />
                        By selecting Type of room corrosponding rent,security deposit will load in to the
                        text boxes
        <br />
        <br />
    Save button : Authorised users can save the data.
    Edit button: Authorized users can edit the saved details.<br />
        <br />
                        Delete Button: Authorized users can delete the saved data
    Clear is used to clear the form
        <br />
        <br />
                        Report button: By clicking the button two report link button can see.
        <br />
                        1.Building wise room list:
                        Select a building from combobox and then click the link button shows
                        the report of selected building.
        <br />
                        2. Donor list with Room details: By clicking this link button shows the Donor details
                with room details
    </asp:Panel>
</asp:Content>

