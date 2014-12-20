<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="display.aspx.cs" Inherits="display" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager id="ScriptManager1" runat="server"> </asp:ScriptManager>
<asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table class="style4">
        <tr><td colspan="8">
            
            <asp:Label ID="lblBuild" runat="server"></asp:Label>
            
            </td></tr>
            <tr>
            
                <td colspan="8" style="height: 26px">
                    <asp:Panel ID="pnldis" runat="server">
                        <asp:DataList ID="DataList1" runat="server" BackColor="#DEBA84" 
                            BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            CellSpacing="2" GridLines="Both" Height="100%" RepeatColumns="12" Width="100%">
                            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                            <ItemStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                            <ItemTemplate>
                                Room No<strong>:<asp:Label ID="Label6" runat="server" 
                                    Text='<%# Eval("roomno") %>'></asp:Label>
                                    <asp:Label ID="Label2" runat="server" 
                                    Text='<%# Eval("roomstatus") %>' Visible="false"></asp:Label>
                                    
                                <br />

                                </strong>
                            </ItemTemplate>
                            <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                        </asp:DataList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="8"><asp:Timer ID="timer1" runat="server" OnTick="timer1_Tick" 
                        Interval="60000"></asp:Timer>
                        
                        <asp:Timer ID="timer2" runat="server" OnTick="timer2_Tick" 
                        Interval="600000"></asp:Timer></td>
                        
            </tr>
            <tr>
                <td width="10%" align="right">
                    <asp:TextBox ID="txtvacate" runat="server" BackColor="White" ReadOnly="True" 
                        Width="25%"></asp:TextBox>
                </td>
                <td width="10%" align="left">
                    <asp:Label ID="lblvac" runat="server" Text="Vacant"></asp:Label>
                </td>
                <td width="10%">
                    <asp:TextBox ID="txtvacate0" runat="server" BackColor="#FF3300" ReadOnly="True" 
                        Width="25%"></asp:TextBox>
                </td>
                <td width="10%">
                    <asp:Label ID="lblres" runat="server" Text="Reserved"></asp:Label>
                </td>
                <td width="10%">
                    <asp:TextBox ID="txtvacate1" runat="server" BackColor="Black" ReadOnly="True" 
                        Width="25%"></asp:TextBox>
                </td>
                <td width="10%">
                    <asp:Label ID="lblvac1" runat="server" Text="Blocked"></asp:Label>
                </td>
                <td width="10%">
                    <asp:TextBox ID="txtvacate2" runat="server" BackColor="#0033CC" ReadOnly="True" 
                        Width="25%"></asp:TextBox>
                </td>
                <td width="10%">
                    <asp:Label ID="lblvac2" runat="server" Text="Occupied"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    
                    <asp:Button ID="btnhold" runat="server" CssClass="btnStyle_medium" 
                        onclick="btnhold_Click" Text="Hold" />
                    &nbsp;<asp:Button ID="Button3" runat="server" CssClass="btnStyle_medium" 
                        Text="Button" onclick="Button3_Click" Visible="False" />
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    &nbsp;</td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
    <asp:PostBackTrigger ControlID="btnhold" />
    <asp:PostBackTrigger ControlID="Button3" />
    </Triggers>
</asp:UpdatePanel>

   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

