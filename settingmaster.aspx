<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Settingmaster.aspx.cs" Inherits="settingmaster" Title="Setting Master" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <asp:Panel ID="pnluserlink" runat="server" GroupingText="Quick links" Height="100%"
        Width="150px">
        <br />          
        <br />
        <asp:HyperLink ID="hlstaffmaster" runat="server" Height="32px" NavigateUrl="~/StaffMaster.aspx"
            Visible="False" Width="128px">Staff master</asp:HyperLink><asp:HyperLink ID="hlroommaster"
                runat="server" Height="32px" NavigateUrl="~/roommaster1.aspx" Visible="False"
                Width="128px">Room master</asp:HyperLink><asp:HyperLink ID="hldonormaster" runat="server"
                    Height="32px" NavigateUrl="~/DonorMaster.aspx" Visible="False" Width="128px">Donor master</asp:HyperLink><asp:HyperLink
                        ID="hlcomplaintmaster" runat="server" Height="32px" NavigateUrl="~/ComplaintMaster.aspx"
                        Visible="False">Complaint master</asp:HyperLink><asp:HyperLink ID="hlteammaster"
                            runat="server" Height="32px" NavigateUrl="~/TeamMaster.aspx" Visible="False"
                            Width="128px">Team master</asp:HyperLink><asp:HyperLink ID="hlinvmaster" runat="server"
                                Height="32px" NavigateUrl="~/inventorymaster.aspx" Visible="False">Inventory master</asp:HyperLink><asp:HyperLink
                                    ID="hlseasonmstr" runat="server" Height="32px" NavigateUrl="~/SeasonMaster.aspx"
                                    Visible="False" Width="136px">Season master</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                                        ID="hlsubmaster" runat="server" Height="32px" NavigateUrl="~/Submasters.aspx"
                                        Visible="False" Width="128px">Submasters</asp:HyperLink><asp:HyperLink ID="hlreservpol"
                                            runat="server" Height="32px" NavigateUrl="~/ReservationPolicy.aspx" Visible="False">reservation Policy</asp:HyperLink><asp:HyperLink
                                                ID="hlroolallocpol" runat="server" Height="32px" NavigateUrl="~/Room Allocation Policy.aspx"
                                                Visible="False" Width="144px">Room Alloc Policy</asp:HyperLink><asp:HyperLink ID="hlbillpolicy"
                                                    runat="server" Height="32px" NavigateUrl="~/Billing and Service charge policy.aspx"
                                                    Visible="False" Width="152px">Bill n service policy</asp:HyperLink><asp:HyperLink
                                                        ID="hlbankpolicy" runat="server" Height="32px" NavigateUrl="~/Cashier and Bank Remittance Policy.aspx"
                                                        Visible="False" Width="136px">Bank policy</asp:HyperLink><asp:HyperLink ID="hlroomallocation"
                                                            runat="server" Height="32px" NavigateUrl="~/roomallocation.aspx" Visible="False"
                                                            Width="128px">Room allocation</asp:HyperLink><asp:HyperLink ID="hlroomreservation"
                                                                runat="server" Height="32px" NavigateUrl="~/Room Reservation.aspx" Visible="False"
                                                                Width="136px">Room reservation</asp:HyperLink><asp:HyperLink ID="hlvacating" runat="server"
                                                                    Height="32px" NavigateUrl="~/vacating and billing.aspx" Visible="False" Width="128px">Room Vacating</asp:HyperLink><asp:HyperLink
                                                                        ID="hldonorpass" runat="server" Height="32px" NavigateUrl="~/donorpassfinal.aspx"
                                                                        Visible="False" Width="128px">Donor pass</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                                                                            ID="hlcmplntrgstr" runat="server" Height="32px" NavigateUrl="~/Complaint Register.aspx"
                                                                            Visible="False" Width="144px">Complaint register</asp:HyperLink><span style="font-size: 9pt">
                                                                            </span>
        <asp:HyperLink ID="hlchellanentry" runat="server" Height="32px" NavigateUrl="~/Chellan Entry.aspx"
            Visible="False" Width="128px">Chellan entry</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                ID="hlroomrsrce" runat="server" Height="32px" NavigateUrl="~/Room Resource Register.aspx"
                Visible="False" Width="128px">Room resource</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                    ID="hlusercrtn" runat="server" Height="32px" NavigateUrl="~/User Account Information.aspx"
                    Visible="False" Width="136px">User creation</asp:HyperLink><span style="font-size: 9pt"></span><asp:HyperLink
                        ID="hluserprvlge" runat="server" Height="32px" NavigateUrl="~/UserPrivilegeSettings.aspx"
                        Visible="False" Width="136px">User privilege</asp:HyperLink><asp:HyperLink ID="hlprinter"
                            runat="server" Height="32px" NavigateUrl="~/PlainPreprintedSettings.aspx" Visible="False"
                            Width="136px">Printer settings</asp:HyperLink><asp:HyperLink ID="hldayclose" runat="server"
                                Height="32px" NavigateUrl="~/DayClosing.aspx" Visible="False" Width="136px">Day close</asp:HyperLink><asp:HyperLink
                                    ID="hlroommgmnt" runat="server" Height="32px" NavigateUrl="~/Room Management.aspx"
                                    Visible="False" Width="152px">Room management</asp:HyperLink><asp:HyperLink ID="hlinvmngmnt"
                                        runat="server" Height="32px" NavigateUrl="~/Room Inventory Management.aspx" Visible="False"
                                        Width="151px">Inventory mangement</asp:HyperLink><asp:HyperLink ID="hlhkmagmnt" runat="server"
                                            Height="32px" NavigateUrl="~/HK management.aspx" Visible="False" Width="152px">HK management</asp:HyperLink></asp:Panel>

</asp:Content>
--%><asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE><TBODY><TR><TD style="TEXT-ALIGN: center"><asp:Label id="lblheading" runat="server" Text="Setting Master" Font-Bold="True" Font-Size="X-Large" __designer:wfdid="w14"></asp:Label></TD></TR><TR><TD>&nbsp;<TABLE><TBODY><TR><TD style="HEIGHT: 208px"><asp:Panel id="Panel1" runat="server" GroupingText="Malayam year setting" __designer:wfdid="w9"><TABLE style="WIDTH: 96px; HEIGHT: 168px; TEXT-ALIGN: center"><TBODY><TR><TD><asp:Label id="Label1" runat="server" Width="61px" Text="Mal year" __designer:wfdid="w1"></asp:Label></TD><TD style="WIDTH: 109px"><asp:TextBox id="txtmalyear" runat="server" Width="100px" Height="17px" __designer:wfdid="w2" AutoPostBack="True" OnTextChanged="txtmalyear_TextChanged"></asp:TextBox></TD><TD style="TEXT-ALIGN: left"><asp:ImageButton id="Imgbutrefresh" onclick="Imgbutrefresh_Click" runat="server" Width="24px" Height="24px" CausesValidation="False" ImageUrl="~/Images/Buttons/refresh.jpg" __designer:wfdid="w3"></asp:ImageButton></TD><TD></TD></TR><TR><TD></TD><TD style="WIDTH: 109px"></TD><TD></TD><TD></TD></TR><TR><TD><asp:Label id="Label2" runat="server" Width="92px" Text="Eng start date" __designer:wfdid="w4"></asp:Label></TD><TD style="WIDTH: 109px"><asp:TextBox id="txtengstartdate" runat="server" Width="100px" __designer:wfdid="w5"></asp:TextBox></TD><TD><asp:Label id="Label3" runat="server" Width="91px" Text="Eng end date" __designer:wfdid="w6"></asp:Label></TD><TD><asp:TextBox id="txtengenddate" runat="server" Width="100px" __designer:wfdid="w7" AutoPostBack="True" OnTextChanged="txtengenddate_TextChanged"></asp:TextBox></TD></TR><TR><TD></TD><TD></TD><TD></TD><TD></TD></TR><TR><TD></TD><TD></TD><TD><asp:Button id="btnsave" onclick="btnsave_Click" runat="server" Text="Save" Font-Bold="True" __designer:wfdid="w8" CssClass="btnStyle_small"></asp:Button></TD><TD><asp:Button id="btnclear" onclick="btnclear_Click" runat="server" CausesValidation="False" Text="Clear" Font-Bold="True" __designer:wfdid="w9" CssClass="btnStyle_small"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel></TD><TD style="HEIGHT: 208px"><asp:Panel id="Panel3" runat="server" GroupingText="Account posting" __designer:wfdid="w18"><TABLE style="WIDTH: 280px; HEIGHT: 168px"><TBODY><TR><TD><asp:Label id="Label7" runat="server" __designer:wfdid="w10" Text="Cashier name"></asp:Label></TD><TD><asp:DropDownList id="ComboBox1" runat="server" Width="110px" Height="22px" __designer:wfdid="w3" AppendDataBoundItems="True" DataValueField="staff_id" DataTextField="staffname" DataSourceID="Sqstaff"><asp:ListItem Selected="True" Value="-1">--Select--</asp:ListItem>
</asp:DropDownList> <asp:SqlDataSource id="Sqstaff" runat="server" __designer:wfdid="w12" UpdateCommand="select staffname from m_staff where rowstatus<>2" SelectCommand="SELECT staff_id, staffname FROM m_staff where rowstatus<>2" ProviderName="<%$ ConnectionStrings:tdbnewConnectionString3.ProviderName %>" ConnectionString="<%$ ConnectionStrings:tdbnewConnectionString3 %>"></asp:SqlDataSource></TD></TR><TR><TD></TD><TD></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE>&nbsp;&nbsp; </TD></TR><TR><TD style="WIDTH: 779px"><asp:Panel id="Panel2" runat="server" Width="624px" GroupingText="Print setting" __designer:wfdid="w32" Visible="False"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:Label id="Label4" runat="server" Width="104px" Visible="False" __designer:wfdid="w128" Text="Enter print count"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:TextBox id="txtcountprint" runat="server" Width="100px" Visible="False" __designer:wfdid="w129"></asp:TextBox></TD><TD style="WIDTH: 100px; HEIGHT: 26px"></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:Button id="Button4" onclick="Button4_Click" runat="server" Width="88px" CausesValidation="False" Visible="False" __designer:wfdid="w130" Text="Clear" Font-Bold="True"></asp:Button></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:Button id="Button2" onclick="Button2_Click" runat="server" Width="150px" Visible="False" __designer:wfdid="w131" ValidationGroup="Panel2" Text="Save print setting" Font-Bold="True"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:Label id="Label5" runat="server" Visible="False" __designer:wfdid="w3" Text="Correspond Mal start month"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:DropDownList id="cmbmalstartmonth" runat="server" Width="105px" Height="22px" Visible="False" __designer:wfdid="w4"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px; HEIGHT: 26px"></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:Label id="Label6" runat="server" Width="91px" Visible="False" __designer:wfdid="w5" Text="Corresponding Mal end month"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 26px"><asp:DropDownList id="cmbmalendmaonth" runat="server" Width="105px" Height="22px" Visible="False" __designer:wfdid="w6"><asp:ListItem></asp:ListItem>
</asp:DropDownList></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="WIDTH: 779px"></TD></TR></TBODY></TABLE><TABLE style="WIDTH: 792px; HEIGHT: 400px"><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 41px"><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Width="165px" ForeColor="White" __designer:wfdid="w12" SetFocusOnError="True" ValidationGroup="Panel1" ErrorMessage="Malayalam year required" ControlToValidate="txtmalyear"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px; HEIGHT: 41px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" __designer:wfdid="w16" TargetControlID="RequiredFieldValidator1"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 100px"><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Width="176px" ForeColor="White" __designer:wfdid="w13" SetFocusOnError="True" ValidationGroup="Panel1" ErrorMessage="English starting date required" ControlToValidate="txtengstartdate"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" __designer:wfdid="w17" TargetControlID="RequiredFieldValidator2"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 100px"><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" Width="176px" ForeColor="White" __designer:wfdid="w15" SetFocusOnError="True" ValidationGroup="Panel1" ErrorMessage="English ending date required" ControlToValidate="txtengenddate"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" __designer:wfdid="w18" TargetControlID="RequiredFieldValidator3"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 100px">&nbsp;</TD><TD style="WIDTH: 100px"><cc1:CalendarExtender id="CalendarExtender1" runat="server" __designer:wfdid="w36" TargetControlID="txtengstartdate" Format="dd/MM/yyyy"></cc1:CalendarExtender></TD></TR><TR><TD style="WIDTH: 100px"><TABLE style="WIDTH: 248px; HEIGHT: 24px"><TBODY><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 110px">&nbsp;</TD></TR></TBODY></TABLE></TD><TD style="WIDTH: 100px"><cc1:CalendarExtender id="CalendarExtender2" runat="server" __designer:wfdid="w37" TargetControlID="txtengenddate" Format="dd/MM/yyyy"></cc1:CalendarExtender></TD></TR><TR><TD style="WIDTH: 100px"><asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ForeColor="White" __designer:wfdid="w23" SetFocusOnError="True" ErrorMessage="Only numbeer" ControlToValidate="txtmalyear" ValidationExpression="[0-9]{1,20}"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" __designer:wfdid="w25" TargetControlID="RegularExpressionValidator1"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 100px"><asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" ForeColor="White" __designer:wfdid="w24" SetFocusOnError="True" ErrorMessage="Only number" ControlToValidate="txtcountprint" ValidationExpression="[0-9]{1,20}"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 100px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" __designer:wfdid="w26" TargetControlID="RegularExpressionValidator2"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ForeColor="White" __designer:wfdid="w27" SetFocusOnError="True" ValidationGroup="Panel2" ErrorMessage="Print count required" ControlToValidate="txtcountprint"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px; HEIGHT: 18px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" __designer:wfdid="w28" TargetControlID="RequiredFieldValidator4"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" ForeColor="White" __designer:wfdid="w29" SetFocusOnError="True" ValidationGroup="Panel2" ErrorMessage="Only numbers allowed" ControlToValidate="txtcountprint" ValidationExpression="[0-9]{1,20}"></asp:RegularExpressionValidator></TD><TD style="WIDTH: 100px; HEIGHT: 18px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender7" runat="server" __designer:wfdid="w30" TargetControlID="RegularExpressionValidator3"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:RangeValidator id="RangeValidator1" runat="server" Width="152px" Height="8px" ForeColor="White" __designer:wfdid="w13" ErrorMessage="Mal year not valid!" ControlToValidate="txtmalyear" MinimumValue="1185" MaximumValue="2000"></asp:RangeValidator></TD><TD style="WIDTH: 100px; HEIGHT: 18px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender9" runat="server" __designer:wfdid="w37" TargetControlID="RangeValidator1"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 18px"><asp:Panel id="pnlMessage" runat="server" __designer:dtid="281483566645258" __designer:wfdid="w27" CssClass="ModalWindow" _designer:dtid="562958543355909" _=""><asp:Panel id="Panel8" runat="server" Width="234px" BackColor="LightSteelBlue" __designer:dtid="281483566645259" __designer:wfdid="w28" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" __designer:dtid="281483566645260" __designer:wfdid="w29" Text="Tsunami ARMS - Confirmation" Font-Bold="True" ForeColor="MediumBlue"></asp:Label></asp:Panel> <ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" __designer:dtid="844433520066613" __designer:wfdid="w30" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize">
                </ajaxToolkit:ModalPopupExtender>&nbsp; <asp:Button style="DISPLAY: none" id="btnHidden" onclick="btnHidden_Click" runat="server" Text="Hidden" __designer:dtid="281483566645253" __designer:wfdid="w32"></asp:Button>&nbsp; <BR __designer:dtid="281483566645261" /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px" __designer:dtid="281483566645262" __designer:wfdid="w34"><TABLE style="WIDTH: 237px" __designer:dtid="281483566645263"><TBODY><TR __designer:dtid="281483566645264"><TD style="HEIGHT: 18px" align=center colSpan=1 __designer:dtid="281483566645265"></TD><TD style="WIDTH: 227px; HEIGHT: 18px" align=center colSpan=3 __designer:dtid="281483566645266"><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" __designer:dtid="281483566645267" Font-Size="Small" __designer:wfdid="w35"></asp:Label></TD></TR><TR __designer:dtid="281483566645268"><TD style="HEIGHT: 10px" __designer:dtid="281483566645269"></TD><TD style="WIDTH: 208px; HEIGHT: 15px" __designer:dtid="281483566645270"></TD><TD style="WIDTH: 13px; HEIGHT: 10px" __designer:dtid="281483566645271"></TD></TR><TR __designer:dtid="281483566645272"><TD style="HEIGHT: 26px" __designer:dtid="281483566645273"></TD><TD style="WIDTH: 208px; HEIGHT: 26px" align=center __designer:dtid="281483566645274"><asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" __designer:dtid="281483566645275" __designer:wfdid="w36" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" __designer:dtid="281483566645276" __designer:wfdid="w37" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="WIDTH: 13px; HEIGHT: 26px" align=center __designer:dtid="281483566645277">&nbsp;</TD></TR><TR __designer:dtid="281483566645278"><TD style="HEIGHT: 18px" __designer:dtid="281483566645279"></TD><TD style="WIDTH: 208px; HEIGHT: 18px" align=center __designer:dtid="281483566645280"></TD><TD style="WIDTH: 13px; HEIGHT: 18px" align=center __designer:dtid="281483566645281"></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px" __designer:dtid="281483566645282" __designer:wfdid="w38"><TABLE style="WIDTH: 237px" __designer:dtid="281483566645283"><TBODY><TR __designer:dtid="281483566645284"><TD align=center colSpan=1 __designer:dtid="281483566645285"></TD><TD style="WIDTH: 224px; TEXT-ALIGN: center" align=center colSpan=3 __designer:dtid="281483566645286"><asp:Label id="lblOk" runat="server" Width="223px" ForeColor="Black" Text="Do you want to ?" __designer:dtid="281483566645287" Font-Size="Small" __designer:wfdid="w39"></asp:Label></TD></TR><TR __designer:dtid="281483566645288"><TD style="HEIGHT: 10px" __designer:dtid="281483566645289"></TD><TD style="HEIGHT: 15px" __designer:dtid="281483566645290"></TD><TD style="HEIGHT: 10px" __designer:dtid="281483566645291"></TD></TR><TR __designer:dtid="281483566645292"><TD __designer:dtid="281483566645293"></TD><TD style="TEXT-ALIGN: center" align=center __designer:dtid="281483566645294">&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" Text="OK" __designer:dtid="281483566645295" __designer:wfdid="w40" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center __designer:dtid="281483566645296">&nbsp;</TD></TR><TR __designer:dtid="281483566645297"><TD __designer:dtid="281483566645298"></TD><TD align=center __designer:dtid="281483566645299"></TD><TD align=center __designer:dtid="281483566645300"></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel> <asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" ForeColor="White" __designer:wfdid="w1" ErrorMessage="Select cashier" ControlToValidate="ComboBox1" InitialValue="-1"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 100px; HEIGHT: 18px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender8" runat="server" __designer:wfdid="w2" TargetControlID="RequiredFieldValidator5"></cc1:ValidatorCalloutExtender></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel4" runat="server" GroupingText="User Tips" Width="100%">
        <br />
        This form is used for setting Malayalam year and corresponding English
    dates.<br />
    <br />
    Authorised user can set the year.<br />
    <br />
    The season dates can be set for the year can be set using the Season master page.<br />
    </asp:Panel>
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
</asp:Content>

