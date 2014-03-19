<%@ Page Language="C#" AutoEventWireup="true" Inherits="Student_CousrInfo_6_ExperimentResource"
    MasterPageFile="~/MasterPage/CourseInfoForStudent.master" CodeBehind="CInfoExperimentResource.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/CourseInfoForStudent.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
    <div class="ui-tabs-panel">
        <asp:DataList ID="dlstExperimentResource" runat="server">
            <HeaderTemplate>
                <table class="mytable mytableWidth" cellspacing="0">
                    <tr>
                        <th scope="col" width="10%">
                            课程实验
                        </th>
                        <th scope="col">
                            <a href="/Student/ExperimentsAndSchoolWorksManage.aspx?fragment=2&courseNoTermTagClassID=<%=Master.courseNo.ToString().Trim()  + Master.termtag.ToString().Trim()+Server.UrlEncode(Master.classID).ToString().Trim() %>">
                                查看我的实验</a>
                        </th>
                    </tr>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table class="mytable mytableWidth" cellspacing="0">
                    <tr>
                        <td width="5%">
                            <%#Container.ItemIndex+1 %>
                        </td>
                        <td>
                            <a href="CInfoExperiment.aspx?courseNo=<%=Master.courseNo.ToString().Trim() %>&classID=<%=Server.UrlEncode(Master.classID).ToString().Trim() %>&termtag=<%=Master.termtag.ToString().Trim() %>&experimentResourceId=<%#Eval("experimentResourceId")%>">
                                <%#Eval("experimentResourceTitle")%></a>
                            <%#Eval("updateTime")%>
                            <%#(isNew(Eval("updateTime").ToString()))?"<img src='../images/new.gif' align='middle' style='margin-bottom:5px;'/>":"" %>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <FooterTemplate>
                <%=(this.dlstExperimentResource.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
            <FooterStyle CssClass="datalistNoLine" />
        </asp:DataList>
    </div>
</asp:Content>
