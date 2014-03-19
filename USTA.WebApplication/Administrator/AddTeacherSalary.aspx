<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTeacherSalary.aspx.cs" Inherits="USTA.WebApplication.Administrator.AddTeacherSalary" %>

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
    <div>
                <h2>教师学期课时及薪酬录入</h2>
                </div>
                <div>
                    <table width="100%" class="tableAddStyleNone" style = "border:1px solid #CAE8EA">
                        <tr>
                            <td width="10%" class="border">教师：</td>
                            <td class="border"><asp:Literal ID="teacherName" runat="server"></asp:Literal>
                                <asp:HiddenField ID="teacherNo" runat="server"/>
                                <asp:HiddenField ID="atCourseType" runat="server"/>
                                <asp:HiddenField ID="teacherType" runat="server"/>
                                (<asp:Literal ID="teacherType_Literal" runat="server"></asp:Literal>)
                            </td>
                        </tr>
                        <tr id="Course_TR" runat="server" visible="false">
                            <td class="border">课程：</td>
                            <td class="border">
                                <asp:Literal ID="CourseName_Literal" runat="server"></asp:Literal>
                                <asp:HiddenField ID="CourseId_hf" runat="server"/>
                            </td>
                        </tr>
                        <tr id="teachPeriod_TR" runat="server" visible="false">
                            <td class="border">理论课时：</td>
                            <td class="border">
                                <asp:TextBox ID="teachPeriod" Width="100" runat="server" CssClass="required  number"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="experPeriod_TR" runat="server" visible="false">
                            <td class="border">实验课时：</td>
                            <td class="border">
                                <asp:TextBox ID="experPeriod" Width="100" runat="server" CssClass="number"></asp:TextBox>
                            </td>
                        </tr>
                       
                        </table>
                        <h3>薪酬预算项：</h3>
                        <asp:DataList Width="100%" ID="TeacherSalary_ItemList" DataKeyField="salaryItemId" runat="server" OnItemDataBound="TeacherSalaryItemList_DataBound">
                        <HeaderTemplate>
                        <table width="100%" class="datagrid2" >
                        <tr>
                            <th width="25%">薪酬项</th>
                            <th width="15%">标准</th>    
                            <th width="30%">数量</th>
                            <th width="15%">总额</th>
                            <th width="15%">发放月数</th>    
                        </tr>
                        </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                        <table width="100%" class="datagrid2" >
                                <tr>
                                    <td width="25%">
                                        <%#Eval("salaryItemName") %>：
                                        <asp:HiddenField ID="salaryItemId" Value='<%#Eval("salaryItemId") %>' runat="server"/>
                                        <asp:HiddenField ID="salaryItemHasTax" Value='<%#Eval("hasTax") %>' runat="server"/>
                                    </td>
                                    <td width="15%">
                                        <asp:TextBox ID="salaryItemStandard" runat="server" Width="40" Text="0" CssClass="required number"></asp:TextBox>
                                        <asp:DropDownList ID="SalaryItemStandard_DropDownList" runat="server" Visible="false"></asp:DropDownList>
                                       </td>
                                       <td width="30%">
                                        <asp:TextBox ID="salaryItemUnit" runat="server" Width="40" Text="1" CssClass="required number"></asp:TextBox>(<asp:Literal ID="Literal1" runat="server" Text='<%#Eval("salaryItemUnit") %>'></asp:Literal>)
                                        </td>
                                        <td width="15%">
                                        <asp:TextBox ID="salaryItemTotal" runat="server" Width="40" Text="0" CssClass="required number"></asp:TextBox>
                                    </td>
                                    <td width="15%">
                                        <asp:TextBox ID="MonthNum" runat="server" Width="40" Text="5" CssClass="required number"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                         <br />
                        <table width="100%" class="tableAddStyleNone" style = "border:1px solid #CAE8EA">
                        <tr>
                            <td  width="10%" class="border">学期：</td>
                            <td  class="border">
                                <asp:DropDownList ID="TeacherSalary_TermTag" runat="server">
                      </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td  class="border">备注：</td>
                            <td  class="border">
                                <asp:TextBox ID="teacherSalary_Memo" TextMode="MultiLine" Rows="3" Columns="40" runat="server" MaxLength="1000"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="teacherSalaryMemoValidator" ControlToValidate="teacherSalary_Memo" Text="超过1000字" ValidationExpression="^[\s\S]{0,1000}$" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right"  class="border">
                                <asp:Button ID="btn_TeacherSalary" Text="添加" runat="server" OnClick="btn_TeacherSalary_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
    </div>
    </form>
</body>
</html>
