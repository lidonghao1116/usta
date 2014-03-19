<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddArchivesForTeacherOrAssistant.aspx.cs" Inherits="USTA.WebApplication.Administrator.AddArchivesForTeacherOrAssistant" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
   <div id="archivesItemsUploadFiles">
            <!--upload start-->
            <asp:PlaceHolder ID="phUpload" runat="server"></asp:PlaceHolder>
            <!--upload end-->
                <br />
            <asp:Button ID="btnUpload" runat="server" Text="提交" OnClick="btnUpload_Click" OnClientClick="if(!checkIsUpload()){alert('无法提交，可能的原因为：\n您未上传任何文件。\n请上传文件后再提交:)');return false;}"  />
     </div>
    </form>
</body>
</html>
