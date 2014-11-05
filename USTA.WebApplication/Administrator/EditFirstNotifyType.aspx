<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditFirstNotifyType.aspx.cs" Inherits="USTA.WebApplication.Administrator.EditFirstNotifyType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="masterHead" runat="server">
    <title></title>
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
    <!--Validate-->
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" class="tableAddStyleNone">
            <tr>
            <td class="border">
                请修改一级分类名称：
            </td>
            <td class="border">
              <asp:TextBox ID="txtTypeName" runat="server" Width="200px" CssClass="required"></asp:TextBox>
            </td>
        </tr>
            <tr>
            <td class="border">
                显示顺序：
            </td>
            <td class="border">
              <asp:TextBox ID="txtSequence" runat="server" Width="30px" CssClass="required number"></asp:TextBox>(输入整数)
            </td>
        </tr><tr>
            <td class="border">
            </td>
            <td class="border"><asp:Button ID="btnSubmit" runat="server" Width="78px" Text="修改" OnClick="btnSubmit_Click" /></td>
        </tr>
        </table>
    </form>
</body>
</html>
