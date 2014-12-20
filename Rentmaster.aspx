<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rentmaster.aspx.cs" Inherits="Rentmaster" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
            <table style="width: 100%">
                <tr>
                    <td>
                        Type of reservation</td>
                    <td>
                        <asp:DropDownList ID="cmbreserve" runat="server" Width="153px" 
                            DataTextField="type" DataValueField="id" TabIndex="20">                        
                        </asp:DropDownList>
                    </td>
                    <td>
                        Rent</td>
                    <td>
                        <asp:TextBox ID="txtrent" runat="server" TabIndex="22" Width="150px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtrent_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtrent">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Type of room</td>
                    <td>
                        <asp:DropDownList ID="cmbroom" runat="server" Width="153px" AutoPostBack="True" 
                            DataTextField="room_cat_name" DataValueField="room_cat_id" TabIndex="21" 
                            >
                        </asp:DropDownList>
                    </td>
                    <td>
                        Security deposit</td>
                    <td>
                        <asp:TextBox ID="txtsecurity" runat="server" Width="150px" TabIndex="24"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtsecurity_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" 
                            TargetControlID="txtsecurity">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        End duration</td>
                    <td>
                        <asp:TextBox ID="txtendduration" runat="server" AutoPostBack="True" 
                            ontextchanged="txtendduration_TextChanged" TabIndex="23" Width="150px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtendduration_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" 
                            TargetControlID="txtendduration">
                        </cc1:FilteredTextBoxExtender>
                        <asp:TextBox ID="txtstartduration" runat="server" Visible="False" Width="20px"></asp:TextBox>
                    </td>
                    <td>
                        Reservation charge</td>
                    <td>
                        <asp:TextBox ID="txtcharge" runat="server" Width="150px" TabIndex="25"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtcharge_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtcharge">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        Extended penality</td>
                    <td>
                        <asp:TextBox ID="txtpenality" runat="server" TabIndex="26" Width="150px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtpenality_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" 
                            TargetControlID="txtpenality">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnsave" runat="server" CssClass="btnStyle_medium" 
                                        onclick="btnsave_Click" Text="Save" TabIndex="27" />
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btnview" runat="server" CssClass="btnStyle_medium" 
                                        onclick="btnview_Click" Text="View" TabIndex="28" />
                                </td>
                                <td>
                                    <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_medium" 
                                        onclick="btnclear_Click" Text="Clear" TabIndex="29" />
                                </td>
                                <td>
                                    <asp:Button ID="btnedit" runat="server" CssClass="btnStyle_medium" 
                                        onclick="btnedit_Click" Text="Edit" TabIndex="30" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:Panel ID="pnlview" runat="server" Visible="False">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gv_details" runat="server" AutoGenerateColumns="False" 
                                            CellPadding="4" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" 
                                            onrowcreated="gv_details_RowCreated" onrowdatabound="gv_details_RowDataBound" 
                                            onselectedindexchanged="gv_details_SelectedIndexChanged" 
                                            style="text-align: center">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:BoundField DataField="reservation_type" HeaderText="Type of reservation" />
                                                <asp:BoundField DataField="room_category" HeaderText="Type of room" />
                                                <asp:BoundField DataField="start_duration" HeaderText="Start duration" />
                                                <asp:BoundField DataField="end_duration" HeaderText="End duration" />
                                                <asp:BoundField DataField="reserve_charge" HeaderText="Reservation charge" />
                                                <asp:BoundField DataField="rent" HeaderText="Rent" />
                                                <asp:BoundField DataField="security_deposit" HeaderText="Security deposit" />
                                                <asp:BoundField DataField="extended_penality" HeaderText="Extended penality" />
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
                    <td align="center" colspan="4">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlgrid" runat="server" Visible="False">
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    Type of reservation</td>
                                                <td>
                                                    <asp:DropDownList ID="cmbreserve1" runat="server" DataTextField="type" 
                                                        DataValueField="id" Width="153px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Type of room</td>
                                                <td>
                                                    <asp:DropDownList ID="cmbroom1" runat="server" AutoPostBack="True" 
                                                        DataTextField="room_cat_name" DataValueField="room_cat_id" 
                                                        onselectedindexchanged="cmbroom1_SelectedIndexChanged" Width="153px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="gv_details1" runat="server" AutoGenerateColumns="False" 
                                                        CellPadding="4" ForeColor="#333333" GridLines="None" 
                                                        onrowdatabound="gv_details1_RowDataBound" 
                                                        onselectedindexchanged="gv_details1_SelectedIndexChanged">
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <Columns>
                                                            <asp:BoundField DataField="id" HeaderText="ID" />
                                                            <asp:BoundField DataField="duration" HeaderText="Duration" />
                                                            <asp:TemplateField HeaderText="Rent">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtgridrent" runat="server"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Security Deposit">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtgridsecurity" runat="server"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Reserve charge">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtgridreserve" runat="server"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Extended penality">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtgridpenality" runat="server"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ShowSelectButton="True" />
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
                                            <tr>
                                                <td width="50%">
                                                    <asp:Button ID="btnupdate" runat="server" CssClass="btnStyle_medium" 
                                                        onclick="btnupdate_Click" Text="Update" />
                                                </td>
                                                <td width="50%">
                                                    <asp:Button ID="btndelete" runat="server" CssClass="btnStyle_medium" 
                                                        onclick="btndelete_Click" Text="Delete" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
            </table>
        </contenttemplate>
        </asp:UpdatePanel>
           <script type="text/javascript">
               function Showalert() {
                   alert('Saved Successfully');
               }
               function ShowNoData() {
                   alert('No Data Found');
               }
               function ShowNoCounter() {
                   alert('no counter is set');
               }
               function ShowDeleted() {
                   alert('Deleted Successfully');
               }
               function ShowNoDeleted() {
                   alert('Deleted UnSuccessfully');
               }
               function ShowAltered() {
                   alert('Updated Successfully');
               }
               function ShowError() {
                   alert('Error in Updation');
               }
               function Showtimeslot() {
                   alert('timeslot is already exist');
               }
        </script>
</asp:Content>


