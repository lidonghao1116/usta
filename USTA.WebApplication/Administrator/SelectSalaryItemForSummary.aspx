<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectSalaryItemForSummary.aspx.cs" Inherits="USTA.WebApplication.Administrator.SelectSalaryItemForSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
 <form id="form1" runat="server">
    <div>
     <asp:HiddenField ID="teacherType" runat="server"/>
        <asp:HiddenField ID="teacherNo" runat="server"/>
        <asp:HiddenField ID="atCourseType" runat="server"/>
    <table  width="100%" class="tableAddStyleNone">
        <tr>
            <td class="border"> 教师：</td>
            <td class="border"> 
                <asp:Literal ID="teacherName" runat="server">
                </asp:Literal>(<asp:Literal ID="teacherType_Literal" runat="server"></asp:Literal>)
            </td>
        </tr>
        <tr>
            <td class="border"> 
                <asp:Literal ID="SelectCourse_Literal" Text="课程：" Visible="false" runat="server"></asp:Literal>
            </td>
            <td class="border">  <asp:DropDownList ID="SelectCourse" OnSelectedIndexChanged="SelectSalaryItem_SelectCourse" runat="server" AutoPostBack="true" Visible="false"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td  colspan="2">请选择预计发放的薪酬项
            </td>
        </tr>
        <tr>
            <td  class="border"colspan="2"> <asp:DataList width="100%" ID="TeacherInSalaryItemList" runat="server" DataKeyField="salaryItemId" RepeatDirection="Horizontal" RepeatColumns="4" >
                <ItemTemplate>
                   
                                <asp:HiddenField ID="salaryItemInId" runat="server" Value='<%#Eval("salaryItemId")%>' />
                                <asp:CheckBox ID="salaryItemInChkBox" Text='<%#Eval("salaryItemName")%>' runat="server" Checked='<%#Eval("isDefaultChecked") %>' />

                            
                </ItemTemplate>
            </asp:DataList></td></tr>
        <tr><td class="border" colspan="2"><asp:Button ID="btn_SubmitSelectSalaryItem" Text="下一步" OnClick="AddSalary_Forwad" runat="server" /></td></tr>
    </table>
    </div>
</form>
</body>
</html>

