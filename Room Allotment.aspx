<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Room Allotment.aspx.cs" Inherits="Room_Allotment" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">    
        <contenttemplate>
            <table style="width: 100%">
                <tr>
                    <td colspan="3">
                        <asp:Label ID="Label1" runat="server" Text="Reserve Date"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:DropDownList ID="cmbReserve" runat="server" 
                            DataTextField="reservedate" DataValueField="reservedate" 
                            onselectedindexchanged="cmbReserve_SelectedIndexChanged" 
                            Width="150px">
                        <asp:ListItem Value="-1">--Select--</asp:ListItem>                      
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblswaminame" runat="server" 
                            Text="Swami name" Width="87px"></asp:Label>
                    </td>
                    <td colspan="3" align="right">
                        <asp:DropDownList ID="cmbSwaminame" runat="server" 
                            AutoPostBack="True" DataTextField="swaminame" 
                            DataValueField="reserve_id" 
                            onselectedindexchanged="cmbSwaminame_SelectedIndexChanged" 
                            Width="140px">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Reserve Type"></asp:Label>
                    </td>
                    <td colspan="3" align="right">
                        <asp:DropDownList ID="cmbreservetype" runat="server" 
                            DataTextField="TYPE" DataValueField="id" 
                            onselectedindexchanged="cmbreservetype_SelectedIndexChanged" 
                            style="margin-top: 0px" Width="150px" 
                            AutoPostBack="True">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>                      
                        </asp:DropDownList>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="Label5" runat="server" Text="Building Name"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:DropDownList ID="cmbBuild" runat="server" 
                            onselectedindexchanged="cmbBuild_SelectedIndexChanged" 
                            Width="140px" DataTextField="buildingname" 
                            DataValueField="build_id" AutoPostBack="True" 
                            Height="22px" tabIndex="13">
                        <asp:ListItem Value="-1">--Select--</asp:ListItem>                      
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="cmbBuild_ListSearchExtender" 
                            runat="server" TargetControlID="cmbBuild">
                        </cc1:ListSearchExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblroomcategory" runat="server" 
                            Text="Room Category"></asp:Label>
                    </td>
                    <td colspan="3" align="right">
                        <asp:DropDownList ID="cmbroomcategory" runat="server" 
                            AutoPostBack="True" DataTextField="room_cat_name" 
                            DataValueField="room_cat_id" 
                            onselectedindexchanged="cmbroomcategory_SelectedIndexChanged" 
                            Width="150px">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="cmbroomcategory_ListSearchExtender" 
                            runat="server" Enabled="True" 
                            TargetControlID="cmbroomcategory">
                        </cc1:ListSearchExtender>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblrooms" runat="server" Text="Room No"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:DropDownList ID="cmbRooms" runat="server" AutoPostBack="True" 
                            DataTextField="roomno" DataValueField="room_id" 
                            OnSelectedIndexChanged="cmbRooms_SelectedIndexChanged" tabIndex="14" 
                            Width="140px">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="cmbRooms_ListSearchExtender" runat="server" 
                            TargetControlID="cmbRooms">
                        </cc1:ListSearchExtender>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:Label ID="lblroomcategory0" runat="server" Text="Reservation No."></asp:Label>
                    </td>
                    <td colspan="2" align="right">
                        <asp:DropDownList ID="ddlreservno" runat="server" AutoPostBack="True" 
                            DataTextField="reserve_no" DataValueField="reserve_no" 
                            onselectedindexchanged="ddlreservno_SelectedIndexChanged" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        &nbsp;</td>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnAllot" runat="server" CssClass="btnStyle_small" 
                            onclick="btnAllot_Click" Text="Allot" />
                    </td>
                    <td colspan="2">
                        <asp:Button ID="btnViewAllot" runat="server" CssClass="btnStyle_medium" 
                            onclick="btnViewAllot_Click" Text="View Allot" />
                    </td>
                    <td colspan="2">
                        <asp:Button ID="btnView" runat="server" CssClass="btnStyle_small" 
                            onclick="btnView_Click" Text="View" />
                    </td>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnClear" runat="server" CssClass="btnStyle_small" 
                            onclick="btnClear_Click" Text="Clear" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3" width="33%">
                        &nbsp;</td>
                    <td align="center" colspan="2" width="34%">
                        &nbsp;</td>
                    <td align="center" colspan="3" width="33%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:Label ID="Label3" runat="server" Text="Reserve Mode" 
                            Visible="False"></asp:Label>
                        <asp:DropDownList ID="cmbreservemode" runat="server" 
                            onselectedindexchanged="cmbreservemode_SelectedIndexChanged" 
                            style="margin-top: 0px" Visible="False" Width="140px">
                        </asp:DropDownList>
                    </td>
                    <td align="center" colspan="4">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="8">
                        <asp:GridView ID="dgAllot" runat="server" CellPadding="4" 
                            ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" 
                                ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" 
                                ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" 
                                HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" 
                                ForeColor="#333333" />                     
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="8">
                        <asp:GridView ID="dgReserve" runat="server" 
                            __designer:wfdid="w14" AutoGenerateColumns="False" 
                            CellPadding="4" DataKeyNames="ReservationNo" 
                            ForeColor="#333333" GridLines="None" HorizontalAlign="Left" 
                            OnPageIndexChanging="dgReserve_PageIndexChanging" 
                            OnRowCreated="dgReserve_RowCreated" 
                            OnSelectedIndexChanged="dgReserve_SelectedIndexChanged" 
                            Width="849px">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" 
                                ForeColor="White" />
                            <Columns>
                                <asp:CommandField SelectText="" ShowSelectButton="True" />
                                <asp:BoundField DataField="ReserveId" HeaderText="Reserve Id" />
                                <asp:BoundField DataField="ReservationNo" 
                                    HeaderText="Reservation No" />
                                <asp:BoundField DataField="swaminame" HeaderText="Swami Name" />
                                <asp:BoundField DataField="PassNo" HeaderText="Pass No" 
                                    Visible="False" />
                                <asp:BoundField DataField="Customer" 
                                    HeaderText="Customer" />
                                <asp:BoundField DataField="RoomNo" HeaderText="Room No" />
                                <asp:BoundField DataField="Building" 
                                    HeaderText="Building" />
                                <asp:BoundField DataField="ReservedDate" 
                                    HeaderText="Reserved Date" />
                                <asp:BoundField DataField="ExpectedVecatingDate" 
                                    HeaderText="Expected Vacating Date" />
                            </Columns>
                            <RowStyle BackColor="#EFF3FB" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" 
                                ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" 
                                HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" 
                                ForeColor="White" HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="8">
                    <br />
                    <asp:Panel id="pnlMessage" runat="server" __designer:dtid="562958543355914" CssClass="ModalWindow" __designer:wfdid="w173" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel6" runat="server" BackColor="LightSteelBlue" __designer:dtid="562958543355915" __designer:wfdid="w174" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Width="197px" Font-Bold="True" Text="Tsunami ARMS - Confirmation" __designer:dtid="562958543355916" __designer:wfdid="w46" ForeColor="MediumBlue"></asp:Label> <asp:Label id="lblHead2" runat="server" Width="191px" Font-Bold="True" Text="Tsunami ARMS - Warning" __designer:wfdid="w56" ForeColor="MediumBlue"></asp:Label><BR /></asp:Panel> <asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px" __designer:dtid="562958543355918" __designer:wfdid="w177"><TABLE style="WIDTH: 237px" __designer:dtid="562958543355919"><TBODY><TR __designer:dtid="562958543355920"><TD align=center colSpan=1 __designer:dtid="562958543355921"></TD><TD align=center colSpan=3 __designer:dtid="562958543355922"><BR /><asp:Label id="lblMsg" runat="server" Height="25px" ForeColor="Black" Text="Do you want to save?" __designer:dtid="562958543355923" __designer:wfdid="w48" Font-Size="Small"></asp:Label></TD></TR><TR __designer:dtid="562958543355928"><TD __designer:dtid="562958543355929"></TD><TD align=center __designer:dtid="562958543355930">&nbsp;&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" Font-Bold="True" __designer:dtid="562958543355931" CssClass="btnStyle" __designer:wfdid="w179"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" Font-Bold="True" __designer:dtid="562958543355932" CssClass="btnStyle" __designer:wfdid="w180"></asp:Button>&nbsp;</TD><TD align=center __designer:dtid="562958543355933">&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> 
                                    <asp:Panel id="pnlOk" runat="server" Width="125px" 
                                        Height="50px" __designer:dtid="562958543355938" 
                                        __designer:wfdid="w181"><TABLE style="WIDTH: 237px" __designer:dtid="562958543355939"><TBODY><TR __designer:dtid="562958543355940"><TD align=center colSpan=1 __designer:dtid="562958543355941"></TD><TD align=center colSpan=3 __designer:dtid="562958543355942"><BR /><asp:Label id="lblOk" runat="server" Height="25px" ForeColor="Black" Text="Do you want to ?" __designer:dtid="562958543355943" __designer:wfdid="w52"></asp:Label></TD></TR><TR __designer:dtid="562958543355948"><TD __designer:dtid="562958543355949"></TD><TD align=center __designer:dtid="562958543355950">&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="Button3" onclick="Button3_Click" runat="server" Width="50px" CausesValidation="False" ForeColor="Black" Text="OK" Font-Bold="True" __designer:dtid="562958543355951" CssClass="btnStyle" __designer:wfdid="w183"></asp:Button> &nbsp; </TD><TD align=center __designer:dtid="562958543355952">&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> <cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" __designer:dtid="562958543355957" __designer:wfdid="w184" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize"></cc1:ModalPopupExtender> <asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden" __designer:dtid="562958543355909" __designer:wfdid="w185"></asp:Button></asp:Panel></td>
                </tr>
                <tr>
                    <td align="center" colspan="8">
                        &nbsp;</td>
                </tr>
            </table>
        </contenttemplate>
        </asp:UpdatePanel>
        <script type="text/javascript">
       function Showalert() {
           alert('Please Select a Value');
       }
       function ShowNoData() {
           alert('No Data Found');
       }
       function Showsuccess() {
           alert('Saved Successfully');
       }
       function Showerror() {
       alert('Error');
       }
       }
        </script>

</asp:Content>


