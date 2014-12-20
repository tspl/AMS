<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Numberofinmates policy .aspx.cs" Inherits="Numberofinmates_policy_" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
 </asp:ScriptManager>
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
 <ContentTemplate>

     <table style="width: 99%; height: 329px">
         <tr>
             <td colspan="4" style="height: 24px">
                 <asp:Panel ID="Panel6" runat="server" GroupingText="inmates details">
                     <table style="width: 100%">
                         <tr>
                             <td>
                                 <asp:Label ID="Label1" runat="server" Text="Policy applicable from"></asp:Label>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtdate" runat="server" AutoPostBack="True" Height="17px" 
                                     MaxLength="10" Width="150px" TabIndex="20"></asp:TextBox>
                                 <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MM-yyyy" 
                                     TargetControlID="txtdate">
                                 </cc1:CalendarExtender>
                             </td>
                             <td>
                                 <asp:Label ID="Label11" runat="server" Text="number of inmates"></asp:Label>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtinmates" runat="server" Width="150px" TabIndex="23"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="txtinmates_FilteredTextBoxExtender" 
                                     runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtinmates">
                                 </cc1:FilteredTextBoxExtender>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 <asp:Label ID="Label13" runat="server" Text="Type of reservation"></asp:Label>
                             </td>
                             <td>
                                 <asp:DropDownList ID="cmbtype" runat="server" DataTextField="type" 
                                     DataValueField="id" Width="152px" TabIndex="21">
                                 </asp:DropDownList>
                             </td>
                             <td>
                                 <asp:Label ID="Label15" runat="server" Text="maximum number of inmates"></asp:Label>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtaddinmates" runat="server" Width="150px" TabIndex="24"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="txtaddinmates_FilteredTextBoxExtender" 
                                     runat="server" Enabled="True" FilterType="Numbers" 
                                     TargetControlID="txtaddinmates">
                                 </cc1:FilteredTextBoxExtender>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 <asp:Label ID="Label14" runat="server" Text="Type of room"></asp:Label>
                             </td>
                             <td>
                                 <asp:DropDownList ID="cmbcategory" runat="server" DataTextField="room_cat_name" 
                                     DataValueField="room_cat_id" 
                                      Width="152px" TabIndex="22">
                                 </asp:DropDownList>
                             </td>
                             <td>
                                 &nbsp;</td>
                             <td>
                                 &nbsp;</td>
                         </tr>
                     </table>
                 </asp:Panel>
             </td>
         </tr>
         <tr>
             <td width="25%">
                 <asp:Button ID="btnsave" runat="server" CssClass="btnStyle_medium" 
                     onclick="btnsave_Click" Text="Save" TabIndex="26" />
             </td>
             <td width="25%">
                 <asp:Button ID="btnedit" runat="server" CssClass="btnStyle_medium" 
                     Text="Edit" onclick="btnedit_Click" TabIndex="27" />
             </td>
             <td width="25%">
                 <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_medium" 
                     Text="Clear" onclick="btnclear_Click" TabIndex="28" />
             </td>
             <td width="25%">
                 <asp:Button ID="btndelete" runat="server" CssClass="btnStyle_medium" 
                     Text="Delete" onclick="btndelete_Click" TabIndex="29" />
             </td>
         </tr>
         <tr>
             <td colspan="4">
                 <asp:GridView ID="gv_details" runat="server" 
                     CellPadding="4" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" 
                     onrowcreated="gv_details_RowCreated" onrowdatabound="gv_details_RowDataBound" 
                     onselectedindexchanged="gv_details_SelectedIndexChanged" 
                     style="text-align: center" AutoGenerateColumns="False">
                     <AlternatingRowStyle BackColor="White" />
                     <Columns>
                         <asp:BoundField DataField="inmates_id" HeaderText="InmatesID" />
                         <asp:BoundField DataField="policy_type" HeaderText="Type" />
                         <asp:BoundField DataField="room_category" HeaderText="Type of room" />
                         <asp:BoundField DataField="fromdate" HeaderText="FromDate" />
                         <asp:BoundField DataField="todate" HeaderText="ToDate" />
                         <asp:BoundField DataField="num_of_inmates" HeaderText="No of inmates" />
                         <asp:BoundField DataField="max_num_of_add_inmates" 
                             HeaderText="Max no of inmates" />
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
         <tr>
             <td colspan="4">
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
     function ShowValue() {
         alert('Enter the value');
     }
               </script>       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

