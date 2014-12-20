<%@ Page Title="Before and After" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="checkingpolicy.aspx.cs" Inherits="checkingpolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
    <asp:Label id="Label1" runat="server" Width="483px" ForeColor="DarkBlue" 
            Text="BEFORE AND AFTER RESERVATION POLICY" Font-Bold="True" Font-Size="16pt"></asp:Label>
    </center>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>

        <table style="width: 100%">
            <tr>
                <td colspan="2">
                    <asp:Panel ID="Panel6" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    Policy applicable from</td>
                                <td>
                                    <asp:TextBox ID="txtdate" runat="server" AutoPostBack="True" Height="17px" 
                                        MaxLength="10" TabIndex="20" Width="160px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" 
                                        TargetControlID="txtdate">
                                    </cc1:CalendarExtender>
                                </td>
                                <td>
                                    Payment</td>
                                <td>
                                    <asp:DropDownList ID="ddlpayment" runat="server" DataTextField="payment" 
                                        DataValueField="id" TabIndex="23" Width="163px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Reservation type</td>
                                <td>
                                    <asp:DropDownList ID="ddltype" runat="server" DataTextField="type" 
                                        DataValueField="id" Width="163px" TabIndex="21">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Holding period</td>
                                <td>
                                    <asp:TextBox ID="txtcancel" runat="server" Width="160px" TabIndex="24"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="txtcancel_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtcancel">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Proposed check in time</td>
                                <td>
                                    <asp:DropDownList ID="ddlcheck" runat="server" 
                                         Width="163px" DataTextField="proposed_check_in" DataValueField="id" 
                                        TabIndex="22">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnsave" runat="server" CssClass="btnStyle_medium" 
                                        Text="Save" onclick="btnsave_Click" TabIndex="25" />
                                </td>
                                <td>
                                    <asp:Button ID="btnedit" runat="server" CssClass="btnStyle_medium" 
                                        Text="Edit" onclick="btnedit_Click" TabIndex="26" />
                                </td>
                                <td>
                                    <asp:Button ID="btndelete" runat="server" CssClass="btnStyle_medium" 
                                        Text="Delete" onclick="btndelete_Click" TabIndex="27" />
                                </td>
                                <td>
                                    <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_medium" 
                                        Text="Clear" onclick="btnclear_Click" TabIndex="28" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: center">
                                    <asp:GridView ID="gv_details" runat="server" CellPadding="4" ForeColor="#333333" 
                                        GridLines="None" HorizontalAlign="Center" AutoGenerateColumns="False" 
                                        onselectedindexchanged="gv_details_SelectedIndexChanged" 
                                        onrowcreated="gv_details_RowCreated" onrowdatabound="gv_details_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="id" />
                                            <asp:BoundField DataField="from_date" HeaderText="From date" />
                                            <asp:BoundField DataField="to_date" HeaderText="To date" />
                                            <asp:BoundField DataField="reserve_type" HeaderText="Type" />
                                            <asp:BoundField DataField="proposed_check_in" HeaderText="Proposed check in" />
                                            <asp:BoundField DataField="payment" HeaderText="Payment" />
                                            <asp:BoundField DataField="holding_period" HeaderText="Holding period" />
                                            <asp:CommandField SelectText="" ShowSelectButton="True" />
                                        </Columns>
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
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
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
        function ShowDeleted() {
            alert('Deleted Successfully');
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

