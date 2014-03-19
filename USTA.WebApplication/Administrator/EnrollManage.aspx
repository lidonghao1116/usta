<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="EnrollManage.aspx.cs" Inherits="USTA.WebApplication.Administrator.EnrollManage" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>活动报名</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>我的报名信息</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>查看抽签结果</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <div style="margin-bottom: 10px;">
                请选择活动届次：<asp:DropDownList ID="ddlEnrollGameCategory" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlEnrollGameCategory_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;请选择活动类型：<asp:DropDownList ID="ddlEnrollGameType" runat="server">
                </asp:DropDownList>
            </div>
            <asp:Button ID="btnEnroll" runat="server" Text="点击报名" OnClick="btnEnroll_Click" OnClientClick="return confirm('确定报名吗？');" />
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
            <div style="margin-bottom: 10px;">
               请选择活动届次：<asp:DropDownList ID="ddlGameCategory" runat="server" ClientIDMode="Static">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;请选择活动类型：<asp:DropDownList ID="ddlGameType" runat="server" ClientIDMode="Static">
                </asp:DropDownList>
            </div>
            <asp:DataList Width="100%" ID="dlEnroll" runat="server" DataKeyField="gameDrawListId"
                OnItemCommand="dlEnroll_ItemCommand" CellPadding="0" OnItemDataBound="dlEnroll_ItemDataBound">
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
            <a href="../Common/ViewGameCategoryInfo.aspx?gameCategoryId=<%#Eval("gameCategoryId").ToString().Trim() %>&keepThis=true&TB_iframe=true&height=500&width=900"
                title="<%#Eval("gameTitle").ToString().Trim()%>" class="thickbox">点击查看详细信息</a>
                            </td>
                            <td width="10%">
                                <%#Eval("gameTypeTitle").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <asp:Literal ID="ltlNoDraw" runat="server" visible="false" Text="暂未抽签"></asp:Literal>
                                <asp:LinkButton ID="lbtnDraw" runat="server" CommandName="draw" CommandArgument='<%#Eval("gameCategoryId").ToString().Trim()+","+Eval("gameTypeId").ToString().Trim() %>' visible="false" OnClientClick="return confirm('确定抽签吗？');">抽签</asp:LinkButton>
                                <asp:Literal ID="ltlDraw" runat="server" visible="false" Text="已抽签"></asp:Literal>
                                <asp:Literal ID="ltlCloseDraw" runat="server" visible="false" Text="暂未开放抽签"></asp:Literal>
                                
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
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
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
    </div>
    </form>
</asp:Content>