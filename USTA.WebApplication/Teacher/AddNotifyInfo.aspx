<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_AddNotifyInfo" Codebehind="AddNotifyInfo.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="masterHead" runat="server">
    <title></title>
 <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
<!--Validate-->

    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>

    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>

    <script type="text/javascript">
        $(document).ready(function() {
        $("#form1").validate();
        });
    </script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'Textarea1', "100%", "200px");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
  <table class="tableAddStylePopup" width="100%">
  <tr><td width="10%" class="border">标题：</td><td class="border"><asp:TextBox ID="txtNotifyTitle" runat="server" CssClass="required" Width="300px"></asp:TextBox></td></tr>
  
  <tr><td colspan="2" class="border">内容：</td></tr>
  <tr><td colspan="2" class="border">
<textarea id="Textarea1" cols="100" rows="8" runat="server" clientidmode="Static"></textarea></td></tr>
  <tr><td class="border" colspan="2">附件：
  <!--upload start-->
           <input id="Button3" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />&nbsp;&nbsp;<b>上传文件大小不超过</b><%=ConfigurationManager.AppSettings["uploadFileLimit"] %>
    <div id="iframes"></div>
    <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server"  Value="" />
    <!--upload end--> 
    </td></tr>
    <tr><td style="border-bottom:0px;">&nbsp;</td><td style="border-bottom:0px;">
    
        <asp:Button ID="btnNotifyAdd" runat="server" Text="提交" 
            onclick="btnNotifyAdd_Click"  OnClientClick="return checkKindValue('Textarea1');"/></td></tr>
    </table>

    </div>
    </form>
</body>
</html>
