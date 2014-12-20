<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Fund Transfer Receipt.aspx.cs" Inherits="Fund_Transfer_Receipt" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel id="UpdatePanel1" runat="server">
 <contenttemplate>
  

     <table width="100%">
         <tr>
             <td width="100%" align="center">
                 <asp:Label ID="Label1" runat="server" Text="Fund Transfer Receipt" 
                     CssClass="heading" Width="812px"></asp:Label></td></tr>
                     <tr><td width="100%">
                     </td>
         </tr>
         
     </table>
  
      <asp:Panel ID="Panel1" runat="server" 
         GroupingText="Pending fund transfer issues">
     <table width="100%">
     <tr>
     <td width="100%" align="center">
     
      <asp:GridView ID="gvDeposit" runat="server" CellPadding="4" ForeColor="#333333" 
          GridLines="None" onrowcreated="gvDeposit_RowCreated" 
          onselectedindexchanged="gvDeposit_SelectedIndexChanged" Width="820px" 
             AutoGenerateColumns="False" onrowdatabound="gvDeposit_RowDataBound" 
             DataKeyNames="id">
          <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
          <Columns>
              <asp:BoundField DataField="id" HeaderText="id" />
              <asp:BoundField DataField="Transfer from" HeaderText="Transfer from" />
              <asp:BoundField DataField="Transfer to" HeaderText="Transfer to" />
              <asp:BoundField DataField="Amount" HeaderText="Amount" />
              <asp:BoundField DataField="Date" HeaderText="Date" />
              <asp:TemplateField>
                  <ItemTemplate>
                      <asp:LinkButton ID="lnkAccept" runat="server" onclick="lnkAccept_Click">Accept</asp:LinkButton>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField>
                  <ItemTemplate>
                      <asp:LinkButton ID="lnkReject" runat="server" onclick="lnkReject_Click">Reject</asp:LinkButton>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:CommandField SelectText="" ShowSelectButton="True" Visible="False" />
          </Columns>
          <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
          <EditRowStyle BackColor="#2461BF" />
          <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
          <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
          <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
              HorizontalAlign="Left" VerticalAlign="Top" />
          <AlternatingRowStyle BackColor="White" />
      </asp:GridView>
      </td>
     </tr>
     </table>
     <table width="100%">
     </table>
   </asp:Panel>
 <table width="100%">
     <tr>
     <td width="100%">
     
    
                                   <asp:Panel ID="pnlMessage" runat="server" _="" _designer:dtid="562958543355909" 
                                    CssClass="ModalWindow">
                                    <asp:Panel ID="Panel8" runat="server" BackColor="LightSteelBlue" 
                                        BorderStyle="Outset">
                                        <asp:Label ID="Label22" runat="server" Font-Bold="True" ForeColor="MediumBlue" 
                                            Text="Tsunami ARMS - Confirmation" Width="238px"></asp:Label>
                                    </asp:Panel>
                                    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                                        PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize" 
                                        TargetControlID="btnHidden">
                                    </cc1:ModalPopupExtender>
                                    &nbsp;
                                    <asp:Button ID="btnHidden" runat="server" style="DISPLAY: none" Text="Hidden" />
                                    &nbsp;
                                    <br />
                                    <asp:Panel ID="pnlYesNo" runat="server" Height="50px" Width="125px">
                                        <table style="WIDTH: 237px">
                                            <tbody>
                                                <tr>
                                                    <td align="center" colspan="1" style="HEIGHT: 18px">
                                                    </td>
                                                    <td align="center" colspan="3" style="WIDTH: 227px; HEIGHT: 18px">
                                                        <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Black" 
                                                            Text="Do you want to save?"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="HEIGHT: 15px">
                                                    </td>
                                                    <td style="WIDTH: 208px; HEIGHT: 15px">
                                                    </td>
                                                    <td style="WIDTH: 13px; HEIGHT: 15px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="HEIGHT: 26px">
                                                    </td>
                                                    <td align="center" style="WIDTH: 208px; HEIGHT: 26px">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btnYes" runat="server" CausesValidation="False" 
                                                            CssClass="btnStyle" onclick="btnYes_Click" Text="Yes" Width="50px" />
                                                        <asp:Button ID="btnNo" runat="server" CausesValidation="False" 
                                                            CssClass="btnStyle" onclick="btnNo_Click" Text="No" Width="50px" />
                                                        &nbsp;</td>
                                                    <td align="center" style="WIDTH: 13px; HEIGHT: 26px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="HEIGHT: 18px">
                                                    </td>
                                                    <td align="center" style="WIDTH: 208px; HEIGHT: 18px">
                                                    </td>
                                                    <td align="center" style="WIDTH: 13px; HEIGHT: 18px">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlOk" runat="server" Height="50px" Width="125px">
                                        <table style="WIDTH: 237px">
                                            <tbody>
                                                <tr>
                                                    <td align="center" colspan="1">
                                                    </td>
                                                    <td align="center" colspan="3" style="WIDTH: 224px">
                                                        <asp:Label ID="lblOk" runat="server" Font-Size="Small" ForeColor="Black" 
                                                            Text="Do you want to ?"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="HEIGHT: 10px">
                                                    </td>
                                                    <td style="HEIGHT: 15px">
                                                    </td>
                                                    <td style="HEIGHT: 10px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="center">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnOk" runat="server" CausesValidation="False" 
                                                            CssClass="btnStyle" onclick="btnOk_Click" Text="OK" Width="50px" />
                                                        &nbsp;
                                                    </td>
                                                    <td align="center">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="center">
                                                    </td>
                                                    <td align="center">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                                 </td>
     </tr>
     </table>

 </contenttemplate>
 </asp:UpdatePanel>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

