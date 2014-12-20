<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="vacating and billing.aspx.cs" Inherits="vacating_and_billing" Title="Tsunami ARMS-Vacating and billing" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentplaceholder3" Runat="Server"><strong><span style="font-size: 14pt">
<style>
         .modalBackground
{
background-color: Gray;
filter: alpha(opacity=80);
opacity: 0.8;
z-index: 10000;
}
</style>
<asp:Panel ID="Panel10" runat="server" GroupingText="User Tips" Height="50px" Width="217px"><br />
    <br />
    </asp:Panel><br />
</span></strong>
    <script language="javascript" type="text/javascript">
        function frame1_onclick() {
        }
    </script>
</asp:Content><asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager><asp:UpdatePanel id="UpdatePanel1" runat="server"><contenttemplate>
<TABLE><TBODY><TR><TD colspan="2"><strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<SPAN style="FONT-SIZE: 12pt"> &nbsp; &nbsp; Vacating and Billing</SPAN></strong></TD></TR><TR><TD vAlign=top colspan="2"><asp:Panel id="Panel6" runat="server" Width="100%" GroupingText="Counter Details"><TABLE><TBODY><TR><TD style="WIDTH: 52px"><asp:Label id="lblcashierliability" runat="server" Width="96px" Height="17px" Text="Counter Liability" __designer:wfdid="w35"></asp:Label></TD><TD style="WIDTH: 82px"><asp:TextBox id="txtLiability" tabIndex=-1 runat="server" Width="100px" Height="20px" ForeColor="#400040" Font-Bold="True" Enabled="false " OnTextChanged="txtliability_TextChanged" __designer:wfdid="w36"></asp:TextBox></TD><TD style="WIDTH: 100px" align=right>
<asp:Label id="Label5" runat="server" Text="SecurityDeposit" 
        __designer:wfdid="w37"></asp:Label></TD><td style="WIDTH: 100px" align="right"><asp:TextBox id="txtDeposit" tabIndex="-1" runat="server" Width="100px" Height="20px" ForeColor="#400040" Font-Bold="True" Enabled="False" OnTextChanged="txtdeposit_TextChanged" __designer:wfdid="w38"></asp:TextBox></TD><TD style="WIDTH: 81px"><asp:Label id="lblresstartno" runat="server" Width="67px" Text=" Receipt No" __designer:wfdid="w39"></asp:Label></TD><TD style="WIDTH: 72px"><asp:TextBox id="txtStartRecieptNo" tabIndex=-1 runat="server" Width="100px" Height="17px" ForeColor="#400040" Font-Bold="True" AutoPostBack="True" Enabled="False" OnTextChanged="txtstartresno_TextChanged" __designer:wfdid="w40"></asp:TextBox></TD><TD style="WIDTH: 72px"><asp:Label id="Label9" runat="server" Width="75px" Text="Rec Balance" __designer:wfdid="w41"></asp:Label></TD><TD style="WIDTH: 72px"><asp:TextBox id="txtRecieptBalance" tabIndex=-1 runat="server" Width="100px" ForeColor="#400040" Font-Bold="True" AutoPostBack="True" Enabled="False" OnTextChanged="txtrecbala_TextChanged" __designer:wfdid="w42"></asp:TextBox></TD><TD><asp:LinkButton id="lnkEditRecieptno" tabIndex=-1 onclick="lnkEditRecieptno_Click" runat="server" CausesValidation="False" __designer:wfdid="w43">Edit</asp:LinkButton></TD>  <td style="WIDTH: 100px">
                        &nbsp;</td>
                    <td style="WIDTH: 100px">
                        &nbsp;</td></TR>
    <tr>
        <td style="WIDTH: 52px">
            Payment mode</td>
        <td style="WIDTH: 82px">
            <asp:DropDownList ID="ddlpayment" runat="server" AutoPostBack="True" 
                Height="20px" Width="100px">
            </asp:DropDownList>
        </td>
        <td align="right" style="WIDTH: 100px">
            &nbsp;</td>
        <td align="right" style="WIDTH: 100px">
            &nbsp;</td>
        <td style="WIDTH: 81px">
            <asp:Label ID="Label34" runat="server" Text="Counter Deposit" Width="103px"></asp:Label>
        </td>
        <td style="WIDTH: 72px">
            <asp:TextBox ID="txtcounterdeposit" runat="server" Font-Bold="True" 
                Font-Size="Small" tabIndex="54" Width="100px"></asp:TextBox>
        </td>
        <td style="WIDTH: 72px">
            <asp:Label ID="Label35" runat="server" Text="Uncliamed " Width="103px"></asp:Label>
        </td>
        <td style="WIDTH: 72px">
            <asp:TextBox ID="txtunclaimed" runat="server" Font-Bold="True" 
                Font-Size="Small" tabIndex="54" Width="100px"></asp:TextBox>
        </td>
        <td>
            &nbsp;</td>
        <td style="WIDTH: 100px">
            &nbsp;</td>
        <td style="WIDTH: 100px">
            &nbsp;</td>
    </tr>
    </TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="HEIGHT: 28px" vAlign=top colSpan=2><asp:Label id="Label16" runat="server" Text="Check Out Method"></asp:Label> 
    <asp:DropDownList id="cmbCheckOutMehtod" tabIndex=-1 runat="server" 
        AutoPostBack="True" 
        OnSelectedIndexChanged="cmbCheckOutMehtod_SelectedIndexChanged" Height="22px"><asp:ListItem>Normal</asp:ListItem>
<asp:ListItem>Force Vacating</asp:ListItem><asp:ListItem>Overstay</asp:ListItem><asp:ListItem>Alternate Room</asp:ListItem>
    <asp:ListItem>Extended Stay</asp:ListItem>
        <asp:ListItem>Inmates Add</asp:ListItem>
</asp:DropDownList> <asp:Button id="btnRoomAlloc" tabIndex=-1 onclick="btnRoomAlloc_Click" runat="server" CausesValidation="False" Text="Room Allocation" CssClass="btnStyle_large"></asp:Button> <asp:CheckBox id="chkOldReceipt" runat="server" AutoPostBack="True" __designer:wfdid="w1" OnCheckedChanged="chkOldReceipt_CheckedChanged"></asp:CheckBox></TD></TR><TR><TD vAlign=top><asp:Panel id="Panel1" runat="server" Width="100%" Height="100%" GroupingText="Customer Details"><TABLE><TBODY><TR><TD style="WIDTH: 5px; HEIGHT: 26px"><asp:Label id="lblrecieptno" runat="server" Width="105px" Text="Adv Rec No"></asp:Label></TD><TD style="WIDTH: 107px; HEIGHT: 26px"><asp:TextBox id="txtAdRecieptNo" runat="server" Width="130px" AutoPostBack="True" OnTextChanged="txtrecieptno_TextChanged" OnUnload="txtAdRecieptNo_Unload"></asp:TextBox></TD></TR><TR><TD style="HEIGHT: 26px"><asp:Label id="Label1" runat="server" Width="57px" Text="Client Id"></asp:Label></TD><TD style="WIDTH: 107px; HEIGHT: 26px"><asp:TextBox id="txtClientid" tabIndex=1 runat="server" Width="130px" AutoPostBack="true" OnTextChanged="txtclientid_TextChanged"></asp:TextBox> </TD></TR><TR><TD><asp:Label id="lblbuildingname" runat="server" Width="84px" Text="Building Name "></asp:Label></TD><TD style="WIDTH: 107px; HEIGHT: 24px"><asp:DropDownList id="cmbBuilding" tabIndex=2 runat="server" Width="136px" AutoPostBack="True" OnSelectedIndexChanged="cmbBuilding_SelectedIndexChanged2" DataValueField="build_id" DataTextField="buildingname"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD><asp:Label id="lblroomno" runat="server" Width="92px" Text="Room Number"></asp:Label></TD><TD style="WIDTH: 107px; HEIGHT: 26px"><asp:TextBox id="txtRoomNo" tabIndex=3 runat="server" Width="132px" AutoPostBack="true" CssClass="UpperCaseFirstLetter" OnTextChanged="txtroomno_TextChanged"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 5px"><asp:Label id="lblcustomername" runat="server" Width="88px" Text="Swami Name"></asp:Label></TD><TD style="WIDTH: 107px"><asp:TextBox id="txtCustomerName" tabIndex=-1 runat="server" Width="130px" AutoPostBack="True" CssClass="UpperCaseFirstLetter" OnTextChanged="txtcustomername_TextChanged"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 5px; HEIGHT: 26px"><asp:Label id="txtcustaddress" runat="server" Width="105px" Text="Swami's Place"></asp:Label></TD><TD style="WIDTH: 107px; HEIGHT: 26px"><asp:TextBox id="txtCustomerPlace" tabIndex=-1 runat="server" Width="130px" AutoPostBack="True" CssClass="UpperCaseFirstLetter" OnTextChanged="txtcustomerplace_TextChanged"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 5px; HEIGHT: 26px"><asp:Label id="Label10" runat="server" Text="District"></asp:Label></TD><TD style="WIDTH: 107px; HEIGHT: 26px"><asp:TextBox id="txtDistrict" tabIndex=-1 runat="server" Width="130px" OnTextChanged="txtDistrict_TextChanged"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 5px; HEIGHT: 26px"><asp:Label id="lablelno" runat="server" Width="70px" Text="Alloc No"></asp:Label></TD><TD style="WIDTH: 107px; HEIGHT: 26px"><asp:TextBox id="txtAllocNo" tabIndex=-1 runat="server" Width="131px"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 5px"><asp:Label id="lblalltime" runat="server" Width="94px" Text="Allocated Time"></asp:Label></TD><TD style="WIDTH: 107px"><asp:TextBox id="txtAllocatedTime" tabIndex=-1 runat="server" Width="130px" OnTextChanged="txtAllocatedTime_TextChanged1"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 5px; HEIGHT: 26px"><asp:Label id="lblalldate" runat="server" Width="89px" Text="Allocated Date"></asp:Label></TD><TD style="WIDTH: 107px; HEIGHT: 26px"><asp:TextBox id="txtAllocatedDate" tabIndex=-1 runat="server" Width="130px"></asp:TextBox></TD></TR></TBODY></TABLE></asp:Panel><asp:Panel id="pnlalternate" runat="server" Width="100%" Height="1%" GroupingText="Alternate Room" Visible="False"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 24px"><asp:Label id="Label19" runat="server" Width="82px" Text="Alternate Building"></asp:Label></TD><TD style="HEIGHT: 24px"><asp:DropDownList id="cmbaltbulilding" runat="server" Width="127px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbaltbulilding_SelectedIndexChanged" DataTextField="buildingname" DataValueField="build_id"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 3px"><asp:Label id="Label20" runat="server" Text="Alternate Room"></asp:Label></TD><TD style="HEIGHT: 3px"><asp:DropDownList id="cmbaltroom" runat="server" Width="127px" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="cmbaltroom_SelectedIndexChanged" DataTextField="roomno" DataValueField="room_id"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="Label25" runat="server" Text="Reason"></asp:Label></TD><TD><asp:DropDownList id="CmbReason" runat="server" Width="126px" OnSelectedIndexChanged="CmbReason_SelectedIndexChanged" DataTextField="reason" DataValueField="reason_id"></asp:DropDownList></TD></TR></TBODY></TABLE></asp:Panel> <asp:HiddenField id="HiddenField1" runat="server" Value="0"></asp:HiddenField> <asp:CheckBox id="chkplain" tabIndex=-1 runat="server" Width="138px" Text="Plain paper Printing" OnCheckedChanged="chkplain_CheckedChanged" Visible="False"></asp:CheckBox></TD><TD style="WIDTH: 659px" vAlign=top><asp:Panel id="Panel2" runat="server" Width="100%" Height="84%" GroupingText="Vacating Details"><TABLE><TBODY><TR>
        <TD vAlign=middle style="height: 19px"><asp:Label id="Label28" runat="server" Width="122px" Text="Proposed Check Out "></asp:Label></TD>
        <TD style="height: 19px"><asp:TextBox id="txtPropCheckOut" tabIndex=-1 runat="server" Width="128px" ForeColor="#400040" Font-Bold="False" Enabled="False"></asp:TextBox></TD>
        <TD style="height: 19px"></TD><TD rowSpan=6 width="50%">
        <asp:Panel ID="pnlPenality" runat="server" GroupingText="Penality Details" 
            Height="100%" Width="100%">
            <table>
                <tbody>
                    <tr>
                        <td width="60%">
                            <asp:Label ID="Label4" runat="server" Height="1px" Text="Key Returned Status" 
                                Width="130px"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdoKeyReturn" runat="server" AutoPostBack="True" 
                                OnSelectedIndexChanged="rdoKeyReturn_SelectedIndexChanged" 
                                RepeatDirection="Horizontal" tabIndex="-1" Width="107px">
                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="HEIGHT: 26px">
                            <asp:Label ID="lblPenalityKey" runat="server" Text="Penalty" Visible="False"></asp:Label>
                        </td>
                        <td style="WIDTH: 100px; HEIGHT: 26px">
                            <asp:TextBox ID="txtKeynotReturnCharge" runat="server" AutoPostBack="True" 
                                OnTextChanged="txtKeynotReturnCharge_TextChanged" tabIndex="-1" Visible="False" 
                                Width="122px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px">
                            <asp:Label ID="Label6" runat="server" Text="Room In Good Condition"></asp:Label>
                        </td>
                        <td style="height: 25px">
                            <asp:RadioButtonList ID="rdoRoomCondition" runat="server" AutoPostBack="True" 
                                OnSelectedIndexChanged="rdoRoomCondition_SelectedIndexChanged" 
                                RepeatDirection="Horizontal" tabIndex="-1">
                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="WIDTH: 100px">
                            <asp:Label ID="lblPealityDamage" runat="server" Text="Penalty" Visible="False"></asp:Label>
                        </td>
                        <td style="WIDTH: 100px">
                            <asp:TextBox ID="txtRoomNotGoodCondition" runat="server" AutoPostBack="True" 
                                OnTextChanged="txtRoomNotGoodCondition_TextChanged" tabIndex="-1" 
                                Visible="False" Width="123px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Complaints"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdoComplaints" runat="server" AutoPostBack="True" 
                                Height="7px" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" 
                                RepeatDirection="Horizontal" tabIndex="-1" Width="112px">
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem Selected="True">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        </TD></TR><TR><TD valign="middle"><asp:Label id="lblvecatingtime" runat="server" Width="84px" Text="Vacating Date"></asp:Label></TD><TD><asp:TextBox id="txtVacatingDate" tabIndex="-1" runat="server" Width="130px" Height="20px" ForeColor="#400040" Font-Bold="False" AutoPostBack="true" Enabled="False" OnTextChanged="txtvecdate_TextChanged"></asp:TextBox></TD><TD></TD></TR><TR>
        <TD valign=top style="height: 16px"><asp:Label id="lblvacatingtime" runat="server" Width="89px" Text="Vacating Time"></asp:Label></TD>
        <TD style="height: 16px"><asp:TextBox id="txtVecatingTime" tabIndex=-1 runat="server" Width="130px" ForeColor="#400040" Font-Bold="False" AutoPostBack="true" Enabled="False" OnTextChanged="txtvecatingtime_TextChanged"></asp:TextBox></TD>
        <TD style="height: 16px"><asp:LinkButton id="lnkDateEdit" tabIndex=-1 onclick="lnkDateEdit_Click" runat="server" CausesValidation="False">Edit</asp:LinkButton></TD></TR><TR>
        <TD valign=top style="height: 17px">
        <asp:Label id="lblstay" runat="server" Width="71px" Text="No of Hours"></asp:Label></TD>
        <TD style="height: 17px"><asp:TextBox id="txtDaysStayed" tabIndex=-1 runat="server" Width="130px" ForeColor="#400040" Font-Bold="False" Enabled="False"></asp:TextBox></TD>
        <TD style="height: 17px">
        <asp:Label ID="lblhrs" runat="server" Visible="False"></asp:Label>
        </TD></TR><TR><TD vAlign=top><asp:Label id="lbladvpaidamt" runat="server" width="105px" Text="Adv Paid Amount"></asp:Label></TD>
            <TD><asp:TextBox id="txtAdvanceAmount" tabIndex=-1 runat="server" Width="130px" ForeColor="#400040" Font-Bold="False" Enabled="False" OnTextChanged="txtAdvanceAmount_TextChanged1"></asp:TextBox></TD><TD></TD></TR><TR><TD vAlign=top><asp:Label id="Label8" runat="server" Width="89px" Text="Gross Amount"></asp:Label></TD><TD style="HEIGHT: 16px"><asp:TextBox id="txtGrossAmount" tabIndex=-1 runat="server" Width="130px" ForeColor="#400040" Font-Bold="False" Enabled="False"></asp:TextBox></TD><TD></TD></TR><TR>
        <TD vAlign=top style="height: 23px">&nbsp;<asp:Label id="lblbalanceindicator" runat="server" Width="102px" Text="Balance  Indicator"></asp:Label></TD>
        <TD style="height: 23px"><asp:Button id="btnBalanceIndicator" tabIndex=-1 runat="server" Width="136px" Height="38px" CausesValidation="False" ForeColor="White" Text="balance indicator"></asp:Button></TD>
        <TD style="height: 23px"></TD>
        <td rowspan="3" width="50%">
            <asp:Panel ID="pnlinmate" runat="server" GroupingText="Inmate Details " 
                Width="100%">
                <table style="width: 100%">
                    <tr>
                        <td width="40%">
                            <asp:Label ID="Label37" runat="server" Text="Inmate Advance"></asp:Label>
                        </td>
                        <td width="60%">
                            <asp:TextBox ID="txtinmateadvance" runat="server" Width="120px" 
                                ontextchanged="txtinmateadvance_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="40%">
                            <asp:Label ID="Label38" runat="server" Text="Inmate Gross"></asp:Label>
                        </td>
                        <td width="60%">
                            <asp:TextBox ID="txtinmategross" runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="40%">
                            <asp:Label ID="Label39" runat="server" Text="Inmate Balance"></asp:Label>
                        </td>
                        <td width="60%">
                            <asp:TextBox ID="txtinmatebal" runat="server" Font-Size="X-Large" 
                                ForeColor="Red" Height="27px" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
        </TR><TR><TD style="HEIGHT: 24px" vAlign=top><asp:Label id="lblbalanceamt" 
                runat="server" width="133px" Text="Deposit"></asp:Label></TD>
            <TD style="HEIGHT: 24px"><asp:TextBox id="txtBalanceAmount" tabIndex=-1 
                    runat="server" Width="130px" Height="25px" ForeColor="Red" Font-Bold="True" 
                    Enabled="False" Font-Size="X-Large" 
                    ontextchanged="txtBalanceAmount_TextChanged"></asp:TextBox></TD>
            <TD style="HEIGHT: 24px"></TD></TR>
        <tr>
            <td style="HEIGHT: 30px" valign="top">
                <asp:Label ID="lblbalanceamt0" runat="server" Text="Total Balance Amt" 
                    width="133px"></asp:Label>
            </td>
            <td style="HEIGHT: 30px">
                <asp:TextBox ID="txttotalbal" runat="server" Enabled="False" Font-Bold="True" 
                    Font-Size="X-Large" ForeColor="Red" Height="31px" tabIndex="-1" Width="130px"></asp:TextBox>
            </td>
            <td style="HEIGHT: 30px">
            </td>
        </tr>
        </TBODY></TABLE></asp:Panel>
        <asp:Panel id="pnlExtend" runat="server" Width="100%" Height="100%" 
            GroupingText="Overstay Details" Visible="False"><TABLE><TBODY><TR><TD style="height: 26px">
                <asp:Label ID="Label46" runat="server" Text="No of Inmates"></asp:Label>
                </TD><TD style="height: 26px">
                    <asp:TextBox ID="txtinmno" runat="server" AutoPostBack="True" 
                        OnTextChanged="txtinmno_TextChanged" tabIndex="-1" Width="100px"></asp:TextBox>
                </TD>
                <TD colSpan=1 style="height: 26px">
                    &nbsp;</TD>
                <TD colSpan=1 style="height: 26px">
                    &nbsp;</TD><TD colSpan=1 style="height: 26px">&nbsp;</TD></TR><TR>
                <TD style="height: 26px">
                    <asp:Label ID="Label26" runat="server" Text="Alloc Reciept No"></asp:Label>
                </TD><TD style="height: 26px">
                    <asp:TextBox ID="txtAllocRecNo" runat="server" AutoPostBack="True" 
                        Enabled="False" OnTextChanged="txtAllocRecNo_TextChanged" tabIndex="-1" 
                        Width="100px"></asp:TextBox>
                </TD>
                <TD colSpan=1 style="height: 26px">
                    <asp:Label ID="Label27" runat="server" Text="Alloc No"></asp:Label>
                </TD>
                <TD colSpan=1 style="height: 26px">
                    <asp:TextBox ID="txtAllocNumber" runat="server" Enabled="False" tabIndex="-1" 
                        Width="100px" ontextchanged="txtAllocNumber_TextChanged"></asp:TextBox>
                </TD><TD colSpan=1 style="height: 26px"></TD></TR><TR><TD>
                <asp:Label id="Label17" runat="server" Width="92px" Text="Extended Date"></asp:Label></TD>
                <TD><asp:TextBox id="txtExtendDate" runat="server" Width="100px" 
                        AutoPostBack="True" OnTextChanged="txtExtendDate_TextChanged"></asp:TextBox></TD><TD colSpan=1>
                    <asp:Label ID="Label43" runat="server" Text="No of Units"></asp:Label>
                </TD><TD colSpan=1>
                    <asp:TextBox ID="txtNoofDays" runat="server" Width="100px"></asp:TextBox>
                </TD><TD colSpan=1></TD></TR><TR><TD>
                <asp:Label id="Label18" runat="server" Text="Extended Time" Width="100px"></asp:Label></TD>
                <TD><asp:TextBox id="txtExtendTime" runat="server" Width="100px" 
                        AutoPostBack="True" OnTextChanged="txtExtendTime_TextChanged" 
                        Height="22px">3:00 PM</asp:TextBox></TD><TD colSpan=1>
                    <asp:Label ID="Label42" runat="server" Text="Deposit"></asp:Label>
                </TD><TD colSpan=1>
                    <asp:TextBox ID="txtDepositAlloc" runat="server" tabIndex="-1" Width="100px"></asp:TextBox>
                </TD><TD colSpan=1>
                    <asp:Label ID="lblrent" runat="server" __designer:wfdid="w1" Font-Bold="True" 
                        Font-Size="Large" ForeColor="Red" Text="Label" Width="100%"></asp:Label>
                </TD></TR>
                <tr>
                    <td>
                        <asp:Label ID="Label44" runat="server" Text="Inmate Charge"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtinmcharge" runat="server" Width="100px" 
                            ontextchanged="txtinmcharge_TextChanged"></asp:TextBox>
                    </td>
                    <td colspan="1">
                        <asp:Label ID="Label41" runat="server" Text="Rent"></asp:Label>
                    </td>
                    <td colspan="1">
                        <asp:TextBox ID="txtRentAlloc" runat="server" tabIndex="-1" Width="100px"></asp:TextBox>
                    </td>
                    <td colspan="1">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label45" runat="server" Text="Inmate Deposit"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtinmdeposit" runat="server" Width="100px"></asp:TextBox>
                    </td>
                    <td colspan="1">
                        <asp:Label ID="Label40" runat="server" Text="Adv Amount" Width="72px"></asp:Label>
                    </td>
                    <td colspan="1">
                        <asp:TextBox ID="txtAdvAmount" runat="server" tabIndex="-1" Width="100px" 
                            ontextchanged="txtAdvAmount_TextChanged"></asp:TextBox>
                    </td>
                    <td colspan="1">
                        &nbsp;</td>
                </tr>
                </TBODY></TABLE><cc1:CalendarExtender id="CalendarExtender4" runat="server" 
                TargetControlID="txtExtendDate" Format="dd-MM-yyyy"></cc1:CalendarExtender></asp:Panel></TD></TR><TR><TD style="HEIGHT: 33px; TEXT-ALIGN: center" colSpan=2>&nbsp; <asp:Button id="btnCheckout" tabIndex=4 onclick="btncheckout_Click" runat="server" ForeColor="Black" Text="Checkout" CssClass="btnStyle_small"></asp:Button>&nbsp;&nbsp; <asp:Button id="btnBillPrint" tabIndex=5 onclick="btnbill_Click" runat="server" ForeColor="Black" Text="Bill print" CssClass="btnStyle_small" Visible="False"></asp:Button> <asp:Button id="btnClear" tabIndex=6 onclick="btnClear_Click1" runat="server" CausesValidation="False" ForeColor="Black" Text="Clear" CssClass="btnStyle_small"></asp:Button> <asp:Button id="btnEdit" tabIndex=7 onclick="btnedit_Click1" runat="server" CausesValidation="False" ForeColor="Black" Text="Edit" CssClass="btnStyle_small" Visible="False"></asp:Button> <asp:Button id="btnSave" onclick="btnsave_Click1" runat="server" CausesValidation="False" ForeColor="Black" Text="Save" CssClass="btnStyle_small" Enabled="False"></asp:Button><asp:Button id="btnAbscending" tabIndex=-1 onclick="btnabscending_Click" runat="server" ForeColor="Black" Text="Inmates Absconding" CssClass="btnStyle_large" Visible="False"></asp:Button>&nbsp;&nbsp; <asp:Button id="btnPrinterOnOff" onclick="btnPrinterOnOff_Click" runat="server" CausesValidation="False" ForeColor="Black" Text="Printer on/off" CssClass="btnStyle_large" Visible="False" Enabled="False"></asp:Button> <asp:Button id="btnReport" tabIndex=8 onclick="btnreport_Click" runat="server" CausesValidation="False" Text="Report" CssClass="btnStyle_small"></asp:Button> <asp:Button id="btnExecutive" tabIndex=9 onclick="btnExecutive_Click" runat="server" CausesValidation="False" Text="Executive Override" CssClass="btnStyle_large"></asp:Button></TD></TR><TR><TD><BR />&nbsp;<BR />&nbsp;<asp:RequiredFieldValidator id="RequiredFieldValidator9" runat="server" Width="96px" ForeColor="WhiteSmoke" InitialValue="-1" ControlToValidate="CmbReason" ErrorMessage="Select Reason"></asp:RequiredFieldValidator>&nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender21" runat="server" TargetControlID="RequiredFieldValidator9"></cc1:ValidatorCalloutExtender></TD><TD style="WIDTH: 659px"><asp:Panel id="pnlLogin" runat="server" Width="46%" Height="1%" GroupingText="Login" BackColor="#C0C0FF" Visible="False"><TABLE><TBODY><TR>
    <td colspan="2">&nbsp;<asp:RadioButton ID="rdodeposit" runat="server" 
            AutoPostBack="True" OnCheckedChanged="rdodeposit_CheckedChanged" Text="Deposit" 
            Visible="False" />
        <asp:RadioButton ID="rdoKey" runat="server" AutoPostBack="True" 
            OnCheckedChanged="rdoKey_CheckedChanged" Text="Key" Visible="False" />
        <asp:RadioButton ID="rbgrace" runat="server" AutoPostBack="True" 
            oncheckedchanged="rbgrace_CheckedChanged" Text="Grace time" />
        &nbsp;<asp:RadioButton ID="rostay" runat="server" AutoPostBack="True" 
            oncheckedchanged="rostay_CheckedChanged" Text="Overstay" />
    </td>
    </tr><tr><td style="WIDTH: 47px"><asp:Label id="Label2" runat="server" Text="Username"></asp:Label></td>
        <td style="WIDTH: 214px"><asp:TextBox id="txtUsername" runat="server" Width="150px"></asp:TextBox></td></tr><tr><td style="WIDTH: 47px"><asp:Label id="Label3" runat="server" Text="Password"></asp:Label></td>
    <td style="WIDTH: 214px"><asp:TextBox id="txtPassword" runat="server" Width="151px" TextMode="Password"></asp:TextBox></td></tr><tr><td></td>
    <td style="width: 214px"><asp:Button id="btnLogin" onclick="btnlogin_Click" runat="server" Width="72px" CausesValidation="False" Text="Submit" CssClass="btnStyle_small"></asp:Button></td></tr></TBODY></TABLE></asp:Panel>&nbsp; </td></tr><tr><td colspan=2><asp:Panel id="pnlReports" runat="server" Width="100%" Height="0%" GroupingText="Reports" Visible="False"><TABLE><TBODY><TR><TD colSpan=2><asp:Label id="Label11" runat="server" Width="76px" Text="From date" Font-Bold="True" __designer:wfdid="w38"></asp:Label><asp:TextBox id="txtFromDate" runat="server" Width="138px" AutoPostBack="True" __designer:wfdid="w39" OnTextChanged="txtfromdate_TextChanged"></asp:TextBox></TD><TD style="WIDTH: 8070px" colSpan=1></TD></TR><TR><TD style="HEIGHT: 10px" colSpan=2>&nbsp;<asp:Label id="Label12" runat="server" Width="69px" Text="To date" Font-Bold="True" __designer:wfdid="w40"></asp:Label><asp:TextBox id="txtToDate" runat="server" Width="136px" AutoPostBack="True" __designer:wfdid="w41" OnTextChanged="txttodate_TextChanged"></asp:TextBox></TD><TD style="HEIGHT: 10px" colSpan=1>&nbsp; <asp:Label id="Label29" runat="server" Text="Day close date" Font-Bold="True" __designer:wfdid="w1"></asp:Label> <asp:TextBox id="txtDaycloseDate" runat="server" __designer:wfdid="w9"></asp:TextBox></TD></TR><TR><TD colSpan=2><asp:Panel id="Panel4" runat="server" Width="100%" Height="100%" GroupingText="Report" Font-Bold="True"><TABLE><TBODY><TR><TD><asp:LinkButton id="lnkDayWiseReport" onclick="lnkConsolidatedIncome_Click" runat="server" Width="183px" CausesValidation="False" Font-Bold="True">  Day Wise Collection Report</asp:LinkButton></TD><TD></TD></TR><TR><TD><asp:LinkButton id="lnkConsolidatedIncomeReport" onclick="lnkConsolidatedIncomeReport_Click" runat="server" Width="226px" CausesValidation="False" Font-Bold="True">Total  Daily Collection Comparison</asp:LinkButton></TD><TD></TD></TR><TR><TD><asp:LinkButton id="lnkDayWiseVacatedRooms" onclick="lnkDayWiseVacatedRooms_Click" runat="server" Width="168px" CausesValidation="False" Font-Bold="True">Day wise Vacating Rooms</asp:LinkButton></TD><TD></TD></TR><TR><TD><asp:LinkButton id="lnkInmatesabscondlist" onclick="lnkInmatesabscondlist_Click" runat="server" Width="144px" CausesValidation="False" Font-Bold="True">Inmates abconding list</asp:LinkButton></TD><TD></TD></TR><TR><TD><asp:LinkButton id="lnkKeyLostInventory" onclick="lnkKeyLostInventory_Click" runat="server" Width="174px" CausesValidation="False" Font-Bold="True">Key Lost Inventory Ledger</asp:LinkButton></TD><TD></TD></TR><TR><TD><asp:LinkButton id="lnlcountercollection" onclick="LinkButton6_Click1" runat="server" Width="134px" CausesValidation="False">Counter collection</asp:LinkButton></TD><TD></TD></TR></TBODY></TABLE> </asp:Panel> </TD><TD style="WIDTH: 8070px" colSpan=1><asp:Panel id="pnlLedgerReports" runat="server" Width="41%" Height="73%" GroupingText="Ledger Reports" Font-Bold="True" __designer:wfdid="w3"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:LinkButton id="lnkUnclaimedSecutityLedger" onclick="lnkUnclaimedSecutityLedger_Click" runat="server" Width="148px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w4">UnclaimedSecurityLedger</asp:LinkButton></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkUnclaimedExcel" onclick="lnkUnclaimedExcel_Click" runat="server" CausesValidation="False" __designer:wfdid="w3">Excel</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 100px"><asp:LinkButton id="lnkDepositLedger" onclick="lnkDepositLedger_Click" runat="server" Width="141px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w5">Security Deposit Ledger</asp:LinkButton></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:LinkButton id="lnkRoomDamageLedger" onclick="lnkRoomDamageLedger_Click" runat="server" Width="134px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w6">Room Damage Ledger</asp:LinkButton></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkExcelDamage" onclick="lnkExcelDamage_Click" runat="server" CausesValidation="False" __designer:wfdid="w1">Excel</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkKeyLostChargeLedger" onclick="lnkKeyLostChargeLedger_Click" runat="server" Width="158px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w7">Key Lost Charge Ledger</asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:LinkButton id="lnkKeylostExcel" onclick="lnkKeylostExcel_Click" runat="server" CausesValidation="False" __designer:wfdid="w2">Excel</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 100px"><asp:LinkButton id="lnkOverStayLedger" onclick="lnkOverStayLedger_Click" runat="server" Width="156px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w8">Over Stay Ledger</asp:LinkButton></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:LinkButton id="lnkOverstayExcel" onclick="lnkOverstayExcel_Click" runat="server" CausesValidation="False" __designer:wfdid="w3">Excel</asp:LinkButton></TD></TR><TR><TD><asp:LinkButton id="lnkKeyLostReport" onclick="lnkKeyLostReport_Click" runat="server" CausesValidation="False" __designer:wfdid="w10">Key Lost Report</asp:LinkButton></TD><TD style="WIDTH: 100px"></TD><TD><asp:LinkButton id="lnkExcelKey" onclick="lnkExcelKey_Click" runat="server" CausesValidation="False" __designer:wfdid="w11">Excel</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=2><asp:Panel id="Panel3" runat="server" Width="54%" Height="100%" GroupingText="Donor and Care taker Liability" Font-Bold="True" __designer:wfdid="w10"><TABLE><TBODY><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:Label id="Label30" runat="server" Text="Building" Font-Bold="True" __designer:wfdid="w42"></asp:Label></TD></TR><TR><TD style="WIDTH: 100px"><asp:LinkButton id="lnkCareTakerLiability" onclick="lnkCareTakerLiability_Click" runat="server" Width="173px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w11">Care Taker Liability</asp:LinkButton></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbBuildReport2" runat="server" Width="144px" __designer:wfdid="w13" DataValueField="build_id" DataTextField="buildingname"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 100px"><asp:LinkButton id="lnkDonorLiability" onclick="lnkDonorLiability_Click" runat="server" Width="86px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w14">DonorLiability</asp:LinkButton></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD></TR></TBODY></TABLE></asp:Panel></TD><TD style="WIDTH: 8070px" colSpan=1><asp:Panel id="Panel9" runat="server" Width="100%" Height="100%" GroupingText="Non Vacating report" Font-Bold="True" __designer:wfdid="w15"><TABLE style="WIDTH: 361px"><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:LinkButton id="lnkDueVacatingReports" onclick="lnkDueVacatingReports_Click" runat="server" Width="180px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w16">Due Vacating Reports for current day for </asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:Label id="Label15" runat="server" Text="Time" Font-Bold="True" __designer:wfdid="w17"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:TextBox id="txtTime" runat="server" Width="104px" __designer:wfdid="w18"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px"><asp:LinkButton id="lnkDueVacatingMaxtime" onclick="lnkDueVacatingMaxtime_Click" runat="server" Width="206px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w19">Due for vacating at max stay   period  of current day</asp:LinkButton></TD><TD style="WIDTH: 100px"><asp:Label id="Label31" runat="server" Text="Building" Font-Bold="True" __designer:wfdid="w20"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="cmbSelectBuilding" runat="server" Width="148px" __designer:wfdid="w21" DataValueField="build_id" DataTextField="buildingname"><asp:ListItem>All</asp:ListItem>
</asp:DropDownList></TD></TR><tr><td style="WIDTH: 100px; HEIGHT: 18px"><asp:LinkButton id="lnkNonvacateWhole" onclick="lnkNonvacateWhole_Click" runat="server" Width="166px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w22">All Non vacating Rooms</asp:LinkButton></TD><TD style="WIDTH: 100px; HEIGHT: 18px"></TD><TD style="WIDTH: 100px; HEIGHT: 18px"></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=2><asp:Panel id="Panel11" runat="server" Width="100%" Height="100%" GroupingText="Room Status &Transaction report " Font-Bold="True" __designer:wfdid="w23"><TABLE><TBODY><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px" rowSpan=1></TD><TD style="WIDTH: 7218206px" rowSpan=1><asp:Label id="Label32" runat="server" Width="124px" Text="Building Name" Font-Bold="True" __designer:wfdid="w43"></asp:Label></TD></TR><TR><TD style="WIDTH: 100px"><asp:LinkButton id="lnkCompleteRoomStatusReport" onclick="lnkCompleteRoomStatusReport_Click" runat="server" Width="168px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w28">Complete Room Status Report</asp:LinkButton></TD><TD style="WIDTH: 100px" rowSpan=3></TD><TD vAlign=top rowSpan=3>&nbsp;<asp:DropDownList id="cmbCompleteBuilding" runat="server" __designer:wfdid="w34" DataTextField="buildingname" DataValueField="build_id"><asp:ListItem Value="-1">All</asp:ListItem>
</asp:DropDownList></TD></TR><tr><td style="WIDTH: 100px"><asp:LinkButton id="lnkHourlyTransaction" onclick="lnkHourlyTransaction_Click1" runat="server" Width="171px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w30">Hourly Room Transaction List</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 100px"><asp:LinkButton id="lnkHourlyTransactionRoomList" onclick="lnkHourlyTransactionRoomList_Click" runat="server" Width="185px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w31">Hourly Trascation Room List</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel></TD><TD style="WIDTH: 8070px" colSpan=1><asp:Button id="btnCloseReport" onclick="btnCloseReport_Click" runat="server" CausesValidation="False" Text="Close Report" CssClass="btnStyle_large" __designer:wfdid="w36"></asp:Button> <cc1:CalendarExtender id="CalendarExtender3" runat="server" __designer:wfdid="w37" TargetControlID="txtDaycloseDate" Format="dd-MM-yyyy"></cc1:CalendarExtender></TD></TR><TR><TD colSpan=3><TABLE><TBODY><TR><TD style="WIDTH: 254px" colSpan=3><asp:LinkButton id="LinkButton4" onclick="LinkButton4_Click" runat="server" Width="163px" CausesValidation="False" Font-Bold="True">Deposit amount paid  report</asp:LinkButton></TD><TD style="WIDTH: 254px" colSpan=1></TD></TR><TR><TD style="WIDTH: 254px" colSpan=3><asp:LinkButton id="lnkExecutKeyReturn" onclick="lnkExecutKeyReturn_Click" runat="server" Width="229px" Height="1px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w45">Executive Override Key Penality Report</asp:LinkButton></TD><TD style="WIDTH: 254px" colSpan=1></TD></TR><TR><TD style="WIDTH: 254px" colSpan=3></TD><TD style="WIDTH: 254px" colSpan=1><asp:LinkButton id="lnkExecutivePay" onclick="lnkExecutivePay_Click" runat="server" Width="225px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w2">Executive  Override Deposit Pay Report</asp:LinkButton></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>&nbsp;<BR />&nbsp; &nbsp; &nbsp;&nbsp; </asp:Panel> <asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" Width="106px" ForeColor="WhiteSmoke" ControlToValidate="txtTime" ErrorMessage="hh:mm AM/PM" ValidationExpression="[0-9]{1,2}[:]{1}[ 0-6]{1}[0-9]{1}[ ]{1,3}[PM,AM,pm,am]{1,2}"></asp:RegularExpressionValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RegularExpressionValidator2"></cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator10" runat="server" ForeColor="WhiteSmoke" InitialValue="0" ControlToValidate="txtAllocRecNo" ErrorMessage="Enter valid Receipt no"></asp:RequiredFieldValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender22" runat="server" TargetControlID="RequiredFieldValidator10"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD colSpan=2><asp:Panel id="Panel5" runat="server" Width="100%" GroupingText="Today's  vacating room details ">&nbsp;<TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:CheckBox id="chkselectall" tabIndex=9 runat="server" Width="196px" Text="Show all occupied  room list" AutoPostBack="true" __designer:wfdid="w1" OnCheckedChanged="CheckBox1_CheckedChanged"></asp:CheckBox><BR /><asp:LinkButton id="lnkRoomsPreviousHours" tabIndex=10 onclick="rooms_Click" runat="server" Width="249px" CausesValidation="False" __designer:wfdid="w2" Visible="False">Rooms due for vacating on previous hours</asp:LinkButton><BR /><asp:LinkButton id="lnkRoomsSucceedingHour" tabIndex=11 onclick="LinkButton6_Click" runat="server" Width="255px" CausesValidation="False" __designer:wfdid="w3" Visible="False">rooms due for vacating in succeeding hours</asp:LinkButton></TD><TD></TD></TR><TR><TD colSpan=2><asp:GridView id="dtgRoomVacateDetails" runat="server" Width="665px" Height="1px" ForeColor="#333333" OnSelectedIndexChanged="dtgRoomVacateDetails_SelectedIndexChanged" __designer:wfdid="w4" AutoGenerateColumns="False" GridLines="None" CellPadding="4" OnSorting="GridView1_Sorting" AllowSorting="True" AllowPaging="True" OnRowCreated="GridView1_RowCreated" OnPageIndexChanging="GridView1_PageIndexChanging">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle><Columns><asp:BoundField DataField="adv_recieptno" HeaderText="Receitpt No"></asp:BoundField><asp:BoundField DataField="buildingname" HeaderText="Building "></asp:BoundField>
<asp:BoundField DataField="roomno" HeaderText="Room No"></asp:BoundField><asp:BoundField DataField="swaminame" HeaderText="Swami Name"></asp:BoundField>
<asp:BoundField DataField="vacatedate" HeaderText="Expect Vacate Date"></asp:BoundField></Columns><RowStyle BackColor="#EFF3FB"></RowStyle>
<EditRowStyle BackColor="#2461BF"></EditRowStyle><SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle><HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle><AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=2><asp:Panel id="Panel7" runat="server" Width="100%" Height="100%" GroupingText="customer details"><TABLE><TBODY><TR><TD style="WIDTH: 100px" vAlign=top><asp:GridView id="dtgCustomerDetails" runat="server" Width="712px" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" ForeColor="#333333" AutoGenerateColumns="False" GridLines="None" CellPadding="4" OnSorting="GridView2_Sorting" AllowSorting="True" AllowPaging="True" OnRowCreated="GridView2_RowCreated" OnPageIndexChanging="GridView2_PageIndexChanging">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle><Columns><asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="swaminame" HeaderText="Swami Name"></asp:BoundField><asp:BoundField DataField="place" HeaderText="Place"></asp:BoundField>
<asp:BoundField DataField="districtname" HeaderText="District"></asp:BoundField><asp:BoundField DataField="buildingname" HeaderText="Building"></asp:BoundField>
<asp:BoundField DataField="roomno" HeaderText="Room No"></asp:BoundField><asp:BoundField DataField="allocdate" HeaderText="Check in Date"></asp:BoundField>
</Columns><RowStyle BackColor="#EFF3FB"></RowStyle><EditRowStyle BackColor="#2461BF"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle><AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=2>&nbsp;<BR /> <cc1:ListSearchExtender id="ListSearchExtender1" runat="server" TargetControlID="cmbBuilding"></cc1:ListSearchExtender> <cc1:ListSearchExtender id="ListSearchExtender2" runat="server" TargetControlID="cmbSelectBuilding"></cc1:ListSearchExtender> </TD></TR>
    <tr>
        <td colspan="2">
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
    <TR><TD colSpan=2>&nbsp;</TD></TR><TR><TD style="HEIGHT: 1px" colSpan=2>&nbsp;&nbsp; <asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" Width="71px" Height="1px" ForeColor="Snow" ControlToValidate="txtVecatingTime" ErrorMessage="hh:mm AM/PM" ValidationExpression="[0-9]{1,2}[:]{1}[ 0-6]{1}[0-9]{1}[ ]{1,3}[PM,AM,pm,am]{1,2}" Font-Size="Smaller"></asp:RegularExpressionValidator> <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" Width="67px" Height="1px" ForeColor="Snow" ControlToValidate="txtVacatingDate" ErrorMessage="dd/mm/yy" ValidationExpression="^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$" Font-Size="Larger"></asp:RegularExpressionValidator>&nbsp;&nbsp; <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RegularExpressionValidator1"></cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RegularExpressionValidator3"></cc1:ValidatorCalloutExtender> <cc1:FilteredTextBoxExtender id="FilteredTextBoxExtender1" runat="server" TargetControlID="txtcustomername" FilterType="Custom, UppercaseLetters, LowercaseLetters" InvalidChars=". ">
</cc1:FilteredTextBoxExtender> <asp:RegularExpressionValidator id="RegularExpressionValidator4" runat="server" ForeColor="LightCyan" ControlToValidate="txtRoomNo" ErrorMessage="Enter correct roomno" ValidationExpression="[1-9]\d{0,6}"></asp:RegularExpressionValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" TargetControlID="RegularExpressionValidator4">
</cc1:ValidatorCalloutExtender> <asp:RegularExpressionValidator id="RegularExpressionValidator5" runat="server" ForeColor="Snow" ControlToValidate="txtAdRecieptNo" ErrorMessage="Enter correct reciept no" ValidationExpression="[1-9]\d{0,12}"></asp:RegularExpressionValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" TargetControlID="RegularExpressionValidator5">
</cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" TargetControlID="RegularExpressionValidator6">
</cc1:ValidatorCalloutExtender> <asp:RegularExpressionValidator id="RegularExpressionValidator6" runat="server" Width="138px" ForeColor="Snow" ControlToValidate="txtStartRecieptNo" ErrorMessage="Enter correct reciept no" ValidationExpression="[1-9]\d{0,12}"></asp:RegularExpressionValidator> <asp:RegularExpressionValidator id="RegularExpressionValidator7" runat="server" ForeColor="Snow" ControlToValidate="txtRecieptBalance" ErrorMessage="Enter correct count" ValidationExpression="[0-9]\d{0,8}"></asp:RegularExpressionValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" TargetControlID="RegularExpressionValidator7">
</cc1:ValidatorCalloutExtender> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender8" runat="server" TargetControlID="RegularExpressionValidator8">
</cc1:ValidatorCalloutExtender> <asp:RegularExpressionValidator id="RegularExpressionValidator8" runat="server" ForeColor="Snow" ControlToValidate="txtLiability" ErrorMessage="enter correct amount" ValidationExpression="[0-9]\d{0,12}"></asp:RegularExpressionValidator> <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="96px" ForeColor="Snow" ControlToValidate="txtAdRecieptNo" ErrorMessage="Enter reciept no"></asp:RequiredFieldValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender9" runat="server" TargetControlID="RequiredFieldValidator1">
</cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Width="87px" ForeColor="Azure" InitialValue="-1" ControlToValidate="cmbBuilding" ErrorMessage="Select building"></asp:RequiredFieldValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender10" runat="server" TargetControlID="RequiredFieldValidator2">
</cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Width="98px" ForeColor="Snow" ControlToValidate="txtRoomNo" ErrorMessage="Enter room no"></asp:RequiredFieldValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender11" runat="server" TargetControlID="RequiredFieldValidator3">
</cc1:ValidatorCalloutExtender> <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ForeColor="LightCyan" ControlToValidate="txtCustomerName" ErrorMessage="Enter customer name"></asp:RequiredFieldValidator> <cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender12" runat="server" TargetControlID="RequiredFieldValidator4">
</cc1:ValidatorCalloutExtender> <cc1:FilteredTextBoxExtender id="FilteredTextBoxExtender2" runat="server" TargetControlID="txtcustomername" FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars=". ">
</cc1:FilteredTextBoxExtender>&nbsp;&nbsp;&nbsp; <cc1:CalendarExtender id="CalendarExtender1" runat="server" TargetControlID="txttodate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:CalendarExtender id="CalendarExtender2" runat="server" TargetControlID="txtfromdate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel8" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Text="Tsunami ARMS - Confirmation" ForeColor="MediumBlue" Font-Bold="True"></asp:Label></asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" TargetControlID="btnHidden" RepositionMode="RepositionOnWindowResize" PopupControlID="pnlMessage">
</ajaxToolkit:ModalPopupExtender> <asp:TextBox id="TextBox1" runat="server" AutoPostBack="True"></asp:TextBox> <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button> <asp:Label id="Label13" runat="server" Text="Label"></asp:Label><BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 26px" align=center colSpan=1></TD><TD style="HEIGHT: 26px" align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD></TD><TD></TD><TD style="WIDTH: 13px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="40px" CausesValidation="False" Text="Yes"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="40px" CausesValidation="False" Text="No"></asp:Button>&nbsp;</TD><TD style="WIDTH: 13px" align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD style="WIDTH: 13px" align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="144px" Height="89px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" Width="198px" Height="30px" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD></TD><TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK"></asp:Button></TD><TD></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlok2" runat="server" Width="125px" Height="162px"><TABLE style="WIDTH: 245px; HEIGHT: 112px"><TBODY><TR><TD style="HEIGHT: 2px" align=center colSpan=1></TD><TD style="HEIGHT: 2px" align=center colSpan=3><asp:Label id="msg2" runat="server" Height="30px" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD></TD><TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="ok2" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK"></asp:Button></TD><TD></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;</TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlYesNo2" runat="server" Width="125px" Height="136px" Visible="False"><TABLE><TBODY><TR><TD align=center colSpan=4><asp:Label id="Label14" runat="server" Width="162px" Text="Where to add liability ?"></asp:Label></TD></TR><TR><TD colSpan=4></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="btnYes1" onclick="btnYes1_Click" runat="server" Text="Donor" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="btnNo2" onclick="btnNo2_Click" runat="server" Text="Care Taker" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="btnNo3" onclick="btnNo3_Click" runat="server" Text="Cashier" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD></TR></TBODY></TABLE></asp:Panel></asp:Panel>&nbsp;&nbsp; </TD></TR></TBODY></TABLE>
</contenttemplate><Triggers><asp:PostBackTrigger ControlID="LinkButton4" /><asp:PostBackTrigger controlID="lnkDayWiseReport"/>
<asp:PostBackTrigger ControlID="lnkConsolidatedIncomeReport" /><asp:PostBackTrigger ControlID="lnkExecutivePay" />
<asp:PostBackTrigger ControlID="lnkExecutKeyReturn" /><asp:PostBackTrigger ControlID="lnkDueVacatingReports" />
<asp:PostBackTrigger ControlID="lnkDepositLedger" /><asp:PostBackTrigger ControlID="lnkDueVacatingMaxtime" />
<asp:PostBackTrigger ControlID="lnkDonorLiability" /><asp:PostBackTrigger ControlID="btnYes" /><asp:PostBackTrigger ControlID="lnkDayWiseVacatedRooms" />
<asp:PostBackTrigger ControlID="lnkOverStayLedger" /><asp:PostBackTrigger ControlID="btnOk" /><asp:PostBackTrigger ControlID="lnkInmatesabscondlist" />
<asp:PostBackTrigger ControlID="lnkKeyLostInventory" /><asp:PostBackTrigger ControlID="lnkNonvacateWhole" />
<asp:PostBackTrigger ControlID="lnkKeyLostChargeLedger" /><asp:PostBackTrigger ControlID="lnkUnclaimedSecutityLedger" />
<asp:PostBackTrigger ControlID="lnkRoomDamageLedger" /><asp:PostBackTrigger ControlID="lnkCareTakerLiability" />
<asp:PostBackTrigger ControlID="lnkConsolidatedIncomeReport" /><asp:PostBackTrigger ControlID="lnlcountercollection" />
<asp:PostBackTrigger ControlID="lnkCompleteRoomStatusReport" /><asp:PostBackTrigger ControlID="lnkHourlyTransaction" />
<asp:PostBackTrigger ControlID="lnkHourlyTransactionRoomList" /><asp:PostBackTrigger ControlID="lnkUnclaimedExcel" />
<asp:PostBackTrigger ControlID="lnkExcelDamage" /><asp:PostBackTrigger ControlID="lnkKeylostExcel" /><asp:PostBackTrigger ControlID="lnkOverstayExcel" />
</Triggers></asp:UpdatePanel><iframe id="frame1" runat="server" style="width: 2px; height: 1px" onclick="return frame1_onclick()"></iframe>
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

