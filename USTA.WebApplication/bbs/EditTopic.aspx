<%@ Page Language="C#" AutoEventWireup="true" Inherits="bbs_EditTopic" Codebehind="EditTopic.aspx.cs" %>
<%@ Register assembly="FredCK.FCKeditorV2" namespace="FredCK.FCKeditorV2" tagprefix="FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="masterHead">
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
            initKindEditor(K, 'Textarea1', "100%", "200px");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="tableAddStyle" style="width:100%;">
            <tr>
                <td>
                    话题：<asp:TextBox ID="txtTitle" runat="server" CssClass="required"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td> <textarea id="Textarea1" cols="100" rows="8" runat="server" clientidmode="Static"></textarea>
                    </td>
               
            </tr>
            <tr>
                <td>
                    附件：
                     <!--upload start-->
    <input id="Button3" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />&nbsp;&nbsp;<b>上传文件大小不超过</b><%=ConfigurationManager.AppSettings["uploadFileLimit"] %>

    <div id="iframes"><asp:Literal ID="ltlAttachment" runat="server"></asp:Literal></div><!--upload end-->
    <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
    <!--upload end--> </td>
                
            </tr>
            <tr><td><asp:Button  runat="server" ID="btnSubmit" text="提交" 
                     OnClick="btnSubmit_Click"  OnClientClick="return checkKindValue('Textarea1');" /></td></tr>
        </table>
    
    </div>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#form1").validate();
            uploadInfo.iframeCount=<%=iframeCount %>;
        });
    </script>
    </form>
</body>
</html>
