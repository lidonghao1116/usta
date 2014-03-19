<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutputGradeCheckApplyWord.aspx.cs" Inherits="USTA.WebApplication.Administrator.OutputGradeCheckApplyWord1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Literal ID="ltlWord" runat="server"></asp:Literal>
     <asp:Button ID="btnOupputWord" runat="server" Text="导出当前所选课程所有审核通过的重修重考信息Word文档" OnClick="btnOutputWord_Click" />
    </div>
    </form>
</body>
</html>
