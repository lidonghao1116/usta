<%@ Page Language="C#" AutoEventWireup="true" Inherits="bbs_UploadAvatar" Codebehind="UploadAvatar.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="masterHead">
    <title></title>
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <!--upload start-->
    请选择要替换的头像：<div id="iframes"></div>
        <script type="text/javascript">addIframe(<%=fileFolderType %>);</script>
    <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
        <asp:Button ID="Button1" runat="server" Text="确定更换头像" onclick="Button1_Click" />
 
    
    <!--upload end-->
    </div>
    </form>
</body>
</html>
