<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Roomdamagepolicy.aspx.cs" Inherits="Roomdamagepolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="Panel6" runat="server">
            <table style="width: 100%">
                <tr>
                    <td>
                        Policy applicable from</td>
                    <td>
                        <asp:TextBox ID="txtdate" runat="server" AutoPostBack="True" Height="17px" 
                            MaxLength="10" TabIndex="20" Width="150px"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MM-yyyy" 
                            TargetControlID="txtdate">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        Damages</td>
                    <td>
                        <asp:DropDownList ID="cmbdamage" runat="server" DataTextField="damages" 
                            DataValueField="id" TabIndex="22" Width="153px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Room category</td>
                    <td>
                        <asp:DropDownList ID="cmbroom" runat="server" DataTextField="room_cat_name" 
                            DataValueField="room_cat_id" TabIndex="21" Width="153px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Rate</td>
                    <td>
                        <asp:TextBox ID="txtrate" runat="server" TabIndex="23" Width="150px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtrate_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtrate">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnsave" runat="server" CssClass="btnStyle_medium" 
                            onclick="btnsave_Click" TabIndex="24" Text="Save" />
                    </td>
                    <td>
                        <asp:Button ID="btnedit" runat="server" CssClass="btnStyle_medium" 
                            onclick="btnedit_Click" TabIndex="25" Text="Edit" />
                    </td>
                    <td>
                        <asp:Button ID="btndelete" runat="server" CssClass="btnStyle_medium" 
                            onclick="btndelete_Click" TabIndex="26" Text="Delete" />
                    </td>
                    <td>
                        <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_medium" 
                            onclick="btnclear_Click" TabIndex="27" Text="Clear" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center">
                        <asp:GridView ID="gv_details" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" 
                            onrowcreated="gv_details_RowCreated" onrowdatabound="gv_details_RowDataBound" 
                            onselectedindexchanged="gv_details_SelectedIndexChanged">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="ID" />
                                <asp:BoundField DataField="policy_applicable_from" 
                                    HeaderText="Policy applicable from" />
                                <asp:BoundField DataField="to_date" HeaderText="To date" />
                                <asp:BoundField DataField="room_category" HeaderText="Room category" />
                                <asp:BoundField DataField="damages" HeaderText="Damages" />
                                <asp:BoundField DataField="rate" HeaderText="Rate" />
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

