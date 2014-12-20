<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="General Reservation.aspx.cs" Inherits="General_Reservation" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">    
        <contenttemplate>
            <table style="width: 100%">
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="pnlcash" runat="server" Enabled="False" 
                            GroupingText="Cashier liability">
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="width: 50px; height: 26px;">
                                            <asp:Label ID="lblstaff" runat="server" Text="Staff Name:" 
                                                Width="69px"></asp:Label>
                                        </td>
                                        <td style="width: 50px; height: 26px;">
                                            <asp:TextBox ID="txtstaffname" runat="server" 
                                                ReadOnly="true" Width="90px"></asp:TextBox>
                                        </td>
                                        <td style="width: 50px; height: 26px;">
                                            <asp:Label ID="Label4" runat="server" 
                                                Text="Cashier Liability" Width="93px"></asp:Label>
                                        </td>
                                        <td style="height: 26px">
                                            <asp:TextBox ID="txtcashierliability" runat="server" 
                                                Font-Bold="True" Font-Size="Small" tabIndex="53" 
                                                Width="66px"></asp:TextBox>
                                        </td>
                                        <td align="left" style="height: 26px">
                                            <asp:Label ID="Label3" runat="server" Text="Receipt no:" 
                                                Width="69px"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 100px; height: 26px;">
                                            <asp:TextBox ID="txtreceiptno1" runat="server" 
                                                OnTextChanged="txtreceiptno1_TextChanged" tabIndex="51" 
                                                Width="66px"></asp:TextBox>
                                            <td style="WIDTH: 100px; height: 26px;">
                                                <asp:Label ID="Label1" runat="server" 
                                                    Text="No of Transactions" Width="122px"></asp:Label>
                                            </td>
                                            <td style="WIDTH: 100px; height: 26px;">
                                                <asp:TextBox ID="txtnooftrans" runat="server" tabIndex="55" 
                                                    Width="66px"></asp:TextBox>
                                            </td>
                                        </td>
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
                                            <asp:Label ID="Label2" runat="server" Text="Login Time:" 
                                                Width="69px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtlogintime" runat="server" 
                                                ReadOnly="true" Width="90px"></asp:TextBox>
                                        </td>
                                        </td>
                                        <td align="left">
                                            Today&#39;s Liability</td>
                                        <td>
                                            <asp:TextBox ID="txtcounterliability" runat="server" 
                                                Width="66px"></asp:TextBox>
                                        </td>
                                        <td style="WIDTH: 100px">
                                            <asp:Label ID="Label10" runat="server" 
                                                Text="Balance Receipt" Width="97px"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 100px">
                                            <asp:TextBox ID="txtreceiptno2" runat="server" 
                                                OnTextChanged="txtreceiptno2_TextChanged" tabIndex="52" 
                                                Width="66px"></asp:TextBox>
                                        </td>
                                        <td style="WIDTH: 100px">
                                            <asp:Label ID="Label18" runat="server" 
                                                Text="Security Deposit" Width="103px"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 100px">
                                            <asp:TextBox ID="txttotsecurity" runat="server" 
                                                Font-Bold="True" Font-Size="Small" tabIndex="54" 
                                                Width="66px"></asp:TextBox>
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
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Label ID="lblheading" runat="server" 
                            __designer:wfdid="w8" CssClass="heading" 
                            Text="General Room  Reservation" Width="330px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="right">
                        <asp:CheckBox ID="chkplainpaper" runat="server" 
                            AutoPostBack="True" 
                            OnCheckedChanged="chkplainpaper_CheckedChanged" 
                            style="height: 20px" Text="Old receipt" Width="153px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlswami" runat="server" 
                            BackColor="Transparent" GroupingText="Swami Details" 
                            Height="1%" Width="336px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="WIDTH: 95px">
                                            <asp:Label ID="lblswminame" runat="server" 
                                                __designer:wfdid="w232" Text="Swami name" Width="80px"></asp:Label>
                                            *</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtSwaminame" runat="server" 
                                                __designer:wfdid="w233" AutoPostBack="True" 
                                                OnTextChanged="txtSwaminame_TextChanged" Width="135px" 
                                                TabIndex="51"></asp:TextBox>
                                        </td>
                                        <td style="WIDTH: 23px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 95px">
                                            <asp:Label ID="lblplce" runat="server" 
                                                __designer:wfdid="w234" Text="Place" Width="61px"></asp:Label>
                                            *</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtPlace" runat="server" 
                                                __designer:wfdid="w235" AutoPostBack="True" 
                                                OnTextChanged="txtPlace_TextChanged" Width="135px" 
                                                TabIndex="52"></asp:TextBox>
                                        </td>
                                        <td style="WIDTH: 23px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 95px">
                                            <asp:Label ID="lblstate" runat="server" 
                                                __designer:wfdid="w236" Text="State" Width="61px"></asp:Label>                                            
                                            *</td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="cmbState" runat="server" 
                                                __designer:wfdid="w237" AutoPostBack="True" 
                                                DataTextField="statename" DataValueField="state_id" 
                                                Height="22px" onkeydown="=enterkey((keycode,shift)" 
                                                OnSelectedIndexChanged="cmbState_SelectedIndexChanged" 
                                                Width="140px" TabIndex="53">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="WIDTH: 23px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 95px" valign="top">
                                            <asp:Label ID="lbldistrict" runat="server" 
                                                __designer:wfdid="w238" Text="District" Width="56px"></asp:Label>                                            
                                            *</td>
                                        <td colspan="2" valign="top">
                                            <asp:DropDownList ID="cmbDistrict" runat="server" 
                                                __designer:wfdid="w239" AutoPostBack="True" 
                                                DataTextField="districtname" DataValueField="district_id" 
                                                Height="22px" 
                                                OnSelectedIndexChanged="cmbDistrict_SelectedIndexChanged" 
                                                Width="140px" TabIndex="54">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td valign="top">
                                            <asp:LinkButton ID="lnkDistrict" runat="server" 
                                                __designer:wfdid="w240" Visible="False">New</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 95px; HEIGHT: 26px">
                                            <asp:Label ID="lblphnno" runat="server" 
                                                __designer:wfdid="w241" Text="Phoneno" Width="64px"></asp:Label>
                                        </td>
                                        <td style="HEIGHT: 26px">
                                            <asp:TextBox ID="txtStd" runat="server" 
                                                __designer:wfdid="w242" AutoPostBack="True" MaxLength="5" 
                                                Width="42px" TabIndex="55"></asp:TextBox>
                                        </td>
                                        <td style="HEIGHT: 26px">
                                            <asp:TextBox ID="txtPhn" runat="server" 
                                                __designer:wfdid="w243" AutoPostBack="True" MaxLength="10" 
                                                Width="83px" TabIndex="56"></asp:TextBox>
                                        </td>
                                        <td rowspan="1" style="WIDTH: 23px; HEIGHT: 26px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 95px; HEIGHT: 26px">
                                            <asp:Label ID="lblMobile" runat="server" 
                                                __designer:wfdid="w16" Text="Mobile No"></asp:Label>
                                        </td>
                                        <td colspan="2" style="HEIGHT: 26px">
                                            <asp:TextBox ID="txtMobileNo" runat="server" 
                                                __designer:wfdid="w17" Width="135px" MaxLength="12" 
                                                TabIndex="57"></asp:TextBox>
                                        </td>
                                        <td rowspan="1" style="WIDTH: 23px; HEIGHT: 26px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 95px; HEIGHT: 26px">
                                            <asp:Label ID="lblEmail" runat="server" 
                                                __designer:wfdid="w18" Text="Email ID"></asp:Label>
                                            </td>
                                        <td colspan="2" style="HEIGHT: 26px">
                                            <asp:TextBox ID="txtEmail" runat="server" 
                                                __designer:wfdid="w19" AutoPostBack="True" 
                                                Width="135px" TabIndex="58" 
                                                ontextchanged="txtEmail_TextChanged"></asp:TextBox>
                                        </td>
                                        <td rowspan="1" style="WIDTH: 23px; HEIGHT: 26px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 95px; HEIGHT: 26px">
                                            <asp:Label ID="lblProofType" runat="server" 
                                                __designer:wfdid="w1" Text="ID Proof Type"></asp:Label>
                                            *</td>
                                        <td colspan="2" style="HEIGHT: 26px">
                                            <asp:DropDownList ID="cmbProofType" runat="server" 
                                                __designer:wfdid="w2" DataTextField="proof" 
                                                DataValueField="proof_id" Width="140px" TabIndex="59">
                                            </asp:DropDownList>
                                        </td>
                                        <td rowspan="1" style="WIDTH: 23px; HEIGHT: 26px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 95px; HEIGHT: 26px">
                                            <asp:Label ID="lblProofNo" runat="server" 
                                                __designer:wfdid="w3" Text="Proof No"></asp:Label>
                                            *</td>
                                        <td colspan="2" style="HEIGHT: 26px">
                                            <asp:TextBox ID="txtProofNo" runat="server" 
                                                __designer:wfdid="w4" Width="135px" TabIndex="60" 
                                                AutoPostBack="True" ontextchanged="txtProofNo_TextChanged"></asp:TextBox>
                                        </td>
                                        <td rowspan="1" style="WIDTH: 23px; HEIGHT: 26px">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="pnlreservtn" runat="server" 
                            __designer:wfdid="w106" BackColor="Transparent" 
                            GroupingText="Reservation  Details" Height="1%" 
                            style="margin-left: 0px" Width="298px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="WIDTH: 100px; HEIGHT: 26px">
                                            <asp:Label ID="lblnoofinmates" runat="server" 
                                                Text="No: of Inmates" Width="85px"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 115px; HEIGHT: 26px">
                                            <asp:TextBox ID="txtnoofinmates" runat="server" 
                                                AutoPostBack="True" OnTextChanged="TextBox5_TextChanged" 
                                                tabIndex="61" Width="135px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 100px; HEIGHT: 26px">
                                            <asp:Label ID="lblfrmdte" runat="server" 
                                                __designer:wfdid="w107" Text="Check in date" 
                                                Width="80px"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 115px; HEIGHT: 26px">
                                            <asp:TextBox ID="txtFrmdate" runat="server" 
                                                __designer:wfdid="w108" AutoPostBack="True" 
                                                OnTextChanged="txtFrmdate_TextChanged" Width="135px" 
                                                TabIndex="62"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 100px; HEIGHT: 34px">
                                            <asp:Label ID="lblchckin" runat="server" 
                                                __designer:wfdid="w109" Text="Check in time" Width="80px"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 115px; HEIGHT: 34px">
                                            <asp:TextBox ID="txtchkin" runat="server" 
                                                __designer:wfdid="w110" AutoPostBack="True" 
                                                OnTextChanged="txtchkin_TextChanged" Width="135px" 
                                                TabIndex="63"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 100px; HEIGHT: 26px">
                                            <asp:Label ID="lbltodate" runat="server" 
                                                __designer:wfdid="w111" Text="Check out date" Width="90px"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 115px">
                                            <asp:TextBox ID="txtTodate" runat="server" 
                                                __designer:wfdid="w112" AutoPostBack="True" MaxLength="1" 
                                                OnTextChanged="txtTodate_TextChanged" 
                                                ValidationGroup="Mtodate" Width="135px" TabIndex="64"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 100px; HEIGHT: 24px">
                                            <asp:Label ID="lblchckout" runat="server" 
                                                __designer:wfdid="w113" Text="Check out time" 
                                                Width="90px"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 115px; HEIGHT: 24px">
                                            <asp:TextBox ID="txtchkout" runat="server" 
                                                __designer:wfdid="w114" AutoPostBack="True" 
                                                OnTextChanged="txtchkout_TextChanged1" 
                                                Width="135px" TabIndex="65"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 100px; HEIGHT: 26px">
                                            <asp:Label ID="Label19" runat="server" Text="No of Hours"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 115px; HEIGHT: 26px">
                                            <asp:TextBox ID="txtnoofhours" runat="server" 
                                                ontextchanged="txtnoofhours_TextChanged" ReadOnly="True" 
                                                TabIndex="66" Width="135px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 100px; HEIGHT: 26px">
                                            <asp:Label ID="lblroomcategory" runat="server" 
                                                Text="Room Category"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 115px; HEIGHT: 26px">
                                            <asp:DropDownList ID="cmbroomcategory" runat="server" 
                                                AutoPostBack="True" DataTextField="room_cat_name" 
                                                DataValueField="room_cat_id" 
                                                onselectedindexchanged="cmbroomcategory_SelectedIndexChanged" 
                                                style="height: 22px" TabIndex="67" Width="140px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="cmbroomcategory_ListSearchExtender" 
                                                runat="server" Enabled="True" 
                                                TargetControlID="cmbroomcategory">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 100px; HEIGHT: 26px">
                                            <asp:Label ID="lblbuildingname" runat="server" Text="Building name" 
                                                Width="87px"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 115px; HEIGHT: 26px">
                                            <asp:DropDownList ID="cmbBuild" runat="server" AutoPostBack="True" 
                                                DataTextField="buildingname" DataValueField="build_id" Height="22px" 
                                                OnSelectedIndexChanged="cmbBuild_SelectedIndexChanged" tabIndex="13" 
                                                Width="140px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="cmbBuild_ListSearchExtender" runat="server" 
                                                TargetControlID="cmbBuild">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 100px; HEIGHT: 25px">
                                            <asp:Label ID="lbroomno" runat="server" 
                                                style="margin-top: 0px" Text="Room no" Width="55px"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 115px; HEIGHT: 25px">
                                            <asp:DropDownList ID="cmbRooms" runat="server" 
                                                AutoPostBack="True" DataTextField="roomno" 
                                                DataValueField="room_id" 
                                                OnSelectedIndexChanged="cmbRooms_SelectedIndexChanged" 
                                                tabIndex="68" Width="140px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender4" 
                                                runat="server" TargetControlID="cmbRooms">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="rentpanel" runat="server" 
                            GroupingText="Rent" Height="1%" 
                            style="margin-left: 0px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="WIDTH: 112px; HEIGHT: 26px">
                                            <asp:Label ID="lblroomrent" runat="server" Text="Room rent" 
                                                Width="60px"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 92px; HEIGHT: 26px">
                                            <asp:TextBox ID="txtroomrent" runat="server" 
                                                Enabled="False" Font-Bold="True" Height="17px" 
                                                tabIndex="68" Width="90px"></asp:TextBox>
                                        </td>
                                        <td style="WIDTH: 92px; HEIGHT: 26px">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 112px">
                                            <asp:Label ID="lblsecuritydeposit" runat="server" 
                                                Text="Security deposit" Width="95px"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 92px">
                                            <asp:TextBox ID="txtsecuritydeposit" runat="server" 
                                                Enabled="False" Font-Bold="True" Height="17px" 
                                                tabIndex="69" Width="90px"></asp:TextBox>
                                        </td>
                                        <td style="WIDTH: 92px">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 112px; HEIGHT: 26px">
                                            <asp:Label ID="Label7" runat="server" Text="Other charge" 
                                                Width="77px"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 92px; HEIGHT: 26px">
                                            <asp:TextBox ID="txtothercharge" runat="server" 
                                                AutoPostBack="True" Font-Bold="True" Height="17px" 
                                                tabIndex="70" Width="90px" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td style="WIDTH: 92px; HEIGHT: 26px">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 112px; HEIGHT: 18px">
                                            <asp:Label ID="lbltotalamount" runat="server" 
                                                Text="Total amount" Width="80px"></asp:Label>
                                            *</td>
                                        <td style="WIDTH: 92px; HEIGHT: 18px">
                                            <asp:TextBox ID="txttotalamount" runat="server" 
                                                Enabled="False" Font-Bold="True" Font-Size="X-Large" 
                                                ForeColor="OliveDrab" Height="33px" tabIndex="71" 
                                                Width="90px" Wrap="False"></asp:TextBox>
                                        </td>
                                        <td style="WIDTH: 92px; HEIGHT: 18px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 112px; HEIGHT: 18px">
                                            Amount Received<asp:RequiredFieldValidator 
                                                ID="valAmountreceived" runat="server" 
                                                ControlToValidate="txtadvance" ErrorMessage="*" 
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="WIDTH: 92px; HEIGHT: 18px">
                                            <asp:TextBox ID="txtadvance" runat="server" Height="17px" 
                                                tabIndex="72" Width="90px" AutoPostBack="True" 
                                                ontextchanged="txtadvance_TextChanged" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td style="WIDTH: 92px; HEIGHT: 18px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 112px; HEIGHT: 18px">
                                            <b>Balance* payable</b>
                                        </td>
                                        <td style="WIDTH: 92px; HEIGHT: 18px">
                                            <asp:TextBox ID="txtnetpayable" runat="server" 
                                                Enabled="False" Font-Size="X-Large" ForeColor="OliveDrab" 
                                                Height="33px" Width="90px" Wrap="False" TabIndex="73"></asp:TextBox>
                                        </td>
                                        <td style="WIDTH: 92px; HEIGHT: 18px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 112px; HEIGHT: 17px">
                                            <asp:Label ID="Label6" runat="server" Text="Grant total" 
                                                Visible="False"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 92px; HEIGHT: 17px">
                                            <asp:TextBox ID="txtgranttotal" runat="server" 
                                                Enabled="False" Font-Bold="True" Font-Size="X-Large" 
                                                ForeColor="OliveDrab" Height="33px" tabIndex="74" 
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
                    <td colspan="3" style="height: 29px">
                        <asp:Button ID="btnsave" runat="server" 
                            __designer:wfdid="w2" 
                            CssClass="btnStyle_large" Font-Bold="True" 
                            onclick="btnsave_Click" Text="Confirm Reservation" 
                            TabIndex="69" />
                        <asp:Button ID="btncancel" runat="server" 
                            __designer:wfdid="w46" CausesValidation="False" 
                            CssClass="btnStyle_small" onclick="btncancel_Click" 
                            Text="Cancel" Visible="False" TabIndex="70" />
                        <asp:Button ID="btnclear" runat="server" 
                            __designer:wfdid="w3" CausesValidation="False" 
                            CssClass="btnStyle_small" onclick="btnclear_Click" 
                            Text="Clear" TabIndex="71" />
                        <asp:Button ID="btnreport" runat="server" 
                            __designer:wfdid="w5" CausesValidation="False" 
                            CssClass="btnStyle_medium" onclick="btnreport_Click" 
                            TabIndex="72" />
                        <asp:Button ID="btnprint" runat="server" 
                            __designer:wfdid="w6" CssClass="btnStyle_small" 
                            onclick="btnprint_Click" Text="Print" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="pnlreport" runat="server" 
                            __designer:wfdid="w1" GroupingText="Report">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblreportdate" runat="server" 
                                                __designer:wfdid="w38" Text="Reservation date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtreportdatefrom" runat="server" 
                                                __designer:wfdid="w39" AutoPostBack="True" 
                                                OnTextChanged="txtreportdatefrom_TextChanged" Width="104px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblreportto" runat="server" 
                                                __designer:wfdid="w40" Text="Reservation to" Width="88px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtreportdateto" runat="server" 
                                                __designer:wfdid="w41" Width="104px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 100px">
                                            <asp:Button ID="btnreservelist" runat="server" 
                                                __designer:wfdid="w43" CausesValidation="False" 
                                                CssClass="btnStyle_large" onclick="btnreservelist_Click" 
                                                Text="Reservation Chart" />
                                        </td>
                                        <td style="WIDTH: 100px">
                                            <asp:Button ID="btnreportclear" runat="server" 
                                                __designer:wfdid="w44" CausesValidation="False" 
                                                CssClass="btnStyle_small" onclick="btnreportclear_Click" 
                                                Text="Clear" />
                                        </td>
                                        <td style="WIDTH: 100px">
                                            &nbsp;</td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="dgReserve" runat="server" 
                            __designer:wfdid="w14" AutoGenerateColumns="False" 
                            CellPadding="4" DataKeyNames="ReservationNo" 
                            ForeColor="#333333" GridLines="None" HorizontalAlign="Left" 
                            OnPageIndexChanging="dgReserve_PageIndexChanging" 
                            OnRowCreated="dgReserve_RowCreated" 
                            OnSelectedIndexChanged="dgReserve_SelectedIndexChanged" 
                            Visible="False" Width="849px">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" 
                                ForeColor="White" />
                            <Columns>
                                <asp:CommandField SelectText="" ShowSelectButton="True" />
                                <asp:BoundField DataField="ReservationNo" 
                                    HeaderText="Reservation No" />
                                <asp:BoundField DataField="PassNo" HeaderText="Pass No" 
                                    Visible="False" />
                                <asp:BoundField DataField="Customer" 
                                    HeaderText="Customer" />
                                <asp:BoundField DataField="RoomNo" HeaderText="Room No" />
                                <asp:BoundField DataField="Building" 
                                    HeaderText="Building" />
                                <asp:BoundField DataField="ReservedDate" 
                                    HeaderText="Reserved Date" />
                                <asp:BoundField DataField="ExpectedVecatingDate" 
                                    HeaderText="Expected Vacating Date" />
                            </Columns>
                            <RowStyle BackColor="#EFF3FB" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" 
                                ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" 
                                HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" 
                                ForeColor="White" HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table><tbody><tr><td vAlign=top colSpan=3><asp:Panel id="pnlpage" runat="server" Width="125px" __designer:wfdid="w9"><TABLE><TBODY><TR><TD style="WIDTH: 100px">
                <asp:Label id="lblpageno" runat="server" Width="52px" Text="Page no" 
                    __designer:wfdid="w10" Visible="False"></asp:Label></TD><TD style="WIDTH: 100px">
                    <asp:TextBox id="txt1" runat="server" Width="50px" __designer:wfdid="w11" 
                        Enabled="False" Visible="False">0</asp:TextBox> </TD><TD style="WIDTH: 100px">
                    <asp:Button id="btnnex" onclick="btnnex_Click" runat="server" Width="100px" 
                        CausesValidation="False" Text=">>" Font-Bold="True" __designer:wfdid="w12" 
                        Visible="False"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD vAlign=top>&nbsp;&nbsp;<BR /><BR />&nbsp;<asp:Button id="btnaltrnteroom" runat="server" Width="147px" CausesValidation="False" Text="Take alternate room" __designer:wfdid="w111" Visible="False"></asp:Button>&nbsp; <BR /><BR /><cc1:CalendarExtender id="cereportfrom" runat="server" __designer:wfdid="w113" TargetControlID="txtreportdatefrom" Format="dd-MM-yyyy"></cc1:CalendarExtender> <cc1:CalendarExtender id="cereportto" runat="server" __designer:wfdid="w114" TargetControlID="txtreportdateto" Format="dd-MM-yyyy"></cc1:CalendarExtender> <asp:Button id="btnnonoccupncy" onclick="btnnonoccupncy_Click" runat="server" CausesValidation="False" Text="Non ocuupancy" CssClass="btnStyle_large" __designer:wfdid="w115" Visible="False"></asp:Button><BR /><asp:RequiredFieldValidator id="rfvswaminame" runat="server" ForeColor="White" __designer:wfdid="w116" ControlToValidate="txtswaminame" ErrorMessage="Name required"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="rvffromdate" runat="server" ForeColor="White" __designer:wfdid="w117" ControlToValidate="txtfrmdate" ErrorMessage="From date required"></asp:RequiredFieldValidator><BR /><asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ForeColor="White" __designer:wfdid="w118" ControlToValidate="txtfrmdate" ErrorMessage="Format is dd/mm/yyyy" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator><BR /><asp:RequiredFieldValidator id="rfvcheckin" runat="server" ForeColor="White" __designer:wfdid="w119" ControlToValidate="txtchkin" ErrorMessage="Check in time required"></asp:RequiredFieldValidator><BR /><BR /><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ForeColor="White" __designer:wfdid="w120" ControlToValidate="txttodate" ErrorMessage="To date required"></asp:RequiredFieldValidator><BR /><BR /><BR /><asp:RequiredFieldValidator id="rfvtodate" runat="server" ForeColor="White" __designer:wfdid="w3" ControlToValidate="txtTodate" ErrorMessage="Todate required"></asp:RequiredFieldValidator><BR /><BR /><BR /><BR /><BR /><BR /><BR /><BR /><BR /><asp:Panel id="pnldays" runat="server" Width="306px" Height="100%" GroupingText="not in design but using in code" __designer:wfdid="w160" Visible="False"><TABLE><TBODY><TR><TD><asp:Label id="lblnoofunits" runat="server" __designer:wfdid="w161"></asp:Label></TD><TD>
                    <asp:TextBox id="txtresno" runat="server" Width="110px" Visible="False" 
                        __designer:wfdid="w162"></asp:TextBox></TD></TR><TR><TD><asp:Label id="lblnoofdys" runat="server" Width="86px" Text="No: of days" __designer:wfdid="w163"></asp:Label></TD><TD><asp:TextBox id="txtnoofdys" runat="server" Width="110px" __designer:wfdid="w164"></asp:TextBox></TD></TR><TR><TD><asp:Label id="lblrsrvtnchrge" runat="server" Width="113px" Text="Reservation charge" __designer:wfdid="w165"></asp:Label></TD><TD><asp:TextBox id="txtrservtnchrge" runat="server" Width="109px" __designer:wfdid="w166"></asp:TextBox></TD></TR><TR><TD><asp:Label id="lbladrs" runat="server" Width="68px" Visible="False" Text="Address" __designer:wfdid="w167"></asp:Label></TD><TD><asp:TextBox id="txtadrs" runat="server" Width="129px" Visible="False" __designer:wfdid="w168" AutoPostBack="True" OnTextChanged="txtadrs_TextChanged"></asp:TextBox></TD></TR><TR><TD><asp:TextBox id="txtseason" runat="server" Width="107px" Visible="False" __designer:wfdid="w169" Enabled="False"></asp:TextBox></TD><TD>
                    <asp:TextBox id="txtyear" runat="server" Width="107px" Visible="False" 
                        __designer:wfdid="w170" Enabled="False"></asp:TextBox></TD></TR><TR><TD></TD><TD>
                <asp:Button id="btnsearch" runat="server" Width="81px" CausesValidation="False" 
                    Text="Search" __designer:wfdid="w171"></asp:Button></TD></TR>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                </tbody></table></asp:Panel></TD><TD style="WIDTH: 329px" vAlign=top>&nbsp;&nbsp;&nbsp;&nbsp;<BR /> <BR /><BR /> <asp:Panel id="pnlMessage" runat="server" __designer:dtid="562958543355914" CssClass="ModalWindow" __designer:wfdid="w173" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel6" runat="server" BackColor="LightSteelBlue" __designer:dtid="562958543355915" __designer:wfdid="w174" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Width="197px" Font-Bold="True" Text="Tsunami ARMS - Confirmation" __designer:dtid="562958543355916" __designer:wfdid="w46" ForeColor="MediumBlue"></asp:Label> <asp:Label id="lblHead2" runat="server" Width="191px" Font-Bold="True" Text="Tsunami ARMS - Warning" __designer:wfdid="w56" ForeColor="MediumBlue"></asp:Label><BR /></asp:Panel> <asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px" __designer:dtid="562958543355918" __designer:wfdid="w177"><TABLE style="WIDTH: 237px" __designer:dtid="562958543355919"><TBODY><TR __designer:dtid="562958543355920"><TD align=center colSpan=1 __designer:dtid="562958543355921"></TD><TD align=center colSpan=3 __designer:dtid="562958543355922"><BR /><asp:Label id="lblMsg" runat="server" Height="25px" ForeColor="Black" Text="Do you want to save?" __designer:dtid="562958543355923" __designer:wfdid="w48" Font-Size="Small"></asp:Label></TD></TR><TR __designer:dtid="562958543355928"><TD __designer:dtid="562958543355929"></TD><TD align=center __designer:dtid="562958543355930">&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" Font-Bold="True" __designer:dtid="562958543355931" CssClass="btnStyle" __designer:wfdid="w179"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" Font-Bold="True" __designer:dtid="562958543355932" CssClass="btnStyle" __designer:wfdid="w180"></asp:Button>&nbsp;</TD><TD align=center __designer:dtid="562958543355933">&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> 
                                    <asp:Panel id="pnlOk" runat="server" Width="125px" 
                                        Height="50px" __designer:dtid="562958543355938" 
                                        __designer:wfdid="w181"><TABLE style="WIDTH: 237px" __designer:dtid="562958543355939"><TBODY><TR __designer:dtid="562958543355940"><TD align=center colSpan=1 __designer:dtid="562958543355941"></TD><TD align=center colSpan=3 __designer:dtid="562958543355942"><BR /><asp:Label id="lblOk" runat="server" Height="25px" ForeColor="Black" Text="Do you want to ?" __designer:dtid="562958543355943" __designer:wfdid="w52"></asp:Label></TD></TR><TR __designer:dtid="562958543355948"><TD __designer:dtid="562958543355949"></TD><TD align=center __designer:dtid="562958543355950">&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="Button3" onclick="Button3_Click" runat="server" Width="50px" CausesValidation="False" ForeColor="Black" Text="OK" Font-Bold="True" __designer:dtid="562958543355951" CssClass="btnStyle" __designer:wfdid="w183"></asp:Button> &nbsp; </TD><TD align=center __designer:dtid="562958543355952">&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> <cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" __designer:dtid="562958543355957" __designer:wfdid="w184" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize"></cc1:ModalPopupExtender> <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden" __designer:dtid="562958543355909" __designer:wfdid="w185"></asp:Button></asp:Panel></TD><TD style="WIDTH: 329px" vAlign=top>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <cc1:ListSearchExtender id="state" runat="server" __designer:wfdid="w149" TargetControlID="cmbState"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="district" runat="server" __designer:wfdid="w150" TargetControlID="cmbDistrict"></cc1:ListSearchExtender>&nbsp; <cc1:CalendarExtender id="CalendarExtender2" runat="server" __designer:wfdid="w44" TargetControlID="txtTodate" Format="dd-MM-yyyy "></cc1:CalendarExtender> <cc1:CalendarExtender id="CalendarExtender1" runat="server" __designer:wfdid="w152" TargetControlID="txtFrmdate" Format="dd-MM-yyyy "></cc1:CalendarExtender><BR /> <cc1:FilteredTextBoxExtender id="FilteredTextBoxswaminame" runat="server" __designer:wfdid="w154" TargetControlID="txtswaminame" FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars=". "></cc1:FilteredTextBoxExtender> <cc1:FilteredTextBoxExtender id="FilteredTextBoxplace" runat="server" __designer:wfdid="w155" TargetControlID="txtplace" FilterType="UppercaseLetters, LowercaseLetters"></cc1:FilteredTextBoxExtender> <cc1:FilteredTextBoxExtender id="FilteredTextBoxstd" runat="server" __designer:wfdid="w156" TargetControlID="txtstd" FilterType="Numbers" ValidChars="+"></cc1:FilteredTextBoxExtender> <cc1:FilteredTextBoxExtender id="FilteredTextBoxphone" runat="server" __designer:wfdid="w157" TargetControlID="txtphn" FilterType="Numbers"></cc1:FilteredTextBoxExtender>&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblcategorydetails" runat="server" 
                            Visible="False" Width="240px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblreporttype" runat="server" 
                            __designer:wfdid="w35" Text="Reservation type" 
                            Visible="False" Width="104px"></asp:Label>
                        <asp:DropDownList ID="cmbReportpass" runat="server" 
                            __designer:wfdid="w36" Height="22px" Visible="False" 
                            Width="111px">
                            <asp:ListItem Value="-1">All</asp:ListItem>
                            <asp:ListItem>Donor Free</asp:ListItem>
                            <asp:ListItem>Donor Paid</asp:ListItem>
                            <asp:ListItem>Tdb</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblmessage" runat="server" 
                            __designer:wfdid="w37" ForeColor="Red" 
                            Text="Select all essential fields" Visible="False" 
                            Width="152px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="Label20" runat="server" Text="Validators" 
                            Visible="False"></asp:Label>
                        <asp:Label ID="lblMsg1" runat="server" 
                            __designer:wfdid="w20" Font-Bold="True" ForeColor="Red" 
                            Text="Label" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:RegularExpressionValidator ID="RegMobile" 
                            runat="server" SetFocusOnError="True" 
                            
                            ValidationExpression="^0{0,1}[1-9]{1}[0-9]{2}[\s]{0,1}[\-]{0,1}[\s]{0,1}[1-9]{1}[0-9]{6}$" 
                            ControlToValidate="txtMobileNo" 
                            ErrorMessage="Enter a valid Mobile number" 
                            Font-Size="Smaller" ForeColor="#EEEEEE"></asp:RegularExpressionValidator>
                        <cc1:ValidatorCalloutExtender ID="RegMobile_ValidatorCalloutExtender" 
                            runat="server" Enabled="True" TargetControlID="RegMobile">
                        </cc1:ValidatorCalloutExtender>
                        <asp:RegularExpressionValidator ID="RegEmail" 
                            runat="server" __designer:wfdid="w22" 
                            ControlToValidate="txtEmail" ErrorMessage="Invalid Email" 
                            ForeColor="#EEEEEE" 
                            
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                            SetFocusOnError="True"></asp:RegularExpressionValidator>
                              <cc1:ValidatorCalloutExtender ID="RegEmail_ValidatorCalloutExtender" 
                            runat="server" Enabled="True" TargetControlID="RegEmail">
                        </cc1:ValidatorCalloutExtender>
                    </td>
                </tr>
                <caption>
                    <tdcolspan="3">
                    &nbsp;</td> 
                </tr>
            </tdcolspan="3">
                </caption>
            </table>
        </contenttemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btnreservelist" />
        <asp:PostBackTrigger ControlID="btnsave" />
        </Triggers>
        </asp:UpdatePanel>               
</asp:Content>

