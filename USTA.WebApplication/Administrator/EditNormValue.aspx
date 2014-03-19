<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditNormValue.aspx.cs" Inherits="USTA.WebApplication.Administrator.EditNormValue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
        <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#form1").validate();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:HiddenField ID="TextNormId" runat="server" />
     <asp:HiddenField ID="TextTeacherNo" runat="server" />
      <asp:HiddenField ID="TextTerm" runat="server" />
     <%-- <table width="80%" class="tableAddStyleNone">
      <tr><td class="border"> 值：</td><td class="border">  <asp:TextBox ID="TextNormValue" runat="server"></asp:TextBox ></td></tr>
    <tr><td colspan="2" class="border"> <asp:Button runat="server" ID="btnSubmit" onclick="btnSubmit_Click" Text="提交" /></td></tr>
       </table>--%>
    </div>
    <div>
     <table width="100%" class="tableAddStyle">
     <tr><td>
    指标：<asp:Label ID="lblnormTitle" runat="server"></asp:Label></td></tr>
     <tr><td>
    所属子指标：</td></tr>
     </table>
    <asp:DataList ID="dsltchildNorm" runat="server" Width="100%" OnItemDataBound="dsltchildNorm_OnItemDataBound">
    <ItemTemplate>
     <table width="100%" class="tableAddStyleNone">
     <tr><td class="border" width="30%"><%#Eval("name") %></td><td class="border">
     <asp:HiddenField ID="hidnormId" runat="server" />
     <asp:TextBox ID="itemValue" runat="server"></asp:TextBox></td></tr>
     </table>
    </ItemTemplate>
    </asp:DataList>
    <table width="100%" class="tableAddStyleNone">
    <tr><td colspan="2" class="border"><asp:Button runat="server" ID="btnSubmit" onclick="btnSubmit_Click" Text="提交" /></td></tr>
    </table>
    </div>
    </form>
</body>
</html>
