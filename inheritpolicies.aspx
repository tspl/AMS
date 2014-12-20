<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="inheritpolicies.aspx.cs" Inherits="inheritpolicies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="style4">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="16pt" 
                            ForeColor="DarkBlue" Text="Inherit policies" Width="244px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="50%">
                        <asp:Panel ID="Panel6" runat="server" GroupingText="Inherit policy">
                            <table class="style4">
                                <tr>
                                    <td width="50%">
                                        Season to be inherited</td>
                                    <td width="50%">
                                        <asp:DropDownList ID="ddlinhseasons" runat="server" DataTextField="season" 
                                            DataValueField="id" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="50%">
                                        Policies</td>
                                    <td width="50%">
                                        <asp:DropDownList ID="ddlpolicies" runat="server" Width="100%">
                                            <asp:ListItem Value="-1">--ALL policies--</asp:ListItem>
                                            <asp:ListItem Value="1">Billing and service policy</asp:ListItem>
                                            <asp:ListItem Value="2">Cashier and bank remitance</asp:ListItem>
                                            <asp:ListItem Value="3">Reservation policy</asp:ListItem>
                                            <asp:ListItem Value="4">Room allocation policy</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="50%">
                                        &nbsp;</td>
                                    <td width="50%">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td width="50%">
                        <asp:Panel ID="pnlperiod" runat="server" GroupingText="Policy Period" 
                            Width="100%">
                            <TABLE>
                                <tbody>
                                    <tr>
                                        <td style="WIDTH: 139px">
                                            <asp:Label ID="lblpolicyseasonfrom" runat="server" Text="Applicable Season" 
                                                Width="133px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:ListBox ID="lstSeasons" runat="server" Height="50px" Width="148px">
                                            </asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 139px; HEIGHT: 1px; TEXT-ALIGN: left" valign="top">
                                            <asp:Label ID="lblpolicyperiodfrom" runat="server" Height="22px" 
                                                Text="Policy applicable from" Width="135px"></asp:Label>
                                        </td>
                                        <td style="FONT-SIZE: 100%; WIDTH: 97px; HEIGHT: 1px" valign="top">
                                            <asp:TextBox ID="txtPolicyperiodFrom" runat="server" AutoPostBack="True" 
                                                OnTextChanged="txtPolicyperiodFrom_TextChanged" Width="146px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtPolicyperiodFrom_CalendarExtender" runat="server" 
                                                Format="dd/MM/yyyy" TargetControlID="txtPolicyperiodFrom">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 139px">
                                            <asp:Label ID="lblpolicyperiodto" runat="server" Text="Policy applicable to" 
                                                Width="132px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPolicyperiodTo" runat="server" AutoPostBack="True" 
                                                OnTextChanged="txtPolicyperiodTo_TextChanged" Width="146px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtPolicyperiodTo_CalendarExtender" runat="server" 
                                                Format="dd/MM/yyyy" TargetControlID="txtPolicyperiodTo">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                </tbody>
                            </TABLE>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="pnlbuttons" runat="server">
                            <table class="style4">
                                <tr>
                                    <td width="10%">
                                        &nbsp;</td>
                                    <td width="10%">
                                        <asp:Button ID="btnsave" runat="server" CssClass="btnStyle_small" Text="Save" 
                                            onclick="btnsave_Click" />
                                    </td>
                                    <td width="10%">
                                        <asp:Button ID="btnedit" runat="server" CssClass="btnStyle_small" Text="Edit" />
                                    </td>
                                    <td width="10%">
                                        <asp:Button ID="btndelete" runat="server" CssClass="btnStyle_small" 
                                            Text="Delete" />
                                    </td>
                                    <td width="10%">
                                        <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_small" 
                                            Text="Clear" />
                                    </td>
                                    <td width="10%">
                                        &nbsp;</td>
                                    <td width="10%">
                                        &nbsp;</td>
                                    <td width="10%">
                                        &nbsp;</td>
                                    <td width="10%">
                                        &nbsp;</td>
                                    <td width="10%">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" 
                            TargetControlID="txtpolicyperiodfrom">
                        </cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" 
                            TargetControlID="txtpolicyperiodto">
                        </cc1:CalendarExtender>
                        <BR />
                        <BR />
                        <asp:Button ID="btnHidden" runat="server" onclick="btnHidden_Click" 
                            style="DISPLAY: none" Text="Hidden" />
                        <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                            PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize" 
                            TargetControlID="btnHidden">
                        </cc1:ModalPopupExtender>
                        <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" 
                            OnTextChanged="TextBox1_TextChanged" Visible="False"></asp:TextBox>
                        <asp:Label ID="Label2" runat="server" Text="Label" Visible="False"></asp:Label>
                        <BR />
                        <BR />
                        <asp:Panel ID="pnlMessage" runat="server" _="" _designer:dtid="562958543355909" 
                            CssClass="ModalWindow">
                            <asp:Panel ID="Panel7" runat="server" BackColor="LightSteelBlue" 
                                BorderStyle="Outset">
                                <asp:Label ID="lblHead" runat="server" Font-Bold="True" ForeColor="MediumBlue" 
                                    Text="Tsunami ARMS -"></asp:Label>
                            </asp:Panel>
                            <BR />
                            <asp:Panel ID="pnlYesNo" runat="server" Height="50px" Width="125px">
                                <table style="WIDTH: 237px">
                                    <tbody>
                                        <tr>
                                            <td align="center" colspan="1">
                                            </td>
                                            <td align="center" colspan="3">
                                                <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Black" 
                                                    Text="Do you want to save?"></asp:Label>
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
                                                &nbsp;
                                                <asp:Button ID="btnYes" runat="server" CausesValidation="False" 
                                                    CssClass="btnStyle" onclick="btnYes_Click" Text="Yes" Width="50px" />
                                                <asp:Button ID="btnNo" runat="server" CausesValidation="False" 
                                                    CssClass="btnStyle" onclick="btnNo_Click" Text="No" Width="50px" />
                                                &nbsp;</td>
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
                            <asp:Panel ID="pnlOk" runat="server" Height="50px" Width="125px">
                                <table style="WIDTH: 237px">
                                    <tbody>
                                        <tr>
                                            <td align="center" colspan="1">
                                            </td>
                                            <td align="center" colspan="3">
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
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnOk" runat="server" CausesValidation="False" 
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
                        <BR />
                        <BR />
                        <BR />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

