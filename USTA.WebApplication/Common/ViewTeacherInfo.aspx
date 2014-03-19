<%@ Page Language="C#" AutoEventWireup="true" Inherits="Common_ViewTeacherInfo" Codebehind="ViewTeacherInfo.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="masterHead">
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
    <title></title>
</head>
<body>
     <form id="form1" runat="server">
      <table align="center" width="100%" class="beautyTableStyle">    
      <tr>
        <td><asp:Label ID="lblNames" runat="server" Font-Size="9pt" Text="教师姓名：" Width="73px"></asp:Label></td>
        <td><asp:Label ID="lblName" runat="server" Font-Size="9pt"  Width="73px"></asp:Label></td>
      </tr>
      <tr>
        <td><asp:Label ID="lblEmails" runat="server" Font-Size="9pt" Text="E-mail：" Width="78px"></asp:Label></td>
        <td><asp:Label ID="lblEmail" runat="server" Font-Size="9pt"  Width="78px"></asp:Label></td>
      </tr>
      <tr>
        <td><asp:Label ID="lblOffices" runat="server" Font-Size="9pt" Text="办公室：" Width="78px"></asp:Label></td>
        <td><asp:Label ID="lblOffice" runat="server" Font-Size="9pt"  Width="78px"></asp:Label></td>
      </tr>
      <tr>
        <td ><asp:Label ID="lblRemarks" runat="server" Font-Size="9pt" Text="备注：" Width="75px"></asp:Label></td>
        <td ><asp:Label ID="lblRemark" runat="server" Font-Size="9pt"  Width="300px"></asp:Label></td>
        
      </tr>
      <tr><td style="border-bottom:0px;">当前学期任课</td><td style="border-bottom:0px;">
      <asp:DataList ID="courses" runat="server" Width="100%">
      <ItemTemplate>
      <a href="/Common/CInfoCourseIntro.aspx?courseNo=<%#Eval("courseNo").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("ClassID").ToString().Trim()) %>&termtag=<%#Eval("termTag") %>" target="_top"><%#Eval("courseName").ToString().Trim()%>(<%#Eval("ClassID") %>)[进入课程]</a><%#Eval("atCourseType").ToString()=="1"?"":"(助教)" %><br />
      </ItemTemplate>
      <ItemStyle BorderWidth="0" />
      </asp:DataList>
      </td></tr>
     </table>
    </form>
</body>
</html>
