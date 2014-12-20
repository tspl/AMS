<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="past allocation policy.aspx.cs" Inherits="past_allocation_policy" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
               </asp:ScriptManager>
   
    <asp:UpdatePanel id="UpdatePanel1" runat="server"><contenttemplate>

               <table style="width: 100%; height: 50px">
                   <tr>
                       <td colspan="4">
                           <asp:Panel ID="allocationdetails" runat="server" 
                               GroupingText="Allocation Details">
                               <table style="width: 100%">
                                   <tr>
                                       <td width="25%">
                                           <asp:Label ID="Label4" runat="server" Text="Policy Applicable From"></asp:Label>
                                       </td>
                                       <td style="width: 25%" width="25%">
                                           <asp:TextBox ID="txtfrmdate" runat="server" AutoPostBack="True" Height="17px" 
                                              MaxLength="10"  Width="150px"></asp:TextBox>
                                           <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MM-yyyy" 
                                               TargetControlID="txtfrmdate">
                                           </cc1:CalendarExtender>
                                       </td>
                                       <td width="25%">
                                           <asp:Label ID="Label2" runat="server" Text="Maximum Allocations"></asp:Label>
                                       </td>
                                       <td style="width: 25%" width="25%">
                                           <asp:TextBox ID="txtallocation" runat="server" Width="150px"></asp:TextBox>
                                       </td>
                                   </tr>
                                   <tr>
                                       <td width="25%">
                                           <asp:Label ID="Label1" runat="server" Text="Allocation Request"></asp:Label>
                                       </td>
                                       <td style="width: 25%" width="25%">
                                           <asp:DropDownList ID="cmballocrequest" runat="server" Width="153px">
                                           <asp:ListItem Value="-1">-- Select --</asp:ListItem>
                                           <asp:ListItem Value="1">Common</asp:ListItem>
                                           <asp:ListItem Value="2">Donor Free Allocation</asp:ListItem>
                                           <asp:ListItem Value="3">Donor Paid Allocation</asp:ListItem>
                                           <asp:ListItem Value="4">Donor multiple pass</asp:ListItem>
                                           <asp:ListItem Value="5">General Allocation</asp:ListItem>
                                           <asp:ListItem Value="6">TDB Allocation</asp:ListItem>
                                           </asp:DropDownList>
                                       </td>
                                       <td width="25%">
                                           <asp:Label ID="Label3" runat="server" Text="Checking Criteria"></asp:Label>
                                       </td>
                                       <td style="width: 25%" width="25%">
                                           <asp:TextBox ID="txtcriteria" runat="server" Width="150px"></asp:TextBox>
                                       </td>
                                   </tr>
                               </table>
                           </asp:Panel>
                       </td>
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
                           <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_medium" 
                               Text="Clear" onclick="btnclear_Click" />
                       </td>
                       <td>
                           <asp:Button ID="btndelete" runat="server" CssClass="btnStyle_medium" 
                               Text="Delete" onclick="btndelete_Click" />
                       </td>
                   </tr>
                   <tr>
                       <td colspan="4">
                           &nbsp;</td>
                   </tr>
                   <tr>
                       <td colspan="4">
                           <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
                               Text="btntest" />
                       </td>
                   </tr>
                   <tr>
                       <td align="center" colspan="4">
                           <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                               CellPadding="4" ForeColor="#333333" GridLines="None" 
                               onrowcreated="GridView1_RowCreated" onrowdatabound="GridView1_RowDataBound" 
                               onselectedindexchanged="GridView1_SelectedIndexChanged">
                               <AlternatingRowStyle BackColor="White" />
                               <Columns>
                                   <asp:BoundField DataField="alloc_ID" HeaderText="AllocID" >
                                   <ItemStyle HorizontalAlign="Center" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="alloc_request" HeaderText="AllocRequest" >
                                   <ItemStyle HorizontalAlign="Center" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="frm_date" HeaderText="FromDate" >
                                   <ItemStyle HorizontalAlign="Center" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="to_date" HeaderText="ToDate" >
                                   <ItemStyle HorizontalAlign="Center" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="max_allocate" HeaderText="MaxAllocate" >
                                   <ItemStyle HorizontalAlign="Center" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="che_id" HeaderText="CheckID" >
                                   <ItemStyle HorizontalAlign="Center" />
                                   </asp:BoundField>
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

               </contenttemplate>
             </asp:UpdatePanel>
</asp:Content> 

