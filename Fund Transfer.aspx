<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Fund Transfer.aspx.cs" Inherits="Fund_Transfer" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel id="UpdatePanel1" runat="server">
 <contenttemplate>
     <asp:Panel ID="Panel1" runat="server">
     <table width="100%">
     <tr>
     <td width="100%" align="center">
     
         <asp:Label ID="Label1" runat="server" CssClass="heading" 
             Text="Fund Transfer Issue" Width="800px"></asp:Label>
     
     </td>
     </tr>
     </table>
     </asp:Panel>
     <asp:Panel ID="Panel2" runat="server" GroupingText="Secerity deposit">
    <table width="100%">
    <tr>
     <td width="100%" align="left">
     
     </td>
      </tr>
       <tr>
     <td width="100%" align="center">
     
         <asp:GridView ID="gvDeposit" runat="server" CellPadding="4" ForeColor="#333333" 
             GridLines="None" 
             Width="700px" onrowcreated="gvDeposit_RowCreated" 
             onselectedindexchanged="gvDeposit_SelectedIndexChanged">
             <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
             <Columns>
                 <asp:CommandField SelectText="" ShowSelectButton="True" />
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

    </asp:Panel>
     <asp:Panel ID="Panel3" runat="server" GroupingText="Transfer Fund">
     <table width="100%">
     <tr>
     <td width="20%" style="height: 24px">
     </td>
     <td width="30%" style="height: 24px">
     </td>
     <td width="20%" style="height: 24px">
     </td>
     <td width="30%" style="height: 24px">
     </td>
     </tr>
         <tr>
             <td style="height: 23px" width="20%">
             </td>
             <td align="center" style="height: 23px" width="30%">
                 <asp:Label ID="Label2" runat="server" Text="Transfer from" Width="120px"></asp:Label>
             </td>
             <td style="height: 23px" width="20%">
                 <asp:DropDownList ID="ddlFrom" runat="server" Width="160px" 
                     DataTextField="counter_no" DataValueField="counter_id" AutoPostBack="True" 
                     onselectedindexchanged="ddlFrom_SelectedIndexChanged">
                 </asp:DropDownList>
             </td>
             <td style="height: 23px" width="30%">
             </td>
         </tr>
         <tr>
             <td width="20%">
                 &nbsp;</td>
             <td align="center" width="30%">
                 <asp:Label ID="Label3" runat="server" Text="Transfer to" Width="120px"></asp:Label>
             </td>
             <td width="20%">
                 <asp:DropDownList ID="ddlTO" runat="server" Width="160px" 
                     DataTextField="counter_no" DataValueField="counter_id" AutoPostBack="True" 
                     onselectedindexchanged="ddlTO_SelectedIndexChanged">
                 </asp:DropDownList>
             </td>
             <td width="30%">
                 &nbsp;</td>
         </tr>
         <tr>
             <td width="20%">
                 &nbsp;</td>
             <td width="30%" align="center">
                 <asp:Label ID="Label4" runat="server" Text="Amount" Width="120px"></asp:Label>
             </td>
             <td width="20%">
                 <asp:TextBox ID="txtAmount" runat="server" Width="160px" AutoPostBack="True" 
                     ontextchanged="txtAmount_TextChanged"></asp:TextBox>
             </td>
             <td width="30%">
                 &nbsp;</td>
         </tr>
         <tr>
             <td width="20%">
                 &nbsp;</td>
             <td align="center" width="30%">
                 &nbsp;</td>
             <td width="20%">
                 &nbsp;</td>
             <td width="30%">
                 &nbsp;</td>
         </tr>
         <tr>
             <td width="20%">
                 &nbsp;</td>
             <td width="30%" align="center">
                 <asp:Button ID="btnTransfer" runat="server" CssClass="btnStyle_medium" 
                     Text="Transfer" onclick="btnTransfer_Click" Width="100px" />
                     </td>
             <td width="20%">
                 <asp:Button ID="btnClear" runat="server" CssClass="btnStyle_medium" 
                     Text="Clear" onclick="btnClear_Click" />
             </td>
             <td width="30%">
                 &nbsp;</td>
         </tr>
         
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

