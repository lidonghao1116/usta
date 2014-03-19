<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_TeachingPlan" 
    MasterPageFile="~/MasterPage/CourseInfoForTeacher.master" Codebehind="CInfoTeachingPlan.aspx.cs" %>

<%@ MasterType  VirtualPath="~/MasterPage/CourseInfoForTeacher.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
          <asp:DataList ID="dlstTeachingPlan" runat="server">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <caption>
                        </caption>
                        <tr>
                            <th scope="col" width="50%">
                                教学安排<a href="EditTeachingPlan.aspx?keepThis=true&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>&TB_iframe=true&height=380&width=800"
                                    title="修改教学安排" class="thickbox">编辑</a>
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <td>
                                <%#Eval("teachingPlan") %>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstTeachingPlan.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
     </div>
</asp:Content>
