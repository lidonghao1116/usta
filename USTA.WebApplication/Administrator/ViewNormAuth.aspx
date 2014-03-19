<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewNormAuth.aspx.cs" Inherits="USTA.WebApplication.Administrator.ViewNormAuth" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:DataList ID="dsltAuthTeacher" runat="server" 
            onitemcommand="dsltAuthTeacher_OnItemCommand" DataKeyField="teacherNo" width="100%" CellPadding="0" CellSpacing="0">
            <HeaderTemplate>
             <table class="datagrid2" >
             <tr><th width="30%">教师名称</th><th>操作</th></tr></table>
            </HeaderTemplate>
                <FooterTemplate>
                    <%=(this.dsltAuthTeacher.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
    <ItemTemplate>
    <table class="datagrid2">
    <tr><td width="30%"><%#Eval("teacherName") %></td><td><asp:LinkButton ID="lnbRemove" runat="server" CommandName="remove">删除</asp:LinkButton></td></tr>
    </table>
    </ItemTemplate>
    </asp:DataList>
    </form>
</body>
</html>
