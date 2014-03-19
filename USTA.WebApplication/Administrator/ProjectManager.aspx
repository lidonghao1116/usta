<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="ProjectManager.aspx.cs" Inherits="USTA.WebApplication.Administrator.ProjectManager" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
    <!--Validate-->
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#aspnetForm").validate();
        });

        function addTip() {
            return confirm("确定添加？");
        }

        function deleteTip() {
            return confirm("确定删除？");
        }

    </script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript" src="../javascript/ui/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'Textarea1', "100%", "200px");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>设置项目报销管理员</span></a></li>
        </ul>
        <ul class="ui-tabs-nav">
            <li id="liFragment2" runat="server" visible="false"><a href="?fragment=2"><span>设置项目报销管理员</span></a></li>
        </ul>
        <ul class="ui-tabs-nav">
            <li id="liFragment3" runat="server" visible="false"><a href="?fragment=3" visible="false"><span>设置项目报销管理员</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <asp:Label ID="lblKeyword" runat="server" Text="关键字(用户ID或者名字)：" Width="150px"></asp:Label>
            <asp:TextBox ID="txtKeyword" ClientIDMode="static" runat="server"></asp:TextBox>
            <asp:Button ID="btnQuery" runat="server" Text="模糊查询"
                OnClick="SearchTeacher_Click" />
                <a href="ViewProjectManagerAuth.aspx?keepThis=true&>&TB_iframe=true&height=300&width=500" title="查看有管理权限的用户" class="thickbox">查看有管理权限的用户</a>
            <br />
            <br />
            <asp:DataList ID="dlSearchTeacher" runat="server" DataKeyField="teacherNo" OnItemCommand="dlSearchTeacher_ItemCommand" OnItemDataBound="dlSearchTeacher_ItemDataBound"
                Width="100%">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="10%">
                                教师姓名
                            </th>
                            <th width="20%">
                                邮箱地址
                            </th>
                            <th width="20%">
                                办公地址
                            </th>
                            <th width="50%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="10%">
                                <%#Eval("teacherName").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <%#Eval("emailAddress").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <%#Eval("officeAddress").ToString().Trim()%>
                            </td>
                            <td width="50%">
                                <asp:LinkButton ID="LinkButton_RemoveProjectManagerPermission" runat="server" CommandName="removeAuth" title="删除管理权限" OnClientClick="return deleteTip();">删除管理权限</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton_AddProjectManagerPermission" runat="server"
                                    CommandName="addAuth" OnClientClick="return addTip('');" title="添加管理权限">添加管理权限</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlSearchTeacher.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;"  ID="TeacherListPager" runat="server"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="TeacherSearch_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server" visible="false"></div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server" visible="false"></div>
        
    </div>
    </form>
</asp:Content>


