<%@ Page Language="C#" AutoEventWireup="true" Inherits="Student_CousrInfo_7_SchoolworkNotify"   
    MasterPageFile="~/MasterPage/CourseInfoForStudent.master" Codebehind="CInfoSchoolworkNotify.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/CourseInfoForStudent.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
    <div class="ui-tabs-panel">
           <asp:DataList ID="dlstSchoolworkNotify" runat="server">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <th scope="col" width="10%">
                                作业
                            </th>
                        <th scope="col">
                            <a href="/Student/ExperimentsAndSchoolWorksManage.aspx?fragment=1&courseNoTermTagClassID=<%=Master.courseNo.ToString().Trim() +Master.termtag.ToString().Trim()+Server.UrlEncode(Master.classID).ToString().Trim() %>">
                                查看我的作业</a>
                        </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                                    <td width="5%"><%#Container.ItemIndex+1 %>
                                    </td>
                            <td>
                                <a href="CInfoSchoolwork.aspx?schoolworkNotifyId=<%#Eval("schoolWorkNotifyId")%>&courseNo=<%=Master.courseNo%>&classId=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>">
                                    <%#Eval("schoolWorkNotifyTitle")%></a> <%#Eval("updateTime")%> <%#(isNew(Eval("updateTime").ToString()))?"<img src='../images/new.gif' align='middle' style='margin-bottom:5px;'/>":"" %>
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