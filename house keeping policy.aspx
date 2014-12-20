<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="house keeping policy.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
    <br />    
    <table class="style28">
    <tr><td align="center" colspan="4"><asp:Label id="lblhousekeepingpolicy" runat="server" Width="371px" ForeColor="#000099" Text="HouseKeeping Policy" Font-Bold="True" __designer:wfdid="w1" CssClass="heading" Font-Size="14pt" Font-Names="Arial"></asp:Label> </td></tr>    
        <tr>
            <td align="center" colspan="4">
                <asp:Panel ID="policydetails" runat="server" GroupingText="Policy Details" 
                    Width="659px">
                    <table class="style28">
                        <tr>
                            <td width="25%">
                                <asp:Label ID="Label1" runat="server" Text="Policy Applicable From"></asp:Label>
                            </td>
                            <td width="25%">
                                <asp:TextBox ID="txtdate" runat="server" AutoPostBack="True" Height="17px" 
                                    MaxLength="10"  Width="150px" 
                                    TabIndex="20"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MM-yyyy" 
                                    TargetControlID="txtdate">
                                </cc1:CalendarExtender>
                            </td>
                            <td style="width: 30%">
                                <asp:Label ID="Label5" runat="server" Text="House keeping period"></asp:Label>
                            </td>
                            <td width="25%">
                                <asp:TextBox ID="txtperiod" runat="server" Width="150px" TabIndex="22"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td width="25%">
                                <asp:Label ID="Label6" runat="server" Text="Policy Type"></asp:Label>
                            </td>
                            <td width="25%">
                                <asp:DropDownList ID="cmbPolicy" runat="server" DataTextField="policy" 
                                    DataValueField="policy_id" 
                                     Width="152px" 
                                    TabIndex="21">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 30%">
                                <asp:Label ID="Label4" runat="server" Text="Urgency"></asp:Label>
                            </td>
                            <td width="25%">
                                <asp:DropDownList ID="cmbUrgency" runat="server" DataTextField="urgname" 
                                    DataValueField="urg_cmp_id" Width="152px" TabIndex="23">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnsave" runat="server" CssClass="btnStyle_medium" 
                    Height="26px" onclick="Button3_Click" Text="Save" TabIndex="24" />
            </td>
            <td align="center">
                <asp:Button ID="btnedit" runat="server" CssClass="btnStyle_medium" 
                    onclick="btnedit_Click" Text="Edit" TabIndex="25" />
            </td>
            <td align="center">
                <asp:Button ID="btnclear" runat="server" CssClass="btnStyle_medium" 
                    ForeColor="Blue" onclick="btnclear_Click" Text="Clear" TabIndex="26" />
            </td>
            <td align="center">
                <asp:Button ID="btndelete" runat="server" CssClass="btnStyle_medium" 
                    onclick="btndelete_Click" Text="Delete" TabIndex="27" />
            </td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;</td>
            <td align="center">
                &nbsp;</td>
            <td align="center">
                &nbsp;</td>
            <td align="center">
                &nbsp;</td>
        </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:GridView ID="gv_details" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" 
                        onrowcreated="gv_details_RowCreated" onrowdatabound="gv_details_RowDataBound" 
                        onselectedindexchanged="gv_details_SelectedIndexChanged" 
                        style="text-align: center">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" />
                            <asp:BoundField DataField="level" HeaderText="Urgency" />
                            <asp:BoundField DataField="f_date" HeaderText="Fromdate" />
                            <asp:BoundField DataField="t_date" HeaderText="Todate" />
                            <asp:BoundField DataField="time" HeaderText="House keeping period" />
                            <asp:BoundField DataField="poltype" HeaderText="Policytype" />
                            <asp:CommandField SelectText="" ShowSelectButton="True" />
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />                       
                    </asp:GridView>
                </td>
        </tr>
            <tr>
                <td colspan="4" style="height: 24px">
                </td>
            </tr>
   
             </table>
    
           
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    </contenttemplate>
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
          function ShowValue() {
              alert('Enter the value');
          }
               </script>       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

