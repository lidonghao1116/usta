<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNormConfirm.aspx.cs" Inherits="USTA.WebApplication.Teacher.AddNormConfirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
    <!--Validate-->
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#form1").validate();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%" class="tableAddStyleNone">
    <tr><td class="border">疑问</td><td class="border"><asp:TextBox ID="txtproblem" runat="server" Height="125px" 
            TextMode="MultiLine" Width="240px" class="required"></asp:TextBox></td></tr>
    <tr><td class="border"></td><td class="border"><asp:Button ID="btnCommit" runat="server" 
            onclick="btnCommit_Click" Text="提交" /></td></tr>
    </table>
    </div>
    </form>
</body>
</html>
