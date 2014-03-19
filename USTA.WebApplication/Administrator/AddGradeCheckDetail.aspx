<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddGradeCheckDetail.aspx.cs"
    Inherits="USTA.WebApplication.Administrator.AddGradeCheckDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    <table class="tableAddStyleNone" border="0" style="width: 100%;">
        <tr>
            <td class="border" align="left" style="width: 25%;">
                备注：
            </td>
            <td class="border" align="left">
                <asp:TextBox runat="server" TextMode="MultiLine" Rows="10" ID="remark" Text="无"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="border" align="left" style="width: 25%;">
                是否符合学位申请条件：
            </td>
            <td class="border" align="left">
                <asp:DropDownList ID="ddlIsAccord" runat="server">
                    <asp:ListItem Text="符合" Value="1"></asp:ListItem>
                    <asp:ListItem Text="不符合" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="border" align="left" style="width: 25%;">
            </td>
            <td class="border" align="left">
                <asp:Button ID="btnAdd" runat="server" Text="确定添加" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
