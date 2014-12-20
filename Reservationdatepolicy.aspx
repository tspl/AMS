<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reservationdatepolicy.aspx.cs" Inherits="Reservationdatepolicy" %>

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
                                <td>
                                    Season name</td>
                                <td>
                                    <asp:DropDownList ID="cmbseason" runat="server" DataTextField="seasonname" 
                                        DataValueField="season_sub_id" TabIndex="20" Width="153px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    check in date</td>
                                <td>
                                    <asp:TextBox ID="txtstartchkdate" runat="server" AutoPostBack="True" 
                                        Height="17px" MaxLength="10" Width="150px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtstartchkdate_CalendarExtender" runat="server" 
                                        Format="dd/MM/yyyy" TargetControlID="txtstartchkdate">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 24px">
                                    Reservation start date </td>
                                <td style="height: 24px">
                                    <asp:TextBox ID="txtstartdate" runat="server" AutoPostBack="True" Height="17px" 
                                        MaxLength="10" Width="150px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" 
                                        TargetControlID="txtstartdate">
                                    </cc1:CalendarExtender>
                                </td>
                                <td style="height: 24px">
                                    Checkout&nbsp; date</td>
                                <td style="height: 24px">
                                    <asp:TextBox ID="txtendchkdate" runat="server" AutoPostBack="True" 
                                        Height="17px" MaxLength="10" Width="150px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtendchkdate_CalendarExtender" runat="server" 
                                        Format="dd/MM/yyyy" TargetControlID="txtendchkdate">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 24px">
                                    Closed reservation day</td>
                                <td style="height: 24px">
                                    <asp:TextBox ID="txtclosed" runat="server" AutoPostBack="True" Height="17px" 
                                        MaxLength="10" Width="150px"></asp:TextBox>
                                </td>
                                <td style="height: 24px">
                                    Alter</td>
                                <td style="height: 24px">
                                    <asp:DropDownList ID="cmbalter" runat="server" DataTextField="options" 
                                        DataValueField="id" TabIndex="26" Width="153px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Type</td>
                                <td>
                                    <asp:DropDownList ID="cmbtype" runat="server" DataTextField="type" 
                                        DataValueField="id" TabIndex="23" Width="153px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Max reservation with id proof</td>
                                <td>
                                    <asp:TextBox ID="txtmaxreserve" runat="server" TabIndex="27" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cancel status</td>
                                <td>
                                    <asp:DropDownList ID="cmbcancel" runat="server" Width="153px" 
                                        DataTextField="cancel" DataValueField="id">                                   
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
                                        Text="Save" onclick="btnsave_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnedit" runat="server" CssClass="btnStyle_medium" 
                                        Text="Edit" onclick="btnedit_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btndelete" runat="server" CssClass="btnStyle_medium" 
                                        Text="Delete" onclick="btndelete_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_medium" 
                                        Text="Clear" onclick="btnclear_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: center">
                                    <asp:GridView ID="gv_details" runat="server" AutoGenerateColumns="False" 
                                        CellPadding="4" ForeColor="#333333" GridLines="None" 
                                        onrowcreated="gv_details_RowCreated" onrowdatabound="gv_details_RowDataBound" 
                                        onselectedindexchanged="gv_details_SelectedIndexChanged" 
                                        HorizontalAlign="Center">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" />
                                            <asp:BoundField DataField="season_sub_id" HeaderText="Season" />
                                            <asp:BoundField DataField="r_startdate" HeaderText="Res start Date" />
                                            <asp:BoundField DataField="day_close" HeaderText="Res closed" />
                                            <asp:BoundField DataField="in_startdate" HeaderText="Checkindate" />
                                            <asp:BoundField DataField="in_enddate" HeaderText="Checkout date" />
                                            <asp:BoundField DataField="alter_status" HeaderText="Alter" />
                                            <asp:BoundField DataField="type_id" HeaderText="Type" />
                                            <asp:BoundField DataField="max_reserv" HeaderText="Max reserve" />
                                            <asp:BoundField DataField="cancel_status" HeaderText="Cancel " />
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

