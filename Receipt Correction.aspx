<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Receipt Correction.aspx.cs" Inherits="Receipt_Correction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td colspan="8">
                        <asp:Label id="lblhead" runat="server" Text="Receipt Correction" Font-Bold="True" Font-Size="Medium" CssClass="heading"></asp:Label></td>
                </tr>
                <tr>
                    <td width="15%">
                        <asp:Label ID="Label1" runat="server" Text="Receipt No"></asp:Label>
                    </td>
                    <td colspan="2" width="20%">
                        <asp:TextBox ID="txtReceiptno" runat="server" Width="160px" AutoPostBack="True" 
                            ontextchanged="txtReceiptno_TextChanged"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:Label ID="Label3" runat="server" Text="Counter"></asp:Label>
                    </td>
                    <td width="15%">
                        <asp:DropDownList ID="cmbCounter" runat="server" Width="163px" DataTextField="counter_no" 
                            DataValueField="counter_id">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2" width="15%">
                        <asp:Label ID="Label2" runat="server" Text="Status"></asp:Label>
                    </td>
                    <td width="20%">
                        <asp:DropDownList ID="cmbStatus" runat="server" Width="163px" DataTextField="receipt_status" 
                            DataValueField="id">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="25%" colspan="2" style="height: 26px">
                        </td>
                    <td width="25%" colspan="2" align="center" style="height: 26px">
                        <asp:Button ID="btnCorrect" runat="server" CssClass="btnStyle_small" onclick="btnCorrect_Click" 
                            Text="Correct" />
                    </td>
                    <td width="25%" colspan="2" style="height: 26px">
                        </td>
                    <td width="25%" colspan="2" align="center" style="height: 26px">
                        <asp:Button ID="btnView" runat="server" CssClass="btnStyle_small" onclick="btnView_Click" 
                            Text="View" />
                    </td>
                </tr>
                <tr>
                    <td width="25%" colspan="2">
                        &nbsp;</td>
                    <td width="25%" colspan="2">
                        &nbsp;</td>
                    <td width="25%" colspan="2">
                        &nbsp;</td>
                    <td width="25%" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="8">
                        <asp:GridView ID="gvView" runat="server">
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" width="25%">
                        &nbsp;</td>
                    <td colspan="2" width="25%">
                        &nbsp;</td>
                    <td colspan="2" width="25%">
                        &nbsp;</td>
                    <td colspan="2" width="25%">
                        &nbsp;</td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function Showalert() {
            alert('Saved Successfully');
        }
        function ShowNoData() {
            alert('No Data Found');
        }
        function ShowDeleted() {
            alert('Deleted Successfully');
        }
        function ShowRequired() {
            alert('Fill the required field');
        }
        function ShowUpdated() {
            alert('Updated Successfully');
        }
        function Showdate() {
            alert('DayClosed!');
        }
        function ShowError() {
            alert('Error!');
        }
               </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

