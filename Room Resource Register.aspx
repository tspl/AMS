<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Room Resource Register.aspx.cs" Inherits="Default2" Title="Tsunami ARMS - Room Resource Register " %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="contentplaceholder3" Runat="Server">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        <br />
    
    This form is used for display room with inventory details.<br />
    <br />
    Authorized user can add new building name.<br />
    <br />
    Authorized user can add multiple inventory items for a room.<br />
    <br />
    Resource details report shows the building details for all resources.<br />
    <br />
    Resource details for a building shows the item details for all building.</asp:Panel>
</asp:Content>


<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
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

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"><table><tr>
<td colspan="7" style="text-align: center; width: 100%; height: 708px;" valign="top"><strong>
<span style="font-size: 14pt; color: midnightblue;">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <contenttemplate>
<SPAN style="FONT-SIZE: 14pt"><SPAN style="FONT-WEIGHT: lighter; COLOR: mediumblue"><SPAN style="FONT-WEIGHT: bold; COLOR: #003399; FONT-STYLE: normal">Room Resource Register</SPAN> </SPAN><BR /></SPAN><TABLE><TBODY><TR><TD style="FONT-WEIGHT: lighter" vAlign=top rowSpan=1><asp:Panel id="pnlroom" runat="server" Width="100%" Height="100%" groupingtext="Room Details"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblbuilding" runat="server" Width="95px" Text="Building Name"></asp:Label></TD><TD><asp:DropDownList onkeydown="if(event.keyCode==13)event.keyCode=9;" id="cmbBuilding" runat="server" width="120px" AutoPostBack="true" OnSelectedIndexChanged="cmbBuilding_SelectedIndexChanged" DataTextField="buildingname" DataValueField="build_id"></asp:DropDownList> </TD><TD><asp:LinkButton onkeydown="if(event.keyCode==13)event.keyCode=9;" id="lnkNew" onclick="lnkNew_Click" runat="server" Width="34px" CausesValidation="False" ForeColor="Blue">New</asp:LinkButton> </TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblfloor" runat="server" Width="97px" Text="Floor number" Visible="False"></asp:Label></TD><TD><asp:DropDownList onkeydown="if(event.keyCode==13)event.keyCode=9;" id="cmbfloor" runat="server" width="120px" Visible="False" AutoPostBack="true"><asp:ListItem></asp:ListItem>
</asp:DropDownList> </TD><TD><asp:LinkButton onkeydown="if(event.keyCode==13)event.keyCode=9;" id="lnknw" runat="server" Width="35px" CausesValidation="False" Visible="False">New</asp:LinkButton> </TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="Label1" runat="server" Width="96px" Text="Room Number"></asp:Label></TD><TD><asp:DropDownList onkeydown="if(event.keyCode==13)event.keyCode=9;" id="cmbRoomNo" runat="server" Width="120px" DataTextField="roomno" DataValueField="roomno"></asp:DropDownList></TD><ASP:LISTITEM /><ASP:LISTITEM /><TD></TD></TR></TBODY></TABLE><BR /><BR /></asp:Panel> </TD><TD style="FONT-WEIGHT: lighter" vAlign=top colSpan=6><asp:Panel id="pnlinventory" runat="server" Width="100%" GroupingText="Inventory Details"><TABLE><TBODY><TR><TD style="HEIGHT: 24px; TEXT-ALIGN: justify"><asp:Label id="lblcategory" runat="server" Width="85px" Text="Item Category"></asp:Label></TD><TD style="HEIGHT: 24px"><asp:DropDownList id="cmbItemCategory" runat="server" Width="152px" DataValueField="itemcat_id" DataTextField="itemcatname" OnSelectedIndexChanged="cmbItemCategory_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD></TR><TR><TD style="TEXT-ALIGN: justify"><asp:Label id="lblitemname" runat="server" Width="73px" Text="Item Name"></asp:Label></TD><TD><asp:DropDownList onkeydown="if(event.keyCode==13)event.keyCode=9;" id="cmbName" runat="server" Width="152px" DataValueField="item_id" DataTextField="itemname" OnSelectedIndexChanged="cmbName_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True"></asp:DropDownList></TD></TR><TR><TD style="TEXT-ALIGN: justify"><asp:Label id="lblicode" runat="server" Width="64px" Text="Item Code"></asp:Label></TD><TD><asp:TextBox id="txtIcode" runat="server" Width="147px" Enabled="False"></asp:TextBox></TD></TR><TR><TD style="TEXT-ALIGN: justify"><asp:Label id="lblclasslevel" runat="server" Width="72px" Text="Item Class "></asp:Label></TD><TD><asp:TextBox onkeydown="if(event.keyCode==13)event.keyCode=9;" id="txtClass" runat="server" Width="147px" Enabled="False"></asp:TextBox></TD></TR><TR><TD style="TEXT-ALIGN: justify"><asp:Label id="lblmodel" runat="server" Width="53px" Text="Model"></asp:Label></TD><TD><asp:TextBox onkeydown="if(event.keyCode==13)event.keyCode=9;" id="txtModel" runat="server" Width="147px" Enabled="False"></asp:TextBox></TD></TR><TR><TD style="TEXT-ALIGN: justify"><asp:Label id="lblitemcode" runat="server" Width="49px" Text="Maker"></asp:Label></TD><TD><asp:TextBox onkeydown="if(event.keyCode==13)event.keyCode=9;" id="txtItemMaker" runat="server" Width="147px" Enabled="False"></asp:TextBox></TD></TR><TR><TD style="TEXT-ALIGN: justify"><asp:Label id="lblQty" runat="server" Text="Quantity"></asp:Label></TD><TD><asp:TextBox id="txtQuantity" runat="server" Width="147px"></asp:TextBox></TD></TR><TR><TD style="TEXT-ALIGN: justify"></TD><TD><asp:Button id="btnAdd" onclick="btnAdd_Click" runat="server" Text="Add item" ValidationGroup="Quant" CssClass="btnStyle_small"></asp:Button></TD></TR><TR><TD style="TEXT-ALIGN: justify" colSpan=2><asp:Panel id="Panel3" runat="server" GroupingText="Add Details" Visible="False"><asp:GridView id="dtgAddItem" runat="server" ForeColor="#333333" AutoGenerateColumns="False" CellPadding="4" GridLines="None" __designer:wfdid="w2">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:BoundField DataField="itemname" HeaderText="Item Name"></asp:BoundField>
<asp:BoundField DataField="Itemcategory" HeaderText="Item Category"></asp:BoundField>
<asp:BoundField DataField="itemcode" HeaderText="Item Code"></asp:BoundField>
<asp:TemplateField><ItemTemplate>
<asp:LinkButton id="lnkDelete" runat="server" OnCommand="itemDelete" CommandArgument='<%#Eval("itemcode").ToString()%>' __designer:wfdid="w9" OnClick="lnkDelete_Click">Delete</asp:LinkButton> 
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>
<EmptyDataTemplate>
<asp:LinkButton id="lnkDelete" runat="server">Delete</asp:LinkButton>
</EmptyDataTemplate>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel><BR /></TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD style="FONT-WEIGHT: lighter" colSpan=7 rowSpan=1><asp:Panel id="Panel2" runat="server" Width="100%"><asp:Button id="btnSave" onclick="btnSave_Click" runat="server" Text="Save" ValidationGroup="vsave" CssClass="btnStyle_small"></asp:Button> <asp:Button id="btndelete" onclick="btndelete_Click1" runat="server" Text="Delete" CssClass="btnStyle_small"></asp:Button> <asp:Button id="btnclear" onclick="btnclear_Click1" runat="server" CausesValidation="False" Text="Clear" CssClass="btnStyle_small"></asp:Button> <asp:Button id="Button2" onclick="Button2_Click1" runat="server" CausesValidation="False" Text="Report" Visible="False" CssClass="btnStyle_small"></asp:Button> <asp:Button id="btnEdit" onclick="btnEdit_Click" runat="server" Text="Edit" Enabled="False" CssClass="btnStyle_small"></asp:Button></asp:Panel></TD></TR><TR><TD style="FONT-WEIGHT: lighter" colSpan=7 rowSpan=1><asp:Panel id="pnlroomdetails" runat="server" Width="100%" Height="100%" groupingtext="Room details" Visible="False"><asp:GridView id="dtgBuilding" runat="server" Width="100%" Height="100%" Visible="False" ForeColor="#333333" OnSelectedIndexChanged="dtgBuilding_SelectedIndexChanged" GridLines="None" CellPadding="4" OnPageIndexChanging="dtgBuilding_PageIndexChanging" AllowPaging="True" Font-Size="Small" Font-Bold="False" OnRowCreated="dtgBuilding_RowCreated">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> </asp:Panel></TD></TR><TR><TD style="FONT-WEIGHT: lighter" colSpan=7 rowSpan=1><asp:Panel id="pnlinvdetails" runat="server" Width="100%" Height="100%" GroupingText="Inventory details" ForeColor="#804040" Visible="False"><asp:GridView id="GridView2" runat="server" Width="100%" Height="85%" Visible="False" ForeColor="#333333" GridLines="None" CellPadding="4" Font-Bold="False" Font-Size="Small" AllowPaging="True" AllowSorting="True">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel></TD></TR><TR><TD colSpan=7 rowSpan=1><asp:Panel id="pnlinv" runat="server" Width="100%" Height="100%" GroupingText="Room with inventory details"><asp:GridView id="dtgRoomInventory" runat="server" Width="100%" Height="100%" ForeColor="#333333" OnSelectedIndexChanged="dtgRoomInventory_SelectedIndexChanged" AutoGenerateColumns="False" CellPadding="4" GridLines="None" OnPageIndexChanging="dtgRoomInventory_PageIndexChanging" AllowPaging="True" Font-Size="Small" Font-Bold="False" OnRowCreated="dtgRoomInventory_RowCreated" OnSorting="dtgRoomInventory_Sorting" DataKeyNames="resource_id,item_id">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="itemname" HeaderText="Item Name"></asp:BoundField>
<asp:BoundField DataField="quantity" HeaderText="Quantity"></asp:BoundField>
<asp:BoundField DataField="unitname" HeaderText="Unit Name"></asp:BoundField>
<asp:BoundField DataField="buildingname" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="roomno" HeaderText="Room"></asp:BoundField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> </asp:Panel></TD></TR><TR><TD style="TEXT-ALIGN: left" colSpan=7 rowSpan=2><asp:Panel id="pnlnn" runat="server" Width="125px" Height="50px"><cc1:listsearchextender id="ListSearchExtender1" runat="server" targetcontrolid="cmbbuilding"></cc1:listsearchextender> <cc1:listsearchextender id="ListSearchExtender2" runat="server" targetcontrolid="cmbfloor"></cc1:listsearchextender>&nbsp; <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Width="206px" ForeColor="White" Font-Bold="True" ValidationGroup="vsave" Font-Size="Small" ErrorMessage="Select Building Name" ControlToValidate="cmbBuilding"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="143px" ForeColor="White" Font-Bold="True" ValidationGroup="Quant" Font-Size="Small" ErrorMessage="Must Item Name" ControlToValidate="cmbName"></asp:RequiredFieldValidator> <cc1:listsearchextender id="ListSearchExtender3" runat="server" targetcontrolid="cmbname"></cc1:listsearchextender> <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ForeColor="White" Font-Bold="True" Font-Size="Small" ErrorMessage="Select a Floor" ControlToValidate="cmbfloor"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ForeColor="White" Font-Bold="True" ValidationGroup="vsave" Font-Size="Small" ErrorMessage="Select a Room No" ControlToValidate="cmbRoomNo"></asp:RequiredFieldValidator> <cc1:listsearchextender id="ListSearchExtender4" runat="server" targetcontrolid="cmbroomno"></cc1:listsearchextender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator1"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator2"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator3"></cc1:ValidatorCalloutExtender><BR /><cc1:FilteredTextBoxExtender id="FilteredTextBoxExtender1" runat="server" TargetControlID="txtQuantity" FilterType="Numbers"></cc1:FilteredTextBoxExtender><BR /><asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" Width="138px" ForeColor="White" __designer:wfdid="w2" ValidationGroup="Quant" ErrorMessage="Must enter Quantity" ControlToValidate="txtQuantity"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" __designer:wfdid="w3" TargetControlID="RequiredFieldValidator5"></cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" Width="260px" ForeColor="White" __designer:wfdid="w17" ValidationGroup="Quant" ErrorMessage="Must select item category" ControlToValidate="cmbItemCategory"></asp:RequiredFieldValidator><BR /><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" __designer:wfdid="w18" TargetControlID="RequiredFieldValidator6"></cc1:ValidatorCalloutExtender><BR /><BR /><asp:Button style="DISPLAY: none" id="btnHidden" onclick="btnHidden_Click" runat="server" Text="Hidden"></asp:Button>&nbsp;<asp:TextBox id="TextBox1" runat="server" Visible="False" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox><BR /><asp:Label id="Label3" runat="server" Text="Label" Visible="False"></asp:Label><BR /><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel7" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" ForeColor="MediumBlue" Text="Tsunami ARMS - " Font-Bold="True"></asp:Label></asp:Panel> <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" __designer:wfdid="w13" Font-Size="Small"></asp:Label><BR /></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center><asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" __designer:wfdid="w15" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" __designer:wfdid="w16" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel><BR /><BR /><cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" TargetControlID="btnHidden" RepositionMode="RepositionOnWindowResize" PopupControlID="pnlMessage">
                </cc1:ModalPopupExtender><BR /></asp:Panel> <asp:Panel id="Panelrep" runat="server" GroupingText="Report" ForeColor="White" BackColor="White" Visible="False" __designer:wfdid="w5"><TABLE><TBODY><TR><TD style="FONT-WEIGHT: lighter; WIDTH: 134px" align=left><asp:LinkButton id="lnkroomlist" runat="server" Width="70px" Visible="False" __designer:wfdid="w6">Room List</asp:LinkButton></TD><TD style="FONT-WEIGHT: lighter; WIDTH: 129px" align=left><asp:LinkButton id="lnkinventorylist" runat="server" width="118px" Visible="False" __designer:wfdid="w7">Inventory List</asp:LinkButton></TD><TD style="FONT-WEIGHT: lighter; WIDTH: 146px" align=left><asp:LinkButton id="lnlrlink" runat="server" Width="155px" Visible="False" __designer:wfdid="w8">Room Inventory List</asp:LinkButton></TD></TR><TR><TD style="FONT-WEIGHT: lighter; WIDTH: 134px" align=left><asp:DropDownList id="cmbres" runat="server" Width="150px" Visible="False" __designer:wfdid="w9"></asp:DropDownList></TD><TD style="FONT-WEIGHT: lighter; WIDTH: 129px" align=left><asp:DropDownList id="cmbbuil" runat="server" Width="154px" Visible="False" __designer:wfdid="w10"></asp:DropDownList></TD><TD style="FONT-WEIGHT: lighter; WIDTH: 146px" align=left></TD></TR><TR><TD style="FONT-WEIGHT: lighter; WIDTH: 134px" align=left><asp:LinkButton id="lnkresource" runat="server" Width="217px" __designer:wfdid="w11">building  details for a resource </asp:LinkButton></TD><TD style="FONT-WEIGHT: lighter" align=left colSpan=2><asp:LinkButton id="lnkrb" runat="server" __designer:wfdid="w12">Resource details for a building</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR></TR></TBODY></TABLE>
</contenttemplate>



 <%--<Triggers>
    <asp:PostBackTrigger ControlID="" />
     
    <asp:PostBackTrigger ControlID="" />
 
  </Triggers>--%>
    </asp:UpdatePanel><br />
    <br />
</span>
    
</strong></td></tr></table>
</asp:Content>

