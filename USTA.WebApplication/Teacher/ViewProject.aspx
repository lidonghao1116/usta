<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewProject.aspx.cs" Inherits="USTA.WebApplication.Teacher.ViewProject" %>

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
        <div><h3>项目详情</h3></div>
        <div>
                <table class="tableAddStyleNone" width="100%">
            <tr>
                <td class="border" width="15%">项目名称：</td>
                <td class="border" width="85%">
                    <asp:Literal ID="literal_ViewProjectName" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="border">项目分类：</td>
                <td class="border">
                    <asp:Literal ID="literal_ViewProjectCategory" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="border">项目负责人：</td>
                <td class="border">
                    <asp:Literal ID="literal_UserName" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="border">登记时间：</td>
                <td class="border">
                    <asp:Literal ID="literal_CreatedTime" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="border" valign="top">
                    项目描述：
                </td>
                <td class="border">
                    <asp:Literal ID="literal_ProjectMemo" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
        </div>
    </div>
    </form>
</body>
</html>
