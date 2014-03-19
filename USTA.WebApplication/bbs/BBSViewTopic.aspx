<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BBSindex.master" AutoEventWireup="true" Inherits="bbs_BBSViewTopic" Codebehind="BBSViewTopic.aspx.cs" %>
<%@ MasterType  VirtualPath="~/MasterPage/BBSindex.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BBSContent" Runat="Server">
 
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'Textarea1', "80%", "200px");
        });
    </script>
<div class="ui-tabs-panel" id="divFragment1" runat="server">
    <div class="tagName topic">
    <a href="BBSTopicList.aspx?forumId=<%=courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Server.UrlDecode(Request["classID"])) %>&termtag=<%=Request["termtag"] %>&tag=<%=Request["tag"]%>">
        <img src="images/bbstree.gif" border="0" /><%=tagName %>-讨论区</a>&nbsp; >>&nbsp;&nbsp;<%=topicName%></div>
            <asp:DataList ID="dlsttopic" runat="server" width="100%">
                
                <ItemTemplate>
                 <table width="100%"><tr><td width="50%">标题:&nbsp;&nbsp;<%#Eval("topicTitle")%></td><td width="50%" style="height:30px;" align="right">
                  <a href="#reply">[回复]</a>&nbsp;&nbsp;
                            <%if (hascontrol)
                              { %>
                            <a href="BBSTopicList.aspx?del=true&forumId=<%=courseNo.Trim() %>&classID=<%=Server.UrlEncode(Server.UrlDecode(Request["classID"])) %>&termtag=<%=Request["termtag"]%>&topicId=<%#Eval("topicId")%>&tag=<%=Request["tag"] %>"
     onclick="return deleteTip();">[删除]</a> 
                             <a href="EditTopic.aspx?keepThis=true&course=<%=courseNo.Trim()%>&classID=<%=Server.UrlEncode(Server.UrlDecode(Request["classID"])) %>&termtag=<%=Request["termtag"]%>&topicId=<%#Eval("topicId").ToString().Trim()%>&tag=<%=tag%>&TB_iframe=true&height=400&width=800" title="修改话题" class="thickbox">[编辑]</a>                            
                            <%} %>
                            <a href="BBSTopicList.aspx?forumId=<%=courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Server.UrlDecode(Request["classID"])) %>&termtag=<%=Request["termtag"] %>&tag=<%=tag %>">[返回]</a>
                 </td></tr></table>
                 
                  <table width="100%" style="border:0px;"><tr><td align="center" width="15%" style="border:0px;background:#f4f9fd;border-top:3px #d3e8f2 solid;padding:10px;" valign="top"><img src="<%#GetAvatar(Eval("topicUserNo").ToString().Trim(),int.Parse(Eval("topicUserType").ToString().Trim()))%>" /><br /><%#Eval("topicUserName")%><br />
                  <%#IsMyself(Eval("topicUserNo").ToString().Trim(), int.Parse(Eval("topicUserType").ToString().Trim())) ? "<a href=\"AvatarsSelect.aspx?keepThis=true&TB_iframe=true&height=400&width=700\" title=\"选择系统自定义头像\" class=\"thickbox\">选择系统自定义头像</a>&nbsp;&nbsp;<br/>或&nbsp;&nbsp;<a href=\"UploadAvatar.aspx?keepThis=true&TB_iframe=true&height=200&width=600\" title=\"上传自定义头像\" class=\"thickbox\">上传自定义头像</a>" : ""%>
                  </td><td width="80%" style="border:0px;border-top:3px #ebf2f8 solid;padding:10px;">
                    <%#int.Parse(Eval("topicUserType").ToString())<3?"<img src=\"../images/star.png\" align=\"middle\" />":"" %>
                    &nbsp;&nbsp; 发布时间：<%#Eval("updateTime") %><span style=" float:right;"><a name='post<%#Eval("topicId") %>' href='#post<%#Eval("topicId") %>'>楼主</a></span>
                  <hr style="color: #CAD9EA; margin-top: 10px; margin-bottom: 10px;" />
                    <%#Eval("topicContent")%>
                    <br />
                    <%#this.GetURL(Eval("attachmentIds").ToString())%>
                    <br />
                    </td></tr></table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlsttopic.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine displayNo" />
            </asp:DataList>
            <asp:DataList ID="dlstposts" runat="server" width="100%">
                <ItemTemplate>
                <table width="100%" style="border:0px;"><tr><td width="15%" align="center" style="border:0px;background:#f4f9fd;border-top:3px #d3e8f2 solid;padding:10px;" valign="top"><img src="<%#this.GetAvatar(Eval("postUserNo").ToString().Trim(),int.Parse(Eval("postUserType").ToString().Trim()))%>" /><br /><%#Eval("postUserName")%><br /><%#IsMyself(Eval("postUserNo").ToString().Trim(), int.Parse(Eval("postUserType").ToString().Trim())) ? "<a href=\"AvatarsSelect.aspx?keepThis=true&TB_iframe=true&height=400&width=700\" title=\"选择系统自定义头像\" class=\"thickbox\">选择系统自定义头像</a>&nbsp;&nbsp;<br/>或&nbsp;&nbsp;<a href=\"UploadAvatar.aspx?keepThis=true&TB_iframe=true&height=200&width=600\" title=\"上传自定义头像\" class=\"thickbox\">上传自定义头像</a>" : ""%></td><td width="80%" style="border:0px;border-top:3px #ebf2f8 solid;padding:10px;">
                    
                    &nbsp;&nbsp;发布时间：<%#Eval("updateTime") %>&nbsp;&nbsp;&nbsp;<span style=" float:right;"><a name='post<%#Eval("postId") %>' href='#post<%#Eval("postId") %>'><%=floor++%>楼</a></span>
                    <%if (hascontrol)
                      { %><a href="BBSViewTopic.aspx?del=true&postId=<%#Eval("postId")%>&topicId=<%#Eval("topicId")%>&tag=<%=tag %>" onclick="return deleteTip();">[删除]</a>
                    <%} %>
                    <hr style="color: #CAD9EA; margin-top: 10px; margin-bottom: 10px;" />
                    <%#Eval("postContent")%>
                    <br />
                    <%#this.GetURL(Eval("attachmentIds").ToString())%>
                    <br />
                    <span style="float:right;"><a href="#top">返回顶部^</a></span>
                     </td></tr>
                     </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstposts.Items.Count == 0 ? "暂无回复" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine displayNo" />
            </asp:DataList>
            <h3 style="height: 30px; margin: 10px 0px 0px 0px">
                回复</h3>
            <table style="width: 100%; border-collapse: collapse; margin: 0px;">
                <tr>
                    <td style="width: 15%; vertical-align: text-top;">
                        内容:
                    </td>
                    <td style="width: 85%;"><a name="reply"></a>
                    <textarea id="Textarea1" cols="100" rows="8" runat="server" clientidmode="Static"></textarea>
                        <br />
                        <%--<asp:TextBox ID="txtContent" runat="server" Height="179px" TextMode="MultiLine" Width="479px"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        附件
                    </td>
                    <td>
                        <!--upload start-->
                        <input id="Button3" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />
                        <div id="iframes"></div>
                        <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
                        <!--upload end-->
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td><br /><asp:Button ID="btnCommit" runat="server" Text="提交" OnClick="btnCommit_Click" OnClientClick="return checkKindValue('Textarea1');" />
                       <%--<input id="btnAddPost" name="btnAddPost" type="button" value="提交" class="footoperation" onclick="addNewPost('<%=Request["tag"] %>','<%=Request["topicId"] %>')"  />--%>
                        </td>
                </tr>
            </table>
            
           
        </div>
</asp:Content>

