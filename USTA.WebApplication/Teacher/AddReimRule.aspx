<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddReimRule.aspx.cs" Inherits="USTA.WebApplication.Teacher.AddReimRule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <div>
        <h3>添加项目报销规则</h3>
    </div>
    <div>
        <table width="80%" class="tableAddStyleNone">
            <tr>
                <td width="35%" class="border">
                    项目名称：
                </td>
                <td width="65%" class="border">
                    <asp:Literal ID="literal_ProjectName" runat="server"></asp:Literal>
                    <asp:HiddenField ID="hf_ProjectId" runat="server"/>
                </td>
            </tr>
            <tr>
                <td class="border">
                    报销项：
                </td>
                <td class="border">
                    <asp:DropDownList ID="ddlReimLists" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="border">
                    单次报销最大金额：
                </td>
                <td class="border">
                    <asp:TextBox ID="tb_reimValue" runat="server" CssClass="required number"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="border">
                    该报销项最大报销金额：
                </td>
                <td class="border">
                    <asp:TextBox ID="tb_maxReimValue" runat="server" CssClass="required number"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btn_AddRule" Text="添加规则" runat="server" OnClick="AddReimRule_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </div>
    </form>
</body>
</html>
