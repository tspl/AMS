<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Donor Reservation.aspx.cs" Inherits="Donor_Free_Reservation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %><asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        <span style="color: black"></span><span style="color: black">
            <br />
            Select building name or donor
            name to short list pass and reservation grid<br />
            <br />
        </span><span style="color: black"></span><span style="color: black"></span><span style="color: black">Press <strong><span style="color: black">
            ENTER</span></strong><span> key or TAB</span><span
                style="color: #000000">
            key to navigate to next line.<br />
                <br />
            </span></span><span style="color: black"></span><span style="color: black">
            Click<span
                style="color: black"> <strong><span>Pass No missing</span></strong></span> &nbsp;button in case of missing
                pass<br />                
                    <br />
                </span><span style="color: black"></span><span>use <span style="color: black"><strong>
            ADD</strong></span> button to reserve multiple rooms or reserve room using multiple
            pass</span></asp:Panel>
        <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">    <contenttemplate><table><tbody><tr>
    <td style="TEXT-ALIGN: center" colSpan=4>
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
                                Width="66px" Height="22px"></asp:TextBox>
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
                                <asp:Label ID="Label2" runat="server" 
                                    Text="No of Transactions" Width="122px"></asp:Label>
                            </td>
                            <td style="WIDTH: 100px; height: 26px;">
                                <asp:TextBox ID="txtnooftrans" runat="server" tabIndex="55" 
                                    Width="66px"></asp:TextBox>
                            </td>
                        </td>
                        <td style="WIDTH: 100px; height: 26px;">
                        <asp:Label ID="Label17" runat="server" Text="Uncliamed " Width="103px"></asp:Label>
                    </td>
                    <td style="WIDTH: 100px; height: 26px;">
                        <asp:TextBox ID="txtunclaimed" runat="server" Font-Bold="True" 
                            Font-Size="Small" tabIndex="54" Width="66px"></asp:TextBox>
                    </tr>
                    <tr>
                        <td style="WIDTH: 50px">
                            <asp:Label ID="Label99" runat="server" Text="Login Time:" 
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
                        <asp:Label ID="Label5" runat="server" 
                            Text=" Counter Deposit" Width="103px"></asp:Label>
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:TextBox ID="txtcounterdeposit" runat="server" Font-Bold="True" 
                            Font-Size="Small" tabIndex="54" Width="66px"></asp:TextBox>
                    </td>                    
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
    </td></tr>
    <tr>
        <td colSpan="4" style="TEXT-ALIGN: center">
            <asp:Label ID="lblheading" runat="server" 
                __designer:wfdid="w8" CssClass="heading" 
                Text="Donor Reservation" Width="330px"></asp:Label>
            &nbsp;
            <asp:CheckBox ID="chkplainpaper" runat="server" 
                AutoPostBack="True" 
                OnCheckedChanged="chkplainpaper_CheckedChanged" 
                style="height: 20px" Text="Old receipt" Width="153px" />
        </td>
    </tr>
    <tr><td vAlign=top rowspan="4" width="200px">
        <asp:Panel id="pnlpassdetails" runat="server" Width="84%" 
            GroupingText="Pass details"><table><tbody><tr><td vAlign=top colSpan=2>
                <asp:RadioButtonList id="rbtnPassIssueType" runat="server" 
                    Width="240px" __designer:wfdid="w205" 
                    OnSelectedIndexChanged="rbtnPassIssueType_SelectedIndexChanged" 
                    AutoPostBack="true" Repeatdirection="Horizontal"><asp:ListItem Selected="true" Value="0">Manual</asp:ListItem>
<asp:ListItem Value="1">Printed</asp:ListItem></asp:RadioButtonList></td></tr><tr>
                    <td style="WIDTH: 103px; HEIGHT: 26px">
            <asp:Label id="lblBarcode" runat="server" Width="45px" 
                Text="Barcode" __designer:wfdid="w206"></asp:Label></td><td style="WIDTH: 160px; HEIGHT: 26px"><asp:TextBox id="txtBarcode" runat="server" Width="144px" __designer:wfdid="w207" AutoPostBack="true" OnTextChanged="txtBarcode_TextChanged"></asp:TextBox></td></tr><tr>
                <td style="WIDTH: 103px">
<asp:Label ID="lblpsstype" runat="server" Text="Pass type" 
            Width="60px" Height="20px"></asp:Label>*</td><td style="WIDTH: 160px">
<asp:DropDownList ID="cmbPasstype" runat="server" AutoPostBack="true" Height="22px" OnSelectedIndexChanged="cmbPasstype_SelectedIndexChanged" Width="150px">
<asp:ListItem Value="-1">-Select-</asp:ListItem><asp:ListItem Value="0">Free Pass</asp:ListItem><asp:ListItem Value="1">Paid Pass</asp:ListItem></asp:DropDownList>
</td></tr><tr><td style="WIDTH: 103px"><asp:Label id="lblpassno" runat="server" 
                        Width="50px" Text="Pass no" __designer:wfdid="w210"></asp:Label>
            *</td><td style="WIDTH: 160px">
<asp:TextBox ID="txtPassNo" runat="server"  AutoPostBack="true" 
                MaxLength="5" OnTextChanged="txtPassNo_TextChanged1" 
                Width="145px" style="height: 22px"></asp:TextBox>
</td></tr><tr><td style="WIDTH: 103px" valign="top">
                    <asp:Label id="lblbldngnme" 
                        runat="server" Width="82px" Text="Building name" 
                        __designer:wfdid="w212"></asp:Label>
            *</td>
<td style="WIDTH: 160px"><asp:DropDownList id="cmbBuilding" runat="server" Width="150px" Height="22px" OnSelectedIndexChanged="cmbBuilding_SelectedIndexChanged" AutoPostBack="true" DataValueField="build_id" DataTextField="buildingname"><asp:ListItem></asp:ListItem></asp:DropDownList></td></tr><tr>
                <td style="WIDTH: 103px" valign="top">
<asp:Label id="lblroomno" runat="server" Text="Room no" Width="55px"></asp:Label>
        *</td><td style="WIDTH: 160px" valign="top">
<asp:DropDownList id="cmbRoom" runat="server" Width="150px" Height="22px" OnSelectedIndexChanged="cmbRoom_SelectedIndexChanged" AutoPostBack="true" DataValueField="room_id" DataTextField="roomno"><asp:ListItem></asp:ListItem>
</asp:DropDownList></td></tr><tr><td style="WIDTH: 103px"><asp:Label id="lbldnrname" runat="server" Text="Donor  name" ></asp:Label>
            *</td><td style="WIDTH: 160px" colspan="3">
            <asp:DropDownList ID="cmbDonor" runat="server" 
                __designer:wfdid="w217" AutoPostBack="true" 
                DataTextField="donor_name" DataValueField="donor_id" 
                Height="22px" Width="150px">
<asp:ListItem></asp:ListItem></asp:DropDownList><BR /><asp:TextBox id="txtdonorname" runat="server" Width="145px" OnTextChanged="txtdonorname_TextChanged" Visible="False"></asp:TextBox></td></tr><tr>
                    <td style="WIDTH: 103px; ">
<asp:Label id="lbldnradd" runat="server" Width="30px" Text="Place" ></asp:Label>
            *</td><td style="WIDTH: 160px; ">
<asp:TextBox ID="txtdonoraddress" runat="server" AutoPostBack="true" OnTextChanged="txtdonoraddress_TextChanged" Width="145px"></asp:TextBox>
</td></tr><tr><td style="WIDTH: 103px; HEIGHT: 14px" vAlign=top>
                    <asp:Label id="lbldnrstate" runat="server" Width="30px" 
                        Text="State"></asp:Label></td><td style="WIDTH: 160px; HEIGHT: 14px" vAlign=top>
<asp:DropDownList id="cmbDnrstate" runat="server" Width="150px" Height="22px" OnSelectedIndexChanged="cmbDnrstate_SelectedIndexChanged" AutoPostBack="true" DataValueField="state_id" DataTextField="statename"><asp:ListItem></asp:ListItem>
</asp:DropDownList></td></tr><tr><td style="WIDTH: 103px; HEIGHT: 14px" 
                        vAlign=top>
<asp:Label id="lbldnrdistrict" runat="server" Text="District" Width="40px"></asp:Label></td><td style="WIDTH: 160px; HEIGHT: 14px" vAlign=top>
<asp:DropDownList ID="cmbDstrct" runat="server" AutoPostBack="true" DataTextField="districtname" DataValueField="district_id" Height="22px" Width="150px">
<asp:ListItem></asp:ListItem></asp:DropDownList></td></tr><tr>
                    <td style="WIDTH: 103px; HEIGHT: 14px" vAlign=top><asp:Label id="lblMob" runat="server" Text="Mobile No" __designer:wfdid="w12"></asp:Label></td><td style="WIDTH: 160px; HEIGHT: 14px" vAlign=top>
<asp:TextBox id="txtMob" runat="server"></asp:TextBox></td></tr><tr>
                    <td style="WIDTH: 103px; HEIGHT: 14px" valign="top"><asp:Label ID="lblEmail1" runat="server" Text="Email ID"></asp:Label>
</td><td style="WIDTH: 160px; HEIGHT: 14px; " valign="top"><asp:TextBox ID="txtEmailID2" runat="server"></asp:TextBox></td></tr>
<tr><td style="WIDTH: 103px; HEIGHT: 13px"></td>
    <td style="WIDTH: 160px; HEIGHT: 13px; TEXT-ALIGN: center" 
        align="center"><asp:Button ID="btnGetPass" runat="server" 
            CausesValidation="False" CssClass="btnStyle_medium" 
            onclick="btngetpass_Click" Text="Pass No missing" 
            UseSubmitBehavior="False" />
</td></tr></tbody></table></asp:Panel>
        <asp:Panel ID="pnlpass" runat="server" BackColor="#8080FF" 
            GroupingText="Pass details" Height="100%" Visible="False" 
            Width="222px">
            <table>
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="lblaoltter" runat="server" 
                                Text="AO letter no" Width="70px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtaoltr" runat="server" 
                                __designer:wfdid="w254" Width="107px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblreason" runat="server" 
                                __designer:wfdid="w255" Text="Reason" Width="60px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cmbPassreason" runat="server" 
                                __designer:wfdid="w256" AutoPostBack="true" 
                                DataTextField="reason" DataValueField="reason_id" 
                                Height="22px" 
                                OnSelectedIndexChanged="cmbReason_SelectedIndexChanged" 
                                Width="115px">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlbuilding" runat="server" 
            __designer:wfdid="w15" 
            GroupingText="Alternate building details" Height="100%" 
            Width="100%">
            <table>
                <tbody>
                    <tr>
                        <td style="WIDTH: 136px" width="100px">
                            <asp:Label ID="lblaltrntebldng" runat="server" 
                                __designer:wfdid="w16" Text="Alternate building name" 
                                Width="100px"></asp:Label>
                        </td>
                        <td style="WIDTH: 116px">
                            <asp:DropDownList ID="cmbaltbuilding" runat="server" 
                                __designer:wfdid="w17" AutoPostBack="true" 
                                DataTextField="buildingname" DataValueField="build_id" 
                                Height="30px" 
                                OnSelectedIndexChanged="cmbaltbuilding_SelectedIndexChanged" 
                                Width="135px">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="WIDTH: 100px; height: 40x;" width="100px">
                            <asp:Label ID="lblAltroomReason" runat="server" 
                                __designer:wfdid="w6" Height="40px" 
                                Text="Reason For Alternate room" Width="100px"></asp:Label>
                        </td>
                        <td style="WIDTH: 116px; height: 27px;" vAlign="middle">
                            <asp:DropDownList ID="cmbReason" runat="server" 
                                AutoPostBack="true" DataTextField="reason" 
                                DataValueField="reason_id" Height="30px" 
                                OnSelectedIndexChanged="cmbReason_SelectedIndexChanged" 
                                Width="135px">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="WIDTH: 136px" width="100px">
                            <asp:Label ID="lblaltrnteroom" runat="server" 
                                __designer:wfdid="w5" Text="Alternate Room " Width="91px"></asp:Label>
                        </td>
                        <td style="WIDTH: 116px" vAlign="middle">
                            <asp:DropDownList ID="cmbaltroom" runat="server" 
                                AutoPostBack="true" DataTextField="roomno" 
                                DataValueField="room_id" Height="30px" 
                                OnSelectedIndexChanged="cmbaltroom_SelectedIndexChanged" 
                                Width="135px">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="WIDTH: 136px" width="100px">
                            &nbsp;</td>
                        <td style="WIDTH: 116px" vAlign="middle">
                            &nbsp;</td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        </td>
        <td vAlign=top rowspan="2">
            <asp:Panel ID="pnlswami" runat="server" 
                GroupingText="Swami Details" Width="260px">
                <table style="height: 214px; width: 140px">
                    <tbody>
                        <tr>
                            <td width="270px">
                                <asp:Label ID="lblswminame" runat="server" 
                                    Text="Swami name" Width="75px"></asp:Label>
                                *</td>
                            <td colSpan="2">
                                <asp:TextBox ID="txtSwaminame" runat="server" 
                                    AutoPostBack="true" 
                                    OnTextChanged="txtSwaminame_TextChanged" Width="120px"></asp:TextBox>
                            </td>
                            <td rowspan="1" style="width: 11px" width="10px">
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 95px">
                                <asp:Label ID="lblplce" runat="server" Text="Place" 
                                    Width="35px"></asp:Label>
                                *</td>
                            <td colSpan="2">
                                <asp:TextBox ID="txtPlace" runat="server" 
                                    AutoPostBack="true" CssClass="UpperCaseFirstLetter" 
                                    OnTextChanged="txtPlace_TextChanged" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 95px">
                                <asp:Label ID="lblstate" runat="server" Text="State" 
                                    Width="30px"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvstate" runat="server" 
                                    ControlToValidate="cmbState" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </td>
                            <td colSpan="2">
                                <asp:DropDownList ID="cmbState" runat="server" 
                                    AutoPostBack="true" DataTextField="statename" 
                                    DataValueField="state_id" Height="22px" 
                                    onkeydown="=enterkey((keycode,shift)" 
                                    OnSelectedIndexChanged="cmbState_SelectedIndexChanged" 
                                    Width="125px">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 11px" width="10px">
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 95px" vAlign="top">
                                <asp:Label ID="lbldistrict" runat="server" Text="District" 
                                    Width="40px"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvDistrict" runat="server" 
                                    ControlToValidate="cmbDistrict" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </td>
                            <td colSpan="2" vAlign="top">
                                <asp:DropDownList ID="cmbDistrict" runat="server" 
                                    AutoPostBack="true" DataTextField="districtname" 
                                    DataValueField="district_id" Height="22px" 
                                    OnSelectedIndexChanged="cmbDistrict_SelectedIndexChanged" 
                                    Width="125px">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td vAlign="top" style="width: 11px" width="10px">
                                <asp:LinkButton ID="lnkDistrict" runat="server" 
                                    Visible="False">New</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td 26px"="" HEIGHT:="">
                                <asp:Label ID="lblphnno" runat="server" Text="Phoneno" 
                                    Width="50px"></asp:Label>
                            </td>
                            <td style="HEIGHT: 26px">
                                <asp:TextBox ID="txtStd" runat="server" AutoPostBack="true" 
                                    MaxLength="5" Width="42px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhn" runat="server" AutoPostBack="true" 
                                    MaxLength="10" Width="70px"></asp:TextBox>
                            </td>
                            <td 26px"="" HEIGHT:="" rowSpan="1" style="width: 11px" 
                                width="10px">
                            </td>
                        </tr>
                        <tr>
                            <td 26px"="" HEIGHT:="">
                                <asp:Label ID="lblMobile" runat="server" Text="Mobile No"></asp:Label>
                            </td>
                            <td colSpan="2">
                                <asp:TextBox ID="txtMobileNo" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td rowSpan="1" style="HEIGHT: 26px; width: 11px;" 
                                >
                            </td>
                        </tr>
                        <tr>
                            <td 26px"="" HEIGHT:="">
                                <asp:Label ID="lblEmail" runat="server" Text="Email ID"></asp:Label>
                                </td>
                            <td colSpan="2">
                                <asp:TextBox ID="txtEmail" runat="server" 
                                    AutoPostBack="true" Width="120px"></asp:TextBox>
                            </td>
                            <td 26px"="" HEIGHT:="" rowSpan="1" style="width: 11px" 
                                >
                                <asp:Label ID="lblMsg1" runat="server" Font-Bold="true" 
                                    ForeColor="Red" Text="Label" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 95px; HEIGHT: 26px">
                                <asp:Label ID="lblProofType" runat="server" 
                                    Text="Proof Type"></asp:Label>
                                </td>
                            <td colSpan="2" style="HEIGHT: 26px">
                                <asp:DropDownList ID="cmbProofType" runat="server" 
                                    DataTextField="proof" DataValueField="proof_id" 
                                    Width="125px">
                                </asp:DropDownList>
                            </td>
                            <td rowSpan="1" style="HEIGHT: 26px; width: 11px;" 
                                >
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 95px; HEIGHT: 26px">
                                <asp:Label ID="lblProofNo" runat="server" Text="Proof No"></asp:Label>
                                </td>
                            <td colSpan="2" style="HEIGHT: 26px">
                                <asp:TextBox ID="txtProofNo" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td  rowSpan="1" style="width: 11px">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </td>
        <td rowspan="2" vAlign="top" colspan="1">
            <asp:Panel ID="pnlreservtn" runat="server" 
                GroupingText="Reservation Date" Height="100%" Width="170px">
                <table>
                    <tbody>
                        <tr>
                            <td style="WIDTH: 100px; HEIGHT: 26px">
                                <asp:Label ID="lblnoofinmates" runat="server" 
                                    Text="No: of Inmates" Width="85px"></asp:Label>
                                *</td>
                            <td style="WIDTH: 115px">
                                <asp:TextBox ID="txtnoofinmates" runat="server" 
                                    AutoPostBack="True" tabIndex="61" Width="70px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 100px; HEIGHT: 26px">
                                <asp:Label ID="lblfrmdte" runat="server" 
                                    Text="Check In date" Width="86px"></asp:Label>
                                *</td>
                            <td style="WIDTH: 115px">
                                <asp:TextBox ID="txtFrmdate" runat="server" 
                                    __designer:wfdid="w108" AutoPostBack="True" Height="22px" 
                                    OnTextChanged="txtFrmdate_TextChanged" TabIndex="62" 
                                    Width="70px"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                    __designer:wfdid="w152" Format="dd-MM-yyyy " 
                                    TargetControlID="txtFrmdate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 100px; HEIGHT: 25px">
                                <asp:Label ID="lblchckin" runat="server" 
                                    Text="Check In time" Width="80px"></asp:Label>
                                *</td>
                            <td style="WIDTH: 115px; HEIGHT: 25px">
                                <asp:TextBox ID="txtchkin" runat="server" 
                                    AutoPostBack="true" OnTextChanged="txtchkin_TextChanged" 
                                    Width="70px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 100px; HEIGHT: 26px">
                                <asp:Label ID="lbltodate" runat="server" 
                                    style="margin-bottom: 0px" Text="Check Out date" 
                                    Width="91px"></asp:Label>
                                *</td>
                            <td style="WIDTH: 115px; HEIGHT: 26px">
                                <asp:TextBox ID="txtTodate" runat="server" 
                                    __designer:wfdid="w112" AutoPostBack="True" MaxLength="1" 
                                    OnTextChanged="txtTodate_TextChanged" TabIndex="64" 
                                    ValidationGroup="Mtodate" Width="70px"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" 
                                    __designer:wfdid="w44" Format="dd-MM-yyyy " 
                                    TargetControlID="txtTodate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 100px; HEIGHT: 34px">
                                <asp:Label ID="lblchckout" runat="server" 
                                    Text="Check Out time" Width="91px"></asp:Label>
                                *</td>
                            <td style="WIDTH: 115px; HEIGHT: 34px">
                                <asp:TextBox ID="txtchkout" runat="server" 
                                    AutoPostBack="true" OnTextChanged="txtchkout_TextChanged1" 
                                    Width="70px" style="height: 22px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 100px; HEIGHT: 34px">
                                <asp:Label ID="Label19" runat="server" Text="No of Hours"></asp:Label>
                            </td>
                            <td style="WIDTH: 115px; HEIGHT: 34px">
                                <asp:TextBox ID="txtnoofhours" runat="server" 
                                    ReadOnly="True" TabIndex="67" Width="70px"></asp:TextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td vAlign="top" colspan="2">
            <asp:Panel ID="rentpanel" runat="server" 
                GroupingText="Rent" Width="113%" Wrap="False" 
                style="margin-left: 0px">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 81px">
                            <asp:Label ID="lblroomrent" runat="server" Text="Room rent" 
                                Width="73px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtroomrent" runat="server" 
                                Enabled="False" Font-Bold="True" Height="17px" 
                                tabIndex="68" Width="60px"></asp:TextBox>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 81px">
                            <asp:Label ID="lblsecuritydeposit" runat="server" 
                                Text="Security deposit" Width="97px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsecuritydeposit" runat="server" 
                                Enabled="False" Font-Bold="True" Height="17px" 
                                tabIndex="69" Width="60px"></asp:TextBox>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 81px">
                            <asp:Label ID="Label7" runat="server" Text="Other charge" 
                                Width="77px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtothercharge" runat="server" 
                                AutoPostBack="True" Font-Bold="True" Height="17px" 
                                ReadOnly="True" tabIndex="70" Width="60px"></asp:TextBox>
                        </td>
                       
                    </tr>
                    <tr>
                        <td style="width: 81px">
                            <asp:Label ID="lbltotalamount" runat="server" 
                                Text="Total amount" Width="80px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttotalamount" runat="server" 
                                Enabled="False" Font-Bold="True" Font-Size="X-Large" 
                                ForeColor="OliveDrab" Height="33px" tabIndex="71" 
                                Width="60px" Wrap="False"></asp:TextBox>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 81px">
                            Amount Received<asp:RequiredFieldValidator 
                                ID="valAmountreceived" runat="server" 
                                ControlToValidate="txtadvance" ErrorMessage="*" 
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtadvance" runat="server" 
                                AutoPostBack="True" Height="17px" 
                                ontextchanged="txtadvance_TextChanged" ReadOnly="True" 
                                tabIndex="72" Width="60px"></asp:TextBox>
                        </td>
                       
                    </tr>
                    <tr>
                        <td style="width: 81px">
                            <b>Balance payable</b></td>
                        <td>
                            <asp:TextBox ID="txtnetpayable" runat="server" 
                                Enabled="False" Font-Size="X-Large" ForeColor="OliveDrab" 
                                Height="33px" TabIndex="73" Width="60px" Wrap="False"></asp:TextBox>
                        </td>
                       
                    </tr>
                    <tr>
                        <td style="width: 81px">
                            <asp:Label ID="Label6" runat="server" Text="Grant total" 
                                Visible="False"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtgranttotal" runat="server" 
                                Enabled="False" Font-Bold="True" Font-Size="X-Large" 
                                ForeColor="OliveDrab" Height="33px" tabIndex="74" 
                                Visible="False" Width="60px"></asp:TextBox>
                        </td>
                        
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr><td style="TEXT-ALIGN: left" vAlign=top colSpan=3 rowSpan=1><table><tbody><tr><td style="HEIGHT: 27px; TEXT-ALIGN: center" colSpan=5>
        <asp:Button id="btnsave" onclick="btnsave_Click" 
            runat="server" Text="Confirm Reservation" Font-Bold="true" 
            CssClass="btnStyle_large" __designer:wfdid="w2" CausesValidation="False"></asp:Button>&nbsp;<asp:Button id="btncancel" onclick="btncancel_Click" runat="server" CausesValidation="False" Text="Cancel" CssClass="btnStyle_small" __designer:wfdid="w46" Visible="False"></asp:Button> <asp:Button id="btnclear" onclick="btnclear_Click" runat="server" CausesValidation="False" Text="Clear" CssClass="btnStyle_small" __designer:wfdid="w3"></asp:Button><asp:Button id="btnnext" onclick="btnnext_Click" runat="server" Text="Add" CssClass="btnStyle_small" __designer:wfdid="w4" OnClientClick="ClearLastMessage('Label1')"></asp:Button>
<asp:Button id="btnreport" onclick="btnreport_Click" runat="server" CausesValidation="False" Text="View Report" CssClass="btnStyle_medium" 
        __designer:wfdid="w5"></asp:Button><asp:Button id="btnprint" onclick="btnprint_Click" runat="server" Text="Print" CssClass="btnStyle_small" __designer:wfdid="w6" Visible="False"></asp:Button></td></tr></tbody></table>
        <asp:Panel id="pnlreport" runat="server" 
            GroupingText="Report" __designer:wfdid="w1"><table><tbody><tr><td><asp:Label id="lblreportdate" runat="server" __designer:wfdid="w38" Text="Reservation date"></asp:Label></td><td><asp:TextBox id="txtreportdatefrom" runat="server" Width="104px" __designer:wfdid="w39" AutoPostBack="true" OnTextChanged="txtreportdatefrom_TextChanged"></asp:TextBox></td><td><asp:Label id="lblreportto" runat="server" Width="88px" __designer:wfdid="w40" Text="Reservation to"></asp:Label></td><td><asp:TextBox id="txtreportdateto" runat="server" Width="104px" __designer:wfdid="w41"></asp:TextBox></td></tr><tr><td style="WIDTH: 100px">
                <asp:Label ID="lblreporttype" runat="server" 
                    __designer:wfdid="w35" Text="Reservation type" 
                    Width="104px"></asp:Label>
                </td><td style="WIDTH: 100px">
                    <asp:DropDownList ID="cmbReportpass" runat="server" 
                        __designer:wfdid="w36" Height="22px" 
                        Width="111px">
                        <asp:ListItem Value="-1">Select</asp:ListItem>
                        <asp:ListItem Value="0">Donor Free</asp:ListItem>
                        <asp:ListItem Value="1">Donor Paid</asp:ListItem>                       
                    </asp:DropDownList>
                </td><td style="WIDTH: 100px">
                    &nbsp;</td><td style="WIDTH: 100px">&nbsp;</td></tr>
                <tr>
                    <td style="WIDTH: 100px">
                        <asp:Button ID="btndirectalloclist" runat="server" 
                            __designer:wfdid="w42" CausesValidation="False" 
                            CssClass="btnStyle_large" 
                            onclick="btndirectalloclist_Click" Text="Direct allocation" 
                            Visible="False" />
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:Button ID="btnreservelist" runat="server"  
                            __designer:wfdid="w43" CausesValidation="False" 
                            CssClass="btnStyle_large" onclick="btnreservelist_Click" 
                            Text="Reservation Chart" />
                    </td>
                    <td style="WIDTH: 100px">
                        <asp:Button ID="btnreportclear" runat="server" 
                            __designer:wfdid="w44" CausesValidation="False" 
                            CssClass="btnStyle_medium" onclick="btnreportclear_Click" 
                            Text="Clear" />
                    </td>
                    <td style="WIDTH: 100px">
                        &nbsp;</td>
                </tr>
                </tbody></table></asp:Panel>
        </td></tr><tr>
    <td vAlign=top colSpan=3 rowSpan=1><asp:Panel id="pnlSeasonEdit" runat="server" Width="100%" GroupingText="Pass Season Edit" __designer:wfdid="w2" Visible="False"><table><tbody><tr><td><asp:Label id="Label1" runat="server" Width="124px" Text="Pass no" __designer:wfdid="w17"></asp:Label></td><td></td><td style="WIDTH: 101px"><asp:TextBox id="txtInvalidPass" runat="server" __designer:wfdid="w18"></asp:TextBox></td><td><asp:Label id="lblSeason" runat="server" Text="Season" __designer:wfdid="w19"></asp:Label></td><td>
        <asp:DropDownList id="cmbSeasonforEdit" runat="server" __designer:wfdid="w20" 
            DataValueField="season_id" DataTextField="seasonname"></asp:DropDownList></td></tr><tr><td colSpan=5><asp:GridView id="dgNotValidPass" runat="server" ForeColor="#333333" __designer:wfdid="w1" OnSelectedIndexChanged="dgNotValidPass_SelectedIndexChanged" GridLines="None" CellPadding="4" OnRowCreated="dgNotValidPass_RowCreated" AutoGenerateSelectButton="true">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="true"></FooterStyle><RowStyle BackColor="#EFF3FB"></RowStyle>
<EditrowStyle BackColor="#2461BF"></EditrowStyle><SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="true"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle><HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="true"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle></asp:GridView></td></tr><tr><td colSpan=5><asp:Button id="btnEditSeason" onclick="btnEditSeason_Click" runat="server" CausesValidation="False" Text="Edt Season" CssClass="btnStyle_medium" __designer:wfdid="w2"></asp:Button></td></tr></tbody></table></asp:Panel></td></tr><tr>
 <td vAlign=top colSpan=4 rowSpan=1><asp:Label id="lblpassyear" runat="server" Width="74px" Text="Pass year" __designer:wfdid="w16"></asp:Label> <asp:TextBox id="txtPassYear" runat="server" Width="79px" __designer:wfdid="w17"></asp:TextBox> <asp:Label id="lblpassseason" runat="server" Width="80px" Text="Pass season" __designer:wfdid="w18"></asp:Label> <asp:DropDownList id="cmbseason" runat="server" Width="155px" __designer:wfdid="w19" OnSelectedIndexChanged="cmbseason_SelectedIndexChanged" AutoPostBack="true" DataValueField="season_sub_id" DataTextField="seasonname"><asp:ListItem></asp:ListItem>
</asp:DropDownList> <asp:GridView id="dgreservation" runat="server" Width="850px" HorizontalAlign="Left" ForeColor="#333333" __designer:wfdid="w15" OnSelectedIndexChanged="dgreservation_SelectedIndexChanged" AutoGenerateColumns="False" GridLines="None" CellPadding="4" OnPageIndexChanging="dgreservation_PageIndexChanging" AllowPaging="true" OnRowCreated="dgreservation_RowCreated" DataKeyNames="pass_id">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="true"></FooterStyle><Columns><asp:CommandField SelectText="" ShowSelectButton="true"></asp:CommandField><asp:BoundField DataField="pass_id" Visible="False" HeaderText="pass_id"></asp:BoundField>
<asp:BoundField DataField="PassNo" HeaderText="Pass No"></asp:BoundField><asp:BoundField DataField="PassType" HeaderText="Pass Type"></asp:BoundField>
<asp:BoundField DataField="DonorId" HeaderText="Donor Id"></asp:BoundField><asp:BoundField DataField="PassStatus" Visible="False" HeaderText="Pass Status"></asp:BoundField>
<asp:BoundField DataField="Season" HeaderText="Season"></asp:BoundField><asp:BoundField DataField="DonorName" HeaderText="Donor Name"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building "></asp:BoundField><asp:BoundField DataField="RoomNo" HeaderText="Room No"></asp:BoundField>
</Columns><RowStyle BackColor="#EFF3FB"></RowStyle><EditrowStyle BackColor="#2461BF"></EditrowStyle><SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="true"></SelectedRowStyle><PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="true"></HeaderStyle><AlternatingRowStyle BackColor="White"></AlternatingRowStyle></asp:GridView> </td></tr><tr>
        <td vAlign=middle colSpan=4 rowSpan=2><asp:GridView id="dgReserve" runat="server" Width="849px" HorizontalAlign="Left" ForeColor="#333333" __designer:wfdid="w14" Visible="False" OnSelectedIndexChanged="dgReserve_SelectedIndexChanged" AutoGenerateColumns="False" GridLines="None" CellPadding="4" OnPageIndexChanging="dgReserve_PageIndexChanging" OnRowCreated="dgReserve_RowCreated" DataKeyNames="ReservationNo">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="true"></FooterStyle><Columns><asp:CommandField SelectText="" ShowSelectButton="true"></asp:CommandField>
<asp:BoundField DataField="ReservationNo" HeaderText="Reservation No"></asp:BoundField><asp:BoundField DataField="PassNo" Visible="False" HeaderText="Pass No"></asp:BoundField>
<asp:BoundField DataField="Customer" HeaderText="Customer"></asp:BoundField><asp:BoundField DataField="RoomNo" HeaderText="Room No"></asp:BoundField>
<asp:BoundField DataField="Building" HeaderText="Building"></asp:BoundField><asp:BoundField DataField="ReservedDate" HeaderText="Reserved Date"></asp:BoundField>
<asp:BoundField DataField="ExpectedVecatingDate" HeaderText="Expected Vecating Date"></asp:BoundField></Columns>
<RowStyle BackColor="#EFF3FB"></RowStyle><EditrowStyle BackColor="#2461BF"></EditrowStyle><SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="true"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle><HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="true"></HeaderStyle>
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle></asp:GridView><BR /><BR /><BR /><BR /><BR /><BR /><BR /><BR /></td></tr></tbody></table><table><tbody><tr><td vAlign=top colSpan=3><asp:Panel id="pnlpage" runat="server" Width="125px" __designer:wfdid="w9"><table><tbody><tr><td style="WIDTH: 100px"><asp:Label id="lblpageno" runat="server" Width="52px" Text="Page no" __designer:wfdid="w10"></asp:Label></td><td style="WIDTH: 100px"><asp:TextBox id="txt1" runat="server" Width="50px" __designer:wfdid="w11" Enabled="False">0</asp:TextBox> </td><td style="WIDTH: 100px"><asp:Button id="btnnex" onclick="btnnex_Click" runat="server" Width="100px" CausesValidation="False" Text=">>" Font-Bold="true" __designer:wfdid="w12"></asp:Button></td></tr></tbody></table></asp:Panel></td></tr><tr><td vAlign=top>&nbsp;&nbsp;<asp:RequiredFieldValidator id="rfvroomno" runat="server" Width="104px" ForeColor="White" __designer:wfdid="w110" ControlToValidate="cmbRoom" ErrorMessage="room no required"></asp:RequiredFieldValidator><BR /><BR />&nbsp;<asp:Button 
            id="btnaltrnteroom" runat="server" Width="147px" 
            CausesValidation="False" Text="Take alternate room" 
            __designer:wfdid="w111" Visible="False"></asp:Button>&nbsp; <BR />
        <asp:Button id="btndonorpass" runat="server" Width="144px" 
            CausesValidation="False" Text="Donor pass status" 
            __designer:wfdid="w112" Visible="False"></asp:Button><BR /><cc1:CalendarExtender id="cereportfrom" runat="server" __designer:wfdid="w113" TargetControlID="txtreportdatefrom" Format="dd-MM-yyyy"></cc1:CalendarExtender> <cc1:CalendarExtender id="cereportto" runat="server" __designer:wfdid="w114" TargetControlID="txtreportdateto" Format="dd-MM-yyyy"></cc1:CalendarExtender> <asp:Button id="btnnonoccupncy" onclick="btnnonoccupncy_Click" runat="server" CausesValidation="False" Text="Non ocuupancy" CssClass="btnStyle_large" __designer:wfdid="w115" Visible="False"></asp:Button><BR /><asp:RequiredFieldValidator id="rfvswaminame" runat="server" ForeColor="White" __designer:wfdid="w116" ControlToValidate="txtswaminame" ErrorMessage="Name required"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="rvffromdate" runat="server" ForeColor="White" __designer:wfdid="w117" ControlToValidate="txtfrmdate" ErrorMessage="From date required"></asp:RequiredFieldValidator><BR /><asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ForeColor="White" __designer:wfdid="w118" ControlToValidate="txtfrmdate" ErrorMessage="Format is dd/mm/yyyy" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"></asp:RegularExpressionValidator><BR /><asp:RequiredFieldValidator id="rfvcheckin" runat="server" ForeColor="White" __designer:wfdid="w119" ControlToValidate="txtchkin" ErrorMessage="Check in time required"></asp:RequiredFieldValidator><BR /><BR /><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ForeColor="White" __designer:wfdid="w120" ControlToValidate="txttodate" ErrorMessage="To date required"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="rfvpassno" runat="server" ForeColor="White" __designer:wfdid="w121" ControlToValidate="txtPassNo" ErrorMessage="Pass no required" SetFocusOnError="true"></asp:RequiredFieldValidator><BR /><BR /><asp:RequiredFieldValidator id="rfvbuilding" runat="server" Width="144px" ForeColor="White" __designer:wfdid="w137" ControlToValidate="cmbBuilding" ErrorMessage="Building name required"></asp:RequiredFieldValidator><BR /><asp:RegularExpressionValidator id="revpassno" runat="server" Width="152px" ForeColor="White" __designer:wfdid="w123" ControlToValidate="txtPassNo" ErrorMessage="Only numbers are allowed" ValidationExpression="[0-9]{1,5}"></asp:RegularExpressionValidator><BR /><asp:RequiredFieldValidator id="rfvReason" runat="server" ForeColor="White" __designer:wfdid="w124" ControlToValidate="cmbReason" ErrorMessage="reason required for alternate room"></asp:RequiredFieldValidator><BR /><asp:RequiredFieldValidator id="rfvtodate" runat="server" ForeColor="White" __designer:wfdid="w3" ControlToValidate="txtTodate" ErrorMessage="Todate required"></asp:RequiredFieldValidator><BR /><BR /><BR /><asp:RegularExpressionValidator id="RegMobile" runat="server" ForeColor="White" __designer:wfdid="w21" ControlToValidate="txtMobileNo" ErrorMessage="Invalid Mobile"></asp:RegularExpressionValidator><BR /><BR /><BR /><BR />
<asp:RequiredFieldValidator ID="rfvpasstype" runat="server" ControlToValidate="cmbPasstype" ErrorMessage="PassType required" ForeColor="White"></asp:RequiredFieldValidator>
<BR /><BR /><asp:Panel id="pnldays" runat="server" Width="306px" Height="100%" GroupingText="not in design but using in code" __designer:wfdid="w160" Visible="False"><table><tbody><tr><td><asp:Label id="lblnoofunits" runat="server" __designer:wfdid="w161"></asp:Label></td><td><asp:TextBox id="txtresno" runat="server" Width="110px" Visible="False" __designer:wfdid="w162"></asp:TextBox></td></tr><tr><td><asp:Label id="lblnoofdys" runat="server" Width="86px" Text="No: of days" __designer:wfdid="w163"></asp:Label></td><td><asp:TextBox id="txtnoofdys" runat="server" Width="110px" __designer:wfdid="w164"></asp:TextBox></td></tr><tr><td><asp:Label id="lblrsrvtnchrge" runat="server" Width="113px" Text="Reservation charge" __designer:wfdid="w165"></asp:Label></td><td><asp:TextBox id="txtrservtnchrge" runat="server" Width="109px" __designer:wfdid="w166"></asp:TextBox></td></tr><tr><td><asp:Label id="lbladrs" runat="server" Width="68px" Visible="False" Text="Address" __designer:wfdid="w167"></asp:Label></td><td><asp:TextBox id="txtadrs" runat="server" Width="129px" Visible="False" __designer:wfdid="w168" AutoPostBack="true" OnTextChanged="txtadrs_TextChanged"></asp:TextBox></td></tr><tr><td><asp:TextBox id="txtseason" runat="server" Width="107px" Visible="False" __designer:wfdid="w169" Enabled="False"></asp:TextBox></td><td><asp:TextBox id="txtyear" runat="server" Width="107px" Visible="False" __designer:wfdid="w170" Enabled="False"></asp:TextBox></td></tr><tr><td></td><td><asp:Button id="btnsearch" onclick="btnsearch_Click" runat="server" Width="81px" CausesValidation="False" Text="Search" __designer:wfdid="w171"></asp:Button></td></tr></tbody></table></asp:Panel></td><td style="WIDTH: 329px" vAlign=top>&nbsp;&nbsp;&nbsp;&nbsp;<BR /><BR /> <BR /><asp:Panel id="pnlMessage" runat="server" __designer:dtid="562958543355914" CssClass="ModalWindow" __designer:wfdid="w173" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel6" runat="server" BackColor="LightSteelBlue" __designer:dtid="562958543355915" __designer:wfdid="w174" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Width="197px" Font-Bold="true" Text="Tsunami ARMS - Confirmation" __designer:dtid="562958543355916" __designer:wfdid="w46" ForeColor="MediumBlue"></asp:Label> <asp:Label id="lblHead2" runat="server" Width="191px" Font-Bold="true" Text="Tsunami ARMS - Warning" __designer:wfdid="w56" ForeColor="MediumBlue"></asp:Label><BR /></asp:Panel> <asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px" __designer:dtid="562958543355918" __designer:wfdid="w177"><table style="WIDTH: 237px" __designer:dtid="562958543355919"><tbody><tr __designer:dtid="562958543355920"><td align=center colSpan=1 __designer:dtid="562958543355921"></td><td align=center colSpan=3 __designer:dtid="562958543355922"><BR /><asp:Label id="lblMsg" runat="server" Height="25px" ForeColor="Black" Text="Do you want to save?" __designer:dtid="562958543355923" __designer:wfdid="w48" Font-Size="Small"></asp:Label></td></tr><tr __designer:dtid="562958543355928"><td __designer:dtid="562958543355929"></td><td align=center __designer:dtid="562958543355930">&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" Font-Bold="true" __designer:dtid="562958543355931" CssClass="btnStyle" __designer:wfdid="w179"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" Font-Bold="true" __designer:dtid="562958543355932" CssClass="btnStyle" __designer:wfdid="w180"></asp:Button>&nbsp;</td><td align=center __designer:dtid="562958543355933">&nbsp;</td></tr></tbody></table></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px" __designer:dtid="562958543355938" __designer:wfdid="w181"><table style="WIDTH: 237px" __designer:dtid="562958543355939"><tbody><tr __designer:dtid="562958543355940"><td align=center colSpan=1 __designer:dtid="562958543355941"></td><td align=center colSpan=3 __designer:dtid="562958543355942"><BR /><asp:Label id="lblOk" runat="server" Height="25px" ForeColor="Black" Text="Do you want to ?" __designer:dtid="562958543355943" __designer:wfdid="w52" Font-Size="Small"></asp:Label></td></tr><tr __designer:dtid="562958543355948"><td __designer:dtid="562958543355949"></td><td align=center __designer:dtid="562958543355950">&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="Button3" onclick="Button3_Click" runat="server" Width="50px" CausesValidation="False" ForeColor="Black" Text="OK" Font-Bold="true" __designer:dtid="562958543355951" CssClass="btnStyle" __designer:wfdid="w183"></asp:Button> &nbsp; </td><td align=center __designer:dtid="562958543355952">&nbsp;</td></tr></tbody></table></asp:Panel> <cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" __designer:dtid="562958543355957" __designer:wfdid="w184" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize"></cc1:ModalPopupExtender> <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden" __designer:dtid="562958543355909" __designer:wfdid="w185"></asp:Button></asp:Panel></td><td style="WIDTH: 329px" vAlign=top>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <cc1:ListSearchExtender id="ListSearchExtenderbuild" runat="server" __designer:wfdid="w145" TargetControlID="cmbBuilding"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtenderroom" runat="server" __designer:wfdid="w146" TargetControlID="cmbRoom"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="donorstate" runat="server" __designer:wfdid="w147" TargetControlID="cmbDnrstate"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="donordistrict" runat="server" __designer:wfdid="w148" TargetControlID="cmbDstrct"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="state" runat="server" __designer:wfdid="w149" TargetControlID="cmbState"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="district" runat="server" __designer:wfdid="w150" TargetControlID="cmbDistrict"></cc1:ListSearchExtender>&nbsp; <BR /><cc1:FilteredTextBoxExtender id="FilteredTextBoxPassno" runat="server" __designer:wfdid="w153" TargetControlID="txtPassNo" FilterType="Numbers"></cc1:FilteredTextBoxExtender> <cc1:FilteredTextBoxExtender id="FilteredTextBoxswaminame" runat="server" __designer:wfdid="w154" TargetControlID="txtswaminame" FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars=". "></cc1:FilteredTextBoxExtender> <cc1:FilteredTextBoxExtender id="FilteredTextBoxplace" runat="server" __designer:wfdid="w155" TargetControlID="txtplace" FilterType="UppercaseLetters, LowercaseLetters"></cc1:FilteredTextBoxExtender> <cc1:FilteredTextBoxExtender id="FilteredTextBoxstd" runat="server" __designer:wfdid="w156" TargetControlID="txtstd" FilterType="Numbers" ValidChars="+"></cc1:FilteredTextBoxExtender> <cc1:FilteredTextBoxExtender id="FilteredTextBoxphone" runat="server" __designer:wfdid="w157" TargetControlID="txtphn" FilterType="Numbers"></cc1:FilteredTextBoxExtender>&nbsp;&nbsp;&nbsp;&nbsp; </td></tr>
        <tr>
            <td vAlign="top">
                &nbsp;</td>
            <td style="WIDTH: 329px" vAlign="top">
                <asp:Label ID="lblmessage" runat="server" 
                    __designer:wfdid="w37" ForeColor="Red" 
                    Text="Select all essential fields" Visible="False" 
                    Width="152px"></asp:Label>
            </td>
            <td style="WIDTH: 329px" vAlign="top">
                <asp:Label ID="lblextraamt" runat="server" 
                    __designer:wfdid="w26" Text="Extra amount" Visible="False" 
                    Width="81px"></asp:Label>
                <asp:TextBox ID="txtextraamt" runat="server" 
                    OnTextChanged="txtextraamt_TextChanged" ReadOnly="true" 
                    Visible="False" Width="130px">0</asp:TextBox>
            </td>
        </tr>
        </tbody></table>
</contenttemplate><triggers><asp:PostBacktrigger ControlID="btnreservelist" /><asp:PostBacktrigger ControlID="btnnonoccupncy" />
<asp:PostBacktrigger ControlID="btndirectalloclist" /><asp:PostBacktrigger ControlID="btnprint" /><asp:PostBacktrigger ControlID="btnsave" />
<asp:PostBacktrigger ControlID="btnnext" /></triggers></asp:UpdatePanel></asp:Content>

