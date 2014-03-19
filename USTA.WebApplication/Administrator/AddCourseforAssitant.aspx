<%@ Page Language="C#" AutoEventWireup="true" Inherits="Administrator_AddCourseforAssitant" Codebehind="AddCourseforAssitant.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="masterHead">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:DataList ID="dlstCourses" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" Width="100%"  CssClass="beautyTableStyle">
    <ItemTemplate>
 <img src="../images/BULLET.GIF" align="middle" /><a href="AddCourseforAssitant.aspx?assistantNo=<%=assistantNo%>&courseNo=<%#Eval("courseNo")%>&classID=<%#Server.UrlEncode(Eval("classID").ToString().Trim()) %>&termTag=<%#Eval("termTag").ToString().Trim() %>"><%#Eval("courseName") %>(<%#Eval("classID").ToString().Trim()%>)</a>
    <br />
    </ItemTemplate><FooterTemplate><%=(this.dlstCourses.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" />
    </asp:DataList>
    </div>
    </form>
</body>
</html>
