<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="clubbingpolicy.aspx.cs" Inherits="clubbingpolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table style="width: 100%">
            <tr>
                <td colspan="2">
                    <asp:Panel ID="Panel6" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td width="25%">
                                    Policy applicable from</td>
                                <td width="25%">
                                    <asp:TextBox ID="txtdate" runat="server" Width="160px" AutoPostBack="True" 
                                        ontextchanged="txtdate_TextChanged" TabIndex="20"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtdate_CalendarExtender" runat="server" 
                                        Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtdate">
                                    </cc1:CalendarExtender>
                                </td>
                                <td width="25%">
                                    Reservation Types</td>
                                <td width="25%">
                                    <asp:DropDownList ID="cmbtype" runat="server" DataTextField="type" 
                                        DataValueField="id" Width="163px" TabIndex="22">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td width="25%">
                                    Policy applicable to</td>
                                <td width="25%">
                                    <asp:TextBox ID="txttodate" runat="server" Width="160px" TabIndex="21"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txttodate_CalendarExtender" runat="server" 
                                        Enabled="True" Format="dd-MM-yyyy" TargetControlID="txttodate">
                                    </cc1:CalendarExtender>
                                </td>
                                <td width="25%">
                                    Clubbing</td>
                                <td width="25%">
                                    <asp:DropDownList ID="cmbclubbing" runat="server" DataTextField="clubbing" 
                                        DataValueField="id" Width="163px" TabIndex="23">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnsave" runat="server" CssClass="btnStyle_medium" 
                                        onclick="btnsave_Click" Text="Save" TabIndex="24" />
                                </td>
                                <td>
                                    <asp:Button ID="btnedit" runat="server" CssClass="btnStyle_medium" 
                                        onclick="btnedit_Click" Text="Edit" TabIndex="25" />
                                </td>
                                <td>
                                    <asp:Button ID="btndelete" runat="server" CssClass="btnStyle_medium" 
                                        Text="Delete" onclick="btndelete_Click" TabIndex="26" />
                                </td>
                                <td>
                                    <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_medium" 
                                        Text="Clear" onclick="btnclear_Click" TabIndex="27" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4" style="text-align: center">
                                    <asp:GridView ID="gv_details" runat="server" AutoGenerateColumns="False" 
                                        CellPadding="4" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" 
                                        onrowcreated="gv_details_RowCreated" onrowdatabound="gv_details_RowDataBound" 
                                        onselectedindexchanged="gv_details_SelectedIndexChanged">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="id" />
                                            <asp:BoundField DataField="from_date" HeaderText="From date" />
                                            <asp:BoundField DataField="to_date" HeaderText="To date" />
                                            <asp:BoundField DataField="reserve_types" HeaderText="Reservationtypes" />
                                            <asp:BoundField DataField="clubbing_status" HeaderText="Clubbing" />
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
         function Showdate() {
             alert('Date already exist');
         }
               </script>       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

