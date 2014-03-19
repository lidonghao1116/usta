<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProject.aspx.cs" Inherits="USTA.WebApplication.Teacher.AddProject" %>

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
    <div><h3>添加新项目</h3></div>
    <div>
        <table>
            <tr>
                <td>项目名称：</td>
                <td><asp:TextBox ID="NewProjectName" runat="server" Width="200" CssClass="required"></asp:TextBox>
                    <asp:HiddenField ID="hf_ProjectId" runat="server"/>
                </td>
            </tr>
            <tr>
                <td valign="top">项目描述：</td>
                <td><asp:TextBox ID="ProjectDesc" runat="server" TextMode="MultiLine" Columns="50" Rows="6" CssClass="required"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    项目分类:
                </td>
                <td>
                    <asp:DropDownList ID="RootProjectCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RootCategoryChanged">
                        <asp:ListItem Value="0">-请选择-</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="SubProjectCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SubCategoryChanged">
                        <asp:ListItem Value="0">-请选择-</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ThirdProjectCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ThirdCategoryChanged">
                        <asp:ListItem Value="0">-请选择-</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>项目负责人：</td>
                <td>
                    <asp:DropDownList ID="ProjectUserName" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0">-请选择-</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btn_AddNewProject" runat="server" Text="提交" OnClick="AddNewProject_Click"/>
                </td>
            </tr>
        </table>
    
    </div>
    </div>
    </form>
</body>
</html>
