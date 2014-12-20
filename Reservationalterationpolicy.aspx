<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reservationalterationpolicy.aspx.cs" Inherits="Reservationalterationpolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table style="width: 100%">
            <tr>
                <td colspan="4">
                    <asp:Panel ID="Panel6" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    Season name</td>
                                <td>
                                    <asp:DropDownList ID="cmbseason" runat="server" DataTextField="seasonname" 
                                        DataValueField="season_sub_id" Width="153px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Alter charges</td>
                                <td>
                                    <asp:TextBox ID="txtalter" runat="server" Width="150px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="txtalter_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtalter">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Type of room</td>
                                <td>
                                    <asp:DropDownList ID="cmbroom" runat="server" DataTextField="room_cat_name" 
                                        DataValueField="room_cat_id" Width="153px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Type</td>
                                <td>
                                    <asp:DropDownList ID="cmbtype" runat="server" DataTextField="type" 
                                        DataValueField="id" Width="153px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnsave" runat="server" CssClass="btnStyle_medium" 
                        onclick="btnsave_Click" Text="Save" />
                </td>
                <td>
                    <asp:Button ID="btnedit" runat="server" CssClass="btnStyle_medium" 
                        onclick="btnedit_Click" Text="Edit" />
                </td>
                <td>
                    <asp:Button ID="btndelete" runat="server" CssClass="btnStyle_medium" 
                        onclick="btndelete_Click" Text="Delete" />
                </td>
                <td>
                    <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_medium" 
                        onclick="btnclear_Click" Text="Clear" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView ID="gv_details" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" 
                        onrowcreated="gv_details_RowCreated" onrowdatabound="gv_details_RowDataBound" 
                        onselectedindexchanged="gv_details_SelectedIndexChanged">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="ID" />
                            <asp:BoundField DataField="season_sub_id" HeaderText="Season name" />
                            <asp:BoundField DataField="room_category_id" HeaderText="Type of room" />
                            <asp:BoundField DataField="alter_charges" HeaderText="Alter charges" />
                            <asp:BoundField DataField="type_id" HeaderText="Type" />
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

