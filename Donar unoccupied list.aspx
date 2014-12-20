<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Donar unoccupied list.aspx.cs" Inherits="Donar_unoccupied_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 &nbsp;&nbsp;&nbsp;
 <asp:ScriptManager ID="ScriptManager1" runat="server">   </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>

        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlreport" runat="server" GroupingText="Report">
                        <TABLE>
                            <tbody>
                                <tr>
                                    <td style="WIDTH: 100px; HEIGHT: 24px">
                                        <asp:Label ID="lblreporttype" runat="server" __designer:wfdid="w35" 
                                            Text="Reservation type" Width="104px"></asp:Label>
                                    </td>
                                    <td style="WIDTH: 100px; HEIGHT: 24px">
                                        <asp:DropDownList ID="cmbReportpass" runat="server" __designer:wfdid="w36" 
                                            Height="22px" Width="111px" TabIndex="20">
                                            <asp:ListItem Value="-1">All</asp:ListItem>
                                             <asp:ListItem>General</asp:ListItem>
                                            <asp:ListItem>Donor Free</asp:ListItem>
                                            <asp:ListItem>Donor Paid</asp:ListItem>
                                            <asp:ListItem>tdb</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2" style="HEIGHT: 24px">
                                        <asp:Label ID="lblmessage" runat="server" __designer:wfdid="w37" 
                                            ForeColor="Red" Text="Select all essential fields" Visible="False" 
                                            Width="152px"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblreportdate" runat="server" Text="Reservation date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtreportdatefrom" runat="server" AutoPostBack="True" 
                                            OnTextChanged="txtreportdatefrom_TextChanged" Width="104px" TabIndex="21"></asp:TextBox>
                                        <cc1:CalendarExtender ID="cereportfrom" runat="server" Format="dd-MM-yyyy" 
                                            TargetControlID="txtreportdatefrom">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblreportto" runat="server" Text="Reservation to" Width="88px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtreportdateto" runat="server" Width="104px" TabIndex="22"></asp:TextBox>
                                        <cc1:CalendarExtender ID="cereportto" runat="server" Format="dd-MM-yyyy" 
                                            TargetControlID="txtreportdateto">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="WIDTH: 100px">
                                        <asp:Button ID="btnview" runat="server" CssClass="btnStyle_medium" 
                                            onclick="btnview_Click" Text="View" TabIndex="23" />
                                    </td>
                                    <td style="WIDTH: 100px">
                                        &nbsp;</td>
                                    <td style="WIDTH: 100px">
                                        &nbsp;</td>
                                    <td style="WIDTH: 100px">
                                        &nbsp;</td>
                                    <td>
                                        <asp:Button ID="btnrelease" runat="server" CssClass="btnStyle_medium" 
                                            Text="Release" onclick="btnrelease_Click" TabIndex="24" />
                                    </td>
                                </tr>
                            </tbody>
                        </TABLE>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:GridView ID="gv_details" runat="server" CellPadding="4" 
                        ForeColor="#333333" GridLines="None" HorizontalAlign="Center" 
                        onrowcreated="gv_details_RowCreated" onrowdatabound="gv_details_RowDataBound" 
                        onselectedindexchanged="gv_details_SelectedIndexChanged">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />                 
                    </asp:GridView>
                </td>
            </tr>
        </table>

    </ContentTemplate>
    </asp:UpdatePanel>
     <script type="text/javascript">
         function Showalert() {
             alert('Saved Successfully');
         }
         function ShowNoData() {
             alert('No Data Found');
         }
         function ShowRelease() {
             alert('Released & send email Successfully');
         }
         function ShowRequired() {
             alert('Fill the required field');
         }
         function ShowUpdated() {
             alert('Updated Successfully');
         }
               </script>       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

