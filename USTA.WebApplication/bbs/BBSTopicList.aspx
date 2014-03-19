<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BBSindex.master" AutoEventWireup="true" Inherits="bbs_BBSTopicList" Codebehind="BBSTopicList.aspx.cs" %>
<%@ MasterType  VirtualPath="~/MasterPage/BBSindex.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BBSContent" Runat="Server">
    <!--Validate-->
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#aspnetForm").validate();
        });
    </script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'Textarea1', "80%", "250px");
        });
    </script>
<div class="ui-tabs-panel"  id="divFragment1" >
<div class="left tagName"><img src="images/bbstree.gif" border="0" /><%=tagName %> - 讨论区</div><div class="right likeBtn" ><a href="BBSTopicSearch.aspx?tag=<%=tag%>&forumId=<%=Request["forumID"].Trim() %>&classID=<%=Server.UrlEncode(classID) %>&termTag=<%=termTag %>">搜索话题</a></div>

                <asp:DataList ID="dlstTopTopic" runat="server" style="margin-top:10px;" Width="100%">
                   <HeaderTemplate>
                   <%=(hasControl && tag == "2" ? "<a href='AddBigTopic.aspx?tagName=" + tagName + "&forumId=" + forumId + "&keepThis=true&TB_iframe=true&height=400&width=800' title=\"添加置顶公告\" class=\"thickbox\">[添加置顶公告]</a>" : "")%>
                <%=(tag == "2") ? "<table width='100%' cellpadding='0' cellspacing='0' border='0' style='margin-top:10px;height:30px;border-top:1px #d3e0ec solid;border-bottom:1px #d3e0ec solid;background:#eef4f9;'><tr><td width='60%'>置顶公告标题</td><td width='15%'>作者</td><td width='10%'>回复/查看</td><td width='15%'>最后回复</td></tr></table>" : ""%>
                   </HeaderTemplate>
                    <ItemTemplate>
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="margin-top:10px;border-bottom:1px #ebebeb solid;">
                <tr><td width="60%"><img src="../images/BULLET.GIF" align="absmiddle" /><a href="BBSViewTopic.aspx?tag=<%=tag%>&topicId=<%#Eval("topicId")%>&forumId=<%#forumId %>"><%#Eval("topicTitle") %></a>
                 <%#(Convert.ToInt32((Eval("isTop").ToString().Trim().Length>0?Eval("isTop").ToString().Trim():"0".ToString()))>0?"<img src=\"../images/onTop.gif\" width=\"36\" height=\"13\" alt=\"置顶\" align='middle' />":null) %><%#USTA.Common.CommonUtility.isInToday(ht3.ContainsKey(Eval("topicId").ToString().Trim()) ? ht3[Eval("topicId").ToString().Trim()].ToString() : string.Empty) ? "<img src='../images/new.gif' align='middle' />" : ""%>
                    <%#(hasControl ? ((Convert.ToInt32((Eval("isTop").ToString().Trim().Length > 0 ? Eval("isTop").ToString().Trim() : "0".ToString())) > 0) ? "&nbsp;&nbsp;<a href=\"?tag=" + tag + "&fragment=1&forumId=" + forumId + "&cancelTopId=" + Eval("topicId") + "\">[取消置顶]</a>" : "&nbsp;&nbsp;<a href=\"?tag=" + tag + "&fragment=1&forumId=" + forumId + "&toTopId=" + Eval("topicId") + "\">[置顶]</a>") : "")%>
                    <%#(hasControl ? "&nbsp;&nbsp;<a href=\"?del=true&fragment=1&forumId=" + forumId + "&topicId=" + Eval("topicId") + "&tag=" + tag + " \" onclick=\"return deleteTip();\">[删除]</a>" : "")%></td>
                <td width="15%"><%#Eval("topicUserName") %><br /><span style="color:#999999;font:11px;"><%#Eval("updateTime")%></span></td>
                <td width="10%"><%#Convert.ToString(this.ht0[Eval("topicId").ToString()]).Length>0?this.ht0[Eval("topicId").ToString()]:"0"%>/<%#Eval("hits") %></td>
                <td width="15%"><%#ht1.ContainsKey(Eval("topicId").ToString()) ? ("  by " + this.ht1[Eval("topicId").ToString()]+"<br />"+"<span style=\"color:#999999;font:11px;\">"+this.ht2[Eval("topicId").ToString()]+"</span>") : "暂无回复"%>  </td></tr></table>
                        <%--<span id="topicTitle">
                        <img src="../images/BULLET.GIF" align="absmiddle" /><a href="BBSViewTopic.aspx?tag=<%=tag%>&topicId=<%#Eval("topicId")%>"><%#Eval("topicTitle") %></a>
                    </span>
                    <%#(Convert.ToInt32((Eval("isTop").ToString().Trim().Length>0?Eval("isTop").ToString().Trim():"0".ToString()))>0?"<img src=\"../images/onTop.gif\" width=\"36\" height=\"13\" alt=\"置顶\" align='middle' />":null) %>
                    (<%#Eval("updateTime")%>) <span>
                        <%#Eval("topicUserName") %>
                    </span><%#USTA.Common.CommonUtility.isInToday(ht3.ContainsKey(Eval("topicId").ToString().Trim()) ? ht3[Eval("topicId").ToString().Trim()].ToString() : string.Empty) ? "<img src='../images/new.gif' align='middle' />" : ""%>
                    <%#(hasControl ? ((Convert.ToInt32((Eval("isTop").ToString().Trim().Length > 0 ? Eval("isTop").ToString().Trim() : "0".ToString())) > 0) ? "<a href=\"?tag="+tag+"&fragment=1&forumId=" + forumId + "&cancelTopId=" + Eval("topicId") + "\">取消置顶</a>" : "<a href=\"?tag="+tag+"&fragment=1&forumId=" + forumId + "&toTopId=" + Eval("topicId") + "\">置顶</a>") : "")%>
                    <%#(hasControl ? "<a href=\"?del=true&fragment=1&forumId=" + forumId + "&topicId=" + Eval("topicId") + "&tag=" + tag + " \" onclick=\"return deleteTip();\">[删除]</a>" : "")%>--%>
                    </ItemTemplate>
                <FooterTemplate>
                    <%=((this.dlstTopTopic.Items.Count == 0 && tag == "2")? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
                </asp:DataList>
                <asp:DataList ID="dlsttopics" runat="server" Width="100%" CssClass="bbsDlsttopics">
                <HeaderTemplate>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" style="margin-top:10px;height:30px;border-top:1px #d3e0ec solid;border-bottom:1px #d3e0ec solid;background:#eef4f9;"><tr><td width="60%">标题</td><td width="15%">作者</td><td width="10%">回复/查看</td><td width="15%">最后回复</td></tr></table>
            
                </HeaderTemplate>
                <ItemTemplate>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" style="margin-top:10px;border-bottom:1px #ebebeb solid;">
                <tr><td width="60%"><img src="../images/BULLET.GIF" align="absmiddle" /><a href="BBSViewTopic.aspx?tag=<%=tag%>&topicId=<%#Eval("topicId")%>&forumId=<%#forumId %>&courseNo=<%=Request["forumId"]%>&classID=<%=Server.UrlEncode(Server.UrlDecode(Request["classID"]))%>&termtag=<%=Request["termtag"] %>"><%#Eval("topicTitle") %></a>
                 <%#(Convert.ToInt32((Eval("isTop").ToString().Trim().Length>0?Eval("isTop").ToString().Trim():"0".ToString()))>0?"<img src=\"../images/onTop.gif\" width=\"36\" height=\"13\" alt=\"置顶\" align='middle' />":null) %><%#USTA.Common.CommonUtility.isInToday(ht3.ContainsKey(Eval("topicId").ToString().Trim()) ? ht3[Eval("topicId").ToString().Trim()].ToString() : string.Empty) ? "<img src='../images/new.gif' align='middle' />" : ""%>
                    <%#(hasControl ? ((Convert.ToInt32((Eval("isTop").ToString().Trim().Length > 0 ? Eval("isTop").ToString().Trim() : "0".ToString())) > 0) ? "&nbsp;&nbsp;<a href=\"?tag=" + tag + "&fragment=1&forumId=" + Request["forumId"] + "&classID=" + Server.UrlEncode(Server.UrlDecode(Request["classID"])) + "&termtag=" + Request["termtag"] + "&cancelTopId=" + Eval("topicId") + "\">[取消置顶]</a>" : "&nbsp;&nbsp;<a href=\"?tag=" + tag + "&fragment=1&forumId=" + Request["forumId"] + "&classID=" + Request["classID"] + "&termtag=" + Request["termtag"] + "&toTopId=" + Eval("topicId") + "\">[置顶]</a>") : "")%>
                    <%#(hasControl ? "<a href=\"?del=true&fragment=1&forumId=" + Request["forumId"] + "&classID=" + Server.UrlEncode(Server.UrlDecode(Request["classID"])) + "&termtag=" + Request["termtag"] + "&topicId=" + Eval("topicId") + "&tag=" + tag + " \" onclick=\"return deleteTip();\">[删除]</a>" : "")%></td>
                <td width="15%"><%#Eval("topicUserName") %><br /><span style="color:#999999;font:11px;"><%#Eval("updateTime")%></span></td>
                <td width="10%"><%#Convert.ToString(this.ht0[Eval("topicId").ToString()]).Length>0?this.ht0[Eval("topicId").ToString()]:"0"%>/<%#Eval("hits") %></td>
                <td width="15%"><%#ht1.ContainsKey(Eval("topicId").ToString()) ? ("  by " + this.ht1[Eval("topicId").ToString()]+"<br />"+"<span style=\"color:#999999;font:11px;\">"+this.ht2[Eval("topicId").ToString()]+"</span>") : "暂无回复"%>  </td></tr></table>
                </ItemTemplate>
                <ItemStyle BorderWidth="0" />
                <FooterTemplate> <%=(this.dlsttopics.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" BorderWidth="0px" />
            </asp:DataList>
 
            
            <h3 style="height: 30px">
                发表新贴</h3>
            <table style="width: 100%; border-collapse: collapse; margin: 0px;">
                <tr style="width: 30%">
                    <td style="width: 15%"><a name="reply"></a>
                        使用标题:
                    </td>
                    <td style="width: 85%">
                        <asp:TextBox ID="txtTilte" runat="server" Width="400px" CssClass="required"></asp:TextBox>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    <textarea id="Textarea1" cols="100" rows="8" runat="server" clientidmode="Static"></textarea>
                    <br />
                      
                    </td>
                </tr>
                <tr>
                    <td>
                        附件:
                    </td>
                    <td style="width: 85%">
                        <!--upload start-->
                        <input id="Button3" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />
                        <div id="iframes" class="bbsupLoadSytle" style="width:100%">
                        </div>
                        <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
                        <!--upload end-->
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td><br /><asp:Button ID="btnCommit" runat="server" Text="提交" OnClick="btnCommit_Click" OnClientClick="return checkKindValue('Textarea1');" />
                        
                     
                    </td>
                </tr>
            </table>
            </div>
</asp:Content>

