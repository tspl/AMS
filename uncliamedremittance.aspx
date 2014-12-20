<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="uncliamedremittance.aspx.cs" Inherits="uncliamedremittance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
       <asp:Label ID="lblhead" runat="server" 
                CssClass="heading" Font-Bold="True" 
                Font-Size="Medium" Text="UNCLAIMED REMITTANCE"></asp:Label>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table width="100%">
            <tr>
                <td colspan="4" width="100%">
                  
               <asp:Panel ID="pnlunlaimed" runat="server">
                  <table width="100%">
            <tr>         
                <td width="25%">               
                    Date</td>
                <td width="25%">
                    <asp:TextBox ID="txtdate" runat="server" AutoPostBack="True" 
                       
                        tabIndex="16" Width="150px"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MM-yyyy" 
                        TargetControlID="txtdate">
                    </cc1:CalendarExtender>
                </td>
                <td width="25%">
                    Season</td>
                <td width="25%">
                    <asp:DropDownList ID="ddlseason" runat="server" Width="150px" 
                        DataTextField="seasonname" DataValueField="season_sub_id" Enabled="False">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="25%">
                    Uncliamed Amount</td>
                <td width="25%">
                    <asp:TextBox ID="txtremitamount" runat="server" Width="150px" 
                        ontextchanged="txtremitamount_TextChanged"></asp:TextBox>
                </td>
                <td width="25%">
                    Unclaimed deposit</td>
                <td width="25%">
                    <asp:TextBox ID="txtunclaimed" runat="server" ReadOnly="True" Width="150px"></asp:TextBox>
                </td>
            </tr>
                      <tr>
                          <td width="25%">
                              &nbsp;</td>
                          <td width="25%">
                              <asp:Button ID="btnremit" runat="server" CssClass="btnStyle_medium" 
                                  Text="Remit" onclick="btnremit_Click" />
                          </td>
                          <td width="25%">
                              <asp:Button ID="btnremit0" runat="server" CssClass="btnStyle_medium" 
                                  onclick="btnremit0_Click" Text="Clear" />
                          </td>
                          <td width="25%">
                              <asp:Button ID="btnunclaimed" runat="server" CssClass="btnStyle_large" 
                                  onclick="btnunclaimed_Click" Text="Unlaimed ledger" />
                          </td>
                      </tr>
                   </table>
                   <asp:Panel ID="pnlledger" runat="server" Visible="False">
                       <table width="100%">
                           <tr>
                            <td width="12%">
                                   From date</td>
                               <td width="13%">
                                   <asp:TextBox ID="txtfromdate" runat="server" AutoPostBack="True" 
                                     
                                       tabIndex="16" Width="150px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                   <cc1:CalendarExtender ID="txtfromdate_CalendarExtender" runat="server" 
                                       Format="dd-MM-yyyy" TargetControlID="txtfromdate">
                                   </cc1:CalendarExtender>
                               </td>
                               <td width="12%">
                                   To date</td>
                               <td width="13%">
                                   <asp:TextBox ID="txttodate" runat="server" AutoPostBack="True" 
                                     
                                       tabIndex="16" Width="150px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                   <cc1:CalendarExtender ID="txttodate_CalendarExtender" runat="server" 
                                       Format="dd-MM-yyyy" TargetControlID="txttodate">
                                   </cc1:CalendarExtender>
                               </td>
                               <td width="25%">
                                   <asp:Button ID="btngenerateledger" runat="server" CssClass="btnStyle_large" 
                                       onclick="Button3_Click" Text="Generate ledger" />
                               </td>
                               <td width="25%">
                                   &nbsp;</td>
                           </tr>
                       </table>
                   </asp:Panel>
             </asp:Panel>
               &nbsp;</td>
            </tr>
            <tr>
                <td width="100%" colspan="4">
                    &nbsp;</td>
            </tr>
              
                  

        </table>
    </ContentTemplate>
    
 <Triggers>
  <asp:PostBackTrigger ControlID="btngenerateledger" />
  </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

