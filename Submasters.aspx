<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Submasters.aspx.cs" Inherits="Submasters" Title="Sub Master" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc2" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<script runat="server">

</script>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        <p>
    &nbsp;</p>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
    <div>
    
    <center>
        <asp:ScriptManager id="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel id="UpdatePanel1" runat="server">
            <contenttemplate>
<TABLE><TBODY><TR><TD style="WIDTH: 832px" colSpan=5><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 18px"></TD><TD style="HEIGHT: 18px; TEXT-ALIGN: center" colSpan=2>&nbsp;<asp:Label id="lblheading" runat="server" Width="119px" Text="Sub Masters" Font-Bold="True" __designer:wfdid="w15" Font-Size="Large"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 18px"></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton20" onclick="ImageButton20_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnState_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton16" onclick="ImageButton16_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnOffice_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton23" onclick="ImageButton23_Click1" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/Store_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton24" onclick="ImageButton24_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnTask_0.jpg"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 21px"><asp:ImageButton id="ImageButton11" onclick="ImageButton11_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnFloor_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 21px"><asp:ImageButton id="ImageButton7" onclick="ImageButton7_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnDesignation_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 21px"><asp:ImageButton id="ImageButton6" onclick="ImageButton6_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnCounter_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 21px"><asp:ImageButton id="ImageButton55" onclick="ImageButton55_Click" runat="server" Width="200px" ImageUrl="~/Images/Buttons/reasonbutton.jpg" __designer:wfdid="w1"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 23px"><asp:ImageButton id="ImageButton8" onclick="ImageButton8_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnDistrict_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 23px"><asp:ImageButton id="ImageButton25" onclick="ImageButton25_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/Department_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 23px"><asp:ImageButton id="ImageButton21" onclick="ImageButton21_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnSupplier_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 23px"><asp:ImageButton id="ImageButton9" onclick="ImageButton9_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnDocument Name_0.jpg"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton33" onclick="ImageButton33_Click" runat="server" Width="200px" Height="20px" ImageUrl="~/Images/Buttons/Room ervice button.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton17" onclick="ImageButton17_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnSeason Name_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton2" onclick="ImageButton2_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnBank Account_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton27" onclick="ImageButton27_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnTransaction Name_0.jpg"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton4" onclick="ImageButton4_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnBuilding Name_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton18" onclick="ImageButton18_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnService Name_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton13" onclick="ImageButton13_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnInventory Item_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton5" onclick="ImageButton5_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnComplaint Category_0.jpg"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px"><asp:ImageButton id="ImageButton10" onclick="ImageButton10_Click1" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/Room Category_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px"><asp:ImageButton id="ImageButton31" onclick="ImageButton31_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/Type of donor_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px"><asp:ImageButton id="ImageButton34" onclick="ImageButton34_Click1" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/Unit Of Measurement_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px"><asp:ImageButton id="ImageButton1" onclick="ImageButton28_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnUrgency of Complaint_0.jpg" __designer:wfdid="w13"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton32" onclick="ImageButton32_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/Type of facility_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton15" onclick="ImageButton15_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnMalayalam Month_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton14" onclick="ImageButton14_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnInventory Item Category_0.jpg"></asp:ImageButton></TD><TD style="WIDTH: 100px; HEIGHT: 22px"></TD></TR></TBODY></TABLE>&nbsp;<TABLE><TBODY><TR><TD vAlign=top><asp:Panel id="Panel1" runat="server" Height="193px" GroupingText=" "><TABLE style="WIDTH: 375px"><TBODY><TR><TD colSpan=5><asp:Label id="lblformname" runat="server" Width="316px" ForeColor="Blue" Text="Label" Font-Bold="True" __designer:wfdid="w2" Font-Size="Large" Font-Names="Arial" Font-Overline="False"></asp:Label> </TD></TR><TR><TD colSpan=2><asp:Label id="lblstate" runat="server" Width="172px" Text="name" __designer:wfdid="w3"></asp:Label></TD><TD style="TEXT-ALIGN: left" colSpan=3><asp:DropDownList id="ComboBox1" runat="server" Width="160px" __designer:wfdid="w5" OnSelectedIndexChanged="ComboBox1_SelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True"><asp:ListItem Selected="True">--Select--</asp:ListItem>
</asp:DropDownList>&nbsp;</TD></TR><TR><TD colSpan=2><asp:Label id="lblbranchname" runat="server" Width="172px" Text="name" __designer:wfdid="w5"></asp:Label> </TD><TD align=left colSpan=3><asp:TextBox id="txtbranch" runat="server" Width="155px" Height="18px" __designer:wfdid="w6"></asp:TextBox> </TD></TR><TR><TD colSpan=2><asp:Label id="lblname" runat="server" Width="171px" Text="name" __designer:wfdid="w7"></asp:Label> </TD><TD align=left colSpan=3><asp:TextBox id="TextBox1" runat="server" Width="155px" Height="18px" __designer:wfdid="w8"></asp:TextBox> </TD></TR><TR><TD style="HEIGHT: 28px" colSpan=2><asp:Label id="lblaccountno" runat="server" Width="172px" Text="name" __designer:wfdid="w9"></asp:Label></TD><TD style="HEIGHT: 28px" align=left colSpan=3><asp:TextBox id="txtaccount" runat="server" Width="155px" Height="18px" __designer:wfdid="w10"></asp:TextBox></TD></TR><TR><TD style="HEIGHT: 28px" colSpan=2><asp:Label id="lblLocation" runat="server" Text="Location" __designer:wfdid="w1" Visible="False"></asp:Label></TD><TD style="HEIGHT: 28px" align=left colSpan=3><asp:TextBox id="txtLocation" runat="server" Width="155px" __designer:wfdid="w2" Visible="False"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 235px; HEIGHT: 27px"><asp:Button id="btnsave" onclick="btnsave_Click" runat="server" Width="70px" Text="Save" Font-Bold="True" __designer:wfdid="w11" ValidationGroup="Panel1" CssClass="btnStyle_small"></asp:Button> </TD><TD style="HEIGHT: 27px"><asp:Button id="Button2" onclick="Button2_Click" runat="server" Width="70px" Text="Delete" Font-Bold="True" __designer:wfdid="w12" ValidationGroup="Panel1" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 22px; HEIGHT: 27px"><asp:Button id="btnclear" onclick="btnclear_Click" runat="server" Width="70px" CausesValidation="False" Text="Clear" Font-Bold="True" __designer:wfdid="w13" CssClass="btnStyle_small"></asp:Button></TD><TD style="WIDTH: 28308px; HEIGHT: 27px">&nbsp;<asp:Button id="Button3" onclick="Button3_Click" runat="server" Width="70px" Text="Close" Font-Bold="True" __designer:wfdid="w14" CssClass="btnStyle_small"></asp:Button> </TD></TR><TR><TD style="WIDTH: 235px; HEIGHT: 27px"></TD><TD style="HEIGHT: 27px"></TD><TD style="WIDTH: 22px; HEIGHT: 27px"></TD><TD style="WIDTH: 28308px; HEIGHT: 27px"></TD></TR></TBODY></TABLE></asp:Panel> </TD><TD style="WIDTH: 409px" vAlign=top><asp:GridView id="GridView1" runat="server" Width="390px" ForeColor="#333333" __designer:wfdid="w1" PageSize="4" OnSorting="GridView1_Sorting" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCreated="GridView1_RowCreated" OnPageIndexChanging="GridView1_PageIndexChanging" CellPadding="4" AllowSorting="True" AllowPaging="True">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
</Columns>

<RowStyle BackColor="#EFF3FB"></RowStyle>

<EditRowStyle BackColor="#2461BF" HorizontalAlign="Left"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR><TR><TD style="WIDTH: 371px" vAlign=top>&nbsp;<asp:SqlDataSource id="SqlDataSource1" runat="server" __designer:wfdid="w1" ConnectionString="<%$ ConnectionStrings:tdbnewConnectionString3 %>" ProviderName="<%$ ConnectionStrings:tdbnewConnectionString3.ProviderName %>" SelectCommand="SELECT state_id, statename FROM m_sub_state"></asp:SqlDataSource></TD><TD style="WIDTH: 409px; HEIGHT: 27px">&nbsp;</TD></TR><TR><TD style="WIDTH: 371px" vAlign=top><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ForeColor="White" ValidationGroup="Panel1" SetFocusOnError="True" ErrorMessage="Data required" ControlToValidate="TextBox1"></asp:RequiredFieldValidator> </TD><TD style="WIDTH: 409px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator1">
            </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 371px" vAlign=top><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ForeColor="White" ValidationGroup="Panel1" SetFocusOnError="True" ErrorMessage="Data required" ControlToValidate="txtbranch"></asp:RequiredFieldValidator> </TD><TD style="WIDTH: 409px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator3">
            </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 371px" vAlign=top><asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ForeColor="White" ValidationGroup="Panel1" SetFocusOnError="True" ErrorMessage="Data required" ControlToValidate="txtaccount"></asp:RequiredFieldValidator> </TD><TD style="WIDTH: 409px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator4">
            </cc1:ValidatorCalloutExtender> </TD></TR><TR><TD style="WIDTH: 371px; HEIGHT: 18px" vAlign=top><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ForeColor="White" __designer:wfdid="w11" ValidationGroup="Panel1" SetFocusOnError="True" ErrorMessage="Data required" ControlToValidate="ComboBox1" InitialValue="--Select--"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 409px; HEIGHT: 18px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" __designer:wfdid="w12" TargetControlID="RequiredFieldValidator2"></cc1:ValidatorCalloutExtender></TD></TR><TR><TD style="WIDTH: 371px; HEIGHT: 18px" vAlign=top><asp:RequiredFieldValidator id="ReqLocation" runat="server" ForeColor="White" __designer:wfdid="w3" ValidationGroup="Panel1" ErrorMessage="Data Required" ControlToValidate="txtLocation"></asp:RequiredFieldValidator></TD><TD style="WIDTH: 409px; HEIGHT: 18px"><cc1:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" __designer:wfdid="w4" TargetControlID="ReqLocation"></cc1:ValidatorCalloutExtender></TD></TR></TBODY></TABLE><asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel3" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Text="Tsunami ARMS - Confirmation" Font-Bold="True" ForeColor="MediumBlue"></asp:Label></asp:Panel> <BR /><asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD style="HEIGHT: 22px"></TD><TD style="HEIGHT: 22px" align=center>&nbsp;<asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="HEIGHT: 22px" align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 10px"></TD><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="btnOk" onclick="btnOk_Click1" runat="server" Width="50px" CausesValidation="False" Text="OK" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD align=center>&nbsp;</TD></TR><TR><TD></TD><TD align=center></TD><TD align=center></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel> <TABLE><TBODY><TR><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton30" onclick="ImageButton30_Click1" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/Policy_0.jpg" __designer:wfdid="w15" Visible="False"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 21px"><asp:ImageButton id="ImageButton22" onclick="ImageButton22_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnWorkingPlace_0.jpg" __designer:wfdid="w7" Visible="False"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px"><asp:ImageButton id="ImageButton29" onclick="ImageButton29_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/Task Action_0.jpg" __designer:wfdid="w8" Visible="False"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton19" onclick="ImageButton19_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnStaff Category_0.jpg" __designer:wfdid="w9" Visible="False"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:ImageButton id="ImageButton26" onclick="ImageButton26_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/Frequency_0.jpg" __designer:wfdid="w10" Visible="False"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px"><asp:ImageButton id="ImageButton12" onclick="ImageButton12_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnForm_0.jpg" __designer:wfdid="w11" Visible="False"></asp:ImageButton></TD></TR><TR><TD style="WIDTH: 100px"><asp:ImageButton id="ImageButton3" onclick="ImageButton3_Click" runat="server" Width="200px" Height="20px" ImageUrl="images/Buttons/SubBtnBudget Head_0.jpg" __designer:wfdid="w12" Visible="False"></asp:ImageButton></TD></TR></TBODY></TABLE>&nbsp;&nbsp; <asp:Button style="DISPLAY: none" id="Button1" runat="server" Text="Hidden"></asp:Button> <cc1:ModalPopupExtender id="ModalPopupExtender2" runat="server" TargetControlID="Button1" RepositionMode="RepositionOnWindowResize" PopupControlID="pnlMessage">
                </cc1:ModalPopupExtender></TD></TR></TBODY></TABLE>
</contenttemplate>
        </asp:UpdatePanel>
         </center>
   
    
    
  
      
      
  
        
     
   
  
     
      
    
    
    
    
   
       
    </div>


</asp:Content>
