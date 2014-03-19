<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_AddExperimentResource" Codebehind="AddExperimentResource.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="masterHead" runat="server">
    <title></title>
 <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
<script type="text/javascript" src="../javascript/ui/My97DatePicker/WdatePicker.js"></script>
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
            initKindEditor(K, 'Textarea1', "100%", "260px");
        });
    </script>
</head>
<body>

    <form id="form1" runat="server">
    <div>
    <table width="100%" class="tableAddStylePopup">
    <tr><td width="10%" class="border">标题：</td><td class="border"><asp:TextBox ID="txtTitle" runat="server" CssClass="required" Width="300px"></asp:TextBox></td></tr>
    <tr><td colspan="2" class="border">正文</td></tr>
    <tr><td colspan="2">
<textarea id="Textarea1" cols="100" rows="8" runat="server" clientidmode="Static"></textarea></td></tr>
    <tr><td class="border">截止时间：</td><td class="border"><input type="text" id="datepicker" runat ="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})" class="required"/></td></tr>    
 
    <tr><td colspan="2" class="border">相关资源：
    <!--upload start-->
    <input id="Button3" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />&nbsp;&nbsp;<b>上传文件大小不超过</b><%=ConfigurationManager.AppSettings["uploadFileLimit"] %>
    <div id="iframes"></div>
    <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
    <!--upload end-->
    <br />
    </td></tr>
    <tr><td style="border-bottom:0px;" class="border">&nbsp;</td><td style="border-bottom:0px;" class="border"><asp:Button ID="btnAddExperimentResource" runat="server" Text="确定" 
            onclick="btnAddExperimentResource_Click" OnClientClick="return checkKindValue('Textarea1');" /></td></tr>
    </table>
        
        
        
    
    </div>
    </form>
</body>
</html>
