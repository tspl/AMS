<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reservation alterations.aspx.cs" Inherits="Reservation_alterations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 78%; height: 457px;">
                <tr>
                    <td colspan="7">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="7">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="7">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="7">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="swamipanel0" runat="server" GroupingText="Swami Details" 
                            Height="1%" Width="325px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="WIDTH: 162px; HEIGHT: 2px">
                                            <asp:Label ID="Label103" runat="server" Text="Reserve No."></asp:Label>
                                        </td>
                                        <td style="WIDTH: 284px; HEIGHT: 2px">
                                            <asp:TextBox ID="txtReserveNo" runat="server" AutoPostBack="True" Height="17px" 
                                                ontextchanged="txtReserveNo_TextChanged" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 162px; HEIGHT: 2px">
                                            <asp:Label ID="lblswaminame0" runat="server" Font-Bold="False" 
                                                Text="Swami name" Width="78px"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 284px; HEIGHT: 2px">
                                            <asp:TextBox ID="txtswaminame" runat="server" AutoPostBack="True" 
                                                CssClass="UpperCaseFirstLetter" Height="17px" tabIndex="5" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 162px; HEIGHT: 23px">
                                            <asp:Label ID="Label104" runat="server" Text="Place" Width="82px"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 284px; HEIGHT: 23px">
                                            <asp:TextBox ID="txtplace" runat="server" AutoPostBack="True" 
                                                CssClass="UpperCaseFirstLetter" Height="17px" tabIndex="6" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <BR />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 162px; HEIGHT: 14px">
                                            <asp:Label ID="lblstate0" runat="server" Text="State" Width="79px"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 284px; HEIGHT: 14px">
                                            <asp:DropDownList ID="cmbState" runat="server" AppendDataBoundItems="True" 
                                                AutoPostBack="True" DataTextField="statename" DataValueField="state_id" 
                                                Height="22px" tabIndex="20" Width="205px" Enabled="False">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <BR />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 162px; HEIGHT: 26px">
                                            <asp:Label ID="Label105" runat="server" Text="District" Width="80px"></asp:Label>
                                            <BR />
                                        </td>
                                        <td style="WIDTH: 284px; HEIGHT: 26px">
                                            <asp:DropDownList ID="cmbDists" runat="server" DataTextField="districtname" 
                                                DataValueField="district_id" Height="22px" tabIndex="20" Width="205px" 
                                                Enabled="False">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="HEIGHT: 26px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 162px">
                                            <asp:Label ID="lblphone0" runat="server" Text="Phone" Width="79px"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 284px">
                                            <asp:TextBox ID="txtphone" runat="server" Height="17px" tabIndex="20" 
                                                Width="200px" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td>
                                            <BR />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 162px; HEIGHT: 31px">
                                            <asp:Label ID="lblidproof0" runat="server" Text="Identity proof" Width="79px"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 284px; HEIGHT: 31px">
                                            <asp:DropDownList ID="cmbIDp" runat="server" DataTextField="idproof" 
                                                DataValueField="pid" Height="22px" tabIndex="20" Width="205px" 
                                                Enabled="False">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <BR />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 162px; HEIGHT: 20px">
                                            <asp:Label ID="Label106" runat="server" Text="Identity ref: no" Width="83px"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 284px; HEIGHT: 20px">
                                            <asp:TextBox ID="txtidrefno" runat="server" EnableTheming="True" Height="17px" 
                                                tabIndex="20" Width="200px" Enabled="False"></asp:TextBox>
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
                    <td colspan="3">
                        <asp:Panel ID="roomallocationpanel" runat="server" 
                            GroupingText="Room Allocation Details" Height="1%" Width="323px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblbuildingname" runat="server" Text="Building name" 
                                                Width="87px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbBuild" runat="server" AutoPostBack="True" 
                                                DataTextField="buildingname" DataValueField="build_id" Height="22px" 
                                                tabIndex="13" Width="175px" Enabled="False">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbroomno" runat="server" Text="Room no" Width="74px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbRooms" runat="server" AutoPostBack="True" 
                                                DataTextField="roomno" DataValueField="room_id" tabIndex="14" 
                                                Width="175px" Enabled="False">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="height: 24px">
                                            <asp:Label ID="lblProposedCheckOutDate" runat="server" 
                                                style="POSITION: relative" Text="Check out date" Width="92px"></asp:Label>
                                        </td>
                                        <td style="height: 24px">
                                            <asp:TextBox ID="txtcheckout" runat="server" AutoPostBack="True" 
                                                style="POSITION: relative; top: 0px; left: 0px; height: 17px;" tabIndex="16" 
                                                Width="170px" ontextchanged="txtcheckout_TextChanged"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtcheckout_CalendarExtender" runat="server" 
                                                Enabled="True" Format="dd/MM/yyyy" 
                                                TargetControlID="txtcheckout" >
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td style="height: 24px">
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
                                                Height="17px" style="LEFT: 0px; POSITION: relative; top: 0px;" 
                                                tabIndex="17" Width="170px" ontextchanged="txtcheckouttime_TextChanged"></asp:TextBox>
                                        </td>
                                        <td style="height: 24px">
                                            &nbsp;&nbsp;</td>
                                    </tr>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label101" runat="server" Text="No of Hours"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txthours" runat="server" ReadOnly="True" Width="170px" 
                                                Enabled="False"></asp:TextBox>
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
                                                Height="17px" style="LEFT: 0px; POSITION: relative" 
                                                tabIndex="57" Width="170px" ontextchanged="txtcheckindate_TextChanged"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtcheckindate_CalendarExtender" runat="server" 
                                                 Enabled="True" Format="dd/MM/yyyy" 
                                                TargetControlID="txtcheckindate" >
                                            </cc1:CalendarExtender>
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
                                                Height="17px" style="POSITION: relative; top: 0px; left: 0px;" tabIndex="56" 
                                                Width="170px" ontextchanged="txtcheckintime_TextChanged"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                    <td colspan="2">
                        <asp:Panel ID="rentpanel" runat="server" GroupingText="Rent" Height="1%" 
                            Width="246px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="WIDTH: 100px; HEIGHT: 26px">
                                            <asp:Label ID="lblroomrent" runat="server" Text="Room rent" Width="73px"></asp:Label>
                                        </td>
                                        <td style="WIDTH: 92px; HEIGHT: 26px">
                                            <asp:TextBox ID="txtroomrent" runat="server" Enabled="False" Font-Bold="True" 
                                                Height="17px" tabIndex="16" 
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
                                                Font-Bold="True" Height="17px" 
                                                tabIndex="19" Width="90px" ontextchanged="txtothercharge_TextChanged"></asp:TextBox>
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
                                            <asp:TextBox ID="txtadvance" runat="server" Height="17px" tabIndex="20" 
                                                Width="90px" AutoPostBack="True" ontextchanged="txtadvance_TextChanged"></asp:TextBox>
                                        </td>
                                        <td style="WIDTH: 92px; HEIGHT: 18px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 100px; HEIGHT: 18px">
                                            <b>Balance payable</b>
                                        </td>
                                        <td style="WIDTH: 92px; HEIGHT: 18px">
                                            <asp:TextBox ID="txtnetpayable" runat="server" Enabled="False" 
                                                Font-Size="X-Large" ForeColor="OliveDrab" Height="33px" Width="90px" 
                                                Wrap="False"></asp:TextBox>
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
                    <td width="20%">
                        &nbsp;</td>
                    <td align="right" colspan="2" width="20%">
                        <asp:Button ID="btnalter" runat="server" CssClass="btnStyle_medium" 
                            Font-Bold="True" tabIndex="18" Text="Alter" onclick="btnalter_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="center" width="20%">
                        <asp:Button ID="btnchange" runat="server" CssClass="btnStyle_medium" 
                            Font-Bold="True" onclick="btnchange_Click" tabIndex="18" Text="Change room" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    <td colspan="2" width="20%">
                        <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_medium" 
                            Font-Bold="True" onclick="btnclear_Click" tabIndex="18" Text="Clear" />
                    </td>
                    <td width="20%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="7"><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel8" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="Label22" runat="server" Width="238px" ForeColor="MediumBlue" Text="Tsunami ARMS - Confirmation" Font-Bold="True"></asp:Label></asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize"></ajaxToolkit:ModalPopupExtender>&nbsp; <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button>&nbsp; <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD style="WIDTH: 227px; HEIGHT: 18px" align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 15px"></TD><TD style="WIDTH: 208px; HEIGHT: 15px"></TD><TD style="WIDTH: 13px; HEIGHT: 15px"></TD></TR><TR><TD style="HEIGHT: 26px"></TD><TD style="WIDTH: 208px; HEIGHT: 26px" align=center>&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="WIDTH: 13px; HEIGHT: 26px" align=center>&nbsp;</TD></TR><TR><TD style="HEIGHT: 18px"></TD><TD style="WIDTH: 208px; HEIGHT: 18px" align=center></TD><TD style="WIDTH: 13px; HEIGHT: 18px" align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD style="WIDTH: 224px" align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="7" align="center">
                        <asp:Panel ID="pnlalternate" runat="server" GroupingText="Alternate Room" 
                            Visible="False" Width="44%">
                            <TABLE>
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
                                                DataTextField="roomno" DataValueField="room_id" Height="22px" tabIndex="28" 
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 100px">
                                        </td>
                                        <td style="WIDTH: 99px">
                                        </td>
                                        <td>
                                            <asp:Button ID="btnchangeroom" runat="server" CausesValidation="False" 
                                                Font-Bold="True" onclick="btnchangeroom_Click" style="height: 26px" 
                                                tabIndex="29" Text="Change room" />
                                        </td>
                                    </tr>
                                </tbody>
                            </TABLE>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="7" style="height: 24px">
                        </td>
                </tr>
                <tr>
                    <td colspan="7">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="7">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="7">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="7">
                        &nbsp;</td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

