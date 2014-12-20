<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login frame.aspx.cs" Inherits="login2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head runat="server">
    <title>Login page</title>
     <link href="TRMS_Style.css" rel="stylesheet" type ="text/css" /> 
     <style type="text/css">
 td {color:darkblue;}
  
         .style1
         {
             height: 20px;
         }
  
  
 
  .modalBackground
{
background-color: Gray;
filter: alpha(opacity=80);
opacity: 0.8;
z-index: 10000;
}
  
 </style>
</head>
<body class="background">
    <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
        &nbsp;
        <asp:Panel ID="Panel1" runat="server" Height="543px" Style="z-index: 101; left: 5px;
            position: absolute; top: 173px" Width="776px">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Banner/untitled.JPG"
                Width="771px" Height="340px" /><br />
            <br />
            
            <asp:Panel ID="Panel4" runat="server" BackColor="White" Width="300px">
                <table width="100%">
                    <tr>
                        <td align="center" class="style1" colspan="2" width="100%">
                            <asp:Label ID="lblCounter" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="style1" colspan="2" width="100%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" width="50%">
                            <asp:Button ID="btnYes" runat="server" Text="Yes" onclick="btnYes_Click" />
                        </td>
                        <td align="center" width="50%">
                            <asp:Button ID="btnNO" runat="server" Text="No" onclick="btnNO_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" Height="540px" Style="z-index: 103; left: 791px;
            position: absolute; top: 173px" Width="304px">
            <br />
            <br />
        <asp:Login ID="Loginframe" runat="server" BackColor="#FFFBD6" BorderColor="#FFDFAD" BorderStyle="Solid"
            BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" Height="174px" Width="274px" BorderPadding="4" ForeColor="#333333" TextLayout="TextOnTop" OnAuthenticate="Loginframe_Authenticate">
            <TitleTextStyle BackColor="#990000" Font-Bold="True" ForeColor="White" Font-Size="0.9em" />
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <TextBoxStyle Font-Size="0.8em" />
            <LoginButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid" BorderWidth="1px"
                Font-Names="Verdana" Font-Size="0.8em" ForeColor="#990000" />
        </asp:Login>
        </asp:Panel>
        &nbsp;
        <asp:Panel ID="Panel3" runat="server" Height="150px" Width="1084px">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Banner/Copy of Masterbanner1.JPG"
                Width="1078px" />
        </asp:Panel>
        <asp:Button ID="btnCheck" runat="server" Text="Button" 
            onclick="btnCheck_Click" style="display:none"/>
        <asp:Button ID="Button1" runat="server" Text="Button" style="display:none" />
        <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                                        PopupControlID="Panel4" RepositionMode="RepositionOnWindowResize" 
                                        TargetControlID="Button1" BackgroundCssClass="modalBackground">
                                    </cc1:ModalPopupExtender>
       



       <asp:Panel ID="Panel5" runat="server" BackColor="White" Width="300px">
                <table width="100%">
                    <tr>
                        <td align="center"   width="100%">
                            <asp:Label ID="lblOk" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center"  width="100%">
                            &nbsp;</td>
                    </tr>
                     <tr>
                        <td align="center"  width="100%">
                            <asp:Button ID="btnOk" runat="server" Text="OK" /></td>
                    </tr>
                    
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                                        PopupControlID="Panel5" RepositionMode="RepositionOnWindowResize" 
                                        TargetControlID="Button1" BackgroundCssClass="modalBackground">
                                    </cc1:ModalPopupExtender>

    </div>
   
    </form>

    <%--<script type="text/javascript">

        document.onkeyup = KeyCheck;

        function KeyCheck(e) {
                //alert("hjv");
            var KeyID = (window.event) ? event.keyCode : e.keyCode;
            if (KeyID == 13) {
               // alert("hjv");
                var btn = document.getElementById('<%= btnCheck.ClientID %>');
                btn.click();

            }

        }
</script>--%>
     
</body>


</html>
