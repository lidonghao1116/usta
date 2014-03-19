<%@ Page Language="C#" AutoEventWireup="true" Inherits="Administrator_EditForum" Codebehind="EditForum.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="masterHead">
    <title></title>
     <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
     <!--Validate-->

    
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>

    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#aspnetForm").validate();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    <table class="tableEditStyle beautyTableStyle" width="100%">
    <tr><td>版面名称：</td><td><asp:TextBox ID="txtForumTitle" runat="server" CssClass="required"></asp:TextBox> </td></tr>
    <tr><td>邮件通知地址：</td><td><asp:TextBox ID="txtEmail" runat="server" CssClass="required"></asp:TextBox></td></tr>
    <tr><td style="border-bottom:0px;"></td><td style="border-bottom:0px;"><asp:Button ID="btnCommit" runat="server" Text="确定" onclick="btnCommit_Click" /></td></tr>
    </table>
    
    
        
    </div>
    </form>
</body>
</html>
