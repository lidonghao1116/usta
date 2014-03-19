<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditEnglishExamSignUpInfo.aspx.cs" Inherits="USTA.WebApplication.Student.EditEnglishExamSignUpInfo" %>

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
                        以下为您的报考基本信息，请确认（标记为<span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>的信息不可修改，若有问题，请和学工部老师联系修改）：
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">
                        姓名：
                    </td>
                    <td class="border">
                        <asp:Literal ID="ltlName" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">                        
                        性别：
                    </td>
                    <td class="border">
                        <asp:Literal ID="ltlSex" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        学号：
                    </td>
                    <td class="border"><asp:Literal ID="ltlStudentNo" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        证件类型：
                    </td>
                    <td class="border"><asp:Literal ID="ltlCardType" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        证件号码：
                    </td>
                    <td class="border"><asp:Literal ID="ltlCardNum" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        入学年份：
                    </td>
                    <td class="border"><asp:Literal ID="ltlMatriculationDate" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        专业：
                    </td>
                    <td class="border"><asp:Literal ID="ltlMajor" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        班级：
                    </td>
                    <td class="border"><asp:Literal ID="ltlSchoolClass" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
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
                        <asp:Button ID="btnConfirm" runat="server" Text="已经确认报考信息并修改报名信息" OnClick="btnConfirm_Click" OnClientClick="if(!confirm('是否确认报考信息（包括基本信息、考试类型、考试地点等）并修改报名信息？')){return false;}" />
                    </td>
                </tr>
    </table>
    <br />
    <br />
    <script type="text/javascript">
        $(document).ready(function() {
            $("#form1").validate();
        });
        
    </script>
    </form>
</body>
</html>