<%@ Page Language="C#" AutoEventWireup="true" Inherits="Common_Viewfeedbackinfo" Codebehind="Viewfeedbackinfo.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="masterHead">
    <title></title>
      <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    
    <asp:DataList ID="dlstfeedback" runat="server" width="100%">
    <ItemTemplate>
    <table class="tableAddStyle beautyTableStyle" align="center" width="100%">
       
     <tr>
         <td width="20%" > 标题：</td>
         <td> <%#Eval("feedBackTitle").ToString().Trim()%>                      
      </td>
    </tr>
    <tr>
         <td > 内容：</td>
         <td > <%#Eval("feedBackContent").ToString().Trim()%>  </td>
    </tr>
    <tr><td>是否被阅读：</td><td><%#(Eval("isRead").ToString().ToLower().Equals("true"))?"已读":"未读"%></td>         
      </tr>
      <tr><td>反馈回复：</td><td><%#Eval("backInfo").ToString().Trim()%></td></tr>
      <tr><td style="border-bottom:0px;">反馈回复时间：</td><td style="border-bottom:0px;"><%#Eval("backTime").ToString().Trim()%></td></tr>
    </table></ItemTemplate>
    </asp:DataList>
    <div style="text-align:center;"><input id="Button1" type="button" value="关闭" onclick="self.parent.tb_remove();window.parent.location.reload(true);" /></div>
    </form>
</body>
</html>
