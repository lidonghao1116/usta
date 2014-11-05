<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddGradeCheckItem.aspx.cs" Inherits="USTA.WebApplication.Administrator.AddGradeCheckItem" %>

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
                请选择规则所属学年：
            </td>
            <td class="border">
                &nbsp;<asp:CheckBoxList ID="ddlTermYears" runat="server" RepeatColumns="3">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="border">
                成绩审核单项名称：
            </td>
            <td class="border">
                <asp:TextBox ID="txtGradeCheckItemName" runat="server" CssClass="required" Width="600px">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="border">
                成绩审核单项默认值：
            </td>
            <td class="border">
                <asp:TextBox ID="txtGradeCheckItemDefaultValue" runat="server" CssClass="required" Width="600px" Text="无">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="border">
                导出Excel时的列显示顺序（请输入整数）：
            </td>
            <td class="border">
                <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="required number" Width="100px">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="border">
            </td>
            <td class="border">
                <asp:Button ID="btnAdd" runat="server" Text="确定添加" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <script type="text/javascript">
        $(document).ready(function() {
            $("#form1").validate();
        });
        
    </script>
    </form>
</body>
</html>
