<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditGradeCheckDetail.aspx.cs" Inherits="USTA.WebApplication.Administrator.EditGradeCheckDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>

      <table class="tableAddStyleNone" width="100%"><tr><td class="border">备注：</td></tr><tr><td class="border"><asp:TextBox runat="server" TextMode="MultiLine" Rows="6" Width="80%" ID="remark"></asp:TextBox>
      </td></tr><tr><td class="border">
      <asp:DropDownList ID="ddlIsAccord" runat="server">
                <asp:ListItem Text="符合" Value="1"></asp:ListItem>
                <asp:ListItem Text="不符合" Value="0"></asp:ListItem>
                </asp:DropDownList>
      </td></tr>
      <tr><td class="border"><asp:Button ID="btnUpdate" runat="server" Text="确定修改" OnClick="btnUpdate_Click" /></td></tr>
      </table>
    </form>
</body>
</html>
