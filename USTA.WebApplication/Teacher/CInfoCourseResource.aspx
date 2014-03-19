<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_CInfoCourseResource" 
    MasterPageFile="~/MasterPage/CourseInfoForTeacher.master" Codebehind="CInfoCourseResource.aspx.cs" %>

<%@ MasterType  VirtualPath="~/MasterPage/CourseInfoForTeacher.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
         <a href="AddCourseResource.aspx?keepThis=true&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>&fragmentFlag=5&TB_iframe=true&height=300&width=500"
                title="添加课程资源" class="thickbox">添加课程资源</a>
                <%--OnDeleteCommand="dlstCourseResource_DeleteCommand"--%>
            <asp:DataList ID="dlstCourseResource" runat="server" 
                DataKeyField="courseResourceId" Width="60%">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <caption>
                        </caption>
                        <tr>
                            <th scope="col" width="20%">
                                资源名称
                            </th>
                            <th scope="col" width="50%">
                                资源名称
                            </th>
                            <th scope="col" width="30%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <td class="row" width="20%">
                                <%#Eval("courseResourceTitle")%> <%#(isNew(Eval("updateTime").ToString()))?"<img src='../images/new.gif' align='middle' style='margin-bottom:5px;'/>":"" %>
                            </td>
                            <td width="50%">
                                <%#GetURL(Eval("attachmentIds").ToString()) %>
                            </td>
                            <td class="row" width="30%">
                                <a href="EditCourseResource.aspx?keepThis=true&courseResourceId=<%#Eval("courseResourceId")%>&classID=<%=Server.UrlEncode(Master.classID).ToString().Trim() %>&termtag=<%=Master.termtag.ToString().Trim() %>&courseNo=<%=Master.courseNo.ToString().Trim()%>&fragment=5&TB_iframe=true&height=300&width=500"
                                    title="修改课程资源" class="thickbox">修改</a>
                             
                                <a href="?courseNo=<%=Master.courseNo%>&classID=<%=Server.UrlEncode(Master.classID).ToString().Trim() %>&termtag=<%=Master.termtag.ToString().Trim() %>&resourceId=<%#Eval("courseResourceId")%>&op=delete" onclick="return deleteTip()">删除</a>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstCourseResource.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
     </div>
</asp:Content>
