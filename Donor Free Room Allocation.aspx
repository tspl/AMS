<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Donor Free Room Allocation.aspx.cs" Inherits="Donor_Free_Room_Allocation" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
<style>
         .modalBackground
{
background-color: Gray;
filter: alpha(opacity=80);
opacity: 0.8;
z-index: 10000;
}
</style>
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
 <p>
        <br />
        This form is used for room allocation for Donor free pass.<br /></p>
        <br />
             <p>
            Use&nbsp; <strong>Tab Key </strong>or <strong>Mouse Click</strong>,
        To go to the Next Field. &nbsp;</p>
        <p>
            Use <strong>Mouse </strong>&nbsp;to select Data from the grid.</p>
        </asp:Panel>
           <p>
            Use <strong>Edit &nbsp;</strong>button for editting cashier details, check in 
            date and time).</p>
               <p>
            Use <strong>Edit &nbsp;</strong>button for editting cashier details, check in 
            date and time).</p>
        <p>
            Use <strong>Report </strong>button to view Reports.</p>
        <p>
            Press <strong>View Alloc </strong>button to View Allocation.</p>
        <p>
            Press <strong>Allocate </strong>button for saving an allocation and print 
            receipt after enter all mandatory fields.</p>
        <p>
            Use <strong>Add </strong>button for allocate multiple room.</p>
        <p>
            Use<strong> AltRoom </strong>button for changing room in case of donor allocation.</p>
        <p>
            Use<strong> Reallocate</strong> button for Reallocation/Changing room in general
            allocation.</p>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE><TBODY><TR align=center><TD colSpan=3><asp:Panel id="pnlcash" runat="server" 
        GroupingText="Counter Status" Enabled="False"><TABLE><TBODY><TR>
    <td>
        <asp:Label ID="lblstaff" runat="server" Text="Staff Name:" Width="69px"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="txtstaffname" runat="server" ReadOnly="true" Width="90px"></asp:TextBox>
    </td>
    <td>
        <asp:Label ID="Label102" runat="server" Text="Cashier Liability" Width="93px"></asp:Label>
    </td>
    <TD>
        <asp:TextBox ID="txtcashierliability" runat="server" Font-Bold="True" 
            Font-Size="Small" tabIndex="53" Width="66px"></asp:TextBox>
    </TD><TD style="WIDTH: 100px">
        <asp:Label ID="Label3" runat="server" Text="Receipt no:" Width="69px"></asp:Label>
    </TD><TD style="width: 17%">
        <asp:TextBox ID="txtreceiptno1" runat="server" 
            OnTextChanged="txtreceiptno1_TextChanged" tabIndex="51" Width="66px"></asp:TextBox>
    </TD>
   </TD><TD style="WIDTH: 100px"><asp:Label id="Label1" runat="server" Width="122px" Text="No of Transactions"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtnooftrans" tabIndex=55 runat="server" Width="66px"></asp:TextBox></TD> <td style="WIDTH: 100px">
                        <asp:Label ID="Label4" runat="server" Text="Uncliamed " Width="103px"></asp:Label>
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:TextBox ID="txtunclaimed" runat="server" Font-Bold="True" 
                            Font-Size="Small" tabIndex="54" Width="66px" Height="22px"></asp:TextBox>
                    </td></TR>
    </TR><tr><td width="16%">
        <asp:Label ID="Label2" runat="server" Text="Login Time:" Width="69px"></asp:Label>
        </td><td width="17%">
            <asp:TextBox ID="txtlogintime" runat="server" ReadOnly="true" Width="90px"></asp:TextBox>
 </td>
        <td align="left">
            Today&#39;s Collection</td>
        <td>
            <asp:TextBox ID="txtcounterliability" runat="server" Width="66px"></asp:TextBox>
        </td>
        <td width="17%"><asp:Label ID="Label10" runat="server" Text="Balance Receipt" Width="97px"></asp:Label></td>
        <td width="16%"><asp:TextBox ID="txtreceiptno2" runat="server" 
                OnTextChanged="txtreceiptno2_TextChanged" tabIndex="52" Width="66px"></asp:TextBox></td>      
        </td><td width="17%"><asp:Label id="Label18" runat="server" Width="103px" Text="Security Deposit"></asp:Label></td>
            <td style="width: 17%"><asp:TextBox id="txttotsecurity" tabIndex=54 runat="server" Width="66px" Font-Bold="True" Font-Size="Small"></asp:TextBox></td>
            <td style="WIDTH: 100px">
                        <asp:Label ID="Label19" runat="server" Text="Counter Deposit" Width="103px"></asp:Label>
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:TextBox ID="txtcounterdeposit" runat="server" Font-Bold="True" 
                            Font-Size="Small" tabIndex="54" Width="66px"></asp:TextBox>
                    </td>
    </tr></TBODY></TABLE></asp:Panel></TD></TR><TR align=center><TD id="TD1" colSpan=3>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label 
        id="lblhead" runat="server" Text="DONOR FREE ROOM ALLOCATION" Font-Bold="True" 
        Font-Size="Medium" CssClass="heading"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:CheckBox 
        id="chkplainpaper" runat="server" Width="153px" Text="Old receipt" 
        AutoPostBack="True" OnCheckedChanged="chkplainpaper_CheckedChanged" 
        Visible="False"></asp:CheckBox> <asp:GridView id="donorgrid" runat="server" 
            Width="840px" ForeColor="#333333" CellPadding="4" GridLines="None" 
            OnSelectedIndexChanged="donorgrid_SelectedIndexChanged" Visible="False">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns><asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField></Columns>
<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left"></RowStyle>
<EditRowStyle BackColor="#2461BF"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True" VerticalAlign="Top"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> &nbsp; </TD></TR><TR align=center><TD colSpan=3><asp:Panel id="donorallocpanel" runat="server" GroupingText="Donor Allocation"><TABLE><TBODY><TR><TD style="WIDTH: 100px">
        <asp:CheckBox ID="chkclubbing" runat="server" AutoPostBack="True" 
            oncheckedchanged="rbclubbing_CheckedChanged" Text="Clubbing" />
        </TD><TD style="WIDTH: 100px">&nbsp;</TD><TD style="WIDTH: 100px">
        <asp:Label ID="Label106" runat="server" Text="Type"></asp:Label>
        </TD><TD colspan="2">
            <asp:DropDownList ID="cmballoctype" runat="server" Width="190px" 
                AutoPostBack="True" Enabled="False" 
                onselectedindexchanged="cmballoctype_SelectedIndexChanged" 
                DataTextField="TYPE" DataValueField="TYPE">
            </asp:DropDownList>
        </TD><TD style="WIDTH: 100px">
            &nbsp;</TD><TD style="WIDTH: 100px">
            &nbsp;</TD><TD>&nbsp;</TD></TR>
        <tr>
            <td style="WIDTH: 100px">
                <asp:Label ID="Label17" runat="server" Text="Barcode"></asp:Label>
            </td>
            <td style="WIDTH: 100px">
                <asp:TextBox ID="txtdonortype" runat="server" AutoPostBack="True" 
                    OnTextChanged="txtdonortype_TextChanged" tabIndex="1" Width="110px"></asp:TextBox>
            </td>
            <td style="WIDTH: 100px">
                <asp:Label ID="lbldonorpass" runat="server" Text="Pass no" Width="56px"></asp:Label>
            </td>
            <td style="WIDTH: 100px">
                <asp:TextBox ID="txtdonorpass" runat="server" AutoPostBack="True" 
                    OnTextChanged="txtdonorpass_TextChanged" tabIndex="2" Width="70px" 
                    Height="22px"></asp:TextBox>
                &nbsp;</td>
            <td style="WIDTH: 100px">
                <asp:Label ID="lblstatus" runat="server" BackColor="Red" Font-Bold="True" 
                    Font-Size="Small" ForeColor="Gold" Width="106px"></asp:Label>
            </td>
            <td style="WIDTH: 100px">
                <asp:Label ID="Label5" runat="server" Text="Donor name" Width="90px"></asp:Label>
            </td>
            <td style="WIDTH: 100px">
                <asp:TextBox ID="txtdonorname" runat="server" tabIndex="3" Enabled="false" 
                    Width="190px"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnpass" runat="server" BackColor="#8080FF" 
                    CausesValidation="False" Font-Bold="True" onclick="btnpass_Click" 
                    Text="Add pass" UseSubmitBehavior="False" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="Label105" runat="server" Text="Reserve No."></asp:Label>
                <asp:TextBox ID="txtReserveNo" runat="server" AutoPostBack="True" Height="17px" 
                    ontextchanged="txtReserveNo_TextChanged" Width="200px"></asp:TextBox>
            </td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        </TBODY></TABLE></asp:Panel> 
        <asp:Label id="lblreceipt" runat="server" Text="Ledger No"></asp:Label><asp:TextBox id="txtreceipt" runat="server" Width="103px" AutoPostBack="True" OnTextChanged="txtreceipt_TextChanged"></asp:TextBox></TD></TR><TR><TD>
        <asp:Panel id="swamipanel" runat="server" Height="1%" 
            GroupingText="Swami Details" Width="100%"><TABLE><TBODY><TR>
                <TD style="WIDTH: 162px; HEIGHT: 2px">
                    <asp:Label id="lblswaminame" runat="server" Width="78px" 
                        Text="Swami name" Font-Bold="False"></asp:Label></TD>
                <TD style="WIDTH: 284px; HEIGHT: 2px">
                    <asp:TextBox id="txtswaminame" tabIndex=5 runat="server" 
                        Width="200px" Height="17px" CssClass="UpperCaseFirstLetter" 
                        AutoPostBack="True" 
                        OnTextChanged="txtswaminame_TextChanged1"></asp:TextBox></TD><TD></TD></TR><TR>
                <TD style="WIDTH: 162px; HEIGHT: 23px"><asp:Label id="Label12" 
                        runat="server" Width="82px" Text="Place"></asp:Label></TD>
                <TD style="WIDTH: 284px; HEIGHT: 23px">
                    <asp:TextBox ID="txtplace" runat="server" 
                        AutoPostBack="True" CssClass="UpperCaseFirstLetter" 
                        Height="17px" OnTextChanged="txtplace_TextChanged" 
                        tabIndex="6" Width="200px"></asp:TextBox>
                </TD><TD><BR /></TD></TR><TR>
                <TD style="WIDTH: 162px; HEIGHT: 14px"><asp:Label id="lblstate" 
                        runat="server" Width="79px" Text="State"></asp:Label></TD>
                <TD style="WIDTH: 284px; HEIGHT: 14px">
                    <asp:DropDownList id="cmbState" tabIndex=20 runat="server" 
                        Width="205px" Height="22px" 
                        OnSelectedIndexChanged="cmbState_SelectedIndexChanged" 
                        DataValueField="state_id" DataTextField="statename" 
                        AppendDataBoundItems="True" AutoPostBack="True"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD>
                    <BR />
                </TD></TR><TR><TD style="WIDTH: 162px; height: 26px;">
                    <asp:Label id="Label11" runat="server" Width="80px" 
                        Text="District"></asp:Label>
                    <BR />
                    </TD><TD style="WIDTH: 284px; height: 26px;">
                        <asp:DropDownList ID="cmbDists" runat="server" 
                            DataTextField="districtname" DataValueField="district_id" 
                            Height="22px" 
                            OnSelectedIndexChanged="cmbDists_SelectedIndexChanged" 
                            tabIndex="20" Width="205px">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                        </asp:DropDownList>
                    </TD><TD style="HEIGHT: 26px">
                        <asp:LinkButton ID="lnkdistrict" runat="server" 
                            CausesValidation="False" onclick="lnkdistrict_Click">New</asp:LinkButton>
                    </TD></TR><TR><TD style="WIDTH: 162px; ">
                    <asp:Label id="lblphone" runat="server" Width="79px" 
                        Text="Phone"></asp:Label></TD>
                    <TD style="WIDTH: 284px; ">
                        <asp:TextBox ID="txtphone" runat="server" Height="17px" 
                            OnTextChanged="txtphone_TextChanged" tabIndex="20" 
                            Width="200px"></asp:TextBox>
                    </TD><TD><BR /></TD></TR><TR>
                <TD style="WIDTH: 162px; HEIGHT: 31px">
                    <asp:Label id="lblidproof" runat="server" Width="79px" 
                        Text="Identity proof"></asp:Label></TD>
                <TD style="WIDTH: 284px; HEIGHT: 31px">
                    <asp:DropDownList ID="cmbIDp" runat="server" Height="22px" 
                        tabIndex="20" Width="205px">
                        <asp:ListItem>--Select--</asp:ListItem>
                        <asp:ListItem>Election ID</asp:ListItem>
                        <asp:ListItem>Driving License</asp:ListItem>
                        <asp:ListItem>Pass Port</asp:ListItem>
                        <asp:ListItem>Other</asp:ListItem>
                    </asp:DropDownList>
                </TD><TD>
                    <BR />
                </TD></TR>
                <tr>
                    <td style="WIDTH: 162px; HEIGHT: 20px">
                        <asp:Label ID="Label8" runat="server" 
                            Text="Identity ref: no" Width="83px"></asp:Label>
                    </td>
                    <td style="WIDTH: 284px; HEIGHT: 20px">
                        <asp:TextBox ID="txtidrefno" runat="server" 
                            EnableTheming="True" Height="17px" 
                            OnTextChanged="txtidrefno_TextChanged" tabIndex="20" 
                            Width="200px"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                </TBODY></TABLE></asp:Panel> </TD>
        <TD><asp:Panel id="roomallocationpanel" runat="server" 
                Height="1%" GroupingText="Room Allocation Details" Width="100%"><TABLE><TBODY><TR><TD><asp:Label id="lblnoofinmates" runat="server" Width="85px" Text="No: of Inmates"></asp:Label></TD>
                <TD style="width: 155px">
                <asp:TextBox id="txtnoofinmates" tabIndex=12 runat="server" Width="150px" 
                    Height="17px" AutoPostBack="True" OnTextChanged="TextBox5_TextChanged"></asp:TextBox></TD><TD><BR /></TD></TR>
                   <TR><TD>
                <asp:Label ID="lblProposedCheckOutDate" runat="server" 
                    style="POSITION: relative" Text="Check out date" Width="92px"></asp:Label>
                </TD><TD style="width: 155px">
                    <asp:TextBox ID="txtcheckout" runat="server" AutoPostBack="True" Height="17px" 
                        OnTextChanged="txtcheckout_TextChanged" style="POSITION: relative; top: 0px; left: -1px;" 
                        tabIndex="16" Width="150px"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" 
                        TargetControlID="txtcheckout">
                    </cc1:CalendarExtender>
                </TD><TD></TD></TR><TR><TD>
                <asp:Label ID="lblcheckouttime0" runat="server" style="POSITION: relative" 
                    Text="Check out time" Width="90px"></asp:Label>
                </TD><TD style="width: 155px">
                    <asp:TextBox ID="txtcheckouttime" runat="server" AutoPostBack="True" OnTextChanged="txtcheckouttime_TextChanged" 
                        style="LEFT: 0px; POSITION: relative; top: 0px; height: 17px;" tabIndex="17" 
                        Width="150px"></asp:TextBox>
                </TD><TD><BR /></TD></TR><TR><TD>
                <asp:Label ID="Label104" runat="server" __designer:wfdid="w2" 
                    style="LEFT: 0px; POSITION: relative; TOP: 3px" Text="No of days(hrs)" 
                    Width="95px"></asp:Label>
                </TD><TD style="width: 155px">
                    <asp:TextBox ID="txtnoofdays" runat="server" __designer:wfdid="w1" 
                        AutoPostBack="True" Height="17px" OnTextChanged="txtnoofdays_TextChanged" 
                        ReadOnly="True" style="LEFT: 0px; POSITION: relative; TOP: 0px" tabIndex="15" 
                        Width="150px" Enabled="False"></asp:TextBox>
                </TD><TD>&nbsp;</TD></TR>
                 <TR><TD><asp:Label id="lblbuildingname" runat="server" Width="87px" Text="Building name"></asp:Label></TD>
                     <TD style="width: 155px">
                <asp:DropDownList id="cmbBuild" tabIndex=13 runat="server" Width="155px" 
                    Height="22px" AutoPostBack="True" 
                    OnSelectedIndexChanged="cmbBuild_SelectedIndexChanged" 
                    DataTextField="buildingname" DataValueField="build_id"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD><BR /></TD></TR><TR><TD style="height: 26px"><asp:Label id="lbroomno" runat="server" Width="74px" Text="Room no"></asp:Label></TD>
                <TD style="width: 155px; height: 26px;">
                <asp:DropDownList id="cmbRooms" tabIndex=14 runat="server" Width="155px" 
                    AutoPostBack="True" OnSelectedIndexChanged="cmbRooms_SelectedIndexChanged" 
                    DataTextField="roomno" DataValueField="room_id"><asp:ListItem Value="-1">--Select--</asp:ListItem>
</asp:DropDownList></TD><TD style="height: 26px"><BR /></TD></TR>
                <TR><TD>
                <asp:Label ID="lblcheckindate0" runat="server" __designer:wfdid="w6" 
                    style="LEFT: 2px; POSITION: relative; TOP: 0px" Text="Check in date" 
                    Width="85px"></asp:Label>
                </TD><TD style="width: 155px">
                    <asp:TextBox ID="txtcheckindate" runat="server" AutoPostBack="True" 
                        Enabled="False" Height="17px" OnTextChanged="txtcheckindate_TextChanged" 
                        style="LEFT: 0px; POSITION: relative" tabIndex="57" Width="150px"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtcheckindate_CalendarExtender" runat="server" 
                        Format="dd/MM/yyyy" TargetControlID="txtcheckindate">
                    </cc1:CalendarExtender>
                </TD><TD></TD></TR>
                <tr>
                    <td>
                        <asp:Label ID="lblcheckintime1" runat="server" style="POSITION: relative; top: 1px; left: 0px;" 
                            Text="Check in time" Width="85px"></asp:Label>
                    </td>
                    <td style="width: 155px">
                        <asp:TextBox ID="txtcheckintime" runat="server" AutoPostBack="True" 
                            Enabled="False" Height="17px" OnTextChanged="txtcheckintime_TextChanged" 
                            style="POSITION: relative; top: 2px; left: 0px;" tabIndex="56" 
                            Width="150px"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td align="center" style="width: 155px">
                        <asp:Button ID="btnclubadd" runat="server" CssClass="btnStyle_small" 
                            Font-Bold="True" onclick="btnclubadd_Click" tabIndex="20" Text="Add" 
                            Visible="False" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                </TBODY></TABLE></asp:Panel> </TD>
        <TD><asp:Panel id="rentpanel" runat="server" Height="1%" GroupingText="Rent" 
                style="margin-left: 0px" Width="279px"><TABLE><TBODY><TR>
            <TD style="WIDTH: 132px; HEIGHT: 26px"><asp:Label id="lblroomrent" runat="server" Width="73px" Text="Room rent"></asp:Label></TD>
            <TD style="WIDTH: 109px; HEIGHT: 26px"><asp:TextBox id="txtroomrent" tabIndex=16 
                    runat="server" Width="90px" Height="17px" Font-Bold="True" Enabled="False" 
                    OnTextChanged="txtroomrent_TextChanged"></asp:TextBox></TD>
            <TD style="WIDTH: 62px; HEIGHT: 26px"><BR /></TD></TR><TR>
            <TD style="WIDTH: 132px"><asp:Label id="lblsecuritydeposit" runat="server" Width="97px" Text="Security deposit"></asp:Label></TD>
            <TD style="WIDTH: 109px"><asp:TextBox id="txtsecuritydeposit" tabIndex=17 
                    runat="server" Width="90px" Height="17px" Font-Bold="True" Enabled="False"></asp:TextBox></TD>
            <TD style="WIDTH: 62px"><BR /></TD></TR><TR>
            <TD style="WIDTH: 132px; HEIGHT: 26px"><asp:Label id="Label7" runat="server" Width="77px" Text="Other charge"></asp:Label></TD>
            <TD style="WIDTH: 109px; HEIGHT: 26px">
                <asp:TextBox id="txtothercharge" tabIndex=18 
                    runat="server" Width="90px" Height="17px" Font-Bold="True" AutoPostBack="True" 
                    OnTextChanged="txtothercharge_TextChanged"></asp:TextBox></TD>
            <TD style="WIDTH: 62px; HEIGHT: 26px"><BR /></TD></TR><TR>
                       <td style="WIDTH: 132px; HEIGHT: 18px">
                        <asp:Label ID="lbltotalamount" runat="server" Text="Total amount" Width="80px"></asp:Label>
                    </td>
                    <td style="WIDTH: 109px; HEIGHT: 18px">
                        <asp:TextBox ID="txttotalamount" runat="server" Enabled="False" 
                            Font-Bold="True" Font-Size="X-Large" ForeColor="OliveDrab" Height="33px" 
                            tabIndex="20" Width="90px" Wrap="False"></asp:TextBox>
                    </td></tr><tr>
            <TD style="WIDTH: 132px; HEIGHT: 18px">
                <asp:Label ID="Label101" runat="server" Text="Amount received" Width="110px"></asp:Label>
            </TD><TD style="WIDTH: 109px; HEIGHT: 18px">
                    <asp:TextBox id="txtadvance" 
                    tabIndex=19 runat="server" Width="90px" Height="17px" 
                    OnTextChanged="txtadvance_TextChanged"></asp:TextBox></TD>
            <TD style="WIDTH: 62px; HEIGHT: 18px"></TD></TR><TR>
            <TD style="WIDTH: 132px; HEIGHT: 18px"><b>Balance payable</b></TD>
<TD style="WIDTH: 109px; HEIGHT: 18px"><asp:TextBox ID="txtnetpayment0" runat="server" Height="17px" TabIndex="21" Width="90px"></asp:TextBox>
                </TD>
            <TD style="WIDTH: 62px; HEIGHT: 18px">&nbsp;</TD></TR>
                <tr>
         
                    <td style="WIDTH: 132px; HEIGHT: 18px">
                        Inmate charge</td>
                    <td style="WIDTH: 109px; HEIGHT: 18px">
                        <asp:TextBox ID="txtinmatecharge" runat="server" Height="17px" 
                            OnTextChanged="txtadvance_TextChanged" tabIndex="20" Width="90px"></asp:TextBox>
                    </td>
                    <td style="WIDTH: 62px; HEIGHT: 18px">
                        &nbsp;</td>
                </tr>
                <tr>
         
                    <td style="WIDTH: 62px; HEIGHT: 18px">
                        <asp:Label ID="Label108" runat="server" Text="Inmate Deposit" Width="90px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtinmatedeposit" runat="server" Height="17px" 
                            OnTextChanged="txtadvance_TextChanged" tabIndex="20" Width="90px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="WIDTH: 132px; HEIGHT: 17px">
                        <asp:Label ID="Label6" runat="server" Text="Grant total"></asp:Label>
                    </td>
                    <td style="WIDTH: 109px; HEIGHT: 17px">
                        <asp:TextBox ID="txtgranttotal" runat="server" Enabled="False" Font-Bold="True" 
                            Font-Size="X-Large" ForeColor="#FF3300" Height="33px" tabIndex="22" 
                            Width="90px"></asp:TextBox>
                    </td>
                    <td style="WIDTH: 62px; HEIGHT: 17px">
                    </td>
                </tr>
                </TBODY></TABLE></asp:Panel> </TD></TR><TR><TD align=center colSpan=3><TABLE><TBODY><TR><TD><asp:Button id="btnallocate" tabIndex=18 onclick="btnallocate_Click" runat="server" Text="Allocate" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button></TD><TD><asp:Button id="btneditcash" tabIndex=26 onclick="btneditcash_Click" runat="server" CausesValidation="False" Text="Edit" Font-Bold="True" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnadd" tabIndex=20 onclick="btnadd_Click" runat="server" Text="Add" Font-Bold="True" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnclear" tabIndex=19 onclick="btnclear_Click" runat="server" CausesValidation="False" Text="Clear" Font-Bold="True" CssClass="btnStyle_small"></asp:Button></TD><TD>
        <asp:Button id="btnaltroom" tabIndex=21 onclick="btnaltroom_Click" 
            runat="server" CausesValidation="False" Text="Change room" Font-Bold="True" 
            CssClass="btnStyle_medium"></asp:Button></TD><TD>
        <asp:Button id="btncancel" tabIndex=23 runat="server" 
            CausesValidation="False" Font-Bold="True" CssClass="btnStyle_medium" 
                onclick="btncancel_Click"></asp:Button></TD><td>
            &nbsp;</td><TD><asp:Button id="btnreport" tabIndex=24 onclick="btnreport_Click" runat="server" CausesValidation="False" Text="Report View" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button></TD></TR></TBODY></TABLE><TABLE><TBODY><TR><TD><asp:Panel id="pnlalternate" runat="server" Width="100%" GroupingText="Alternate Room"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="Label9" runat="server" Width="82px" Text="New building"></asp:Label></TD><TD style="WIDTH: 99px"><asp:DropDownList id="cmbaltbulilding" tabIndex=28 runat="server" Width="150px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbaltbulilding_SelectedIndexChanged" DataTextField="buildingname" DataValueField="build_id"></asp:DropDownList></TD><TD></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 3px"><asp:Label id="Label13" runat="server" Text="New room"></asp:Label></TD><TD style="WIDTH: 99px; HEIGHT: 3px"><asp:DropDownList id="cmbaltroom" tabIndex=28 runat="server" Width="150px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbaltroom_SelectedIndexChanged" DataTextField="roomno" DataValueField="room_id"></asp:DropDownList></TD><TD></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 3px"><asp:Label id="lblreason" runat="server" Text="Reason"></asp:Label></TD><TD style="WIDTH: 99px; HEIGHT: 3px"><asp:DropDownList id="cmbReason" runat="server" Width="150px" DataTextField="reason" DataValueField="reason_id"></asp:DropDownList></TD><TD></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 99px"></TD><TD><asp:Button id="btnchangeroom" tabIndex=29 onclick="btnchangeroom_Click" runat="server" CausesValidation="False" Text="Change room" Font-Bold="True"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlletter" runat="server" Width="100%" GroupingText="CEO Letter" Visible="False" __designer:wfdid="w3"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblletterbuilding" runat="server" Text="Building name" __designer:wfdid="w8"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbletterbuilding" runat="server" Width="150px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbletterbuilding_SelectedIndexChanged" DataTextField="buildingname" DataValueField="build_id" __designer:wfdid="w10"></asp:DropDownList></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblletterroom" runat="server" Text="Room no" __designer:wfdid="w9"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbletterroom" runat="server" Width="150px" Height="22px" DataTextField="roomno" DataValueField="room_id" __designer:wfdid="w11"></asp:DropDownList></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:Button id="btnletterdetails" runat="server" CausesValidation="False" Text="Show details" Font-Bold="True" __designer:wfdid="w13" OnClick="btnletterdetails_Click"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel>&nbsp;&nbsp; </TD>
    <TD valign="top"> <asp:Panel id="userpanel" runat="server" Width="100%" GroupingText="User Allocation Panel" BackColor="#C0C0FF"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:Label id="Label15" runat="server" Width="66px" Text="User name"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:TextBox id="txtuname" tabIndex=33 runat="server"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:Label id="Label16" runat="server" Text="Password"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:TextBox id="txtupass" tabIndex=34 runat="server" TextMode="Password"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 26px"></TD><TD style="WIDTH: 100px; HEIGHT: 26px">
        <asp:Button id="btnsubmit" tabIndex=35 onclick="btnsubmit_Click" runat="server" 
            Width="100px" CausesValidation="False" Font-Bold="True" Text="SUBMIT" 
            Height="26px"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> </TD></TR></TBODY></TABLE></TD></TR><TR><TD align=center colSpan=3>
            <asp:GridView id="gvclub" runat="server" Width="840px" ForeColor="#333333" 
                CellPadding="4" GridLines="None" __designer:wfdid="w14">
                <EmptyDataRowStyle HorizontalAlign="Center" />
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<RowStyle BackColor="#EFF3FB" HorizontalAlign="Center"></RowStyle>
<EditRowStyle BackColor="#2461BF"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True" 
                    HorizontalAlign="Center"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>
<AlternatingRowStyle BackColor="White" HorizontalAlign="Center"></AlternatingRowStyle>
</asp:GridView>
            <asp:GridView id="gdroomallocation" runat="server" Width="840px" ForeColor="#333333" CellPadding="4" GridLines="None" OnSelectedIndexChanged="gdroomallocation_SelectedIndexChanged" OnSorting="gdroomallocation_Sorting" OnRowCreated="gdroomallocation_RowCreated" OnPageIndexChanging="gdroomallocation_PageIndexChanging" Caption="gridview" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="id" PageSize="5">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="id" Visible="False" HeaderText="id"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="Room No" HeaderText="Room No"></asp:BoundField>
<asp:BoundField DataField="Inmates" HeaderText="Inmates"></asp:BoundField>
<asp:BoundField DataField="Area" HeaderText="Area"></asp:BoundField>
<asp:BoundField DataField="Rent" HeaderText="Rent"></asp:BoundField>
</Columns>
<RowStyle BackColor="#EFF3FB" Width="50px" BorderColor="Black" HorizontalAlign="Left"></RowStyle>
<EditRowStyle BackColor="#2461BF" BorderColor="Black"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True" VerticalAlign="Middle"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> <asp:GridView id="gdDonor" runat="server" Width="840px" ForeColor="#333333" CellPadding="4" GridLines="None" OnSelectedIndexChanged="gdDonor_SelectedIndexChanged" OnRowCreated="gdDonor_RowCreated" OnPageIndexChanging="gdDonor_PageIndexChanging" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="id" PageSize="5">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="id" Visible="False" HeaderText="id"></asp:BoundField>
<asp:BoundField DataField="Pass No" HeaderText="Pass No"></asp:BoundField>
<asp:BoundField DataField="PassType" HeaderText="PassType"></asp:BoundField>
<asp:BoundField DataField="Donor Name" HeaderText="Donor Name"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="Room" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="ResStatus" HeaderText="ResStatus"></asp:BoundField>
</Columns>
<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left"></RowStyle>
<EditRowStyle BackColor="#2461BF"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True" VerticalAlign="Top"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView><asp:GridView id="gdalloc" runat="server" Width="840px" ForeColor="#333333" CellPadding="4" GridLines="None" OnSelectedIndexChanged="gdalloc_SelectedIndexChanged" OnRowCreated="gdalloc_RowCreated" OnPageIndexChanging="gdalloc_PageIndexChanging" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="id">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="id" Visible="False" HeaderText="id"></asp:BoundField>
<asp:BoundField DataField="No" HeaderText="No"></asp:BoundField>
<asp:BoundField DataField="Reciept" HeaderText="Reciept"></asp:BoundField>
<asp:BoundField DataField="Swami Name" HeaderText="Swami Name"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="Room" HeaderText="Room"></asp:BoundField>
<asp:BoundField DataField="Alloc Date" HeaderText="Alloc Date"></asp:BoundField>
<asp:BoundField DataField="Vecate Date" HeaderText="Vecate Date"></asp:BoundField>
<asp:BoundField DataField="Rent" HeaderText="Rent"></asp:BoundField>
<asp:BoundField DataField="Deposit" HeaderText="Deposit"></asp:BoundField>
<asp:BoundField DataField="Amt" HeaderText="Amt"></asp:BoundField>
</Columns>
<RowStyle BackColor="#EFF3FB" HorizontalAlign="Left"></RowStyle>
<EditRowStyle BackColor="#2461BF"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> <asp:GridView id="gdletter" runat="server" Width="840px" ForeColor="#333333" CellPadding="4" GridLines="None" __designer:wfdid="w14">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<RowStyle BackColor="#EFF3FB"></RowStyle>
<EditRowStyle BackColor="#2461BF"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView>&nbsp;</TD></TR><TR><TD style="HEIGHT: 744px" vAlign=top align=center colSpan=3>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Panel id="Panel1" runat="server" Width="100%" Height="50px"><TABLE width="100%"><TBODY><TR><TD style="WIDTH: 111px"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="86px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Name required" ControlToValidate="txtswaminame"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator1">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" Width="84px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Only alphabet" ControlToValidate="txtswaminame" ValidationExpression="[a-z A-Z . ]{1,25}"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RegularExpressionValidator3">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 41px"><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Width="141px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Building name required" ControlToValidate="cmbBuild"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px; HEIGHT: 41px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator2">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Width="114px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Room no required" ControlToValidate="cmbRooms"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator3">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RegularExpressionValidator id="RegularExpressionValidator4" runat="server" Width="125px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Only Numbers(1-10)" ControlToValidate="txtphone" ValidationExpression="[0-9]{1,10}"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" TargetControlID="RegularExpressionValidator4">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px"><asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" ForeColor="White" SetFocusOnError="True" ErrorMessage="No days required" ControlToValidate="txtnoofdays"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredFieldValidator5">
                                </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px">&nbsp;<asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" ForeColor="White" ErrorMessage="No of inmates required" ControlToValidate="txtnoofinmates"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 111px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" TargetControlID="RequiredFieldValidator6"></cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 18px"><asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ForeColor="White" SetFocusOnError="True" ErrorMessage="DD/MM/YYYY" ControlToValidate="txtcheckindate" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator> </TD><TD style="WIDTH: 111px; HEIGHT: 18px"><cc1:ListSearchExtender id="ListSearchExtender1" runat="server" TargetControlID="cmbState" IsSorted="True"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender2" runat="server" TargetControlID="cmbDists"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender3" runat="server" TargetControlID="cmbBuild"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender4" runat="server" TargetControlID="cmbRooms"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender5" runat="server" TargetControlID="cmbaltbulilding"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender6" runat="server" TargetControlID="cmbaltroom"></cc1:ListSearchExtender></TD></TR><TR><TD style="WIDTH: 111px"><asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" Width="82px" ForeColor="White" SetFocusOnError="True" ErrorMessage="DD/MM/YYYY" ControlToValidate="txtcheckout" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 111px"></TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 17px"><asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" Width="107px" ForeColor="White" SetFocusOnError="True" ErrorMessage="Enter no of days" ControlToValidate="txtnoofdays" Display="Dynamic"></asp:RequiredFieldValidator> </TD><TD style="WIDTH: 111px; HEIGHT: 17px">&nbsp;</TD></TR>
        <tr>
            <td colspan="2" style="HEIGHT: 17px">
                <asp:UpdateProgress ID="UpdateProgress" runat="server">
                    <progresstemplate>
                        <asp:Image ID="Image1" runat="server" AlternateText="Processing" 
                            ImageUrl="Images/waiting.gif" />
                    </progresstemplate>
                </asp:UpdateProgress>
                <cc1:ModalPopupExtender ID="modalPopup" runat="server" 
                    BackgroundCssClass="modalBackground" PopupControlID="UpdateProgress" 
                    TargetControlID="UpdateProgress" />
            </td>
        </tr>
        <TR><TD style="WIDTH: 111px; HEIGHT: 17px">
                                                                <asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel8" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="Label22" runat="server" Width="238px" ForeColor="MediumBlue" Text="Tsunami ARMS - Confirmation" Font-Bold="True"></asp:Label></asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize"></ajaxToolkit:ModalPopupExtender>&nbsp; <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button>&nbsp; <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD style="WIDTH: 227px; HEIGHT: 18px" align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 15px"></TD><TD style="WIDTH: 208px; HEIGHT: 15px"></TD><TD style="WIDTH: 13px; HEIGHT: 15px"></TD></TR><TR><TD style="HEIGHT: 26px"></TD><TD style="WIDTH: 208px; HEIGHT: 26px" align=center>&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="WIDTH: 13px; HEIGHT: 26px" align=center>&nbsp;</TD></TR><TR><TD style="HEIGHT: 18px"></TD><TD style="WIDTH: 208px; HEIGHT: 18px" align=center></TD><TD style="WIDTH: 13px; HEIGHT: 18px" align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD style="WIDTH: 224px" align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel>
                                
                                  <asp:Panel id="pnlAbnormal" runat="server" Width="100%" BackColor="#C0C0FF"
            GroupingText="Abnormal history"  __designer:wfdid="w3"><TABLE><TBODY><TR><TD style="WIDTH: 100px">
                <asp:Label id="Label20" runat="server" Text="Inmates name" 
                    __designer:wfdid="w8"></asp:Label></TD><TD style="WIDTH: 100px">
                    <asp:TextBox ID="txtAbnormal" runat="server" Width="160px"></asp:TextBox>
                </TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px">
                <asp:Label ID="Label107" runat="server" Text="Abnormal Type"></asp:Label>
                </TD><TD style="WIDTH: 100px">
                    <asp:DropDownList ID="ddlAbnormal" runat="server" DataTextField="abnormal_type" 
                        DataValueField="id" Width="162px">
                    </asp:DropDownList>
                </TD><TD style="WIDTH: 100px">&nbsp;</TD></TR><TR><TD style="WIDTH: 100px">
                <asp:Label ID="Label21" runat="server" __designer:wfdid="w9" Text="Remarks"></asp:Label>
                </TD><TD style="WIDTH: 100px">
                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="162px"></asp:TextBox>
                </TD><TD style="WIDTH: 100px">
                </TD></TR>
                <tr>
                   
                    <td style="WIDTH: 100px" colspan="3" align="center">
                       
                        <asp:Button ID="btnAb" runat="server"  OnClick="btnAb_Click" CausesValidation="False" Text="Save Abnormality" CssClass="btnStyle" />
                    </td>
                </tr>
                </TBODY></TABLE></asp:Panel>



                                
                                
                                 </asp:Panel>
                                </TD><TD style="WIDTH: 111px; HEIGHT: 17px">
        <asp:TextBox ID="txtreson" runat="server" Height="17px" 
            OnTextChanged="TextBox2_TextChanged" tabIndex="19" Visible="False" 
            Width="100px"></asp:TextBox>
        <asp:Label ID="Label14" runat="server" Text="Reason" Visible="False"></asp:Label>
        <asp:Button ID="btnreallocate" runat="server" CausesValidation="False" 
            CssClass="btnStyle_medium" Font-Bold="True" onclick="btnreallocate_Click" 
            tabIndex="22" Visible="False" />
        </TD></TR></TBODY></TABLE></asp:Panel>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<BR />&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE><IFRAME style="WIDTH: 200px; HEIGHT: 200px" id="frame1" runat="server" visible="true"></IFRAME>
</contenttemplate>
 <Triggers>
  <asp:PostBackTrigger ControlID="btnOk" />
   </Triggers>
  </asp:UpdatePanel>    
    <br />
    <br />
 <script type="text/javascript">     function ClearLastMessage(elem) {
         $get(elem).innerHTML = '';
     } 
</script>
    <br />
    <br />
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
        prm.add_beginRequest(BeginRequestHandler);
        // Raised after an asynchronous postback is finished and control has been returned to the browser.
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            //Shows the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();

            }
        }

        function EndRequestHandler(sender, args) {
            //Hide the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
        function openNewWindows1() {

            window.open("onlinehelp.aspx");
        }
        function openNewWindows2() {

            window.open("important steps.aspx");
        }
        $(function () {
            $(document).tooltip({
                track: true
            });
        });
  

</script>  
</asp:Content>
