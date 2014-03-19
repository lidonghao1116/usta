<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSalaryStandardValue.aspx.cs" Inherits="USTA.WebApplication.Administrator.AddSalaryStandardValue" %>

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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            标准值：<asp:TextBox ID="SalaryStandardValue" runat="server" Width="100" CssClass="required number"></asp:TextBox>
            <asp:HiddenField ID="hf_SalaryItemId" runat="server"/>
            <asp:Button ID="btn_AddSalaryStandardValue" runat="server" Text="添加标准值" OnClick="AddSalaryStandardValue_Click"/>
        </div>
        <div>
            <div>
                <h3>已添加的标准值列表：</h3>
            </div>
            <div>
                <asp:DataList Width="80%" ID="StandardValueList" runat="server" DataKeyField="SalaryStandardValueId" OnItemCommand="StandardValueList_Command">
                    <HeaderTemplate>
                        <table class="datagrid2"  width="100%">
                            <tr>
                                <th width="20%">编号</th>
                                <th width="60%">金额(单位：元)</th>
                                <th width="20%">操作</th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table  width="100%"  class="datagrid2" >
                            <tr>
                                <td width="20%">
                                    <%#Container.ItemIndex + 1 %>
                                </td>
                                <td width="60%">
                                    <%#Eval("SalaryItemValue")%>
                                </td>
                                <td width="20%">
                                    <asp:LinkButton ID="DelSalaryStandardValue" CommandName="delValue" runat="server">删除</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%=(this.StandardValueList.Items.Count == 0 ? "未找到数据" : null)%>
                    </FooterTemplate>
                </asp:DataList>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
