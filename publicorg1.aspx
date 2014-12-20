<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="publicorg1.aspx.cs" Inherits="publicorg1" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
    <asp:ScriptManager id="ScriptManager1" runat="server">
                    </asp:ScriptManager><asp:UpdatePanel id="UpdatePanel1" runat="server"><contenttemplate>
<TABLE width=100%><TBODY><TR><TD align=center colSpan=3><asp:Panel id="Panel1" runat="server" Width="100%" Height="100%"><table width="100%"><TBODY><TR><TD vAlign=top colSpan=3 align=center><asp:Image id="Image1" runat="server" Width=100% ImageUrl="~/Images/Banner/Copy of Masterbanner1.JPG"></asp:Image></TD></TR><TR><TD style="WIDTH:100%" align=center colSpan=3>&nbsp;<asp:Label id="Label1" runat="server" Font-Bold="True" Font-Size="XX-Large" ForeColor="#C00000" Width="741px" Text="VACANT ROOM RENT"></asp:Label></TD></TR><TR><TD align=center colSpan=3><asp:GridView id="dtgRoomDetails" runat="server" Font-Bold="True" Font-Size="XX-Large" ForeColor="Yellow" Width="934px" Height="414px" HorizontalAlign="Center" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnPageIndexChanging="roomdetails_PageIndexChanging" BackColor="Navy" AllowPaging="True">
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView> 
   <center><asp:GridView id="dtgDetailedStatus" runat="server" Font-Bold="True" 
        Font-Size="X-Large" ForeColor="Yellow" Width="100%" HorizontalAlign="Center" 
        OnPageIndexChanging="detailedstatus_PageIndexChanging" BackColor="Navy" >
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
   </center>
    <center>
        <strong><span style="color: #ff3300">Powered by Tsunami Software Pvt Ltd</span></strong></center>
    <asp:GridView id="dtgVacantRent" runat="server" Font-Bold="True" Font-Size="XX-Large" ForeColor="Yellow" Width="929px" HorizontalAlign="Center" OnPageIndexChanging="vacantrent_PageIndexChanging" BackColor="Navy" AllowPaging="True">
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView> <asp:GridView id="dtgReserved" runat="server" Font-Bold="True" Font-Size="XX-Large" ForeColor="Yellow" Width="927px" HorizontalAlign="Center" OnSelectedIndexChanged="dtgReserved_SelectedIndexChanged" OnPageIndexChanging="reserved_PageIndexChanging" BackColor="Navy" AllowPaging="True" BorderStyle="Solid" BorderColor="White"></asp:GridView> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Timer id="Timer1" runat="server" OnTick="Timer1_Tick1" Interval="1000">
                    </asp:Timer> <asp:Panel id="pnlInstructions" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Width="927px" BackColor="#C0C0FF" Visible="False"><TABLE width=527><TBODY><TR><TD align=center colSpan=2><BR /><asp:Label id="lblInstruction" runat="server" Font-Size="XX-Large" ForeColor="Red" Width="604px" Text="SWAMI SARANAM "></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE><BR /><asp:GridView id="dtgInstructions" runat="server" Font-Bold="True" Font-Size="30pt" ForeColor="Blue" Width="908px" HorizontalAlign="Center" BackColor="White" BorderColor="White" AutoGenerateColumns="False" GridLines="None" OnRowCreated="dtgInstructions_RowCreated" EnableTheming="True" OnRowDataBound="dtgInstructions_RowDataBound">
<EmptyDataRowStyle BackColor="White"></EmptyDataRowStyle>
<Columns>
<asp:BoundField DataField="ins_details"></asp:BoundField>
</Columns>
</asp:GridView>&nbsp;&nbsp;&nbsp;&nbsp; <asp:Image id="imgAyyappa" runat="server" Width="446px" ImageUrl="~/Images/ayyappan.jpg"></asp:Image></asp:Panel> &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; </TD></TR></TBODY></TABLE>&nbsp; </asp:Panel> &nbsp; &nbsp;&nbsp; </TD></TR></TBODY></TABLE>
</contenttemplate>
        </asp:UpdatePanel>
         
                
 <table>
                        <tr>
                            <td align="center" colspan="3">
                                <marquee direction="left" style="width: 929px; height: 14px"><asp:Label id="lblscroll" runat="server" ForeColor="Maroon" Font-Size="X-Large" Font-Bold="True" __designer:wfdid="w71"></asp:Label><BR /><BR /><BR /></marquee>
                            </td>
                        </tr>
                    </table>
          
</asp:Content>

