<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Payment reconcilation.aspx.cs" Inherits="Payment_reconcilation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <table style="width: 100%">
            <tr>
                <td align="center">
                    <asp:Label ID="Label1" runat="server" CssClass="heading" 
                        Text="Payment Reconcillation"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="100%">
        <tr>
        <td width="25%">
        
        </td>
        <td width="25%">
        
        </td>
        <td width="25%">
        
        </td>
        <td width="25%">
        
        </td>
        </tr>
            <tr>
                <td style="height: 44px" width="25%">
                    <asp:Label ID="Label2" runat="server" Text="Reservation type"></asp:Label>
                </td>
                <td style="height: 44px" width="25%">
                    <asp:DropDownList ID="ddlReservation" runat="server" Width="163px" 
                        AutoPostBack="True" 
                        onselectedindexchanged="ddlReservation_SelectedIndexChanged">
                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                        <asp:ListItem Value="0">General</asp:ListItem>
                        <asp:ListItem Value="1">TDB</asp:ListItem>
                        <asp:ListItem Value="2">Donor Paid</asp:ListItem>
                        <asp:ListItem Value="3">Donor Free</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="height: 44px" width="25%">
                    <asp:Label ID="Label7" runat="server" Text="DD Date"></asp:Label>
                </td>
                <td style="height: 44px" width="25%">
                    <asp:TextBox ID="txtDDdate" runat="server" Width="162px"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtDDdate_CalendarExtender" runat="server" 
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDDdate">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td style="height: 44px" width="25%">
                    <asp:Label ID="Label3" runat="server" Text="Reservation No"></asp:Label>
                </td>
                <td style="height: 44px" width="25%">
                    <asp:TextBox ID="txtResNo" runat="server" Width="162px"></asp:TextBox>
                </td>
                <td style="height: 44px" width="25%">
                    <asp:Label ID="Label8" runat="server" Text="Bank name"></asp:Label>
                </td>
                <td style="height: 44px" width="25%">
                    <asp:TextBox ID="txtBank" runat="server" Width="162px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 44px" width="25%">
                    <asp:Label ID="Label4" runat="server" Text="Devotee name"></asp:Label>
                </td>
                <td style="height: 44px" width="25%">
                    <asp:TextBox ID="txtName" runat="server" Width="162px"></asp:TextBox>
                </td>
                <td style="height: 44px" width="25%">
                    <asp:Label ID="Label9" runat="server" Text="Amount committed"></asp:Label>
                    &nbsp;</td>
                <td style="height: 44px" width="25%">
                    <asp:TextBox ID="txtAmount" runat="server" Width="162px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="25%">
                    <asp:Label ID="Label5" runat="server" Text="Payment mode"></asp:Label>
                </td>
                <td width="25%">
                    <asp:DropDownList ID="ddlMode" runat="server" Width="163px" 
                        DataTextField="payment_mode" DataValueField="payment_id">
                    </asp:DropDownList>
                </td>
                <td width="25%">
                    <asp:Label ID="Label10" runat="server" Text="Amount Received"></asp:Label>
                </td>
                <td width="25%">
                    <asp:TextBox ID="txtAmount2" runat="server" Width="162px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 59px" width="25%">
                    <asp:Label ID="Label6" runat="server" Text="DD NO"></asp:Label>
                </td>
                <td style="height: 59px" width="25%">
                    <asp:TextBox ID="txtDDno" runat="server" Width="162px"></asp:TextBox>
                </td>
                <td style="height: 59px" width="25%">
                    <asp:Label ID="Label11" runat="server" Text="Bank trans no"></asp:Label>
                </td>
                <td style="height: 59px" width="25%">
                    <asp:TextBox ID="txtTrans" runat="server" Width="162px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="25%" style="height: 24px">
                    </td>
                <td width="25%" style="height: 24px">
                    </td>
                <td width="25%" style="height: 24px">
                    </td>
                <td width="25%" style="height: 24px">
                    </td>
            </tr>
            <tr>
                <td align="center" width="25%">
                    <asp:Button ID="btnView" runat="server" CssClass="btnStyle_medium" 
                        Text="View" onclick="btnView_Click" />
                </td>
                <td align="center" width="25%">
                    <asp:Button ID="btnDD" runat="server" CssClass="btnStyle_medium" 
                        Text="DD Received" onclick="btnDD_Click" />
                </td>
                <td align="center" width="25%">
                    <asp:Button ID="btnConfirm" runat="server" CssClass="btnStyle_large" 
                        Text="Confirm reservation" onclick="btnConfirm_Click" />
                </td>
                <td align="center" width="25%">
                    <asp:Button ID="btnCancel" runat="server" CssClass="btnStyle_large" 
                        Text="Cancel reservation" onclick="btnCancel_Click" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlDetails" runat="server" GroupingText="Reservaton Details" 
            Visible="False">
       <asp:GridView id="gvDetails" runat="server" Width="840px" ForeColor="#333333" 
                CellPadding="4" GridLines="None" __designer:wfdid="w14" 
                onrowcreated="gvDetails_RowCreated" 
                onselectedindexchanged="gvDetails_SelectedIndexChanged" 
                onrowdatabound="gvDetails_RowDataBound">
                <EmptyDataRowStyle HorizontalAlign="Center" />
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<RowStyle BackColor="#EFF3FB" HorizontalAlign="Center"></RowStyle>
                <Columns>
                    <asp:CommandField SelectText="" ShowSelectButton="True" />
                </Columns>
<EditRowStyle BackColor="#2461BF"></EditRowStyle>
<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True" 
                    HorizontalAlign="Center"></SelectedRowStyle>
<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>
<AlternatingRowStyle BackColor="White" HorizontalAlign="Center"></AlternatingRowStyle>
</asp:GridView>
        </asp:Panel>

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

    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

