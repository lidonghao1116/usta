<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddGameCategory.aspx.cs" Inherits="USTA.WebApplication.Administrator.AddGameCategory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
    <script type="text/javascript" src="../javascript/ui/My97DatePicker/WdatePicker.js"></script>
    <!--Validate-->
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#form1").validate();
        });
    </script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'txttGameContent', "100%", "200px");
        }); 
        
        function checkNotifyInfo() {
            var startTime = Date.parse($('#startTime').val().replace(/-/g, "/"));
            var endTime = Date.parse($('#endTime').val().replace(/-/g, "/"));
            if (startTime > endTime) {
                alert("开始时间不能晚于截止时间，请修改:)");
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
   
            <table class="tableAddStyle" width="100%">
            <tr>
                    <td width="15%">报名开始时间：
                    </td>
                    <td>
                       <input type="text" id="startTime" runat="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})"
                class="required" clientidmode="Static" />
                    </td>
                </tr>
                <tr>
                    <td width="15%">报名截止时间：
                    </td>
                    <td>
                        <input type="text" id="endTime" runat="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})"
                class="required" clientidmode="Static" />
                    </td>
                </tr>
                <tr>
                    <td width="15%">通知标题：
                    </td>
                    <td>
                        <asp:TextBox ID="txtGameTitle" runat="server" Width="600px" ClientIDMode="Static"
                CssClass="required"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        通知内容：
                    </td>
                    <td>
                        <asp:TextBox ID="txttGameContent" runat="server" TextMode="MultiLine" ClientIDMode="Static"
                CssClass="required"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        附件：
                    </td>
                    <td>
                        <span id="spanAttachment" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                    </td>
                    <td>
                        <!--upload start-->
                        <input id="Button4" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />&nbsp;&nbsp;<b>上传文件大小不超过</b><%=ConfigurationManager.AppSettings["uploadFileLimit"] %>
                        <div id="iframes">
                            <%-- <div id="iframes1">
                                <div id="upLoading1">
                                </div>
                                <div id="upSuccess1">
                                </div>
                                <div id="upError1">
                                </div>
                                <iframe id="frUpload1" scrolling="no" src="../Common/Upload.aspx?frId=1&fileFolderType=<%=fileFolderType %>"
                                    height="30px" style="width: 799px" frameborder="0"></iframe>
                            </div>--%>
                        </div>
                        <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
                        <!--upload end-->
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                    </td>
                    <td>
                        <asp:Button ID="Button2" runat="server" Text="提交" OnClick="Button2_Click" OnClientClick="checkNotifyInfo();" />
                    </td>
                </tr>
            </table>
    </form>
</body>
</html>
