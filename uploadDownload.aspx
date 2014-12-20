<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="uploadDownload.aspx.cs" Inherits="uploadDownload" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 893px"><TBODY><TR><TD style="WIDTH: 873px; HEIGHT: 22px" colSpan=5><H1 style="TEXT-ALIGN: center"><asp:Label id="lblCaption" runat="server" Text="UPLOAD & DOWN LOAD DETAILS" Font-Size="0.65em"></asp:Label>&nbsp;</H1></TD></TR><TR><TD vAlign=top colSpan=5><asp:RadioButtonList id="rdoStatus" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdoStatus_SelectedIndexChanged"><asp:ListItem Value="0">Down Load</asp:ListItem>
<asp:ListItem Value="1">Up Load</asp:ListItem>
</asp:RadioButtonList> </TD></TR><TR><TD vAlign=top colSpan=5><asp:Panel id="pnlDownloadDetails" runat="server" Width="85%" GroupingText="Down Load Details" Visible="False"><TABLE style="WIDTH: 718px"><TBODY><TR><TD vAlign=top><asp:Label id="lblTableName" runat="server" Text="Select table details  to DownLoad" Font-Bold="True" __designer:wfdid="w5"></asp:Label></TD><TD vAlign=top><asp:DropDownList id="cmbDownLoad" runat="server" Width="220px" AutoPostBack="True" OnSelectedIndexChanged="cmbDownLoad_SelectedIndexChanged" __designer:wfdid="w6"><asp:ListItem Value="-1">--Select--</asp:ListItem>
<asp:ListItem Value="0">Donor Reservation</asp:ListItem>
<asp:ListItem Value="1">TDB Reservation</asp:ListItem>
<asp:ListItem Value="t_key_lost">Key Lost</asp:ListItem>
<asp:ListItem Value="t_donorpass">Orginal Pass Issue</asp:ListItem>
<asp:ListItem Value="2">General Reservation</asp:ListItem>
</asp:DropDownList></TD><TD vAlign=top><asp:LinkButton id="lnkKeyLost" onclick="lnkKeyLost_Click" runat="server" CausesValidation="False" Font-Bold="True" __designer:wfdid="w7">Key Lost</asp:LinkButton><BR /><asp:LinkButton id="lnkOrginalPass" onclick="lnkOrginalPass_Click" runat="server" CausesValidation="False" Font-Bold="True" __designer:wfdid="w8">Orginal Pass Details</asp:LinkButton><BR /><asp:LinkButton id="lnkDonorReservation" onclick="lnkDonorReservation_Click" runat="server" CausesValidation="False" Font-Bold="True" __designer:wfdid="w9">Donor Reservation Details</asp:LinkButton><BR /><asp:LinkButton id="lnkTDBReservation" onclick="lnkTDBReservation_Click" runat="server" CausesValidation="False" Font-Bold="True" __designer:wfdid="w10">TDB Reservation</asp:LinkButton></TD><TD vAlign=top>
        <asp:Button id="btnDownLoad" runat="server" 
            CausesValidation="False" Text="Down Load" 
            CssClass="btnStyle_large" __designer:wfdid="w11" 
            onclick="btnDownLoad_Click" ></asp:Button></TD></TR>
        <tr>
            <td valign="top">
                &nbsp;</td>
            <td valign="top">
                &nbsp;</td>
            <td valign="top">
                <asp:LinkButton ID="lnkGeneralReservation" runat="server" 
                    __designer:wfdid="w10" CausesValidation="False" 
                    Font-Bold="True" onclick="lnkGeneralReservation_Click">General 
                Reservation</asp:LinkButton>
            </td>
            <td valign="top">
                &nbsp;</td>
        </tr>
        </TBODY></TABLE></asp:Panel> <asp:Panel id="pnlUpdateDetails" runat="server" Width="82%" GroupingText="Update Details" Visible="False"><TABLE style="WIDTH: 740px"><TBODY><TR><TD style="WIDTH: 162px">
            Mode</TD><TD style="WIDTH: 1px">
                <asp:DropDownList ID="ddlmode" runat="server" AutoPostBack="True" Width="220px" 
                    onselectedindexchanged="ddlmode_SelectedIndexChanged1">
                    <asp:ListItem Value="0">Excel</asp:ListItem>
                    <asp:ListItem Value="1">Download</asp:ListItem>
                </asp:DropDownList>
            </TD><TD style="WIDTH: 80px">&nbsp;</TD><TD>&nbsp;</TD></TR>
                
                  <tr>
                      <td style="WIDTH: 162px">
                          <asp:Label ID="lblTblName" runat="server" __designer:wfdid="w12" 
                              Font-Bold="True" Text="Select Table to Update"></asp:Label>
                      </td>
                      <td style="WIDTH: 1px">
                          <asp:DropDownList ID="cmbTableName" runat="server" __designer:wfdid="w13" 
                              AutoPostBack="True" Width="220px">
                              <asp:ListItem Value="-1">--Select--</asp:ListItem>
                              <asp:ListItem Value="2">Donor Reservation</asp:ListItem>
                              <asp:ListItem Value="1">General Reservation</asp:ListItem>
                              <asp:ListItem Value="3">TDB Reservation</asp:ListItem>
                          </asp:DropDownList>
                      </td>
                      <td style="WIDTH: 80px">
                          <asp:Button ID="btnUpload" runat="server" __designer:wfdid="w14" 
                              CausesValidation="False" CssClass="btnStyle_small" onclick="btnUpload_Click" 
                              Text="Upload" />
                      </td>
                      <td>
                      </td>
            </tr>
                
                  <tr>
                <td  width="100%" colspan="4" >
               <asp:Panel ID="pnlweb" runat="server" Width="100%" Visible="False">
               <table width ="100%">
            <tr> 
                <td  width="25%" >
              
              
                    Date</td>
             
                <td width="25%">
                    <asp:TextBox ID="txtDate" runat="server" AutoPostBack="True" Enabled="true" 
                        tabIndex="16" Width="150px"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtdate_CalendarExtender" runat="server" 
                        Format="dd-MM-yyyy" TargetControlID="txtdate">
                    </cc1:CalendarExtender>
                </td>
                <td width="25%">
                </td>
                <td width="25%">
                </td>
             
            </tr>
            </table>
                  </asp:Panel>
                  </td>
            </tr>
            <TR><TD style="WIDTH: 162px"><asp:Label id="lblfileName" runat="server" Text="Select File to Upload" Font-Bold="True" __designer:wfdid="w15"></asp:Label></TD><TD style="WIDTH: 1px; HEIGHT: 42px"><asp:FileUpload id="txtFilePath" runat="server" __designer:wfdid="w16"></asp:FileUpload></TD><TD style="WIDTH: 80px; HEIGHT: 42px"><asp:Button id="btnUpdate" onclick="btnUpdate_Click" runat="server" CausesValidation="False" Text="Update" CssClass="btnStyle_small" __designer:wfdid="w17"></asp:Button></TD><TD></TD></TR><TR><TD vAlign=top colSpan=4>
            <asp:Panel id="Panel1" runat="server" Width="503px" 
                Visible="False" __designer:wfdid="w18" ScrollBars="Both">
            <asp:GridView id="dtgDetails" runat="server" Width="479px" 
                ForeColor="#333333" __designer:wfdid="w19" CellPadding="4">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></asp:Panel></TD></TR><TR><TD vAlign=top colSpan=4></TD></TR></TBODY></TABLE></asp:Panel> <asp:Button style="DISPLAY: none" id="btnHidden" onclick="btnHidden_Click" runat="server" Text="Hidden"></asp:Button> <asp:TextBox id="TextBox1" runat="server" AutoPostBack="True" Visible="False" OnTextChanged="TextBox1_TextChanged"></asp:TextBox> <asp:Label id="Label3" runat="server" Text="Label" Visible="False"></asp:Label><BR /><asp:Panel id="pnlMessage" runat="server" _designer:dtid="562958543355909" _="" CssClass="ModalWindow"><asp:Panel id="Panel7" runat="server" Width="99%" Height="31px" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Width="231px" ForeColor="MediumBlue" Text="  Tsunami ARMS -  Confirmation" Font-Bold="True"></asp:Label></asp:Panel> <BR /><asp:Panel id="pnlYesNo" runat="server"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD></TD><TD></TD></TR><TR><TD></TD><TD align=center>&nbsp;<asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server"><TABLE style="WIDTH: 237px"><TBODY><TR><TD style="HEIGHT: 18px" align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD></TD><TD></TD><TD></TD></TR><TR><TD></TD><TD align=center>&nbsp; &nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel></asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" __designer:wfdid="w2" TargetControlID="btnHidden" PopupControlID="pnlMessage"></ajaxToolkit:ModalPopupExtender><BR /></TD></TR></TBODY></TABLE>
</contenttemplate>
                              <Triggers>
                              <asp:PostBackTrigger ControlID="rdoStatus" />
                              <asp:PostBackTrigger ControlID="cmbTableName" /> 
                              <asp:PostBackTrigger ControlID="lnkDonorReservation" /> 
                               <asp:PostBackTrigger ControlID="lnkKeyLost" /> 
                               <asp:PostBackTrigger ControlID="lnkOrginalPass" /> 
                               <asp:PostBackTrigger ControlID="lnkTDBReservation" /> 
                               <asp:PostBackTrigger ControlID="btnUpload" /> 
                               <asp:PostBackTrigger ControlID="lnkGeneralReservation" />                                
                              </Triggers>
                              
    </asp:UpdatePanel>
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

