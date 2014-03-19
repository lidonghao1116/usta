<%@ Page Language="C#" AutoEventWireup="true" Inherits="Student_CousrInfo_3_CourseNotify"
    MasterPageFile="~/MasterPage/CourseInfoForStudent.master" Codebehind="CInfoCourseNotify.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/CourseInfoForStudent.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
    <div class="ui-tabs-panel">
         <asp:DataList ID="dlstCourseNotify" runat="server">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <th scope="col">
                                课程通知
                            </th>
                             
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0" >
                        <tr>
                            <td>
                            <img src="../images/BULLET.GIF" align="absmiddle" />    <a href="/Common/ViewCourseNotify.aspx?keepThis=true&courseNotifyId=<%#Eval("courseNotifyInfoId")%>&fragment=5&TB_iframe=true&height=300&width=800"
                                    title="查看课程通知" class="thickbox">
                                    <%#Eval("courseNotifyInfoTitle")%></a>
                               <%#(Convert.ToInt32((Eval("isTop").ToString().Trim().Length>0?Eval("isTop").ToString().Trim():"0".ToString()))>0?"<img src=\"../images/onTop.gif\" width=\"36\" height=\"13\" alt=\"置顶\" align='middle' />":null) %>
                               &nbsp;&nbsp;&nbsp;
                                 <%#Eval("updateTime")%> <%#(isNew(Eval("updateTime").ToString()))?"<img src='../images/new.gif' align='middle' style='margin-bottom:5px;'/>":"" %>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstCourseNotify.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
    </div>
</asp:Content>