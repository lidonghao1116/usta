<%@ Page Language="C#" AutoEventWireup="true" Inherits="Common_Upload" Codebehind="Upload.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        try {
            if (window.parent.document.domain == null) {
                location.href = '/Common/NotifyList.aspx';
            }
            else if (window.parent.uploadInfo.iframeCount == null) {
                location.href = '/Common/NotifyList.aspx';
            }
        }
        catch (e) {
            location.href = '/Common/NotifyList.aspx';
        }

        //检测扩展名
        function checkExtension() {
            var fileName = document.getElementById('upfile').value;
            var strExtensionPosition = fileName.lastIndexOf('.');
            var strExtension = '<%=fileExtension %>';
            var strImageExtension = '<%=imageFileExtension %>';
            //alert(strExtension);
            if ('<%=Request["fileFolderType"] %>' == 9) {
                if (strImageExtension.indexOf(fileName.substring(strExtensionPosition, fileName.length).toLowerCase()) == -1) {

                    alert('上传文件类型有误，系统只允许上传以下图片文件类型：\n<%=imageFileExtension %>\n请重新选择文件进行上传！');
                    window.parent.uploadError('<%=Request["frId"] %>', '<%=Request["fileFolderType"] %>', '上传文件类型有误！请重新选择文件上传');
                    return;
                } 
            }
            
            if (strExtension.indexOf(fileName.substring(strExtensionPosition, fileName.length).toLowerCase()) == -1) {
                alert('上传文件类型有误，系统只允许上传以下文件类型：\n<%=fileExtension %>\n请重新选择文件进行上传！');
                window.parent.uploadError('<%=Request["frId"] %>', '<%=Request["fileFolderType"] %>', '上传文件类型有误！请重新选择文件上传');

                return;
                }
            else {
                window.parent.uploadLoading('<%=Request["frId"] %>', '<%=Request["fileFolderType"] %>');
                document.getElementById('btnSumbit').click();
            }
        }
    </script>
<style type="text/css">
body
{
    margin:0 auto;
    background:#cae8ea;
}
</style>
</head>
<body>
    <form method="post" action="../ashx/ajaxupload.ashx" name="form1" enctype="multipart/form-data"
    target="_self">
    <input type="file" name="upfile" id="upfile" onchange="checkExtension();" />
    <iframe name="hidIframeAjax" id="hidIframeAjax" style="display:none;" width="0" height="0"></iframe>
    <input type="hidden" name="hidFrId" id="hidFrId" value="<%=Request["frId"] %>" />
    <input type="hidden" name="hidHiddenId" id="hidHiddenId" value="<%=this.hiddenId %>" />
    <input type="hidden" name="hidFileFolderType" id="hidFileFolderType" value="<%=Request["fileFolderType"] %>" />
    <input type="submit" name="submit" id="btnSumbit" value="上传" style="display: none;" />
    </form>
</body>
</html>
