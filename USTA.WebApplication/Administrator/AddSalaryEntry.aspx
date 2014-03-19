<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSalaryEntry.aspx.cs" Inherits="USTA.WebApplication.Administrator.AddSalaryEntry" %>

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
    <h2>薪酬录入</h2>
        <div>
        <table class="tableAddStyleNone" width="100%">
            <tr>
                <td class="border">
                教师：
                </td>
                <td class="border">
                <asp:Literal ID="teacherName" runat="server"></asp:Literal>(<asp:Literal ID="TeacherType_Literal" runat="server"></asp:Literal>)
                <asp:HiddenField ID="teacherNo" runat="server"/>
            <asp:HiddenField ID="courseNo" runat="server"/>
            <asp:HiddenField ID="atCourseType" runat="server"/>
            <asp:HiddenField ID="teacherType" runat="server"/>
                </td>
            </tr>
            <tr id="Course_TR" runat="server" visible="false">
                <td class="border">课程：</td>
                <td class="border">
                    <asp:Literal ID="CourseName_Literal" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td id="CoursePeriod_TR" runat="server" visible="false" class="border">课时：</td>
                <td class="border">
                    <asp:Literal ID="CoursePeriod_Literal" runat="server"></asp:Literal>&nbsp;
                </td>
            </tr>
        </table>
            
            
        </div>
        <h3>
            添加待发薪酬项
        </h3>    
        <div>
            
                <asp:DataList Width="100%" ID="InSalaryItemList" runat="server" DataKeyField="salaryItemId" OnItemDataBound="SalaryEntryItemList_DataBound">
                       <HeaderTemplate>
                       <table width="100%" class="datagrid2">
                <tr>
                    <th width="20%">
                        薪酬项
                    </th>
                    <th width="20%">
                        标准
                    </th>
                    <th width="40%">
                        数量
                    </th>
                    <th width="20%">
                        调整因子
                    </th>
                </tr>
                </table>
                       </HeaderTemplate>
                        <ItemTemplate>
                        <table class="datagrid2" width="100%">
                            <tr>
                                <td width="20%"><%#Eval("salaryItemName") %>
                                <asp:HiddenField ID="InSalaryItemId" Value='<%#Eval("salaryItemId") %>' runat="server"/>
                                <asp:HiddenField ID="InSalaryItemHasTax" Value='<%#Eval("hasTax").ToString().Trim() %>' runat="server"/>
                                </td>
                                <td width="20%">
                                    <asp:TextBox ID="InSalaryStandard" runat="server" Width="60px" CssClass="required number" Text='<%#Eval("salaryItemElement.salaryStandard") %>'></asp:TextBox>
                                    <asp:DropDownList ID="InSalaryItemStandard_DropDownList" runat="server" Visible="false"></asp:DropDownList>
                                </td>
                                <td width="40%">
                                    <asp:TextBox ID="InSalaryUnit" runat="server" Text='<%#Eval("salaryItemElement.times") %>' Width="60px"  CssClass="required number"></asp:TextBox>(<%#Eval("salaryItemUnit") %>)
                                </td>
                                <td width="20%">
                                    <asp:TextBox ID="InSalaryAdjust" runat="server" Text="1" Width="60px" CssClass="required number"></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                    </ItemTemplate>
                </asp:DataList>
        </div>
    </div>
    <div>
        <% if (OutSalaryItemList.Items.Count > 0){ %>
        <h3>
            添加待扣薪酬项
        </h3>
        <div>
             <asp:DataList width="100%" ID="OutSalaryItemList" runat="server" DataKeyField="salaryItemId">
                                    <HeaderTemplate>
                       <table width="100%" class="datagrid2">
                <tr>
                    <th width="20%">
                        薪酬项
                    </th>
                    <th width="20%">
                        标准
                    </th>
                    <th width="40%">
                        数量
                    </th>
                    <th width="20%">
                        调整因子
                    </th>
                </tr>
                </table>
                       </HeaderTemplate>
                <ItemTemplate>
                <table class="datagrid2" width="100%">
                    <tr>
                        <td width="20%"><%#Eval("salaryItemName") %>
                                    <asp:HiddenField ID="OutSalaryItemId" Value='<%#Eval("salaryItemId") %>' runat="server"/>
                                </td>
                                <td width="20%">
                                    <asp:TextBox ID="OutSalaryStandard" runat="server" Width="60px" CssClass="required number"></asp:TextBox>
                                </td>
                                <td width="40%">
                                    <asp:TextBox ID="OutSalaryUnit" runat="server" Text="1" Width="60px"  CssClass="required number"></asp:TextBox>(<%#Eval("salaryItemUnit") %>)
                                </td>
                                <td width="20%">
                                    <asp:TextBox ID="OutSalaryAdjust" runat="server" Text="1" Width="60px" CssClass="required number"></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                    </ItemTemplate>
                    </asp:DataList>
        </div>
        <% }%>
    </div>
   
    <div>
        <div>
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Button ID="btn_AddSalaryValue" Text="下一步" runat="server" OnClick="AddSalaryValue_Forward"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
