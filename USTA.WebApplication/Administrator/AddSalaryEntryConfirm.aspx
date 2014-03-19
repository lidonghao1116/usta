<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSalaryEntryConfirm.aspx.cs" Inherits="USTA.WebApplication.Administrator.AddSalaryEntryConfirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" src="../javascript/ui/My97DatePicker/WdatePicker.js"></script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <h2>月薪酬发放确认</h2>
        <table width="100%" class="tableAddStyleNone">
            <tr>
                <td width="10%" class="border">
                    姓名：
                </td>
                <td colspan="2" class="border">
                    <asp:Literal ID="TeacherName" runat="server"></asp:Literal>
                </td>
            </tr>
            </table>
            <h3>收入项</h3>
            <asp:DataList Width="100%" ID="ShowSalaryInItems" runat="server">
            <HeaderTemplate>
            <table class="datagrid2" width="100%">
            <tr><th width="30%">酬金项</th><th width="50%">详情</th><th width="20%">总额</th></tr>
            </table>
            </HeaderTemplate>
                <ItemTemplate>
                <table class="datagrid2" width="100%">
                <tr>
                    <td width="30%"><%#Eval("salaryItemName") %></td>
                    <td width="50%"><%#Eval("salaryStandard")%> * <%#Eval("times") %>(<%#Eval("itemUnit") %>) * <%#Eval("adjustFactor")%></td>
                    <td width="20%"><%#Eval("itemCost") %></td>
                </tr>
                </table>
                </ItemTemplate>
            </asp:DataList>
            <% if (ShowSalaryOutItems.Items.Count > 0)
               { %>
            <h3>
                    扣除项
             </h3>
            <asp:DataList Width="100%" ID="ShowSalaryOutItems" runat="server">
            <HeaderTemplate>
            <table class="datagrid2" >
            <tr><th width="30%">酬金项</th><th width="50%">详情</th><th width="20%">总额</th></tr>
            </table>
            </HeaderTemplate>
               <ItemTemplate>
               <table class="datagrid2">
               <tr>
                    <td width="30%"><%#Eval("salaryItemName") %></td>
                    <td width="50%"><%#Eval("salaryStandard")%> * <%#Eval("times") %>(<%#Eval("itemUnit") %>) * <%#Eval("adjustFactor")%></td>
                    <td width="20%"><%#Eval("itemCost") %></td>
                </tr>
                </table>
               </ItemTemplate>
            </asp:DataList>
            <%
                } %>
                <h3>
                        税前薪酬：
                   </h3>
            <table class="tableAddStyleNone">
            <tr>
                <td width="30%" class="border">
                     含税部分：
                </td>
                <td colspan="2" class="border">
                    <asp:Literal ID="TeacherSalaryCostWithTax" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td width="20%" class="border">
                     不含税部分：
                </td>
                <td colspan="2" class="border">
                    <asp:Literal ID="TeacherSalaryCostWithoutTax" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td width="20%" class="border">
                    税后总薪酬：
                </td>
                <td colspan="2" class="border">
                    <asp:TextBox ID="TeacherTotalCost" runat="server" CssClass="required number"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td width="20%" class="border">
                    学期
                </td>
                <td colspan="2" class="border">
                    <asp:DropDownList ID="SalaryTermTag" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="20%" class="border">
                    月份：
                </td>
                <td colspan="2" class="border">
                    <input type="text" id="SalaryMonth" runat ="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM',alwaysUseStartDate:true})" class="required"/>
                </td>
            </tr>
            <tr>
                <td width="20%" class="border">
                    备注：
                </td>
                <td colspan="2" class="border">
                    <asp:TextBox ID="SalaryEntryMemo" TextMode="MultiLine" Rows="4" Columns="40" runat="server" MaxLength="1000"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="SalaryEntryMemoValidator" ControlToValidate="SalaryEntryMemo" Text="超过1000字" ValidationExpression="^[\s\S]{0,1000}$" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="3" class="border">
                    <asp:HiddenField ID="TeacherId" runat="server"/>
                    <asp:HiddenField ID="CourseId" runat="server"/>
                    <asp:HiddenField ID="atCourseType" runat="server"/>
                    <asp:HiddenField ID="InSalaryItemValueList" runat="server"/>
                    <asp:HiddenField ID="OutSalaryItemValueList" runat="server"/>
                    <asp:HiddenField ID="teacherType" runat="server"/>
                    <asp:Button ID="btn_AddSalary" runat="server" Text="提交" OnClick="AddSalary_Submit"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
