<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnswerNormConfirm.aspx.cs" Inherits="USTA.WebApplication.Administrator.AnswerNormConfirm" %>

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
       <table width="80%" class="tableAddStyleNone">
       <tr><td class="border">问题：</td><td class="border"><asp:Label id="lblquestion" runat="server"></asp:Label></td></tr>
    <tr><td class="border">回答：</td><td class="border"><asp:TextBox ID="txtAnswer" runat="server" Height="89px" 
            TextMode="MultiLine" Width="225px" class="required"></asp:TextBox></td></tr>
    <tr><td class="border" colspan="2"><asp:Button ID="btnCommit" runat="server" Text="提交" 
            onclick="btnCommit_Click" /></td></tr>
    </table>
    </div>
    </form>
</body>
</html>
