<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BBSindex.master" AutoEventWireup="true" Inherits="bbs_BBSCoursesindex" Codebehind="BBSCoursesindex.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/BBSindex.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%-- Add content controls here --%>
<asp:Content ContentPlaceHolderID="BBSContent" runat="server">
    <div class="ui-tabs-panel">
        <div style="width: 100%; margin-left: auto; margin-right: auto; height: 40px; background: #FFFFFF;">
            <span style="float: left; margin-left: 20px;">请选择要查看的学期数据：<asp:DropDownList ID="ddltTerms"
                runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddltTerms_SelectedIndexChanged">
            </asp:DropDownList>
            </span>
        </div>
        <asp:DataList ID="dlstAboutCourses" runat="server" RepeatDirection="Horizontal" RepeatColumns="3"
            Width="100%" CssClass="bbsDlstAboutCourses" EnableViewState="false">
            <ItemTemplate>
                <table style="color: #666666;">
                    <tr>
                        <td>
                            <%#USTA.Common.CommonUtility.isInToday(ht6.ContainsKey(Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()) ? ht6[Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()].ToString() : string.Empty) ? "<img src='images/forum_new.gif' align='absmiddle' alt='有新帖' />" : "<img src='images/forum.gif' align='middle' />"%>&nbsp;
                        </td>
                        <td>
                            <a href="BBSTopicList.aspx?tag=1&forumId=<%#Eval("courseNo").ToString().Trim()%>&termtag=<%#Eval("termtag").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("ClassID").ToString().Trim()) %>">
                                <%#Eval("courseName").ToString().Trim()%>(<%#Eval("classID").ToString().Trim() %>)</a> (今日：<span style="color: #ff6600; font-weight: bold;"><%#Convert.ToString((this.ht4.ContainsKey(Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()) ? int.Parse(this.ht4[Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()].ToString()) : 0) + (this.ht5.ContainsKey(Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()) ? int.Parse(this.ht5[Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()].ToString()) : 0))%></span>)<br />
                            主题：<%#Convert.ToString(this.ht0.ContainsKey(Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()) ? this.ht0[Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()] : "0")%>回复数：<%#Convert.ToString(this.ht1.ContainsKey(Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()) ? this.ht1[Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()] : "0")%><br />
                            最后回复：<br />
                            <%#this.ht2.ContainsKey(Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()) ? ("  by " + this.ht2[Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()] + "<span style=\"color:#999999;font:11px;\">" + this.ht3[Eval("courseNo").ToString().Trim() + Eval("classID").ToString().Trim() + Eval("termTag").ToString().Trim()] + "</span>") : "暂无回复"%>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <ItemStyle CssClass="datalistLine" Height="65px" />
            <FooterTemplate>
                <%=(this.dlstAboutCourses.Items.Count==0?"对不起，未找到数据":null) %></FooterTemplate>
            <FooterStyle CssClass="datalistNoLine" BorderWidth="0" />
        </asp:DataList>
    </div>
</asp:Content>
