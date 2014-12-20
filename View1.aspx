<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="View1.aspx.cs" Inherits="View1" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <asp:Label ID="lblHeading" runat="server" Text="Label" Font-Names="Arial" Font-Size="11pt" ForeColor="MidnightBlue" Height="40px" Width="266px"></asp:Label><br />
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<asp:GridView id="dtgview" runat="server" Width="100%" ForeColor="#333333" AllowSorting="True" OnRowCreated="dtgview_RowCreated" OnRowDataBound="dtgview_RowDataBound" OnSorting="dtgview_Sorting" PageSize="15" ShowFooter="True" CellPadding="4" GridLines="None">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:TemplateField HeaderText="Sl.No:"><ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView> 
</contenttemplate>
    </asp:UpdatePanel>
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

