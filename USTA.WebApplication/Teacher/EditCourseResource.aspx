<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_EditCourseResource" Codebehind="EditCourseResource.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="masterHead" runat="server">
    <title></title>
 <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>

        <!--Validate-->

    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>

    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>

</head>
<body>

    <form id="form1" runat="server" method="post" enctype="multipart/form-data">
    <div>
    <table width="100%" class="tableEditStyle beautyTableStyle"><tr><td>标题：</td><td><asp:TextBox ID="txtTitle" runat="server" CssClass="required"></asp:TextBox></td></tr>
        <tr><td colspan="2"> <input id="Button3" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />&nbsp;&nbsp;<b>上传文件大小不超过</b><%=ConfigurationManager.AppSettings["uploadFileLimit"] %>
    <div id="iframes"><asp:Literal ID="ltlAttachment" runat="server"></asp:Literal></div>
            <!--upload end-->
    <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server"  Value="" />
        <!--upload end--></td></tr>
        <tr><td style="border-bottom:0px;"><asp:Button ID="Button1" runat="server" Text="修改" OnClick="Button1_Click" 
            style="width: 40px" /></td><td style="border-bottom:0px;">&nbsp;</td></tr>
        </table>
        <br />

        <!--upload start-->
   
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
