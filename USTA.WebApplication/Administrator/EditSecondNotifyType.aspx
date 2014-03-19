<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditSecondNotifyType.aspx.cs" Inherits="USTA.WebApplication.Administrator.EditSecondNotifyType" %>

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
    <div>
    <table align="center" width="100%" class="tableAddStyle">
    
                    <tr>
                        <td>
                            所属一级分类：
                        <asp:DropDownList ID="ddlNotifyType" runat="server">
                            
                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            类别名称：<asp:TextBox ID="txtTypeName" runat="server" Width="200px" CssClass="required"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            显示顺序：<asp:TextBox ID="txtSequence" runat="server" Width="30px" CssClass="required number"></asp:TextBox>(输入整数)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Width="78px" Text="添加" OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                </table>
    </div>
    </form>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $("#form1").validate();
        });        </script>
</body>
</html>