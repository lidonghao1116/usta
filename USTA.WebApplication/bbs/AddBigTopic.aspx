<%@ Page Language="C#" AutoEventWireup="true" Inherits="bbs_AddBigTopic" Codebehind="AddBigTopic.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="masterHead">
    <title></title>
     <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
    
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'Textarea1', "100%", "200px");
        });
    </script>
    
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    
</head>
<body>
    <form id="form1" runat="server">
    <p></p>
            <table class="tableAddStyle" style="width:100%;">
                <tr>
                    <td style="width: 10%"><a name="reply"></a>
                        使用标题:
                    </td>
                    <td style="width: 90%">
                        <asp:TextBox ID="txtTilte" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="2">
                    内容：
                    </td>

                    </tr>
                <tr>
                    <td colspan="2">
               <textarea id="Textarea1" cols="100" rows="8" runat="server" clientidmode="Static"></textarea>
                         
                    </td>
                </tr>
                <tr>
                    <td>
                        附件:
                    </td>
                    <td style="width: 90%">
                         
                        <input id="Button3" class="footoperation" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />
                        <div id="iframes" class="bbsupLoadSytle" style="width:100%">
                        </div>
                        <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
                        
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td><asp:Button ID="btnCommit" CssClass="footoperation" runat="server" Text="提交" OnClick="btnCommit_Click" />
                         
                    </td>
                </tr>
            </table>
    </form>
</body>
</html>
