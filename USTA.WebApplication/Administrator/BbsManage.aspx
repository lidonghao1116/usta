<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master"
    AutoEventWireup="true" Inherits="Administrator_BbsManage" EnableViewState CodeBehind="BbsManage.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
    <!--Validate-->
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#aspnetForm").validate();

            $("#teacher").change(function () {
                $("#assistants").toggle();
                $("#teachers").toggle();
                $("#ctl00_ContentPlaceHolder1_userType").val("1");
            });
            $("#assistant").change(function () {
                $("#assistants").toggle();
                $("#teachers").toggle();
                $("#userType").val("2");
            });
            $("td[col='tel']").click(function () {
                alert("您选择的老师是：" + $.trim($(this).text()));
                $("#userNo").val($(this).attr("name"));
                $("#username").html($(this).html());
                location.href = '#selectedAnchor';
            });
            $("td[col='tel']").hover(function () {
                $(this).addClass("usersdisplay");
            }, function () {
                $(this).removeClass("usersdisplay");
            }
	);
        });
    </script>
    <style type="text/css">
        .displays
        {
            display: block;
        }
        .nodisplays
        {
            display: none;
        }
        .users
        {
            text-align: center;
            height:40px;
            vertical-align:middle;
            display:block;
        }
        .usersdisplay
        {
            background: #9cf;
            cursor:pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>讨论区版块管理</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>添加讨论区版块</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>搜索帖子</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <asp:DataList ID="dlstforums" runat="server" Width="100%">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <th width="50%">
                            版块标题
                        </th>
                        <th width="50%">
                            操作
                        </th>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="50%">
                                <%#Eval("forumTitle")%>
                            </td>
                            <td width="50%">
                                <a href="EditForum.aspx?keepThis=true&forumId=<%#Eval("forumId")%>&TB_iframe=true&height=250&width=400"
                                    title="修改版块" class="thickbox">编辑</a> <a href="BbsManage.aspx?del=true&forumId=<%#Eval("forumId") %>"
                                        onclick="return deleteTip();">删除</a>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstforums.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server"><a name="selectedAnchor"></a>
            <div style="width: 100%; border: 1px solid #034F93; background: #EDEDED; margin-top: 10px;
                margin-bottom: 15px; margin-left: auto; margin-right: auto; clear: both; height: 80px;
                line-height: 40px;">
                <span style="margin-left: 10%">所属讨论区:
                    <asp:DropDownList ID="ddltforunType" runat="server" Width="">
                        <asp:ListItem Value="0" Selected="True">其他讨论区</asp:ListItem>
                        <asp:ListItem Value="1">专业交流区</asp:ListItem>
                    </asp:DropDownList>
                </span><span style="margin-left: 10%">版面标题：<asp:TextBox ID="txtTitle" runat="server"></asp:TextBox></span>
                <br />
                <span style="margin-left: 10%">指定版主：&nbsp;&nbsp;<span id="username" style="font: red"></span></span>
                <span style="margin-left: 25%">
                    <asp:Button ID="btnCommit" CssClass="Btn" runat="server" Text="添加" OnClick="btnCommit_Click" /></span>
            </div>
            <asp:HiddenField ID="userType" runat="server" Value="1" ClientIDMode="static" />
            <asp:HiddenField ID="userNo" runat="server" ClientIDMode="static" />
            <asp:DataList ID="dlstteachers" runat="server" Width="100%" RepeatDirection="Horizontal"
                RepeatColumns="6">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th>
                                老师列表如下，请选择：
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td name="<%#Eval("teacherNo").ToString().Trim()%>" col="tel" class="users">
                                    <%#Eval("teacherName").ToString().Trim()%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div id="assistants" style="display: none; margin-left: 200px">
            <asp:DataList ID="dlstAssistant" runat="server" Width="600px" RepeatDirection="Horizontal"
                RepeatColumns="6">
                <ItemTemplate>
                    <div name="<%#Eval("assistantNo").ToString().Trim()%>" col="tel" class="users">
                        <%#Eval("assistantName").ToString().Trim()%></div>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>
    <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        <asp:TextBox ID="txtSearchString" runat="server"></asp:TextBox>
        <asp:DropDownList ID="ddlttype" runat="server">
            <asp:ListItem Text="话题" Value="0" Selected="True"></asp:ListItem>
            <asp:ListItem Text="回复" Value="1"></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="Button1" runat="server" Text="搜索" OnClick="btsearch_Click" />
        <asp:DataList ID="dlsttopicresult" runat="server" Width="100%">
            <HeaderTemplate>
                <table class="datagrid2">
                    <th width="60%">
                        话题
                    </th>
                    <th width="20%">
                        作者
                    </th>
                    <th width="20%">
                        发表时间
                    </th>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table class="datagrid2">
                    <tr>
                        <td width="60%">
                            <a href="/bbs/BBSViewTopic.aspx?topicId=<%#Eval("topicId") %>">
                                <%#Eval("topicTitle")%></a>
                        </td>
                        <td width="20%">
                            <%#Eval("topicUserName")%>
                        </td>
                        <td width="20%">
                            <%#Eval("updateTime") %>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <FooterTemplate>
                <%=(this.dlsttopicresult.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
            <FooterStyle CssClass="datalistNoLine" />
        </asp:DataList>
        <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager2" runat="server"
            FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager2_PageChanged"
            PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
            HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
            SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
            CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;"
            ShowBoxThreshold="1">
        </webdiyer:AspNetPager>
        <asp:DataList ID="dlstpostresult" runat="server" Width="100%">
            <HeaderTemplate>
                <table class="datagrid2">
                    <th width="60%">
                        回复话题：
                    </th>
                    <th width="20%">
                        回复者
                    </th>
                    <th width="20%">
                        发表时间
                    </th>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table class="datagrid2">
                    <tr>
                        <td width="60%">
                            <%#Eval("topicTitle")%>
                        </td>
                        <td>
                            <%#Eval("postUserName")%>
                        </td>
                        <td>
                            <%#Eval("updateTime") %>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <FooterTemplate>
                <%=(this.dlstpostresult.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
            <FooterStyle CssClass="datalistNoLine" />
        </asp:DataList>
        <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager3" runat="server"
            FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager3_PageChanged"
            PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
            HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
            SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
            CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;"
            ShowBoxThreshold="1">
        </webdiyer:AspNetPager>
    </div>
    </div>
    </form>
</asp:Content>
