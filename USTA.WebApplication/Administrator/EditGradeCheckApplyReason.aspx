<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditGradeCheckApplyReason.aspx.cs" Inherits="USTA.WebApplication.Administrator.EditGradeCheckApplyReason" %>

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
                重修重考原因标题：
            </td>
            <td class="border">
                <asp:TextBox ID="txtGradeCheckApplyReasonTitle" runat="server" CssClass="required" Width="600px">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="border">
                备注：
            </td>
            <td class="border">
                <asp:TextBox ID="txtGradeCheckApplyReasonRemark" runat="server" Width="600px" TextMode="MultiLine" Rows="10">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="border">
            </td>
            <td class="border">
                <asp:Button ID="btnAdd" runat="server" Text="确定修改" OnClick="btnUpdate_Click" />
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
