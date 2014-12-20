<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="tdb room allocation.aspx.cs" Inherits="tdb_room_allocation" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
 <p>
        <br />
        This form is used for room allocation for General Public.<br />
        <br />
        </asp:Panel>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<table><tbody><tr align=center><td colSpan=3>
    <asp:Panel ID="pnlcash" runat="server" Enabled="False" 
        GroupingText="Cashier liability">
        <table>
            <tbody>
                <tr>
                    <td style="width: 50px">
                        <asp:Label ID="lblstaff" runat="server" Text="Staff Name:" Width="69px"></asp:Label>
                    </td>
                    <td style="width: 50px">
                        <asp:TextBox ID="txtstaffname" runat="server" ReadOnly="true" Width="90px"></asp:TextBox>
                    </td>
                    <td style="width: 50px">
                        <asp:Label ID="Label4" runat="server" Text="Cashier Liability" Width="93px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtcashierliability" runat="server" Font-Bold="True" 
                            Font-Size="Small" tabIndex="53" Width="66px"></asp:TextBox>
                    </td>
                    <td align="left">
                        <asp:Label ID="Label3" runat="server" Text="Receipt no:" Width="69px"></asp:Label>
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:TextBox ID="txtreceiptno1" runat="server" 
                            OnTextChanged="txtreceiptno1_TextChanged" tabIndex="51" Width="66px"></asp:TextBox>
                 <TD style="WIDTH: 100px"><asp:Label id="Label1" runat="server" Width="122px" Text="No of Transactions"></asp:Label></TD><TD style="WIDTH: 100px"><asp:TextBox id="txtnooftrans" tabIndex=55 runat="server" Width="66px"></asp:TextBox></TD>
                 <td style="WIDTH: 100px">
                        <asp:Label ID="Label17" runat="server" Text="Uncliamed " Width="103px"></asp:Label>
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:TextBox ID="txtunclaimed" runat="server" Font-Bold="True" 
                            Font-Size="Small" tabIndex="54" Width="66px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="WIDTH: 50px">
                        <asp:Label ID="Label2" runat="server" Text="Login Time:" Width="69px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtlogintime" runat="server" ReadOnly="true" Width="90px"></asp:TextBox>
                    </td>
                     </td><td align="left">
          Today's Liability</td>
        <td>
            <asp:TextBox ID="txtcounterliability" runat="server" 
                Width="66px"></asp:TextBox>
        </td>
              <td style="WIDTH: 100px">
                        <asp:Label ID="Label10" runat="server" Text="Balance Receipt" Width="97px"></asp:Label>
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:TextBox ID="txtreceiptno2" runat="server" 
                            OnTextChanged="txtreceiptno2_TextChanged" tabIndex="52" Width="66px"></asp:TextBox>
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:Label ID="Label18" runat="server" Text="Security Deposit" Width="103px"></asp:Label>
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:TextBox ID="txttotsecurity" runat="server" Font-Bold="True" 
                            Font-Size="Small" tabIndex="54" Width="66px"></asp:TextBox>
                    </td>
              
                    <td style="WIDTH: 100px">
                        <asp:Label ID="Label5" runat="server" Text="Counter Deposit" Width="103px"></asp:Label>
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:TextBox ID="txtcounterdeposit" runat="server" Font-Bold="True" 
                            Font-Size="Small" tabIndex="54" Width="66px"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
    </td>
    <tr align="center">
        <td colspan="3">
            &nbsp;&nbsp;<asp:Label ID="lblhead0" runat="server" CssClass="heading" Font-Bold="True" 
                Font-Size="Medium" Text="TDB  ROOM ALLOCATION"></asp:Label>
            &nbsp;&nbsp;<asp:Label ID="lblhead" runat="server" CssClass="heading" Font-Bold="True" 
                Font-Size="Medium" Text="TDB  ROOM ALLOCATION" Visible="False"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox 
                ID="chkplainpaper" runat="server" AutoPostBack="True" 
                OnCheckedChanged="chkplainpaper_CheckedChanged" 
                Text="Old receipt" Width="153px" />
            &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:GridView ID="donorgrid" runat="server" CellPadding="4" ForeColor="#333333" 
                GridLines="None" OnSelectedIndexChanged="donorgrid_SelectedIndexChanged" 
                Width="840px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
                </Columns>
                <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                    HorizontalAlign="Left" VerticalAlign="Top" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            &nbsp;
        </td>
    </tr>  
    <tr align="center">
         <td style="width: 372px; height: 29px;" align="center">
            <asp:Label ID="Label102" runat="server" Text="Reserve No."></asp:Label>
             <asp:TextBox ID="txtReserveNo" runat="server" AutoPostBack="True" Height="16px" 
                 ontextchanged="txtReserveNo_TextChanged" Width="149px"></asp:TextBox>
        </td>
       
        <td align="center" style="height: 29px" valign="middle" >
            <asp:Label ID="lblreceipt" runat="server" Text="Receipt No" Visible="False"></asp:Label>
            <asp:TextBox ID="txtreceipt" runat="server" AutoPostBack="True" 
                OnTextChanged="txtreceipt_TextChanged" Visible="False" Width="103px"></asp:TextBox>
        </td>
        <td style="height: 29px" >
        </td>
    </tr>

    <tr>
        <td style="width: 372px" >
            <asp:Panel ID="swamipanel" runat="server" GroupingText="Swami Details" 
                Height="1%">
                <table>
                    <tbody>
                        <tr>
                            <td style="WIDTH: 162px; HEIGHT: 2px">
                                <asp:Label ID="lblswaminame" runat="server" Font-Bold="False" Text="Swami name" 
                                    Width="78px"></asp:Label>
                            </td>
                            <td style="WIDTH: 284px; HEIGHT: 2px">
                                <asp:TextBox ID="txtswaminame" runat="server" AutoPostBack="True" 
                                    CssClass="UpperCaseFirstLetter" Height="17px" 
                                    OnTextChanged="txtswaminame_TextChanged1" tabIndex="5" Width="200px"></asp:TextBox>
                            </td>
                            <td style="height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 162px; HEIGHT: 23px">
                                <asp:Label ID="Label12" runat="server" Text="Place" Width="82px"></asp:Label>
                            </td>
                            <td style="WIDTH: 284px; HEIGHT: 23px">
                                <asp:TextBox ID="txtplace" runat="server" AutoPostBack="True" 
                                    CssClass="UpperCaseFirstLetter" Height="17px" 
                                    OnTextChanged="txtplace_TextChanged" tabIndex="6" Width="200px"></asp:TextBox>
                            </td>
                            <td>
                                <BR />
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 162px; HEIGHT: 14px">
                                <asp:Label ID="lblstate" runat="server" Text="State" Width="79px"></asp:Label>
                            </td>
                            <td style="WIDTH: 284px; HEIGHT: 14px">
                                <asp:DropDownList ID="cmbState" runat="server" AppendDataBoundItems="True" 
                                    AutoPostBack="True" DataTextField="statename" DataValueField="state_id" 
                                    Height="22px" OnSelectedIndexChanged="cmbState_SelectedIndexChanged" 
                                    tabIndex="20" Width="205px">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <BR />
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 162px; HEIGHT: 26px">
                                <asp:Label ID="Label11" runat="server" Text="District" Width="80px"></asp:Label>
                                <BR />
                            </td>
                            <td style="WIDTH: 284px; HEIGHT: 26px">
                                <asp:DropDownList ID="cmbDists" runat="server" DataTextField="districtname" 
                                    DataValueField="district_id" Height="22px" 
                                    OnSelectedIndexChanged="cmbDists_SelectedIndexChanged" tabIndex="20" 
                                    Width="205px">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="HEIGHT: 26px">
                                <asp:LinkButton ID="lnkdistrict" runat="server" CausesValidation="False" 
                                    onclick="lnkdistrict_Click">New</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 162px">
                                <asp:Label ID="lblphone" runat="server" Text="Phone" Width="79px"></asp:Label>
                            </td>
                            <td style="WIDTH: 284px">
                                <asp:TextBox ID="txtphone" runat="server" Height="17px" 
                                    OnTextChanged="txtphone_TextChanged" tabIndex="20" Width="200px"></asp:TextBox>
                            </td>
                            <td>
                                <BR />
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 162px; HEIGHT: 31px">
                                <asp:Label ID="lblidproof" runat="server" Text="Identity proof" Width="79px"></asp:Label>
                            </td>
                            <td style="WIDTH: 284px; HEIGHT: 31px">
                                <asp:DropDownList ID="cmbIDp" runat="server" Height="22px" tabIndex="20" 
                                    Width="205px" 
                                    onselectedindexchanged="cmbIDp_SelectedIndexChanged" 
                                    DataTextField="idproof" DataValueField="pid">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <BR />
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 162px; HEIGHT: 20px">
                                <asp:Label ID="Label8" runat="server" Text="Identity ref: no" Width="83px"></asp:Label>
                            </td>
                            <td style="WIDTH: 284px; HEIGHT: 20px">
                                <asp:TextBox ID="txtidrefno" runat="server" EnableTheming="True" Height="17px" 
                                    OnTextChanged="txtidrefno_TextChanged" tabIndex="20" Width="200px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 162px; HEIGHT: 9px">
                            </td>
                            <td style="WIDTH: 284px; HEIGHT: 9px">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </td>
        <td>
            <asp:Panel ID="roomallocationpanel" runat="server" GroupingText="Room Allocation Details" Height="1%">
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <asp:Label ID="lblnoofinmates" runat="server" Text="No: of Inmates" 
                                    Width="85px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtnoofinmates" runat="server" AutoPostBack="True" 
                                    Height="17px" OnTextChanged="TextBox5_TextChanged" tabIndex="12" Width="170px"></asp:TextBox>
                            </td>
                            <td>
                                <BR />
                            </td>
                        </tr>                                               
                        <tr>
                            <td>
                                <asp:Label ID="lblbuildingname" runat="server" 
                                    Text="Building name" Width="87px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbBuild" runat="server" 
                                    AutoPostBack="True" DataTextField="buildingname" 
                                    DataValueField="build_id" Height="22px" 
                                    OnSelectedIndexChanged="cmbBuild_SelectedIndexChanged" 
                                    tabIndex="13" Width="175px">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbroomno" runat="server" Text="Room no" 
                                    Width="74px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbRooms" runat="server" 
                                    AutoPostBack="True" DataTextField="roomno" 
                                    DataValueField="room_id" 
                                    OnSelectedIndexChanged="cmbRooms_SelectedIndexChanged" 
                                    tabIndex="14" Width="175px">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblProposedCheckOutDate" runat="server" 
                                    style="POSITION: relative" Text="Check out date" 
                                    Width="92px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtcheckout" runat="server" 
                                    AutoPostBack="True" Height="17px" 
                                    OnTextChanged="txtcheckout_TextChanged" 
                                    style="POSITION: relative; top: 0px; left: 0px;" tabIndex="16" Width="170px" 
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 24px">
                                <asp:Label ID="lblcheckouttime" runat="server" style="POSITION: relative" 
                                    Text="Check out time" Width="90px"></asp:Label>
                            </td>
                            <td style="height: 24px">
                                <asp:TextBox ID="txtcheckouttime" runat="server" AutoPostBack="True" 
                                    Height="17px" OnTextChanged="txtcheckouttime_TextChanged" 
                                    style="LEFT: 0px; POSITION: relative; top: 0px;" 
                                    tabIndex="17" Width="170px" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td style="height: 24px">
                                &nbsp;</td>
                                </tr>
                                   <tr>
                            <td>
                                <asp:Label ID="Label100" runat="server" 
                                    style="LEFT: 0px; POSITION: relative; TOP: 3px" Text="No of days" 
                                    Width="95px" Visible="False"></asp:Label></td>
                            <td><asp:TextBox ID="txtnoofdays" runat="server" Height="17px" 
                                    OnTextChanged="txtnoofdays_TextChanged" 
                                    style="LEFT: 0px; POSITION: relative; TOP: 0px" tabIndex="15" 
                                    Width="170px" ReadOnly="True" Visible="False"></asp:TextBox>
                                 </td>
                            <td>
                                </td>
                        </tr>
                        </tr>
                         <tr>
                             <td>
                                 <asp:Label ID="Label101" runat="server" Text="No of Hours"></asp:Label>
                             </td>
                             <td>
                                 <asp:TextBox ID="txthours" runat="server" ReadOnly="True" 
                                     Width="170px" ontextchanged="txthours_TextChanged"></asp:TextBox>
                             </td>
                             <td>
                                 <asp:Label ID="lblmin" runat="server"></asp:Label>
                             </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:Label ID="lblcheckindate" runat="server" 
                                    style="LEFT: 2px; POSITION: relative; TOP: 0px" Text="Check in date" 
                                    Width="85px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtcheckindate" runat="server" AutoPostBack="True" 
                                    Enabled="False" Height="17px" OnTextChanged="txtcheckindate_TextChanged" 
                                    style="LEFT: 0px; POSITION: relative" tabIndex="57" Width="170px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                              <asp:Label ID="lblcheckintime" runat="server" style="POSITION: relative" 
                                    Text="Check in time" Width="85px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtcheckintime" runat="server" AutoPostBack="True" 
                                    Enabled="False" Height="17px" OnTextChanged="txtcheckintime_TextChanged" 
                                    style="POSITION: relative" tabIndex="56" Width="170px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>                     
                    </tbody>
                </table>
            </asp:Panel>
        </td>
        <td>
            <asp:Panel ID="rentpanel" runat="server" GroupingText="Rent" Height="1%">
                <table>
                    <tbody>
                        <tr>
                            <td style="WIDTH: 100px; HEIGHT: 26px">
                                <asp:Label ID="lblroomrent" runat="server" Text="Room rent" Width="73px"></asp:Label>
                            </td>
                            <td style="WIDTH: 92px; HEIGHT: 26px">
                                <asp:TextBox ID="txtroomrent" runat="server" Enabled="False" Font-Bold="True" 
                                    Height="17px" OnTextChanged="txtroomrent_TextChanged" tabIndex="16" 
                                    Width="90px"></asp:TextBox>
                            </td>
                            <td style="WIDTH: 92px; HEIGHT: 26px">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 94px">
                                <asp:Label ID="lblsecuritydeposit" runat="server" Text="Security deposit" 
                                    Width="97px"></asp:Label>
                            </td>
                            <td style="WIDTH: 92px">
                                <asp:TextBox ID="txtsecuritydeposit" runat="server" Enabled="False" 
                                    Font-Bold="True" Height="17px" tabIndex="17" Width="90px"></asp:TextBox>
                            </td>
                            <td style="WIDTH: 92px">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 100px; HEIGHT: 26px">
                                <asp:Label ID="Label7" runat="server" Text="Other charge" Width="77px"></asp:Label>
                            </td>
                            <td style="WIDTH: 92px; HEIGHT: 26px">
                                <asp:TextBox ID="txtothercharge" runat="server" AutoPostBack="True" 
                                    Font-Bold="True" Height="17px" OnTextChanged="txtothercharge_TextChanged" 
                                    tabIndex="19" Width="90px"></asp:TextBox>
                            </td>
                            <td style="WIDTH: 92px; HEIGHT: 26px">
                                <br />
                            </td>
                        </tr>
                                  <tr>
                            <td style="WIDTH: 100px; HEIGHT: 18px">
                                <asp:Label ID="lbltotalamount" runat="server" Text="Total amount" Width="80px"></asp:Label>
                            </td>
                            <td style="WIDTH: 92px; HEIGHT: 18px">
                                <asp:TextBox ID="txttotalamount" runat="server" Enabled="False" 
                                    Font-Bold="True" Font-Size="X-Large" ForeColor="OliveDrab" Height="33px" 
                                    tabIndex="21" Width="90px" Wrap="False"></asp:TextBox>
                            </td>
                            <td style="WIDTH: 92px; HEIGHT: 18px">
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 100px; HEIGHT: 18px">
                                Amount Received</td>
                            <td style="WIDTH: 92px; HEIGHT: 18px">
                                <asp:TextBox ID="txtadvance" runat="server" Height="17px" 
                                    OnTextChanged="txtadvance_TextChanged" tabIndex="20" Width="90px"></asp:TextBox>
                            </td>
                            <td style="WIDTH: 92px; HEIGHT: 18px">
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 100px; HEIGHT: 18px">
                             <b>Balance payable</b> </td>
                            <td style="WIDTH: 92px; HEIGHT: 18px">
                                <asp:TextBox ID="txtnetpayable" runat="server" Width="90px" 
                                    Enabled="False" Font-Size="X-Large" ForeColor="OliveDrab" 
                                    Height="33px" Wrap="False"></asp:TextBox>
                            </td>
                            <td style="WIDTH: 92px; HEIGHT: 18px">
                                &nbsp;</td>
                        </tr>
              
                        <tr>
                            <td style="WIDTH: 100px; HEIGHT: 17px">
                                <asp:Label ID="Label6" runat="server" Text="Grant total" Visible="False"></asp:Label>
                            </td>
                            <td style="WIDTH: 92px; HEIGHT: 17px">
                                <asp:TextBox ID="txtgranttotal" runat="server" Enabled="False" Font-Bold="True" 
                                    Font-Size="X-Large" ForeColor="OliveDrab" Height="33px" tabIndex="22" 
                                    Visible="False" Width="90px"></asp:TextBox>
                            </td>
                            <td style="WIDTH: 92px; HEIGHT: 17px">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="3">
            <table>
                <tbody>
                    <tr>
                        <td>
                            <asp:Button ID="btnallocate" runat="server" CssClass="btnStyle_medium" 
                                Font-Bold="True" onclick="btnallocate_Click" tabIndex="18" Text="Allocate" 
                                CausesValidation="False" />
                        </td>
                         <td>
                            <asp:Button ID="btneditcash" runat="server" CausesValidation="False" 
                                CssClass="btnStyle_small" Font-Bold="True" onclick="btneditcash_Click" 
                                tabIndex="26" Text="Edit" />
                        </td>
                        <td>
                            <asp:Button ID="btnclear" runat="server" CausesValidation="False" 
                                CssClass="btnStyle_small" Font-Bold="True" onclick="btnclear_Click" 
                                tabIndex="19" Text="Clear" />
                        </td>
                        <td>
                        <asp:Button id="btncancel" tabIndex=23 onclick="btncancel_Click" runat="server" CausesValidation="False" Font-Bold="True" CssClass="btnStyle_medium"></asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="btnaltroom" runat="server" CausesValidation="False" 
                                CssClass="btnStyle_medium" Font-Bold="True" onclick="btnaltroom_Click" 
                                tabIndex="21" Text="Change room" />
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Button ID="btnreport" runat="server" CausesValidation="False" 
                                CssClass="btnStyle_medium" Font-Bold="True" onclick="btnreport_Click" 
                                tabIndex="24" Text="Report View" />
                        </td>
                       
                        <td>
                            <asp:Button ID="btnreallocate" runat="server" 
                                CausesValidation="False" CssClass="btnStyle_medium" 
                                Font-Bold="True" onclick="btnreallocate_Click" 
                                tabIndex="22" />
                        </td>
                       
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlalternate" runat="server" GroupingText="Alternate Room" 
                                Width="100%">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td style="WIDTH: 100px">
                                                <asp:Label ID="Label9" runat="server" Text="New building" Width="82px"></asp:Label>
                                            </td>
                                            <td style="WIDTH: 99px">
                                                <asp:DropDownList ID="cmbaltbulilding" runat="server" AutoPostBack="True" 
                                                    DataTextField="buildingname" DataValueField="build_id" Height="22px" 
                                                    OnSelectedIndexChanged="cmbaltbulilding_SelectedIndexChanged" tabIndex="28" 
                                                    Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="WIDTH: 100px; HEIGHT: 3px">
                                                <asp:Label ID="Label13" runat="server" Text="New room"></asp:Label>
                                            </td>
                                            <td style="WIDTH: 99px; HEIGHT: 3px">
                                                <asp:DropDownList ID="cmbaltroom" runat="server" AutoPostBack="True" 
                                                    DataTextField="roomno" DataValueField="room_id" Height="22px" 
                                                    OnSelectedIndexChanged="cmbaltroom_SelectedIndexChanged" tabIndex="28" 
                                                    Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="WIDTH: 100px; HEIGHT: 3px">
                                                <asp:Label ID="lblreason" runat="server" Text="Reason"></asp:Label>
                                            </td>
                                            <td style="WIDTH: 99px; HEIGHT: 3px">
                                                <asp:DropDownList ID="cmbReason" runat="server" DataTextField="reason" 
                                                    DataValueField="reason_id" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="WIDTH: 100px">
                                            </td>
                                            <td style="WIDTH: 99px">
                                            </td>
                                            <td>
                                                <asp:Button ID="btnchangeroom" runat="server" CausesValidation="False" 
                                                    Font-Bold="True" onclick="btnchangeroom_Click" tabIndex="29" 
                                                    Text="Change room" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                            &nbsp;&nbsp;
                        </td>
                        <td valign="top">
                            <asp:Panel ID="userpanel" runat="server" BackColor="#C0C0FF" 
                                GroupingText="User Allocation Panel" Width="100%">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td style="WIDTH: 100px; HEIGHT: 18px">
                                                <asp:Label ID="Label15" runat="server" Text="User name" Width="66px"></asp:Label>
                                            </td>
                                            <td style="WIDTH: 100px; HEIGHT: 18px">
                                                <asp:TextBox ID="txtuname" runat="server" tabIndex="33"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="WIDTH: 100px; HEIGHT: 26px">
                                                <asp:Label ID="Label16" runat="server" Text="Password"></asp:Label>
                                            </td>
                                            <td style="WIDTH: 100px; HEIGHT: 26px">
                                                <asp:TextBox ID="txtupass" runat="server" tabIndex="34" TextMode="Password"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="WIDTH: 100px; HEIGHT: 26px">
                                            </td>
                                            <td style="WIDTH: 100px; HEIGHT: 26px">
                                                <asp:Button ID="btnsubmit" runat="server" CausesValidation="False" 
                                                    Font-Bold="True" onclick="btnsubmit_Click" tabIndex="35" Text="SUBMIT" 
                                                    Width="100px" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="3">
            <asp:GridView ID="gdroomallocation" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" Caption="gridview" 
                CellPadding="4" DataKeyNames="id" ForeColor="#333333" GridLines="None" 
                OnPageIndexChanging="gdroomallocation_PageIndexChanging" 
                OnRowCreated="gdroomallocation_RowCreated" 
                OnSelectedIndexChanged="gdroomallocation_SelectedIndexChanged" 
                OnSorting="gdroomallocation_Sorting" PageSize="5" Width="840px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
                    <asp:BoundField DataField="id" HeaderText="id" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
                    <asp:BoundField DataField="Room No" HeaderText="Room No"></asp:BoundField>
                    <asp:BoundField DataField="Inmates" HeaderText="Inmates"></asp:BoundField>
                    <asp:BoundField DataField="Area" HeaderText="Area"></asp:BoundField>
                    <asp:BoundField DataField="Rent" HeaderText="Rent"></asp:BoundField>
                </Columns>
                <RowStyle BackColor="#EFF3FB" BorderColor="Black" HorizontalAlign="Left" 
                    Width="50px" />
                <EditRowStyle BackColor="#2461BF" BorderColor="Black" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                    HorizontalAlign="Left" VerticalAlign="Middle" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:GridView ID="gdalloc" runat="server" AllowPaging="True" 
                AutoGenerateColumns="False" CellPadding="4" DataKeyNames="id" 
                ForeColor="#333333" GridLines="None" 
                OnPageIndexChanging="gdalloc_PageIndexChanging" 
                OnRowCreated="gdalloc_RowCreated" 
                OnSelectedIndexChanged="gdalloc_SelectedIndexChanged" Width="840px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
                    <asp:BoundField DataField="id" HeaderText="id" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="No" HeaderText="No"></asp:BoundField>
                    <asp:BoundField DataField="Reciept" HeaderText="Reciept"></asp:BoundField>
                    <asp:BoundField DataField="Swami Name" HeaderText="Swami Name"></asp:BoundField>
                    <asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField>
                    <asp:BoundField DataField="Room" HeaderText="Room"></asp:BoundField>
                    <asp:BoundField DataField="Alloc Date" HeaderText="Alloc Date"></asp:BoundField>
                    <asp:BoundField DataField="Vecate Date" HeaderText="Vecate Date">
                    </asp:BoundField>
                    <asp:BoundField DataField="Rent" HeaderText="Rent"></asp:BoundField>
                    <asp:BoundField DataField="Deposit" HeaderText="Deposit"></asp:BoundField>
                    <asp:BoundField DataField="Amt" HeaderText="Amt"></asp:BoundField>
                </Columns>
                <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                    HorizontalAlign="Left" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:GridView ID="gdletter" runat="server" __designer:wfdid="w14" 
                CellPadding="4" ForeColor="#333333" GridLines="None" Width="840px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            &nbsp;</td>
    </tr>
    <tr>
        <td align="center" colspan="3" style="HEIGHT: 744px" valign="top">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Panel ID="Panel1" runat="server" Height="50px" Width="100%">
                <table width="100%">
                    <tbody>
                        <tr>
                            <td style="WIDTH: 111px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="txtswaminame" ErrorMessage="Name required" ForeColor="White" 
                                    SetFocusOnError="True" Width="86px"></asp:RequiredFieldValidator>
                            </td>
                            <td style="WIDTH: 111px">
                                <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" 
                                    TargetControlID="RequiredFieldValidator1">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 111px">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                    ControlToValidate="txtswaminame" ErrorMessage="Only alphabet" ForeColor="White" 
                                    SetFocusOnError="True" ValidationExpression="[a-z A-Z . ]{1,25}" Width="84px"></asp:RegularExpressionValidator>
                            </td>
                            <td style="WIDTH: 111px">
                                <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" 
                                    TargetControlID="RegularExpressionValidator3">
                                </cc1:ValidatorCalloutExtender>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" 
                                    TargetControlID="txtcheckindate">
                                </cc1:CalendarExtender>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" 
                                    TargetControlID="txtcheckout">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 111px; HEIGHT: 41px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ControlToValidate="cmbBuild" ErrorMessage="Building name required" 
                                    ForeColor="White" SetFocusOnError="True" Width="141px"></asp:RequiredFieldValidator>
                            </td>
                            <td style="WIDTH: 111px; HEIGHT: 41px">
                                <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" 
                                    TargetControlID="RequiredFieldValidator2">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 111px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                    ControlToValidate="cmbRooms" ErrorMessage="Room no required" ForeColor="White" 
                                    SetFocusOnError="True" Width="114px"></asp:RequiredFieldValidator>
                            </td>
                            <td style="WIDTH: 111px">
                                <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" 
                                    TargetControlID="RequiredFieldValidator3">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 111px">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                    ControlToValidate="txtphone" ErrorMessage="Only Numbers(1-10)" 
                                    ForeColor="White" SetFocusOnError="True" ValidationExpression="[0-9]{1,10}" 
                                    Width="125px"></asp:RegularExpressionValidator>
                            </td>
                            <td style="WIDTH: 111px">
                                <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" 
                                    TargetControlID="RegularExpressionValidator4">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 111px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                    ControlToValidate="txtnoofdays" ErrorMessage="No days required" 
                                    ForeColor="White" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                            <td style="WIDTH: 111px">
                                <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" 
                                    TargetControlID="RequiredFieldValidator5">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 111px">
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                    ControlToValidate="txtnoofinmates" ErrorMessage="No of inmates required" 
                                    ForeColor="White"></asp:RequiredFieldValidator>
                            </td>
                            <td style="WIDTH: 111px">
                                <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" 
                                    TargetControlID="RequiredFieldValidator6">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 111px; HEIGHT: 18px">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ControlToValidate="txtcheckindate" ErrorMessage="DD/MM/YYYY" ForeColor="White" 
                                    SetFocusOnError="True" 
                                    ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator>
                            </td>
                            <td style="WIDTH: 111px; HEIGHT: 18px">
                                <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" IsSorted="True" 
                                    TargetControlID="cmbState">
                                </cc1:ListSearchExtender>
                                <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" 
                                    TargetControlID="cmbDists">
                                </cc1:ListSearchExtender>
                                <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" 
                                    TargetControlID="cmbBuild">
                                </cc1:ListSearchExtender>
                                <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" 
                                    TargetControlID="cmbRooms">
                                </cc1:ListSearchExtender>
                                <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" 
                                    TargetControlID="cmbaltbulilding">
                                </cc1:ListSearchExtender>
                                <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" 
                                    TargetControlID="cmbaltroom">
                                </cc1:ListSearchExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 111px">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                    ControlToValidate="txtcheckout" ErrorMessage="DD/MM/YYYY" ForeColor="White" 
                                    SetFocusOnError="True" 
                                    ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$" 
                                    Width="82px"></asp:RegularExpressionValidator>
                            </td>
                            <td style="WIDTH: 111px">
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 111px; HEIGHT: 17px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ControlToValidate="txtnoofdays" Display="Dynamic" 
                                    ErrorMessage="Enter no of days" ForeColor="White" SetFocusOnError="True" 
                                    Width="107px"></asp:RequiredFieldValidator>
                            </td>
                            <td style="WIDTH: 111px; HEIGHT: 17px">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 111px; HEIGHT: 17px">
                                <asp:Panel ID="pnlMessage" runat="server" _="" _designer:dtid="562958543355909" 
                                    CssClass="ModalWindow">
                                    <asp:Panel ID="Panel8" runat="server" BackColor="LightSteelBlue" 
                                        BorderStyle="Outset">
                                        <asp:Label ID="Label22" runat="server" Font-Bold="True" ForeColor="MediumBlue" 
                                            Text="Tsunami ARMS - Confirmation" Width="238px"></asp:Label>
                                    </asp:Panel>
                                    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                                        PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize" 
                                        TargetControlID="btnHidden">
                                    </cc1:ModalPopupExtender>
                                    &nbsp;
                                    <asp:Button ID="btnHidden" runat="server" style="DISPLAY: none" Text="Hidden" />
                                    &nbsp;
                                    <br />
                                    <asp:Panel ID="pnlYesNo" runat="server" Height="50px" Width="125px">
                                        <table style="WIDTH: 237px">
                                            <tbody>
                                                <tr>
                                                    <td align="center" colspan="1" style="HEIGHT: 18px">
                                                    </td>
                                                    <td align="center" colspan="3" style="WIDTH: 227px; HEIGHT: 18px">
                                                        <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Black" 
                                                            Text="Do you want to save?"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="HEIGHT: 15px">
                                                    </td>
                                                    <td style="WIDTH: 208px; HEIGHT: 15px">
                                                    </td>
                                                    <td style="WIDTH: 13px; HEIGHT: 15px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="HEIGHT: 26px">
                                                    </td>
                                                    <td align="center" style="WIDTH: 208px; HEIGHT: 26px">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btnYes" runat="server" CausesValidation="False" 
                                                            CssClass="btnStyle" onclick="btnYes_Click" Text="Yes" Width="50px" />
                                                        <asp:Button ID="btnNo" runat="server" CausesValidation="False" 
                                                            CssClass="btnStyle" onclick="btnNo_Click" Text="No" Width="50px" />
                                                        &nbsp;</td>
                                                    <td align="center" style="WIDTH: 13px; HEIGHT: 26px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="HEIGHT: 18px">
                                                    </td>
                                                    <td align="center" style="WIDTH: 208px; HEIGHT: 18px">
                                                    </td>
                                                    <td align="center" style="WIDTH: 13px; HEIGHT: 18px">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlOk" runat="server" Height="50px" Width="125px">
                                        <table style="WIDTH: 237px">
                                            <tbody>
                                                <tr>
                                                    <td align="center" colspan="1">
                                                    </td>
                                                    <td align="center" colspan="3" style="WIDTH: 224px">
                                                        <asp:Label ID="lblOk" runat="server" Font-Size="Small" ForeColor="Black" 
                                                            Text="Do you want to ?"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="HEIGHT: 10px">
                                                    </td>
                                                    <td style="HEIGHT: 15px">
                                                    </td>
                                                    <td style="HEIGHT: 10px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="center">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnOk" runat="server" CausesValidation="False" 
                                                            CssClass="btnStyle" onclick="btnOk_Click" Text="OK" Width="50px" />
                                                        &nbsp;
                                                    </td>
                                                    <td align="center">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="center">
                                                    </td>
                                                    <td align="center">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                                <asp:Label ID="Label14" runat="server" Text="Reason" Visible="False"></asp:Label>
                                <asp:TextBox ID="txtreson" runat="server" Height="17px" 
                                    OnTextChanged="TextBox2_TextChanged" tabIndex="19" Width="100px" 
                                    Wrap="False" Visible="False"></asp:TextBox>
                                <asp:Button ID="btnviewallocation" runat="server" 
                                    CausesValidation="False" CssClass="btnStyle_medium" 
                                    Font-Bold="True" onclick="btncancel_Click" tabIndex="23" 
                                    Text="View Allocation" Visible="False" />
                            </td>
                            <td style="WIDTH: 111px; HEIGHT: 17px">
                                &nbsp;</td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<BR />&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </td>
    </tr>
    </TBODY></TABLE><IFRAME style="WIDTH: 200px; HEIGHT: 200px" id="frame1" runat="server" visible="true"></IFRAME>
</contenttemplate>
 <Triggers>
  <asp:PostBackTrigger ControlID="btnOk" />
  <asp:PostBackTrigger ControlID="btnallocate" />
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
</asp:Content>

