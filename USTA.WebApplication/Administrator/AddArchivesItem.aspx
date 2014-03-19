<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddArchivesItem.aspx.cs" Inherits="USTA.WebApplication.Administrator.AddArchivesItem" %>

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
                请选择学期：
            </td>
            <td class="border">
               <asp:DropDownList ID="ddlTermTags" runat="server">
            </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="border">
                请选择要添加结课资料的角色：
            </td>
            <td class="border">
                <asp:DropdownList ID="ddlTeacherType" runat="server">
                <asp:ListItem Text="教师" Value="教师"></asp:ListItem>
                <asp:ListItem Text="助教" Value="助教"></asp:ListItem>
                </asp:DropdownList>
            </td>
        </tr>
        <tr>
            <td class="border">
                要上传的资料名称：
            </td>
            <td class="border">
                <asp:TextBox ID="txtArchiveItemName" runat="server" CssClass="required" Width="600px">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="border">
                备注：
            </td>
            <td class="border">
                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="10" Width="600px">
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
        $(document).ready(function () {
            $("#form1").validate();
        });
        
    </script>
    </form>
</body>
</html>

