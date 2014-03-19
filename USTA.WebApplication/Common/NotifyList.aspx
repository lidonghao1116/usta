<%@ Page Language="C#" AutoEventWireup="true" Inherits="Common_NotifyList"
    MasterPageFile="~/MasterPage/FrameManage.master" Codebehind="NotifyList.aspx.cs" EnableViewState="false" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <%--    
    <span id="tip" runat="server"></span>--%>
    <table align="center" id="tbTip" runat="server" visible="false" width="710px" style="margin-left: auto;
        margin-right: auto;" cellpadding="5" cellspacing="5">
        <tr>
            <td style="padding-top: 10px;" id="tdSchoolWorks" runat="server">
                <div class="ViewTips" id="divSchoolWorks" runat="server">
                    <div class="TipsTopic">
                        <asp:Literal ID="ltlSchoolWorksTip" runat="server"></asp:Literal>
                        &nbsp;&nbsp;&nbsp; <a id="schoolWorksInfo" href="#" onclick="tipsFold('schoolWorksInfo', 'schoolWorksTip');">
                            [查看提示]</a></div>
                    <div class="TipsContend" id="schoolWorksTip">
                        <asp:DataList ID="dlstSchoolwork" runat="server">
                            <ItemTemplate>
                                &nbsp;&nbsp;<img src="../images/ann.gif" /><a href="/Student/CInfoSchoolwork.aspx?courseNo=<%#Eval("courseNo").ToString().Trim() %>&ClassID=<%#Server.UrlEncode(Eval("ClassID").ToString().Trim()) %>&termTag=<%#Eval("termTag").ToString().Trim() %>&schoolworkNotifyId=<%#Eval("schoolWorkNofityId")%>">
                                    <%#Eval("schoolWorkNotifyTitle")%></a><%#Convert.ToDateTime(Eval("deadline"))< DateTime.Now ? "（已过期）" : ""%>
                            </ItemTemplate>
                            <ItemStyle Height="30px" />
                        </asp:DataList>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px;" id="tdSchoolWorksPaper" runat="server">
                <div class="ViewTips" id="divSchoolWorksPaper" runat="server">
                    <div class="TipsTopic">
                        <asp:Literal ID="ltlschoolworkpaper" runat="server"></asp:Literal>
                        &nbsp;&nbsp;&nbsp; <a id="schoolWorksPaperInfo" href="#" onclick="tipsFold('schoolWorksPaperInfo', 'schoolWorksPaperTip');">
                            [查看提示]</a></div>
                    <div class="TipsContend" id="schoolWorksPaperTip">
                        <asp:DataList ID="dlstSchoolworkpa" runat="server">
                            <ItemTemplate>
                                &nbsp;&nbsp;<img src="../images/ann.gif" /><a href="/Student/CInfoSchoolwork.aspx?courseNo=<%#Eval("courseNo").ToString().Trim() %>&ClassID=<%#Server.UrlEncode(Eval("ClassID").ToString().Trim()) %>&termTag=<%#Eval("termTag").ToString().Trim() %>&schoolworkNotifyId=<%#Eval("schoolWorkNofityId")%>">
                                    <%#Eval("schoolWorkNotifyTitle")%></a><%#Convert.ToDateTime(Eval("deadline"))< DateTime.Now ? "（已过期）" : ""%>
                            </ItemTemplate>
                            <ItemStyle Height="30px" />
                        </asp:DataList></div>
                </div>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px;" id="tdExperiments" runat="server">
                <div class="ViewTips" id="divExperiments" runat="server">
                    <div class="TipsTopic">
                        <asp:Literal ID="ltlExperimentsTip" runat="server"></asp:Literal>次实验待提交&nbsp;&nbsp;&nbsp;&nbsp;<a
                            id="experimentsInfo" href="#" onclick="tipsFold('experimentsInfo','experimentsTip');">[查看提示]</a></div>
                    <div class="TipsContend" id="experimentsTip">
                        <asp:DataList ID="dlstExpriment" runat="server">
                            <ItemTemplate>
                                &nbsp;&nbsp;<img src="../images/ann.gif" />
                                <%--<a href="/Student/CourseInfo.aspx?fragment=9&courseNo=<%#Eval("courseNo").ToString().Trim() %>&experimentResourceId=<%#Eval("experimentResourceId")%>">--%>
                                <a href="/Student/CInfoExperiment.aspx?fragment=9&courseNo=<%#Eval("courseNo").ToString().Trim() %>&ClassID=<%#Server.UrlEncode(Eval("ClassID").ToString().Trim()) %>&termTag=<%#Eval("termTag").ToString().Trim() %>&experimentResourceId=<%#Eval("experimentResourceId")%>">
                                    <%#Eval("experimentResourceTitle")%></a><%#Convert.ToDateTime(Eval("deadline"))< DateTime.Now ? "（已过期）" : ""%>
                            </ItemTemplate>
                            <ItemStyle Height="30px" />
                        </asp:DataList></div>
                </div>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px;" id="tdFeedBack" runat="server">
                <div class="ViewTips" id="divFeedBack" runat="server">
                    <div class="TipsTopic">
                        <asp:Literal ID="ltlFeedBack" runat="server"></asp:Literal>条意见反馈回复&nbsp;&nbsp;&nbsp;&nbsp;<a
                            id="feedBackInfo" href="#" onclick="tipsFold('feedBackInfo','feedBackTip');">[查看提示]</a></div>
                    <div class="TipsContend" id="feedBackTip">
                        <asp:DataList ID="dlistFeedBack" runat="server">
                            <ItemTemplate>
                                &nbsp;&nbsp;<img src="../images/ann.gif" /><a href="/Common/FeedBack.aspx?fragment=2">
                                查看意见反馈回复
                            </ItemTemplate>
                            <ItemStyle Height="30px" />
                        </asp:DataList></div>
                </div>
            </td>
        </tr>

        <tr>
            <td style="padding-top: 10px;" id="tdExam" runat="server">
                <div class="ViewTips" id="divExam" runat="server">
                    <div class="TipsTopic">
                        <asp:Literal ID="ltlExamTip" runat="server"></asp:Literal>条考试信息&nbsp;&nbsp;&nbsp;&nbsp;<a
                            id="examInfo" href="#" onclick="tipsFold('examInfo','examTip');">[查看提示]</a></div>
                    <div class="TipsContend" id="examTip">
                        <asp:DataList ID="dlstExam" runat="server">
                            <ItemTemplate>
                                &nbsp;&nbsp;<img src="../images/ann.gif" /><%#Eval("courseName")%>，时间：<%#Convert.ToDateTime(Eval("examArrangeTime")).ToString("yyyy-MM-dd HH:mm")%>，地点：<%#Eval("examArrageAddress")%>
                            </ItemTemplate>
                        </asp:DataList></div>
                </div>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px;" id="tdNotify" runat="server">
                <div class="ViewTips" id="divNotify" runat="server">
                    <div class="TipsTopic">
                        近期课程通知&nbsp;&nbsp;(<asp:Literal ID="ltlnotify" runat="server"></asp:Literal>)&nbsp;&nbsp;<a
                            id="A1" href="#" onclick="tipsFold('A1','Div2');">[查看提示]</a></div>
                    <div class="TipsContend" id="Div2">
                        <asp:DataList ID="DataList1" runat="server">
                            <ItemTemplate>
                                &nbsp;&nbsp;<img src="../images/ann.gif" /><%#Eval("courseName").ToString().Trim()%>:<a href="/Common/ViewCourseNotify.aspx?keepThis=true&courseNotifyId=<%#Eval("courseNotifyInfoId")%>&fragment=5&TB_iframe=true&height=300&width=800"
                                    title="查看课程通知" class="thickbox"><%#Eval("courseNotifyInfoTitle").ToString().Trim()%></a>
                            </ItemTemplate>
                            <ItemStyle Height="30px" />
                        </asp:DataList>
                    </div>
                </div>
            </td>
        </tr>
    </table>

    <table align="center" id="tbTeacherNotifyInfo" runat="server" width="710px" style="margin-left: auto;
        margin-right: auto;" cellpadding="5" cellspacing="5">
        <tr>
            <td style="padding-top: 10px;" id="tdArchivesNotify" runat="server" visible="false">
                <div class="ViewTips" id="divArchivesNotify" runat="server">
                    <div class="TipsTopic">
                        <%--<asp:Literal ID="Literal1" runat="server"></asp:Literal>--%>温馨提示：请及时上传结课资料（此为提示，不代表您未上传结课资料）&nbsp;&nbsp;&nbsp;&nbsp;<%--<a
                            id="aArchivesNotify" href="#" onclick="tipsFold('aArchivesNotify','ArchivesNotify');">[查看提示]</a>--%></div>
                    <div class="TipsContend" id="ArchivesNotify"><%--
                        <asp:DataList ID="DataList2" runat="server">
                            <ItemTemplate>
                                &nbsp;&nbsp;<img src="../images/ann.gif" /><a href="/Common/FeedBack.aspx?fragment=2">
                                查看意见反馈回复
                            </ItemTemplate>
                            <ItemStyle Height="30px" />
                        </asp:DataList>--%></div>
                </div>
            </td>
        </tr>

        
        <tr>
            <td style="padding-top: 10px;" id="tdGameCategory" runat="server" visible="false">
                <div class="ViewTips" id="divGameCategory" runat="server">
                    <div class="TipsTopic">
                        <%--<asp:Literal ID="Literal1" runat="server"></asp:Literal>--%>温馨提示：当前有正在进行报名的活动&nbsp;&nbsp;<a href="../<%=(isAdmin=="0"?"Administrator":"Teacher") %>/EnrollManage.aspx?fragment=1">点击报名</a>&nbsp;&nbsp;（此为提示，不代表您未报名）&nbsp;&nbsp;&nbsp;&nbsp;<%--<a
                            id="aArchivesNotify" href="#" onclick="tipsFold('aArchivesNotify','ArchivesNotify');">[查看提示]</a>--%></div>
                    <div class="TipsContend" id="GameCategory"><%--
                        <asp:DataList ID="DataList2" runat="server">
                            <ItemTemplate>
                                &nbsp;&nbsp;<img src="../images/ann.gif" /><a href="/Common/FeedBack.aspx?fragment=2">
                                查看意见反馈回复
                            </ItemTemplate>
                            <ItemStyle Height="30px" />
                        </asp:DataList>--%></div>
                </div>
            </td>
        </tr>
        </table>
    <div id="container-1">
        <ul class="ui-tabs-nav">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            <li id="liFragment1" runat="server"><a href="?pid=5&fragment=1"><span>教学相关</span></a></li>
            <li id="liFragment4" runat="server"><a href="?pid=6&fragment=4"><span>学位论文申请</span></a></li>
            <li id="liFragment2" visible="false" runat="server"><a href="?fragment=2"><span>查看通知信息</span></a></li>
            <li id="liFragment3" visible="false" runat="server"><a href="javascript:void(0);"><span>
                <%=Request["notifyTypeName"] %></span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment4" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <asp:DataList ID="dlstNotifyType" runat="server" align="center" Width="100%" CellPadding="0"
                OnItemDataBound="dlstNotifyType_ItemDataBound" RepeatDirection="Horizontal" RepeatColumns="2">
                <ItemTemplate>
                    <table style="border-bottom: 1px solid #056FAE; height: 25px;" width="90%">
                        <tr>
                            <td style="height: 25px; width: 70%;">
                                <%#Eval("notifyTypeName")%>
                            </td>
                            <td style="height: 25px; width: 30%; text-align: right;">
                                <a href="?fragment=3&notifyTypeId=<%#Eval("notifyTypeId") %>&notifyTypeName=<%#Eval("notifyTypeName")%>">
                                    更多>></a></span>
                            </td>
                        </tr>
                    </table>
                    <asp:DataList ID="dlstNotify" runat="server" CellPadding="0" Width="100%" Style="margin-bottom: 10px;">
                        <ItemTemplate>
                            <table style="border-bottom: 1px dashed #CCCCCC;" width="90%">
                                <tr>
                                    <td width="70%" style="height: 25px;">
                                        <img src="../images/BULLET.GIF" align="absmiddle" />
                                        <a href="/Common/NotifyList.aspx?fragment=2&adminNotifyInfoId=<%#Eval("adminNotifyInfoId") %>">
                                            <%#Eval("notifyTitle")%></a>
                                        <%#(Convert.ToInt32((Eval("isTop").ToString().Trim().Length>0?Eval("isTop").ToString().Trim():"0".ToString()))>0?"<img src=\"../images/onTop.gif\" width=\"36\" height=\"13\" alt=\"置顶\" align='absmiddle' />":null) %>
                                        <%#(isNew(Eval("updateTime").ToString()))?"<img src='../images/new.gif' align='middle' style='margin-bottom:5px;'/>":"" %>
                                    </td>
                                    <td width="30%" align="left">
                                        <%#Eval("updateTime")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </ItemTemplate>
                <ItemStyle Width="50%" VerticalAlign="Top" />
                <FooterTemplate>
                    <%=(this.dlstNotifyType.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
                <HeaderTemplate>
                </HeaderTemplate>
            </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
            <asp:DataList ID="news" runat="server" Width="100%">
                <ItemTemplate>
                    <table class="tableEditStyle" style="width: 100%;">
                        <tr>
                            <td style="height: 25px; font-size: 16px; font-weight: bold; text-align: center;">
                                <%#Eval("notifyTitle").ToString().Trim()%>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                发布时间：<%#Eval("updateTime") %>
                                &nbsp;&nbsp;&nbsp;&nbsp;浏览次数：<%#Eval("scanCount") %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%#Eval("notifyContent").ToString().Trim() %>
                                <br />
                                <br />
                                <%#GetURL(Eval("attachmentIds").ToString().Trim())%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
            <asp:DataList ID="dlistNotifyList" runat="server" align="center" Width="100%" CellPadding="0">
                <ItemTemplate>
                    <table style="border-bottom: 1px dashed #CCCCCC;" width="100%">
                        <tr>
                            <td width="50%" style="height: 25px;">
                                <img src="../images/BULLET.GIF" align="absmiddle" />
                                <a href="/Common/NotifyList.aspx?fragment=2&adminNotifyInfoId=<%#Eval("adminNotifyInfoId") %>">
                                    <%#Eval("notifyTitle")%></a>
                                <%#(Convert.ToInt32((Eval("isTop").ToString().Trim().Length>0?Eval("isTop").ToString().Trim():"0".ToString()))>0?"<img src=\"../images/onTop.gif\" width=\"36\" height=\"13\" alt=\"置顶\" align='absmiddle' />":null) %>
                                <%#(isNew(Eval("updateTime").ToString()))?"<img src='../images/new.gif' align='middle' style='margin-bottom:5px;'/>":"" %>
                            </td>
                            <td width="50%" align="left">
                                <%#Eval("updateTime")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlistNotifyList.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
                <HeaderTemplate>
                </HeaderTemplate>
            </asp:DataList>
            <div align="center">
                <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager1"
                    runat="server" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" 
                    PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
                </webdiyer:AspNetPager>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
