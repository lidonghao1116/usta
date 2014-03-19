<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditEnglishExamSignUpInfoState.aspx.cs" Inherits="USTA.WebApplication.Teacher.EditEnglishExamSignUpInfoState" %>

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
                    <td colspan="2" class="border">
                        以下为学生报名相关状态信息：
                    </td>
                </tr>
    <tr>
        <td width="200px" class="border">                        
                        当前所属考试：
                    </td>
                    <td class="border">
                        <asp:DropDownList ID="ddlEnglishExamNotify" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">
                        缴费状态：
                    </td>
                    <td class="border"><asp:DropDownList ID="ddlIspaid" runat="server">
                            <asp:ListItem Text="已缴费" Value="1"></asp:ListItem>
                            <asp:ListItem Text="未缴费" Value="0"></asp:ListItem>
                </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">
                        缴费状态备注说明：
                    </td>
                    <td class="border">
                        <asp:TextBox ID="txtIsPaidRemark" runat="server" Width="339px" TextMode="MultiLine" Rows="4" Columns="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">
                        准考证状态：
                    </td>
                    <td class="border">
                        <asp:DropDownList ID="ddlExamCertificate" runat="server">
                            <asp:ListItem Text="暂无" Value="暂无"></asp:ListItem>
                            <asp:ListItem Text="待领" Value="待领"></asp:ListItem>
                            <asp:ListItem Text="代领" Value="代领"></asp:ListItem>
                            <asp:ListItem Text="已领" Value="已领"></asp:ListItem>
                </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">
                        准考证状态备注说明：
                    </td>
                    <td class="border">
                        <asp:TextBox ID="txtExamCertificateStateRemark" runat="server" Width="339px" TextMode="MultiLine" Rows="4" Columns="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">
                        成绩单状态：
                    </td>
                    <td class="border">
                       <asp:DropDownList ID="ddlGradeCertificate" runat="server">
                            <asp:ListItem Text="暂无" Value="暂无"></asp:ListItem>
                            <asp:ListItem Text="待领" Value="待领"></asp:ListItem>
                            <asp:ListItem Text="代领" Value="代领"></asp:ListItem>
                            <asp:ListItem Text="已领" Value="已领"></asp:ListItem>
                </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">
                        成绩单备注说明：
                    </td>
                    <td class="border">
                        <asp:TextBox ID="txtGradeCertificateStateRemark" runat="server" Width="339px" TextMode="MultiLine" Rows="4" Columns="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">
                         分数：
                    </td>
                    <td class="border">
                        <asp:TextBox ID="txtGrade" runat="server" Width="339px"></asp:TextBox>
                    </td>
                </tr>
                <tr><td width="200px" class="border">    
                    </td>
                    <td class="border">
                        <asp:Button ID="btnConfirm" runat="server" Text="修改报名相关状态信息" OnClick="btnConfirm_Click" OnClientClick="return confirm('是否修改报名相关状态信息？');" />
                    </td>
                </tr>
    </table>
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
