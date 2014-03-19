<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BBSindex.master" AutoEventWireup="true" Inherits="bbs_BBSOtherindex"  EnableViewState="false" Codebehind="BBSOtherindex.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/BBSindex.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BBSContent" runat="Server">
    <div class="ui-tabs-panel">
        <asp:DataList ID="dlstAboutOther" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"
            Width="100%" CssClass="bbsDlstAboutCourses">
            <ItemTemplate>
                <table style="color: #666666;">
                    <tr>
                        <td>
                            <%#USTA.Common.CommonUtility.isInToday(ht6.ContainsKey(Eval("forumId").ToString().Trim())?ht6[Eval("forumId").ToString().Trim()].ToString():string.Empty) ? "<img src='images/forum_new.gif' align='absmiddle' alt='有新帖' />" : "<img src='images/forum.gif' align='middle' />"%>&nbsp;
                        </td>
                        <td>
                            <a href="BBSTopicList.aspx?tag=3&forumId=<%#Eval("forumId") %>">
                                <%#Eval("forumTitle")%></a> (今日：<span style="color: #ff6600; font-weight: bold;"><%#Convert.ToString((this.ht4.ContainsKey(Eval("forumId").ToString().Trim())?int.Parse(this.ht4[Eval("forumId").ToString().Trim()].ToString()):0)+(this.ht5.ContainsKey(Eval("forumId").ToString().Trim()) ? int.Parse(this.ht5[Eval("forumId").ToString().Trim()].ToString()) : 0))%></span>)<br />
                            主题：<%#Convert.ToString(this.ht0.ContainsKey(Eval("forumId").ToString().Trim()) ? this.ht0[Eval("forumId").ToString().Trim()] : "0") %>
                            回复数：<%#Convert.ToString(this.ht1.ContainsKey(Eval("forumId").ToString().Trim()) ? this.ht1[Eval("forumId").ToString().Trim()] : "0") %><br />
                            最后回复：<br />
                            <%#ht2.ContainsKey(Eval("forumId").ToString()) ? ("  by " + this.ht2[Eval("forumId").ToString()]  + "<span style=\"color:#999999;font:11px;\">" + this.ht3[Eval("forumId").ToString()] + "</span>") : "暂无回复"%>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <ItemStyle CssClass="datalistLine" />
            <FooterTemplate>
                <%=(this.dlstAboutOther.Items.Count==0?"对不起，未找到数据":null) %></FooterTemplate>
            <FooterStyle CssClass="datalistNoLine" BorderWidth="0" />
        </asp:DataList>
    </div>
</asp:Content>
