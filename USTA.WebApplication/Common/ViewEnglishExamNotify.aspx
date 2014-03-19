<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ViewEnglishExamNotify.aspx.cs" Inherits="USTA.WebApplication.Common.ViewEnglishExamNotify" %>

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
            <td class="border">
                标题：
            </td>
            <td class="border">
                <asp:Literal ID="ltlTitle" runat="server">
                </asp:Literal>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="border">
                内容：
            </td>
        </tr>
        <tr>
            <td colspan="2" class="border">
                <asp:Literal ID="ltlContent" runat="server">
                </asp:Literal>
            </td>
        </tr><tr><td class="border">截止日期：</td><td class="border">
                <asp:Literal ID="ltlDeadLineTime" runat="server">
                </asp:Literal></td></tr>  
        <tr>
            <td colspan="2" class="border">
                <asp:Literal ID="ltlAttachment" runat="server">
                </asp:Literal>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <script type="text/javascript">
        $(document).ready(function() {
            $("#form1").validate();
            
         uploadInfo.iframeCount=<%=iframeCount %>;
        });
        
    </script>
    </form>
</body>
</html>