<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_CInfoSchoolworkNotify" 
    MasterPageFile="~/MasterPage/CourseInfoForTeacher.master" Codebehind="CInfoSchoolworkNotify.aspx.cs" %>

<%@ MasterType  VirtualPath="~/MasterPage/CourseInfoForTeacher.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
           
            <a href="AddSchoolworkNotify.aspx?keepThis=true&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>&fragment=7&TB_iframe=true&height=500&width=900"
                title="布置课程作业" class="thickbox">布置课程作业</a>
               <%--  OnDeleteCommand="dlstSchoolworkNotify_DeleteCommand"--%>
            <asp:DataList ID="dlstSchoolworkNotify" runat="server"
                DataKeyField="schoolWorkNotifyId" Width="60%">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <caption>
                        </caption>
                        <tr>
                            <th scope="col" width="50%">
                                作业标题
                            </th>
                            <th scope="col" width="50%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <td class="row" width="50%">
                                <a href="VSWorkNWorkReq.aspx?schoolworkNotifyId=<%#Eval("schoolWorkNotifyId")%>&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>">
                                    <%#Eval("schoolWorkNotifyTitle")%></a> <%#(isNew(Eval("updateTime").ToString()))?"<img src='../images/new.gif' align='middle' style='margin-bottom:5px;'/>":"" %>
                            </td>
                            <td class="row" width="50%">
                                <a href="EditSchoolworkNotify.aspx?keepThis=true&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>&schoolworkNotifyId=<%#Eval("schoolWorkNotifyId")%>&fragment=7&TB_iframe=true&height=500&width=900"
                                    title="编辑课程作业" class="thickbox">[编辑]</a>
                                <a href="?courseNo=<%=Master.courseNo%>&classID=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>&schoolworkNotifyId=<%#Eval("schoolWorkNotifyId")%>&op=delete"
                                    onclick="return deleteTip();">[删除]</a>
                           <a href="VSWorkNWorkReq.aspx?schoolworkNotifyId=<%#Eval("schoolWorkNotifyId")%>&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>">[进入查看]</a>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstSchoolworkNotify.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
     </div>
</asp:Content>