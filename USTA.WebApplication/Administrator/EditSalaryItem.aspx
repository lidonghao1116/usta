<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditSalaryItem.aspx.cs" Inherits="USTA.WebApplication.Administrator.EditSalaryItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                    <table class="tableAddStyleNone" width="100%">
                        <tr>
                            <td class="border">薪酬项名称：</td>
                            <td  class="border">
                                <asp:TextBox ID="salaryItemName" runat="server" Width="100" CssClass="required"></asp:TextBox>&nbsp;&nbsp(例如：“课酬”)
                            </td>
                        </tr>
                        <tr>
                            <td  class="border">
                                计量单位：
                            </td>
                            <td  class="border">
                                <asp:TextBox ID="salaryItemUnit" runat="server" Width="100" CssClass="required"></asp:TextBox>&nbsp;&nbsp(例如：课酬的计量单位“课时”)
                            </td>
                        </tr>
                        <tr id="UserFor_TR" runat="server" visible="false">
                            <td  class="border">适用角色：</td>
                            <td  class="border">
                                <asp:RadioButton ID="SalaryItemForCollegeTeacher" GroupName="SalaryFor" Text="院内教师/助教" runat="server" Checked="true"/>
                                <asp:RadioButton ID="SalaryItemForOutTeacher" GroupName="SalaryFor" Text="院外教师" runat="server"/>
                                <asp:RadioButton ID="SalaryItemForOutAssistant" GroupName="SalaryFor" Text="院外助教" runat="server"/>
                            </td>
                        </tr>
                        <tr ID="SalaryVisible_TR" runat="server">
                            <td  class="border">是否展示：</td>
                            <td  class="border">
                                <asp:RadioButton ID="SalaryItemIsVisible" GroupName="SalaryItemVisible" Text="默认展示" runat="server" Checked="true"/>
                                <asp:RadioButton ID="SalaryItemIsHidden" GroupName="SalaryItemVisible" Text="默认隐藏" runat="server"/>
                            </td>
                        </tr>
                        <tr>
                            <td  class="border">&nbsp;</td>
                            <td  class="border">
                                <asp:CheckBox ID="cb_hasTax" Text="含税" runat="server"/>
                                <asp:CheckBox ID="cb_defaultChecked" Text="默认选中" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top"  class="border">薪酬项说明：</td>
                            <td  class="border">
                                <asp:TextBox ID="salaryItemDesc" Rows="6" Columns="40" runat="server" TextMode="multiLine" MaxLength="1000"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="salaryItemDescValidator" ControlToValidate="salaryItemDesc" Text="超过1000字" ValidationExpression="^[\s\S]{0,1000}$" runat="server" />
                            </td >
                        </tr>
                        <tr>
                            <td colspan="2" align="right"  class="border">
                            <asp:HiddenField ID="hf_page" runat="server"/>
                                <asp:Button ID="btnSubmit" Text="添加" runat="server" OnClick="btnSubmit_Click"/>
                            </td>
                        </tr>
                    </table>
    </form>
</body>
</html>
