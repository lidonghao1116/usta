<%@ Page Language="C#" AutoEventWireup="true" Inherits="Common_ViewCourseNotify" Codebehind="ViewCourseNotify.aspx.cs" %>

<%@ Register assembly="AspNetPager" namespace="Wuqi.Webdiyer" tagprefix="webdiyer" %>
        
        <html xmlns="http://www.w3.org/1999/xhtml">
<head id="masterHead" runat="server">
    <title></title>
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
    </head>
    <body>
    <form id="form1" runat="server">
            <asp:DataList ID="dlstcNotify" runat="server" width="100%"><ItemTemplate>
        <table width="100%" class="tableEditStyle beautyTableStyle">
        <tr><td colspan="2" align="center"><h3><%#Eval("courseNotifyInfoTitle")%></h3></td></tr>
        <tr><td colspan="2">发布者: <%#Eval("publishUserNo")%>&nbsp;&nbsp;&nbsp;&nbsp;时间：<%#Eval("updateTime")%>&nbsp;&nbsp;浏览次数：<%#Eval("scanCount")%></td></tr>
        
        <tr><td >相关资源:</td><td>&nbsp;<%#GetURL(Eval("attachmentIds").ToString()) %></td></tr>
        <tr><td colspan="2" style="border-bottom:0px;"><%#Eval("courseNotifyInfoContent")%></td></tr>
        </table></ItemTemplate>
        </asp:DataList>
    </form>
</body>
</html>
