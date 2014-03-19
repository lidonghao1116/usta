<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectSalaryItem.aspx.cs" Inherits="USTA.WebApplication.Administrator.WebForm2" %>

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
    <div>
      <asp:HiddenField ID="teacherNo" runat="server"/>
        <asp:HiddenField ID="teacherType" runat="server" />
        <asp:HiddenField ID="atCourseType" runat="server"/><br />
        <h2>薪酬项选择</h2>
        <table width="100%" class="tableAddStyleNone">
            <tr>
                <td class="border" width="20%"> 教师：</td>
                <td class="border"> 
                    <asp:Literal ID="teacherName" runat="server"></asp:Literal>(<asp:Literal ID="TeacherType_Literal" runat="server"></asp:Literal>)
                </td>
            </tr>
            <tr>
                <td class="border"> 
                    <asp:Literal ID="SelectCourse_Literal" Text="课程：" runat="server" Visible="false"></asp:Literal>
                </td>
                <td class="border">
                    <asp:DropDownList ID="SelectCourse" OnSelectedIndexChanged="SelectSalaryItem_SelectCourse" runat="server" AutoPostBack="true" Visible="false"></asp:DropDownList>
                </td>
             </tr>
   <tr><td  colspan="2"><h4>选择薪酬的待发项</h4></td>
   </tr><tr>
   <td class="border"  colspan="2">
   <asp:DataList width="100%" ID="TeacherInSalaryItemList" runat="server" DataKeyField="salaryItemId" RepeatDirection="Horizontal" RepeatColumns="4" >
                <ItemTemplate>
                   
                                <asp:HiddenField ID="salaryItemInId" runat="server" Value='<%#Eval("salaryItemId")%>' />
                                <asp:CheckBox ID="salaryItemInChkBox" Text='<%#Eval("salaryItemName")%>' runat="server" Checked='<%#Eval("isDefaultChecked") %>' />

                           
                </ItemTemplate>
            </asp:DataList></td></tr>

            <tr><td  colspan="2"> <h4>选择薪酬的扣除项</h4></td></tr>
            <tr><td class="border" colspan="2"> 
            <asp:DataList width="100%" ID="TeacherOutSalaryItemList" runat="server" DataKeyField="salaryItemId" RepeatDirection="Horizontal" RepeatColumns="4" >
                <ItemTemplate>
                    
                                <asp:HiddenField ID="salaryItemOutId" runat="server" Value='<%#Eval("salaryItemId")%>' />
                            
                                <asp:CheckBox ID="salaryItemOutChkBox" Text='<%#Eval("salaryItemName")%>' runat="server" />

                            
                </ItemTemplate>
            </asp:DataList></td></tr>
            
   </table>
      <table width="100%" class="tableAddStyleNone">
       <tr>
                <td align="center" class="border" colspan="2">
                    <asp:Button ID="Button1" Text="下一步" OnClick="AddSalary_Forwad" runat="server" />
                </td>
            </tr>
      </table>
       
       
    </div>
    
</form>
</body>
</html>
