<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  AutoEventWireup="true" CodeFile="TeamMaster.aspx.cs" Inherits="Team_Master" Title="Untitled Page" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE><TBODY><TR><TD align=center colSpan=2><asp:Label id="lblheading" runat="server" ForeColor="#000099" Text="Team Master" Font-Bold="True" Font-Size="14pt" Font-Names="Arial" CssClass="heading" __designer:wfdid="w15"></asp:Label></TD></TR><TR><TD vAlign=top><asp:Panel id="pnlteam" runat="server" GroupingText="Team Details" BackColor="Transparent"><TABLE><TBODY><TR><TD vAlign=top><asp:Label id="lblTeamName" runat="server" Width="118px" Height="12px" Text="Team Name" __designer:wfdid="w8"></asp:Label></TD><TD style="WIDTH: 156px; TEXT-ALIGN: left" vAlign=top colSpan=1><asp:DropDownList id="cmbTeam" runat="server" Width="148px" Height="22px" __designer:wfdid="w9" ValidationGroup="team" OnSelectedIndexChanged="cmbTeam_SelectedIndexChanged" AutoPostBack="True" DataTextField="teamname" DataValueField="team_id"><asp:ListItem></asp:ListItem>

                </asp:DropDownList> <asp:TextBox id="txtTeam" runat="server" Width="140px" CssClass="UpperCaseFirstLetter" __designer:wfdid="w10" Visible="False" AutoPostBack="True" OnTextChanged="txtTeam_TextChanged"></asp:TextBox></TD><TD vAlign=top colSpan=3><asp:LinkButton id="lnkteam" onclick="lnkteam_Click" runat="server" CausesValidation="False" ForeColor="Blue" __designer:wfdid="w11">New</asp:LinkButton> &nbsp; &nbsp; <BR /></TD></TR><TR><TD style="HEIGHT: 22px" vAlign=top colSpan=1><asp:Label id="lblSuprvise" runat="server" Width="111px" Text="Supervising Office" __designer:wfdid="w12"></asp:Label> </TD><TD style="WIDTH: 156px; HEIGHT: 22px" vAlign=top colSpan=1><asp:DropDownList id="cmbOfficer" runat="server" Width="148px" Height="22px" CausesValidation="True" __designer:wfdid="w13" ValidationGroup="team" AutoPostBack="True" DataTextField="office" DataValueField="office_id"><asp:ListItem></asp:ListItem>

                </asp:DropDownList></TD><TD colSpan=4><asp:Label id="lblduplicteteam" runat="server" ForeColor="Red" Text="label" Font-Bold="True" __designer:wfdid="w14" Visible="False"></asp:Label></TD></TR><TR><TD style="HEIGHT: 22px" vAlign=top colSpan=1></TD><TD style="WIDTH: 156px; HEIGHT: 22px" vAlign=top colSpan=1></TD><TD colSpan=4></TD></TR></TBODY></TABLE></asp:Panel></TD><TD vAlign=top><asp:Panel id="pnlstaffdtls" runat="server" Width="100%" GroupingText="Staff Details" BackColor="Transparent"><TABLE><TBODY><TR><TD><asp:Label id="lblstaff" runat="server" Width="84px" Text="Staff Name" __designer:wfdid="w7"></asp:Label></TD><TD><asp:DropDownList id="cmbStaff" runat="server" Width="150px" Height="22px" ValidationGroup="staff" OnSelectedIndexChanged="cmbStaff_SelectedIndexChanged" AutoPostBack="True" DataTextField="staffname" DataValueField="staff_id"><asp:ListItem></asp:ListItem>

                </asp:DropDownList></TD><TD><asp:Button id="btnaddmem" onclick="btnaddmem_Click" runat="server" Text="Add Members" CssClass="btnStyle_medium" ValidationGroup="staff"></asp:Button></TD></TR><TR><TD colSpan=3><asp:GridView id="dgstaff" runat="server" Width="452px" HorizontalAlign="Left" ForeColor="#333333" DataKeyNames="staff_id" GridLines="None" CellPadding="4" PageSize="5" AllowPaging="True" OnPageIndexChanging="dgstaff_PageIndexChanging" AutoGenerateColumns="False">
<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"></FooterStyle>

<RowStyle BackColor="#EFF3FB"></RowStyle>
<Columns>
<asp:BoundField DataField="staff_id" HeaderText="Staffid" Visible="False"></asp:BoundField>
<asp:BoundField DataField="staffname" HeaderText="Staff Name"></asp:BoundField>
<asp:BoundField DataField="designation" HeaderText="Designation"></asp:BoundField>
<asp:BoundField DataField="office" HeaderText="Office"></asp:BoundField>
<asp:BoundField DataField="status" HeaderText="status" Visible="False"></asp:BoundField>
<asp:TemplateField><ItemTemplate>
<asp:LinkButton id="lbtndel" runat="server" CausesValidation="False" OnCommand="staffDelete" CommandArgument='<%#Eval("staff_id")%>'>Delete</asp:LinkButton> 
 
    </ItemTemplate>
</asp:TemplateField>

                    </Columns>

<PagerStyle HorizontalAlign="Center" BackColor="#2461BF" ForeColor="White"></PagerStyle>
<EmptyDataTemplate>
&nbsp; 
 
                    </EmptyDataTemplate>

<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

<HeaderStyle HorizontalAlign="Left" BackColor="#507CD1" Font-Bold="True" ForeColor="White"></HeaderStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>

                </asp:GridView></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=2><asp:Panel id="pnltskdtl" runat="server" Width="100%" Height="100%" GroupingText="Task Details" BackColor="Transparent"><TABLE><TBODY><TR><TD style="TEXT-ALIGN: center" vAlign=top align=left colSpan=2><TABLE><TBODY><TR><TD colSpan=1><asp:Label id="lblPlace" runat="server" Width="100px" Text="Working Place" meta:resourcekey="LblPlaceResource1"></asp:Label></TD><TD colSpan=1><asp:DropDownList id="cmbWork" runat="server" Width="146px" Height="22px" ValidationGroup="task" OnSelectedIndexChanged="cmbWork_SelectedIndexChanged" AutoPostBack="True" DataTextField="buildingname" DataValueField="build_id">
                </asp:DropDownList></TD><TD style="WIDTH: 74px"></TD></TR><TR><TD><asp:Label id="lblTask" runat="server" Width="100px" Text="Team Task" meta:resourcekey="LblTaskResource1"></asp:Label></TD><TD><asp:DropDownList id="cmbTask" runat="server" Width="145px" Height="22px" ValidationGroup="task" OnSelectedIndexChanged="cmbTask_SelectedIndexChanged" AutoPostBack="True" DataTextField="taskname" DataValueField="task_id">
                </asp:DropDownList></TD><TD style="WIDTH: 74px"><asp:LinkButton id="lnktask" onclick="lnktask_Click" runat="server" Width="33px" CausesValidation="False" ForeColor="Blue" Text="New" meta:resourcekey="LNkNewTskResource1"></asp:LinkButton></TD></TR><TR><TD></TD><TD align=left><asp:Button id="btnAddTask" onclick="btnAddTask_Click" runat="server" Text="Add Task" CssClass="btnStyle_medium" ValidationGroup="task">
                </asp:Button></TD><TD style="WIDTH: 74px"></TD></TR><TR><TD colSpan=3><asp:GridView id="dgWrk" runat="server" Width="300px" Height="100%" HorizontalAlign="Left" ForeColor="#333333" OnSelectedIndexChanged="dgWrk_SelectedIndexChanged" DataKeyNames="task_id" GridLines="None" CellPadding="4" PageSize="3" AllowPaging="True" OnPageIndexChanging="dgWrk_PageIndexChanging" AutoGenerateColumns="False">

<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
<Columns>

<asp:BoundField DataField="task_id" Visible="False" HeaderText="Task Id"></asp:BoundField>
<asp:BoundField DataField="taskname" HeaderText="Task Name"></asp:BoundField>
<asp:BoundField DataField="build_id" Visible="False" HeaderText="Work place Id"></asp:BoundField>
<asp:BoundField DataField="workplace" HeaderText="Work Place"></asp:BoundField>
<asp:TemplateField><ItemTemplate> 

<asp:LinkButton id="wrklnk" runat="server" CausesValidation="False" OnCommand="taskDelete" CommandArgument='<%#Eval("task_id").ToString()+","+Eval("build_id").ToString()%>'>Delete</asp:LinkButton> 
    </ItemTemplate>
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
</asp:TemplateField>
                            </Columns>
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            

<RowStyle BackColor="#EFF3FB"></RowStyle>
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            

<EditRowStyle BackColor="#2461BF"></EditRowStyle>
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True">
                            </SelectedRowStyle>
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center">
                            </PagerStyle>
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                </asp:GridView></TD></TR></TBODY></TABLE>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD><TD vAlign=top align=center><TABLE><TBODY><TR><TD>Item Category</TD><TD><asp:DropDownList id="cmbItemcat" runat="server" Width="200px" ValidationGroup="inventory" OnSelectedIndexChanged="cmbItemcat_SelectedIndexChanged1" AutoPostBack="True" DataTextField="itemcatname" DataValueField="itemcat_id"><asp:ListItem></asp:ListItem>
                </asp:DropDownList></TD><TD style="WIDTH: 25px"><asp:LinkButton id="lnkCatgry" onclick="lnkCatgry_Click" runat="server" Width="25px" CausesValidation="False" ForeColor="Blue">New</asp:LinkButton></TD><TD><asp:Label id="lblmin" runat="server" Width="85px" Text="MInimum Qty" __designer:wfdid="w3"></asp:Label></TD><TD><asp:Label id="lblmax" runat="server" Width="85px" Text="Maximum Qty" __designer:wfdid="w5"></asp:Label></TD></TR><TR><TD>Item Name</TD><TD><asp:DropDownList id="cmbItem" runat="server" Width="200px" Height="22px" __designer:wfdid="w6" ValidationGroup="inventory" AutoPostBack="True" DataTextField="itemname" DataValueField="item_id"><asp:ListItem></asp:ListItem>
                </asp:DropDownList></TD><TD style="WIDTH: 25px"><asp:LinkButton id="lnkItem" onclick="lnkItem_Click" runat="server" Width="25px" CausesValidation="False" ForeColor="Blue">New</asp:LinkButton></TD><TD><asp:TextBox id="txtMin" runat="server" Width="59px" Height="15px" __designer:wfdid="w4" ValidationGroup="inventory" OnTextChanged="txtMin_TextChanged"></asp:TextBox></TD><TD><asp:TextBox id="txtMax" runat="server" Width="58px" Height="15px" ValidationGroup="inventory" OnTextChanged="txtMax_TextChanged"></asp:TextBox></TD></TR><TR><TD></TD><TD align=left><asp:Button id="btnaddinven" onclick="btnaddinven_Click" runat="server" Text="Add Inventory" CssClass="btnStyle_medium" ValidationGroup="inventory">
                </asp:Button></TD><TD style="WIDTH: 25px"></TD><TD></TD><TD></TD></TR><TR><TD colSpan=5><asp:GridView id="dgitem" runat="server" Width="458px" HorizontalAlign="Left" ForeColor="#333333" OnSelectedIndexChanged="dgitem_SelectedIndexChanged" DataKeyNames="item_id,task_id" GridLines="None" CellPadding="4" PageSize="3" AllowPaging="True" OnPageIndexChanging="dgitem_PageIndexChanging" AutoGenerateColumns="False" OnRowDeleting="dgitem_RowDeleting">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True">
                            </FooterStyle>
                            
                            
                            
                            
                            
            
                    
            
                    
            
                    
            
                    
            
                    
<Columns>

<asp:BoundField DataField="item_id" Visible="False" HeaderText="Item Id"></asp:BoundField>
<asp:BoundField DataField="category" HeaderText="Category"></asp:BoundField>
<asp:BoundField DataField="itemname" HeaderText="Item "></asp:BoundField>
<asp:BoundField DataField="task_id" Visible="False" HeaderText="Task Id"></asp:BoundField>
<asp:BoundField DataField="taskname" HeaderText="Task Name"></asp:BoundField>
<asp:BoundField DataField="min_qty" HeaderText="Min: Qty"></asp:BoundField>
<asp:BoundField DataField="max_qty" HeaderText="Max: Qty"></asp:BoundField>
<asp:CommandField SelectText=""></asp:CommandField>
<asp:TemplateField><ItemTemplate> 

<asp:LinkButton id="lnkbtn" runat="server" CausesValidation="False" OnCommand="itemDelete" CommandArgument='<%#Eval("item_id").ToString()+","+Eval("task_id").ToString()%>'>Delete</asp:LinkButton> 
    </ItemTemplate>
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
</asp:TemplateField>
                            </Columns>
                            
                            
                            
                            
                            
            
                    
            
                    
            
                    
            
                    
            
                    

<RowStyle BackColor="#EFF3FB"></RowStyle>
                            
                            
                            
                            
                            
            
                    
            
                    
            
                    
            
                    
            
                    

<EditRowStyle BackColor="#2461BF"></EditRowStyle>
                            
                            
                            
                            
                            
            
                    
            
                    
            
                    
            
                    
            
                    

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True">
                            </SelectedRowStyle>
                            
                            
                            
                            
                            
            
                    
            
                    
            
                    
            
                    
            
                    

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center">
                            </PagerStyle>
                            
                            
                            
                            
                            
            
                    
            
                    
            
                    
            
                    
            
                    

<HeaderStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                            
                            
                            
                            
                            
            
                    
            
                    
            
                    
            
                    
            
                    

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
                            
                            
                            
                            
                            
            
                    
            
                    
            
                    
            
                    
            
                    
                </asp:GridView></TD></TR></TBODY></TABLE><asp:Label id="Label3" runat="server" Width="88px" Text="Item Name" Visible="False"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label id="lblItemcat" runat="server" Width="81px" Text="Item Category" Visible="False"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD align=center colSpan=2><asp:Label id="lblMessage" runat="server" Width="315px" ForeColor="Red" Font-Bold="True"></asp:Label></TD></TR><TR><TD align=center colSpan=2><asp:Button id="btnSaveteam" onclick="btnSaveteam_Click" runat="server" Text="Save" CssClass="btnStyle_small" ValidationGroup="team"></asp:Button> <asp:Button id="btndelete" onclick="btndelete_Click" runat="server" Text="Delete" CssClass="btnStyle_small"></asp:Button>&nbsp;<asp:Button id="btnclrtask" onclick="btnclrtask_Click" runat="server" CausesValidation="False" Text="Clear" CssClass="btnStyle_small"></asp:Button> <asp:Button id="btnReport" onclick="btnReport_Click" runat="server" CausesValidation="False" Text="Report" CssClass="btnStyle_small"></asp:Button> </TD></TR><TR><TD style="HEIGHT: 15px" align=center colSpan=2></TD></TR><TR><TD align=center colSpan=2><asp:GridView id="dgteam" runat="server" Width="830px" ForeColor="Blue" OnSelectedIndexChanged="dgteam_SelectedIndexChanged" DataKeyNames="Team No" GridLines="None" CellPadding="4" PageSize="8" AllowPaging="True" OnPageIndexChanging="dgteam_PageIndexChanging" AllowSorting="True" OnRowCreated="dgteam_RowCreated" Caption="TEAM DETAILS">
<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"></FooterStyle>

<RowStyle HorizontalAlign="Left" BackColor="#EFF3FB"></RowStyle>
<Columns>
<asp:CommandField SelectText="" ShowSelectButton="True"></asp:CommandField>
</Columns>

<PagerStyle HorizontalAlign="Center" BackColor="#2461BF" ForeColor="White"></PagerStyle>

<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

<HeaderStyle HorizontalAlign="Left" BackColor="#507CD1" Font-Bold="True" ForeColor="White"></HeaderStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</asp:GridView></TD></TR><TR><TD colSpan=2><asp:Panel id="pnlReport" runat="server" Width="100%" Height="100%" GroupingText="Report" BackColor="Transparent" Visible="False"><TABLE><TBODY><TR><TD></TD><TD style="TEXT-ALIGN: center" vAlign=top colSpan=3><asp:RadioButtonList id="RadioButtonList1" runat="server" Width="320px" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="True" RepeatDirection="Horizontal"><asp:ListItem>Team Wise</asp:ListItem>
<asp:ListItem>Task Wise</asp:ListItem>

                </asp:RadioButtonList></TD><TD style="TEXT-ALIGN: center" vAlign=top colSpan=1><asp:Button id="btnHidereport" onclick="btnHidereport_Click" runat="server" CausesValidation="False" Text="Hide Report" CssClass="btnStyle_medium"></asp:Button></TD></TR><TR><TD></TD><TD style="TEXT-ALIGN: center" vAlign=top><asp:Label id="Label4" runat="server" Visible="False" Text="Team Name"></asp:Label></TD><TD style="TEXT-ALIGN: center" vAlign=top><asp:DropDownList id="cmbReport" runat="server" Width="152px" Height="22px" Visible="False" OnSelectedIndexChanged="cmbReport_SelectedIndexChanged" DataValueField="team_id" DataTextField="teamname" AutoPostBack="True"><asp:ListItem></asp:ListItem>

                </asp:DropDownList></TD><TD style="TEXT-ALIGN: right" vAlign=top></TD></TR><TR><TD></TD><TD style="TEXT-ALIGN: center" vAlign=top><asp:Label id="lblreporttask" runat="server" Visible="False" Text="Task Name"></asp:Label></TD><TD style="TEXT-ALIGN: center" vAlign=top><asp:DropDownList id="cmbreporttask" runat="server" Width="151px" Height="22px" Visible="False" OnSelectedIndexChanged="cmbreporttask_SelectedIndexChanged" DataValueField="task_id" DataTextField="taskname" AutoPostBack="True"><asp:ListItem Value="-1">-Select-</asp:ListItem>

                </asp:DropDownList></TD><TD style="TEXT-ALIGN: center" vAlign=top></TD><TD style="TEXT-ALIGN: right" vAlign=top></TD></TR><TR><TD></TD><TD style="TEXT-ALIGN: center" vAlign=top colSpan=2><asp:Button id="btnShowreport" onclick="btnshowreport_Click" runat="server" CausesValidation="False" Visible="False" Text="Show Report" CssClass="btnStyle_medium"></asp:Button></TD><TD vAlign=top></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE><TABLE><TBODY><TR vAlign=top><TD rowSpan=1></TD><TD style="WIDTH: 471px" vAlign=top colSpan=1 rowSpan=1></TD><TD style="HEIGHT: 1px; TEXT-ALIGN: center" colSpan=1 rowSpan=1></TD></TR><TR vAlign=top><TD colSpan=2 rowSpan=1>&nbsp;</TD><TD style="TEXT-ALIGN: center" colSpan=1 rowSpan=1></TD></TR><TR vAlign=top><TD style="WIDTH: 289px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RequiredFieldValidator id="rfvteam" runat="server" ForeColor="White" ValidationGroup="team" InitialValue="-1" ControlToValidate="cmbTeam" ErrorMessage="Team Required"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:RequiredFieldValidator id="rfvStaff" runat="server" ForeColor="White" ValidationGroup="staff" InitialValue="-1" ControlToValidate="cmbStaff" ErrorMessage="Staff Required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="rfvWorkplace" runat="server" ForeColor="White" ValidationGroup="task" InitialValue="-1" ControlToValidate="cmbWork" ErrorMessage="Required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="rfvCategory" runat="server" ForeColor="White" ValidationGroup="inventory" InitialValue="-1" ControlToValidate="cmbItemcat" ErrorMessage="Required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="rfvItem" runat="server" ForeColor="White" ValidationGroup="inventory" InitialValue="-1" ControlToValidate="cmbItem" ErrorMessage="Required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="rfvMin" runat="server" ForeColor="White" ValidationGroup="inventory" ControlToValidate="txtMin" ErrorMessage="Required"></asp:RequiredFieldValidator> <asp:RequiredFieldValidator id="rfvMax" runat="server" ForeColor="White" ValidationGroup="inventory" ControlToValidate="txtMax" ErrorMessage="Required"></asp:RequiredFieldValidator>&nbsp; <asp:RequiredFieldValidator id="rfvTask" runat="server" ForeColor="White" ValidationGroup="task" InitialValue="-1" ControlToValidate="cmbTask" ErrorMessage="Selct a task"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;<asp:RequiredFieldValidator id="rfvOffice" runat="server" ForeColor="White" __designer:wfdid="w16" ValidationGroup="team" InitialValue="-1" ControlToValidate="cmbOfficer" ErrorMessage="Select office"></asp:RequiredFieldValidator> <asp:CompareValidator id="CompareValidator1" runat="server" ForeColor="White" __designer:wfdid="w1" ValidationGroup="inventory" ControlToValidate="txtMax" ErrorMessage="Max Qty shold be greater than minimum" Type="Double" Operator="GreaterThan" ControlToCompare="txtMin"></asp:CompareValidator> <cc2:ValidatorCalloutExtender id="vceStaff" runat="server" TargetControlID="rfvStaff"></cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" __designer:wfdid="w2" TargetControlID="CompareValidator1"></cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="vceWorkplace" runat="server" TargetControlID="rfvWorkplace"></cc2:ValidatorCalloutExtender><cc2:ValidatorCalloutExtender id="vceCategory" runat="server" TargetControlID="rfvCategory">
</cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="vceItem" runat="server" TargetControlID="rfvItem">
    </cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="vceOffice" runat="server" __designer:wfdid="w17" TargetControlID="rfvOffice"></cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="vceMax" runat="server" TargetControlID="rfvMax">
    </cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="vceMin" runat="server" TargetControlID="rfvMin">
    </cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="vceTask" runat="server" TargetControlID="rfvTask"></cc2:ValidatorCalloutExtender> <cc2:ValidatorCalloutExtender id="vceteam" runat="server" TargetControlID="rfvteam"></cc2:ValidatorCalloutExtender>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <cc2:ListSearchExtender id="LSEteam" runat="server" TargetControlID="cmbTeam"></cc2:ListSearchExtender> <cc2:ListSearchExtender id="LSEtoffice" runat="server" TargetControlID="cmbOfficer"></cc2:ListSearchExtender> <cc2:ListSearchExtender id="LSEstaff" runat="server" TargetControlID="cmbStaff"></cc2:ListSearchExtender> <cc2:ListSearchExtender id="LSEwork" runat="server" TargetControlID="cmbWork"></cc2:ListSearchExtender> <cc2:ListSearchExtender id="LSEtask" runat="server" TargetControlID="cmbTask"></cc2:ListSearchExtender>&nbsp; <cc2:FilteredTextBoxExtender id="fteteam" runat="server" TargetControlID="txtTeam" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="' '"></cc2:FilteredTextBoxExtender>&nbsp; </TD><TD style="WIDTH: 471px" rowSpan=1><asp:Button style="DISPLAY: none" id="btnHidden" runat="server" Text="Hidden"></asp:Button>&nbsp;<BR />&nbsp;<asp:Panel id="pnlMessage" runat="server" CssClass="ModalWindow" _="" _designer:dtid="562958543355909"><asp:Panel id="Panel6" runat="server" BackColor="LightSteelBlue" BorderStyle="Outset"><asp:Label id="lblHead" runat="server" Text="Tsunami ARMS - Confirmation" ForeColor="MediumBlue" Font-Bold="True"></asp:Label><BR /><asp:Label id="lblHead2" runat="server" Text="Tsunami ARMS - Warning" ForeColor="MediumBlue" Font-Bold="True"></asp:Label></asp:Panel> <asp:Panel id="pnlYesNo" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblMsg" runat="server" Width="220px" ForeColor="Black" Text="Do you want to save?" Font-Size="Small"></asp:Label></TD></TR><TR><TD style="HEIGHT: 26px"></TD><TD style="HEIGHT: 26px" align=center>&nbsp; <asp:Button id="btnYes" onclick="btnYes_Click" runat="server" Width="50px" CausesValidation="False" Text="Yes" Font-Bold="True" CssClass="btnStyle"></asp:Button> <asp:Button id="btnNo" onclick="btnNo_Click" runat="server" Width="50px" CausesValidation="False" Text="No" Font-Bold="True" CssClass="btnStyle"></asp:Button>&nbsp;</TD><TD style="HEIGHT: 26px" align=center>&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="pnlOk" runat="server" Width="125px" Height="50px"><TABLE style="WIDTH: 237px"><TBODY><TR><TD align=center colSpan=1></TD><TD align=center colSpan=3><asp:Label id="lblOk" runat="server" Width="223px" ForeColor="Black" Text="Do you want to ?" Font-Size="Small"></asp:Label></TD></TR><TR><TD></TD><TD align=center>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnOk" onclick="btnOk_Click" runat="server" Width="50px" CausesValidation="False" ForeColor="Black" Text="OK" Font-Bold="True" CssClass="btnStyle"></asp:Button> &nbsp; </TD><TD style="WIDTH: 20px" align=center>&nbsp;</TD></TR></TBODY></TABLE></asp:Panel> <cc2:ModalPopupExtender id="ModalPopupExtender2" runat="server" TargetControlID="btnHidden" PopupControlID="pnlMessage" RepositionMode="RepositionOnWindowResize">
                </cc2:ModalPopupExtender></asp:Panel>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD><TD>&nbsp;</TD></TR><TR vAlign=top><TD style="WIDTH: 289px">&nbsp; &nbsp;&nbsp; </TD><TD style="WIDTH: 471px" rowSpan=1></TD><TD></TD></TR></TBODY></TABLE>
</contenttemplate>


<Triggers>
    <asp:PostBackTrigger ControlID="btnShowreport" />
    
  </Triggers>





    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:Panel ID="Panel5" runat="server" GroupingText="User Tips" Width="100%">
        <br />
                Select a Team name or Enter the new team by Clicking the corresponding New
                Link.
        <p>
            A supervising
                officer is needed for each team. Select Required staff from list and Press Add Members Button. 
        </p>
        <p>
            Then
                Add the workplace and task of a team.</p>
        <p>
            Select a workplace and task before selecting the items needed to complete the
                task
        </p>
        <p>
            Items needed can be selected by selecting the appropriate category of item and
                    then itemname,which is not mandatory.
        </p>
        <p>
            After selecting sufficient
                details of a team.Click on Save Button.
        </p>
        <p>
            Authorised user can view
                the report of each team task
        </p>
    </asp:Panel>
</asp:Content>

