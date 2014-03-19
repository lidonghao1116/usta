<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewGradeCheckNotify.aspx.cs"
    Inherits="USTA.WebApplication.Common.ViewGradeCheckNotify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table class="tableAddStyle" width="100%">
        <tr>
            <td align="center">
                <asp:Literal ID="ltlNotifyTitle" runat="server"></asp:Literal>
            </td></tr>
            <tr>
            <td>
                <asp:Literal ID="ltlNotifyContent" runat="server"></asp:Literal>
                                <br />
                                <br />
                                <%=GetURL(attachmentIds)%>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
