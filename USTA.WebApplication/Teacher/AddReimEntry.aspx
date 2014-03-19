<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddReimEntry.aspx.cs" Inherits="USTA.WebApplication.Teacher.AddReimEntry" %>

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
    <div><h3>添加报销记录</h3></div>
    <div>
        <table class="tableAddStyleNone">
            <tr>
                <td class="border">
                    项目名称：
                </td>
                <td class="border">
                    <asp:Literal ID="literal_ProjectName" runat="server"></asp:Literal>
                    <asp:HiddenField ID="hf_ProjectId" runat="server"/>
                </td>
            </tr>
            <tr>
                <td class="border">
                    选择报销项：
                </td>
                <td class="border">
                    <asp:DropDownList ID="ddl_ReimList" runat="server" OnSelectedIndexChanged="ReimListChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">-请选择-</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="border">
                    报销金额：
                </td>
                <td class="border">
                    <asp:TextBox ID="tb_ReimValue" runat="server" Width="100" CssClass="required number"></asp:TextBox>
                    <div id="reimRuleDiv" runat="server" visible="false">
                        此报销项单次报销金额不超过<asp:Literal ID="literal_ReimValue" runat="server"></asp:Literal>元；<br />
                        此报销项总报销金额不超过<asp:Literal ID="literal_MaxReimValue" runat="server"></asp:Literal>元
                    </div>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    备注：
                </td>
                <td>
                    <asp:TextBox Columns="40" Rows="4" runat="server" TextMode="MultiLine" ID="ReimEntryMemo" CssClass="required"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button Text="提交" runat="server" OnClick="ReimEntry_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </div>
    </form>
</body>
</html>
