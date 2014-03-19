<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddGameType.aspx.cs" Inherits="USTA.WebApplication.Administrator.AddGameType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
    <!--Validate-->
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
    <table class="tableAddStyle" width="100%">
        <tr>
            <td width="15%">
                请选择活动届次：
            </td>
            <td>
                <asp:DropdownList ID="ddlGameCategory" runat="server" ClientIDMode="Static"></asp:DropdownList>
            </td>
        </tr>
        <tr>
            <td width="15%">
                活动类型名称：
            </td>
            <td>
                <asp:TextBox ID="txtGameTypeTitle" runat="server" Width="600px" ClientIDMode="Static"
                    CssClass="required"></asp:TextBox>
            </td>
        </tr>
                <tr>
                    <td width="15%">每组人数设定：
                    </td>
                    <td>
                        <asp:TextBox ID="txtGroupCapability" runat="server" Width="200px" ClientIDMode="Static"
                CssClass="required number" Text="4"></asp:TextBox>
                    </td>
                </tr>
        <tr>
            <td width="15%">
                请选择活动面向的教师类型：
            </td>
            <td>
                <asp:RadioButtonList ID="rblTeacher" runat="server">
                    <asp:ListItem Value="1">男教师</asp:ListItem>
                    <asp:ListItem Value="2">女教师</asp:ListItem>
                    <asp:ListItem Value="12">男教师和女教师</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="rblTeacher" ErrorMessage="请选择活动面向的教师类型"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td width="15%">
            </td>
            <td>
                <asp:Button ID="Button2" runat="server" Text="提交" OnClick="Button2_Click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
