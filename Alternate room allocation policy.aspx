<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Alternate room allocation policy.aspx.cs" Inherits="Alternate_room_allocation_policy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"> 
</asp:ScriptManager>
 <asp:UpdatePanel id="UpdatePanel1" runat="server">
 <ContentTemplate>

     <table style="width: 100%">
         <tr>
             <td>
                 <asp:Panel ID="Panel6" runat="server">
                     <table style="width: 100%">
                         <tr>
                             <td>
                                 Policy applicable from</td>
                             <td>
                                 <asp:TextBox ID="txtdate" runat="server" Width="160px" TabIndex="20"></asp:TextBox>
                                 <cc1:CalendarExtender ID="txtdate_CalendarExtender" runat="server" 
                                     Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtdate">
                                 </cc1:CalendarExtender>
                             </td>
                             <td>
                                 Extra billing</td>
                             <td>
                                 <asp:DropDownList ID="ddlbill" runat="server" DataTextField="billing" 
                                     DataValueField="id" Width="163px" TabIndex="22">
                                 </asp:DropDownList>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Type of allocation</td>
                             <td>
                                 <asp:DropDownList ID="ddltype" runat="server" DataTextField="type" 
                                     DataValueField="id" Width="163px" TabIndex="21">
                                 </asp:DropDownList>
                             </td>
                             <td>
                                 &nbsp;</td>
                             <td>
                                 &nbsp;</td>
                         </tr>
                         <tr>
                             <td style="text-align: center">
                                 <asp:Button ID="btnsave" runat="server" CssClass="btnStyle_medium" 
                                     Text="Save" onclick="btnsave_Click" TabIndex="23" />
                             </td>
                             <td style="text-align: center">
                                 <asp:Button ID="btnedit" runat="server" CssClass="btnStyle_medium" 
                                     Text="Edit" onclick="btnedit_Click" TabIndex="24" />
                             </td>
                             <td style="text-align: center">
                                 <asp:Button ID="btndelete" runat="server" CssClass="btnStyle_medium" 
                                     Text="Delete" onclick="btndelete_Click" TabIndex="25" />
                             </td>
                             <td style="text-align: center">
                                 <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_medium" 
                                     Text="Clear" onclick="btnclear_Click" TabIndex="26" />
                             </td>
                         </tr>
                     </table>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

