<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_CInfoCourseNotify" 
    MasterPageFile="~/MasterPage/CourseInfoForTeacher.master" Codebehind="CInfoCourseNotify.aspx.cs" %>

<%@ MasterType  VirtualPath="~/MasterPage/CourseInfoForTeacher.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
         <a href="AddNotifyInfo.aspx?keepThis=true&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>&fragment=3&TB_iframe=true&height=400&width=830"
                title="添加课程通知" class="thickbox">添加通知</a>
          <%--OnItemCommand="dlstCourseNotify_ItemCommand"--%>
            <asp:DataList ID="dlstCourseNotify" runat="server" 
                DataKeyField="courseNotifyInfoId" Width="60%">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <caption>
                        </caption>
                        <tr>
                            <th scope="col" width="40%">
                                通知标题
                            </th >
                            <th scope="col" width="20%">
                               发布时间
                            </th>
                            <th scope="col" width="40%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <td class="row" width="40%">
                            <asp:Label ID="lblcourseNotifyInfoId" runat="server"  Text='<%#Eval("courseNotifyInfoId")%>'  Visible="false"></asp:Label>
                                <asp:CheckBox ID="ChkBox" ToolTip="选择/不选"  runat="server" />
                              &nbsp; <a href="/Common/ViewCourseNotify.aspx?keepThis=true&courseNotifyId=<%#Eval("courseNotifyInfoId")%>&classID=<%#Server.UrlEncode(Eval("classID").ToString().Trim())%>&termtag=<%#Eval("termTag") %>&fragment=5&TB_iframe=true&height=300&width=800"
                                    title="查看课程通知" class="thickbox"><%#Eval("courseNotifyInfoTitle")%></a>
                                    <%#(Convert.ToInt32((Eval("isTop").ToString().Trim().Length>0?Eval("isTop").ToString().Trim():"0".ToString()))>0?"<img src=\"../images/onTop.gif\" width=\"36\" height=\"13\" alt=\"置顶\" align='middle' />":null) %>
                                    <%#(isNew(Eval("updateTime").ToString()))?"<img src='../images/new.gif' align='middle' style='margin-bottom:5px;'/>":"" %>
                            </td>
                             <td class="row" width="20%">
                                 <%#Eval("updateTime")%>
                            </td>
                            <td class="row" width="40%">
                                <a href="EditNotifyInfo.aspx?keepThis=true&&page=<%=pageIndex %>&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID).ToString().Trim() %>&termtag=<%=Master.termtag.ToString().Trim() %>&courseNotifyId=<%#Eval("courseNotifyInfoId")%>&fragment=3&TB_iframe=true&height=400&width=800"
                                    title="修改课程通知" class="thickbox">修改</a> &nbsp;
                                <a href="?courseNo=<%=Master.courseNo%>&classID=<%=Server.UrlEncode(Master.classID).ToString().Trim() %>&termtag=<%=Master.termtag.ToString().Trim() %>&notifyId=<%#Eval("courseNotifyInfoId")%>&op=delete&page=<%=pageIndex %>">删除</a> &nbsp;  
                                <a href="?courseNo=<%=Master.courseNo%>&classID=<%=Server.UrlEncode(Master.classID).ToString().Trim() %>&termtag=<%=Master.termtag.ToString().Trim() %>&notifyId=<%#Eval("courseNotifyInfoId")%>&op=toTop"><%#(Convert.ToInt32(Eval("isTop").ToString().Trim()) > 0 ? "取消置顶" : "置顶")%></a>
                              
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstCourseNotify.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <input id="dzxBtnSelectAll" name="dzxBtnSelectAll" type="button" value="全选" onclick="selectAll();" />
            <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click" />
             <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager2" runat="server" UrlPaging="true" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager2_PageChanged"
                    PrevPageText="上一页"
                         LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;" HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
                </webdiyer:AspNetPager>
     </div>
</asp:Content>
