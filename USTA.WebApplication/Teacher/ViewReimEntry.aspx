<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReimEntry.aspx.cs" Inherits="USTA.WebApplication.Teacher.ViewReimEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
        <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#form1").validate();
        });
        function deleteTip() {
            return confirm("确定删除？");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div><h3>报销详情</h3></div>
        <div>
            <table class="tableAddStyleNone" width="100%">
                <tr>
                    <td class="border" width="25%">项目名称：</td>
                    <td class="border" width="80%">
                        <asp:Literal ID="ReimEntry_ProjectName" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="border">报销项：</td>
                    <td class="border">
                        <asp:Literal ID="ReimEntry_ReimName" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="border">项目报销总金额：</td>
                    <td class="border">
                        <asp:Literal ID="ReimEntry_ProjectValue" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="border">
                        此项报销总金额：
                    </td>
                    <td class="border">
                        <asp:Literal ID="ReimEntry_ProjectReimValue" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
            <div>
                <span>报销详情：</span>
                <asp:DataList ID="ReimEntry_ReimItemList" runat="server" Width="100%" OnItemCommand="ReimItemList_Command" DataKeyField="id">
                    <HeaderTemplate>
                        <table width="100%" class="datagrid2">
                            <tr>
                                <th width="10%">编号</th>
                                <th width="10%">金额</th>
                                <th width="20%">时间</th>
                                <th width="50%">备注</th>
                                <th width="10%">操作</th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table width="100%" class="datagrid2">
                            <tr>
                                <td width="10%"><%#Container.ItemIndex + 1 %></td>
                                <td width="10%"><%#Eval("value") %></td>
                                <td width="20%"><%#Eval("createdTime") %></td>
                                <td width="50%"><%#Eval("memo") %></td>
                                <td width="10%">
                                    <asp:LinkButton ID="RemoveReimEntry" CommandName="delReimItem" runat="server" title="删除报销记录" OnClientClick="return deleteTip();">删除</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div>无报销记录</div>
                    </FooterTemplate>
                </asp:DataList>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
