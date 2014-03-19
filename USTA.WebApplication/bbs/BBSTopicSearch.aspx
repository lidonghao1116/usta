<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BBSindex.master" AutoEventWireup="true" Inherits="bbs_BBSTopicSearch" Codebehind="BBSTopicSearch.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BBSContent" Runat="Server">
<div class="ui-tabs-panel" id="divFragment2" runat="server">
<div style="height: 30px">在<a href="BBSTopicList.aspx?forumId=<%=forumId%>&classID=<%=Server.UrlEncode(classID) %>&termTag=<%=termTag %>&tag=<%=Request["tag"]%>"><font size="3"><%=tagName %></font></a>中&nbsp;搜索话题 <a href="BBSTopicList.aspx?forumId=<%=forumId%>&classID=<%=Server.UrlEncode(classID) %>&termTag=<%=termTag %>&tag=<%=Request["tag"]%>">[返回]</a></div> 
             
              <table style="width: 100%; border-collapse: collapse; margin: 0px;">
                <tr>
                    <td>
                    <%-- 
              <asp:DropDownList ID="ddlttype" runat="server">
                            <asp:ListItem Text="话题" Value="0" Selected="True"></asp:ListItem>
                             <asp:ListItem Text="回复" Value="1"></asp:ListItem> 
                        </asp:DropDownList>
                        --%>
                        <asp:TextBox ID="txtSearchString" runat="server"></asp:TextBox>
                        <asp:Button ID="btnsearch" runat="server" Text="搜索" OnClick="btnsearch_Click" />
                    </td>
                </tr>           
                    <asp:DataList ID="dlsttopicresult" runat="server" Width="100%" CssClass="bbsDlsttopics">
                <HeaderTemplate>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" style="margin-top:10px;height:30px;border-top:1px #d3e0ec solid;border-bottom:1px #d3e0ec solid;background:#eef4f9;"><tr><td width="60%">标题</td><td width="20%">作者</td><td width="20%">发布时间</td></tr></table>
                </HeaderTemplate>
                <ItemTemplate>
                      <table width="100%" cellpadding="0" cellspacing="0" border="0" style="margin-top:10px;border-bottom:1px #ebebeb solid;">
                <tr><td width="60%"> <img src="../images/BULLET.GIF" align="absmiddle" /><a href="BBSViewTopic.aspx?courseNo=<%=forumId%>&classID=<%=Server.UrlEncode(classID) %>&termTag=<%=termTag %>tag=<%=tag %>&topicId=<%#Eval("topicId")%>"><%#Eval("topicTitle") %></a>
                             </td><td width="20%"> 
                            <%#Eval("topicUserName")%></td><td width="20%"> <%#Eval("updateTime") %></td></tr>
                            </table>
                        </ItemTemplate>
                        <FooterTemplate>
                            <%=(this.dlsttopicresult.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                        <FooterStyle CssClass="datalistNoLine" />
                    </asp:DataList>
                 <%-- 
                    <asp:DataList ID="dlstpostresult" runat="server">
                        <ItemTemplate>
                            回复话题：<img src="../images/BULLET.GIF" align="absmiddle" /><a href="ViewTopic.aspx?tagPreName=<%=tagName %>&tagName=<%# Server.UrlEncode(DataBinder.Eval(Container.DataItem,"topicTitle").ToString().Trim()) %>&tag=<%=tag %>&topicId=<%#Eval("topicId")%>"><%#Eval("topicTitle") %></a>
                            回复者：<%#Eval("postUserName")%>
                            发表时间：<%#Eval("updateTime") %>
                        </ItemTemplate>
                        <FooterTemplate>
                            <%=(this.dlstpostresult.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                        <FooterStyle CssClass="datalistNoLine" />
                    </asp:DataList>
                    --%>
            </table>
        </div>
</asp:Content>

