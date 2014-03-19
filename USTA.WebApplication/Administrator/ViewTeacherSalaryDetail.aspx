<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewTeacherSalaryDetail.aspx.cs" Inherits="USTA.WebApplication.Administrator.ViewTeacherSalaryDetail" %>

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
        <table width="100%" class="tableAddStyleNone" >
            <tr>
                <td width="30%" class="border">
                    姓名：
                </td>
                <td colspan="2" class="border">
                    <asp:Literal ID="TeacherName" runat="server"></asp:Literal>(<asp:Literal ID="TeacherPosition" runat="server"></asp:Literal>)
                    <asp:HiddenField ID="SalaryEntryId" runat="server"/>
                </td>
            </tr>
            <tr>
                <td width="20%" class="border">
                    学期：
                </td>
                <td colspan="2" class="border">
                    <asp:Literal ID="TermTag" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="TeacherCourse_TR" runat="server" visible="false">
                <td width="20%" class="border">
                    课程：
                </td>
                <td colspan="2" class="border">
                    <asp:Literal ID="TeacherCourse" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td width="20%" class="border">
                    月份：
                </td>
                <td colspan="2" class="border">
                    <asp:Literal ID="SalaryMonth" runat="server"></asp:Literal>
                    <input type="text" id="Input_SalaryMonth" runat ="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM',alwaysUseStartDate:true})" class="required" visible="false"/>
                </td>
            </tr>
            </table>
            <h2>收入项</h2>
            <asp:DataList Width="100%" ID="ShowSalaryInItems" runat="server">
            <HeaderTemplate>
            <table class="datagrid2" width="100%">
            <tr><th width="30%">薪酬项</th><th width="50%">详情</th><th width="20%">总和</th></tr></table>
            </HeaderTemplate>
           
                <ItemTemplate>
                <table class="datagrid2">
                <tr>
                    <td width="30%"><%#Eval("salaryItemName") %>
                        <%#bool.Parse(Eval("hasTax").ToString().Trim()) ? "(含税)" : "" %>
                    </td>
                    <td width="50%"><%#Eval("salaryStandard")%> * <%#Eval("times") %>(<%#Eval("itemUnit") %>) * <%#Eval("adjustFactor")%></td>
                    <td width="20%"><%#Eval("itemCost") %></td>
                </tr>
                </table>
                </ItemTemplate>
            </asp:DataList>
            <% if (ShowSalaryOutItems.Items.Count > 0)
               { %>
            <h3>扣除项</h3>
            <asp:DataList Width="100%" ID="ShowSalaryOutItems" runat="server">
            <HeaderTemplate>
            <table class="datagrid2" width="100%">
            <tr><th width="30%">薪酬项</th><th width="50%">详情</th><th width="20%">总和</th></tr></table>
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
            <%
                } %>
                
      
              <h3> 税前薪酬：</h3>    
             <table width="100%" class="tableAddStyleNone">
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
                <td colspan="" class="border">
                    <asp:Literal ID="TeacherTotalSalaryCost" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td width="10%" class="border" valign="top">
                    备注：
                </td>
                <td colspan="2" class="border">
                    <asp:Literal ID="SalaryEntryMemo" runat="server"></asp:Literal>
                    <asp:TextBox ID="TB_SalaryEntryMemo" runat="server" Visible="false" TextMode="MultiLine" Columns="50" Rows="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="border">
                    <asp:Button ID="btn_TeacherSalaryPay" Text="发放" OnClick="TeacherSalaryPay_Click" Visible="false" runat="server"/>
                    <asp:Button ID="btn_TeacherSalaryConfirm" Text="确认" OnClick="TeacherSalaryConfirm_Click" Visible="false" runat="server"/>
                    <asp:Button ID="btn_TeacherSalaryQA" Text="有疑问反馈" OnClick="TeacherSalaryQA_Click" Visible="false" runat="server"/>
                    <asp:HiddenField ID="hf_salaryId" runat="server"/>
                    <asp:HiddenField ID="hf_salaryType" runat="server" Value="2"/>
                    
                </td>
            </tr>
        </table>
        </div>
    </form>
</body>
</html>
