<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditEnglishExamSignUpInfo.aspx.cs" Inherits="USTA.WebApplication.Teacher.EditEnglishExamSignUpInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="masterHead" runat="server">
    <title></title>
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
<script type="text/javascript" src="../javascript/ui/My97DatePicker/WdatePicker.js"></script>
    <!--Validate-->
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'Textarea1', "100%", "260px");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" class="tableAddStyleNone">
    <tr>
        <td width="200px" class="border">                        
                        当前考试类型：
                    </td>
                    <td class="border">
                        <asp:DropDownList ID="ddlEnglishExamNotify" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="border">
                        以下为学生报考基本信息：
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">
                        姓名：
                    </td>
                    <td class="border">
                        <asp:Literal ID="ltlName" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">                        
                        性别：
                    </td>
                    <td class="border">
                        <asp:Literal ID="ltlSex" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        学号：
                    </td>
                    <td class="border"><asp:Literal ID="ltlStudentNo" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        证件类型：
                    </td>
                    <td class="border"><asp:Literal ID="ltlCardType" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        证件号码：
                    </td>
                    <td class="border"><asp:Literal ID="ltlCardNum" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        入学年份：
                    </td>
                    <td class="border"><asp:Literal ID="ltlMatriculationDate" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        专业：
                    </td>
                    <td class="border"><asp:Literal ID="ltlMajor" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        班级：
                    </td>
                    <td class="border"><asp:Literal ID="ltlSchoolClass" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        考试类型：
                    </td>
                    <td class="border">
                        <asp:DropDownList ID="ddlEnglishExamType" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        考试地点：
                    </td>
                    <td class="border">
                        <asp:DropDownList ID="ddlEnglishExamPlace" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr><td width="200px" class="border">    
                    </td>
                    <td class="border">
                        <asp:Button ID="btnConfirm" runat="server" Text="确认报名信息" OnClick="btnConfirm_Click" OnClientClick="if($.trim($('#hidExamType').val())!=$.trim($('#ddlEnglishExamType').val())){return confirm('确认将考试类型更改为'+$.trim($('#ddlEnglishExamType').val())+'吗？\n学生报名考试类型原始数据为：'+$.trim($('#hidExamType').val()));};if($.trim($('#hidExamPlace').val())!=$.trim($('#ddlEnglishExamPlace').val())){return confirm('确认将考试地点更改为'+$.trim($('#ddlEnglishExamPlace').val())+'吗？\n学生报名考试地点原始数据为：'+$.trim($('#hidExamPlace').val()));}" />&nbsp;&nbsp;&nbsp;<input type="button" value="取消" onclick="self.parent.tb_remove();" />                    </td>
                </tr>
    </table><input type="hidden" id="hidExamType" runat="server" /><input type="hidden" id="hidExamPlace" runat="server" />
    <br />
    <br />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#form1").validate();
        });        
    </script>
    </form>
</body>
</html>