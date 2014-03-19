<%@ Page Title="" Language="C#"  AutoEventWireup="true" Inherits="Teacher_EditSchoolworkNotify" Codebehind="EditSchoolworkNotify.aspx.cs" %>

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
            initKindEditor(K, 'Textarea1', "100%", "250px");
        });
    </script>
</head><body>
<form id="form1" runat="server">
    
    <table class="tableAddStyle" width="100%">
    <tr><td width="10%">标题：</td><td><asp:TextBox ID="txtTitle" runat="server" CssClass="required"></asp:TextBox></td></tr>
    <tr><td colspan="2">正文：</td></tr><tr>
    <td  colspan="2">
<textarea id="Textarea1" cols="100" rows="8" runat="server" clientidmode="Static"></textarea></td></tr>
     <tr><td>截止时间：</td><td><input type="text" id="datepicker" runat ="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})" class="required" /></td></tr>
    <tr><td>在线提交：</td><td> <asp:DropDownList ID="ddltOnline" runat="server">
            <asp:ListItem Selected="True" Value="true">是</asp:ListItem>
            <asp:ListItem Value="false">否</asp:ListItem>
        </asp:DropDownList></td></tr>
    <tr><td colspan="2">相关资源：</td></tr>
    <tr><td colspan="2"><!--upload start-->
    <input id="Button3" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />&nbsp;&nbsp;<b>上传文件大小不超过</b><%=ConfigurationManager.AppSettings["uploadFileLimit"] %>
    <div id="iframes"><asp:Literal ID="ltlAttachment" runat="server"></asp:Literal></div>
     <!--upload end-->
    <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
        <!--upload end--></td></tr>
        <tr><td style="border-bottom:0px;">&nbsp;</td><td style="border-bottom:0px;"><asp:Button ID="btnEditSchoolworkNotify" runat="server" Text="修改" onclick="btnEditSchoolworkNotify_Click" OnClientClick="return checkKindValue('Textarea1');"/></td></tr>
    </table>
    
    <script type="text/javascript">
        $(document).ready(function() {
            $("#form1").validate();
         uploadInfo.iframeCount=<%=iframeCount %>;
        });
    </script>
    </form>
</body>
</html>

