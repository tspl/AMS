<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reports New.aspx.cs" Inherits="Reports_New" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        <p>
            Use&nbsp; <strong>Tab Key </strong>or <strong>Mouse Click</strong>,
        To go to the Next Field. &nbsp;</p>       
    </asp:Panel>
    <strong><span style="text-decoration: underline"></span></strong>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel id="UpdatePanel1" runat="server">
<ContentTemplate>
<table><tbody><tr><td>
    <asp:Panel id="pnlledger" runat="server" Width="100%" GroupingText="Ledger Reports" __designer:wfdid="w12"><TABLE width="100%"><TBODY><TR><TD><asp:Label id="Label19" runat="server" Width="60px" Text="From date" __designer:wfdid="w1"></asp:Label></TD><TD>
        <asp:TextBox ID="txtfromd" runat="server" __designer:wfdid="w2" tabIndex="41" 
            Width="90px"></asp:TextBox>
        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" 
            TargetControlID="txtfromd">
        </cc1:CalendarExtender>
        </TD><TD><asp:Label id="Label20" runat="server" Width="48px" Text="To date" __designer:wfdid="w3"></asp:Label></TD><TD>
        <asp:TextBox ID="txttod" runat="server" __designer:wfdid="w4" tabIndex="42" 
            Width="90px" Height="22px"></asp:TextBox>
        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" 
            TargetControlID="txttod">
        </cc1:CalendarExtender>
        </TD><TD style="WIDTH: 278px"><asp:LinkButton id="lnktotalallocseasonreport" tabIndex=43 onclick="lnktotalallocseasonreport_Click" runat="server" Width="301px" Height="20px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w5" Font-Size="Small">Accommodation ledger report between dates</asp:LinkButton></TD><TD><asp:LinkButton id="lnkAccLedBetExcel" onclick="lnkAccLedBetExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w4">.Excel</asp:LinkButton></TD></TR><TR><TD style="HEIGHT: 26px"><asp:Label id="Label7" runat="server" Width="60px" Text="Counter" __designer:wfdid="w1"></asp:Label></TD><TD><asp:DropDownList id="cmbcounter" runat="server" Width="100px" __designer:wfdid="w2" DataValueField="counter_id" DataTextField="counter_ip"></asp:DropDownList></TD><TD><asp:Label id="Label21" runat="server" Width="59px" Text="Date" __designer:wfdid="w6"></asp:Label></TD><TD>
        <asp:TextBox ID="txtdate" runat="server" __designer:wfdid="w7" tabIndex="39" 
            Width="90px"></asp:TextBox>
        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy" 
            TargetControlID="txtdate">
        </cc1:CalendarExtender>
        </TD><TD style="WIDTH: 278px"><asp:LinkButton id="lnkDonorPaidRoomAllocationReport" 
                tabIndex=40 onclick="lnkDonorPaidRoomAllocationReport_Click" runat="server" 
                Width="265px" CausesValidation="False" Font-Bold="True" __designer:wfdid="w8" 
                Font-Size="Small" style="height: 20px">Accommodation ledger  on  current date</asp:LinkButton></TD><TD><asp:LinkButton id="lnkAccLedExcel" onclick="lnkAccLedExcel_Click" runat="server" Font-Bold="True" __designer:wfdid="w3">.Excel</asp:LinkButton></TD></TR><TR>
        <TD style="height: 24px">
            <asp:Label ID="lblseasoncomp" runat="server" Text="Season"></asp:Label>
        </TD>
        <td style="height: 24px">
            <asp:DropDownList ID="cmbseasoncomp" runat="server" Width="100px" 
                DataTextField="seasonname" 
                DataValueField="season_sub_id">
            </asp:DropDownList>
        </td>
        <TD style="height: 24px">
            <asp:Label ID="lblyearcomp" runat="server" Text="Year"></asp:Label>
        </TD>
        <td style="height: 24px">
            <asp:DropDownList ID="cmbyearcomp" runat="server" Width="100px" 
                DataTextField="mal_year" DataValueField="mal_year_id">
            </asp:DropDownList>
        </td>
        <TD style="height: 24px">
        <asp:LinkButton ID="lnkCollectionCompare" runat="server" Font-Bold="True" 
            onclick="lnkCollectionCompare_Click" >Collection Comparison Report</asp:LinkButton>
        </TD><td style="height: 24px"></td></tr>                      
        <tr>
            <td style="height: 24px">
                &nbsp;</td>
            <td style="height: 24px">
                &nbsp;</td>
            <td style="height: 24px">
                &nbsp;</td>
            <td style="height: 24px">
                &nbsp;</td>
            <td style="height: 24px">
                &nbsp;</td>
            <td style="height: 24px">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="height: 24px">
                <asp:Label ID="Label23" runat="server" __designer:wfdid="w1" Text="From date" 
                    Width="60px"></asp:Label>
            </td>
            <td style="height: 24px">
                <asp:TextBox ID="txtfromd0" runat="server" __designer:wfdid="w2" tabIndex="41" 
                    Width="90px"></asp:TextBox>
                <cc1:CalendarExtender ID="txtfromd0_CalendarExtender" runat="server" 
                    Format="dd/MM/yyyy" TargetControlID="txtfromd0">
                </cc1:CalendarExtender>
            </td>
            <td style="height: 24px">
                <asp:Label ID="Label24" runat="server" __designer:wfdid="w3" Text="To date" 
                    Width="48px"></asp:Label>
            </td>
            <td style="height: 24px">
                <asp:TextBox ID="txttod0" runat="server" __designer:wfdid="w4" Height="22px" 
                    tabIndex="42" Width="90px"></asp:TextBox>
                <cc1:CalendarExtender ID="txttod0_CalendarExtender" runat="server" 
                    Format="dd/MM/yyyy" TargetControlID="txttod0">
                </cc1:CalendarExtender>
            </td>
            <td style="height: 24px">
                <asp:LinkButton ID="lnkonline" runat="server" __designer:wfdid="w5" 
                    CausesValidation="False" Font-Bold="True" Font-Size="Small" Height="20px" 
                    onclick="lnkonline_Click" tabIndex="43" Width="301px">Online reserved and 
                allocated list
                </asp:LinkButton>
            </td>
            <td style="height: 24px">
                &nbsp;</td>
        </tr>
        </tbody></table></asp:Panel>
 </td></tr><tr><td><asp:Panel id="pnlreport" runat="server" GroupingText="Report"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 24px"><asp:Label id="lblreporttype" runat="server" Width="104px" __designer:wfdid="w35" Text="Reservation type"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 24px">
        <asp:DropDownList id="cmbReportpass" runat="server" 
            Width="111px" Height="22px" __designer:wfdid="w36"><asp:ListItem Value="-1">All</asp:ListItem>
<asp:ListItem>Donor Free</asp:ListItem>
<asp:ListItem>Donor Paid</asp:ListItem>
<asp:ListItem>Tdb</asp:ListItem>
</asp:DropDownList></TD><TD style="HEIGHT: 24px" colSpan=2><asp:Label id="lblmessage" runat="server" Width="152px" __designer:wfdid="w37" Text="Select all essential fields" Visible="False" ForeColor="Red"></asp:Label></TD>
        <td>
        </td>
        </TR><TR><TD><asp:Label id="lblreportdate" runat="server" Text="Reservation date"></asp:Label></TD><TD>
            <asp:TextBox ID="txtreportdatefrom" runat="server" 
                AutoPostBack="True" OnTextChanged="txtreportdatefrom_TextChanged" Width="104px"></asp:TextBox>
            <cc1:CalendarExtender ID="cereportfrom" runat="server"  
                Format="dd-MM-yyyy" TargetControlID="txtreportdatefrom">
            </cc1:CalendarExtender>
            </TD><TD><asp:Label id="lblreportto" runat="server" Width="88px"  Text="Reservation to"></asp:Label></TD><TD>
            <asp:TextBox ID="txtreportdateto" runat="server" 
                Width="104px"></asp:TextBox>
            <cc1:CalendarExtender ID="cereportto" runat="server"  
                Format="dd-MM-yyyy" TargetControlID="txtreportdateto">
            </cc1:CalendarExtender>
            </TD><td>
                <asp:LinkButton ID="lnkreservelist" runat="server" Font-Bold="True" 
                    onclick="lnkreservelist_Click">Room Reservation Chart</asp:LinkButton>
        </td></TR><TR><TD style="WIDTH: 100px">
            <asp:Label ID="lbltime" runat="server" 
               __designer:wfdid="w17" Text="Time"></asp:Label>
            </TD><TD style="WIDTH: 100px">
                <asp:TextBox ID="txtTime" runat="server" 
                    __designer:wfdid="w18" Width="104px"></asp:TextBox>
            </TD><TD style="WIDTH: 100px">
                <asp:Label ID="lbldayclose" runat="server" 
                    __designer:wfdid="w1" Font-Bold="True" 
                    Text="Day close date"></asp:Label>
            </TD><TD style="WIDTH: 100px">
                <asp:TextBox ID="txtDaycloseDate" runat="server" 
                    __designer:wfdid="w9" Width="104px"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" 
                    __designer:wfdid="w37" Format="dd-MM-yyyy" 
                    TargetControlID="txtDaycloseDate">
                </cc1:CalendarExtender>
            </TD>
        <td>
            <asp:LinkButton ID="lnkDueVacatingReports" runat="server" 
                 CausesValidation="False" 
                Font-Bold="True" onclick="lnkDueVacatingReports_Click" 
                Width="140px">Due Vacating Rooms</asp:LinkButton>
        </td>
        </TR>
        <tr>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td style="WIDTH: 100px">
                &nbsp;</td>
            <td>
                <asp:LinkButton ID="lnkreservedunoccupied" Font-Bold="True" 
                    runat="server" onclick="lnkreservedunoccupied_Click">Reserved 
                But Unoccupied</asp:LinkButton>
             
            </td>
        </tr>
        </TBODY></TABLE></asp:Panel>    <asp:Panel ID="pnlliability" runat="server" 
            Width="100%">
                <table class="style4">
                    <tr>
                        <td width="25%">
                            Season</td>
                        <td width="25%">
                            <asp:DropDownList ID="ddlseason" runat="server" DataTextField="seasonname" 
                                DataValueField="season_sub_id" Enabled="False" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td width="25%">
                            <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
                                Text="Cashier liability" CssClass="btnStyle_large" />
                        </td>
                        <td width="25%">
                            <asp:Button ID="Button4" runat="server" CssClass="btnStyle_large" 
                                onclick="Button4_Click" Text="Cash remittance" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" width="100%">
                            <asp:GridView ID="gvview" runat="server" CellPadding="4" ForeColor="#333333" 
                                GridLines="None" Visible="False" Width="100%">
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" width="100%">
                            &nbsp;</td>
                    </tr>
                </table>
                </asp:Panel></td>
              
            </tr></tbody>
    </table>    
    <td>
                    <asp:Panel ID="pnlMessage" runat="server" _="" _designer:dtid="562958543355909" 
                        CssClass="ModalWindow">
                        <asp:Panel ID="Panel8" runat="server" BackColor="LightSteelBlue" 
                            BorderStyle="Outset">
                            <asp:Label ID="Label22" runat="server" Font-Bold="True" ForeColor="MediumBlue" 
                                Text="Tsunami ARMS - Confirmation"></asp:Label>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                            PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize" 
                            TargetControlID="btnHidden">
                        </cc1:ModalPopupExtender>
                        &nbsp;
                        <asp:Button ID="btnHidden" runat="server" style="DISPLAY: none" Text="Hidden" />
                        &nbsp;
                        <BR />
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
                                            &nbsp; &nbsp;&nbsp;
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
                                            <asp:Label ID="lblOk" runat="server" Font-Size="Small" ForeColor="Black"></asp:Label>
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
                                                  <asp:Label id="lblHead" runat="server" Width="197px" Font-Bold="True" Text="Tsunami ARMS - Confirmation" __designer:dtid="562958543355916" __designer:wfdid="w46" ForeColor="MediumBlue"></asp:Label>
                <asp:Label id="lblHead2" runat="server" Width="191px" Font-Bold="True" Text="Tsunami ARMS - Warning" __designer:wfdid="w56" ForeColor="MediumBlue"></asp:Label>
                                        </td>
                                        <td align="center">
                                            &nbsp; &nbsp; &nbsp;<asp:Button ID="btnOk" runat="server" CausesValidation="False" 
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
                    <cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" __designer:dtid="562958543355957" __designer:wfdid="w184" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize"></cc1:ModalPopupExtender>      
                </td>
<table><tbody></tbody>    
    </table>
    </ContentTemplate>
     <Triggers>
  <asp:PostBackTrigger ControlID="lnktotalallocseasonreport" />
  <asp:PostBackTrigger ControlID="lnkDonorPaidRoomAllocationReport" />  
  <asp:PostBackTrigger ControlID="lnkreservelist" />
  <asp:PostBackTrigger ControlID="lnkCollectionCompare" />  
  <asp:PostBackTrigger ControlID="lnkDueVacatingReports" />  
    <asp:PostBackTrigger ControlID="lnkreservedunoccupied" />  
       <asp:PostBackTrigger ControlID="lnkonline" />  
      
  </Triggers>
    </asp:UpdatePanel>
</asp:Content>


