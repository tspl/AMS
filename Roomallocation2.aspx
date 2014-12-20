<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Roomallocation2.aspx.cs" Inherits="Roomallocation2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>




      




        <table class="style4">
            <tr>
                <td colspan="4">
                    <asp:Panel ID="pnlt" runat="server">



                      <table class="style4">
            <tr>
                <td align="center" colspan="4">
                    <strong><span style="FONT-WEIGHT: bold; FONT-SIZE: 14pt; COLOR: #003399">Room&nbsp;&nbsp; 
                        Allotment</span></strong></td>
            </tr>
            <tr>
                <td align="center" width="25%">
                    Reservation date</td>
                <td width="25%">
                    <asp:DropDownList ID="cmbReserve" runat="server" AutoPostBack="True" 
                        DataTextField="reservedate" DataValueField="reservedate" 
                        onselectedindexchanged="cmbReserve_SelectedIndexChanged" Width="153px">
                    </asp:DropDownList>
                </td>
                <td width="25%">
                    Category</td>
                <td width="25%">
                    <asp:DropDownList ID="ddlcat" runat="server" AutoPostBack="True" 
                        DataTextField="room_cat_name" DataValueField="room_cat_id" 
                        onselectedindexchanged="ddlcat_SelectedIndexChanged" Width="153px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="justify">
                    Type</td>
                <td>
                    <asp:DropDownList ID="ddltype" runat="server" Width="153px" 
                        DataTextField="TYPE" DataValueField="id" 
                        onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    Building</td>
                <td>
                    <asp:DropDownList ID="ddlbuild" runat="server" AutoPostBack="True" 
                        DataTextField="buildingname" DataValueField="build_id" 
                        onselectedindexchanged="ddlbuild_SelectedIndexChanged" Width="153px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Label ID="lbl1" runat="server" Text="Alloted Room List"></asp:Label>
                </td>
            </tr>
                          <tr>
                              <td align="center" colspan="4">
                                  Total allotments =<asp:Label ID="lblreserve" runat="server"></asp:Label>
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Balance allotments =<asp:Label ID="lblallot" runat="server"></asp:Label>
                                  <asp:Label runat="server"></asp:Label>
                              </td>
                          </tr>
            <tr>
                <td align="left" colspan="4">
                    <asp:GridView ID="gdshow" runat="server" CellPadding="4" ForeColor="#333333" 
                        AllowPaging="true" PageSize ="15"
                        GridLines="None" Width="900px" 
                        onpageindexchanging="gdshow_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                            BorderStyle="Dotted" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4" width="100%">
                    <asp:GridView ID="gdbind" runat="server" Width="900px" AllowPaging="True" PageSize="20"
                        AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                        GridLines="None" onpageindexchanging="gdbind_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkj" runat="server" AutoPostBack="True" 
                                        oncheckedchanged="chkj_CheckedChanged" Text="Select All" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkacc" runat="server" AutoPostBack="True" 
                                        oncheckedchanged="chkacc_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Roomno" DataField="roomno">
                            </asp:BoundField>
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button4" runat="server" CssClass="btnStyle_medium" 
                        Text="Allot" onclick="Button4_Click" />
                </td>
                <td>
                    <asp:Button ID="Button3" runat="server" CssClass="btnStyle_medium" 
                        Text="Remove" onclick="Button3_Click" />
                </td>
                <td>
                    <asp:Button ID="Button5" runat="server" CssClass="btnStyle_medium" 
                        onclick="Button5_Click" Text="View" />
                </td>
                <td>
                    <asp:Panel ID="Panel6" runat="server">
                    </asp:Panel>
                    <asp:Button ID="btnrealloc" runat="server" CssClass="btnStyle_medium" 
                        onclick="btnrealloc_Click" Text="Reallocate" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Panel ID="pnlrec" runat="server">
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Panel ID="pnlr" runat="server">
                        <table class="style4">
                            <tr>
                                <td colspan="4" width="25%">
                                    <strong><span style="FONT-WEIGHT: bold; FONT-SIZE: 14pt; COLOR: #003399">Room&nbsp;&nbsp; 
                                    Reallotment</span></strong></td>
                            </tr>
                            <tr>
                                <td width="25%">
                                    Category</td>
                                <td>
                                    <asp:DropDownList ID="ddlcid" runat="server" AutoPostBack="True" 
                                        DataTextField="room_cat_name" DataValueField="room_cat_id" 
                                        onselectedindexchanged="ddlcid_SelectedIndexChanged" Width="153px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    Building name</td>
                                <td width="25%">
                                    <asp:DropDownList ID="ddlbn" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddlbn_SelectedIndexChanged" Width="153px">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlbui" runat="server" DataTextField="buildingname" 
                                        DataValueField="build_id" Width="153px" AutoPostBack="True" 
                                        onselectedindexchanged="ddlbui_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td width="25%">
                                    &nbsp;</td>
                                <td width="25%">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    Room No</td>
                                <td width="25%">
                                    <asp:DropDownList ID="ddlrno" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddlrno_SelectedIndexChanged" Width="153px">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlr" runat="server" DataTextField="roomno" 
                                        DataValueField=" room_id" Width="153px">
                                        <asp:ListItem Value="-1">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="25%">
                                    &nbsp;</td>
                                <td width="25%">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:GridView ID="gdre" runat="server" CellPadding="4" ForeColor="#333333" 
                                        GridLines="None" Width="875px">
                                        <AlternatingRowStyle BackColor="White" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="btnun" runat="server" CssClass="btnStyle_medium" 
                                        onclick="btnun_Click" Text="Unallocate" />
                                    <asp:Button ID="btna" runat="server" CssClass="btnStyle_medium" 
                                        onclick="btna_Click" Text="Allocate" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>




      




    </ContentTemplate> 
    </asp:UpdatePanel>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">

</asp:Content>
   

