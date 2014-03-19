<%@ Page Language="C#" AutoEventWireup="true" Inherits="Administrator_ViewArchives" Codebehind="ViewArchives.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="masterHead" runat="server">
    <title></title>
     <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" class="tableAddStyle" align="center">
        <tr><td width="15%">课程编号：</td><td><%=course.courseNo%></td></tr>
        <tr><td width="15%">课程名称：</td><td><%=course.courseName%></td></tr>
        <tr><td width="15%">期末归档资料列表：</td><td style="border-bottom:0px;"><asp:Literal ID="ltlAttachments" runat="server"></asp:Literal><asp:PlaceHolder ID="phUpload" runat="server"></asp:PlaceHolder></td></tr>
    </table>
    </form>
</body>
</html>
