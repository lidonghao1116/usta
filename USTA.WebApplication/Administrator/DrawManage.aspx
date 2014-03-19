<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master"
    AutoEventWireup="true" CodeBehind="DrawManage.aspx.cs" Inherits="USTA.WebApplication.Administrator.DrawManage" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>活动届次管理</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>活动类型管理</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>报名信息管理</span></a></li>
            <li id="liFragment4" runat="server"><a href="?fragment=4"><span>抽签结果管理</span></a></li>
            <li id="liFragment5" runat="server" visible="false"><a href="?fragment=5"><span></span>
            </a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <a href="AddGameCategory.aspx?page=<%=pageIndex %>&keepThis=true&TB_iframe=true&height=500&width=900"
                title="添加活动届次" class="thickbox">添加活动届次</a>
            <asp:DataList Width="100%" ID="dlGameCategory" runat="server" DataKeyField="gameCategoryId"
                OnItemCommand="dlGameCategory_ItemCommand" CellPadding="0" OnItemDataBound="dlGameCategory_ItemDataBound">
                <HeaderTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <th width="30%">
                                标题
                            </th>
                            <th width="15%">
                                发布时间
                            </th>
                            <th width="15%">
                                开始时间
                            </th>
                            <th width="15%">
                                截止时间
                            </th>
                            <th width="15%">
                                当前抽签状态
                            </th>
                            <th width="10%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <td width="30%">
                                <%#Eval("gameTitle").ToString().Trim()%>
            <a href="../Common/ViewGameCategoryInfo.aspx?gameCategoryId=<%#Eval("gameCategoryId").ToString().Trim() %>&keepThis=true&TB_iframe=true&height=500&width=900"
                title="<%#Eval("gameTitle").ToString().Trim()%>" class="thickbox">点击查看详细信息</a>
                            </td>
                            <td width="15%">
                                <%#Eval("updateTime").ToString().Trim()%>
                            </td>
                            <td width="15%">
                                <%#Eval("startTime").ToString().Trim()%>
                            </td>
                            <td width="15%">
                                <%#Eval("endTime").ToString().Trim()%>
                            </td>
                            <td width="15%">
                                <asp:Literal ID="ltlOpenDraw" runat="server" Visible="false" Text="开放&nbsp;&nbsp;"></asp:Literal>
                                <asp:Literal ID="ltlCloseDraw" runat="server" Visible="false" Text="关闭&nbsp;&nbsp;"></asp:Literal>
                                <asp:LinkButton ID="lbtnOpenDraw" runat="server" CommandName="openDraw" OnClientClick="return confirm('确认开放抽签吗？');" Visible="false">开放抽签</asp:LinkButton>
                                <asp:LinkButton ID="lbtnCloseDraw" runat="server" CommandName="closeDraw" OnClientClick="return confirm('确认关闭抽签吗？');" Visible="false">关闭抽签</asp:LinkButton>
                            </td>
                            <td width="10%">
                                <a href="EditGameCategory.aspx?gameCategoryId=<%#Eval("gameCategoryId")%>&page=<%=pageIndex %>&keepThis=true&TB_iframe=true&height=500&width=900"
                                    title="修改通知" class="thickbox">修改</a>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="delete" OnClientClick="return deleteTip();">删除</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlGameCategory.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;"
                UrlPaging="true" ID="AspNetPager1" runat="server" FirstPageText="首页" LastPageText="尾页"
                NextPageText="下一页" OnPageChanged="AspNetPager1_PageChanged" PrevPageText="上一页"
                LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;"
                ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        <div style="margin-bottom: 10px;">
        请选择活动届次：<asp:DropDownList ID="ddlGameCategoryAboutGameType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGameCategoryAboutGameType_SelectedIndexChanged"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
            <a href="AddGameType.aspx?page=<%=pageIndex %>&keepThis=true&TB_iframe=true&height=250&width=700"
                title="添加活动类型" class="thickbox">添加活动类型</a></div>
            <asp:DataList Width="100%" ID="dlGameType" runat="server" DataKeyField="gameTypeId"
                OnItemCommand="dlGameType_ItemCommand" CellPadding="0">
                <HeaderTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <th width="25%">
                                所属届次
                            </th>
                            <th width="25%">
                                标题
                            </th>
                            <th width="10%">
                                分组人数
                            </th>
                            <th width="15%">
                                面向教师类型
                            </th>
                            <th width="15%">
                                发布时间
                            </th>
                            <th width="10%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <td width="25%">
                                <%#Eval("gameTitle").ToString().Trim()%>
            <a href="../Common/ViewGameCategoryInfo.aspx?gameCategoryId=<%#Eval("gameCategoryId").ToString().Trim() %>&keepThis=true&TB_iframe=true&height=500&width=900"
                title="<%#Eval("gameTitle").ToString().Trim()%>" class="thickbox">点击查看详细信息</a>
                            </td>
                            <td width="25%">
                                <%#Eval("gameTypeTitle").ToString().Trim()%>
                            </td>
                            <td width="10%">
                                <%#Eval("groupCapability").ToString().Trim()%>
                            </td>
                            <td width="15%">
                                <%#USTA.Common.CommonUtility.ReturnAllowSexTypeByVal(Eval("allowSexType").ToString().Trim())%>
                            </td>
                            <td width="15%">
                                <%#Eval("updateTime").ToString().Trim()%>
                            </td>
                            <td width="10%">
                                <a href="EditGameType.aspx?gameTypeId=<%#Eval("gameTypeId")%>&keepThis=true&TB_iframe=true&height=250&width=700"
                                    title="修改活动类型" class="thickbox">修改</a>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="delete" OnClientClick="return deleteTip();">删除</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlGameType.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
            <div style="margin-bottom: 10px;">
                请选择活动届次：<asp:DropDownList ID="ddlGameCategory" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlGameCategory_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;请选择活动类型：<asp:DropDownList ID="ddlGameType" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlGameType_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <asp:DataList Width="100%" ID="dlEnroll" runat="server" DataKeyField="gameDrawListId"
                OnItemCommand="dlEnroll_ItemCommand" CellPadding="0">
                <HeaderTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <th width="10%">
                                教师姓名
                            </th>
                            <th width="20%">
                                报名时间
                            </th>
                            <th width="20%">
                                所属活动届次
                            </th>
                            <th width="10%">
                                活动类型
                            </th>
                            <th width="20%">
                                是否已经抽签
                            </th>
                            <th width="10%">
                                抽签时间
                            </th>
                            <th width="10%">
                                抽签组号
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <td width="10%">
                                <%#Eval("teacherName").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <%#Eval("enrollUpdateTime").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <%#Eval("gameTitle").ToString().Trim()%>
                            </td>
                            <td width="10%">
                                <%#Eval("gameTypeTitle").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <%#(string.IsNullOrEmpty(Eval("gameDrawListId").ToString().Trim())? "暂未抽签" : "已抽签")%>
                            </td>
                            <td width="10%">
                                <%#(string.IsNullOrEmpty(Eval("drawUpdateTime").ToString().Trim()) ? "暂未抽签" : Eval("drawUpdateTime").ToString().Trim())%>
                            </td>
                            <td width="10%">
                                <%#(string.IsNullOrEmpty(Eval("groupIndex").ToString().Trim()) ? "暂未抽签" : Eval("groupNum").ToString().Trim() + Eval("groupIndex").ToString().Trim())%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlEnroll.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager3" runat="server" FirstPageText="首页" LastPageText="尾页"
                NextPageText="下一页" OnPageChanged="AspNetPager3_PageChanged" PrevPageText="上一页"
                LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;"
                ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment4" runat="server">
            <div style="margin-bottom: 10px;">
                请选择活动届次：<asp:DropDownList ID="ddlDrawGameCategory" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlDrawGameCategory_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;请选择活动类型：<asp:DropDownList ID="ddlDrawGameType" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlDrawGameType_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        <asp:DataList ID="dlstGroupNum" runat="server" align="center" Width="100%" CellPadding="0"
                OnItemDataBound="dlstGroupNum_ItemDataBound" RepeatDirection="Horizontal" RepeatColumns="1">
                <ItemTemplate>
                    <table style="border-bottom: 1px solid #056FAE; height: 25px;" width="100%">
                        <tr>
                            <td style="height: 25px;">所属组：
                            <%#Eval("groupNum").ToString().Trim() %>
                            </td>
                        </tr>
                    </table>
                    <asp:DataList ID="dlstGroupIndex" runat="server" CellPadding="0" Width="100%" Style="margin-bottom: 10px;" RepeatColumns="6">
                        <ItemTemplate>
                            <table style="border-bottom: 1px dashed #CCCCCC;" width="100%">
                                <tr>
                                    <td style="height: 25px;">
                                    <%#Eval("groupIndex").ToString().Trim() %>：
                                    <%#Eval("teacherName").ToString().Trim() %>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                <FooterStyle CssClass="datalistNoLine" />
                    </asp:DataList>
                </ItemTemplate>
                <ItemStyle Width="50%" VerticalAlign="Top" />
                <FooterTemplate>
                    <%=(this.dlstGroupNum.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
                <HeaderTemplate>
                </HeaderTemplate>
            </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment5" runat="server">
        </div>
    </div>
    </form>
</asp:Content>
