<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewReim.aspx.cs" Inherits="USTA.WebApplication.Teacher.AddNewReim" %>

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
        <div><h3>添加报销项</h3></div>
        <div>
            <table>
                <tr>
                    <td>报销项名称：</td>
                    <td>
                        <asp:TextBox ID="ReimName" Width="100" runat="server" CssClass="required"></asp:TextBox>
                        <asp:HiddenField ID="hf_ReimId" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        报销项描述：
                    </td>
                    <td>
                        <asp:TextBox ID="ReimDesc" TextMode="MultiLine" Columns="50" Rows="4" runat="server" CssClass="required"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btn_NewReim" Text="提交" OnClick="NewReim_Click" runat="server"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
