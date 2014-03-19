<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewTeacherSalarySummaryDetail.aspx.cs" Inherits="USTA.WebApplication.Teacher.ViewTeacherSalarySummaryDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">    
    <div>
        <table width="100%" class="tableAddStyleNone">
            <tr>
                <td width="30%" class="border">
                    姓名：
                </td>
                <td colspan="2" class="border">
                    <asp:Literal ID="TeacherName" runat="server"></asp:Literal>(<asp:Literal ID="TeacherPosition" runat="server"></asp:Literal>)
                    <asp:HiddenField ID="TeacherSalaryId" runat="server"/>
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
            <tr id="TeacherCoursePeriod_TR" runat="server" visible="false">
                <td width="20%" class="border">
                    课时：
                </td>
                <td colspan="2" class="border">
                    <asp:Literal ID="TeacherCoursePeriod" runat="server"></asp:Literal>
                </td>
            </tr>
            </table>
            <h3> 预算薪酬项</h3>
            
            <asp:DataList Width="80%" ID="ShowSalarySummaryItems" runat="server">
            <HeaderTemplate>
            <table width="100%" class="datagrid2">
            <tr>
                <th width="30%">薪酬项</th>
                <th width="20%">薪酬标准</th>
                <th width="15%">数量</th>
                <th width="15%">总额</th>
                <th width="20%">发放月数</th>
            </tr>
            </table>
            </HeaderTemplate>
                <ItemTemplate>
                <table width="100%" class="datagrid2">
                <tr>
                    <td width="30%"><%#Eval("salaryItemName") %>：</td>
                    <td width="20%"><%#Eval("salaryStandard")%></td>
                    <td width="15%"><%#Eval("times") %>(<%#Eval("itemUnit")%>)</td>
                    <td width="15%"> <%#Eval("itemCost")%></td>
                    <td width="20%"><%#Eval("MonthNum")%> </td>
                </tr>
                </table>
                </ItemTemplate>
            </asp:DataList>
                <table width="100%" class="tableAddStyleNone">
                
            <tr>
                <td width="20%" class="border">
                    预算总薪酬：
                </td>
                <td colspan="" class="border">
                    <asp:Literal ID="TeacherTotalSummaryCost" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td width="10%" class="border">
                    备注：
                </td>
                <td colspan="2" class="border">
                    <asp:Literal ID="SalaryEntryMemo" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="border">
                    <asp:Button ID="btn_TeacherSalaryConfirm" Text="确认" OnClick="TeacherSalaryConfirm_Click" Visible="false" runat="server"/>
                    <asp:Button ID="btn_TeacherSalaryQA" Text="有疑问反馈" OnClick="TeacherSalaryQA_Click" Visible="false" runat="server"/>
                    <asp:HiddenField ID="hf_salaryId" runat="server"/>
                    <asp:HiddenField ID="hf_salaryType" runat="server" Value="1"/>
                    
                </td>
            </tr>
        </table>
        </div>
    </form>
</body>
</html>
