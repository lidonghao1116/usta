<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeacherSalaryQA.aspx.cs" Inherits="USTA.WebApplication.Administrator.TeacherSalaryQA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
    <script type="text/javascript" src="../javascript/ui/My97DatePicker/WdatePicker.js"></script>
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
        <asp:DataList Width="100%" ID="TeacherSalaryQAList" runat="server" DataKeyField="salaryQaId">
            <HeaderTemplate>
                <table width="100%" class="datagrid2">
                    <tr>
                        <th width="20%">姓名</th>
                        <th width="80%">内容</th>
                    </tr>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table width="100%" class="datagrid2">
                    <tr>
                        <td width="20%">
                            <%#Eval("teacher.teacherName") %><br />
                            <%#Eval("createdTime") %>
                        </td>
                        <td width="80%">
                            <%#Eval("qaContent") %>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>
    </div>
    <div>
        <span>提交疑问&回复</span>
        <table width="80%" class="tableAddStyleNone">
            <tr>
                <td width="20%" class="border">
                    <asp:Literal ID="teacherName" runat="server"></asp:Literal>
                </td>
                <td width="80%"  class="border">
                     <asp:TextBox ID="newTeacherSalaryQA" Rows="3" Columns="50" MaxLength="500" TextMode="MultiLine" runat="server" CssClass="required" ></asp:TextBox>
                     <asp:RegularExpressionValidator ID="TeacherSalaryQAValidator" ControlToValidate="newTeacherSalaryQA" Text="超过500字" ValidationExpression="^[\s\S]{0,500}$" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2"  class="border">
                    <asp:HiddenField ID="hf_SalaryId" runat="server"/>
                    <asp:HiddenField ID="hf_SalaryType" runat="server"/>
                    <asp:Button ID="btn_TeacherSalaryQA" OnClick="TeacherSalaryQA_Click" Text="提交" runat="server"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

