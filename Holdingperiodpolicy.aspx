<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Holdingperiodpolicy.aspx.cs" Inherits="Holdingperiodpolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
   <asp:UpdatePanel id="UpdatePanel1" runat="server">
   <ContentTemplate>

       <table style="width: 100%">
           <tr>
               <td>
                   <asp:Panel ID="Panel6" runat="server" GroupingText="Holding period">
                       <table style="width: 100%">
                           <tr>
                               <td width="25%">
                                   Policy applicable from</td>
                               <td width="25%">
                                   <asp:TextBox ID="txtdate" runat="server" Width="160px"></asp:TextBox>
                                   <cc1:CalendarExtender ID="txtdate_CalendarExtender" runat="server" 
                                       Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtdate">
                                   </cc1:CalendarExtender>
                               </td>
                               <td width="25%">
                                   Releasing time</td>
                               <td width="25%">
                                   <asp:TextBox ID="txtrelease" runat="server" Width="160px"></asp:TextBox>
                                   <cc1:FilteredTextBoxExtender ID="txtrelease_FilteredTextBoxExtender" 
                                       runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtrelease">
                                   </cc1:FilteredTextBoxExtender>
                               </td>
                           </tr>
                           <tr>
                               <td>
                                   Reservation type</td>
                               <td>
                                   <asp:DropDownList ID="ddltype" runat="server" DataTextField="type" 
                                       DataValueField="id" Width="163px">
                                   </asp:DropDownList>
                               </td>
                               <td>
                                   Minimum time required for cancelation</td>
                               <td>
                                   <asp:TextBox ID="txtcancel" runat="server" Width="160px"></asp:TextBox>
                                   <cc1:FilteredTextBoxExtender ID="txtcancel_FilteredTextBoxExtender" 
                                       runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtcancel">
                                   </cc1:FilteredTextBoxExtender>
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
                       </table>
                   </asp:Panel>
               </td>
           </tr>
           <tr>
               <td style="text-align: center">
                   <asp:GridView ID="gv_details" runat="server" AutoGenerateColumns="False" 
                       CellPadding="4" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" 
                       onrowcreated="gv_details_RowCreated" onrowdatabound="gv_details_RowDataBound" 
                       onselectedindexchanged="gv_details_SelectedIndexChanged">
                       <AlternatingRowStyle BackColor="White" />
                       <Columns>
                           <asp:BoundField DataField="id" HeaderText="id" />
                           <asp:BoundField DataField="from_date" HeaderText="From date" />
                           <asp:BoundField DataField="to_date" HeaderText="To date" />
                           <asp:BoundField DataField="type_id" HeaderText="Type" />
                           <asp:BoundField DataField="release_time" HeaderText="Releasing time" />
                           <asp:BoundField DataField="cancelation_time" 
                               HeaderText="Min time cancelation" />
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

