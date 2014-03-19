<%@ Page Language="C#" AutoEventWireup="true" Inherits="Student_EditExperiment"  Codebehind="EditExperiment.aspx.cs" %>

 <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="masterHead" runat="server">
    <title></title>
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
 <!--Validate-->

   
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:HiddenField ID="hidexperimentId" runat="server" />
        <br />
        <br />
        选择要更改的上传的实验报告文件：<br />
        <br />
        <!--upload start-->
    <div id="iframes"></div>
        <script type="text/javascript">addIframe(<%=fileFolderType %>);</script>
    <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
 
    
    <!--upload end-->
        <br />
        <br />
        <asp:Button ID="commit" runat="server" Text="修改" onclick="commit_Click" />
    
    </div>
    </form>
</body>
</html>
